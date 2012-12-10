namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Text;

    [DebuggerDisplay("{ToString()}")]
    internal class ProjectionPath : List<ProjectionPathSegment>
    {
        public Expression ExpectedRootType { get; set; }
        public ParameterExpression Root { get; set; }
        public Expression RootEntry { get; set; }
        //[CompilerGenerated]
        //private Expression <ExpectedRootType>k__BackingField;
        //[CompilerGenerated]
        //private ParameterExpression <Root>k__BackingField;
        //[CompilerGenerated]
        //private Expression <RootEntry>k__BackingField;

        internal ProjectionPath()
        {
        }

        internal ProjectionPath(ParameterExpression root, Expression expectedRootType, Expression rootEntry)
        {
            this.Root = root;
            this.RootEntry = rootEntry;
            this.ExpectedRootType = expectedRootType;
        }

        internal ProjectionPath(ParameterExpression root, Expression expectedRootType, Expression rootEntry, IEnumerable<Expression> members) : this(root, expectedRootType, rootEntry)
        {
            Debug.Assert(members != null, "members != null");
            foreach (Expression expression in members)
            {
                base.Add(new ProjectionPathSegment(this, ((MemberExpression) expression).Member.Name, expression.Type));
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Root.ToString());
            builder.Append("->");
            for (int i = 0; i < base.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append('.');
                }
                builder.Append((base[i].Member == null) ? "*" : base[i].Member);
            }
            return builder.ToString();
        }

        //internal Expression ExpectedRootType
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ExpectedRootType>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<ExpectedRootType>k__BackingField = value;
        //    }
        //}

        //internal ParameterExpression Root
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Root>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Root>k__BackingField = value;
        //    }
        //}

        //internal Expression RootEntry
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<RootEntry>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<RootEntry>k__BackingField = value;
        //    }
        //}
    }
}

