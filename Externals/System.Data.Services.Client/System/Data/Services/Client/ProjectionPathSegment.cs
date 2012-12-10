namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("Segment {ProjectionType} {Member}")]
    internal class ProjectionPathSegment
    {
        public string Member { get; set; }
        public Type ProjectionType { get; set; }
        public ProjectionPath StartPath { get; set; }
        //[CompilerGenerated]
        //private string <Member>k__BackingField;
        //[CompilerGenerated]
        //private Type <ProjectionType>k__BackingField;
        //[CompilerGenerated]
        //private ProjectionPath <StartPath>k__BackingField;

        internal ProjectionPathSegment(ProjectionPath startPath, string member, Type projectionType)
        {
            Debug.Assert(startPath != null, "startPath != null");
            this.Member = member;
            this.StartPath = startPath;
            this.ProjectionType = projectionType;
        }

        //internal string Member
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Member>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Member>k__BackingField = value;
        //    }
        //}

        //internal Type ProjectionType
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ProjectionType>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ProjectionType>k__BackingField = value;
        //    }
        //}

        //internal ProjectionPath StartPath
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<StartPath>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<StartPath>k__BackingField = value;
        //    }
        //}
    }
}

