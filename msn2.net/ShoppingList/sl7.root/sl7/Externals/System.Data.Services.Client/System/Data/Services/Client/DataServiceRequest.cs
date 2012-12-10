namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public abstract class DataServiceRequest
    {
        internal DataServiceRequest()
        {
        }

        internal IAsyncResult BeginExecute(object source, DataServiceContext context, AsyncCallback callback, object state)
        {
            QueryResult result = this.CreateResult(source, context, callback, state);
            result.BeginExecute();
            return result;
        }

        private QueryResult CreateResult(object source, DataServiceContext context, AsyncCallback callback, object state)
        {
            Debug.Assert(null != context, "context is null");
            return new QueryResult(source, "Execute", this, context.CreateRequest(this.QueryComponents.Uri, "GET", false, null, this.QueryComponents.Version, false), callback, state);
        }

        internal static IEnumerable<TElement> EndExecute<TElement>(object source, DataServiceContext context, IAsyncResult asyncResult)
        {
            QueryResult result = null;
            try
            {
                result = QueryResult.EndExecute<TElement>(source, asyncResult);
                return result.ProcessResult<TElement>(context, result.ServiceRequest.Plan);
            }
            catch (DataServiceQueryException exception)
            {
                Exception innerException = exception;
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }
                DataServiceClientException exception3 = innerException as DataServiceClientException;
                if ((!context.IgnoreResourceNotFoundException || (exception3 == null)) || (exception3.StatusCode != 0x194))
                {
                    throw;
                }
                QueryOperationResponse response = new QueryOperationResponse<TElement>(new Dictionary<string, string>(exception.Response.Headers), exception.Response.Query, MaterializeAtom.EmptyResults);
                response.StatusCode = 0x194;
                return (IEnumerable<TElement>) response;
            }
        }

        internal static DataServiceRequest GetInstance(Type elementType, Uri requestUri)
        {
            return (DataServiceRequest) Activator.CreateInstance(typeof(DataServiceRequest<>).MakeGenericType(new Type[] { elementType }), new object[] { requestUri });
        }

        internal static MaterializeAtom Materialize(DataServiceContext context, System.Data.Services.Client.QueryComponents queryComponents, ProjectionPlan plan, string contentType, Stream response)
        {
            Debug.Assert(null != queryComponents, "querycomponents");
            string mime = null;
            Encoding encoding = null;
            if (!string.IsNullOrEmpty(contentType))
            {
                HttpProcessUtility.ReadContentType(contentType, out mime, out encoding);
            }
            if ((string.Equals(mime, "application/atom+xml", StringComparison.OrdinalIgnoreCase) || string.Equals(mime, "application/xml", StringComparison.OrdinalIgnoreCase)) && (null != response))
            {
                return new MaterializeAtom(context, XmlUtil.CreateXmlReader(response, encoding), queryComponents, plan, context.MergeOption);
            }
            return MaterializeAtom.EmptyResults;
        }

        public override string ToString()
        {
            return this.QueryComponents.Uri.ToString();
        }

        public abstract Type ElementType { get; }

        internal abstract ProjectionPlan Plan { get; }

        internal abstract System.Data.Services.Client.QueryComponents QueryComponents { get; }

        public abstract Uri RequestUri { get; }
    }
}

