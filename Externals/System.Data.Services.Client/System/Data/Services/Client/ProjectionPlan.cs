namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    internal class ProjectionPlan
    {
        public Type LastSegmentType { get; set; }
        public Func<object, object, Type, object> Plan { get; set; }
        public Type ProjectedType { get; set; }
        public Expression SourceProjection { get; set; }
        public Expression TargetProjection { get; set; }

        //[CompilerGenerated]
        //private Type <LastSegmentType>k__BackingField;
        //[CompilerGenerated]
        //private Func<object, object, Type, object> <Plan>k__BackingField;
        //[CompilerGenerated]
        //private Type <ProjectedType>k__BackingField;
        //[CompilerGenerated]
        //private Expression <SourceProjection>k__BackingField;
        //[CompilerGenerated]
        //private Expression <TargetProjection>k__BackingField;

        internal object Run(AtomMaterializer materializer, AtomEntry entry, Type expectedType)
        {
            Debug.Assert(materializer != null, "materializer != null");
            Debug.Assert(entry != null, "entry != null");
            return this.Plan.Invoke(materializer, entry, expectedType);
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "Plan - projection: ", this.SourceProjection, "\r\nBecomes: ", this.TargetProjection });
        }

        //internal Type LastSegmentType
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<LastSegmentType>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<LastSegmentType>k__BackingField = value;
        //    }
        //}

        //internal Func<object, object, Type, object> Plan
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Plan>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<Plan>k__BackingField = value;
        //    }
        //}

        //internal Type ProjectedType
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ProjectedType>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ProjectedType>k__BackingField = value;
        //    }
        //}

        //internal Expression SourceProjection
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<SourceProjection>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<SourceProjection>k__BackingField = value;
        //    }
        //}

        //internal Expression TargetProjection
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<TargetProjection>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<TargetProjection>k__BackingField = value;
        //    }
        //}
    }
}

