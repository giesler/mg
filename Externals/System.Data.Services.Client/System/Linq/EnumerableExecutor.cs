namespace System.Linq
{
    using System;
    using System.Linq.Expressions;

    public abstract class EnumerableExecutor
    {
        protected EnumerableExecutor()
        {
        }

        internal static EnumerableExecutor Create(Expression expression)
        {
            return (EnumerableExecutor) Activator.CreateInstance(typeof(EnumerableExecutor<>).MakeGenericType(new Type[] { expression.Type }), new object[] { expression });
        }

        internal abstract object ExecuteBoxed();
    }
}

