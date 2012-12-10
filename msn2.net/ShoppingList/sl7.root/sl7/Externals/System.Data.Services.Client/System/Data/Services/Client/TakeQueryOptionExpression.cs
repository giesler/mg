namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;

    [DebuggerDisplay("TakeQueryOptionExpression {TakeAmount}")]
    internal class TakeQueryOptionExpression : QueryOptionExpression
    {
        private ConstantExpression takeAmount;

        internal TakeQueryOptionExpression(Type type, ConstantExpression takeAmount) : base((ExpressionType) 0x2713, type)
        {
            this.takeAmount = takeAmount;
        }

        internal override QueryOptionExpression ComposeMultipleSpecification(QueryOptionExpression previous)
        {
            Debug.Assert(previous != null, "other != null");
            Debug.Assert(previous.GetType() == base.GetType(), "other.GetType == this.GetType() -- otherwise it's not the same specification");
            Debug.Assert(this.takeAmount != null, "this.takeAmount != null");
            Debug.Assert(this.takeAmount.Type == typeof(int), "this.takeAmount.Type == typeof(int) -- otherwise it wouldn't have matched the Enumerable.Take(source, int count) signature");
            int num = (int) this.takeAmount.Value;
            int num2 = (int) ((TakeQueryOptionExpression) previous).takeAmount.Value;
            return ((num < num2) ? this : previous);
        }

        internal ConstantExpression TakeAmount
        {
            get
            {
                return this.takeAmount;
            }
        }
    }
}

