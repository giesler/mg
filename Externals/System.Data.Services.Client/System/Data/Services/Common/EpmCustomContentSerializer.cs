namespace System.Data.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Xml;

    internal sealed class EpmCustomContentSerializer : EpmContentSerializerBase, IDisposable
    {
        private bool disposed;
        private Dictionary<EpmTargetPathSegment, EpmCustomContentWriterNodeData> visitorContent;

        internal EpmCustomContentSerializer(EpmTargetTree targetTree, object element, XmlWriter target) : base(targetTree, false, element, target)
        {
            this.InitializeVisitorContent();
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                foreach (EpmTargetPathSegment segment in base.Root.SubSegments)
                {
                    EpmCustomContentWriterNodeData data = this.visitorContent[segment];
                    Debug.Assert(data != null, "Must have custom data for all the children of root");
                    if (base.Success)
                    {
                        data.AddContentToTarget(base.Target);
                    }
                    data.Dispose();
                }
                this.disposed = true;
            }
        }

        private void InitializeSubSegmentVisitorContent(EpmTargetPathSegment subSegment)
        {
            foreach (EpmTargetPathSegment segment in subSegment.SubSegments)
            {
                this.visitorContent.Add(segment, new EpmCustomContentWriterNodeData(this.visitorContent[subSegment], segment, base.Element));
                this.InitializeSubSegmentVisitorContent(segment);
            }
        }

        private void InitializeVisitorContent()
        {
            this.visitorContent = new Dictionary<EpmTargetPathSegment, EpmCustomContentWriterNodeData>(ReferenceEqualityComparer<EpmTargetPathSegment>.Instance);
            foreach (EpmTargetPathSegment segment in base.Root.SubSegments)
            {
                this.visitorContent.Add(segment, new EpmCustomContentWriterNodeData(segment, base.Element));
                this.InitializeSubSegmentVisitorContent(segment);
            }
        }

        protected override void Serialize(EpmTargetPathSegment targetSegment, EpmSerializationKind kind)
        {
            if (targetSegment.IsAttribute)
            {
                this.WriteAttribute(targetSegment);
            }
            else
            {
                this.WriteElement(targetSegment);
            }
        }

        private void WriteAttribute(EpmTargetPathSegment targetSegment)
        {
            Debug.Assert(targetSegment.HasContent, "Must have content for attributes");
            EpmCustomContentWriterNodeData data = this.visitorContent[targetSegment];
            data.XmlContentWriter.WriteAttributeString(targetSegment.SegmentNamespacePrefix, targetSegment.SegmentName.Substring(1), targetSegment.SegmentNamespaceUri, data.Data);
        }

        private void WriteElement(EpmTargetPathSegment targetSegment)
        {
            EpmCustomContentWriterNodeData data = this.visitorContent[targetSegment];
            data.XmlContentWriter.WriteStartElement(targetSegment.SegmentNamespacePrefix, targetSegment.SegmentName, targetSegment.SegmentNamespaceUri);
            base.Serialize(targetSegment, EpmSerializationKind.Attributes);
            if (targetSegment.HasContent)
            {
                Debug.Assert(data.Data != null, "Must always have non-null data content value");
                data.XmlContentWriter.WriteString(data.Data);
            }
            base.Serialize(targetSegment, EpmSerializationKind.Elements);
            data.XmlContentWriter.WriteEndElement();
        }
    }
}

