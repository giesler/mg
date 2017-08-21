namespace System.Data.Services.Client.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Xml;

    internal class XmlWrappingReader : XmlReader, IXmlLineInfo
    {
        private string previousReaderBaseUri;
        private XmlReader reader;
        private IXmlLineInfo readerAsIXmlLineInfo;
        private Stack<XmlBaseState> xmlBaseStack;

        internal XmlWrappingReader(XmlReader baseReader)
        {
            this.Reader = baseReader;
        }

        public override void Close()
        {
            this.reader.Close();
        }

        internal static XmlWrappingReader CreateReader(string currentBaseUri, XmlReader newReader)
        {
            Debug.Assert(!(newReader is XmlWrappingReader), "The new reader must not be a xmlWrappingReader");
            XmlWrappingReader reader = new XmlWrappingReader(newReader);
            reader.previousReaderBaseUri = currentBaseUri;
            return reader;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.reader != null)
            {
                ((IDisposable) this.reader).Dispose();
            }
            base.Dispose(disposing);
        }

        public override string GetAttribute(int i)
        {
            return this.reader.GetAttribute(i);
        }

        public override string GetAttribute(string name)
        {
            return this.reader.GetAttribute(name);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return this.reader.GetAttribute(name, namespaceURI);
        }

        public virtual bool HasLineInfo()
        {
            return ((this.readerAsIXmlLineInfo != null) && this.readerAsIXmlLineInfo.HasLineInfo());
        }

        public override string LookupNamespace(string prefix)
        {
            return this.reader.LookupNamespace(prefix);
        }

        public override void MoveToAttribute(int i)
        {
            this.reader.MoveToAttribute(i);
        }

        public override bool MoveToAttribute(string name)
        {
            return this.reader.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return this.reader.MoveToAttribute(name, ns);
        }

        public override bool MoveToElement()
        {
            return this.reader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return this.reader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return this.reader.MoveToNextAttribute();
        }

        private void PopXmlBase()
        {
            if (((this.xmlBaseStack != null) && (this.xmlBaseStack.Count > 0)) && (this.reader.Depth == this.xmlBaseStack.Peek().Depth))
            {
                this.xmlBaseStack.Pop();
            }
        }

        public override bool Read()
        {
            if (this.reader.NodeType == XmlNodeType.EndElement)
            {
                this.PopXmlBase();
            }
            else
            {
                this.reader.MoveToElement();
                if (this.reader.IsEmptyElement)
                {
                    this.PopXmlBase();
                }
            }
            bool flag = this.reader.Read();
            if (flag && ((this.reader.NodeType == XmlNodeType.Element) && this.reader.HasAttributes))
            {
                string attribute = this.reader.GetAttribute("xml:base");
                if (string.IsNullOrEmpty(attribute))
                {
                    return flag;
                }
                Uri requestUri = null;
                requestUri = Util.CreateUri(attribute, UriKind.RelativeOrAbsolute);
                if (this.xmlBaseStack == null)
                {
                    this.xmlBaseStack = new Stack<XmlBaseState>();
                }
                if (this.xmlBaseStack.Count > 0)
                {
                    requestUri = Util.CreateUri(this.xmlBaseStack.Peek().BaseUri, requestUri);
                }
                this.xmlBaseStack.Push(new XmlBaseState(requestUri, this.reader.Depth));
            }
            return flag;
        }

        public override bool ReadAttributeValue()
        {
            return this.reader.ReadAttributeValue();
        }

        public override void ResolveEntity()
        {
            this.reader.ResolveEntity();
        }

        public override void Skip()
        {
            this.reader.Skip();
        }

        public override int AttributeCount
        {
            get
            {
                return this.reader.AttributeCount;
            }
        }

        public override string BaseURI
        {
            get
            {
                if ((this.xmlBaseStack != null) && (this.xmlBaseStack.Count > 0))
                {
                    return this.xmlBaseStack.Peek().BaseUri.AbsoluteUri;
                }
                if (!string.IsNullOrEmpty(this.previousReaderBaseUri))
                {
                    return this.previousReaderBaseUri;
                }
                return this.reader.BaseURI;
            }
        }

        public override bool CanResolveEntity
        {
            get
            {
                return this.reader.CanResolveEntity;
            }
        }

        public override int Depth
        {
            get
            {
                return this.reader.Depth;
            }
        }

        public override bool EOF
        {
            get
            {
                return this.reader.EOF;
            }
        }

        public override bool HasAttributes
        {
            get
            {
                return this.reader.HasAttributes;
            }
        }

        public override bool HasValue
        {
            get
            {
                return this.reader.HasValue;
            }
        }

        public override bool IsDefault
        {
            get
            {
                return this.reader.IsDefault;
            }
        }

        public override bool IsEmptyElement
        {
            get
            {
                return this.reader.IsEmptyElement;
            }
        }

        public virtual int LineNumber
        {
            get
            {
                if (this.readerAsIXmlLineInfo != null)
                {
                    return this.readerAsIXmlLineInfo.LineNumber;
                }
                return 0;
            }
        }

        public virtual int LinePosition
        {
            get
            {
                if (this.readerAsIXmlLineInfo != null)
                {
                    return this.readerAsIXmlLineInfo.LinePosition;
                }
                return 0;
            }
        }

        public override string LocalName
        {
            get
            {
                return this.reader.LocalName;
            }
        }

        public override string Name
        {
            get
            {
                return this.reader.Name;
            }
        }

        public override string NamespaceURI
        {
            get
            {
                return this.reader.NamespaceURI;
            }
        }

        public override XmlNameTable NameTable
        {
            get
            {
                return this.reader.NameTable;
            }
        }

        public override XmlNodeType NodeType
        {
            get
            {
                return this.reader.NodeType;
            }
        }

        public override string Prefix
        {
            get
            {
                return this.reader.Prefix;
            }
        }

        protected XmlReader Reader
        {
            get
            {
                return this.reader;
            }
            set
            {
                this.reader = value;
                this.readerAsIXmlLineInfo = value as IXmlLineInfo;
            }
        }

        public override System.Xml.ReadState ReadState
        {
            get
            {
                return this.reader.ReadState;
            }
        }

        public override XmlReaderSettings Settings
        {
            get
            {
                return this.reader.Settings;
            }
        }

        public override string Value
        {
            get
            {
                return this.reader.Value;
            }
        }

        public override Type ValueType
        {
            get
            {
                return this.reader.ValueType;
            }
        }

        public override string XmlLang
        {
            get
            {
                return this.reader.XmlLang;
            }
        }

        public override System.Xml.XmlSpace XmlSpace
        {
            get
            {
                return this.reader.XmlSpace;
            }
        }

        private class XmlBaseState
        {
            public Uri BaseUri { get; set; }
            public int Depth { get; set; }
            //[CompilerGenerated]
            //private Uri <BaseUri>k__BackingField;
            //[CompilerGenerated]
            //private int <Depth>k__BackingField;

            internal XmlBaseState(Uri baseUri, int depth)
            {
                this.BaseUri = baseUri;
                this.Depth = depth;
            }

            //public Uri BaseUri
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<BaseUri>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    private set
            //    {
            //        this.<BaseUri>k__BackingField = value;
            //    }
            //}

            //public int Depth
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Depth>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    private set
            //    {
            //        this.<Depth>k__BackingField = value;
            //    }
            //}
        }
    }
}

