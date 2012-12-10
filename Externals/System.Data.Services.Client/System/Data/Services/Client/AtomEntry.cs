namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("AtomEntry {ResolvedObject} @ {Identity}")]
    internal class AtomEntry
    {
        
        public ClientType ActualType {get; set;}
        
        public List<AtomContentProperty> DataValues {get; set;}
        
        public Uri EditLink {get; set;}
        
        public string ETagText {get; set;}
        
        public string Identity {get; set;}
        
        public Uri MediaContentUri {get; set;}
        
        public Uri MediaEditUri {get; set;}
        
        public Uri QueryLink {get; set;}
        
        public object ResolvedObject {get; set;}
        
        public string StreamETagText {get; set;}
        
        public object Tag {get; set;}
        
        public string TypeName {get; set;}
        private EntryFlags flags;

        private bool GetFlagValue(EntryFlags mask)
        {
            return ((this.flags & mask) != 0);
        }

        private void SetFlagValue(EntryFlags mask, bool value)
        {
            if (value)
            {
                this.flags |= mask;
            }
            else
            {
                this.flags &= ~mask;
            }
        }

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

        public bool CreatedByMaterializer
        {
            get
            {
                return this.GetFlagValue(EntryFlags.CreatedByMaterializer);
            }
            set
            {
                this.SetFlagValue(EntryFlags.CreatedByMaterializer, value);
            }
        }

        //public List<AtomContentProperty> DataValues
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<DataValues>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<DataValues>k__BackingField = value;
        //    }
        //}

        //public Uri EditLink
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<EditLink>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<EditLink>k__BackingField = value;
        //    }
        //}

        public bool EntityHasBeenResolved
        {
            get
            {
                return this.GetFlagValue(EntryFlags.EntityHasBeenResolved);
            }
            set
            {
                this.SetFlagValue(EntryFlags.EntityHasBeenResolved, value);
            }
        }

        public bool EntityPropertyMappingsApplied
        {
            get
            {
                return this.GetFlagValue(EntryFlags.EntityPropertyMappingsApplied);
            }
            set
            {
                this.SetFlagValue(EntryFlags.EntityPropertyMappingsApplied, value);
            }
        }

        //public string ETagText
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ETagText>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ETagText>k__BackingField = value;
        //    }
        //}

        //public string Identity
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Identity>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Identity>k__BackingField = value;
        //    }
        //}

        public bool IsNull
        {
            get
            {
                return this.GetFlagValue(EntryFlags.IsNull);
            }
            set
            {
                this.SetFlagValue(EntryFlags.IsNull, value);
            }
        }

        //public Uri MediaContentUri
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<MediaContentUri>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<MediaContentUri>k__BackingField = value;
        //    }
        //}

        //public Uri MediaEditUri
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<MediaEditUri>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<MediaEditUri>k__BackingField = value;
        //    }
        //}

        public bool? MediaLinkEntry
        {
            get
            {
                return (this.GetFlagValue(EntryFlags.MediaLinkEntryAssigned) ? new bool?(this.GetFlagValue(EntryFlags.MediaLinkEntryValue)) : null);
            }
            set
            {
                Debug.Assert(value.HasValue, "value.HasValue -- callers shouldn't set the value to unknown");
                this.SetFlagValue(EntryFlags.MediaLinkEntryAssigned, true);
                this.SetFlagValue(EntryFlags.MediaLinkEntryValue, value.Value);
            }
        }

        //public Uri QueryLink
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<QueryLink>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<QueryLink>k__BackingField = value;
        //    }
        //}

        //public object ResolvedObject
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ResolvedObject>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ResolvedObject>k__BackingField = value;
        //    }
        //}

        public bool ShouldUpdateFromPayload
        {
            get
            {
                return this.GetFlagValue(EntryFlags.ShouldUpdateFromPayload);
            }
            set
            {
                this.SetFlagValue(EntryFlags.ShouldUpdateFromPayload, value);
            }
        }

        //public string StreamETagText
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<StreamETagText>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<StreamETagText>k__BackingField = value;
        //    }
        //}

        //public object Tag
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Tag>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Tag>k__BackingField = value;
        //    }
        //}

        //public string TypeName
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<TypeName>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<TypeName>k__BackingField = value;
        //    }
        //}

        [Flags]
        private enum EntryFlags
        {
            CreatedByMaterializer = 2,
            EntityHasBeenResolved = 4,
            EntityPropertyMappingsApplied = 0x20,
            IsNull = 0x40,
            MediaLinkEntryAssigned = 0x10,
            MediaLinkEntryValue = 8,
            ShouldUpdateFromPayload = 1
        }
    }
}

