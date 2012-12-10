namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;

    [DebuggerDisplay("InputReferenceExpression -> {Type}")]
    internal sealed class InputReferenceExpression : Expression
    {
        private ResourceExpression target;

        internal InputReferenceExpression(ResourceExpression target) : base((ExpressionType) 0x2717, target.ResourceType)
        {
            Debug.Assert(target != null, "Target resource set cannot be null");
            this.target = target;
        }

        internal void OverrideTarget(ResourceSetExpression newTarget)
        {
            Debug.Assert(newTarget != null, "Resource set cannot be null");
            Debug.Assert(newTarget.ResourceType.Equals(base.Type), "Cannot reference a resource set with a different resource type");
            this.target = newTarget;
        }

        internal ResourceExpression Target
        {
            get
            {
                return this.target;
            }
        }
    }
}

