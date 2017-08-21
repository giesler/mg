namespace System.Linq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class EnumerableQuery<T> : EnumerableQuery, IOrderedQueryable<T>, IQueryable<T>, IOrderedQueryable, IQueryable, IQueryProvider, IEnumerable<T>, IEnumerable
    {
        private IEnumerable<T> enumerable;
        private System.Linq.Expressions.Expression expression;

        public EnumerableQuery(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
            this.expression = System.Linq.Expressions.Expression.Constant(this);
        }

        public EnumerableQuery(System.Linq.Expressions.Expression expression)
        {
            this.expression = expression;
        }

        private IEnumerator<T> GetEnumerator()
        {
            if (this.enumerable == null)
            {
                EnumerableRewriter rewriter = new EnumerableRewriter();
                Expression<Func<IEnumerable<T>>> expression2 = System.Linq.Expressions.Expression.Lambda<Func<IEnumerable<T>>>(rewriter.Visit(this.expression), (IEnumerable<ParameterExpression>) null);
                // TODO: Fix compile
                //this.enumerable = expression2.Compile().Invoke();
            }
            return this.enumerable.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IQueryable IQueryProvider.CreateQuery(System.Linq.Expressions.Expression expression)
        {
            if (expression == null)
            {
                throw LinqError.ArgumentNull("expression");
            }
            Type type = TypeHelper.FindGenericType(typeof(IQueryable<>), expression.Type);
            if (type == null)
            {
                throw LinqError.ArgumentNotValid("expression");
            }
            return EnumerableQuery.Create(type.GetGenericArguments()[0], expression);
        }

        IQueryable<S> IQueryProvider.CreateQuery<S>(System.Linq.Expressions.Expression expression)
        {
            if (expression == null)
            {
                throw LinqError.ArgumentNull("expression");
            }
            if (!typeof(IQueryable<S>).IsAssignableFrom(expression.Type))
            {
                throw LinqError.ArgumentNotValid("expression");
            }
            return new EnumerableQuery<S>(expression);
        }

        S IQueryProvider.Execute<S>(System.Linq.Expressions.Expression expression)
        {
            if (expression == null)
            {
                throw LinqError.ArgumentNull("expression");
            }
            if (!typeof(S).IsAssignableFrom(expression.Type))
            {
                throw LinqError.ArgumentNotValid("expression");
            }
            return new EnumerableExecutor<S>(expression).Execute();
        }

        object IQueryProvider.Execute(System.Linq.Expressions.Expression expression)
        {
            if (expression == null)
            {
                throw LinqError.ArgumentNull("expression");
            }
            Type type = typeof(EnumerableExecutor<>).MakeGenericType(new Type[] { expression.Type });
            return EnumerableExecutor.Create(expression).ExecuteBoxed();
        }

        public override string ToString()
        {
            ConstantExpression expression = this.expression as ConstantExpression;
            if ((expression != null) && (expression.Value == this))
            {
                if (this.enumerable != null)
                {
                    return this.enumerable.ToString();
                }
                return "null";
            }
            return this.expression.ToString();
        }

        internal override IEnumerable Enumerable
        {
            get
            {
                return this.enumerable;
            }
        }

        internal override System.Linq.Expressions.Expression Expression
        {
            get
            {
                return this.expression;
            }
        }

        Type IQueryable.ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get
            {
                return this.expression;
            }
        }

        IQueryProvider IQueryable.Provider
        {
            get
            {
                return this;
            }
        }
    }
}

