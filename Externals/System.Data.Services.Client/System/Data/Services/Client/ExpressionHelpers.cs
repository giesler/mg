namespace System.Data.Services.Client
{
    using IQToolkit;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    internal static class ExpressionHelpers
    {
        private static MethodInfo lambdaFunc;

        internal static LambdaExpression CreateLambda(Expression body, params ParameterExpression[] parameters)
        {
            return CreateLambda(InferDelegateType(body, parameters), body, parameters);
        }

        internal static LambdaExpression CreateLambda(Type delegateType, Expression body, params ParameterExpression[] parameters)
        {
            ParameterExpression[] expressionArray = new ParameterExpression[] { Expression.Parameter(typeof(Expression), "body"), Expression.Parameter(typeof(ParameterExpression[]), "parameters") };
            Expression<Func<Expression, ParameterExpression[], LambdaExpression>> function = Expression.Lambda<Func<Expression, ParameterExpression[], LambdaExpression>>(Expression.Call(GetLambdaFactoryMethod(delegateType), expressionArray), expressionArray);
            return ExpressionEvaluator.CreateDelegate<Func<Expression, ParameterExpression[], LambdaExpression>>(function).Invoke(body, parameters);
        }

        private static MethodInfo GetLambdaFactoryMethod(Type delegateType)
        {
            if (lambdaFunc == null)
            {
                lambdaFunc = new Func<Expression, ParameterExpression[], Expression<Action>>(Expression.Lambda<Action>).Method.GetGenericMethodDefinition();
            }
            return lambdaFunc.MakeGenericMethod(new Type[] { delegateType });

        }

        private static Type InferDelegateType(Expression body, params ParameterExpression[] parameters)
        {
            bool flag = body.Type == typeof(void);
            int index = (parameters == null) ? 0 : parameters.Length;
            Type[] typeArgs = new Type[index + (flag ? 0 : 1)];
            for (int i = 0; i < index; i++)
            {
                typeArgs[i] = parameters[i].Type;
            }
            if (flag)
            {
                return Expression.GetActionType(typeArgs);
            }
            typeArgs[index] = body.Type;
            return Expression.GetFuncType(typeArgs);
        }
    }
}

