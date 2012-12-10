namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class QueryOperationResponse<T> : QueryOperationResponse, IEnumerable<T>, IEnumerable
    {
        internal QueryOperationResponse(Dictionary<string, string> headers, DataServiceRequest query, MaterializeAtom results) : base(headers, query, results)
        {
        }

        public DataServiceQueryContinuation<T> GetContinuation()
        {
            return (DataServiceQueryContinuation<T>) base.GetContinuation();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return base.Results.Cast<T>().GetEnumerator();
        }

        public override long TotalCount
        {
            get
            {
                if ((base.Results == null) || base.Results.IsEmptyResults)
                {
                    throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
                }
                return base.Results.CountValue();
            }
        }
    }
}

