namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Services.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class BindingEntityInfo
    {
        private static readonly Dictionary<Type, BindingEntityInfoPerType> bindingEntityInfos = new Dictionary<Type, BindingEntityInfoPerType>(EqualityComparer<Type>.Default);
        private static readonly object FalseObject = new object();
        private static readonly HashSet<Type> knownNonEntityTypes = new HashSet<Type>(EqualityComparer<Type>.Default);
        private static readonly Dictionary<Type, object> knownObservableCollectionTypes = new Dictionary<Type, object>(EqualityComparer<Type>.Default);
        private static readonly ReaderWriterLockSlim metadataCacheLock = new ReaderWriterLockSlim();
        private static readonly object TrueObject = new object();

        private static bool CanBeComplexProperty(ClientType.ClientProperty property)
        {
            Debug.Assert(property != null, "property != null");
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(property.PropertyType))
            {
                Debug.Assert(!property.IsKnownType, "Known types do not implement INotifyPropertyChanged.");
                return true;
            }
            return false;
        }

        private static BindingEntityInfoPerType GetBindingEntityInfoFor(Type entityType)
        {
            BindingEntityInfoPerType type;
            metadataCacheLock.EnterReadLock();
            try
            {
                if (bindingEntityInfos.TryGetValue(entityType, out type))
                {
                    return type;
                }
            }
            finally
            {
                metadataCacheLock.ExitReadLock();
            }
            type = new BindingEntityInfoPerType();
            object[] customAttributes = entityType.GetCustomAttributes(typeof(EntitySetAttribute), true);
            type.EntitySet = ((customAttributes != null) && (customAttributes.Length == 1)) ? ((EntitySetAttribute) customAttributes[0]).EntitySet : null;
            type.ClientType = ClientType.Create(entityType);
            foreach (ClientType.ClientProperty property in type.ClientType.Properties)
            {
                BindingPropertyInfo item = null;
                Type propertyType = property.PropertyType;
                if (property.CollectionType != null)
                {
                    if (IsDataServiceCollection(propertyType))
                    {
                        BindingPropertyInfo info2 = new BindingPropertyInfo();
                        info2.PropertyKind = BindingPropertyKind.BindingPropertyKindCollection;
                        item = info2;
                    }
                }
                else if (IsEntityType(propertyType))
                {
                    BindingPropertyInfo info3 = new BindingPropertyInfo();
                    info3.PropertyKind = BindingPropertyKind.BindingPropertyKindEntity;
                    item = info3;
                }
                else if (CanBeComplexProperty(property))
                {
                    BindingPropertyInfo info4 = new BindingPropertyInfo();
                    info4.PropertyKind = BindingPropertyKind.BindingPropertyKindComplex;
                    item = info4;
                }
                if (item != null)
                {
                    item.PropertyInfo = property;
                    if (type.ClientType.IsEntityType || (item.PropertyKind == BindingPropertyKind.BindingPropertyKindComplex))
                    {
                        type.ObservableProperties.Add(item);
                    }
                }
            }
            metadataCacheLock.EnterWriteLock();
            try
            {
                if (!bindingEntityInfos.ContainsKey(entityType))
                {
                    bindingEntityInfos[entityType] = type;
                }
            }
            finally
            {
                metadataCacheLock.ExitWriteLock();
            }
            return type;
        }

        internal static ClientType GetClientType(Type entityType)
        {
            return GetBindingEntityInfoFor(entityType).ClientType;
        }

        internal static string GetEntitySet(object target, string targetEntitySet)
        {
            Debug.Assert(target != null, "Argument 'target' cannot be null.");
            Debug.Assert(IsEntityType(target.GetType()), "Argument 'target' must be an entity type.");
            if (!string.IsNullOrEmpty(targetEntitySet))
            {
                return targetEntitySet;
            }
            return GetEntitySetAttribute(target.GetType());
        }

        private static string GetEntitySetAttribute(Type entityType)
        {
            return GetBindingEntityInfoFor(entityType).EntitySet;
        }

        internal static IList<BindingPropertyInfo> GetObservableProperties(Type entityType)
        {
            return GetBindingEntityInfoFor(entityType).ObservableProperties;
        }

        internal static object GetPropertyValue(object source, string sourceProperty, out BindingPropertyInfo bindingPropertyInfo)
        {
            Type sourceType = source.GetType();

            bindingPropertyInfo = BindingEntityInfo.GetObservableProperties(sourceType)
                                                   .SingleOrDefault(x => x.PropertyInfo.PropertyName == sourceProperty);

            if (bindingPropertyInfo == null)
            {
                return BindingEntityInfo.GetClientType(sourceType)
                                        .GetProperty(sourceProperty, false)
                                        .GetValue(source);
            }
            else
            {
                return bindingPropertyInfo.PropertyInfo.GetValue(source);
            }
        }

        internal static bool IsDataServiceCollection(Type collectionType)
        {
            Debug.Assert(collectionType != null, "Argument 'collectionType' cannot be null.");
            metadataCacheLock.EnterReadLock();
            try
            {
                object obj2;
                if (knownObservableCollectionTypes.TryGetValue(collectionType, out obj2))
                {
                    return (obj2 == TrueObject);
                }
            }
            finally
            {
                metadataCacheLock.ExitReadLock();
            }
            Type c = collectionType;
            bool flag = false;
            while (c != null)
            {
                if (c.IsGenericType)
                {
                    Type[] genericArguments = c.GetGenericArguments();
                    if (((genericArguments != null) && (genericArguments.Length == 1)) && IsEntityType(genericArguments[0]))
                    {
                        Type dataServiceCollectionOfT = WebUtil.GetDataServiceCollectionOfT(genericArguments);
                        if ((dataServiceCollectionOfT != null) && dataServiceCollectionOfT.IsAssignableFrom(c))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                c = c.BaseType;
            }
            metadataCacheLock.EnterWriteLock();
            try
            {
                if (!knownObservableCollectionTypes.ContainsKey(collectionType))
                {
                    knownObservableCollectionTypes[collectionType] = flag ? TrueObject : FalseObject;
                }
            }
            finally
            {
                metadataCacheLock.ExitWriteLock();
            }
            return flag;
        }

        internal static bool IsEntityType(Type type)
        {
            Debug.Assert(type != null, "Argument 'type' cannot be null.");
            metadataCacheLock.EnterReadLock();
            try
            {
                if (knownNonEntityTypes.Contains(type))
                {
                    return false;
                }
            }
            finally
            {
                metadataCacheLock.ExitReadLock();
            }
            try
            {
                if (IsDataServiceCollection(type))
                {
                    return false;
                }
                return ClientType.Create(type).IsEntityType;
            }
            catch (InvalidOperationException)
            {
                metadataCacheLock.EnterWriteLock();
                try
                {
                    if (!knownNonEntityTypes.Contains(type))
                    {
                        knownNonEntityTypes.Add(type);
                    }
                }
                finally
                {
                    metadataCacheLock.ExitWriteLock();
                }
                return false;
            }
        }

        private sealed class BindingEntityInfoPerType
        {
            public System.Data.Services.Client.ClientType ClientType { get; set; }
            public string EntitySet { get; set; }
            //[CompilerGenerated]
            //private System.Data.Services.Client.ClientType <ClientType>k__BackingField;
            //[CompilerGenerated]
            //private string <EntitySet>k__BackingField;
            private List<BindingEntityInfo.BindingPropertyInfo> observableProperties = new List<BindingEntityInfo.BindingPropertyInfo>();

            //public System.Data.Services.Client.ClientType ClientType
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<ClientType>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<ClientType>k__BackingField = value;
            //    }
            //}

            //public string EntitySet
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<EntitySet>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<EntitySet>k__BackingField = value;
            //    }
            //}

            public List<BindingEntityInfo.BindingPropertyInfo> ObservableProperties
            {
                get
                {
                    return this.observableProperties;
                }
            }
        }

        internal class BindingPropertyInfo
        {
            public ClientType.ClientProperty PropertyInfo { get; set; }
            public BindingPropertyKind PropertyKind { get; set; }
            //[CompilerGenerated]
            //private ClientType.ClientProperty <PropertyInfo>k__BackingField;
            //[CompilerGenerated]
            //private BindingPropertyKind <PropertyKind>k__BackingField;

            //public ClientType.ClientProperty PropertyInfo
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<PropertyInfo>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<PropertyInfo>k__BackingField = value;
            //    }
            //}

            //public BindingPropertyKind PropertyKind
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<PropertyKind>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<PropertyKind>k__BackingField = value;
            //    }
            //}
        }

        private sealed class ReaderWriterLockSlim
        {
            private object _lock = new object();

            internal void EnterReadLock()
            {
                Monitor.Enter(this._lock);
            }

            internal void EnterWriteLock()
            {
                Monitor.Enter(this._lock);
            }

            internal void ExitReadLock()
            {
                Monitor.Exit(this._lock);
            }

            internal void ExitWriteLock()
            {
                Monitor.Exit(this._lock);
            }
        }
    }
}

