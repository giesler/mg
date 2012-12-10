namespace System.Data.Services.Client
{
    using IQToolkit;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml.Linq;
    using System.Xml;

    [DebuggerDisplay("AtomMaterializer {parser}")]
    internal class AtomMaterializer
    {
        private readonly DataServiceContext context;
        private object currentValue;
        private readonly Type expectedType;
        private bool ignoreMissingProperties;
        private readonly AtomMaterializerLog log;
        private readonly Action<object, object> materializedObjectCallback;
        private readonly ProjectionPlan materializeEntryPlan;
        private readonly MergeOption mergeOption;
        private readonly Dictionary<IEnumerable, DataServiceQueryContinuation> nextLinkTable;
        private readonly AtomParser parser;
        private object targetInstance;

        internal AtomMaterializer(AtomParser parser, DataServiceContext context, Type expectedType, bool ignoreMissingProperties, MergeOption mergeOption, AtomMaterializerLog log, Action<object, object> materializedObjectCallback, QueryComponents queryComponents, ProjectionPlan plan)
        {
            Debug.Assert(context != null, "context != null");
            Debug.Assert(parser != null, "parser != null");
            Debug.Assert(log != null, "log != null");
            this.context = context;
            this.parser = parser;
            this.expectedType = expectedType;
            this.ignoreMissingProperties = ignoreMissingProperties;
            this.mergeOption = mergeOption;
            this.log = log;
            this.materializedObjectCallback = materializedObjectCallback;
            this.nextLinkTable = new Dictionary<IEnumerable, DataServiceQueryContinuation>(ReferenceEqualityComparer<IEnumerable>.Instance);
            this.materializeEntryPlan = plan ?? CreatePlan(queryComponents);
        }

        private static void ApplyDataValue(ClientType type, AtomContentProperty property, bool ignoreMissingProperties, DataServiceContext context, object instance)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(property != null, "property != null");
            Debug.Assert(context != null, "context != null");
            Debug.Assert(instance != null, "instance != context");
            ClientType.ClientProperty property2 = type.GetProperty(property.Name, ignoreMissingProperties);
            if (property2 != null)
            {
                object obj2;
                if (property.Properties != null)
                {
                    if (property2.IsKnownType || ClientConvert.IsKnownType(MaterializeAtom.GetEntryClientType(property.TypeName, context, property2.PropertyType, true).ElementType))
                    {
                        throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
                    }
                    bool flag = false;
                    ClientType actualType = ClientType.Create(property2.PropertyType);
                    obj2 = property2.GetValue(instance);
                    if (obj2 == null)
                    {
                        obj2 = actualType.CreateInstance();
                        flag = true;
                    }
                    if (null != property2.CollectionType)
                    {
                        property2.Clear(obj2);
                        ClientType type3 = ClientType.Create(property2.CollectionType);
                        foreach (AtomContentProperty property3 in property.Properties)
                        {
                            if (null != property3.Properties)
                            {
                                object obj3 = type3.CreateInstance();
                                MaterializeDataValues(type3, property3.Properties, ignoreMissingProperties, context);
                                ApplyDataValues(type3, property3.Properties, ignoreMissingProperties, context, obj3);
                                property2.SetValue(obj2, obj3, property.Name, true);
                            }
                            else
                            {
                                MaterializeDataValue(property2.CollectionType, property3, context);
                                property2.SetValue(obj2, property3.MaterializedValue, property.Name, true);
                            }
                        }
                    }
                    else
                    {
                        MaterializeDataValues(actualType, property.Properties, ignoreMissingProperties, context);
                        ApplyDataValues(actualType, property.Properties, ignoreMissingProperties, context, obj2);
                    }
                    if (flag)
                    {
                        property2.SetValue(instance, obj2, property.Name, true);
                    }
                }
                else if (null != property2.CollectionType)
                {
                    obj2 = property2.GetValue(instance);
                    if (null != obj2)
                    {
                        property2.Clear(obj2);
                    }
                }
                else
                {
                    property2.SetValue(instance, property.MaterializedValue, property.Name, true);
                }
            }

            //Debug.Assert(type != null, "type != null");
            //Debug.Assert(property != null, "property != null");
            //Debug.Assert(context != null, "context != null");
            //Debug.Assert(instance != null, "instance != context");
            //ClientType.ClientProperty property2 = type.GetProperty(property.Name, ignoreMissingProperties);
            //if (property2 != null)
            //{
            //    if (property.Properties != null)
            //    {
            //        if (property2.IsKnownType || ClientConvert.IsKnownType(MaterializeAtom.GetEntryClientType(property.TypeName, context, property2.PropertyType, true).ElementType))
            //        {
            //            throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
            //        }
            //        bool flag = false;
            //        ClientType actualType = ClientType.Create(property2.PropertyType);
            //        object obj2 = property2.GetValue(instance);
            //        if (obj2 == null)
            //        {
            //            obj2 = actualType.CreateInstance();
            //            flag = true;
            //        }
            //        MaterializeDataValues(actualType, property.Properties, ignoreMissingProperties, context);
            //        ApplyDataValues(actualType, property.Properties, ignoreMissingProperties, context, obj2);
            //        if (flag)
            //        {
            //            property2.SetValue(instance, obj2, property.Name, true);
            //        }
            //    }
            //    else
            //    {
            //        property2.SetValue(instance, property.MaterializedValue, property.Name, true);
            //    }
            //}
        }

        private static void ApplyDataValues(ClientType type, IEnumerable<AtomContentProperty> properties, bool ignoreMissingProperties, DataServiceContext context, object instance)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(properties != null, "properties != null");
            Debug.Assert(context != null, "properties != context");
            Debug.Assert(instance != null, "instance != context");
            foreach (AtomContentProperty property in properties)
            {
                ApplyDataValue(type, property, ignoreMissingProperties, context, instance);
            }

        }

        private static void ApplyEntityPropertyMappings(AtomEntry entry, ClientType entryType)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(entry.Tag is XElement, "entry.Tag is XElement");
            Debug.Assert(entryType != null, "entryType != null -- othewise how would we know to apply property mappings (note that for projections entry.ActualType may be different that entryType)?");
            Debug.Assert(!entry.EntityPropertyMappingsApplied, "!entry.EntityPropertyMappingsApplied -- EPM should happen only once per entry");
            if (entryType.HasEntityPropertyMappings)
            {
                XElement tag = entry.Tag as XElement;
                Debug.Assert(tag != null, "entryElement != null");
                ApplyEntityPropertyMappings(entry, tag, entryType.EpmTargetTree.SyndicationRoot);
                ApplyEntityPropertyMappings(entry, tag, entryType.EpmTargetTree.NonSyndicationRoot);
            }
            entry.EntityPropertyMappingsApplied = true;
        }

        private static void ApplyEntityPropertyMappings(AtomEntry entry, XElement entryElement, EpmTargetPathSegment target)
        {
           
            Debug.Assert(target != null, "target != null");
            Debug.Assert(!target.HasContent, "!target.HasContent");

            Stack<System.Data.Services.Common.EpmTargetPathSegment> segments = new Stack<System.Data.Services.Common.EpmTargetPathSegment>();
            Stack<XElement> elements = new Stack<XElement>();

            segments.Push(target);
            elements.Push(entryElement);

            while (segments.Count > 0)
            {
                System.Data.Services.Common.EpmTargetPathSegment segment = segments.Pop();
                XElement element = elements.Pop();
                if (segment.HasContent)
                {
                    var node = element.Nodes().Where(n => n.NodeType == XmlNodeType.Text || n.NodeType == XmlNodeType.SignificantWhitespace).FirstOrDefault();
                    string elementValue = (node == null) ? null : ((XText)node).Value;
                    Debug.Assert(segment.EpmInfo != null, "segment.EpmInfo != null -- otherwise segment.HasValue should be false");

                    string path = segment.EpmInfo.Attribute.SourcePath;
                    string typeName = (string)element.Attribute(XName.Get(XmlConstants.AtomTypeAttributeName, XmlConstants.DataWebMetadataNamespace));

                    SetValueOnPath(entry.DataValues, path, elementValue, typeName);
                }

                foreach (var item in segment.SubSegments)
                {
                    if (item.IsAttribute)
                    {
                        string localName = item.SegmentName.Substring(1);
                        var attribute = element.Attribute(XName.Get(localName, item.SegmentNamespaceUri));
                        if (attribute != null)
                        {
                            SetValueOnPath(entry.DataValues, item.EpmInfo.Attribute.SourcePath, attribute.Value, null);
                        }
                    }
                    else
                    {
                        var child = element.Element(XName.Get(item.SegmentName, item.SegmentNamespaceUri));
                        if (child != null)
                        {
                            segments.Push(item);
                            elements.Push(child);
                        }
                    }
                }

                Debug.Assert(segments.Count == elements.Count, "segments.Count == elements.Count -- otherwise they're out of sync");
            }
        }

        private void ApplyFeedToCollection(AtomEntry entry, ClientType.ClientProperty property, AtomFeed feed, bool includeLinks)
        {
             Debug.Assert(entry != null, "entry != null");
            Debug.Assert(property != null, "property != null");
            Debug.Assert(feed != null, "feed != null");

            ClientType collectionType = ClientType.Create(property.CollectionType);
            foreach (AtomEntry feedEntry in feed.Entries)
            {
                this.Materialize(feedEntry, collectionType.ElementType, includeLinks);
            }

            ProjectionPlan continuationPlan = includeLinks ? CreatePlanForDirectMaterialization(property.CollectionType) : CreatePlanForShallowMaterialization(property.CollectionType);
            this.ApplyItemsToCollection(entry, property, feed.Entries.Select(e => e.ResolvedObject), feed.NextLink, continuationPlan);
        }

        private void ApplyItemsToCollection(AtomEntry entry, ClientType.ClientProperty property, IEnumerable items, Uri nextLink, ProjectionPlan continuationPlan)
        {
             Debug.Assert(entry != null, "entry != null");
            Debug.Assert(property != null, "property != null");
            Debug.Assert(items != null, "items != null");

            object collection = entry.ShouldUpdateFromPayload ? GetOrCreateCollectionProperty(entry.ResolvedObject, property, null) : null;
            ClientType collectionType = ClientType.Create(property.CollectionType);
            foreach (object item in items)
            {
                if (!collectionType.ElementType.IsAssignableFrom(item.GetType()))
                {
                    string message = Strings.AtomMaterializer_EntryIntoCollectionMismatch(
                        item.GetType().FullName,
                        collectionType.ElementType.FullName);
                    throw new InvalidOperationException(message);
                }

                if (entry.ShouldUpdateFromPayload)
                {
                    property.SetValue(collection, item, property.PropertyName, true );
                    this.log.AddedLink(entry, property.PropertyName, item);
                }
            }

            if (entry.ShouldUpdateFromPayload)
            {
                this.FoundNextLinkForCollection(collection as IEnumerable, nextLink, continuationPlan);
            }
            else
            {
                this.FoundNextLinkForUnmodifiedCollection(property.GetValue(entry.ResolvedObject) as IEnumerable);
            }

            if (this.mergeOption == MergeOption.OverwriteChanges || this.mergeOption == MergeOption.PreserveChanges)
            {
                var itemsToRemove =
                    from x in this.context.GetLinks(entry.ResolvedObject, property.PropertyName)
                    where MergeOption.OverwriteChanges == this.mergeOption || EntityStates.Added != x.State
                    select x.Target;
                itemsToRemove = itemsToRemove.Except(EnumerateAsElementType<object>(items));
                foreach (var item in itemsToRemove)
                {
                    if (collection != null)
                    {
                        property.RemoveValue(collection, item);
                    }

                    this.log.RemovedLink(entry, property.PropertyName, item);
                }
            }
        }

        private static void CheckEntryToAccessNotNull(AtomEntry entry, string name)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(name != null, "name != null");
            if (entry.IsNull)
            {
                throw new NullReferenceException(Strings.AtomMaterializer_EntryToAccessIsNull(name));
            }
        }

        private static ProjectionPlan CreatePlan(QueryComponents queryComponents)
        {
            LambdaExpression projection = queryComponents.Projection;
            if (projection == null)
            {
                return CreatePlanForDirectMaterialization(queryComponents.LastSegmentType);
            }
            ProjectionPlan plan = ProjectionPlanCompiler.CompilePlan(projection, queryComponents.NormalizerRewrites);
            plan.LastSegmentType = queryComponents.LastSegmentType;
            return plan;
        }

        private static ProjectionPlan CreatePlanForDirectMaterialization(Type lastSegmentType)
        {
            ProjectionPlan result = new ProjectionPlan();
            result.Plan = AtomMaterializerInvoker.DirectMaterializePlan;
            result.ProjectedType = lastSegmentType;
            result.LastSegmentType = lastSegmentType;
            return result;
        }

        private static ProjectionPlan CreatePlanForShallowMaterialization(Type lastSegmentType)
        {
            ProjectionPlan result = new ProjectionPlan();
            result.Plan = AtomMaterializerInvoker.ShallowMaterializePlan;
            result.ProjectedType = lastSegmentType;
            result.LastSegmentType = lastSegmentType;
            return result;
        }

        internal static object DirectMaterializePlan(AtomMaterializer materializer, AtomEntry entry, Type expectedEntryType)
        {
            materializer.Materialize(entry, expectedEntryType, true);
            return entry.ResolvedObject;
        }

        internal static IEnumerable<T> EnumerateAsElementType<T>(IEnumerable source)
        {
            Debug.Assert(source != null, "source != null");
            IEnumerable<T> enumerable = source as IEnumerable<T>;
            if (enumerable != null)
            {
                return enumerable;
            }
            return EnumerateAsElementTypeInternal<T>(source);
        }

        internal static IEnumerable<T> EnumerateAsElementTypeInternal<T>(IEnumerable source)
        {
             Debug.Assert(source != null, "source != null");

            foreach (object item in source)
            {
                yield return (T)item;
            }
        }

        private void FoundNextLinkForCollection(IEnumerable collection, Uri link, ProjectionPlan plan)
        {
            Debug.Assert((plan != null) || (link == null), "plan != null || link == null");
            if (!((collection == null) || this.nextLinkTable.ContainsKey(collection)))
            {
                DataServiceQueryContinuation continuation = DataServiceQueryContinuation.Create(link, plan);
                this.nextLinkTable.Add(collection, continuation);
                Util.SetNextLinkForCollection(collection, continuation);
            }
        }

        private void FoundNextLinkForUnmodifiedCollection(IEnumerable collection)
        {
            if (!((collection == null) || this.nextLinkTable.ContainsKey(collection)))
            {
                this.nextLinkTable.Add(collection, null);
            }
        }

        private static Action<object, object> GetAddToCollectionDelegate(Type listType)
        {
            Type type;
            ParameterExpression expression;
            ParameterExpression expression2;
            Debug.Assert(listType != null, "listType != null");
            MethodInfo addToCollectionMethod = ClientType.GetAddToCollectionMethod(listType, out type);
            return (Action<object, object>) ExpressionEvaluator.CreateDelegate(ExpressionHelpers.CreateLambda(Expression.Call(Expression.Convert(expression = Expression.Parameter(typeof(object), "list"), listType), addToCollectionMethod, new Expression[] { Expression.Convert(expression2 = Expression.Parameter(typeof(object), "element"), type) }), new ParameterExpression[] { expression, expression2 }));
        }

        private static object GetOrCreateCollectionProperty(object instance, ClientType.ClientProperty property, Type collectionType)
        {
            Debug.Assert(instance != null, "instance != null");
            Debug.Assert(property != null, "property != null");
            Debug.Assert(property.CollectionType != null, "property.CollectionType != null -- otherwise property isn't a collection");
            object obj2 = property.GetValue(instance);
            if (obj2 == null)
            {
                if (collectionType == null)
                {
                    collectionType = property.PropertyType;
                    if (collectionType.IsInterface)
                    {
                        collectionType = typeof(Collection<>).MakeGenericType(new Type[] { property.CollectionType });
                    }
                }
                obj2 = Activator.CreateInstance(collectionType);
                property.SetValue(instance, obj2, property.PropertyName, false);
            }
            Debug.Assert(obj2 != null, "result != null -- otherwise GetOrCreateCollectionProperty didn't fall back to creation");
            return obj2;
        }

        private static AtomContentProperty GetPropertyOrThrow(List<AtomContentProperty> properties, string propertyName)
        {
            AtomContentProperty atomProperty = null;
            if (properties != null)
            {
                atomProperty = properties.Where(p => p.Name == propertyName).FirstOrDefault();
            }

            if (atomProperty == null)
            {
                throw new InvalidOperationException(Strings.AtomMaterializer_PropertyMissing(propertyName));
            }

            Debug.Assert(atomProperty != null, "atomProperty != null");
            return atomProperty;
        }

        private static AtomContentProperty GetPropertyOrThrow(AtomEntry entry, string propertyName)
        {
            AtomContentProperty atomProperty = null;
            var properties = entry.DataValues;
            if (properties != null)
            {
                atomProperty = properties.Where(p => p.Name == propertyName).FirstOrDefault();
            }

            if (atomProperty == null)
            {
                throw new InvalidOperationException(Strings.AtomMaterializer_PropertyMissingFromEntry(propertyName, entry.Identity));
            }

            Debug.Assert(atomProperty != null, "atomProperty != null");
            return atomProperty;
        }

        internal static bool IsDataServiceCollection(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType && WebUtil.IsDataServiceCollectionType(type.GetGenericTypeDefinition()))
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        internal static List<TTarget> ListAsElementType<T, TTarget>(AtomMaterializer materializer, IEnumerable<T> source) where T: TTarget
        {
            List<TTarget> list2;
            DataServiceQueryContinuation continuation;
            Debug.Assert(materializer != null, "materializer != null");
            Debug.Assert(source != null, "source != null");
            List<TTarget> list = source as List<TTarget>;
            if (list != null)
            {
                return list;
            }
            IList list3 = source as IList;
            if (list3 != null)
            {
                list2 = new List<TTarget>(list3.Count);
            }
            else
            {
                list2 = new List<TTarget>();
            }
            foreach (T local in source)
            {
                list2.Add((TTarget) local);
            }
            if (materializer.nextLinkTable.TryGetValue(source, out continuation))
            {
                materializer.nextLinkTable[list2] = continuation;
            }
            return list2;
        }

        private void Materialize(AtomEntry entry, Type expectedEntryType, bool includeLinks)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(entry.DataValues != null, "entry.DataValues != null -- otherwise not correctly initialized");
            Debug.Assert((entry.ResolvedObject == null) || (entry.ResolvedObject == this.targetInstance), "entry.ResolvedObject == null || entry.ResolvedObject == this.targetInstance -- otherwise getting called twice");
            Debug.Assert(expectedEntryType != null, "expectedType != null");
            this.ResolveOrCreateInstance(entry, expectedEntryType);
            Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null -- otherwise ResolveOrCreateInstnace didn't do its job");
            this.MaterializeResolvedEntry(entry, includeLinks);
        }

        private static bool MaterializeDataValue(Type type, AtomContentProperty atomProperty, DataServiceContext context)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(atomProperty != null, "atomProperty != null");
            Debug.Assert(context != null, "context != null");
            string typeName = atomProperty.TypeName;
            string text = atomProperty.Text;
            ClientType type2 = null;
            Type type3 = Nullable.GetUnderlyingType(type) ?? type;
            bool flag = ClientConvert.IsKnownType(type3);
            if (!flag)
            {
                type2 = MaterializeAtom.GetEntryClientType(typeName, context, type, true);
                Debug.Assert(type2 != null, "nestedElementType != null -- otherwise ReadTypeAttribute (or someone!) should throw");
                flag = ClientConvert.IsKnownType(type2.ElementType);
            }
            if (flag)
            {
                if (atomProperty.IsNull)
                {
                    if (!ClientType.CanAssignNull(type))
                    {
                        throw new InvalidOperationException(Strings.AtomMaterializer_CannotAssignNull(atomProperty.Name, type.FullName));
                    }
                    atomProperty.MaterializedValue = null;
                    return true;
                }
                object obj2 = text;
                if (text != null)
                {
                    obj2 = ClientConvert.ChangeType(text, (type2 != null) ? type2.ElementType : type3);
                }
                atomProperty.MaterializedValue = obj2;
                return true;
            }
            return false;

            //Debug.Assert(type != null, "type != null");
            //Debug.Assert(atomProperty != null, "atomProperty != null");
            //Debug.Assert(context != null, "context != null");
            //string typeName = atomProperty.TypeName;
            //string text = atomProperty.Text;
            //ClientType type2 = null;
            //Type type3 = Nullable.GetUnderlyingType(type) ?? type;
            //bool flag = ClientConvert.IsKnownType(type3);
            //if (!flag)
            //{
            //    type2 = MaterializeAtom.GetEntryClientType(typeName, context, type, true);
            //    Debug.Assert(type2 != null, "nestedElementType != null -- otherwise ReadTypeAttribute (or someone!) should throw");
            //    flag = ClientConvert.IsKnownType(type2.ElementType);
            //}
            //if (flag)
            //{
            //    if (atomProperty.IsNull)
            //    {
            //        if (!ClientType.CanAssignNull(type))
            //        {
            //            throw new InvalidOperationException(Strings.AtomMaterializer_CannotAssignNull(atomProperty.Name, type.FullName));
            //        }
            //        atomProperty.MaterializedValue = null;
            //        return true;
            //    }
            //    object obj2 = text;
            //    if (text != null)
            //    {
            //        obj2 = ClientConvert.ChangeType(text, (type2 != null) ? type2.ElementType : type3);
            //    }
            //    atomProperty.MaterializedValue = obj2;
            //    return true;
            //}
            //return false;
        }

        private static void MaterializeDataValues(ClientType actualType, List<AtomContentProperty> values, bool ignoreMissingProperties, DataServiceContext context)
        {
            //Debug.Assert(actualType != null, "actualType != null");
            //Debug.Assert(values != null, "values != null");
            //Debug.Assert(context != null, "context != null");
            //foreach (AtomContentProperty property in values)
            //{
            //    string name = property.Name;
            //    ClientType.ClientProperty property2 = actualType.GetProperty(name, ignoreMissingProperties);
            //    if ((property2 != null) && (((property.Feed == null) && (property.Entry == null)) && !(MaterializeDataValue(property2.NullablePropertyType, property, context) || (property2.CollectionType == null))))
            //    {
            //        throw Error.NotSupported(Strings.ClientType_CollectionOfNonEntities);
            //    }
            //}

            Debug.Assert(actualType != null, "actualType != null");
            Debug.Assert(values != null, "values != null");
            Debug.Assert(context != null, "context != null");
            foreach (AtomContentProperty property in values)
            {
                string name = property.Name;
                ClientType.ClientProperty property2 = actualType.GetProperty(name, ignoreMissingProperties);
                if ((property2 != null) && ((property.Feed == null) && (property.Entry == null)))
                {
                    bool flag = MaterializeDataValue(property2.NullablePropertyType, property, context);
                }
            }

        }

        private void MaterializeResolvedEntry(AtomEntry entry, bool includeLinks)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null -- otherwise not resolved/created!");
            ClientType actualType = entry.ActualType;
            if (!entry.EntityPropertyMappingsApplied)
            {
                ApplyEntityPropertyMappings(entry, entry.ActualType);
            }
            MaterializeDataValues(actualType, entry.DataValues, this.ignoreMissingProperties, this.context);
            foreach (AtomContentProperty property in entry.DataValues)
            {
                ClientType.ClientProperty property2 = actualType.GetProperty(property.Name, this.ignoreMissingProperties);
                if (((property2 != null) && ((entry.ShouldUpdateFromPayload || (property.Entry != null)) || (property.Feed != null))) && (includeLinks || ((property.Entry == null) && (property.Feed == null))))
                {
                    ValidatePropertyMatch(property2, property);
                    AtomFeed feed = property.Feed;
                    if (feed != null)
                    {
                        Debug.Assert(includeLinks, "includeLinks -- otherwise we shouldn't be materializing this entry");
                        this.ApplyFeedToCollection(entry, property2, feed, includeLinks);
                    }
                    else if (property.Entry != null)
                    {
                        if (!property.IsNull)
                        {
                            Debug.Assert(includeLinks, "includeLinks -- otherwise we shouldn't be materializing this entry");
                            this.Materialize(property.Entry, property2.PropertyType, includeLinks);
                        }
                        if (entry.ShouldUpdateFromPayload)
                        {
                            property2.SetValue(entry.ResolvedObject, property.Entry.ResolvedObject, property.Name, true);
                            this.log.SetLink(entry, property2.PropertyName, property.Entry.ResolvedObject);
                        }
                    }
                    else
                    {
                        Debug.Assert(entry.ShouldUpdateFromPayload, "entry.ShouldUpdateFromPayload -- otherwise we're about to set a property we shouldn't");
                        ApplyDataValue(actualType, property, this.ignoreMissingProperties, this.context, entry.ResolvedObject);
                    }
                }
            }
            Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null -- otherwise we didn't do any useful work");
            if (this.materializedObjectCallback != null)
            {
                this.materializedObjectCallback(entry.Tag, entry.ResolvedObject);
            }
        }

        private static void MaterializeToList(AtomMaterializer materializer, IEnumerable list, Type nestedExpectedType, IEnumerable<AtomEntry> entries)
        {
            Debug.Assert(materializer != null, "materializer != null");
            Debug.Assert(list != null, "list != null");
            Action<object, object> addToCollectionDelegate = GetAddToCollectionDelegate(list.GetType());
            foreach (AtomEntry entry in entries)
            {
                if (!entry.EntityHasBeenResolved)
                {
                    materializer.Materialize(entry, nestedExpectedType, false);
                }
                addToCollectionDelegate(list, entry.ResolvedObject);
            }
        }

        private void MergeLists(AtomEntry entry, ClientType.ClientProperty property, IEnumerable list, Uri nextLink, ProjectionPlan plan)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null");
            Debug.Assert(property != null, "property != null");
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert((plan != null) || (nextLink == null), "plan != null || nextLink == null");
            if ((entry.ShouldUpdateFromPayload && (property.NullablePropertyType == list.GetType())) && (property.GetValue(entry.ResolvedObject) == null))
            {
                property.SetValue(entry.ResolvedObject, list, property.PropertyName, false);
                this.FoundNextLinkForCollection(list, nextLink, plan);
                foreach (object obj2 in list)
                {
                    this.log.AddedLink(entry, property.PropertyName, obj2);
                }
            }
            else
            {
                this.ApplyItemsToCollection(entry, property, list, nextLink, plan);
            }
        }

        internal static bool ProjectionCheckValueForPathIsNull(AtomEntry entry, Type expectedType, ProjectionPath path)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(path != null, "path != null");
            if ((path.Count == 0) || ((path.Count == 1) && (path[0].Member == null)))
            {
                return entry.IsNull;
            }
            bool isNull = false;
            AtomContentProperty atomProperty = null;
            List<AtomContentProperty> dataValues = entry.DataValues;
            for (int i = 0; i < path.Count; i++)
            {
                ProjectionPathSegment segment = path[i];
                if (segment.Member != null)
                {
                    bool condition = i == (path.Count - 1);
                    string member = segment.Member;
                    ClientType.ClientProperty property = ClientType.Create(expectedType).GetProperty(member, false);
                    atomProperty = GetPropertyOrThrow(dataValues, member);
                    ValidatePropertyMatch(property, atomProperty);
                    if (atomProperty.Feed != null)
                    {
                        Debug.Assert(condition, "segmentIsLeaf -- otherwise the path generated traverses a feed, which should be disallowed");
                        isNull = false;
                    }
                    else
                    {
                        Debug.Assert(atomProperty.Entry != null, "atomProperty.Entry != null -- otherwise a primitive property / complex type is being rewritte with a null check; this is only supported for entities and collection");
                        if (condition)
                        {
                            isNull = atomProperty.Entry.IsNull;
                        }
                        dataValues = atomProperty.Entry.DataValues;
                        entry = atomProperty.Entry;
                    }
                    expectedType = property.PropertyType;
                }
            }
            return isNull;
        }

        internal static void ProjectionEnsureEntryAvailableOfType(AtomMaterializer materializer, AtomEntry entry, Type requiredType)
        {
            Debug.Assert(materializer != null, "materializer != null");
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(materializer.targetInstance == null, "materializer.targetInstance == null -- projection shouldn't have a target instance set; that's only used for POST replies");
            if (entry.EntityHasBeenResolved)
            {
                if (!requiredType.IsAssignableFrom(entry.ResolvedObject.GetType()))
                {
                    throw new InvalidOperationException(string.Concat(new object[] { "Expecting type '", requiredType, "' for '", entry.Identity, "', but found a previously created instance of type '", entry.ResolvedObject.GetType() }));
                }
            }
            else
            {
                if (entry.Identity == null)
                {
                    throw Error.InvalidOperation(Strings.Deserialize_MissingIdElement);
                }
                if (!(materializer.TryResolveAsCreated(entry) || materializer.TryResolveFromContext(entry, requiredType)))
                {
                    materializer.ResolveByCreatingWithType(entry, requiredType);
                }
                else if (!requiredType.IsAssignableFrom(entry.ResolvedObject.GetType()))
                {
                    throw Error.InvalidOperation(Strings.Deserialize_Current(requiredType, entry.ResolvedObject.GetType()));
                }
            }
        }

        internal static AtomEntry ProjectionGetEntry(AtomEntry entry, string name)
        {
            Debug.Assert(entry != null, "entry != null -- ProjectionGetEntry never returns a null entry, and top-level materialization shouldn't pass one in");
            AtomContentProperty propertyOrThrow = GetPropertyOrThrow(entry, name);
            if (propertyOrThrow.Entry == null)
            {
                throw new InvalidOperationException(Strings.AtomMaterializer_PropertyNotExpectedEntry(name, entry.Identity));
            }
            CheckEntryToAccessNotNull(propertyOrThrow.Entry, name);
            return propertyOrThrow.Entry;
        }

        internal static object ProjectionInitializeEntity(AtomMaterializer materializer, AtomEntry entry, Type expectedType, Type resultType, string[] properties, Func<object, object, Type, object>[] propertyValues)
        {
            if ((entry == null) || entry.IsNull)
            {
                throw new NullReferenceException(Strings.AtomMaterializer_EntryToInitializeIsNull(resultType.FullName));
            }
            if (!entry.EntityHasBeenResolved)
            {
                ProjectionEnsureEntryAvailableOfType(materializer, entry, resultType);
            }
            else if (!resultType.IsAssignableFrom(entry.ActualType.ElementType))
            {
                throw new InvalidOperationException(Strings.AtomMaterializer_ProjectEntityTypeMismatch(resultType.FullName, entry.ActualType.ElementType.FullName, entry.Identity));
            }
            object resolvedObject = entry.ResolvedObject;
            for (int i = 0; i < properties.Length; i++)
            {
                ClientType.ClientProperty property = entry.ActualType.GetProperty(properties[i], materializer.ignoreMissingProperties);
                object target = propertyValues[i].Invoke(materializer, entry, expectedType);
                if (entry.ShouldUpdateFromPayload && ClientType.Create(property.NullablePropertyType, false).IsEntityType)
                {
                    materializer.Log.SetLink(entry, property.PropertyName, target);
                }
                bool flag = (property.CollectionType == null) || !ClientType.CheckElementTypeIsEntity(property.CollectionType);
                if (entry.ShouldUpdateFromPayload)
                {
                    if (flag)
                    {
                        property.SetValue(resolvedObject, target, property.PropertyName, false);
                    }
                    else
                    {
                        IEnumerable list = (IEnumerable) target;
                        DataServiceQueryContinuation continuation = materializer.nextLinkTable[list];
                        Uri nextLink = (continuation == null) ? null : continuation.NextLinkUri;
                        ProjectionPlan plan = (continuation == null) ? null : continuation.Plan;
                        materializer.MergeLists(entry, property, list, nextLink, plan);
                    }
                }
                else if (!flag)
                {
                    materializer.FoundNextLinkForUnmodifiedCollection(property.GetValue(entry.ResolvedObject) as IEnumerable);
                }
            }
            return resolvedObject;
        }

        internal static IEnumerable ProjectionSelect(AtomMaterializer materializer, AtomEntry entry, Type expectedType, Type resultType, ProjectionPath path, Func<object, object, Type, object> selector)
        {
            ClientType type = entry.ActualType ?? ClientType.Create(expectedType);
            IEnumerable collection = (IEnumerable) Util.ActivatorCreateInstance(typeof(List<>).MakeGenericType(new Type[] { resultType }), new object[0]);
            AtomContentProperty atomProperty = null;
            ClientType.ClientProperty property = null;
            for (int i = 0; i < path.Count; i++)
            {
                ProjectionPathSegment segment = path[i];
                if (segment.Member != null)
                {
                    string member = segment.Member;
                    property = type.GetProperty(member, false);
                    atomProperty = GetPropertyOrThrow(entry, member);
                    if (atomProperty.Entry != null)
                    {
                        entry = atomProperty.Entry;
                        type = ClientType.Create(property.NullablePropertyType, false);
                    }
                }
            }
            ValidatePropertyMatch(property, atomProperty);
            AtomFeed feed = atomProperty.Feed;
            Debug.Assert(feed != null, "sourceFeed != null -- otherwise ValidatePropertyMatch should have thrown or property isn't a collection (and should be part of this plan)");
            Action<object, object> addToCollectionDelegate = GetAddToCollectionDelegate(collection.GetType());
            foreach (AtomEntry entry2 in feed.Entries)
            {
                object obj2 = selector.Invoke(materializer, entry2, property.CollectionType);
                addToCollectionDelegate(collection, obj2);
            }
            ProjectionPlan plan = new ProjectionPlan();
            plan.LastSegmentType = property.CollectionType;
            plan.Plan = selector;
            plan.ProjectedType = resultType;
            materializer.FoundNextLinkForCollection(collection, feed.NextLink, plan);
            return collection;
        }

        internal static object ProjectionValueForPath(AtomMaterializer materializer, AtomEntry entry, Type expectedType, ProjectionPath path)
        {
            Debug.Assert(materializer != null, "materializer != null");
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(path != null, "path != null");
            if ((path.Count == 0) || ((path.Count == 1) && (path[0].Member == null)))
            {
                if (!entry.EntityHasBeenResolved)
                {
                    materializer.Materialize(entry, expectedType, false);
                }
                return entry.ResolvedObject;
            }
            object resolvedObject = null;
            AtomContentProperty atomProperty = null;
            List<AtomContentProperty> dataValues = entry.DataValues;
            for (int i = 0; i < path.Count; i++)
            {
                ProjectionPathSegment segment = path[i];
                if (segment.Member != null)
                {
                    bool condition = i == (path.Count - 1);
                    string member = segment.Member;
                    if (condition)
                    {
                        CheckEntryToAccessNotNull(entry, member);
                        if (!entry.EntityPropertyMappingsApplied)
                        {
                            ClientType entryType = MaterializeAtom.GetEntryClientType(entry.TypeName, materializer.context, expectedType, false);
                            ApplyEntityPropertyMappings(entry, entryType);
                        }
                    }
                    ClientType.ClientProperty property = ClientType.Create(expectedType).GetProperty(member, false);
                    atomProperty = GetPropertyOrThrow(dataValues, member);
                    ValidatePropertyMatch(property, atomProperty);
                    AtomFeed feed = atomProperty.Feed;
                    if (feed != null)
                    {
                        Debug.Assert(condition, "segmentIsLeaf -- otherwise the path generated traverses a feed, which should be disallowed");
                        Type implementationType = ClientType.GetImplementationType(segment.ProjectionType, typeof(ICollection<>));
                        if (implementationType == null)
                        {
                            implementationType = ClientType.GetImplementationType(segment.ProjectionType, typeof(IEnumerable<>));
                        }
                        Debug.Assert(implementationType != null, "collectionType != null -- otherwise the property should never have been recognized as a collection");
                        Type nestedExpectedType = implementationType.GetGenericArguments()[0];
                        Type projectionType = segment.ProjectionType;
                        if (projectionType.IsInterface || IsDataServiceCollection(projectionType))
                        {
                            projectionType = typeof(Collection<>).MakeGenericType(new Type[] { nestedExpectedType });
                        }
                        IEnumerable list = (IEnumerable) Util.ActivatorCreateInstance(projectionType, new object[0]);
                        MaterializeToList(materializer, list, nestedExpectedType, feed.Entries);
                        if (IsDataServiceCollection(segment.ProjectionType))
                        {
                            list = (IEnumerable) Util.ActivatorCreateInstance(WebUtil.GetDataServiceCollectionOfT(new Type[] { nestedExpectedType }), new object[] { list, TrackingMode.None });
                        }
                        ProjectionPlan plan = CreatePlanForShallowMaterialization(nestedExpectedType);
                        materializer.FoundNextLinkForCollection(list, feed.NextLink, plan);
                        resolvedObject = list;
                    }
                    else if (atomProperty.Entry != null)
                    {
                        if (!((!condition || atomProperty.Entry.EntityHasBeenResolved) || atomProperty.IsNull))
                        {
                            materializer.Materialize(atomProperty.Entry, property.PropertyType, false);
                        }
                        dataValues = atomProperty.Entry.DataValues;
                        resolvedObject = atomProperty.Entry.ResolvedObject;
                        entry = atomProperty.Entry;
                    }
                    else
                    {
                        if (atomProperty.Properties != null)
                        {
                            if (!((atomProperty.MaterializedValue != null) || atomProperty.IsNull))
                            {
                                ClientType actualType = ClientType.Create(property.PropertyType);
                                object instance = Util.ActivatorCreateInstance(property.PropertyType, new object[0]);
                                MaterializeDataValues(actualType, atomProperty.Properties, materializer.ignoreMissingProperties, materializer.context);
                                ApplyDataValues(actualType, atomProperty.Properties, materializer.ignoreMissingProperties, materializer.context, instance);
                                atomProperty.MaterializedValue = instance;
                            }
                        }
                        else
                        {
                            MaterializeDataValue(property.NullablePropertyType, atomProperty, materializer.context);
                        }
                        dataValues = atomProperty.Properties;
                        resolvedObject = atomProperty.MaterializedValue;
                    }
                    expectedType = property.PropertyType;
                }
            }
            return resolvedObject;
        }

        internal void PropagateContinuation<T>(IEnumerable<T> from, DataServiceCollection<T> to)
        {
            DataServiceQueryContinuation continuation;
            if (this.nextLinkTable.TryGetValue(from, out continuation))
            {
                this.nextLinkTable.Add(to, continuation);
                Util.SetNextLinkForCollection(to, continuation);
            }
        }

        internal bool Read()
        {
            this.currentValue = null;
            this.nextLinkTable.Clear();
            while (this.parser.Read())
            {
                Debug.Assert(this.parser.DataKind != AtomDataKind.None, "parser.DataKind != AtomDataKind.None -- otherwise parser.Read() didn't update its state");
                Debug.Assert(this.parser.DataKind != AtomDataKind.Finished, "parser.DataKind != AtomDataKind.Finished -- otherwise parser.Read() shouldn't have returned true");
                switch (this.parser.DataKind)
                {
                    case AtomDataKind.Entry:
                        Debug.Assert(this.parser.CurrentEntry != null, "parser.CurrentEntry != null -- otherwise parser.DataKind shouldn't be Entry");
                        this.CurrentEntry.ResolvedObject = this.TargetInstance;
                        this.currentValue = this.materializeEntryPlan.Run(this, this.CurrentEntry, this.expectedType);
                        return true;

                    case AtomDataKind.Feed:
                    case AtomDataKind.FeedCount:
                    case AtomDataKind.PagingLinks:
                        break;

                    default:
                    {
                        Debug.Assert(this.parser.DataKind == AtomDataKind.Custom, "parser.DataKind == AtomDataKind.Custom -- otherwise AtomMaterializer.Read switch is missing a case");
                        Type type = Nullable.GetUnderlyingType(this.expectedType) ?? this.expectedType;
                        ClientType actualType = ClientType.Create(type);
                        if (ClientConvert.IsKnownType(type))
                        {
                            string propertyValue = this.parser.ReadCustomElementString();
                            if (propertyValue != null)
                            {
                                this.currentValue = ClientConvert.ChangeType(propertyValue, type);
                            }
                            return true;
                        }
                        if (!actualType.IsEntityType && this.parser.IsDataWebElement)
                        {
                            AtomContentProperty property = this.parser.ReadCurrentPropertyValue();
                            if ((property == null) || property.IsNull)
                            {
                                this.currentValue = null;
                            }
                            else
                            {
                                this.currentValue = actualType.CreateInstance();
                                MaterializeDataValues(actualType, property.Properties, this.ignoreMissingProperties, this.context);
                                ApplyDataValues(actualType, property.Properties, this.ignoreMissingProperties, this.context, this.currentValue);
                            }
                            return true;
                        }
                        break;
                    }
                }
            }
            Debug.Assert(this.parser.DataKind == AtomDataKind.Finished, "parser.DataKind == AtomDataKind.None");
            Debug.Assert(this.parser.CurrentEntry == null, "parser.Current == null");
            return false;
        }

        private void ResolveByCreating(AtomEntry entry, Type expectedEntryType)
        {
            Debug.Assert(entry.ResolvedObject == null, "entry.ResolvedObject == null -- otherwise we're about to overwrite - should never be called");
            ClientType type = MaterializeAtom.GetEntryClientType(entry.TypeName, this.context, expectedEntryType, true);
            Debug.Assert(type != null, "actualType != null -- otherwise ClientType.Create returned a null value");
            this.ResolveByCreatingWithType(entry, type.ElementType);
        }

        private void ResolveByCreatingWithType(AtomEntry entry, Type type)
        {
            Debug.Assert(entry.ResolvedObject == null, "entry.ResolvedObject == null -- otherwise we're about to overwrite - should never be called");
            entry.ActualType = ClientType.Create(type);
            entry.ResolvedObject = Activator.CreateInstance(type);
            entry.CreatedByMaterializer = true;
            entry.ShouldUpdateFromPayload = true;
            entry.EntityHasBeenResolved = true;
            this.log.CreatedInstance(entry);
        }

        private void ResolveOrCreateInstance(AtomEntry entry, Type expectedEntryType)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(expectedEntryType != null, "expectedEntryType != null");
            Debug.Assert(!entry.EntityHasBeenResolved, "entry.EntityHasBeenResolved == false");
            if (!this.TryResolveAsTarget(entry))
            {
                if (entry.Identity == null)
                {
                    throw Error.InvalidOperation(Strings.Deserialize_MissingIdElement);
                }
                if (!this.TryResolveAsCreated(entry) && !this.TryResolveFromContext(entry, expectedEntryType))
                {
                    this.ResolveByCreating(entry, expectedEntryType);
                }
            }
            Debug.Assert(entry.ActualType != null, "entry.ActualType != null");
            Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null");
            Debug.Assert(entry.EntityHasBeenResolved, "entry.EntityHasBeenResolved");
        }

        private static void SetValueOnPath(List<AtomContentProperty> values, string path, string value, string typeName)
        {
            Debug.Assert(values != null, "values != null");
            Debug.Assert(path != null, "path != null");

            bool existing = true;
            AtomContentProperty property = null;
            foreach (string step in path.Split('/'))
            {
                if (values == null)
                {
                    Debug.Assert(property != null, "property != null -- if values is null then this isn't the first step");
                    property.EnsureProperties();
                    values = property.Properties;
                }

                property = values.Where(v => v.Name == step).FirstOrDefault();
                if (property == null)
                {
                    AtomContentProperty newProperty = new AtomContentProperty();
                    existing = false;
                    newProperty.Name = step;
                    values.Add(newProperty);
                    property = newProperty;
                }
                else
                {
                    if (property.IsNull)
                    {
                        return;
                    }
                }

                values = property.Properties;
            }

            Debug.Assert(property != null, "property != null -- property path should have at least one segment");

            if (existing == false)
            {
                property.TypeName = typeName;
                property.Text = value;
            }
        }

        internal static object ShallowMaterializePlan(AtomMaterializer materializer, AtomEntry entry, Type expectedEntryType)
        {
            materializer.Materialize(entry, expectedEntryType, false);
            return entry.ResolvedObject;
        }

        private bool TryResolveAsCreated(AtomEntry entry)
        {
            AtomEntry entry2;
            if (!this.log.TryResolve(entry, out entry2))
            {
                return false;
            }
            Debug.Assert(entry2.ResolvedObject != null, "existingEntry.ResolvedObject != null -- how did it get there otherwise?");
            entry.ActualType = entry2.ActualType;
            entry.ResolvedObject = entry2.ResolvedObject;
            entry.CreatedByMaterializer = entry2.CreatedByMaterializer;
            entry.ShouldUpdateFromPayload = entry2.ShouldUpdateFromPayload;
            entry.EntityHasBeenResolved = true;
            return true;
        }

        private bool TryResolveAsTarget(AtomEntry entry)
        {
            if (entry.ResolvedObject == null)
            {
                return false;
            }
            Debug.Assert(entry.ResolvedObject == this.TargetInstance, "entry.ResolvedObject == this.TargetInstance -- otherwise there we ResolveOrCreateInstance more than once on the same entry");
            Debug.Assert((this.mergeOption == MergeOption.OverwriteChanges) || (this.mergeOption == MergeOption.PreserveChanges), "MergeOption.OverwriteChanges and MergeOption.PreserveChanges are the only expected values during SaveChanges");
            entry.ActualType = ClientType.Create(entry.ResolvedObject.GetType());
            this.log.FoundTargetInstance(entry);
            entry.ShouldUpdateFromPayload = this.mergeOption != MergeOption.PreserveChanges;
            entry.EntityHasBeenResolved = true;
            return true;
        }

        private bool TryResolveFromContext(AtomEntry entry, Type expectedEntryType)
        {
            if (this.mergeOption != MergeOption.NoTracking)
            {
                EntityStates states;
                entry.ResolvedObject = this.context.TryGetEntity(entry.Identity, entry.ETagText, this.mergeOption, out states);
                if (entry.ResolvedObject != null)
                {
                    if (!expectedEntryType.IsInstanceOfType(entry.ResolvedObject))
                    {
                        throw Error.InvalidOperation(Strings.Deserialize_Current(expectedEntryType, entry.ResolvedObject.GetType()));
                    }
                    entry.ActualType = ClientType.Create(entry.ResolvedObject.GetType());
                    entry.EntityHasBeenResolved = true;
                    entry.ShouldUpdateFromPayload = ((this.mergeOption == MergeOption.OverwriteChanges) || ((this.mergeOption == MergeOption.PreserveChanges) && (states == EntityStates.Unchanged))) || ((this.mergeOption == MergeOption.PreserveChanges) && (states == EntityStates.Deleted));
                    this.log.FoundExistingInstance(entry);
                    return true;
                }
            }
            return false;
        }

        internal static void ValidatePropertyMatch(ClientType.ClientProperty property, AtomContentProperty atomProperty)
        {
            Debug.Assert(property != null, "property != null");
            Debug.Assert(atomProperty != null, "atomProperty != null");
            if (property.IsKnownType && ((atomProperty.Feed != null) || (atomProperty.Entry != null)))
            {
                throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkLocalSimple);
            }
            if ((atomProperty.Feed != null) && (property.CollectionType == null))
            {
                throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkFeedPropertyNotCollection(property.PropertyName));
            }
            if ((atomProperty.Entry != null) && (property.CollectionType != null))
            {
                throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkEntryPropertyIsCollection(property.PropertyName));
            }
        }

        internal DataServiceContext Context
        {
            get
            {
                return this.context;
            }
        }

        internal AtomEntry CurrentEntry
        {
            get
            {
                return this.parser.CurrentEntry;
            }
        }

        internal AtomFeed CurrentFeed
        {
            get
            {
                return this.parser.CurrentFeed;
            }
        }

        internal object CurrentValue
        {
            get
            {
                return this.currentValue;
            }
        }

        internal bool IsEndOfStream
        {
            get
            {
                return (this.parser.DataKind == AtomDataKind.Finished);
            }
        }

        internal AtomMaterializerLog Log
        {
            get
            {
                return this.log;
            }
        }

        internal ProjectionPlan MaterializeEntryPlan
        {
            get
            {
                return this.materializeEntryPlan;
            }
        }

        internal Dictionary<IEnumerable, DataServiceQueryContinuation> NextLinkTable
        {
            get
            {
                return this.nextLinkTable;
            }
        }

        internal object TargetInstance
        {
            get
            {
                return this.targetInstance;
            }
            set
            {
                Debug.Assert(value != null, "value != null -- otherwise we have no instance target.");
                this.targetInstance = value;
            }
        }

        
    }
}

