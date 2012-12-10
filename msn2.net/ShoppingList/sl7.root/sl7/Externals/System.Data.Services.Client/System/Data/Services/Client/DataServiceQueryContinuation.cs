namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    [DebuggerDisplay("{NextLinkUri}")]
    public abstract class DataServiceQueryContinuation
    {
        private readonly Uri nextLinkUri;
        private readonly ProjectionPlan plan;

        internal DataServiceQueryContinuation(Uri nextLinkUri, ProjectionPlan plan)
        {
            Debug.Assert(nextLinkUri != null, "nextLinkUri != null");
            Debug.Assert(plan != null, "plan != null");
            this.nextLinkUri = nextLinkUri;
            this.plan = plan;
        }

        internal static DataServiceQueryContinuation Create(Uri nextLinkUri, ProjectionPlan plan)
        {
            Debug.Assert((plan != null) || (nextLinkUri == null), "plan != null || nextLinkUri == null");
            if (nextLinkUri == null)
            {
                return null;
            }
            ConstructorInfo[] constructors = typeof(DataServiceQueryContinuation<>).MakeGenericType(new Type[] { plan.ProjectedType }).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            Debug.Assert(constructors.Length == 1, "constructors.Length == 1");
            return (DataServiceQueryContinuation) Util.ConstructorInvoke(constructors[0], new object[] { nextLinkUri, plan });
        }

        internal QueryComponents CreateQueryComponents()
        {
            return new QueryComponents(this.NextLinkUri, Util.DataServiceVersionEmpty, this.Plan.LastSegmentType, null, null);
        }

        public override string ToString()
        {
            return this.NextLinkUri.ToString();
        }

        internal abstract Type ElementType { get; }

        public Uri NextLinkUri
        {
            get
            {
                return this.nextLinkUri;
            }
        }

        internal ProjectionPlan Plan
        {
            get
            {
                return this.plan;
            }
        }
    }
}

