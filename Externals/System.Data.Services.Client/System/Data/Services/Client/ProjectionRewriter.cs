namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    internal class ProjectionRewriter : ALinqExpressionVisitor
    {
        private readonly ParameterExpression newLambdaParameter;
        private ParameterExpression oldLambdaParameter;
        private bool sucessfulRebind;

        private ProjectionRewriter(Type proposedParameterType)
        {
            Debug.Assert(proposedParameterType != null, "proposedParameterType != null");
            this.newLambdaParameter = Expression.Parameter(proposedParameterType, "it");
        }

        internal LambdaExpression Rebind(LambdaExpression lambda)
        {
            this.sucessfulRebind = true;
            this.oldLambdaParameter = lambda.Parameters[0];
            Expression body = this.Visit(lambda.Body);
            if (!this.sucessfulRebind)
            {
                throw new NotSupportedException(Strings.ALinq_CanOnlyProjectTheLeaf);
            }
            return ExpressionHelpers.CreateLambda(typeof(Func<,>).MakeGenericType(new Type[] { this.newLambdaParameter.Type, lambda.Body.Type }), body, new ParameterExpression[] { this.newLambdaParameter });
        }

        internal static LambdaExpression TryToRewrite(LambdaExpression le, Type proposedParameterType)
        {
            LambdaExpression result;
            if (!ResourceBinder.PatternRules.MatchSingleArgumentLambda(le, out le) || ClientType.CheckElementTypeIsEntity(le.Parameters[0].Type) || !(le.Parameters[0].Type.GetProperties().Any(p => p.PropertyType == proposedParameterType)))
            {
                result = le;
            }
            else
            {
                ProjectionRewriter rewriter = new ProjectionRewriter(proposedParameterType);
                result = rewriter.Rebind(le);
            }

            return result;
        }

        internal override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression == this.oldLambdaParameter)
            {
                if (m.Type == this.newLambdaParameter.Type)
                {
                    return this.newLambdaParameter;
                }
                this.sucessfulRebind = false;
            }
            return base.VisitMemberAccess(m);
        }
    }
}

