namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    internal class MemberAssignmentAnalysis : ALinqExpressionVisitor
    {
        internal static readonly Expression[] EmptyExpressionArray = new Expression[0];
        private readonly Expression entity;
        private Exception incompatibleAssignmentsException;
        private bool multiplePathsFound;
        private List<Expression> pathFromEntity;

        private MemberAssignmentAnalysis(Expression entity)
        {
            Debug.Assert(entity != null, "entity != null");
            this.entity = entity;
            this.pathFromEntity = new List<Expression>();
        }

        internal static MemberAssignmentAnalysis Analyze(Expression entityInScope, Expression assignmentExpression)
        {
            Debug.Assert(entityInScope != null, "entityInScope != null");
            Debug.Assert(assignmentExpression != null, "assignmentExpression != null");
            MemberAssignmentAnalysis analysis = new MemberAssignmentAnalysis(entityInScope);
            analysis.Visit(assignmentExpression);
            return analysis;
        }

        internal Exception CheckCompatibleAssignments(Type targetType, ref MemberAssignmentAnalysis previous)
        {
            if (previous == null)
            {
                previous = this;
                return null;
            }
            Expression[] expressionsToTargetEntity = previous.GetExpressionsToTargetEntity();
            Expression[] candidate = this.GetExpressionsToTargetEntity();
            return CheckCompatibleAssignments(targetType, expressionsToTargetEntity, candidate);
        }

        private static Exception CheckCompatibleAssignments(Type targetType, Expression[] previous, Expression[] candidate)
        {
            Debug.Assert(targetType != null, "targetType != null");
            Debug.Assert(previous != null, "previous != null");
            Debug.Assert(candidate != null, "candidate != null");
            if (previous.Length != candidate.Length)
            {
                throw CheckCompatibleAssignmentsFail(targetType, previous, candidate);
            }
            for (int i = 0; i < previous.Length; i++)
            {
                Expression expression = previous[i];
                Expression expression2 = candidate[i];
                if (expression.NodeType != expression2.NodeType)
                {
                    throw CheckCompatibleAssignmentsFail(targetType, previous, candidate);
                }
                if (expression != expression2)
                {
                    if (expression.NodeType != ExpressionType.MemberAccess)
                    {
                        return CheckCompatibleAssignmentsFail(targetType, previous, candidate);
                    }
                    if (((MemberExpression) expression).Member.Name != ((MemberExpression) expression2).Member.Name)
                    {
                        return CheckCompatibleAssignmentsFail(targetType, previous, candidate);
                    }
                }
            }
            return null;
        }

        private static Exception CheckCompatibleAssignmentsFail(Type targetType, Expression[] previous, Expression[] candidate)
        {
            Debug.Assert(targetType != null, "targetType != null");
            Debug.Assert(previous != null, "previous != null");
            Debug.Assert(candidate != null, "candidate != null");
            return new NotSupportedException(Strings.ALinq_ProjectionMemberAssignmentMismatch(targetType.FullName, previous.LastOrDefault<Expression>(), candidate.LastOrDefault<Expression>()));
        }

        internal Expression[] GetExpressionsBeyondTargetEntity()
        {
            Debug.Assert(!this.multiplePathsFound, "this.multiplePathsFound -- otherwise GetExpressionsToTargetEntity won't return reliable (consistent) results");
            if (this.pathFromEntity.Count <= 1)
            {
                return EmptyExpressionArray;
            }
            return new Expression[] { this.pathFromEntity[this.pathFromEntity.Count - 1] };
        }

        internal Expression[] GetExpressionsToTargetEntity()
        {
            Debug.Assert(!this.multiplePathsFound, "this.multiplePathsFound -- otherwise GetExpressionsToTargetEntity won't return reliable (consistent) results");
            if (this.pathFromEntity.Count <= 1)
            {
                return EmptyExpressionArray;
            }
            Expression[] expressionArray = new Expression[this.pathFromEntity.Count - 1];
            for (int i = 0; i < expressionArray.Length; i++)
            {
                expressionArray[i] = this.pathFromEntity[i];
            }
            return expressionArray;
        }

        internal override Expression Visit(Expression expression)
        {
            if (this.multiplePathsFound || (this.incompatibleAssignmentsException != null))
            {
                return expression;
            }
            return base.Visit(expression);
        }

        internal override Expression VisitConditional(ConditionalExpression c)
        {
            ResourceBinder.PatternRules.MatchNullCheckResult result = ResourceBinder.PatternRules.MatchNullCheck(this.entity, c);
            if (result.Match)
            {
                this.Visit(result.AssignExpression);
                return c;
            }
            return base.VisitConditional(c);
        }

        internal override Expression VisitMemberAccess(MemberExpression m)
        {
            Expression expression = base.VisitMemberAccess(m);
            if (this.pathFromEntity.Contains(m.Expression))
            {
                this.pathFromEntity.Add(m);
            }
            return expression;
        }

        internal override Expression VisitMemberInit(MemberInitExpression init)
        {
            Expression expression = init;
            MemberAssignmentAnalysis previous = null;
            foreach (MemberBinding binding in init.Bindings)
            {
                MemberAssignment assignment = binding as MemberAssignment;
                if (assignment != null)
                {
                    MemberAssignmentAnalysis analysis2 = Analyze(this.entity, assignment.Expression);
                    if (analysis2.MultiplePathsFound)
                    {
                        this.multiplePathsFound = true;
                        return expression;
                    }
                    Exception exception = analysis2.CheckCompatibleAssignments(init.Type, ref previous);
                    if (exception != null)
                    {
                        this.incompatibleAssignmentsException = exception;
                        return expression;
                    }
                    if (this.pathFromEntity.Count == 0)
                    {
                        this.pathFromEntity.AddRange(analysis2.GetExpressionsToTargetEntity());
                    }
                }
            }
            return expression;
        }

        internal override Expression VisitMethodCall(MethodCallExpression call)
        {
            if (ReflectionUtil.IsSequenceMethod(call.Method, SequenceMethod.Select))
            {
                this.Visit(call.Arguments[0]);
                return call;
            }
            return base.VisitMethodCall(call);
        }

        internal override Expression VisitParameter(ParameterExpression p)
        {
            if (p == this.entity)
            {
                if (this.pathFromEntity.Count != 0)
                {
                    this.multiplePathsFound = true;
                    return p;
                }
                this.pathFromEntity.Add(p);
            }
            return p;
        }

        internal Exception IncompatibleAssignmentsException
        {
            get
            {
                return this.incompatibleAssignmentsException;
            }
        }

        internal bool MultiplePathsFound
        {
            get
            {
                return this.multiplePathsFound;
            }
        }
    }
}

