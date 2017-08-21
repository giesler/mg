namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;

    [DebuggerDisplay("SkipQueryOptionExpression {SkipAmount}")]
    internal class SkipQueryOptionExpression : QueryOptionExpression
    {
        private ConstantExpression skipAmount;

        internal SkipQueryOptionExpression(Type type, ConstantExpression skipAmount) : base((ExpressionType) 0x2714, type)
        {
            this.skipAmount = skipAmount;
        }

        internal override QueryOptionExpression ComposeMultipleSpecification(QueryOptionExpression previous)
        {
            Debug.Assert(previous != null, "other != null");
            Debug.Assert(previous.GetType() == base.GetType(), "other.GetType == this.GetType() -- otherwise it's not the same specification");
            Debug.Assert(this.skipAmount != null, "this.skipAmount != null");
            Debug.Assert(this.skipAmount.Type == typeof(int), "this.skipAmount.Type == typeof(int) -- otherwise it wouldn't have matched the Enumerable.Skip(source, int count) signature");
            int num = (int) this.skipAmount.Value;
            int num2 = (int) ((SkipQueryOptionExpression) previous).skipAmount.Value;
            return new SkipQueryOptionExpression(base.Type, Expression.Constant(num + num2, typeof(int)));
        }

        internal ConstantExpression SkipAmount
        {
            get
            {
                return this.skipAmount;
            }
        }
    }
}

