namespace System.Data.Services.Common
{
    using System;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("EntityPropertyMappingInfo {DefiningType}")]
    internal sealed class EntityPropertyMappingInfo
    {
        public ClientType ActualType { get; set; }
        public EntityPropertyMappingAttribute Attribute { get; set; }
        public Type DefiningType { get; set; }

        //[CompilerGenerated]
        //private ClientType <ActualType>k__BackingField;
        //[CompilerGenerated]
        //private EntityPropertyMappingAttribute <Attribute>k__BackingField;
        //[CompilerGenerated]
        //private Type <DefiningType>k__BackingField;

        //public ClientType ActualType
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ActualType>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ActualType>k__BackingField = value;
        //    }
        //}

        //public EntityPropertyMappingAttribute Attribute
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Attribute>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Attribute>k__BackingField = value;
        //    }
        //}

        //public Type DefiningType
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<DefiningType>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<DefiningType>k__BackingField = value;
        //    }
        //}
    }
}

