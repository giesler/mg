namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class DataServiceQuery<TElement> : DataServiceQuery, IQueryable<TElement>, IEnumerable<TElement>, IQueryable, IEnumerable
    {
        private System.Data.Services.Client.QueryComponents queryComponents;
        private readonly System.Linq.Expressions.Expression queryExpression;
        private readonly DataServiceQueryProvider queryProvider;

        private DataServiceQuery(System.Linq.Expressions.Expression expression, DataServiceQueryProvider provider)
        {
            Debug.Assert(null != provider.Context, "null context");
            Debug.Assert(expression != null, "null expression");
            Debug.Assert(provider != null, "Currently only support Web Query Provider");
            this.queryExpression = expression;
            this.queryProvider = provider;
        }

        public DataServiceQuery<TElement> AddQueryOption(string name, object value)
        {
            Util.CheckArgumentNull(name, "name");
            Util.CheckArgumentNull(value, "value");
            MethodInfo mi = typeof(DataServiceQuery<TElement>).GetMethod("AddQueryOption");
            return (DataServiceQuery<TElement>)this.Provider.CreateQuery<TElement>(
                Expression.Call(
                    Expression.Convert(this.Expression, typeof(DataServiceQuery<TElement>.DataServiceOrderedQuery)),
                    mi,
                    new Expression[] { Expression.Constant(name), Expression.Constant(value, typeof(object)) }));
        }

        public IAsyncResult BeginExecute(AsyncCallback callback, object state)
        {
            return base.BeginExecute(this, this.queryProvider.Context, callback, state);
        }

        internal override IAsyncResult BeginExecuteInternal(AsyncCallback callback, object state)
        {
            return this.BeginExecute(callback, state);
        }

        public IEnumerable<TElement> EndExecute(IAsyncResult asyncResult)
        {
            return DataServiceRequest.EndExecute<TElement>(this, this.queryProvider.Context, asyncResult);
        }

        internal override IEnumerable EndExecuteInternal(IAsyncResult asyncResult)
        {
            return this.EndExecute(asyncResult);
        }

        public DataServiceQuery<TElement> Expand(string path)
        {
            Util.CheckArgumentNull(path, "path");
            Util.CheckArgumentNotEmpty(path, "path");

            MethodInfo mi = typeof(DataServiceQuery<TElement>).GetMethod("Expand");
            return (DataServiceQuery<TElement>)this.Provider.CreateQuery<TElement>(
                Expression.Call(
                    Expression.Convert(this.Expression, typeof(DataServiceQuery<TElement>.DataServiceOrderedQuery)),
                    mi,
                    new Expression[] { Expression.Constant(path) }));
        }

        public DataServiceQuery<TElement> IncludeTotalCount()
        {
            MethodInfo mi = typeof(DataServiceQuery<TElement>).GetMethod("IncludeTotalCount");

            return (DataServiceQuery<TElement>)this.Provider.CreateQuery<TElement>(
                Expression.Call(
                    Expression.Convert(this.Expression, typeof(DataServiceQuery<TElement>.DataServiceOrderedQuery)),
                    mi));
        }

        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
        {
            throw Error.NotSupported(Strings.DataServiceQuery_EnumerationNotSupportedInSL);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw Error.NotSupported();
        }

        public override string ToString()
        {
            try
            {
                return base.ToString();
            }
            catch (NotSupportedException exception)
            {
                return Strings.ALinq_TranslationError(exception.Message);
            }
        }

        private System.Data.Services.Client.QueryComponents Translate()
        {
            if (this.queryComponents == null)
            {
                this.queryComponents = this.queryProvider.Translate(this.queryExpression);
            }
            return this.queryComponents;
        }

        public override Type ElementType
        {
            get
            {
                return typeof(TElement);
            }
        }

        public override System.Linq.Expressions.Expression Expression
        {
            get
            {
                return this.queryExpression;
            }
        }

        internal override ProjectionPlan Plan
        {
            get
            {
                return null;
            }
        }

        public override IQueryProvider Provider
        {
            get
            {
                return this.queryProvider;
            }
        }

        internal override System.Data.Services.Client.QueryComponents QueryComponents
        {
            get
            {
                return this.Translate();
            }
        }

        public override Uri RequestUri
        {
            get
            {
                return this.Translate().Uri;
            }
        }

        internal class DataServiceOrderedQuery : DataServiceQuery<TElement>, IOrderedQueryable<TElement>, IQueryable<TElement>, IEnumerable<TElement>, IOrderedQueryable, IQueryable, IEnumerable
        {
            internal DataServiceOrderedQuery(Expression expression, DataServiceQueryProvider provider) : base(expression, provider)
            {
            }
        }
    }
}

