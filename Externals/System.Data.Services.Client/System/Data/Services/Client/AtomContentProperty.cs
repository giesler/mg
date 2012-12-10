namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("AtomContentProperty {TypeName} {Name}")]
    internal class AtomContentProperty
    {
        
        public AtomEntry Entry {get; set;}
        
        public AtomFeed Feed {get; set;}
        
        public bool IsNull {get; set;}
        
        public object MaterializedValue {get; set;}
        
        public string Name {get; set;}
        
        public List<AtomContentProperty> Properties {get; set;}
        
        public string Text {get; set;}
        
        public string TypeName {get; set;}

        public void EnsureProperties()
        {
            if (this.Properties == null)
            {
                this.Properties = new List<AtomContentProperty>();
            }
        }

        //public AtomEntry Entry
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Entry {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Entry>k__BackingField = value;
        //    }
        //}

        //public AtomFeed Feed
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Feed {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Feed>k__BackingField = value;
        //    }
        //}

        //public bool IsNull
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<IsNull {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<IsNull>k__BackingField = value;
        //    }
        //}

        //public object MaterializedValue
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<MaterializedValue {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<MaterializedValue>k__BackingField = value;
        //    }
        //}

        //public string Name
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Name {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Name>k__BackingField = value;
        //    }
        //}

        //public List<AtomContentProperty> Properties
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Properties {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Properties>k__BackingField = value;
        //    }
        //}

        //public string Text
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Text {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Text>k__BackingField = value;
        //    }
        //}

        //public string TypeName
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<TypeName {get; set;}
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<TypeName>k__BackingField = value;
        //    }
        //}
    }
}

