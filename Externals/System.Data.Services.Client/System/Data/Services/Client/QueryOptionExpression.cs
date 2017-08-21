namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;

    internal abstract class QueryOptionExpression : Expression
    {
        internal QueryOptionExpression(ExpressionType nodeType, Type type) : base(nodeType, type)
        {
        }

        internal virtual QueryOptionExpression ComposeMultipleSpecification(QueryOptionExpression previous)
        {
            Debug.Assert(previous != null, "other != null");
            Debug.Assert(previous.GetType() == base.GetType(), "other.GetType == this.GetType() -- otherwise it's not the same specification");
            return this;
        }
    }
}

