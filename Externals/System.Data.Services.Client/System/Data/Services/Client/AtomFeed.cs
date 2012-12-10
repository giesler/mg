namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class AtomFeed
    {
        public long? Count { get; set; }
        public IEnumerable<AtomEntry> Entries { get; set; }
        public Uri NextLink { get; set; }
        //[CompilerGenerated]
        //private long? <Count>k__BackingField;
        //[CompilerGenerated]
        //private IEnumerable<AtomEntry> <Entries>k__BackingField;
        //[CompilerGenerated]
        //private Uri <NextLink>k__BackingField;

        //public long? Count
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Count>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Count>k__BackingField = value;
        //    }
        //}

        //public IEnumerable<AtomEntry> Entries
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Entries>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Entries>k__BackingField = value;
        //    }
        //}

        //public Uri NextLink
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<NextLink>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<NextLink>k__BackingField = value;
        //    }
        //}
    }
}

