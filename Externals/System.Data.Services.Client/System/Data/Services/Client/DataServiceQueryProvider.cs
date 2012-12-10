namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    internal sealed class DataServiceQueryProvider : IQueryProvider
    {
        internal readonly DataServiceContext Context;

        internal DataServiceQueryProvider(DataServiceContext context)
        {
            this.Context = context;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Util.CheckArgumentNull(expression, "expression");
            Type et = TypeSystem.GetElementType(expression.Type);
            Type qt = typeof(DataServiceQuery<>.DataServiceOrderedQuery).MakeGenericType(et);
            object[] args = new object[] { expression, this };

            ConstructorInfo ci = qt.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] { typeof(Expression), typeof(DataServiceQueryProvider) },
                null);

            return (IQueryable)Util.ConstructorInvoke(ci, args);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            Util.CheckArgumentNull<Expression>(expression, "expression");
            return new DataServiceQuery<TElement>.DataServiceOrderedQuery(expression, this);
        }

        public object Execute(Expression expression)
        {
            Util.CheckArgumentNull<Expression>(expression, "expression");
            return typeof(DataServiceQueryProvider).GetMethod("ReturnSingleton", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new Type[] { expression.Type }).Invoke(this, new object[] { expression });
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Util.CheckArgumentNull<Expression>(expression, "expression");
            return this.ReturnSingleton<TResult>(expression);
        }

        internal TElement ReturnSingleton<TElement>(Expression expression)
        {
            SequenceMethod method;
            IQueryable<TElement> source = new DataServiceQuery<TElement>.DataServiceOrderedQuery(expression, this);
            MethodCallExpression m = expression as MethodCallExpression;
            Debug.Assert(m != null, "mce != null");
            if (!ReflectionUtil.TryIdentifySequenceMethod(m.Method, out method))
            {
                Debug.Assert(false, "Not supported singleton operator not caught by Resource Binder");
                throw Error.MethodNotSupported(m);
            }
            switch (method)
            {
                case SequenceMethod.First:
                    return source.AsEnumerable<TElement>().First<TElement>();

                case SequenceMethod.FirstOrDefault:
                    return source.AsEnumerable<TElement>().FirstOrDefault<TElement>();

                case SequenceMethod.Single:
                    return source.AsEnumerable<TElement>().Single<TElement>();

                case SequenceMethod.SingleOrDefault:
                    return source.AsEnumerable<TElement>().SingleOrDefault<TElement>();
            }
            throw Error.MethodNotSupported(m);
        }

        internal QueryComponents Translate(Expression e)
        {
            Uri uri;
            Version version;
            bool addTrailingParens = false;
            Dictionary<Expression, Expression> rewrites = null;
            if (!(e is ResourceSetExpression))
            {
                rewrites = new Dictionary<Expression, Expression>(ReferenceEqualityComparer<Expression>.Instance);
                e = Evaluator.PartialEval(e);
                e = ExpressionNormalizer.Normalize(e, rewrites);
                e = ResourceBinder.Bind(e);
                addTrailingParens = true;
            }
            UriWriter.Translate(this.Context, addTrailingParens, e, out uri, out version);
            ResourceExpression expression = e as ResourceExpression;
            Type lastSegmentType = (expression.Projection == null) ? expression.ResourceType : expression.Projection.Selector.Parameters[0].Type;
            return new QueryComponents(uri, version, lastSegmentType, (expression.Projection == null) ? null : expression.Projection.Selector, rewrites);
        }
    }
}

