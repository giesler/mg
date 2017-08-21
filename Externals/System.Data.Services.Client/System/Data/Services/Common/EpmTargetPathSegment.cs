namespace System.Data.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("EpmTargetPathSegment {SegmentName} HasContent={HasContent}")]
    internal class EpmTargetPathSegment
    {
        //[CompilerGenerated]
        //private EntityPropertyMappingInfo <EpmInfo>k__BackingField;
        public EntityPropertyMappingInfo EpmInfo { get; set; }
        private EpmTargetPathSegment parentSegment;
        private string segmentName;
        private string segmentNamespacePrefix;
        private string segmentNamespaceUri;
        private List<EpmTargetPathSegment> subSegments;

        internal EpmTargetPathSegment()
        {
            this.subSegments = new List<EpmTargetPathSegment>();
        }

        internal EpmTargetPathSegment(string segmentName, string segmentNamespaceUri, string segmentNamespacePrefix, EpmTargetPathSegment parentSegment) : this()
        {
            this.segmentName = segmentName;
            this.segmentNamespaceUri = segmentNamespaceUri;
            this.segmentNamespacePrefix = segmentNamespacePrefix;
            this.parentSegment = parentSegment;
        }

        //internal EntityPropertyMappingInfo EpmInfo
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<EpmInfo>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<EpmInfo>k__BackingField = value;
        //    }
        //}

        internal bool HasContent
        {
            get
            {
                return (this.EpmInfo != null);
            }
        }

        internal bool IsAttribute
        {
            get
            {
                return (this.SegmentName[0] == '@');
            }
        }

        internal EpmTargetPathSegment ParentSegment
        {
            get
            {
                return this.parentSegment;
            }
        }

        internal string SegmentName
        {
            get
            {
                return this.segmentName;
            }
        }

        internal string SegmentNamespacePrefix
        {
            get
            {
                return this.segmentNamespacePrefix;
            }
        }

        internal string SegmentNamespaceUri
        {
            get
            {
                return this.segmentNamespaceUri;
            }
        }

        internal List<EpmTargetPathSegment> SubSegments
        {
            get
            {
                return this.subSegments;
            }
        }
    }
}

