namespace System.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class EnumerableExecutor<T> : EnumerableExecutor
    {
        private Expression expression;
        private Func<T> func;

        public EnumerableExecutor(Expression expression)
        {
            this.expression = expression;
        }

        internal T Execute()
        {
            if (this.func == null)
            {
                EnumerableRewriter rewriter = new EnumerableRewriter();
                // TODO: Fix compile
                //this.func = Expression.Lambda<Func<T>>(rewriter.Visit(this.expression), (IEnumerable<ParameterExpression>) null).Compile();
            }
            return this.func.Invoke();
        }

        internal override object ExecuteBoxed()
        {
            return this.Execute();
        }
    }
}

