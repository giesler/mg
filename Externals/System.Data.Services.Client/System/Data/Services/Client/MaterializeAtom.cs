namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client.Xml;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Xml;
    using System.Xml.Linq;

    internal class MaterializeAtom : IDisposable, IEnumerable, IEnumerator
    {
        private bool calledGetEnumerator;
        private readonly DataServiceContext context;
        private const long CountStateFailure = -2L;
        private const long CountStateInitial = -1L;
        private long countValue;
        private object current;
        private readonly Type elementType;
        private readonly bool expectingSingleValue;
        private readonly bool ignoreMissingProperties;
        private readonly AtomMaterializer materializer;
        internal readonly MergeOption MergeOptionValue;
        private bool moved;
        private readonly AtomParser parser;
        private XmlReader reader;
        private TextWriter writer;

        private MaterializeAtom()
        {
        }

        private MaterializeAtom(DataServiceContext context, XmlReader reader, Type type, MergeOption mergeOption) : this(context, reader, new QueryComponents(null, Util.DataServiceVersionEmpty, type, null, null), null, mergeOption)
        {
        }

        internal MaterializeAtom(DataServiceContext context, XmlReader reader, QueryComponents queryComponents, ProjectionPlan plan, MergeOption mergeOption)
        {
            Debug.Assert(queryComponents != null, "queryComponents != null");

            this.context = context;
            this.elementType = queryComponents.LastSegmentType;
            this.MergeOptionValue = mergeOption;
            this.ignoreMissingProperties = context.IgnoreMissingProperties;
            this.reader = (reader == null) ? null : new System.Data.Services.Client.Xml.XmlAtomErrorReader(reader);
            this.countValue = CountStateInitial;
            this.expectingSingleValue = ClientConvert.IsKnownNullableType(elementType);

            Debug.Assert(reader != null, "Materializer reader is null! Did you mean to use Materializer.ResultsWrapper/EmptyResults?");

            reader.Settings.NameTable.Add(context.DataNamespace);

            string typeScheme = this.context.TypeScheme.OriginalString;
            this.parser = new AtomParser(this.reader, AtomParser.XElementBuilderCallback, typeScheme, context.DataNamespace);
            AtomMaterializerLog log = new AtomMaterializerLog(this.context, mergeOption);
            Type implementationType;
            Type materializerType = GetTypeForMaterializer(this.expectingSingleValue, this.elementType, out implementationType);
            this.materializer = new AtomMaterializer(parser, context, materializerType, this.ignoreMissingProperties, mergeOption, log, this.MaterializedObjectCallback, queryComponents, plan);
        }

        private void CheckGetEnumerator()
        {
            if (this.calledGetEnumerator)
            {
                throw Error.NotSupported(Strings.Deserialize_GetEnumerator);
            }
            this.calledGetEnumerator = true;
        }

        internal long CountValue()
        {
            if (this.countValue == -1L)
            {
                this.ReadCountValue();
            }
            else if (this.countValue == -2L)
            {
                throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
            }
            return this.countValue;
        }

        internal static MaterializeAtom CreateWrapper(IEnumerable results)
        {
            return new ResultsWrapper(results, null);
        }

        internal static MaterializeAtom CreateWrapper(IEnumerable results, DataServiceQueryContinuation continuation)
        {
            return new ResultsWrapper(results, continuation);
        }

        public void Dispose()
        {
            this.current = null;
            if (null != this.reader)
            {
                ((IDisposable) this.reader).Dispose();
            }
            if (null != this.writer)
            {
                this.writer.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        internal virtual DataServiceQueryContinuation GetContinuation(IEnumerable key)
        {
            DataServiceQueryContinuation continuation;
            Debug.Assert(this.materializer != null, "Materializer is null!");
            if (key == null)
            {
                if ((this.expectingSingleValue && !this.moved) || (!this.expectingSingleValue && !this.materializer.IsEndOfStream))
                {
                    throw new InvalidOperationException(Strings.MaterializeFromAtom_TopLevelLinkNotAvailable);
                }
                if (this.expectingSingleValue || (this.materializer.CurrentFeed == null))
                {
                    return null;
                }
                return DataServiceQueryContinuation.Create(this.materializer.CurrentFeed.NextLink, this.materializer.MaterializeEntryPlan);
            }
            if (!this.materializer.NextLinkTable.TryGetValue(key, out continuation))
            {
                throw new ArgumentException(Strings.MaterializeFromAtom_CollectionKeyNotPresentInLinkTable);
            }
            return continuation;
        }

        internal static ClientType GetEntryClientType(string typeName, DataServiceContext context, Type expectedType, bool checkAssignable)
        {
            Debug.Assert(context != null, "context != null");
            ClientType type2 = ClientType.Create(context.ResolveTypeFromName(typeName, expectedType, checkAssignable));
            Debug.Assert(type2 != null, "result != null -- otherwise ClientType.Create returned null");
            return type2;
        }

        public virtual IEnumerator GetEnumerator()
        {
            this.CheckGetEnumerator();
            return this;
        }

        private static Type GetTypeForMaterializer(bool expectingSingleValue, Type elementType, out Type implementationType)
        {
            if (!expectingSingleValue && typeof(IEnumerable).IsAssignableFrom(elementType))
            {
                implementationType = ClientType.GetImplementationType(elementType, typeof(ICollection<>));
                if (implementationType != null)
                {
                    return implementationType.GetGenericArguments()[0];
                }
            }
            implementationType = null;
            return elementType;
        }

        private void MaterializedObjectCallback(object tag, object entity)
        {
            Debug.Assert(tag != null, "tag != null");
            Debug.Assert(entity != null, "entity != null");
            XElement element = (XElement) tag;
            if (this.context.HasReadingEntityHandlers)
            {
                XmlUtil.RemoveDuplicateNamespaceAttributes(element);
                this.context.FireReadingEntityEvent(entity, element);
            }
        }

        public bool MoveNext()
        {
            bool flag2;
            bool applyingChanges = this.context.ApplyingChanges;
            try
            {
                this.context.ApplyingChanges = true;
                flag2 = this.MoveNextInternal();
            }
            finally
            {
                this.context.ApplyingChanges = applyingChanges;
            }
            return flag2;
        }

        private bool MoveNextInternal()
        {
            Type elementType;
            if (this.reader == null)
            {
                Debug.Assert(this.current == null, "this.current == null -- otherwise this.reader should have some value.");
                return false;
            }
            this.current = null;
            this.materializer.Log.Clear();
            bool flag = false;
            GetTypeForMaterializer(this.expectingSingleValue, this.elementType, out elementType);
            if (elementType != null)
            {
                if (this.moved)
                {
                    return false;
                }
                Type type2 = elementType.GetGenericArguments()[0];
                elementType = this.elementType;
                if (elementType.IsInterface)
                {
                    elementType = typeof(Collection<>).MakeGenericType(new Type[] { type2 });
                }
                IList list = (IList) Activator.CreateInstance(elementType);
                while (this.materializer.Read())
                {
                    this.moved = true;
                    list.Add(this.materializer.CurrentValue);
                }
                this.current = list;
                flag = true;
            }
            if (null == this.current)
            {
                if (this.expectingSingleValue && this.moved)
                {
                    flag = false;
                }
                else
                {
                    flag = this.materializer.Read();
                    if (flag)
                    {
                        this.current = this.materializer.CurrentValue;
                    }
                    this.moved = true;
                }
            }
            this.materializer.Log.ApplyToContext();
            return flag;
        }

        private void ReadCountValue()
        {
            Debug.Assert(this.countValue == -1L, "Count value is not in the initial state");
            if ((this.materializer.CurrentFeed != null) && this.materializer.CurrentFeed.Count.HasValue)
            {
                this.countValue = this.materializer.CurrentFeed.Count.Value;
            }
            else
            {
                while ((this.reader.NodeType != XmlNodeType.Element) && this.reader.Read())
                {
                }
                if (this.reader.EOF)
                {
                    throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
                }
                Debug.Assert((Util.AreSame("http://www.w3.org/2005/Atom", this.reader.NamespaceURI) && Util.AreSame("feed", this.reader.LocalName)) || (Util.AreSame("http://schemas.microsoft.com/ado/2007/08/dataservices", this.reader.NamespaceURI) && Util.AreSame("links", this.reader.LocalName)), "<feed> or <links> tag expected");
                XElement element = XElement.Load(this.reader);
                this.reader.Close();
                XElement element2 = element.Descendants(XNamespace.Get("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata") + "count").FirstOrDefault<XElement>();
                if (element2 == null)
                {
                    throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
                }
                if (!long.TryParse(element2.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out this.countValue))
                {
                    throw new FormatException(Strings.MaterializeFromAtom_CountFormatError);
                }
                if (this.countValue < 0L)
                {
                    throw new FormatException(Strings.MaterializeFromAtom_CountFormatError);
                }
                this.reader = new XmlAtomErrorReader(element.CreateReader());
                this.parser.ReplaceReader(this.reader);
            }
        }

        internal static string ReadElementString(XmlReader reader, bool checkNullAttribute)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert(reader.NodeType == XmlNodeType.Element, "reader.NodeType == XmlNodeType.Element -- otherwise caller is confused as to where the reader is");
            string str = null;
            bool flag = checkNullAttribute && !Util.DoesNullAttributeSayTrue(reader);
            if (reader.IsEmptyElement)
            {
                return (flag ? string.Empty : null);
            }
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                        if (null != str)
                        {
                            throw Error.InvalidOperation(Strings.Deserialize_MixedTextWithComment);
                        }
                        str = reader.Value;
                        break;

                    case XmlNodeType.Comment:
                    case XmlNodeType.Whitespace:
                        break;

                    case XmlNodeType.EndElement:
                        return (str ?? (flag ? string.Empty : null));

                    default:
                        throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
                }
            }
            throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
        }

        internal void SetInsertingObject(object addedObject)
        {
            this.materializer.TargetInstance = addedObject;
        }

        internal static void SkipToEnd(XmlReader reader)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert(reader.NodeType == XmlNodeType.Element, "reader.NodeType == XmlNodeType.Element");
            if (!reader.IsEmptyElement)
            {
                int depth = reader.Depth;
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.EndElement) && (reader.Depth == depth))
                    {
                        break;
                    }
                }
            }
        }

        void IEnumerator.Reset()
        {
            throw Error.NotSupported();
        }

        internal DataServiceContext Context
        {
            get
            {
                return this.context;
            }
        }

        public object Current
        {
            get
            {
                return this.current;
            }
        }

        internal static MaterializeAtom EmptyResults
        {
            get
            {
                return new ResultsWrapper(null, null);
            }
        }

        internal bool IsEmptyResults
        {
            get
            {
                return (this.reader == null);
            }
        }

        private class ResultsWrapper : MaterializeAtom
        {
            private readonly DataServiceQueryContinuation continuation;
            private readonly IEnumerable results;

            internal ResultsWrapper(IEnumerable results, DataServiceQueryContinuation continuation)
            {
                this.results = results ?? new object[0];
                this.continuation = continuation;
            }

            internal override DataServiceQueryContinuation GetContinuation(IEnumerable key)
            {
                if (key != null)
                {
                    throw new InvalidOperationException(Strings.MaterializeFromAtom_GetNestLinkForFlatCollection);
                }
                return this.continuation;
            }

            public override IEnumerator GetEnumerator()
            {
                return this.results.GetEnumerator();
            }
        }
    }
}

