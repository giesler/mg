namespace System.Data.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class EpmSourcePathSegment
    {
        public EntityPropertyMappingInfo EpmInfo { get; set; }
        //[CompilerGenerated]
        //private EntityPropertyMappingInfo <EpmInfo>k__BackingField;
        private string propertyName;
        private List<EpmSourcePathSegment> subProperties;

        internal EpmSourcePathSegment(string propertyName)
        {
            this.propertyName = propertyName;
            this.subProperties = new List<EpmSourcePathSegment>();
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

        internal string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }

        internal List<EpmSourcePathSegment> SubProperties
        {
            get
            {
                return this.subProperties;
            }
        }
    }
}

