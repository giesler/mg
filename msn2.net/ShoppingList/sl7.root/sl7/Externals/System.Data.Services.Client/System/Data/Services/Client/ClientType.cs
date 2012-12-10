namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Collections;

    [DebuggerDisplay("{ElementTypeName}")]
    internal sealed class ClientType
    {
        internal readonly Type ElementType;
        internal readonly string ElementTypeName;
        private System.Data.Services.Common.EpmSourceTree epmSourceTree;
        private System.Data.Services.Common.EpmTargetTree epmTargetTree;
        internal readonly bool IsEntityType;
        internal readonly int KeyCount;
        private ClientProperty mediaDataMember;
        private bool mediaLinkEntry;
        private static readonly Dictionary<TypeName, Type> namedTypes = new Dictionary<TypeName, Type>(new TypeNameEqualityComparer());
        private ArraySet<ClientProperty> properties;
        private static readonly Dictionary<Type, ClientType> types = new Dictionary<Type, ClientType>(EqualityComparer<Type>.Default);

        private ClientType(Type type, string typeName, bool skipSettableCheck)
        {
            Func<string, bool> predicate = null;
            Debug.Assert(null != type, "null type");
            Debug.Assert(!string.IsNullOrEmpty(typeName), "empty typeName");
            this.ElementTypeName = typeName;
            this.ElementType = Nullable.GetUnderlyingType(type) ?? type;
            if (!ClientConvert.IsKnownType(this.ElementType))
            {
                Type c = null;
                bool flag = type.GetCustomAttributes(true).OfType<DataServiceEntityAttribute>().Any<DataServiceEntityAttribute>();
                DataServiceKeyAttribute attribute = type.GetCustomAttributes(true).OfType<DataServiceKeyAttribute>().FirstOrDefault<DataServiceKeyAttribute>();
                foreach (PropertyInfo info in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    Type propertyType = info.PropertyType;
                    propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    if (((!propertyType.IsPointer && ((!propertyType.IsArray || (typeof(byte[]) == propertyType)) || (typeof(char[]) == propertyType))) && (typeof(IntPtr) != propertyType)) && (typeof(UIntPtr) != propertyType))
                    {
                        Debug.Assert(!propertyType.ContainsGenericParameters, "remove when test case is found that encounters this");
                        if (((info.CanRead && (!propertyType.IsValueType || info.CanWrite)) && !propertyType.ContainsGenericParameters) && (0 == info.GetIndexParameters().Length))
                        {
                            bool keyProperty = (attribute != null) ? attribute.KeyNames.Contains(info.Name) : false;
                            if (keyProperty)
                            {
                                if (null == c)
                                {
                                    c = info.DeclaringType;
                                }
                                else if (c != info.DeclaringType)
                                {
                                    throw Error.InvalidOperation(Strings.ClientType_KeysOnDifferentDeclaredType(this.ElementTypeName));
                                }
                                if (!ClientConvert.IsKnownType(propertyType))
                                {
                                    throw Error.InvalidOperation(Strings.ClientType_KeysMustBeSimpleTypes(this.ElementTypeName));
                                }
                                this.KeyCount++;
                            }
                            ClientProperty item = new ClientProperty(info, propertyType, keyProperty);
                            if (!this.properties.Add(item, new Func<ClientProperty, ClientProperty, bool>(ClientProperty.NameEquality)))
                            {
                                int index = this.IndexOfProperty(item.PropertyName);
                                if (!item.DeclaringType.IsAssignableFrom(this.properties[index].DeclaringType))
                                {
                                    this.properties.RemoveAt(index);
                                    this.properties.Add(item, null);
                                }
                            }
                        }
                    }
                }
                if (null == c)
                {
                    ClientProperty property2 = null;
                    for (int i = this.properties.Count - 1; 0 <= i; i--)
                    {
                        string propertyName = this.properties[i].PropertyName;
                        if (propertyName.EndsWith("ID", StringComparison.Ordinal))
                        {
                            string name = this.properties[i].DeclaringType.Name;
                            if ((propertyName.Length == (name.Length + 2)) && propertyName.StartsWith(name, StringComparison.Ordinal))
                            {
                                if ((c == null) || this.properties[i].DeclaringType.IsAssignableFrom(c))
                                {
                                    c = this.properties[i].DeclaringType;
                                    property2 = this.properties[i];
                                }
                            }
                            else if ((c == null) && (2 == propertyName.Length))
                            {
                                c = this.properties[i].DeclaringType;
                                property2 = this.properties[i];
                            }
                        }
                    }
                    if (null != property2)
                    {
                        Debug.Assert(0 == this.KeyCount, "shouldn't have a key yet");
                        property2.KeyProperty = true;
                        this.KeyCount++;
                    }
                }
                else if (this.KeyCount != attribute.KeyNames.Count)
                {
                    if (predicate == null)
                    {
                        predicate = delegate(string a)
                        {
                            return null == this.properties.Where<ClientProperty>(((Func<ClientProperty, bool>)(b => (b.PropertyName == a)))).FirstOrDefault<ClientProperty>();
                        };
                    }
                    string str3 = attribute.KeyNames.Cast<string>().Where<string>(predicate).First<string>();
                    throw Error.InvalidOperation(Strings.ClientType_MissingProperty(this.ElementTypeName, str3));
                }
                this.IsEntityType = (c != null) || flag;
                Debug.Assert(this.KeyCount == this.Properties.Where<ClientProperty>(delegate(ClientProperty k)
                {
                    return k.KeyProperty;
                }).Count<ClientProperty>(), "KeyCount mismatch");
                this.WireUpMimeTypeProperties();
                this.CheckMediaLinkEntry();
                if (!skipSettableCheck && !((this.properties.Count != 0) || typeof(ICollection).IsAssignableFrom(this.ElementType)))
                {
                    throw Error.InvalidOperation(Strings.ClientType_NoSettableFields(this.ElementTypeName));
                }
            }
            this.properties.TrimToSize();
            this.properties.Sort<string>(new Func<ClientProperty, string>(ClientProperty.GetPropertyName), new Func<string, string, int>(string.CompareOrdinal));
            this.BuildEpmInfo(type);

            //Func<string, bool> func = null;
            //Debug.Assert(null != type, "null type");
            //Debug.Assert(!string.IsNullOrEmpty(typeName), "empty typeName");
            //this.ElementTypeName = typeName;
            //this.ElementType = Nullable.GetUnderlyingType(type) ?? type;
            //if (!ClientConvert.IsKnownType(this.ElementType))
            //{
            //    Type c = null;
            //    bool flag = type.GetCustomAttributes(true).OfType<DataServiceEntityAttribute>().Any<DataServiceEntityAttribute>();
            //    DataServiceKeyAttribute attribute = type.GetCustomAttributes(true).OfType<DataServiceKeyAttribute>().FirstOrDefault<DataServiceKeyAttribute>();
            //    foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            //    {
            //        Type propertyType = info.PropertyType;
            //        propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            //        if (((!propertyType.IsPointer && ((!propertyType.IsArray || (typeof(byte[]) == propertyType)) || (typeof(char[]) == propertyType))) && (typeof(IntPtr) != propertyType)) && (typeof(UIntPtr) != propertyType))
            //        {
            //            Debug.Assert(!propertyType.ContainsGenericParameters, "remove when test case is found that encounters this");
            //            if (((info.CanRead && (!propertyType.IsValueType || info.CanWrite)) && !propertyType.ContainsGenericParameters) && (0 == info.GetIndexParameters().Length))
            //            {
            //                bool keyProperty = (attribute != null) ? attribute.KeyNames.Contains(info.Name) : false;
            //                if (keyProperty)
            //                {
            //                    if (null == c)
            //                    {
            //                        c = info.DeclaringType;
            //                    }
            //                    else if (!(c == info.DeclaringType))
            //                    {
            //                        throw Error.InvalidOperation(Strings.ClientType_KeysOnDifferentDeclaredType(this.ElementTypeName));
            //                    }
            //                    if (!ClientConvert.IsKnownType(propertyType))
            //                    {
            //                        throw Error.InvalidOperation(Strings.ClientType_KeysMustBeSimpleTypes(this.ElementTypeName));
            //                    }
            //                    this.KeyCount++;
            //                }
            //                ClientProperty item = new ClientProperty(info, propertyType, keyProperty);
            //                if (!this.properties.Add(item, ClientProperty.NameEquality))
            //                {
            //                    int index = this.IndexOfProperty(item.PropertyName);
            //                    if (!item.DeclaringType.IsAssignableFrom(this.properties[index].DeclaringType))
            //                    {
            //                        this.properties.RemoveAt(index);
            //                        this.properties.Add(item, null);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    if (null == c)
            //    {
            //        ClientProperty property2 = null;
            //        for (int i = this.properties.Count - 1; 0 <= i; i--)
            //        {
            //            string propertyName = this.properties[i].PropertyName;
            //            if (propertyName.EndsWith("ID", StringComparison.Ordinal))
            //            {
            //                string name = this.properties[i].DeclaringType.Name;
            //                if ((propertyName.Length == (name.Length + 2)) && propertyName.StartsWith(name, StringComparison.Ordinal))
            //                {
            //                    if ((c == null) || this.properties[i].DeclaringType.IsAssignableFrom(c))
            //                    {
            //                        c = this.properties[i].DeclaringType;
            //                        property2 = this.properties[i];
            //                    }
            //                }
            //                else if ((c == null) && (2 == propertyName.Length))
            //                {
            //                    c = this.properties[i].DeclaringType;
            //                    property2 = this.properties[i];
            //                }
            //            }
            //        }
            //        if (null != property2)
            //        {
            //            Debug.Assert(0 == this.KeyCount, "shouldn't have a key yet");
            //            property2.KeyProperty = true;
            //            this.KeyCount++;
            //        }
            //    }
            //    else if (this.KeyCount != attribute.KeyNames.Count)
            //    {
            //        var m = (from string a in attribute.KeyNames
            //                 where null == (from b in this.properties
            //                                where b.PropertyName == a
            //                                select b).FirstOrDefault()
            //                 select a).First<string>();
            //        throw Error.InvalidOperation(Strings.ClientType_MissingProperty(this.ElementTypeName, m));
            //    }
            //    this.IsEntityType = (null != c) || flag;
            //    Debug.Assert(this.KeyCount == this.Properties.Where(k => k.KeyProperty).Count(), "KeyCount mismatch");
            //    //this.IsEntityType = (c != null) || flag;
            //    //if (CS$<>9__CachedAnonymousMethodDelegate4 == null)
            //    //{
            //    //    CS$<>9__CachedAnonymousMethodDelegate4 = new Func<ClientProperty, bool>(null, (IntPtr) <.ctor>b__2);
            //    //}
            //    //Debug.Assert(this.KeyCount == Enumerable.Where<ClientProperty>(this.Properties, CS$<>9__CachedAnonymousMethodDelegate4).Count<ClientProperty>(), "KeyCount mismatch");
            //    this.WireUpMimeTypeProperties();
            //    this.CheckMediaLinkEntry();
            //    if (!skipSettableCheck && (0 == this.properties.Count))
            //    {
            //        throw Error.InvalidOperation(Strings.ClientType_NoSettableFields(this.ElementTypeName));
            //    }
            //}
            //this.properties.TrimToSize();
            //this.properties.Sort<string>(ClientProperty.GetPropertyName, String.CompareOrdinal);
            ////this.properties.Sort<string>(new Func<ClientProperty, string>(null, (IntPtr) ClientProperty.GetPropertyName), new Func<string, string, int>(null, (IntPtr) string.CompareOrdinal));
            //this.BuildEpmInfo(type);
        }

        private void BuildEpmInfo(Type type)
        {
            if ((type.BaseType != null) && (type.BaseType != typeof(object)))
            {
                this.BuildEpmInfo(type.BaseType);
            }
            foreach (EntityPropertyMappingAttribute attribute in type.GetCustomAttributes(typeof(EntityPropertyMappingAttribute), false))
            {
                this.BuildEpmInfo(attribute, type);
            }
        }

        private void BuildEpmInfo(EntityPropertyMappingAttribute epmAttr, Type definingType)
        {
            EntityPropertyMappingInfo epmInfo = new EntityPropertyMappingInfo();
            epmInfo.Attribute = epmAttr;
            epmInfo.DefiningType = definingType;
            epmInfo.ActualType = this;
            this.EpmSourceTree.Add(epmInfo);
        }

        internal static bool CanAssignNull(Type type)
        {
            Debug.Assert(type != null, "type != null");
            return (!type.IsValueType || (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>))));
        }

        internal static bool CheckElementTypeIsEntity(Type t)
        {
            t = TypeSystem.GetElementType(t);
            t = Nullable.GetUnderlyingType(t) ?? t;
            return Create(t, false).IsEntityType;
        }

        private void CheckMediaLinkEntry()
        {
            object[] customAttributes = this.ElementType.GetCustomAttributes(typeof(MediaEntryAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                Debug.Assert(customAttributes.Length == 1, "The AttributeUsage in the attribute definition should be preventing more than 1 per property");
                MediaEntryAttribute attribute = (MediaEntryAttribute) customAttributes[0];
                this.mediaLinkEntry = true;
                int num = this.IndexOfProperty(attribute.MediaMemberName);
                if (num < 0)
                {
                    throw Error.InvalidOperation(Strings.ClientType_MissingMediaEntryProperty(attribute.MediaMemberName));
                }
                this.mediaDataMember = this.properties[num];
            }
            customAttributes = this.ElementType.GetCustomAttributes(typeof(HasStreamAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                Debug.Assert(customAttributes.Length == 1, "The AttributeUsage in the attribute definition should be preventing more than 1 per property");
                this.mediaLinkEntry = true;
            }
        }

        internal static ClientType Create(Type type)
        {
            return Create(type, true);
        }

        internal static ClientType Create(Type type, bool expectModelType)
        {
            ClientType type2;
            Dictionary<Type, ClientType> dictionary;
            lock ((dictionary = types))
            {
                types.TryGetValue(type, out type2);
            }
            if (null == type2)
            {
                bool skipSettableCheck = !expectModelType;
                type2 = new ClientType(type, type.ToString(), skipSettableCheck);
                if (!expectModelType)
                {
                    return type2;
                }
                lock ((dictionary = types))
                {
                    ClientType type3;
                    if (types.TryGetValue(type, out type3))
                    {
                        return type3;
                    }
                    types.Add(type, type2);
                }
            }
            return type2;

            //ClientType type2;
            //Dictionary<Type, ClientType> dictionary;
            //lock ((dictionary = types))
            //{
            //    types.TryGetValue(type, out type2);
            //}
            //if (null == type2)
            //{
            //    if (CommonUtil.IsUnsupportedType(type))
            //    {
            //        throw new InvalidOperationException(Strings.ClientType_UnsupportedType(type));
            //    }
            //    bool skipSettableCheck = !expectModelType;
            //    type2 = new ClientType(type, type.ToString(), skipSettableCheck);
            //    if (!expectModelType)
            //    {
            //        return type2;
            //    }
            //    lock ((dictionary = types))
            //    {
            //        ClientType type3;
            //        if (types.TryGetValue(type, out type3))
            //        {
            //            return type3;
            //        }
            //        types.Add(type, type2);
            //    }
            //}
            //return type2;
        }

        internal object CreateInstance()
        {
            return Activator.CreateInstance(this.ElementType);
        }

        internal static MethodInfo GetAddToCollectionMethod(Type collectionType, out Type type)
        {
            return GetCollectionMethod(collectionType, typeof(ICollection<>), "Add", out type);
        }

        internal static MethodInfo GetCollectionMethod(Type propertyType, Type genericTypeDefinition, string methodName, out Type type)
        {
            Debug.Assert(null != propertyType, "null propertyType");
            Debug.Assert(null != genericTypeDefinition, "null genericTypeDefinition");
            Debug.Assert(genericTypeDefinition.IsGenericTypeDefinition, "!IsGenericTypeDefinition");
            type = null;
            Type implementationType = GetImplementationType(propertyType, genericTypeDefinition);
            if (null != implementationType)
            {
                Type[] genericArguments = implementationType.GetGenericArguments();
                MethodInfo method = implementationType.GetMethod(methodName);
                Debug.Assert(null != method, "should have found the method");
                Debug.Assert(null != genericArguments, "null genericArguments");
                ParameterInfo[] parameters = method.GetParameters();
                if (0 < parameters.Length)
                {
                    Debug.Assert(genericArguments.Length == parameters.Length, "genericArguments don't match parameters");
                    for (int i = 0; i < genericArguments.Length; i++)
                    {
                        Debug.Assert(genericArguments[i] == parameters[i].ParameterType, "parameter doesn't match generic argument");
                    }
                }
                type = genericArguments[genericArguments.Length - 1];
                return method;
            }
            return null;
        }

        internal static Type GetImplementationType(Type propertyType, Type genericTypeDefinition)
        {
            if (IsConstructedGeneric(propertyType, genericTypeDefinition))
            {
                return propertyType;
            }
            Type type = null;
            foreach (Type type2 in propertyType.GetInterfaces())
            {
                if (IsConstructedGeneric(type2, genericTypeDefinition))
                {
                    if (null != type)
                    {
                        throw Error.NotSupported(Strings.ClientType_MultipleImplementationNotSupported);
                    }
                    type = type2;
                }
            }
            return type;
        }

        internal ClientProperty GetProperty(string propertyName, bool ignoreMissingProperties)
        {
            int num = this.IndexOfProperty(propertyName);
            if (0 <= num)
            {
                return this.properties[num];
            }
            if (!ignoreMissingProperties)
            {
                throw Error.InvalidOperation(Strings.ClientType_MissingProperty(this.ElementTypeName, propertyName));
            }
            return null;
        }

        internal static MethodInfo GetRemoveFromCollectionMethod(Type collectionType, out Type type)
        {
            return GetCollectionMethod(collectionType, typeof(ICollection<>), "Remove", out type);
        }

        private int IndexOfProperty(string propertyName)
        {
            return this.properties.IndexOf(propertyName, ClientProperty.GetPropertyName, String.Equals);
        }

        private static bool IsConstructedGeneric(Type type, Type genericTypeDefinition)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(!type.ContainsGenericParameters, "remove when test case is found that encounters this");
            Debug.Assert(genericTypeDefinition != null, "genericTypeDefinition != null");
            return ((type.IsGenericType && (type.GetGenericTypeDefinition() == genericTypeDefinition)) && !type.ContainsGenericParameters);
        }

        internal static object ReadPropertyValue(object element, ClientType resourceType, string srcPath)
        {
            Debug.Assert(srcPath != null, "srcPath must not be null");
            ClientProperty resourceProperty = null;
            return ReadPropertyValue(element, resourceType, srcPath.Split(new char[] { '/' }), 0, ref resourceProperty);
        }

        private static object ReadPropertyValue(object element, ClientType resourceType, string[] srcPathSegments, int currentSegment, ref ClientProperty resourceProperty)
        {
            if ((element == null) || (currentSegment == srcPathSegments.Length))
            {
                return element;
            }
            string propertyName = srcPathSegments[currentSegment];
            resourceProperty = resourceType.GetProperty(propertyName, true);
            if (resourceProperty == null)
            {
                throw Error.InvalidOperation(Strings.EpmSourceTree_InaccessiblePropertyOnType(propertyName, resourceType.ElementTypeName));
            }
            if (resourceProperty.IsKnownType ^ (currentSegment == (srcPathSegments.Length - 1)))
            {
                throw Error.InvalidOperation(!resourceProperty.IsKnownType ? Strings.EpmClientType_PropertyIsComplex(resourceProperty.PropertyName) : Strings.EpmClientType_PropertyIsPrimitive(resourceProperty.PropertyName));
            }
            PropertyInfo property = element.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            Debug.Assert(property != null, "Cannot find property " + propertyName + "on type " + element.GetType().Name);
            return ReadPropertyValue(property.GetValue(element, null), resourceProperty.IsKnownType ? null : Create(resourceProperty.PropertyType), srcPathSegments, ++currentSegment, ref resourceProperty);
        }

        internal static Type ResolveFromName(string wireName, Type userType, Type contextType)
        {
            Type type;
            TypeName name;
            bool flag;
            Dictionary<TypeName, Type> dictionary;
            name.Type = userType;
            name.Name = wireName;
            lock ((dictionary = namedTypes))
            {
                flag = namedTypes.TryGetValue(name, out type);
            }
            if (!flag)
            {
                string wireClassName = wireName;
                int num = wireName.LastIndexOf('.');
                if ((0 <= num) && (num < (wireName.Length - 1)))
                {
                    wireClassName = wireName.Substring(num + 1);
                }
                if (userType.Name == wireClassName)
                {
                    type = userType;
                }
                else
                {
                    foreach (Assembly assembly in new Assembly[] { userType.Assembly, contextType.Assembly }.Distinct<Assembly>())
                    {
                        Type type2 = assembly.GetType(wireName, false);
                        ResolveSubclass(wireClassName, userType, type2, ref type);
                        if (null == type2)
                        {
                            Type[] types = null;
                            try
                            {
                                types = assembly.GetTypes();
                            }
                            catch (ReflectionTypeLoadException)
                            {
                            }
                            if (null != types)
                            {
                                foreach (Type type3 in types)
                                {
                                    ResolveSubclass(wireClassName, userType, type3, ref type);
                                }
                            }
                        }
                    }
                }
                lock ((dictionary = namedTypes))
                {
                    namedTypes[name] = type;
                }
            }
            return type;
        }

        private static void ResolveSubclass(string wireClassName, Type userType, Type type, ref Type existing)
        {
            if ((((type != null) && type.IsVisible) && (wireClassName == type.Name)) && userType.IsAssignableFrom(type))
            {
                if (null != existing)
                {
                    throw Error.InvalidOperation(Strings.ClientType_Ambiguous(wireClassName, userType));
                }
                existing = type;
            }
        }

        private void WireUpMimeTypeProperties()
        {
            MimeTypePropertyAttribute attribute = (MimeTypePropertyAttribute) this.ElementType.GetCustomAttributes(typeof(MimeTypePropertyAttribute), true).SingleOrDefault<object>();
            if (null != attribute)
            {
                int num;
                int num2;
                if ((0 > (num = this.IndexOfProperty(attribute.DataPropertyName))) || (0 > (num2 = this.IndexOfProperty(attribute.MimeTypePropertyName))))
                {
                    throw Error.InvalidOperation(Strings.ClientType_MissingMimeTypeProperty(attribute.DataPropertyName, attribute.MimeTypePropertyName));
                }
                Debug.Assert(0 <= num, "missing data property");
                Debug.Assert(0 <= num2, "missing mime type property");
                this.Properties[num].MimeTypeProperty = this.Properties[num2];
            }
        }

        internal bool EpmIsV1Compatible
        {
            get
            {
                return (!this.HasEntityPropertyMappings || this.EpmTargetTree.IsV1Compatible);
            }
        }

        internal System.Data.Services.Common.EpmSourceTree EpmSourceTree
        {
            get
            {
                if (this.epmSourceTree == null)
                {
                    this.epmTargetTree = new System.Data.Services.Common.EpmTargetTree();
                    this.epmSourceTree = new System.Data.Services.Common.EpmSourceTree(this.epmTargetTree);
                }
                return this.epmSourceTree;
            }
        }

        internal System.Data.Services.Common.EpmTargetTree EpmTargetTree
        {
            get
            {
                Debug.Assert(this.epmTargetTree != null, "Must have valid target tree");
                return this.epmTargetTree;
            }
        }

        internal bool HasEntityPropertyMappings
        {
            get
            {
                return (this.epmSourceTree != null);
            }
        }

        internal bool IsMediaLinkEntry
        {
            get
            {
                return this.mediaLinkEntry;
            }
        }

        internal ClientProperty MediaDataMember
        {
            get
            {
                return this.mediaDataMember;
            }
        }

        internal ArraySet<ClientProperty> Properties
        {
            get
            {
                return this.properties;
            }
        }

        [DebuggerDisplay("{PropertyName}")]
        internal sealed class ClientProperty
        {
            private readonly MethodInfo addMethod;
            private readonly MethodInfo clearMethod;
            internal readonly Type CollectionType;
            private readonly MethodInfo containsMethod;
            internal readonly bool IsKnownType;
            private bool keyProperty;
            private ClientType.ClientProperty mimeTypeProperty;
            internal readonly Type NullablePropertyType;
            private readonly MethodInfo propertyGetter;
            internal readonly string PropertyName;
            private readonly MethodInfo propertySetter;
            internal readonly Type PropertyType;
            private readonly MethodInfo removeMethod;
            private readonly MethodInfo setMethod;

            internal ClientProperty(PropertyInfo property, Type propertyType, bool keyProperty)
            {
                //Debug.Assert(null != property, "null property");
                //Debug.Assert(null != propertyType, "null propertyType");
                //Debug.Assert(null == Nullable.GetUnderlyingType(propertyType), "should already have been denullified");
                //this.PropertyName = property.Name;
                //this.NullablePropertyType = property.PropertyType;
                //this.PropertyType = propertyType;
                //this.propertyGetter = property.GetGetMethod();
                //this.propertySetter = property.GetSetMethod();
                //this.keyProperty = keyProperty;
                //this.IsKnownType = ClientConvert.IsKnownType(propertyType);
                //if (!this.IsKnownType)
                //{
                //    this.setMethod = ClientType.GetCollectionMethod(this.PropertyType, typeof(IDictionary<,>), "set_Item", out this.CollectionType);
                //    if (null == this.setMethod)
                //    {
                //        this.containsMethod = ClientType.GetCollectionMethod(this.PropertyType, typeof(ICollection<>), "Contains", out this.CollectionType);
                //        this.addMethod = ClientType.GetAddToCollectionMethod(this.PropertyType, out this.CollectionType);
                //        this.removeMethod = ClientType.GetRemoveFromCollectionMethod(this.PropertyType, out this.CollectionType);
                //    }
                //}
                //Debug.Assert(!this.keyProperty || this.IsKnownType, "can't have an random type as key");
                Debug.Assert(null != property, "null property");
                Debug.Assert(null != propertyType, "null propertyType");
                Debug.Assert(null == Nullable.GetUnderlyingType(propertyType), "should already have been denullified");
                this.PropertyName = property.Name;
                this.NullablePropertyType = property.PropertyType;
                this.PropertyType = propertyType;
                this.propertyGetter = property.GetGetMethod(true);
                this.propertySetter = property.GetSetMethod(true);
                this.keyProperty = keyProperty;
                this.IsKnownType = ClientConvert.IsKnownType(propertyType);
                if (!this.IsKnownType)
                {
                    this.setMethod = ClientType.GetCollectionMethod(this.PropertyType, typeof(IDictionary<,>), "set_Item", out this.CollectionType);
                    if (null == this.setMethod)
                    {
                        this.containsMethod = ClientType.GetCollectionMethod(this.PropertyType, typeof(ICollection<>), "Contains", out this.CollectionType);
                        this.addMethod = ClientType.GetAddToCollectionMethod(this.PropertyType, out this.CollectionType);
                        this.removeMethod = ClientType.GetRemoveFromCollectionMethod(this.PropertyType, out this.CollectionType);
                        this.clearMethod = ClientType.GetCollectionMethod(this.PropertyType, typeof(ICollection<>), "Clear", out this.CollectionType);
                    }
                }
                Debug.Assert(!this.keyProperty || this.IsKnownType, "can't have an random type as key");

            }

            internal static bool GetKeyProperty(ClientType.ClientProperty x)
            {
                return x.KeyProperty;
            }

            internal static string GetPropertyName(ClientType.ClientProperty x)
            {
                return x.PropertyName;
            }

            internal object GetValue(object instance)
            {
                Debug.Assert(null != instance, "null instance");
                Debug.Assert(null != this.propertyGetter, "null propertyGetter");
                return this.propertyGetter.Invoke(instance, null);
            }

            internal static bool NameEquality(ClientType.ClientProperty x, ClientType.ClientProperty y)
            {
                return string.Equals(x.PropertyName, y.PropertyName);
            }

            internal void Clear(object instance)
            {
                Debug.Assert(null != this.clearMethod);
                this.clearMethod.Invoke(instance, new object[0]);
            }
            
            internal void RemoveValue(object instance, object value)
            {
                Debug.Assert(null != instance, "null instance");
                Debug.Assert(null != this.removeMethod, "missing removeMethod");
                Debug.Assert(this.PropertyType.IsAssignableFrom(instance.GetType()), "unexpected collection instance");
                Debug.Assert((value == null) || this.CollectionType.IsAssignableFrom(value.GetType()), "unexpected collection value to add");
                this.removeMethod.Invoke(instance, new object[] { value });
            }

            internal void SetValue(object instance, object value, string propertyName, bool allowAdd)
            {
                Debug.Assert(null != instance, "null instance");
                if (null != this.setMethod)
                {
                    Debug.Assert(this.PropertyType.IsAssignableFrom(instance.GetType()), "unexpected dictionary instance");
                    Debug.Assert((value == null) || this.CollectionType.IsAssignableFrom(value.GetType()), "unexpected dictionary value to set");
                    this.setMethod.Invoke(instance, new object[] { propertyName, value });
                }
                else if (allowAdd && (null != this.addMethod))
                {
                    if (!((bool)this.containsMethod.Invoke(instance, new object[] { value })))
                    {
                        this.addMethod.Invoke(instance, new object[] { value });
                    }
                }
                else
                {
                    if (null == this.propertySetter)
                    {
                        throw Error.InvalidOperation(Strings.ClientType_MissingProperty(value.GetType().ToString(), propertyName));
                    }
                    Debug.Assert((value == null) || this.PropertyType.IsAssignableFrom(value.GetType()), "unexpected property value to set");
                    this.propertySetter.Invoke(instance, new object[] { value });
                }

                //Debug.Assert(null != instance, "null instance");
                //if (null != this.setMethod)
                //{
                //    Debug.Assert(this.PropertyType.IsAssignableFrom(instance.GetType()), "unexpected dictionary instance");
                //    Debug.Assert((value == null) || this.CollectionType.IsAssignableFrom(value.GetType()), "unexpected dictionary value to set");
                //    this.setMethod.Invoke(instance, new object[] { propertyName, value });
                //}
                //else if (allowAdd && (null != this.addMethod))
                //{
                //    Debug.Assert(this.PropertyType.IsAssignableFrom(instance.GetType()), "unexpected collection instance");
                //    Debug.Assert((value == null) || this.CollectionType.IsAssignableFrom(value.GetType()), "unexpected collection value to add");
                //    if (!((bool) this.containsMethod.Invoke(instance, new object[] { value })))
                //    {
                //        this.addMethod.Invoke(instance, new object[] { value });
                //    }
                //}
                //else
                //{
                //    if (null == this.propertySetter)
                //    {
                //        throw Error.InvalidOperation(Strings.ClientType_MissingProperty(value.GetType().ToString(), propertyName));
                //    }
                //    Debug.Assert((value == null) || this.PropertyType.IsAssignableFrom(value.GetType()), "unexpected property value to set");
                //    this.propertySetter.Invoke(instance, new object[] { value });
                //}
            }

            internal Type DeclaringType
            {
                get
                {
                    return this.propertyGetter.DeclaringType;
                }
            }

            internal bool KeyProperty
            {
                get
                {
                    return this.keyProperty;
                }
                set
                {
                    this.keyProperty = value;
                }
            }

            internal ClientType.ClientProperty MimeTypeProperty
            {
                get
                {
                    return this.mimeTypeProperty;
                }
                set
                {
                    this.mimeTypeProperty = value;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TypeName
        {
            internal System.Type Type;
            internal string Name;
        }

        private sealed class TypeNameEqualityComparer : IEqualityComparer<ClientType.TypeName>
        {
            public bool Equals(ClientType.TypeName x, ClientType.TypeName y)
            {
                return ((x.Type == y.Type) && (x.Name == y.Name));
            }

            public int GetHashCode(ClientType.TypeName obj)
            {
                return (obj.Type.GetHashCode() ^ obj.Name.GetHashCode());
            }
        }
    }
}

