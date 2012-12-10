namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class DataServiceQuery : DataServiceRequest, IQueryable, IEnumerable
    {
        internal DataServiceQuery()
        {
        }

        public IAsyncResult BeginExecute(AsyncCallback callback, object state)
        {
            return this.BeginExecuteInternal(callback, state);
        }

        internal abstract IAsyncResult BeginExecuteInternal(AsyncCallback callback, object state);
        public IEnumerable EndExecute(IAsyncResult asyncResult)
        {
            return this.EndExecuteInternal(asyncResult);
        }

        internal abstract IEnumerable EndExecuteInternal(IAsyncResult asyncResult);
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw Error.NotImplemented();
        }

        public abstract System.Linq.Expressions.Expression Expression { get; }

        public abstract IQueryProvider Provider { get; }
    }
}

