namespace System.Data.Services.Common
{
    using System;
    using System.Data.Services.Client;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Xml;

    internal sealed class EpmCustomContentWriterNodeData : IDisposable
    {
        public string Data { get; set; }
        public MemoryStream XmlContentStream { get; set; }
        public XmlWriter XmlContentWriter { get; set; }

        //[CompilerGenerated]
        //private string <Data>k__BackingField;
        //[CompilerGenerated]
        //private MemoryStream <XmlContentStream>k__BackingField;
        //[CompilerGenerated]
        //private XmlWriter <XmlContentWriter>k__BackingField;
        private bool disposed;

        internal EpmCustomContentWriterNodeData(EpmTargetPathSegment segment, object element)
        {
            this.XmlContentStream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            this.XmlContentWriter = XmlWriter.Create(this.XmlContentStream, settings);
            this.PopulateData(segment, element);
        }

        internal EpmCustomContentWriterNodeData(EpmCustomContentWriterNodeData parentData, EpmTargetPathSegment segment, object element)
        {
            this.XmlContentStream = parentData.XmlContentStream;
            this.XmlContentWriter = parentData.XmlContentWriter;
            this.PopulateData(segment, element);
        }

        internal void AddContentToTarget(XmlWriter target)
        {
            Util.CheckArgumentNull<XmlWriter>(target, "target");
            this.XmlContentWriter.Close();
            this.XmlContentWriter = null;
            this.XmlContentStream.Seek(0L, SeekOrigin.Begin);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader reader = XmlReader.Create(this.XmlContentStream, settings);
            this.XmlContentStream = null;
            target.WriteNode(reader, false);
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.XmlContentWriter != null)
                {
                    this.XmlContentWriter.Close();
                    this.XmlContentWriter = null;
                }
                if (this.XmlContentStream != null)
                {
                    this.XmlContentStream.Dispose();
                    this.XmlContentStream = null;
                }
                this.disposed = true;
            }
        }

        private void PopulateData(EpmTargetPathSegment segment, object element)
        {
            if (segment.EpmInfo != null)
            {
                object obj2;
                try
                {
                    obj2 = ClientType.ReadPropertyValue(element, segment.EpmInfo.ActualType, segment.EpmInfo.Attribute.SourcePath);
                }
                catch (TargetInvocationException)
                {
                    throw;
                }
                this.Data = (obj2 == null) ? string.Empty : ClientConvert.ToString(obj2, false);
            }
        }

        //internal string Data
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Data>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Data>k__BackingField = value;
        //    }
        //}

        //internal MemoryStream XmlContentStream
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<XmlContentStream>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<XmlContentStream>k__BackingField = value;
        //    }
        //}

        //internal XmlWriter XmlContentWriter
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<XmlContentWriter>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<XmlContentWriter>k__BackingField = value;
        //    }
        //}
    }
}

