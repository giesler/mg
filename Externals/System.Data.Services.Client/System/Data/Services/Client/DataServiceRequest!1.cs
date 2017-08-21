namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;

    public sealed class DataServiceRequest<TElement> : DataServiceRequest
    {
        private readonly ProjectionPlan plan;
        private readonly System.Data.Services.Client.QueryComponents queryComponents;

        public DataServiceRequest(Uri requestUri)
        {
            Util.CheckArgumentNull<Uri>(requestUri, "requestUri");
            Type type = typeof(TElement);
            type = ClientConvert.IsKnownType(type) ? type : TypeSystem.GetElementType(type);
            this.queryComponents = new System.Data.Services.Client.QueryComponents(requestUri, Util.DataServiceVersionEmpty, type, null, null);
        }

        internal DataServiceRequest(System.Data.Services.Client.QueryComponents queryComponents, ProjectionPlan plan)
        {
            Debug.Assert(queryComponents != null, "queryComponents != null");
            this.queryComponents = queryComponents;
            this.plan = plan;
        }

        public override Type ElementType
        {
            get
            {
                return typeof(TElement);
            }
        }

        internal override ProjectionPlan Plan
        {
            get
            {
                return this.plan;
            }
        }

        internal override System.Data.Services.Client.QueryComponents QueryComponents
        {
            get
            {
                return this.queryComponents;
            }
        }

        public override Uri RequestUri
        {
            get
            {
                return this.queryComponents.Uri;
            }
        }
    }
}

