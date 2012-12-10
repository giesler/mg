namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public class QueryOperationResponse : OperationResponse, IEnumerable
    {
        private readonly DataServiceRequest query;
        private readonly MaterializeAtom results;

        internal QueryOperationResponse(Dictionary<string, string> headers, DataServiceRequest query, MaterializeAtom results) : base(headers)
        {
            this.query = query;
            this.results = results;
        }

        public DataServiceQueryContinuation GetContinuation()
        {
            return this.results.GetContinuation(null);
        }

        public DataServiceQueryContinuation<T> GetContinuation<T>(IEnumerable<T> collection)
        {
            return (DataServiceQueryContinuation<T>) this.results.GetContinuation(collection);
        }

        public DataServiceQueryContinuation GetContinuation(IEnumerable collection)
        {
            return this.results.GetContinuation(collection);
        }

        public IEnumerator GetEnumerator()
        {
            return this.Results.GetEnumerator();
        }

        internal static QueryOperationResponse GetInstance(Type elementType, Dictionary<string, string> headers, DataServiceRequest query, MaterializeAtom results)
        {
            ConstructorInfo[] constructors = typeof(QueryOperationResponse<>).MakeGenericType(new Type[] { elementType }).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            Debug.Assert(1 == constructors.Length, "only expected 1 ctor");
            return (QueryOperationResponse) Util.ConstructorInvoke(constructors[0], new object[] { headers, query, results });
        }

        public DataServiceRequest Query
        {
            get
            {
                return this.query;
            }
        }

        internal MaterializeAtom Results
        {
            get
            {
                if (null != this.Error)
                {
                    throw System.Data.Services.Client.Error.InvalidOperation(Strings.Context_BatchExecuteError, this.Error);
                }

                return this.results;
            }
        }

        public virtual long TotalCount
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}

