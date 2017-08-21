namespace System.Linq
{
    using System;
    using System.Collections;
    using System.Linq.Expressions;

    public abstract class EnumerableQuery
    {
        protected EnumerableQuery()
        {
        }

        internal static IQueryable Create(Type elementType, IEnumerable sequence)
        {
            return (IQueryable) Activator.CreateInstance(typeof(EnumerableQuery<>).MakeGenericType(new Type[] { elementType }), new object[] { sequence });
        }

        internal static IQueryable Create(Type elementType, System.Linq.Expressions.Expression expression)
        {
            return (IQueryable) Activator.CreateInstance(typeof(EnumerableQuery<>).MakeGenericType(new Type[] { elementType }), new object[] { expression });
        }

        internal abstract IEnumerable Enumerable { get; }

        internal abstract System.Linq.Expressions.Expression Expression { get; }
    }
}

