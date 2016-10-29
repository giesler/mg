namespace IQToolkit
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class ExpressionEvaluator
    {
        public static object Eval(Expression expression)
        {
            LambdaExpression lambda = expression as LambdaExpression;
            if (lambda != null && lambda.Parameters.Count == 0)
            {
                expression = lambda.Body;
            }
            else
            {
                throw new InvalidOperationException("Wrong number of arguments specified");
            }
            var result = EvaluatorBuilder.Build(null, null, expression);
            return result.EvalBoxed(new EvaluatorState(null, null));
        }

        public static object Eval(LambdaExpression function, params object[] args)
        {
            if (function.Parameters.Count != args.Length)
            {
                throw new InvalidOperationException("Wrong number of arguments specified");
            }
            var result = EvaluatorBuilder.Build(null, function.Parameters, function.Body);
            return result.EvalBoxed(new EvaluatorState(null, args));
        }

        public static Delegate CreateDelegate(LambdaExpression function)
        {
            var result = EvaluatorBuilder.Build(null, function.Parameters, function.Body);
            return CreateDelegate(function.Type, null, function.Parameters.Count, result);
        }

        public static D CreateDelegate<D>(Expression<D> function)
        {
            return (D)(object)CreateDelegate((LambdaExpression)function);
        }

        public static Delegate CreateDelegate(Type delegateType, EvaluatorState outer, int nArgs, Evaluator evaluator)
        {
            MethodInfo miInvoke = delegateType.GetMethod("Invoke");
            var host = new EvaluatorHost(outer, nArgs, evaluator);
            return StrongDelegate.CreateDelegate(delegateType, host.Eval);
        }

        public class EvaluatorHost
        {
            EvaluatorState outer;
            int nArgs;
            Evaluator evaluator;


            public EvaluatorHost(EvaluatorState outer, int nArgs, Evaluator evaluator)
            {
                this.outer = outer;
                this.nArgs = nArgs;
                this.evaluator = evaluator;
            }

            public object Eval(object[] args)
            {
                int len = (args != null ? args.Length : 0);
                if (len != this.nArgs)
                {
                    object[] tmp = new object[this.nArgs];
                    if (args != null)
                        Array.Copy(args, tmp, len);
                    args = tmp;
                }
                return this.evaluator.EvalBoxed(new EvaluatorState(this.outer, args));
            }
        }

        public class EvaluatorState
        {
            EvaluatorState outer;
            object[] values;
            int start;

            public EvaluatorState(EvaluatorState outer, object[] values)
            {
                this.outer = outer;
                this.values = values;
                this.start = (outer != null ? outer.start + (outer.values != null ? outer.values.Length : 0) : 0);
            }

            public object GetBoxedValue(int index)
            {
                var state = this;
                while (index < state.start)
                {
                    state = state.outer;
                }
                return state.values[index - state.start];
            }

            public T GetValue<T>(int index)
            {
                var state = this;
                while (index < state.start)
                {
                    state = state.outer;
                }
                return (T)state.values[index - state.start];
            }

            public void SetValue<T>(int index, T value)
            {
                var state = this;
                while (index < state.start)
                {
                    state = state.outer;
                }
                state.values[index - state.start] = value;
            }
        }

        private class EvaluatorBuilder
        {
            EvaluatorBuilder outer;
            ReadOnlyCollection<ParameterExpression> parameters;
            int count = -1;

            private EvaluatorBuilder(EvaluatorBuilder outer, List<ParameterExpression> parameters)
            {
                this.outer = outer;
                this.parameters = parameters.ToReadOnly();
            }

            internal static Evaluator Build(EvaluatorBuilder outer, IEnumerable<ParameterExpression> parameters, Expression expression)
            {
                var list = parameters.ToList();
                list.AddRange(VariableFinder.Find(expression));
                return new EvaluatorBuilder(outer, list).Build(expression);
            }

            private int Count
            {
                get
                {
                    if (this.count == -1)
                    {
                        this.count = (this.outer != null ? this.outer.Count : 0) + (this.parameters != null ? this.parameters.Count : 0);
                    }
                    return this.count;
                }
            }

            // treat invocations with nested lambda's as nested expressions w/ variable declarations
            class VariableFinder : OldExpressionVisitor
            {
                List<ParameterExpression> variables = new List<ParameterExpression>();

                internal static List<ParameterExpression> Find(Expression expression)
                {
                    var finder = new VariableFinder();
                    finder.Visit(expression);
                    return finder.variables;
                }

                internal override Expression VisitInvocation(InvocationExpression iv)
                {
                    LambdaExpression lambda = iv.Expression as LambdaExpression;
                    if (lambda != null)
                    {
                        this.variables.AddRange(lambda.Parameters);
                    }
                    return base.VisitInvocation(iv);
                }
            }

            private Evaluator Build(Expression exp)
            {
                if (exp == null)
                    return null;
                switch (exp.NodeType)
                {
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.ArrayLength:
                    case ExpressionType.Quote:
                    case ExpressionType.TypeAs:
                    case ExpressionType.UnaryPlus:
                        return Unary((UnaryExpression)exp);
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Coalesce:
                    case ExpressionType.ArrayIndex:
                    case ExpressionType.RightShift:
                    case ExpressionType.LeftShift:
                    case ExpressionType.ExclusiveOr:
                    case ExpressionType.Power:
                        return Binary((BinaryExpression)exp);
                    case ExpressionType.Constant:
                        return Constant((ConstantExpression)exp);
                    case ExpressionType.Parameter:
                        return Parameter((ParameterExpression)exp);
                    case ExpressionType.MemberAccess:
                        return MemberAccess((MemberExpression)exp);
                    case ExpressionType.Call:
                        return Call((MethodCallExpression)exp);
                    case ExpressionType.Conditional:
                        return Conditional((ConditionalExpression)exp);
                    case ExpressionType.TypeIs:
                        return TypeIs((TypeBinaryExpression)exp);
                    case ExpressionType.New:
                        return New((NewExpression)exp);
                    case ExpressionType.Lambda:
                        return Lambda((LambdaExpression)exp);
                    case ExpressionType.NewArrayInit:
                        return NewArrayInit((NewArrayExpression)exp);
                    case ExpressionType.NewArrayBounds:
                        return NewArrayBounds((NewArrayExpression)exp);
                    case ExpressionType.Invoke:
                        return Invoke((InvocationExpression)exp);
                    case ExpressionType.MemberInit:
                        return MemberInit((MemberInitExpression)exp);
                    case ExpressionType.ListInit:
                        return ListInit((ListInitExpression)exp);
                    default:
                        throw new InvalidOperationException();
                }
            }

            private Evaluator Build(Type resultType, Expression expression)
            {
                if (expression.Type != resultType)
                {
                    expression = Expression.Convert(expression, resultType);
                }
                return Build(expression);
            }

            private Evaluator Unary(UnaryExpression u)
            {
                var operand = Build(u.Operand);

                bool isSourceTypeNullable = IsNullable(u.Operand.Type);
                bool isTargetTypeNullable = IsNullable(u.Type);
                Type sourceType = GetNonNullType(u.Operand.Type);
                Type targetType = GetNonNullType(u.Type);

                if (u.Method != null)
                {
                    return this.GetUnaryOperator(u, sourceType, targetType, u.Method, operand);
                }

                switch (u.NodeType)
                {
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                        {
                            MethodInfo mi = FindBestMethod(Operators.GetOperatorMethods(u.NodeType.ToString()), new Type[] { sourceType }, targetType);
                            return this.GetUnaryOperator(u, sourceType, targetType, mi, operand);
                        }
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        {
                            if (u.Type == u.Operand.Type)
                            {
                                // no conversion necessary
                                return operand;
                            }
                            else if (!sourceType.IsValueType || !targetType.IsValueType)
                            {
                                // reference or boxing conversion
                                return (Evaluator)Activator.CreateInstance(
                                    typeof(Convert<,>).MakeGenericType(u.Operand.Type, u.Type),
                                    new object[] { operand }
                                    );
                            }
                            else if (sourceType == targetType)
                            {
                                if (isSourceTypeNullable && !isTargetTypeNullable)
                                {
                                    return (Evaluator)Activator.CreateInstance(
                                        typeof(ConvertNtoNN<>).MakeGenericType(sourceType),
                                        new object[] { operand }
                                        );
                                }
                                else
                                {
                                    System.Diagnostics.Debug.Assert(!isSourceTypeNullable && isTargetTypeNullable);
                                    return (Evaluator)Activator.CreateInstance(
                                        typeof(ConvertNNtoN<>).MakeGenericType(sourceType),
                                        new object[] { operand }
                                        );
                                }
                            }
                            else
                            {
                                MethodInfo mi = FindBestMethod(Operators.GetOperatorMethods(u.NodeType + "To" + targetType.Name), new Type[] { sourceType }, targetType);
                                Delegate fn = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(sourceType, targetType), null, mi);
                                if (!isSourceTypeNullable && !isTargetTypeNullable)
                                {
                                    return (Evaluator)Activator.CreateInstance(
                                        typeof(Convert<,>).MakeGenericType(sourceType, targetType),
                                        new object[] { operand, fn }
                                        );
                                }
                                else if (isSourceTypeNullable && !isTargetTypeNullable)
                                {
                                    return (Evaluator)Activator.CreateInstance(
                                        typeof(ConvertNtoNN<,>).MakeGenericType(sourceType, targetType),
                                        new object[] { operand, fn }
                                        );
                                }
                                else if (!isSourceTypeNullable && isTargetTypeNullable)
                                {
                                    return (Evaluator)Activator.CreateInstance(
                                        typeof(ConvertNNtoN<,>).MakeGenericType(sourceType, targetType),
                                        new object[] { operand, fn }
                                        );
                                }
                                else
                                {
                                    System.Diagnostics.Debug.Assert(isSourceTypeNullable && isTargetTypeNullable);
                                    return (Evaluator)Activator.CreateInstance(
                                        typeof(ConvertNtoN<,>).MakeGenericType(sourceType, targetType),
                                        new object[] { operand, fn }
                                        );
                                }
                            }
                        }
                    case ExpressionType.TypeAs:
                        return (Evaluator)Activator.CreateInstance(
                            typeof(TypeAsEvaluator<,>).MakeGenericType(u.Operand.Type, u.Type),
                            new object[] { operand }
                            );
                    case ExpressionType.UnaryPlus:
                        return operand;
                    case ExpressionType.ArrayLength:
                        return (Evaluator)Activator.CreateInstance(
                            typeof(ArrayLengthEvaluator<>).MakeGenericType(u.Operand.Type.GetElementType()),
                            new object[] { operand }
                            );
                    case ExpressionType.Quote:
                        return Quote(u);
                    default:
                        throw new InvalidOperationException();
                }
            }

            private Evaluator GetUnaryOperator(UnaryExpression u, Type sourceType, Type targetType, MethodInfo method, Evaluator operand)
            {
                Delegate opFunc = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(sourceType, targetType), null, method);
                if (u.IsLiftedToNull)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(LiftToNullUnaryEvaluator<,>).MakeGenericType(sourceType, targetType),
                        new object[] { operand, opFunc }
                        );
                }
                else if (u.IsLifted)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(LiftToFalseUnaryEvaluator<>).MakeGenericType(sourceType),
                        new object[] { operand, opFunc }
                        );
                }
                else
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(UnaryEvaluator<,>).MakeGenericType(sourceType, targetType),
                        new object[] { operand, opFunc }
                        );
                }
            }

            private Evaluator Binary(BinaryExpression b)
            {
                var opLeft = Build(b.Left);
                var opRight = Build(b.Right);

                Type sourceType = GetNonNullType(b.Left.Type);
                Type targetType = GetNonNullType(b.Type);

                if (b.Method != null)
                {
                    return GetBinaryOperator(b, sourceType, targetType, b.Method, opLeft, opRight);
                }

                switch (b.NodeType)
                {
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.And:
                    case ExpressionType.Or:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.ExclusiveOr:
                    case ExpressionType.RightShift:
                    case ExpressionType.LeftShift:
                    case ExpressionType.Power:
                        {
                            MethodInfo mi = this.FindBestMethod(
                                Operators.GetOperatorMethods(b.NodeType.ToString()),
                                new Type[] { sourceType, sourceType }, targetType
                                );
                            System.Diagnostics.Debug.Assert(mi != null);
                            return GetBinaryOperator(b, sourceType, targetType, mi, opLeft, opRight);
                        }
                    case ExpressionType.AndAlso:
                        if (b.IsLiftedToNull)
                        {
                            return new LiftToNullAndAlsoEvaluator((Evaluator<bool?>)opLeft, (Evaluator<bool?>)opRight);
                        }
                        else if (b.IsLifted)
                        {
                            return new LiftToFalseAndAlsoEvaluator((Evaluator<bool?>)opLeft, (Evaluator<bool?>)opRight);
                        }
                        else
                        {
                            return new AndAlsoEvaluator((Evaluator<bool>)opLeft, (Evaluator<bool>)opRight);
                        }
                    case ExpressionType.OrElse:
                        if (b.IsLiftedToNull)
                        {
                            return new LiftToNullOrElseEvaluator((Evaluator<bool?>)opLeft, (Evaluator<bool?>)opRight);
                        }
                        else if (b.IsLifted)
                        {
                            return new LiftToFalseOrElseEvaluator((Evaluator<bool?>)opLeft, (Evaluator<bool?>)opRight);
                        }
                        else
                        {
                            return new OrElseEvaluator((Evaluator<bool>)opLeft, (Evaluator<bool>)opRight);
                        }
                    case ExpressionType.Coalesce:
                        Type rightType = GetNonNullType(b.Right.Type);
                        if (b.Conversion != null)
                        {
                            LambdaExpression conv = b.Conversion;
                            if (conv.Body.Type != b.Type)
                            {
                                conv = Expression.Lambda(Expression.Convert(conv.Body, b.Type), conv.Parameters.ToArray());
                            }
                            if (conv.Parameters[0].Type == b.Left.Type || conv.Parameters[0].Type == sourceType)
                            {
                                if (conv.Parameters[0].Type == sourceType)
                                {
                                    var p = Expression.Parameter(b.Left.Type, "left");
                                    conv = Expression.Lambda(Expression.Invoke(conv, Expression.Convert(p, sourceType)), p);
                                }
                                Delegate fnConv = ExpressionEvaluator.CreateDelegate(conv);
                                return (Evaluator)Activator.CreateInstance(
                                    typeof(CoalesceREvaluator<,>).MakeGenericType(b.Left.Type, b.Right.Type),
                                    new object[] { opLeft, opRight, fnConv }
                                    );
                            }
                            else if (conv.Parameters[0].Type == b.Right.Type || conv.Parameters[0].Type == rightType)
                            {
                                if (conv.Parameters[0].Type == rightType)
                                {
                                    var p = Expression.Parameter(b.Right.Type, "right");
                                    conv = Expression.Lambda(Expression.Invoke(conv, Expression.Convert(p, rightType)), p);
                                }
                                Delegate fnConv = ExpressionEvaluator.CreateDelegate(conv);
                                return (Evaluator)Activator.CreateInstance(
                                    typeof(CoalesceLEvaluator<,>).MakeGenericType(b.Left.Type, b.Right.Type),
                                    new object[] { opLeft, opRight, fnConv }
                                    );
                            }
                        }
                        else if (b.Type == b.Right.Type)
                        {
                            var p = Expression.Parameter(b.Left.Type, "left");
                            var lambda = Expression.Lambda(Expression.Convert(p, b.Type), p);
                            Delegate fnConv = ExpressionEvaluator.CreateDelegate(lambda);
                            return (Evaluator)Activator.CreateInstance(
                                typeof(CoalesceREvaluator<,>).MakeGenericType(b.Left.Type, b.Right.Type),
                                new object[] { opLeft, opRight, fnConv }
                                );
                        }
                        else if (b.Type == b.Left.Type)
                        {
                            var p = Expression.Parameter(b.Right.Type, "right");
                            var lambda = Expression.Lambda(Expression.Convert(p, b.Type), p);
                            Delegate fnConv = ExpressionEvaluator.CreateDelegate(lambda);
                            return (Evaluator)Activator.CreateInstance(
                                typeof(CoalesceLEvaluator<,>).MakeGenericType(b.Left.Type, b.Right.Type),
                                new object[] { opLeft, opRight, fnConv }
                                );
                        }
                        throw new InvalidOperationException("Unhandled Coalesce transaltion");
                    case ExpressionType.ArrayIndex:
                        return (Evaluator)Activator.CreateInstance(
                            typeof(ArrayIndexEvaluator<>).MakeGenericType(b.Left.Type.GetElementType()),
                            new object[] { opLeft, opRight }
                            );
                    default:
                        throw new InvalidOperationException();
                }
            }

            public Evaluator GetBinaryOperator(BinaryExpression b, Type sourceType, Type targetType, MethodInfo method, Evaluator opLeft, Evaluator opRight)
            {
                Delegate opFunc = Delegate.CreateDelegate(typeof(Func<,,>).MakeGenericType(sourceType, sourceType, targetType), null, method);
                if (b.IsLiftedToNull)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(LiftToNullBinaryEvaluator<,>).MakeGenericType(sourceType, targetType),
                        new object[] { opLeft, opRight, opFunc }
                        );
                }
                else if (b.IsLifted)
                {
                    if (b.NodeType == ExpressionType.Equal)
                    {
                        return (Evaluator)Activator.CreateInstance(
                            typeof(LiftToEqualBinaryEvaluator<>).MakeGenericType(sourceType),
                            new object[] { opLeft, opRight, opFunc }
                            );
                    }
                    else if (b.NodeType == ExpressionType.NotEqual)
                    {
                        return (Evaluator)Activator.CreateInstance(
                            typeof(LiftToNotEqualBinaryEvaluator<>).MakeGenericType(sourceType),
                            new object[] { opLeft, opRight, opFunc }
                            );
                    }
                    else
                    {
                        return (Evaluator)Activator.CreateInstance(
                            typeof(LiftToFalseBinaryEvaluator<>).MakeGenericType(sourceType),
                            new object[] { opLeft, opRight, opFunc }
                            );
                    }
                }
                else
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(BinaryEvaluator<,>).MakeGenericType(sourceType, targetType),
                        new object[] { opLeft, opRight, opFunc }
                        );
                }
            }

            public Evaluator Constant(ConstantExpression c)
            {
                return (Evaluator)Activator.CreateInstance(
                    typeof(ConstantEvaluator<>).MakeGenericType(c.Type),
                    new object[] { c.Value }
                    );
            }

            public Evaluator Parameter(ParameterExpression p)
            {
                int index = this.FindParameterIndex(p);
                return (Evaluator)Activator.CreateInstance(
                    typeof(ParameterEvaluator<>).MakeGenericType(p.Type),
                    new object[] { index }
                    );
            }

            private int FindParameterIndex(ParameterExpression p)
            {
                for (var builder = this; builder != null; builder = builder.outer)
                {
                    if (this.parameters != null)
                    {
                        for (int i = 0, n = this.parameters.Count; i < n; i++)
                        {
                            if (this.parameters[i] == p)
                            {
                                return i + (this.outer != null ? this.outer.Count : 0);
                            }
                        }
                    }
                }
                throw new InvalidOperationException(string.Format("Parameter '{0}' not in scope", p.Name));
            }

            public Evaluator Call(MethodCallExpression mc)
            {
                var opInst = (mc.Object != null) ? Build(mc.Object) : null;
                var opArgs = mc.Arguments.Select(a => Build(a)).ToArray();
                return this.GetMethodCallOperator(mc.Method, opInst, opArgs);
            }

            public Evaluator GetMethodCallOperator(MethodInfo method, Evaluator opInst, Evaluator[] opArgs)
            {
                var parameters = method.GetParameters();
                if (parameters.Any(p => p.ParameterType.IsByRef))
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(MethodCallWithRefArgsEvaluator<>).MakeGenericType(method.ReturnType),
                        new object[] { method, opInst, opArgs }
                        );
                }
                else if (method.IsStatic && opArgs.Length == 1)
                {
                    var types = new Type[] { parameters[0].ParameterType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,>).MakeGenericType(types),
                        new object[] { opArgs[0], fn }
                        );
                }
                else if (!method.IsStatic && opArgs.Length == 0)
                {
                    var types = new Type[] { opInst.ReturnType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,>).MakeGenericType(types),
                        new object[] { opInst, fn }
                        );
                }
                else if (method.IsStatic && opArgs.Length == 2)
                {
                    var types = new Type[] { parameters[0].ParameterType, parameters[1].ParameterType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,,>).MakeGenericType(types),
                        new object[] { opArgs[0], opArgs[1], fn }
                        );
                }
                else if (!method.IsStatic && opArgs.Length == 1)
                {
                    var types = new Type[] { opInst.ReturnType, parameters[0].ParameterType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,,>).MakeGenericType(types),
                        new object[] { opInst, opArgs[0], fn }
                        );
                }
                else if (method.IsStatic && opArgs.Length == 3)
                {
                    var types = new Type[] { parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,,,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,,,>).MakeGenericType(types),
                        new object[] { opArgs[0], opArgs[1], opArgs[2], fn }
                        );
                }
                else if (!method.IsStatic && opArgs.Length == 2)
                {
                    var types = new Type[] { opInst.ReturnType, parameters[0].ParameterType, parameters[1].ParameterType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,,,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,,,>).MakeGenericType(types),
                        new object[] { opInst, opArgs[0], opArgs[1], fn }
                        );
                }
                else if (method.IsStatic && opArgs.Length == 4)
                {
                    var types = new Type[] { parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType, parameters[3].ParameterType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,,,,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,,,,>).MakeGenericType(types),
                        new object[] { opArgs[0], opArgs[1], opArgs[2], opArgs[3], fn }
                        );
                }
                else if (!method.IsStatic && opArgs.Length == 3)
                {
                    var types = new Type[] { opInst.ReturnType, parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType, method.ReturnType };
                    var fn = Delegate.CreateDelegate(typeof(Func<,,,,>).MakeGenericType(types), null, method);
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FuncEvaluator<,,,,>).MakeGenericType(types),
                        new object[] { opInst, opArgs[0], opArgs[1], opArgs[2], fn }
                        );
                }
                else
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(MethodCallEvaluator<>).MakeGenericType(method.ReturnType),
                        new object[] { method, opInst, opArgs }
                        );
                }
            }

            public Evaluator MemberAccess(MemberExpression m)
            {
                var operand = m.Expression != null ? Build(m.Expression) : null;
                FieldInfo field = m.Member as FieldInfo;
                if (field != null)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(FieldAccessEvaluator<>).MakeGenericType(field.FieldType),
                        new object[] { operand, field }
                        );
                }
                PropertyInfo property = m.Member as PropertyInfo;
                if (property != null)
                {
                    Type opType = operand != null ? operand.ReturnType : m.Member.DeclaringType;
                    return (Evaluator)Activator.CreateInstance(
                        typeof(PropertyAccessEvaluator<,>).MakeGenericType(opType, property.PropertyType),
                        new object[] { operand, property }
                        );
                }
                throw new NotSupportedException();
            }

            public Evaluator Conditional(ConditionalExpression c)
            {
                var opTest = Build(c.Test);
                var opIfTrue = Build(c.IfTrue);
                var opIfFalse = Build(c.IfFalse);
                return (Evaluator)Activator.CreateInstance(
                    typeof(ConditionalEvaluator<>).MakeGenericType(c.Type),
                    new object[] { opTest, opIfTrue, opIfFalse }
                    );
            }

            public Evaluator TypeIs(TypeBinaryExpression t)
            {
                var thing = Build(t.Expression);
                return new TypeIsEvaluator(thing, t.TypeOperand);
            }

            public Evaluator New(NewExpression n)
            {
                var opArgs = n.Arguments.Select(a => Build(a)).ToArray();
                if (opArgs.Length == 0)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(NewEvaluator<>).MakeGenericType(n.Type),
                        new object[] { n.Constructor }
                        );
                }
                else if (opArgs.Length == 1)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(NewEvaluator<,>).MakeGenericType(n.Type, n.Arguments[0].Type),
                        new object[] { n.Constructor, opArgs[0] }
                        );
                }
                else if (opArgs.Length == 2)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(NewEvaluator<,,>).MakeGenericType(n.Type, n.Arguments[0].Type, n.Arguments[1].Type),
                        new object[] { n.Constructor, opArgs[0], opArgs[1] }
                        );
                }
                else if (opArgs.Length == 3)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(NewEvaluator<,,,>).MakeGenericType(n.Type, n.Arguments[0].Type, n.Arguments[1].Type, n.Arguments[2].Type),
                        new object[] { n.Constructor, opArgs[0], opArgs[1], opArgs[2] }
                        );
                }
                else if (opArgs.Length == 4)
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(NewEvaluator<,,,,>).MakeGenericType(n.Type, n.Arguments[0].Type, n.Arguments[1].Type, n.Arguments[2].Type, n.Arguments[3].Type),
                        new object[] { n.Constructor, opArgs[0], opArgs[1], opArgs[2], opArgs[3] }
                        );
                }
                else
                {
                    return (Evaluator)Activator.CreateInstance(
                        typeof(NewEvaluatorN<>).MakeGenericType(n.Type),
                        new object[] { n.Constructor, opArgs }
                        );
                }
            }

            public Evaluator NewArrayInit(NewArrayExpression n)
            {
                Type elementType = n.Type.GetElementType();
                var initializers = n.Expressions.Select(e => Build(elementType, e)).ToArray();
                return (Evaluator)Activator.CreateInstance(
                    typeof(NewArrayInitEvaluator<>).MakeGenericType(elementType),
                    new object[] { initializers }
                    );
            }

            public Evaluator NewArrayBounds(NewArrayExpression n)
            {
                var bounds = n.Expressions.Select(e => Build(e)).ToArray();
                return (Evaluator)Activator.CreateInstance(
                    typeof(NewArrayBoundsEvaluator<>).MakeGenericType(n.Type.GetElementType()),
                    new object[] { bounds }
                    );
            }

            public Evaluator Lambda(LambdaExpression lambda)
            {
                var evaluator = EvaluatorBuilder.Build(this, lambda.Parameters, lambda.Body);
                return (Evaluator)Activator.CreateInstance(
                    typeof(LambdaEvaluator<>).MakeGenericType(lambda.Type),
                    new object[] { lambda.Parameters.Count, evaluator }
                    );
            }

            public Evaluator Invoke(InvocationExpression inv)
            {
                var lambda = inv.Expression as LambdaExpression;
                if (lambda != null)
                {
                    // assume parameters from nested lambda area already in scope (see VariableFinder above)
                    Evaluator ev = this.Build(lambda.Body);

                    // make nested let expressions...
                    for (int i = lambda.Parameters.Count - 1; i >= 0; i--)
                    {
                        var parameter = lambda.Parameters[i];
                        int index = this.FindParameterIndex(parameter);
                        var evValue = this.Build(inv.Arguments[i]);
                        ev = (Evaluator)Activator.CreateInstance(
                            typeof(LetEvaluator<,>).MakeGenericType(parameter.Type, lambda.Body.Type),
                            new object[] { index, evValue, ev }
                            );
                    }

                    return ev;
                }
                else
                {
                    var opFunction = new EvaluatorBuilder(this, null).Build(inv.Expression);
                    var opArgs = inv.Arguments.Select(a => Build(a)).ToArray();
                    return (Evaluator)Activator.CreateInstance(
                        typeof(InvokeEvaluator<>).MakeGenericType(inv.Type),
                        new object[] { opFunction, opArgs }
                        );
                }
            }

            private Evaluator Quote(UnaryExpression u)
            {
                var external = ExternalReferenceGatherer.Gather(this, u.Operand).ToDictionary(p => this.FindParameterIndex(p));
                return (Evaluator)Activator.CreateInstance(
                    typeof(QuoteEvaluator<>).MakeGenericType(u.Type),
                    new object[] { u.Operand, external }
                    );
            }

            class ExternalReferenceGatherer : OldExpressionVisitor
            {
                EvaluatorBuilder builder;
                System.Collections.Generic.HashSet<ParameterExpression> external = new System.Collections.Generic.HashSet<ParameterExpression>();

                private ExternalReferenceGatherer(EvaluatorBuilder builder)
                {
                    this.builder = builder;
                }

                static internal IEnumerable<ParameterExpression> Gather(EvaluatorBuilder builder, Expression expression)
                {
                    var visitor = new ExternalReferenceGatherer(builder);
                    visitor.Visit(expression);
                    return visitor.external;
                }

                internal override Expression VisitParameter(ParameterExpression p)
                {
                    if (!this.builder.parameters.Contains(p))
                    {
                        this.external.Add(p);
                    }
                    return base.VisitParameter(p);
                }
            }


            private Evaluator MemberInit(MemberInitExpression mini)
            {
                var evNew = Build(mini.NewExpression);
                var initializers = mini.Bindings.Select(b => MemberBinding(mini.Type, b)).ToArray();
                return (Evaluator)Activator.CreateInstance(
                    typeof(InitializerEvaluator<>).MakeGenericType(mini.Type),
                    new object[] { evNew, initializers }
                    );
            }

            private Evaluator ListInit(ListInitExpression lini)
            {
                var evNew = Build(lini.NewExpression);
                var initializers = lini.Initializers.Select(b => Element(lini.Type, b)).ToArray();
                return (Evaluator)Activator.CreateInstance(
                    typeof(InitializerEvaluator<>).MakeGenericType(lini.Type),
                    new object[] { evNew, initializers }
                    );
            }

            private Initializer MemberBinding(Type type, MemberBinding mb)
            {
                switch (mb.BindingType)
                {
                    case MemberBindingType.Assignment:
                        return MemberAssignment(type, (MemberAssignment)mb);
                    case MemberBindingType.MemberBinding:
                        return MemberMemberBinding(type, (MemberMemberBinding)mb);
                    case MemberBindingType.ListBinding:
                        return MemberListBinding(type, (MemberListBinding)mb);
                    default:
                        throw new NotImplementedException();
                }
            }

            private Initializer MemberAssignment(Type type, MemberAssignment ma)
            {
                Evaluator evaluator = Build(ma.Expression);
                if (ma.Member is FieldInfo)
                {
                    if (type.IsValueType)
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(ValueTypeFieldAssignmentInitializer<>).MakeGenericType(type),
                            new object[] { ma.Member, evaluator }
                            );
                    }
                    else
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(FieldAssignmentInitializer<>).MakeGenericType(type),
                            new object[] { ma.Member, evaluator }
                            );
                    }
                }
                else
                {
                    var property = (PropertyInfo)ma.Member;
                    if (type.IsValueType)
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(ValueTypePropertyAssignmentInitializer<,>).MakeGenericType(type, property.PropertyType),
                            new object[] { property, evaluator }
                            );
                    }
                    else
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(PropertyAssignmentInitializer<,>).MakeGenericType(type, property.PropertyType),
                            new object[] { property, evaluator }
                            );
                    }
                }
            }

            private Initializer MemberMemberBinding(Type type, MemberMemberBinding mb)
            {
                FieldInfo fi = mb.Member as FieldInfo;
                if (fi != null)
                {
                    var initializers = mb.Bindings.Select(b => MemberBinding(fi.FieldType, b)).ToArray();
                    if (type.IsValueType)
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(ValueTypeFieldMemberInitializer<,>).MakeGenericType(type, fi.FieldType),
                            new object[] { fi, initializers }
                            );
                    }
                    else
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(FieldMemberInitializer<,>).MakeGenericType(type, fi.FieldType),
                            new object[] { fi, initializers }
                            );
                    }
                }
                PropertyInfo pi = mb.Member as PropertyInfo;
                if (pi != null)
                {
                    var initializers = mb.Bindings.Select(b => MemberBinding(pi.PropertyType, b)).ToArray();
                    if (type.IsValueType)
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(ValueTypePropertyMemberInitializer<,>).MakeGenericType(type, pi.PropertyType),
                            new object[] { pi, initializers }
                            );
                    }
                    else
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(PropertyMemberInitializer<,>).MakeGenericType(type, pi.PropertyType),
                            new object[] { pi, initializers }
                            );
                    }
                }
                throw new InvalidOperationException();
            }

            private Initializer MemberListBinding(Type type, MemberListBinding mb)
            {
                FieldInfo fi = mb.Member as FieldInfo;
                if (fi != null)
                {
                    var initializers = mb.Initializers.Select(e => Element(fi.FieldType, e)).ToArray();
                    if (type.IsValueType)
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(ValueTypeFieldMemberInitializer<,>).MakeGenericType(type, fi.FieldType),
                            new object[] { fi, initializers }
                            );
                    }
                    else
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(FieldMemberInitializer<,>).MakeGenericType(type, fi.FieldType),
                            new object[] { fi, initializers }
                            );
                    }
                }
                PropertyInfo pi = mb.Member as PropertyInfo;
                if (pi != null)
                {
                    var initializers = mb.Initializers.Select(e => Element(pi.PropertyType, e)).ToArray();
                    if (type.IsValueType)
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(ValueTypePropertyMemberInitializer<,>).MakeGenericType(type, pi.PropertyType),
                            new object[] { pi, initializers }
                            );
                    }
                    else
                    {
                        return (Initializer)Activator.CreateInstance(
                            typeof(PropertyMemberInitializer<,>).MakeGenericType(type, pi.PropertyType),
                            new object[] { pi, initializers }
                            );
                    }
                }
                throw new InvalidOperationException();
            }

            private Initializer Element(Type type, ElementInit elem)
            {
                var evArgs = elem.Arguments.Select(a => Build(a)).ToArray();
                if (evArgs.Length == 1)
                {
                    var types = new Type[] { type, evArgs[0].ReturnType };
                    return (Initializer)Activator.CreateInstance(
                        typeof(ElementInitializer<,>).MakeGenericType(types),
                        new object[] { elem.AddMethod, evArgs[0] }
                        );
                }
                else if (evArgs.Length == 2)
                {
                    var types = new Type[] { type, evArgs[0].ReturnType, evArgs[01].ReturnType };
                    return (Initializer)Activator.CreateInstance(
                        typeof(ElementInitializer<,,>).MakeGenericType(types),
                        new object[] { elem.AddMethod, evArgs[0], evArgs[1] }
                        );
                }
                else
                {
                    return (Initializer)Activator.CreateInstance(
                        typeof(ElementInitializer<>).MakeGenericType(type),
                        new object[] { elem.AddMethod, evArgs }
                        );
                }
            }

            private MethodInfo FindBestMethod(IEnumerable<MethodInfo> methods, Type[] argTypes, Type returnType)
            {
                var meth = methods.FirstOrDefault(m => MethodMatchesExact(m, argTypes, returnType));
                if (meth == null)
                    meth = methods.FirstOrDefault(m => MethodMatches(m, argTypes, returnType));
                return meth;
            }

            private bool MethodMatchesExact(MethodInfo method, Type[] argTypes, Type returnType)
            {
                if (method.ReturnType != returnType)
                    return false;
                var parameters = method.GetParameters();
                if (parameters.Length != argTypes.Length)
                    return false;
                for (int i = 0, n = parameters.Length; i < n; i++)
                {
                    if (parameters[i].ParameterType != argTypes[i])
                        return false;
                }
                return true;
            }

            private bool MethodMatches(MethodInfo method, Type[] argTypes, Type returnType)
            {
                if (returnType != method.ReturnType && !method.ReflectedType.IsSubclassOf(returnType))
                    return false;
                var parameters = method.GetParameters();
                if (parameters.Length != argTypes.Length)
                    return false;
                for (int i = 0, n = parameters.Length; i < n; i++)
                {
                    if (parameters[i].ParameterType != argTypes[i] && !argTypes[i].IsSubclassOf(parameters[i].ParameterType))
                        return false;
                }
                return true;
            }

            private static Type GetNonNullType(Type type)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return type.GetGenericArguments()[0];
                }
                return type;
            }

            private static bool IsNullable(Type type)
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
        }

        public abstract class Evaluator
        {
            public abstract object EvalBoxed(EvaluatorState state);
            public abstract Address EvalAddressBoxed(EvaluatorState state);
            public abstract Type ReturnType { get; }
        }

        public abstract class Evaluator<T> : Evaluator
        {
            public abstract T Eval(EvaluatorState state);

            public override object EvalBoxed(EvaluatorState state)
            {
                return this.Eval(state);
            }

            public virtual Address<T> EvalAddress(EvaluatorState state)
            {
                return new ValueAddress<T>(this.Eval(state));
            }

            public override Address EvalAddressBoxed(EvaluatorState state)
            {
                return this.EvalAddress(state);
            }

            public override Type ReturnType
            {
                get { return typeof(T); }
            }
        }

        public abstract class Address
        {
            public abstract object GetBoxedValue(EvaluatorState state);
            public abstract void SetBoxedValue(EvaluatorState state, object value);
        }

        public abstract class Address<T> : Address
        {
            public abstract T GetValue(EvaluatorState state);
            public abstract void SetValue(EvaluatorState state, T value);

            public override object GetBoxedValue(EvaluatorState state)
            {
                return this.GetValue(state);
            }

            public override void SetBoxedValue(EvaluatorState state, object value)
            {
                this.SetValue(state, (T)value);
            }
        }

        public class ValueAddress<T> : Address<T>
        {
            T value;

            public ValueAddress(T value)
            {
                this.value = value;
            }

            public override T GetValue(EvaluatorState state)
            {
                return this.value;
            }

            public override void SetValue(EvaluatorState state, T value)
            {
                this.value = value;
            }
        }

        public class UnaryEvaluator<S, T> : Evaluator<T>
        {
            Evaluator<S> operand;
            Func<S, T> op;

            public UnaryEvaluator(Evaluator<S> operand, Func<S, T> op)
            {
                this.operand = operand;
                this.op = op;
            }

            public override T Eval(EvaluatorState state)
            {
                return op(operand.Eval(state));
            }
        }

        public class LiftToNullUnaryEvaluator<S, T> : Evaluator<T?>
            where S : struct
            where T : struct
        {
            Evaluator<S?> operand;
            Func<S, T> op;

            public LiftToNullUnaryEvaluator(Evaluator<S?> operand, Func<S, T> op)
            {
                this.operand = operand;
                this.op = op;
            }

            public override T? Eval(EvaluatorState state)
            {
                var val = operand.Eval(state);
                if (val == null)
                    return null;
                return op(val.GetValueOrDefault());
            }
        }

        public class LiftToFalseUnaryEvaluator<S> : Evaluator<bool> where S : struct
        {
            Evaluator<S?> operand;
            Func<S, bool> op;

            public LiftToFalseUnaryEvaluator(Evaluator<S?> operand, Func<S, bool> op)
            {
                this.operand = operand;
                this.op = op;
            }

            public override bool Eval(EvaluatorState state)
            {
                var val = operand.Eval(state);
                if (val == null)
                    return false;
                return op(val.GetValueOrDefault());
            }
        }

        public class BinaryEvaluator<S, T> : Evaluator<T>
        {
            Evaluator<S> left;
            Evaluator<S> right;
            Func<S, S, T> op;

            public BinaryEvaluator(Evaluator<S> left, Evaluator<S> right, Func<S, S, T> op)
            {
                this.left = left;
                this.right = right;
                this.op = op;
            }

            public override T Eval(EvaluatorState state)
            {
                return op(left.Eval(state), right.Eval(state));
            }
        }

        public class LiftToNullBinaryEvaluator<S, T> : Evaluator<T?>
            where T : struct
            where S : struct
        {
            Evaluator<S?> left;
            Evaluator<S?> right;
            Func<S, S, T> op;

            public LiftToNullBinaryEvaluator(Evaluator<S?> left, Evaluator<S?> right, Func<S, S, T> op)
            {
                this.left = left;
                this.right = right;
                this.op = op;
            }

            public override T? Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                var rval = right.Eval(state);
                if (lval == null || rval == null)
                    return null;
                return op(lval.GetValueOrDefault(), rval.GetValueOrDefault());
            }
        }

        public class LiftToFalseBinaryEvaluator<S> : Evaluator<bool> where S : struct
        {
            Evaluator<S?> left;
            Evaluator<S?> right;
            Func<S, S, bool> op;

            public LiftToFalseBinaryEvaluator(Evaluator<S?> left, Evaluator<S?> right, Func<S, S, bool> op)
            {
                this.left = left;
                this.right = right;
                this.op = op;
            }

            public override bool Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                var rval = right.Eval(state);
                if (lval == null || rval == null)
                    return false;
                return op(lval.GetValueOrDefault(), rval.GetValueOrDefault());
            }
        }

        public class LiftToEqualBinaryEvaluator<S> : Evaluator<bool> where S : struct
        {
            Evaluator<S?> left;
            Evaluator<S?> right;
            Func<S, S, bool> op;

            public LiftToEqualBinaryEvaluator(Evaluator<S?> left, Evaluator<S?> right, Func<S, S, bool> op)
            {
                this.left = left;
                this.right = right;
                this.op = op;
            }

            public override bool Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                var rval = right.Eval(state);
                if (lval == null || rval == null)
                {
                    return (lval == null && rval == null);
                }
                return op(lval.GetValueOrDefault(), rval.GetValueOrDefault());
            }
        }

        public class LiftToNotEqualBinaryEvaluator<S> : Evaluator<bool> where S : struct
        {
            Evaluator<S?> left;
            Evaluator<S?> right;
            Func<S, S, bool> op;

            public LiftToNotEqualBinaryEvaluator(Evaluator<S?> left, Evaluator<S?> right, Func<S, S, bool> op)
            {
                this.left = left;
                this.right = right;
                this.op = op;
            }

            public override bool Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                var rval = right.Eval(state);
                if (lval == null || rval == null)
                {
                    return !(lval == null && rval == null);
                }
                return op(lval.GetValueOrDefault(), rval.GetValueOrDefault());
            }
        }

        public class AndAlsoEvaluator : Evaluator<bool>
        {
            Evaluator<bool> left;
            Evaluator<bool> right;

            public AndAlsoEvaluator(Evaluator<bool> left, Evaluator<bool> right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool Eval(EvaluatorState state)
            {
                return left.Eval(state) && right.Eval(state);
            }
        }

        public class LiftToNullAndAlsoEvaluator : Evaluator<bool?>
        {
            Evaluator<bool?> left;
            Evaluator<bool?> right;

            public LiftToNullAndAlsoEvaluator(Evaluator<bool?> left, Evaluator<bool?> right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool? Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                if (lval == null) return null;
                if (!lval.GetValueOrDefault()) return false;
                var rval = right.Eval(state);
                if (rval == null) return null;
                return rval.GetValueOrDefault();
            }
        }

        public class LiftToFalseAndAlsoEvaluator : Evaluator<bool>
        {
            Evaluator<bool?> left;
            Evaluator<bool?> right;

            public LiftToFalseAndAlsoEvaluator(Evaluator<bool?> left, Evaluator<bool?> right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                if (lval == null) return false;
                var rval = right.Eval(state);
                if (rval == null) return false;
                return lval.GetValueOrDefault() && rval.GetValueOrDefault();
            }
        }

        public class OrElseEvaluator : Evaluator<bool>
        {
            Evaluator<bool> left;
            Evaluator<bool> right;

            public OrElseEvaluator(Evaluator<bool> left, Evaluator<bool> right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool Eval(EvaluatorState state)
            {
                return left.Eval(state) || right.Eval(state);
            }
        }

        public class LiftToNullOrElseEvaluator : Evaluator<bool?>
        {
            Evaluator<bool?> left;
            Evaluator<bool?> right;

            public LiftToNullOrElseEvaluator(Evaluator<bool?> left, Evaluator<bool?> right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool? Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                if (lval == null) return null;
                if (lval.GetValueOrDefault()) return true;
                var rval = right.Eval(state);
                if (rval == null) return null;
                return rval.GetValueOrDefault();
            }
        }

        public class LiftToFalseOrElseEvaluator : Evaluator<bool>
        {
            Evaluator<bool?> left;
            Evaluator<bool?> right;

            public LiftToFalseOrElseEvaluator(Evaluator<bool?> left, Evaluator<bool?> right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool Eval(EvaluatorState state)
            {
                var lval = left.Eval(state);
                if (lval == null) return false;
                if (lval.GetValueOrDefault()) return true;
                var rval = right.Eval(state);
                if (rval == null) return false;
                return rval.GetValueOrDefault();
            }
        }

        public class Convert<S, T> : Evaluator<T>
        {
            Evaluator<S> operand;
            Func<S, T> fnConvert;

            public Convert(Evaluator<S> operand)
                : this(operand, null)
            {
            }

            public Convert(Evaluator<S> operand, Func<S, T> fnConvert)
            {
                this.operand = operand;
                this.fnConvert = fnConvert;
            }

            public override T Eval(EvaluatorState state)
            {
                var value = this.operand.Eval(state);
                if (this.fnConvert != null)
                {
                    return fnConvert(value);
                }
                else
                {
                    return (T)(object)value;
                }
            }
        }

        public class ConvertNtoNN<T> : Evaluator<T> where T : struct
        {
            Evaluator<T?> operand;

            public ConvertNtoNN(Evaluator<T?> operand)
            {
                this.operand = operand;
            }

            public override T Eval(EvaluatorState state)
            {
                return (T)this.operand.Eval(state);
            }
        }

        public class ConvertNNtoN<T> : Evaluator<T?> where T : struct
        {
            Evaluator<T> operand;

            public ConvertNNtoN(Evaluator<T> operand)
            {
                this.operand = operand;
            }

            public override T? Eval(EvaluatorState state)
            {
                return this.operand.Eval(state);
            }
        }

        public class ConvertNtoNN<S, T> : Evaluator<T>
            where S : struct
            where T : struct
        {
            Evaluator<S?> operand;
            Func<S, T> fnConvert;

            public ConvertNtoNN(Evaluator<S?> operand, Func<S, T> fnConvert)
            {
                this.operand = operand;
                this.fnConvert = fnConvert;
            }

            public override T Eval(EvaluatorState state)
            {
                return this.fnConvert((S)this.operand.Eval(state));
            }
        }

        public class ConvertNNtoN<S, T> : Evaluator<T?>
            where S : struct
            where T : struct
        {
            Evaluator<S> operand;
            Func<S, T> fnConvert;

            public ConvertNNtoN(Evaluator<S> operand, Func<S, T> fnConvert)
            {
                this.operand = operand;
                this.fnConvert = fnConvert;
            }

            public override T? Eval(EvaluatorState state)
            {
                return this.fnConvert(this.operand.Eval(state));
            }
        }

        public class ConvertNtoN<S, T> : Evaluator<T?>
            where S : struct
            where T : struct
        {
            Evaluator<S?> operand;
            Func<S, T> fnConvert;

            public ConvertNtoN(Evaluator<S?> operand, Func<S, T> fnConvert)
            {
                this.operand = operand;
                this.fnConvert = fnConvert;
            }

            public override T? Eval(EvaluatorState state)
            {
                var value = this.operand.Eval(state);
                if (value == null)
                    return null;
                return this.fnConvert(value.GetValueOrDefault());
            }
        }

        public class CoalesceREvaluator<L, R> : Evaluator<R>
        {
            Evaluator<L> opLeft;
            Evaluator<R> opRight;
            Func<L, R> fnConversion;

            public CoalesceREvaluator(Evaluator<L> opLeft, Evaluator<R> opRight, Func<L, R> fnConversion)
            {
                this.opLeft = opLeft;
                this.opRight = opRight;
                this.fnConversion = fnConversion;
            }

            public override R Eval(EvaluatorState state)
            {
                var lval = opLeft.Eval(state);
                if (lval != null)
                    return this.fnConversion(lval);
                return opRight.Eval(state);
            }
        }

        public class CoalesceLEvaluator<L, R> : Evaluator<L> where R : struct
        {
            Evaluator<L> opLeft;
            Evaluator<R> opRight;
            Func<R, L> fnConversion;

            public CoalesceLEvaluator(Evaluator<L> opLeft, Evaluator<R> opRight, Func<R, L> fnConversion)
            {
                this.opLeft = opLeft;
                this.opRight = opRight;
                this.fnConversion = fnConversion;
            }

            public override L Eval(EvaluatorState state)
            {
                var lval = opLeft.Eval(state);
                if (lval != null)
                    return lval;
                return this.fnConversion(opRight.Eval(state));
            }
        }

        public class ConstantEvaluator<T> : Evaluator<T>
        {
            T value;

            public ConstantEvaluator(T value)
            {
                this.value = value;
            }

            public override T Eval(EvaluatorState state)
            {
                return value;
            }
        }

        public class ParameterEvaluator<T> : Evaluator<T>
        {
            int index;

            public ParameterEvaluator(int index)
            {
                this.index = index;
            }

            public override T Eval(EvaluatorState state)
            {
                return state.GetValue<T>(this.index);
            }

            public override Address<T> EvalAddress(EvaluatorState state)
            {
                return new ParameterAddress(this.index);
            }

            class ParameterAddress : Address<T>
            {
                int index;

                public ParameterAddress(int index)
                {
                    this.index = index;
                }

                public override T GetValue(EvaluatorState state)
                {
                    return state.GetValue<T>(this.index);
                }

                public override void SetValue(EvaluatorState state, T value)
                {
                    state.SetValue<T>(this.index, value);
                }
            }
        }

        public class MethodCallEvaluator<T> : Evaluator<T>
        {
            MethodInfo method;
            Evaluator opInst;
            Evaluator[] opArgs;

            public MethodCallEvaluator(MethodInfo method, Evaluator opInst, Evaluator[] opArgs)
            {
                this.method = method;
                this.opInst = opInst;
                this.opArgs = opArgs;
            }

            public override T Eval(EvaluatorState state)
            {
                try
                {
                    object result;
                    if (opInst != null && opInst.ReturnType.IsValueType)
                    {
                        Address addrInstr = opInst.EvalAddressBoxed(state);
                        object instance = addrInstr.GetBoxedValue(state);
                        object[] args = opArgs.Select(a => a.EvalBoxed(state)).ToArray();
                        result = method.Invoke(instance, args);
                        addrInstr.SetBoxedValue(state, instance);
                    }
                    else
                    {
                        object instance = opInst != null ? opInst.EvalBoxed(state) : null;
                        object[] args = opArgs.Select(a => a.EvalBoxed(state)).ToArray();
                        result = method.Invoke(instance, args);
                    }
                    return (T)result;
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }
        }

        public class FuncEvaluator<R> : Evaluator<R>
        {
            Func<R> method;

            public FuncEvaluator(Func<R> method)
            {
                this.method = method;
            }

            public override R Eval(EvaluatorState state)
            {
                return this.method();
            }
        }

        public class FuncEvaluator<A, R> : Evaluator<R>
        {
            Evaluator<A> arg;
            Func<A, R> method;

            public FuncEvaluator(Evaluator<A> arg, Func<A, R> method)
            {
                this.arg = arg;
                this.method = method;
            }

            public override R Eval(EvaluatorState state)
            {
                A a = this.arg.Eval(state);
                return this.method(a);
            }
        }

        public class FuncEvaluator<A1, A2, R> : Evaluator<R>
        {
            Evaluator<A1> arg1;
            Evaluator<A2> arg2;
            Func<A1, A2, R> method;

            public FuncEvaluator(Evaluator<A1> arg1, Evaluator<A2> arg2, Func<A1, A2, R> method)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.method = method;
            }

            public override R Eval(EvaluatorState state)
            {
                return this.method(arg1.Eval(state), arg2.Eval(state));
            }
        }

        public class FuncEvaluator<A1, A2, A3, R> : Evaluator<R>
        {
            Evaluator<A1> arg1;
            Evaluator<A2> arg2;
            Evaluator<A3> arg3;
            Func<A1, A2, A3, R> method;

            public FuncEvaluator(Evaluator<A1> arg1, Evaluator<A2> arg2, Evaluator<A3> arg3, Func<A1, A2, A3, R> method)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.method = method;
            }

            public override R Eval(EvaluatorState state)
            {
                return this.method(arg1.Eval(state), arg2.Eval(state), arg3.Eval(state));
            }
        }

        public class FuncEvaluator<A1, A2, A3, A4, R> : Evaluator<R>
        {
            Evaluator<A1> arg1;
            Evaluator<A2> arg2;
            Evaluator<A3> arg3;
            Evaluator<A4> arg4;
            Func<A1, A2, A3, A4, R> method;

            public FuncEvaluator(Evaluator<A1> arg1, Evaluator<A2> arg2, Evaluator<A3> arg3, Evaluator<A4> arg4, Func<A1, A2, A3, A4, R> method)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
                this.arg4 = arg4;
                this.method = method;
            }

            public override R Eval(EvaluatorState state)
            {
                return this.method(arg1.Eval(state), arg2.Eval(state), arg3.Eval(state), arg4.Eval(state));
            }
        }

        public class MethodCallWithRefArgsEvaluator<T> : Evaluator<T>
        {
            MethodInfo method;
            ParameterInfo[] parameters;
            Evaluator opInst;
            Evaluator[] opArgs;

            public MethodCallWithRefArgsEvaluator(MethodInfo method, Evaluator opInst, Evaluator[] opArgs)
            {
                this.method = method;
                this.parameters = method.GetParameters();
                this.opInst = opInst;
                this.opArgs = opArgs;
            }

            public override T Eval(EvaluatorState state)
            {
                try
                {
                    Address addrInstr = null;
                    object instance;
                    Address[] addrs;
                    object[] args;
                    object result;

                    if (opInst != null && opInst.ReturnType.IsValueType)
                    {
                        addrInstr = opInst.EvalAddressBoxed(state);
                        instance = addrInstr.GetBoxedValue(state);
                    }
                    else
                    {
                        instance = opInst != null ? opInst.EvalBoxed(state) : null;
                    }

                    addrs = opArgs.Select(a => a.EvalAddressBoxed(state)).ToArray();
                    args = addrs.Select(a => a.GetBoxedValue(state)).ToArray();
                    result = method.Invoke(instance, args);

                    for (int i = 0, n = args.Length; i < n; i++)
                    {
                        if (this.parameters[i].ParameterType.IsByRef)
                        {
                            addrs[i].SetBoxedValue(state, args[i]);
                        }
                    }

                    if (addrInstr != null)
                    {
                        addrInstr.SetBoxedValue(state, instance);
                    }

                    return (T)result;
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }
        }

        public class LiftToNullMethodCallEvaluator<T> : Evaluator<T?> where T : struct
        {
            MethodInfo method;
            Evaluator opInst;
            Evaluator[] opArgs;

            public LiftToNullMethodCallEvaluator(MethodInfo method, Evaluator opInst, Evaluator[] opArgs)
            {
                this.method = method;
                this.opInst = opInst;
                this.opArgs = opArgs;
            }

            public override T? Eval(EvaluatorState state)
            {
                object instance = opInst != null ? opInst.EvalBoxed(state) : null;
                object[] args = opArgs.Select(a => a.EvalBoxed(state)).ToArray();
                if (instance == null || args.Any(a => a == null))
                    return null;
                try
                {
                    return (T?)method.Invoke(instance, args);
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }
        }

        public class LiftToFalseMethodCallEvaluator : Evaluator<bool>
        {
            MethodInfo method;
            Evaluator opInst;
            Evaluator[] opArgs;

            public LiftToFalseMethodCallEvaluator(MethodInfo method, Evaluator opInst, Evaluator[] opArgs)
            {
                this.method = method;
                this.opInst = opInst;
                this.opArgs = opArgs;
            }

            public override bool Eval(EvaluatorState state)
            {
                object instance = opInst != null ? opInst.EvalBoxed(state) : null;
                object[] args = opArgs.Select(a => a.EvalBoxed(state)).ToArray();
                if (instance == null || args.Any(a => a == null))
                    return false;
                try
                {
                    return (bool)method.Invoke(instance, args);
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }
        }

        public class FieldAccessEvaluator<T> : Evaluator<T>
        {
            Evaluator operand;
            FieldInfo field;

            public FieldAccessEvaluator(Evaluator operand, FieldInfo field)
            {
                this.operand = operand;
                this.field = field;
            }

            public override T Eval(EvaluatorState state)
            {
                var instance = this.operand != null ? this.operand.EvalBoxed(state) : null;
                return (T)field.GetValue(instance);
            }

            public override Address<T> EvalAddress(EvaluatorState state)
            {
                Address addr = this.operand != null ? this.operand.EvalAddressBoxed(state) : null;
                return new FieldAddress(addr, this.field);
            }

            class FieldAddress : Address<T>
            {
                Address instance;
                FieldInfo field;

                public FieldAddress(Address instance, FieldInfo field)
                {
                    this.instance = instance;
                    this.field = field;
                }

                public override T GetValue(EvaluatorState state)
                {
                    object boxedInst = this.instance != null ? this.instance.GetBoxedValue(state) : null;
                    return (T)field.GetValue(boxedInst);
                }

                public override void SetValue(EvaluatorState state, T value)
                {
                    object boxedInst = this.instance != null ? this.instance.GetBoxedValue(state) : null;
                    field.SetValue(boxedInst, value);
                    if (this.instance != null)
                    {
                        this.instance.SetBoxedValue(state, boxedInst);
                    }
                }
            }
        }

        public class PropertyAccessEvaluator<T, V> : Evaluator<V>
        {
            Evaluator<T> operand;
            Func<T, V> fnGetter;

            public PropertyAccessEvaluator(Evaluator<T> operand, PropertyInfo property)
            {
                this.operand = operand;
                this.fnGetter = (Func<T, V>)Delegate.CreateDelegate(typeof(Func<T, V>), property.GetGetMethod(true));
            }

            public override V Eval(EvaluatorState state)
            {
                T item = this.operand != null ? this.operand.Eval(state) : default(T);
                return this.fnGetter(item);
            }
        }

        public class ConditionalEvaluator<T> : Evaluator<T>
        {
            Evaluator<bool> test;
            Evaluator<T> ifTrue;
            Evaluator<T> ifFalse;

            public ConditionalEvaluator(Evaluator<bool> test, Evaluator<T> ifTrue, Evaluator<T> ifFalse)
            {
                this.test = test;
                this.ifTrue = ifTrue;
                this.ifFalse = ifFalse;
            }

            public override T Eval(EvaluatorState state)
            {
                return (test.Eval(state)) ? ifTrue.Eval(state) : ifFalse.Eval(state);
            }
        }

        public class TypeIsEvaluator : Evaluator<bool>
        {
            Evaluator thing;
            Type type;

            public TypeIsEvaluator(Evaluator thing, Type type)
            {
                this.thing = thing;
                this.type = type;
            }

            public override bool Eval(EvaluatorState state)
            {
                var result = thing.EvalBoxed(state);
                if (result == null) return false;
                Type resultType = result.GetType();
                return (resultType == type || resultType.IsSubclassOf(type));
            }
        }

        public class TypeAsEvaluator<S, T> : Evaluator<T>
            where S : class
            where T : class
        {
            Evaluator<S> operand;

            public TypeAsEvaluator(Evaluator<S> operand)
            {
                this.operand = operand;
            }

            public override T Eval(EvaluatorState state)
            {
                return operand.Eval(state) as T;
            }
        }

        private static Func<Type, object, RuntimeMethodHandle, Delegate> fnCreateDelegate;
        private static Delegate CreateDelegate(Type delegateType, ConstructorInfo constructor)
        {
            if (fnCreateDelegate == null)
            {
                MethodInfo miCreateDelegate =
                    typeof(Delegate).GetMethod(
                        "CreateDelegate", BindingFlags.Static | BindingFlags.NonPublic, null,
                        new Type[] { typeof(Type), typeof(object), typeof(RuntimeMethodHandle) }, null
                        );
                fnCreateDelegate = (Func<Type, object, RuntimeMethodHandle, Delegate>)
                    Delegate.CreateDelegate(typeof(Func<Type, object, RuntimeMethodHandle, Delegate>), miCreateDelegate);
            }
            return fnCreateDelegate(delegateType, null, constructor.MethodHandle);
        }

        //public class NewEvaluator<T> : Evaluator<T>
        //{
        //    Action<T> fnConstructor;

        //    public NewEvaluator(ConstructorInfo constructor)
        //    {
        //        if (constructor != null)
        //            this.fnConstructor = (Action<T>)CreateDelegate(typeof(Action<T>), constructor);
        //    }

        //    public override T Eval(EvaluatorState state)
        //    {
        //        T t = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T));
        //        if (this.fnConstructor != null)
        //            this.fnConstructor(t);
        //        return t;
        //    }
        //}

        //public class NewEvaluator<T, A> : Evaluator<T>
        //{
        //    Action<T, A> fnConstructor;
        //    Evaluator<A> opArg;

        //    public NewEvaluator(ConstructorInfo constructor, Evaluator<A> opArg)
        //    {
        //        this.fnConstructor = (Action<T, A>)CreateDelegate(typeof(Action<T, A>), constructor);
        //        this.opArg = opArg;
        //    }

        //    public override T Eval(EvaluatorState state)
        //    {
        //        A a = this.opArg.Eval(state);
        //        T t = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T));
        //        this.fnConstructor(t, a);
        //        return t;
        //    }
        //}

        //public class NewEvaluator<T, A1, A2> : Evaluator<T>
        //{
        //    Action<T, A1, A2> fnConstructor;
        //    Evaluator<A1> opArg1;
        //    Evaluator<A2> opArg2;

        //    public NewEvaluator(ConstructorInfo constructor, Evaluator<A1> opArg1, Evaluator<A2> opArg2)
        //    {
        //        this.fnConstructor = (Action<T, A1, A2>)CreateDelegate(typeof(Action<T, A1, A2>), constructor);
        //        this.opArg1 = opArg1;
        //        this.opArg2 = opArg2;
        //    }

        //    public override T Eval(EvaluatorState state)
        //    {
        //        A1 a1 = this.opArg1.Eval(state);
        //        A2 a2 = this.opArg2.Eval(state);
        //        T t = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T));
        //        this.fnConstructor(t, a1, a2);
        //        return t;
        //    }
        //}

        //public class NewEvaluator<T, A1, A2, A3> : Evaluator<T>
        //{
        //    Action<T, A1, A2, A3> fnConstructor;
        //    Evaluator<A1> opArg1;
        //    Evaluator<A2> opArg2;
        //    Evaluator<A3> opArg3;

        //    public NewEvaluator(ConstructorInfo constructor, Evaluator<A1> opArg1, Evaluator<A2> opArg2, Evaluator<A3> opArg3)
        //    {
        //        this.fnConstructor = (Action<T, A1, A2, A3>)CreateDelegate(typeof(Action<T, A1, A2, A3>), constructor);
        //        this.opArg1 = opArg1;
        //        this.opArg2 = opArg2;
        //        this.opArg3 = opArg3;
        //    }

        //    public override T Eval(EvaluatorState state)
        //    {
        //        A1 a1 = this.opArg1.Eval(state);
        //        A2 a2 = this.opArg2.Eval(state);
        //        A3 a3 = this.opArg3.Eval(state);
        //        T t = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T));
        //        this.fnConstructor(t, a1, a2, a3);
        //        return t;
        //    }
        //}

        //public class NewEvaluator<T, A1, A2, A3, A4> : Evaluator<T>
        //{
        //    delegate void Action(T t, A1 a1, A2 a2, A3 a3, A4 a4);
        //    Action fnConstructor;
        //    Evaluator<A1> opArg1;
        //    Evaluator<A2> opArg2;
        //    Evaluator<A3> opArg3;
        //    Evaluator<A4> opArg4;

        //    public NewEvaluator(ConstructorInfo constructor, Evaluator<A1> opArg1, Evaluator<A2> opArg2, Evaluator<A3> opArg3, Evaluator<A4> opArg4)
        //    {
        //        this.fnConstructor = (Action)CreateDelegate(typeof(Action), constructor);
        //        this.opArg1 = opArg1;
        //        this.opArg2 = opArg2;
        //        this.opArg3 = opArg3;
        //        this.opArg4 = opArg4;
        //    }

        //    public override T Eval(EvaluatorState state)
        //    {
        //        A1 a1 = this.opArg1.Eval(state);
        //        A2 a2 = this.opArg2.Eval(state);
        //        A3 a3 = this.opArg3.Eval(state);
        //        A4 a4 = this.opArg4.Eval(state);
        //        T t = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T));
        //        this.fnConstructor(t, a1, a2, a3, a4);
        //        return t;
        //    }
        //}

        public class NewEvaluatorN<T> : Evaluator<T>
        {
            ConstructorInfo constructor;
            Evaluator[] opArgs;

            public NewEvaluatorN(ConstructorInfo constructor, Evaluator[] opArgs)
            {
                this.constructor = constructor;
                this.opArgs = opArgs;
            }

            public override T Eval(EvaluatorState state)
            {
                var args = opArgs.Select(a => a.EvalBoxed(state)).ToArray();
                try
                {
                    return (T)constructor.Invoke(args);
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }
        }

        public class NewArrayInitEvaluator<T> : Evaluator<T[]>
        {
            Evaluator<T>[] initializers;

            public NewArrayInitEvaluator(Evaluator[] initializers)
            {
                this.initializers = initializers.Select(i => (Evaluator<T>)i).ToArray();
            }

            public override T[] Eval(EvaluatorState state)
            {
                T[] array = new T[initializers.Length];
                for (int i = 0, n = initializers.Length; i < n; i++)
                {
                    array[i] = initializers[i].Eval(state);
                }
                return array;
            }
        }

        public class NewArrayBoundsEvaluator<T> : Evaluator<T[]>
        {
            Evaluator<int>[] bounds;

            public NewArrayBoundsEvaluator(Evaluator[] bounds)
            {
                this.bounds = bounds.Select(b => (Evaluator<int>)b).ToArray();
            }

            public override T[] Eval(EvaluatorState state)
            {
                int[] bounds = this.bounds.Select(b => b.Eval(state)).ToArray();
                return (T[])Array.CreateInstance(typeof(T), bounds);
            }
        }

        public class ArrayIndexEvaluator<T> : Evaluator<T>
        {
            Evaluator<T[]> opArray;
            Evaluator<int> opIndex;

            public ArrayIndexEvaluator(Evaluator<T[]> opArray, Evaluator<int> opIndex)
            {
                this.opArray = opArray;
                this.opIndex = opIndex;
            }

            public override T Eval(EvaluatorState state)
            {
                return this.opArray.Eval(state)[this.opIndex.Eval(state)];
            }

            public override Address<T> EvalAddress(EvaluatorState state)
            {
                return new ArrayElementAddress(this.opArray.Eval(state), this.opIndex.Eval(state));
            }

            class ArrayElementAddress : Address<T>
            {
                T[] array;
                int index;

                public ArrayElementAddress(T[] array, int index)
                {
                    this.array = array;
                    this.index = index;
                }

                public override T GetValue(EvaluatorState state)
                {
                    return this.array[this.index];
                }

                public override void SetValue(EvaluatorState state, T value)
                {
                    this.array[this.index] = value;
                }
            }
        }

        public class ArrayLengthEvaluator<T> : Evaluator<int>
        {
            Evaluator<T[]> opArray;

            public ArrayLengthEvaluator(Evaluator<T[]> opArray)
            {
                this.opArray = opArray;
            }

            public override int Eval(EvaluatorState state)
            {
                return this.opArray.Eval(state).Length;
            }
        }

        public class LambdaEvaluator<T> : Evaluator<T> where T : class
        {
            int nArgs;
            Evaluator evaluator;

            public LambdaEvaluator(int nArgs, Evaluator evaluator)
            {
                this.nArgs = nArgs;
                this.evaluator = evaluator;
            }

            public override T Eval(EvaluatorState state)
            {
                T d = (T)(object)CreateDelegate(typeof(T), state, this.nArgs, this.evaluator);
                return d;
            }
        }

        public class LetEvaluator<V, E> : Evaluator<E>
        {
            int index;
            Evaluator<V> evValue;
            Evaluator<E> evExpression;

            public LetEvaluator(int index, Evaluator<V> evValue, Evaluator<E> evExpression)
            {
                this.index = index;
                this.evValue = evValue;
                this.evExpression = evExpression;
            }

            public override E Eval(EvaluatorState state)
            {
                V value = this.evValue.Eval(state);
                state.SetValue<V>(this.index, value);
                return this.evExpression.Eval(state);
            }
        }

        public class InvokeEvaluator<T> : Evaluator<T>
        {
            Evaluator opFunction;
            Evaluator[] opArgs;

            public InvokeEvaluator(Evaluator opFunction, Evaluator[] opArgs)
            {
                this.opFunction = opFunction;
                this.opArgs = opArgs;
            }

            public override T Eval(EvaluatorState state)
            {
                var function = opFunction.EvalBoxed(state);
                var args = opArgs.Select(a => a.EvalBoxed(state));
                try
                {
                    return (T)((Delegate)function).DynamicInvoke(args);
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }
        }

        public class QuoteEvaluator<T> : Evaluator<T> where T : Expression
        {
            Expression expression;
            Dictionary<ParameterExpression, int> external;

            public QuoteEvaluator(Expression expression, Dictionary<ParameterExpression, int> external)
            {
                this.expression = expression;
                this.external = external;
            }

            public override T Eval(EvaluatorState state)
            {
                if (external.Count > 0)
                {
                    // replace external parameter references with current values
                    return (T)QuoteRewriter.Rewrite(this.external, state, this.expression);
                }
                return (T)this.expression;
            }

            class QuoteRewriter : OldExpressionVisitor
            {
                Dictionary<ParameterExpression, int> external;
                EvaluatorState state;

                private QuoteRewriter(Dictionary<ParameterExpression, int> external, EvaluatorState state)
                {
                    this.external = external;
                    this.state = state;
                }

                internal static Expression Rewrite(Dictionary<ParameterExpression, int> external, EvaluatorState state, Expression expression)
                {
                    return new QuoteRewriter(external, state).Visit(expression);
                }

                internal override Expression VisitParameter(ParameterExpression p)
                {
                    int externalIndex;
                    if (this.external.TryGetValue(p, out externalIndex))
                    {
                        return Expression.Constant(this.state.GetBoxedValue(externalIndex), p.Type);
                    }
                    return p;
                }
            }
        }

        public class InitializerEvaluator<T> : Evaluator<T>
        {
            Evaluator<T> opNew;
            Initializer<T>[] initializers;

            public InitializerEvaluator(Evaluator<T> opNew, Initializer[] initializers)
            {
                this.opNew = opNew;
                this.initializers = initializers.Select(i => (Initializer<T>)i).ToArray();
            }

            public override T Eval(EvaluatorState state)
            {
                var instance = opNew.Eval(state);
                for (int i = 0, n = initializers.Length; i < n; i++)
                {
                    initializers[i].Init(state, ref instance);
                }
                return instance;
            }
        }

        public abstract class Initializer
        {
        }

        public abstract class Initializer<T> : Initializer
        {
            public abstract void Init(EvaluatorState state, ref T instance);
        }

        public class FieldAssignmentInitializer<T> : Initializer<T> where T : class
        {
            FieldInfo field;
            Evaluator evaluator;

            public FieldAssignmentInitializer(FieldInfo field, Evaluator evaluator)
            {
                this.field = field;
                this.evaluator = evaluator;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                var value = this.evaluator.EvalBoxed(state);
                field.SetValue(instance, value);
            }
        }

        public class ValueTypeFieldAssignmentInitializer<T> : Initializer<T> where T : struct
        {
            FieldInfo field;
            Evaluator evaluator;

            public ValueTypeFieldAssignmentInitializer(FieldInfo field, Evaluator evaluator)
            {
                this.field = field;
                this.evaluator = evaluator;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                var value = this.evaluator.EvalBoxed(state);
                object boxedInstance = instance;
                field.SetValue(boxedInstance, value);
                instance = (T)boxedInstance;
            }
        }

        public class ValueTypePropertyAssignmentInitializer<T, V> : Initializer<T> where T : struct
        {
            delegate void Assigner(ref T instance, V value);
            Assigner fnSetter;
            Evaluator<V> evaluator;

            public ValueTypePropertyAssignmentInitializer(PropertyInfo property, Evaluator<V> evaluator)
            {
                this.fnSetter = (Assigner)Delegate.CreateDelegate(typeof(Assigner), property.GetSetMethod(true));
                this.evaluator = evaluator;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                V value = this.evaluator.Eval(state);
                fnSetter(ref instance, value);
            }
        }

        public class PropertyAssignmentInitializer<T, V> : Initializer<T> where T : class
        {
            Action<T, V> fnSetter;
            Evaluator<V> evaluator;

            public PropertyAssignmentInitializer(PropertyInfo property, Evaluator<V> evaluator)
            {
                this.fnSetter = (Action<T, V>)Delegate.CreateDelegate(typeof(Action<T, V>), property.GetSetMethod(true));
                this.evaluator = evaluator;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                V value = this.evaluator.Eval(state);
                fnSetter(instance, value);
            }
        }

        public class FieldMemberInitializer<T, V> : Initializer<T> where T : class
        {
            FieldInfo field;
            Initializer<V>[] initializers;

            public FieldMemberInitializer(FieldInfo field, Initializer[] initializers)
            {
                this.field = field;
                this.initializers = initializers.Select(i => (Initializer<V>)i).ToArray();
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                V value = (V)field.GetValue(instance);
                for (int i = 0, n = this.initializers.Length; i < n; i++)
                {
                    this.initializers[i].Init(state, ref value);
                }
                field.SetValue(instance, value);
            }
        }

        public class ValueTypeFieldMemberInitializer<T, V> : Initializer<T> where T : struct
        {
            FieldInfo field;
            Initializer<V>[] initializers;
            bool valueIsValueType;

            public ValueTypeFieldMemberInitializer(FieldInfo field, Initializer[] initializers)
            {
                this.field = field;
                this.initializers = initializers.Select(i => (Initializer<V>)i).ToArray();
                this.valueIsValueType = field.FieldType.IsValueType;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                object boxedInstance = instance;
                V value = (V)field.GetValue(boxedInstance);
                for (int i = 0, n = this.initializers.Length; i < n; i++)
                {
                    this.initializers[i].Init(state, ref value);
                }
                if (this.valueIsValueType)
                    field.SetValue(boxedInstance, value);
                instance = (T)boxedInstance;
            }
        }

        public class PropertyMemberInitializer<T, V> : Initializer<T>
            where T : class
        {
            Func<T, V> fnGetter;
            Initializer<V>[] initializers;

            public PropertyMemberInitializer(PropertyInfo property, Initializer[] initializers)
            {
                this.fnGetter = (Func<T, V>)Delegate.CreateDelegate(typeof(Func<T, V>), property.GetGetMethod(true));
                this.initializers = initializers.Select(i => (Initializer<V>)i).ToArray();
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                V value = this.fnGetter(instance);
                for (int i = 0, n = this.initializers.Length; i < n; i++)
                {
                    this.initializers[i].Init(state, ref value);
                }
            }
        }

        public class ValueTypePropertyMemberInitializer<T, V> : Initializer<T>
            where T : struct
        {
            delegate V Getter(ref T instance);
            Getter fnGetter;
            Initializer<V>[] initializers;

            public ValueTypePropertyMemberInitializer(PropertyInfo property, Initializer[] initializers)
            {
                this.fnGetter = (Getter)Delegate.CreateDelegate(typeof(Getter), property.GetGetMethod(true));
                this.initializers = initializers.Select(i => (Initializer<V>)i).ToArray();
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                V value = this.fnGetter(ref instance);
                for (int i = 0, n = this.initializers.Length; i < n; i++)
                {
                    this.initializers[i].Init(state, ref value);
                }
            }
        }

        public class ElementInitializer<T> : Initializer<T>
            where T : class
        {
            MethodInfo addMethod;
            Evaluator[] evArgs;

            public ElementInitializer(MethodInfo addMethod, Evaluator[] evArgs)
            {
                this.addMethod = addMethod;
                this.evArgs = evArgs;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                try
                {
                    var args = evArgs.Select(a => a.EvalBoxed(state)).ToArray();
                    addMethod.Invoke(instance, args);
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }
        }

        public class ElementInitializer<T, A> : Initializer<T>
            where T : class
        {
            Action<T, A> fnAdder;
            Evaluator<A> evArg;

            public ElementInitializer(MethodInfo addMethod, Evaluator<A> evArg)
            {
                this.fnAdder = (Action<T, A>)Delegate.CreateDelegate(typeof(Action<T, A>), addMethod);
                this.evArg = evArg;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                A arg = this.evArg.Eval(state);
                this.fnAdder(instance, arg);
            }
        }

        public class ElementInitializer<T, A1, A2> : Initializer<T>
            where T : class
        {
            Action<T, A1, A2> fnAdder;
            Evaluator<A1> evArg1;
            Evaluator<A2> evArg2;

            public ElementInitializer(MethodInfo addMethod, Evaluator<A1> evArg1, Evaluator<A2> evArg2)
            {
                this.fnAdder = (Action<T, A1, A2>)Delegate.CreateDelegate(typeof(Action<T, A1, A2>), addMethod);
                this.evArg1 = evArg1;
                this.evArg2 = evArg2;
            }

            public override void Init(EvaluatorState state, ref T instance)
            {
                A1 arg1 = this.evArg1.Eval(state);
                A2 arg2 = this.evArg2.Eval(state);
                this.fnAdder(instance, arg1, arg2);
            }
        }

        public static class Operators
        {
            public static SByte Add(SByte left, SByte right) { return (SByte)(left + right); }
            public static Int16 Add(Int16 left, Int16 right) { return (Int16)(left + right); }
            public static Int32 Add(Int32 left, Int32 right) { return (Int32)(left + right); }
            public static Int64 Add(Int64 left, Int64 right) { return (Int64)(left + right); }
            public static Byte Add(Byte left, Byte right) { return (Byte)(left + right); }
            public static UInt16 Add(UInt16 left, UInt16 right) { return (UInt16)(left + right); }
            public static UInt32 Add(UInt32 left, UInt32 right) { return (UInt32)(left + right); }
            public static UInt64 Add(UInt64 left, UInt64 right) { return (UInt64)(left + right); }
            public static Single Add(Single left, Single right) { return (Single)(left + right); }
            public static Double Add(Double left, Double right) { return (Double)(left + right); }
            public static Decimal Add(Decimal left, Decimal right) { return (Decimal)(left + right); }
            public static SByte AddChecked(SByte left, SByte right) { return checked((SByte)(left + right)); }
            public static Int16 AddChecked(Int16 left, Int16 right) { return checked((Int16)(left + right)); }
            public static Int32 AddChecked(Int32 left, Int32 right) { return checked((Int32)(left + right)); }
            public static Int64 AddChecked(Int64 left, Int64 right) { return checked((Int64)(left + right)); }
            public static Byte AddChecked(Byte left, Byte right) { return checked((Byte)(left + right)); }
            public static UInt16 AddChecked(UInt16 left, UInt16 right) { return checked((UInt16)(left + right)); }
            public static UInt32 AddChecked(UInt32 left, UInt32 right) { return checked((UInt32)(left + right)); }
            public static UInt64 AddChecked(UInt64 left, UInt64 right) { return checked((UInt64)(left + right)); }
            public static Single AddChecked(Single left, Single right) { return checked((Single)(left + right)); }
            public static Double AddChecked(Double left, Double right) { return checked((Double)(left + right)); }
            public static Decimal AddChecked(Decimal left, Decimal right) { return checked((Decimal)(left + right)); }

            public static SByte Subtract(SByte left, SByte right) { return (SByte)(left - right); }
            public static Int16 Subtract(Int16 left, Int16 right) { return (Int16)(left - right); }
            public static Int32 Subtract(Int32 left, Int32 right) { return (Int32)(left - right); }
            public static Int64 Subtract(Int64 left, Int64 right) { return (Int64)(left - right); }
            public static Byte Subtract(Byte left, Byte right) { return (Byte)(left - right); }
            public static UInt16 Subtract(UInt16 left, UInt16 right) { return (UInt16)(left - right); }
            public static UInt32 Subtract(UInt32 left, UInt32 right) { return (UInt32)(left - right); }
            public static UInt64 Subtract(UInt64 left, UInt64 right) { return (UInt64)(left - right); }
            public static Single Subtract(Single left, Single right) { return (Single)(left - right); }
            public static Double Subtract(Double left, Double right) { return (Double)(left - right); }
            public static Decimal Subtract(Decimal left, Decimal right) { return (Decimal)(left - right); }
            public static SByte SubtractChecked(SByte left, SByte right) { return checked((SByte)(left - right)); }
            public static Int16 SubtractChecked(Int16 left, Int16 right) { return checked((Int16)(left - right)); }
            public static Int32 SubtractChecked(Int32 left, Int32 right) { return checked((Int32)(left - right)); }
            public static Int64 SubtractChecked(Int64 left, Int64 right) { return checked((Int64)(left - right)); }
            public static Byte SubtractChecked(Byte left, Byte right) { return checked((Byte)(left - right)); }
            public static UInt16 SubtractChecked(UInt16 left, UInt16 right) { return checked((UInt16)(left - right)); }
            public static UInt32 SubtractChecked(UInt32 left, UInt32 right) { return checked((UInt32)(left - right)); }
            public static UInt64 SubtractChecked(UInt64 left, UInt64 right) { return checked((UInt64)(left - right)); }
            public static Single SubtractChecked(Single left, Single right) { return checked((Single)(left - right)); }
            public static Double SubtractChecked(Double left, Double right) { return checked((Double)(left - right)); }
            public static Decimal SubtractChecked(Decimal left, Decimal right) { return checked((Decimal)(left - right)); }

            public static SByte Multiply(SByte left, SByte right) { return (SByte)(left * right); }
            public static Int16 Multiply(Int16 left, Int16 right) { return (Int16)(left * right); }
            public static Int32 Multiply(Int32 left, Int32 right) { return (Int32)(left * right); }
            public static Int64 Multiply(Int64 left, Int64 right) { return (Int64)(left * right); }
            public static Byte Multiply(Byte left, Byte right) { return (Byte)(left * right); }
            public static UInt16 Multiply(UInt16 left, UInt16 right) { return (UInt16)(left * right); }
            public static UInt32 Multiply(UInt32 left, UInt32 right) { return (UInt32)(left * right); }
            public static UInt64 Multiply(UInt64 left, UInt64 right) { return (UInt64)(left * right); }
            public static Single Multiply(Single left, Single right) { return (Single)(left * right); }
            public static Double Multiply(Double left, Double right) { return (Double)(left * right); }
            public static Decimal Multiply(Decimal left, Decimal right) { return (Decimal)(left * right); }
            public static SByte MultiplyChecked(SByte left, SByte right) { return checked((SByte)(left * right)); }
            public static Int16 MultiplyChecked(Int16 left, Int16 right) { return checked((Int16)(left * right)); }
            public static Int32 MultiplyChecked(Int32 left, Int32 right) { return checked((Int32)(left * right)); }
            public static Int64 MultiplyChecked(Int64 left, Int64 right) { return checked((Int64)(left * right)); }
            public static Byte MultiplyChecked(Byte left, Byte right) { return checked((Byte)(left * right)); }
            public static UInt16 MultiplyChecked(UInt16 left, UInt16 right) { return checked((UInt16)(left * right)); }
            public static UInt32 MultiplyChecked(UInt32 left, UInt32 right) { return checked((UInt32)(left * right)); }
            public static UInt64 MultiplyChecked(UInt64 left, UInt64 right) { return checked((UInt64)(left * right)); }
            public static Single MultiplyChecked(Single left, Single right) { return checked((Single)(left * right)); }
            public static Double MultiplyChecked(Double left, Double right) { return checked((Double)(left * right)); }
            public static Decimal MultiplyChecked(Decimal left, Decimal right) { return checked((Decimal)(left * right)); }

            public static SByte Divide(SByte left, SByte right) { return (SByte)(left / right); }
            public static Int16 Divide(Int16 left, Int16 right) { return (Int16)(left / right); }
            public static Int32 Divide(Int32 left, Int32 right) { return (Int32)(left / right); }
            public static Int64 Divide(Int64 left, Int64 right) { return (Int64)(left / right); }
            public static Byte Divide(Byte left, Byte right) { return (Byte)(left / right); }
            public static UInt16 Divide(UInt16 left, UInt16 right) { return (UInt16)(left / right); }
            public static UInt32 Divide(UInt32 left, UInt32 right) { return (UInt32)(left / right); }
            public static UInt64 Divide(UInt64 left, UInt64 right) { return (UInt64)(left / right); }
            public static Single Divide(Single left, Single right) { return (Single)(left / right); }
            public static Double Divide(Double left, Double right) { return (Double)(left / right); }
            public static Decimal Divide(Decimal left, Decimal right) { return (Decimal)(left / right); }

            public static SByte Modulo(SByte left, SByte right) { return (SByte)(left % right); }
            public static Int16 Modulo(Int16 left, Int16 right) { return (Int16)(left % right); }
            public static Int32 Modulo(Int32 left, Int32 right) { return (Int32)(left % right); }
            public static Int64 Modulo(Int64 left, Int64 right) { return (Int64)(left % right); }
            public static Byte Modulo(Byte left, Byte right) { return (Byte)(left % right); }
            public static UInt16 Modulo(UInt16 left, UInt16 right) { return (UInt16)(left % right); }
            public static UInt32 Modulo(UInt32 left, UInt32 right) { return (UInt32)(left % right); }
            public static UInt64 Modulo(UInt64 left, UInt64 right) { return (UInt64)(left % right); }
            public static Single Modulo(Single left, Single right) { return (Single)(left % right); }
            public static Double Modulo(Double left, Double right) { return (Double)(left % right); }
            public static Decimal Modulo(Decimal left, Decimal right) { return (Decimal)(left % right); }

            public static SByte Power(SByte left, SByte right) { return (SByte)Math.Pow(left, right); }
            public static Int16 Power(Int16 left, Int16 right) { return (Int16)Math.Pow(left, right); }
            public static Int32 Power(Int32 left, Int32 right) { return (Int32)Math.Pow(left, right); }
            public static Int64 Power(Int64 left, Int64 right) { return (Int64)Math.Pow(left, right); }
            public static Byte Power(Byte left, Byte right) { return (Byte)Math.Pow(left, right); }
            public static UInt16 Power(UInt16 left, UInt16 right) { return (UInt16)Math.Pow(left, right); }
            public static UInt32 Power(UInt32 left, UInt32 right) { return (UInt32)Math.Pow(left, right); }
            public static UInt64 Power(UInt64 left, UInt64 right) { return (UInt64)Math.Pow(left, right); }
            public static Single Power(Single left, Single right) { return (Single)Math.Pow(left, right); }
            public static Double Power(Double left, Double right) { return (Double)Math.Pow(left, right); }
            public static Decimal Power(Decimal left, Decimal right) { return (Decimal)Math.Pow((double)left, (double)right); }

            public static SByte LeftShift(SByte left, SByte right) { return (SByte)(left << right); }
            public static Int16 LeftShift(Int16 left, Int16 right) { return (Int16)(left << right); }
            public static Int32 LeftShift(Int32 left, Int32 right) { return (Int32)(left << right); }
            public static Int64 LeftShift(Int64 left, Int64 right) { return (Int64)(left << (int)right); }
            public static Byte LeftShift(Byte left, Byte right) { return (Byte)(left << right); }
            public static UInt16 LeftShift(UInt16 left, UInt16 right) { return (UInt16)(left << right); }
            public static UInt32 LeftShift(UInt32 left, UInt32 right) { return (UInt32)(left << (int)right); }
            public static UInt64 LeftShift(UInt64 left, UInt64 right) { return (UInt64)(left << (int)right); }

            public static SByte RightShift(SByte left, SByte right) { return (SByte)(left >> right); }
            public static Int16 RightShift(Int16 left, Int16 right) { return (Int16)(left >> right); }
            public static Int32 RightShift(Int32 left, Int32 right) { return (Int32)(left >> right); }
            public static Int64 RightShift(Int64 left, Int64 right) { return (Int64)(left >> (int)right); }
            public static Byte RightShift(Byte left, Byte right) { return (Byte)(left >> right); }
            public static UInt16 RightShift(UInt16 left, UInt16 right) { return (UInt16)(left >> right); }
            public static UInt32 RightShift(UInt32 left, UInt32 right) { return (UInt32)(left >> (int)right); }
            public static UInt64 RightShift(UInt64 left, UInt64 right) { return (UInt64)(left >> (int)right); }

            public static Boolean And(Boolean left, Boolean right) { return left && right; }
            public static SByte And(SByte left, SByte right) { return (SByte)(left & right); }
            public static Int16 And(Int16 left, Int16 right) { return (Int16)(left & right); }
            public static Int32 And(Int32 left, Int32 right) { return (Int32)(left & right); }
            public static Int64 And(Int64 left, Int64 right) { return (Int64)(left & right); }
            public static Byte And(Byte left, Byte right) { return (Byte)(left & right); }
            public static UInt16 And(UInt16 left, UInt16 right) { return (UInt16)(left & right); }
            public static UInt32 And(UInt32 left, UInt32 right) { return (UInt32)(left & right); }
            public static UInt64 And(UInt64 left, UInt64 right) { return (UInt64)(left & right); }

            public static Boolean Or(Boolean left, Boolean right) { return left || right; }
            public static SByte Or(SByte left, SByte right) { return (SByte)(left | right); }
            public static Int16 Or(Int16 left, Int16 right) { return (Int16)(left | right); }
            public static Int32 Or(Int32 left, Int32 right) { return (Int32)(left | right); }
            public static Int64 Or(Int64 left, Int64 right) { return (Int64)(left | right); }
            public static Byte Or(Byte left, Byte right) { return (Byte)(left | right); }
            public static UInt16 Or(UInt16 left, UInt16 right) { return (UInt16)(left | right); }
            public static UInt32 Or(UInt32 left, UInt32 right) { return (UInt32)(left | right); }
            public static UInt64 Or(UInt64 left, UInt64 right) { return (UInt64)(left | right); }

            public static Boolean ExclusiveOr(Boolean left, Boolean right) { return left ^ right; }
            public static SByte ExclusiveOr(SByte left, SByte right) { return (SByte)(left ^ right); }
            public static Int16 ExclusiveOr(Int16 left, Int16 right) { return (Int16)(left ^ right); }
            public static Int32 ExclusiveOr(Int32 left, Int32 right) { return (Int32)(left ^ right); }
            public static Int64 ExclusiveOr(Int64 left, Int64 right) { return (Int64)(left ^ right); }
            public static Byte ExclusiveOr(Byte left, Byte right) { return (Byte)(left ^ right); }
            public static UInt16 ExclusiveOr(UInt16 left, UInt16 right) { return (UInt16)(left ^ right); }
            public static UInt32 ExclusiveOr(UInt32 left, UInt32 right) { return (UInt32)(left ^ right); }
            public static UInt64 ExclusiveOr(UInt64 left, UInt64 right) { return (UInt64)(left ^ right); }

            public static Boolean LessThan(SByte left, SByte right) { return left < right; }
            public static Boolean LessThan(Int16 left, Int16 right) { return left < right; }
            public static Boolean LessThan(Int32 left, Int32 right) { return left < right; }
            public static Boolean LessThan(Int64 left, Int64 right) { return left < right; }
            public static Boolean LessThan(Byte left, Byte right) { return left < right; }
            public static Boolean LessThan(UInt16 left, UInt16 right) { return left < right; }
            public static Boolean LessThan(UInt32 left, UInt32 right) { return left < right; }
            public static Boolean LessThan(UInt64 left, UInt64 right) { return left < right; }
            public static Boolean LessThan(Single left, Single right) { return left < right; }
            public static Boolean LessThan(Double left, Double right) { return left < right; }
            public static Boolean LessThan(Decimal left, Decimal right) { return left < right; }

            public static Boolean LessThanOrEqual(SByte left, SByte right) { return left <= right; }
            public static Boolean LessThanOrEqual(Int16 left, Int16 right) { return left <= right; }
            public static Boolean LessThanOrEqual(Int32 left, Int32 right) { return left <= right; }
            public static Boolean LessThanOrEqual(Int64 left, Int64 right) { return left <= right; }
            public static Boolean LessThanOrEqual(Byte left, Byte right) { return left <= right; }
            public static Boolean LessThanOrEqual(UInt16 left, UInt16 right) { return left <= right; }
            public static Boolean LessThanOrEqual(UInt32 left, UInt32 right) { return left <= right; }
            public static Boolean LessThanOrEqual(UInt64 left, UInt64 right) { return left <= right; }
            public static Boolean LessThanOrEqual(Single left, Single right) { return left <= right; }
            public static Boolean LessThanOrEqual(Double left, Double right) { return left <= right; }
            public static Boolean LessThanOrEqual(Decimal left, Decimal right) { return left <= right; }

            public static Boolean GreaterThan(SByte left, SByte right) { return left > right; }
            public static Boolean GreaterThan(Int16 left, Int16 right) { return left > right; }
            public static Boolean GreaterThan(Int32 left, Int32 right) { return left > right; }
            public static Boolean GreaterThan(Int64 left, Int64 right) { return left > right; }
            public static Boolean GreaterThan(Byte left, Byte right) { return left > right; }
            public static Boolean GreaterThan(UInt16 left, UInt16 right) { return left > right; }
            public static Boolean GreaterThan(UInt32 left, UInt32 right) { return left > right; }
            public static Boolean GreaterThan(UInt64 left, UInt64 right) { return left > right; }
            public static Boolean GreaterThan(Single left, Single right) { return left > right; }
            public static Boolean GreaterThan(Double left, Double right) { return left > right; }
            public static Boolean GreaterThan(Decimal left, Decimal right) { return left > right; }

            public static Boolean GreaterThanOrEqual(SByte left, SByte right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(Int16 left, Int16 right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(Int32 left, Int32 right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(Int64 left, Int64 right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(Byte left, Byte right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(UInt16 left, UInt16 right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(UInt32 left, UInt32 right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(UInt64 left, UInt64 right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(Single left, Single right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(Double left, Double right) { return left >= right; }
            public static Boolean GreaterThanOrEqual(Decimal left, Decimal right) { return left >= right; }

            public static Boolean Equal(SByte left, SByte right) { return left == right; }
            public static Boolean Equal(Int16 left, Int16 right) { return left == right; }
            public static Boolean Equal(Int32 left, Int32 right) { return left == right; }
            public static Boolean Equal(Int64 left, Int64 right) { return left == right; }
            public static Boolean Equal(Byte left, Byte right) { return left == right; }
            public static Boolean Equal(UInt16 left, UInt16 right) { return left == right; }
            public static Boolean Equal(UInt32 left, UInt32 right) { return left == right; }
            public static Boolean Equal(UInt64 left, UInt64 right) { return left == right; }
            public static Boolean Equal(Single left, Single right) { return left == right; }
            public static Boolean Equal(Double left, Double right) { return left == right; }
            public static Boolean Equal(Decimal left, Decimal right) { return left == right; }
            public static Boolean Equal(object left, object right) { return left == right; }

            public static Boolean NotEqual(SByte left, SByte right) { return left != right; }
            public static Boolean NotEqual(Int16 left, Int16 right) { return left != right; }
            public static Boolean NotEqual(Int32 left, Int32 right) { return left != right; }
            public static Boolean NotEqual(Int64 left, Int64 right) { return left != right; }
            public static Boolean NotEqual(Byte left, Byte right) { return left != right; }
            public static Boolean NotEqual(UInt16 left, UInt16 right) { return left != right; }
            public static Boolean NotEqual(UInt32 left, UInt32 right) { return left != right; }
            public static Boolean NotEqual(UInt64 left, UInt64 right) { return left != right; }
            public static Boolean NotEqual(Single left, Single right) { return left != right; }
            public static Boolean NotEqual(Double left, Double right) { return left != right; }
            public static Boolean NotEqual(Decimal left, Decimal right) { return left != right; }
            public static Boolean NotEqual(object left, object right) { return left != right; }

            public static SByte Negate(SByte operand) { return (SByte)(-operand); }
            public static Int16 Negate(Int16 operand) { return (Int16)(-operand); }
            public static Int32 Negate(Int32 operand) { return (Int32)(-operand); }
            public static Int64 Negate(Int64 operand) { return (Int64)(-operand); }
            public static Byte Negate(Byte operand) { return (Byte)(-operand); }
            public static UInt16 Negate(UInt16 operand) { return (UInt16)(-operand); }
            public static UInt32 Negate(UInt32 operand) { return (UInt32)(-operand); }
            public static UInt64 Negate(UInt64 operand) { return (UInt64)(-(Int64)operand); }
            public static Single Negate(Single operand) { return (Single)(-operand); }
            public static Double Negate(Double operand) { return (Double)(-operand); }
            public static Decimal Negate(Decimal operand) { return (Decimal)(-operand); }
            public static SByte NegateChecked(SByte operand) { return checked((SByte)(-operand)); }
            public static Int16 NegateChecked(Int16 operand) { return checked((Int16)(-operand)); }
            public static Int32 NegateChecked(Int32 operand) { return checked((Int32)(-operand)); }
            public static Int64 NegateChecked(Int64 operand) { return checked((Int64)(-operand)); }
            public static Single NegateChecked(Single operand) { return checked((Single)(-operand)); }
            public static Double NegateChecked(Double operand) { return checked((Double)(-operand)); }
            public static Decimal NegateChecked(Decimal operand) { return checked((Decimal)(-operand)); }

            public static bool Not(bool operand) { return !operand; }
            public static SByte Not(SByte operand) { return (SByte)~operand; }
            public static Int16 Not(Int16 operand) { return (Int16)~operand; }
            public static Int32 Not(Int32 operand) { return (Int32)~operand; }
            public static Int64 Not(Int64 operand) { return (Int64)~operand; }
            public static Byte Not(Byte operand) { return (Byte)~operand; }
            public static UInt16 Not(UInt16 operand) { return (UInt16)~operand; }
            public static UInt32 Not(UInt32 operand) { return (UInt32)~operand; }
            public static UInt64 Not(UInt64 operand) { return (UInt64)~operand; }

            public static SByte ConvertToSByte(SByte operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(Int16 operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(Int32 operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(Int64 operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(Byte operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(UInt16 operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(UInt32 operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(UInt64 operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(Single operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(Double operand) { return (SByte)operand; }
            public static SByte ConvertToSByte(Decimal operand) { return (SByte)operand; }
            public static SByte ConvertCheckedToSByte(SByte operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(Int16 operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(Int32 operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(Int64 operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(Byte operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(UInt16 operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(UInt32 operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(UInt64 operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(Single operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(Double operand) { return checked((SByte)operand); }
            public static SByte ConvertCheckedToSByte(Decimal operand) { return checked((SByte)operand); }

            public static Int16 ConvertToInt16(SByte operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(Int16 operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(Int32 operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(Int64 operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(Byte operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(UInt16 operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(UInt32 operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(UInt64 operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(Single operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(Double operand) { return (Int16)operand; }
            public static Int16 ConvertToInt16(Decimal operand) { return (Int16)operand; }
            public static Int16 ConvertCheckedToInt16(SByte operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(Int16 operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(Int32 operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(Int64 operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(Byte operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(UInt16 operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(UInt32 operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(UInt64 operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(Single operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(Double operand) { return checked((Int16)operand); }
            public static Int16 ConvertCheckedToInt16(Decimal operand) { return checked((Int16)operand); }

            public static Int32 ConvertToInt32(SByte operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(Int16 operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(Int32 operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(Int64 operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(Byte operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(UInt16 operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(UInt32 operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(UInt64 operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(Single operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(Double operand) { return (Int32)operand; }
            public static Int32 ConvertToInt32(Decimal operand) { return (Int32)operand; }
            public static Int32 ConvertCheckedToInt32(SByte operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(Int16 operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(Int32 operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(Int64 operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(Byte operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(UInt16 operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(UInt32 operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(UInt64 operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(Single operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(Double operand) { return checked((Int32)operand); }
            public static Int32 ConvertCheckedToInt32(Decimal operand) { return checked((Int32)operand); }

            public static Int64 ConvertToInt64(SByte operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(Int16 operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(Int32 operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(Int64 operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(Byte operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(UInt16 operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(UInt32 operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(UInt64 operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(Single operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(Double operand) { return (Int64)operand; }
            public static Int64 ConvertToInt64(Decimal operand) { return (Int64)operand; }
            public static Int64 ConvertCheckedToInt64(SByte operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(Int16 operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(Int32 operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(Int64 operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(Byte operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(UInt16 operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(UInt32 operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(UInt64 operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(Single operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(Double operand) { return checked((Int64)operand); }
            public static Int64 ConvertCheckedToInt64(Decimal operand) { return checked((Int64)operand); }

            public static Byte ConvertToByte(SByte operand) { return (Byte)operand; }
            public static Byte ConvertToByte(Int16 operand) { return (Byte)operand; }
            public static Byte ConvertToByte(Int32 operand) { return (Byte)operand; }
            public static Byte ConvertToByte(Int64 operand) { return (Byte)operand; }
            public static Byte ConvertToByte(Byte operand) { return (Byte)operand; }
            public static Byte ConvertToByte(UInt16 operand) { return (Byte)operand; }
            public static Byte ConvertToByte(UInt32 operand) { return (Byte)operand; }
            public static Byte ConvertToByte(UInt64 operand) { return (Byte)operand; }
            public static Byte ConvertToByte(Single operand) { return (Byte)operand; }
            public static Byte ConvertToByte(Double operand) { return (Byte)operand; }
            public static Byte ConvertToByte(Decimal operand) { return (Byte)operand; }
            public static Byte ConvertCheckedToByte(SByte operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(Int16 operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(Int32 operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(Int64 operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(Byte operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(UInt16 operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(UInt32 operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(UInt64 operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(Single operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(Double operand) { return checked((Byte)operand); }
            public static Byte ConvertCheckedToByte(Decimal operand) { return checked((Byte)operand); }

            public static UInt16 ConvertToUInt16(SByte operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(Int16 operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(Int32 operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(Int64 operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(Byte operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(UInt16 operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(UInt32 operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(UInt64 operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(Single operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(Double operand) { return (UInt16)operand; }
            public static UInt16 ConvertToUInt16(Decimal operand) { return (UInt16)operand; }
            public static UInt16 ConvertCheckedToUInt16(SByte operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(Int16 operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(Int32 operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(Int64 operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(Byte operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(UInt16 operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(UInt32 operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(UInt64 operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(Single operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(Double operand) { return checked((UInt16)operand); }
            public static UInt16 ConvertCheckedToUInt16(Decimal operand) { return checked((UInt16)operand); }

            public static UInt32 ConvertToUInt32(SByte operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(Int16 operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(Int32 operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(Int64 operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(Byte operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(UInt16 operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(UInt32 operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(UInt64 operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(Single operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(Double operand) { return (UInt32)operand; }
            public static UInt32 ConvertToUInt32(Decimal operand) { return (UInt32)operand; }
            public static UInt32 ConvertCheckedToUInt32(SByte operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(Int16 operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(Int32 operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(Int64 operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(Byte operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(UInt16 operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(UInt32 operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(UInt64 operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(Single operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(Double operand) { return checked((UInt32)operand); }
            public static UInt32 ConvertCheckedToUInt32(Decimal operand) { return checked((UInt32)operand); }

            public static UInt64 ConvertToUInt64(SByte operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(Int16 operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(Int32 operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(Int64 operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(Byte operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(UInt16 operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(UInt32 operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(UInt64 operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(Single operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(Double operand) { return (UInt64)operand; }
            public static UInt64 ConvertToUInt64(Decimal operand) { return (UInt64)operand; }
            public static UInt64 ConvertCheckedToUInt64(SByte operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(Int16 operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(Int32 operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(Int64 operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(Byte operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(UInt16 operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(UInt32 operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(UInt64 operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(Single operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(Double operand) { return checked((UInt64)operand); }
            public static UInt64 ConvertCheckedToUInt64(Decimal operand) { return checked((UInt64)operand); }

            public static Single ConvertToSingle(SByte operand) { return (Single)operand; }
            public static Single ConvertToSingle(Int16 operand) { return (Single)operand; }
            public static Single ConvertToSingle(Int32 operand) { return (Single)operand; }
            public static Single ConvertToSingle(Int64 operand) { return (Single)operand; }
            public static Single ConvertToSingle(Byte operand) { return (Single)operand; }
            public static Single ConvertToSingle(UInt16 operand) { return (Single)operand; }
            public static Single ConvertToSingle(UInt32 operand) { return (Single)operand; }
            public static Single ConvertToSingle(UInt64 operand) { return (Single)operand; }
            public static Single ConvertToSingle(Single operand) { return (Single)operand; }
            public static Single ConvertToSingle(Double operand) { return (Single)operand; }
            public static Single ConvertToSingle(Decimal operand) { return (Single)operand; }
            public static Single ConvertCheckedToSingle(SByte operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(Int16 operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(Int32 operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(Int64 operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(Byte operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(UInt16 operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(UInt32 operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(UInt64 operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(Single operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(Double operand) { return checked((Single)operand); }
            public static Single ConvertCheckedToSingle(Decimal operand) { return checked((Single)operand); }

            public static Double ConvertToDouble(SByte operand) { return (Double)operand; }
            public static Double ConvertToDouble(Int16 operand) { return (Double)operand; }
            public static Double ConvertToDouble(Int32 operand) { return (Double)operand; }
            public static Double ConvertToDouble(Int64 operand) { return (Double)operand; }
            public static Double ConvertToDouble(Byte operand) { return (Double)operand; }
            public static Double ConvertToDouble(UInt16 operand) { return (Double)operand; }
            public static Double ConvertToDouble(UInt32 operand) { return (Double)operand; }
            public static Double ConvertToDouble(UInt64 operand) { return (Double)operand; }
            public static Double ConvertToDouble(Single operand) { return (Double)operand; }
            public static Double ConvertToDouble(Double operand) { return (Double)operand; }
            public static Double ConvertToDouble(Decimal operand) { return (Double)operand; }
            public static Double ConvertCheckedToDouble(SByte operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(Int16 operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(Int32 operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(Int64 operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(Byte operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(UInt16 operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(UInt32 operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(UInt64 operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(Single operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(Double operand) { return checked((Double)operand); }
            public static Double ConvertCheckedToDouble(Decimal operand) { return checked((Double)operand); }

            public static Decimal ConvertToDecimal(SByte operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(Int16 operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(Int32 operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(Int64 operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(Byte operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(UInt16 operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(UInt32 operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(UInt64 operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(Single operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(Double operand) { return (Decimal)operand; }
            public static Decimal ConvertToDecimal(Decimal operand) { return (Decimal)operand; }
            public static Decimal ConvertCheckedToDecimal(SByte operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(Int16 operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(Int32 operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(Int64 operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(Byte operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(UInt16 operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(UInt32 operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(UInt64 operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(Single operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(Double operand) { return checked((Decimal)operand); }
            public static Decimal ConvertCheckedToDecimal(Decimal operand) { return checked((Decimal)operand); }

            static Operators()
            {
                _methods = typeof(Operators).GetMethods(BindingFlags.Static | BindingFlags.Public).ToLookup(m => m.Name);
            }

            static ILookup<string, MethodInfo> _methods;

            public static IEnumerable<MethodInfo> GetOperatorMethods(string name)
            {
                return _methods[name];
            }
        }



        //public static class ExpressionEvaluator
        //{
        //    private static Func<Type, object, RuntimeMethodHandle, Delegate> fnCreateDelegate;

        //    public static D CreateDelegate<D>(Expression<D> function)
        //    {
        //        return (D) CreateDelegate(function);
        //    }

        //    public static Delegate CreateDelegate(LambdaExpression function)
        //    {
        //        Evaluator evaluator = EvaluatorBuilder.Build(null, function.Parameters, function.Body);
        //        return CreateDelegate(function.Type, null, function.Parameters.Count, evaluator);
        //    }

        //    private static Delegate CreateDelegate(Type delegateType, ConstructorInfo constructor)
        //    {
        //        if (fnCreateDelegate == null)
        //        {
        //            MethodInfo method = typeof(Delegate).GetMethod("CreateDelegate", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(Type), typeof(object), typeof(RuntimeMethodHandle) }, null);
        //            fnCreateDelegate = (Func<Type, object, RuntimeMethodHandle, Delegate>) Delegate.CreateDelegate(typeof(Func<Type, object, RuntimeMethodHandle, Delegate>), method);
        //        }
        //        return fnCreateDelegate.Invoke(delegateType, null, constructor.MethodHandle);
        //    }

        //    //public static Delegate CreateDelegate(Type delegateType, EvaluatorState outer, int nArgs, Evaluator evaluator)
        //    //{
        //    //    MethodInfo method = delegateType.GetMethod("Invoke");
        //    //    EvaluatorHost host = new EvaluatorHost(outer, nArgs, evaluator);
        //    //    return StrongDelegate.CreateDelegate(delegateType, new Func<object[], object>(host, (IntPtr) this.Eval));
        //    //}

        //     public static Delegate CreateDelegate(Type delegateType, EvaluatorState outer, int nArgs, Evaluator evaluator)
        //    {
        //        MethodInfo miInvoke = delegateType.GetMethod("Invoke");
        //        var host = new EvaluatorHost(outer, nArgs, evaluator);
        //        return StrongDelegate.CreateDelegate(delegateType, host.Eval);
        //    }

        //    public static object Eval(Expression expression)
        //    {
        //        LambdaExpression expression2 = expression as LambdaExpression;
        //        if ((expression2 == null) || (expression2.Parameters.Count != 0))
        //        {
        //            throw new InvalidOperationException("Wrong number of arguments specified");
        //        }
        //        expression = expression2.Body;
        //        return EvaluatorBuilder.Build(null, null, expression).EvalBoxed(new EvaluatorState(null, null));
        //    }

        //    public static object Eval(LambdaExpression function, params object[] args)
        //    {
        //        if (function.Parameters.Count != args.Length)
        //        {
        //            throw new InvalidOperationException("Wrong number of arguments specified");
        //        }
        //        return EvaluatorBuilder.Build(null, function.Parameters, function.Body).EvalBoxed(new EvaluatorState(null, args));
        //    }

        //    public abstract class Address
        //    {
        //        protected Address()
        //        {
        //        }

        //        public abstract object GetBoxedValue(ExpressionEvaluator.EvaluatorState state);
        //        public abstract void SetBoxedValue(ExpressionEvaluator.EvaluatorState state, object value);
        //    }

        //    public abstract class Address<T> : ExpressionEvaluator.Address
        //    {
        //        protected Address()
        //        {
        //        }

        //        public override object GetBoxedValue(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.GetValue(state);
        //        }

        //        public abstract T GetValue(ExpressionEvaluator.EvaluatorState state);
        //        public override void SetBoxedValue(ExpressionEvaluator.EvaluatorState state, object value)
        //        {
        //            this.SetValue(state, (T) value);
        //        }

        //        public abstract void SetValue(ExpressionEvaluator.EvaluatorState state, T value);
        //    }

        //    public class AndAlsoEvaluator : ExpressionEvaluator.Evaluator<bool>
        //    {
        //        private ExpressionEvaluator.Evaluator<bool> left;
        //        private ExpressionEvaluator.Evaluator<bool> right;

        //        public AndAlsoEvaluator(ExpressionEvaluator.Evaluator<bool> left, ExpressionEvaluator.Evaluator<bool> right)
        //        {
        //            this.left = left;
        //            this.right = right;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return (!this.left.Eval(state) ? false : this.right.Eval(state));
        //        }
        //    }

        //    public class ArrayIndexEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private ExpressionEvaluator.Evaluator<T[]> opArray;
        //        private ExpressionEvaluator.Evaluator<int> opIndex;

        //        public ArrayIndexEvaluator(ExpressionEvaluator.Evaluator<T[]> opArray, ExpressionEvaluator.Evaluator<int> opIndex)
        //        {
        //            this.opArray = opArray;
        //            this.opIndex = opIndex;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.opArray.Eval(state)[this.opIndex.Eval(state)];
        //        }

        //        public override ExpressionEvaluator.Address<T> EvalAddress(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return new ArrayElementAddress<T>(this.opArray.Eval(state), this.opIndex.Eval(state));
        //        }

        //        private class ArrayElementAddress : ExpressionEvaluator.Address<T>
        //        {
        //            private T[] array;
        //            private int index;

        //            public ArrayElementAddress(T[] array, int index)
        //            {
        //                this.array = array;
        //                this.index = index;
        //            }

        //            public override T GetValue(ExpressionEvaluator.EvaluatorState state)
        //            {
        //                return this.array[this.index];
        //            }

        //            public override void SetValue(ExpressionEvaluator.EvaluatorState state, T value)
        //            {
        //                this.array[this.index] = value;
        //            }
        //        }
        //    }

        //    public class ArrayLengthEvaluator<T> : ExpressionEvaluator.Evaluator<int>
        //    {
        //        private ExpressionEvaluator.Evaluator<T[]> opArray;

        //        public ArrayLengthEvaluator(ExpressionEvaluator.Evaluator<T[]> opArray)
        //        {
        //            this.opArray = opArray;
        //        }

        //        public override int Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.opArray.Eval(state).Length;
        //        }
        //    }

        //    public class BinaryEvaluator<S, T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private ExpressionEvaluator.Evaluator<S> left;
        //        private Func<S, S, T> op;
        //        private ExpressionEvaluator.Evaluator<S> right;

        //        public BinaryEvaluator(ExpressionEvaluator.Evaluator<S> left, ExpressionEvaluator.Evaluator<S> right, Func<S, S, T> op)
        //        {
        //            this.left = left;
        //            this.right = right;
        //            this.op = op;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.op.Invoke(this.left.Eval(state), this.right.Eval(state));
        //        }
        //    }

        //    public class CoalesceLEvaluator<L, R> : ExpressionEvaluator.Evaluator<L> where R: struct
        //    {
        //        private Func<R, L> fnConversion;
        //        private ExpressionEvaluator.Evaluator<L> opLeft;
        //        private ExpressionEvaluator.Evaluator<R> opRight;

        //        public CoalesceLEvaluator(ExpressionEvaluator.Evaluator<L> opLeft, ExpressionEvaluator.Evaluator<R> opRight, Func<R, L> fnConversion)
        //        {
        //            this.opLeft = opLeft;
        //            this.opRight = opRight;
        //            this.fnConversion = fnConversion;
        //        }

        //        public override L Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            L local = this.opLeft.Eval(state);
        //            if (local != null)
        //            {
        //                return local;
        //            }
        //            return this.fnConversion.Invoke(this.opRight.Eval(state));
        //        }
        //    }

        //    public class CoalesceREvaluator<L, R> : ExpressionEvaluator.Evaluator<R>
        //    {
        //        private Func<L, R> fnConversion;
        //        private ExpressionEvaluator.Evaluator<L> opLeft;
        //        private ExpressionEvaluator.Evaluator<R> opRight;

        //        public CoalesceREvaluator(ExpressionEvaluator.Evaluator<L> opLeft, ExpressionEvaluator.Evaluator<R> opRight, Func<L, R> fnConversion)
        //        {
        //            this.opLeft = opLeft;
        //            this.opRight = opRight;
        //            this.fnConversion = fnConversion;
        //        }

        //        public override R Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            L local = this.opLeft.Eval(state);
        //            if (local != null)
        //            {
        //                return this.fnConversion.Invoke(local);
        //            }
        //            return this.opRight.Eval(state);
        //        }
        //    }

        //    public class ConditionalEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private ExpressionEvaluator.Evaluator<T> ifFalse;
        //        private ExpressionEvaluator.Evaluator<T> ifTrue;
        //        private ExpressionEvaluator.Evaluator<bool> test;

        //        public ConditionalEvaluator(ExpressionEvaluator.Evaluator<bool> test, ExpressionEvaluator.Evaluator<T> ifTrue, ExpressionEvaluator.Evaluator<T> ifFalse)
        //        {
        //            this.test = test;
        //            this.ifTrue = ifTrue;
        //            this.ifFalse = ifFalse;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return (this.test.Eval(state) ? this.ifTrue.Eval(state) : this.ifFalse.Eval(state));
        //        }
        //    }

        //    public class ConstantEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private T value;

        //        public ConstantEvaluator(T value)
        //        {
        //            this.value = value;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.value;
        //        }
        //    }

        //    public class Convert<S, T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private Func<S, T> fnConvert;
        //        private ExpressionEvaluator.Evaluator<S> operand;

        //        public Convert(ExpressionEvaluator.Evaluator<S> operand) : this(operand, null)
        //        {
        //        }

        //        public Convert(ExpressionEvaluator.Evaluator<S> operand, Func<S, T> fnConvert)
        //        {
        //            this.operand = operand;
        //            this.fnConvert = fnConvert;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S local = this.operand.Eval(state);
        //            if (this.fnConvert != null)
        //            {
        //                return this.fnConvert.Invoke(local);
        //            }
        //            return (T) local;
        //        }
        //    }

        //    public class ConvertNNtoN<T> : ExpressionEvaluator.Evaluator<T?> where T: struct
        //    {
        //        private ExpressionEvaluator.Evaluator<T> operand;

        //        public ConvertNNtoN(ExpressionEvaluator.Evaluator<T> operand)
        //        {
        //            this.operand = operand;
        //        }

        //        public override T? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return new T?(this.operand.Eval(state));
        //        }
        //    }

        //    public class ConvertNNtoN<S, T> : ExpressionEvaluator.Evaluator<T?> where S: struct where T: struct
        //    {
        //        private Func<S, T> fnConvert;
        //        private ExpressionEvaluator.Evaluator<S> operand;

        //        public ConvertNNtoN(ExpressionEvaluator.Evaluator<S> operand, Func<S, T> fnConvert)
        //        {
        //            this.operand = operand;
        //            this.fnConvert = fnConvert;
        //        }

        //        public override T? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return new T?(this.fnConvert.Invoke(this.operand.Eval(state)));
        //        }
        //    }

        //    public class ConvertNtoN<S, T> : ExpressionEvaluator.Evaluator<T?> where S: struct where T: struct
        //    {
        //        private Func<S, T> fnConvert;
        //        private ExpressionEvaluator.Evaluator<S?> operand;

        //        public ConvertNtoN(ExpressionEvaluator.Evaluator<S?> operand, Func<S, T> fnConvert)
        //        {
        //            this.operand = operand;
        //            this.fnConvert = fnConvert;
        //        }

        //        public override T? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S? nullable = this.operand.Eval(state);
        //            if (!nullable.HasValue)
        //            {
        //                return null;
        //            }
        //            return new T?(this.fnConvert.Invoke(nullable.GetValueOrDefault()));
        //        }
        //    }

        //    public class ConvertNtoNN<T> : ExpressionEvaluator.Evaluator<T> where T: struct
        //    {
        //        private ExpressionEvaluator.Evaluator<T?> operand;

        //        public ConvertNtoNN(ExpressionEvaluator.Evaluator<T?> operand)
        //        {
        //            this.operand = operand;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.operand.Eval(state).Value;
        //        }
        //    }

        //    public class ConvertNtoNN<S, T> : ExpressionEvaluator.Evaluator<T> where S: struct where T: struct
        //    {
        //        private Func<S, T> fnConvert;
        //        private ExpressionEvaluator.Evaluator<S?> operand;

        //        public ConvertNtoNN(ExpressionEvaluator.Evaluator<S?> operand, Func<S, T> fnConvert)
        //        {
        //            this.operand = operand;
        //            this.fnConvert = fnConvert;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.fnConvert.Invoke(this.operand.Eval(state).Value);
        //        }
        //    }

        //    public class ElementInitializer<T> : ExpressionEvaluator.Initializer<T> where T: class
        //    {
        //        private MethodInfo addMethod;
        //        private ExpressionEvaluator.Evaluator[] evArgs;

        //        public ElementInitializer(MethodInfo addMethod, ExpressionEvaluator.Evaluator[] evArgs)
        //        {
        //            this.addMethod = addMethod;
        //            this.evArgs = evArgs;
        //        }

        //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
        //        {
        //            Func<ExpressionEvaluator.Evaluator, object> func = null;
        //            try
        //            {
        //                if (func == null)
        //                {
        //                    <>c__DisplayClass51<T> class2;
        //                    func = new Func<ExpressionEvaluator.Evaluator, object>(class2, (IntPtr) this.<Init>b__4f);
        //                }
        //                object[] parameters = Enumerable.Select<ExpressionEvaluator.Evaluator, object>(this.evArgs, func).ToArray<object>();
        //                this.addMethod.Invoke((T) instance, parameters);
        //            }
        //            catch (TargetInvocationException exception)
        //            {
        //                throw exception.InnerException;
        //            }
        //        }
        //    }

        //    public class ElementInitializer<T, A> : ExpressionEvaluator.Initializer<T> where T: class
        //    {
        //        private ExpressionEvaluator.Evaluator<A> evArg;
        //        private Action<T, A> fnAdder;

        //        public ElementInitializer(MethodInfo addMethod, ExpressionEvaluator.Evaluator<A> evArg)
        //        {
        //            this.fnAdder = (Action<T, A>) Delegate.CreateDelegate(typeof(Action<T, A>), addMethod);
        //            this.evArg = evArg;
        //        }

        //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
        //        {
        //            A local = this.evArg.Eval(state);
        //            this.fnAdder(instance, local);
        //        }
        //    }

        //    public class ElementInitializer<T, A1, A2> : ExpressionEvaluator.Initializer<T> where T: class
        //    {
        //        private ExpressionEvaluator.Evaluator<A1> evArg1;
        //        private ExpressionEvaluator.Evaluator<A2> evArg2;
        //        private Action<T, A1, A2> fnAdder;

        //        public ElementInitializer(MethodInfo addMethod, ExpressionEvaluator.Evaluator<A1> evArg1, ExpressionEvaluator.Evaluator<A2> evArg2)
        //        {
        //            this.fnAdder = (Action<T, A1, A2>) Delegate.CreateDelegate(typeof(Action<T, A1, A2>), addMethod);
        //            this.evArg1 = evArg1;
        //            this.evArg2 = evArg2;
        //        }

        //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
        //        {
        //            A1 local = this.evArg1.Eval(state);
        //            A2 local2 = this.evArg2.Eval(state);
        //            this.fnAdder(instance, local, local2);
        //        }
        //    }

        //    public abstract class Evaluator
        //    {
        //        protected Evaluator()
        //        {
        //        }

        //        public abstract ExpressionEvaluator.Address EvalAddressBoxed(ExpressionEvaluator.EvaluatorState state);
        //        public abstract object EvalBoxed(ExpressionEvaluator.EvaluatorState state);

        //        public abstract Type ReturnType { get; }
        //    }

        //    public abstract class Evaluator<T> : ExpressionEvaluator.Evaluator
        //    {
        //        protected Evaluator()
        //        {
        //        }

        //        public abstract T Eval(ExpressionEvaluator.EvaluatorState state);
        //        public virtual ExpressionEvaluator.Address<T> EvalAddress(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return new ExpressionEvaluator.ValueAddress<T>(this.Eval(state));
        //        }

        //        public override ExpressionEvaluator.Address EvalAddressBoxed(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.EvalAddress(state);
        //        }

        //        public override object EvalBoxed(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.Eval(state);
        //        }

        //        public override Type ReturnType
        //        {
        //            get
        //            {
        //                return typeof(T);
        //            }
        //        }
        //    }

        //    private class EvaluatorBuilder
        //    {
        //        private int count = -1;
        //        private ExpressionEvaluator.EvaluatorBuilder outer;
        //        private ReadOnlyCollection<ParameterExpression> parameters;

        //        private EvaluatorBuilder(ExpressionEvaluator.EvaluatorBuilder outer, List<ParameterExpression> parameters)
        //        {
        //            this.outer = outer;
        //            this.parameters = parameters.ToReadOnly<ParameterExpression>();
        //        }

        //        private ExpressionEvaluator.Evaluator Binary(BinaryExpression b)
        //        {
        //            ExpressionEvaluator.Evaluator opLeft = this.Build(b.Left);
        //            ExpressionEvaluator.Evaluator opRight = this.Build(b.Right);
        //            Type nonNullType = GetNonNullType(b.Left.Type);
        //            Type targetType = GetNonNullType(b.Type);
        //            if (b.Method != null)
        //            {
        //                return this.GetBinaryOperator(b, nonNullType, targetType, b.Method, opLeft, opRight);
        //            }
        //            switch (b.NodeType)
        //            {
        //                case ExpressionType.Add:
        //                case ExpressionType.AddChecked:
        //                case ExpressionType.And:
        //                case ExpressionType.Divide:
        //                case ExpressionType.Equal:
        //                case ExpressionType.ExclusiveOr:
        //                case ExpressionType.GreaterThan:
        //                case ExpressionType.GreaterThanOrEqual:
        //                case ExpressionType.LeftShift:
        //                case ExpressionType.LessThan:
        //                case ExpressionType.LessThanOrEqual:
        //                case ExpressionType.Modulo:
        //                case ExpressionType.Multiply:
        //                case ExpressionType.MultiplyChecked:
        //                case ExpressionType.NotEqual:
        //                case ExpressionType.Or:
        //                case ExpressionType.Power:
        //                case ExpressionType.RightShift:
        //                case ExpressionType.Subtract:
        //                case ExpressionType.SubtractChecked:
        //                {
        //                    MethodInfo method = this.FindBestMethod(ExpressionEvaluator.Operators.GetOperatorMethods(b.NodeType.ToString()), new Type[] { nonNullType, nonNullType }, targetType);
        //                    Debug.Assert(method != null);
        //                    return this.GetBinaryOperator(b, nonNullType, targetType, method, opLeft, opRight);
        //                }
        //                case ExpressionType.AndAlso:
        //                    if (!b.IsLiftedToNull)
        //                    {
        //                        if (b.IsLifted)
        //                        {
        //                            return new ExpressionEvaluator.LiftToFalseAndAlsoEvaluator((ExpressionEvaluator.Evaluator<bool?>) opLeft, (ExpressionEvaluator.Evaluator<bool?>) opRight);
        //                        }
        //                        return new ExpressionEvaluator.AndAlsoEvaluator((ExpressionEvaluator.Evaluator<bool>) opLeft, (ExpressionEvaluator.Evaluator<bool>) opRight);
        //                    }
        //                    return new ExpressionEvaluator.LiftToNullAndAlsoEvaluator((ExpressionEvaluator.Evaluator<bool?>) opLeft, (ExpressionEvaluator.Evaluator<bool?>) opRight);

        //                case ExpressionType.ArrayIndex:
        //                    return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ArrayIndexEvaluator<T><>).MakeGenericType(new Type[] { b.Left.Type.GetElementType() }), new object[] { opLeft, opRight });

        //                case ExpressionType.Coalesce:
        //                {
        //                    ParameterExpression expression2;
        //                    Delegate delegate2;
        //                    Type type = GetNonNullType(b.Right.Type);
        //                    if (b.Conversion == null)
        //                    {
        //                        if (b.Type == b.Right.Type)
        //                        {
        //                            expression2 = Expression.Parameter(b.Left.Type, "left");
        //                            delegate2 = ExpressionEvaluator.CreateDelegate(Expression.Lambda(Expression.Convert(expression2, b.Type), new ParameterExpression[] { expression2 }));
        //                            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.CoalesceREvaluator<L, R><,>).MakeGenericType(new Type[] { b.Left.Type, b.Right.Type }), new object[] { opLeft, opRight, delegate2 });
        //                        }
        //                        if (b.Type == b.Left.Type)
        //                        {
        //                            expression2 = Expression.Parameter(b.Right.Type, "right");
        //                            delegate2 = ExpressionEvaluator.CreateDelegate(Expression.Lambda(Expression.Convert(expression2, b.Type), new ParameterExpression[] { expression2 }));
        //                            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.CoalesceLEvaluator<L, R><,>).MakeGenericType(new Type[] { b.Left.Type, b.Right.Type }), new object[] { opLeft, opRight, delegate2 });
        //                        }
        //                        break;
        //                    }
        //                    LambdaExpression conversion = b.Conversion;
        //                    if (!(conversion.Body.Type == b.Type))
        //                    {
        //                        conversion = Expression.Lambda(Expression.Convert(conversion.Body, b.Type), conversion.Parameters.ToArray<ParameterExpression>());
        //                    }
        //                    if ((conversion.Parameters[0].Type != b.Left.Type) && !(conversion.Parameters[0].Type == nonNullType))
        //                    {
        //                        if ((conversion.Parameters[0].Type != b.Right.Type) && !(conversion.Parameters[0].Type == type))
        //                        {
        //                            break;
        //                        }
        //                        if (conversion.Parameters[0].Type == type)
        //                        {
        //                            expression2 = Expression.Parameter(b.Right.Type, "right");
        //                            conversion = Expression.Lambda(Expression.Invoke(conversion, new Expression[] { Expression.Convert(expression2, type) }), new ParameterExpression[] { expression2 });
        //                        }
        //                        delegate2 = ExpressionEvaluator.CreateDelegate(conversion);
        //                        return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.CoalesceLEvaluator<L, R><,>).MakeGenericType(new Type[] { b.Left.Type, b.Right.Type }), new object[] { opLeft, opRight, delegate2 });
        //                    }
        //                    if (conversion.Parameters[0].Type == nonNullType)
        //                    {
        //                        expression2 = Expression.Parameter(b.Left.Type, "left");
        //                        conversion = Expression.Lambda(Expression.Invoke(conversion, new Expression[] { Expression.Convert(expression2, nonNullType) }), new ParameterExpression[] { expression2 });
        //                    }
        //                    delegate2 = ExpressionEvaluator.CreateDelegate(conversion);
        //                    return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.CoalesceREvaluator<L, R><,>).MakeGenericType(new Type[] { b.Left.Type, b.Right.Type }), new object[] { opLeft, opRight, delegate2 });
        //                }
        //                case ExpressionType.OrElse:
        //                    if (!b.IsLiftedToNull)
        //                    {
        //                        if (b.IsLifted)
        //                        {
        //                            return new ExpressionEvaluator.LiftToFalseOrElseEvaluator((ExpressionEvaluator.Evaluator<bool?>) opLeft, (ExpressionEvaluator.Evaluator<bool?>) opRight);
        //                        }
        //                        return new ExpressionEvaluator.OrElseEvaluator((ExpressionEvaluator.Evaluator<bool>) opLeft, (ExpressionEvaluator.Evaluator<bool>) opRight);
        //                    }
        //                    return new ExpressionEvaluator.LiftToNullOrElseEvaluator((ExpressionEvaluator.Evaluator<bool?>) opLeft, (ExpressionEvaluator.Evaluator<bool?>) opRight);

        //                default:
        //                    throw new InvalidOperationException();
        //            }
        //            throw new InvalidOperationException("Unhandled Coalesce transaltion");
        //        }

        //        private ExpressionEvaluator.Evaluator Build(Expression exp)
        //        {
        //            if (exp == null)
        //            {
        //                return null;
        //            }
        //            switch (exp.NodeType)
        //            {
        //                case ExpressionType.Add:
        //                case ExpressionType.AddChecked:
        //                case ExpressionType.And:
        //                case ExpressionType.AndAlso:
        //                case ExpressionType.ArrayIndex:
        //                case ExpressionType.Coalesce:
        //                case ExpressionType.Divide:
        //                case ExpressionType.Equal:
        //                case ExpressionType.ExclusiveOr:
        //                case ExpressionType.GreaterThan:
        //                case ExpressionType.GreaterThanOrEqual:
        //                case ExpressionType.LeftShift:
        //                case ExpressionType.LessThan:
        //                case ExpressionType.LessThanOrEqual:
        //                case ExpressionType.Modulo:
        //                case ExpressionType.Multiply:
        //                case ExpressionType.MultiplyChecked:
        //                case ExpressionType.NotEqual:
        //                case ExpressionType.Or:
        //                case ExpressionType.OrElse:
        //                case ExpressionType.Power:
        //                case ExpressionType.RightShift:
        //                case ExpressionType.Subtract:
        //                case ExpressionType.SubtractChecked:
        //                    return this.Binary((BinaryExpression) exp);

        //                case ExpressionType.ArrayLength:
        //                case ExpressionType.Convert:
        //                case ExpressionType.ConvertChecked:
        //                case ExpressionType.Negate:
        //                case ExpressionType.UnaryPlus:
        //                case ExpressionType.NegateChecked:
        //                case ExpressionType.Not:
        //                case ExpressionType.Quote:
        //                case ExpressionType.TypeAs:
        //                    return this.Unary((UnaryExpression) exp);

        //                case ExpressionType.Call:
        //                    return this.Call((MethodCallExpression) exp);

        //                case ExpressionType.Conditional:
        //                    return this.Conditional((ConditionalExpression) exp);

        //                case ExpressionType.Constant:
        //                    return this.Constant((ConstantExpression) exp);

        //                case ExpressionType.Invoke:
        //                    return this.Invoke((InvocationExpression) exp);

        //                case ExpressionType.Lambda:
        //                    return this.Lambda((LambdaExpression) exp);

        //                case ExpressionType.ListInit:
        //                    return this.ListInit((ListInitExpression) exp);

        //                case ExpressionType.MemberAccess:
        //                    return this.MemberAccess((MemberExpression) exp);

        //                case ExpressionType.MemberInit:
        //                    return this.MemberInit((MemberInitExpression) exp);

        //                case ExpressionType.New:
        //                    return this.New((NewExpression) exp);

        //                case ExpressionType.NewArrayInit:
        //                    return this.NewArrayInit((NewArrayExpression) exp);

        //                case ExpressionType.NewArrayBounds:
        //                    return this.NewArrayBounds((NewArrayExpression) exp);

        //                case ExpressionType.Parameter:
        //                    return this.Parameter((ParameterExpression) exp);

        //                case ExpressionType.TypeIs:
        //                    return this.TypeIs((TypeBinaryExpression) exp);
        //            }
        //            throw new InvalidOperationException();
        //        }

        //        private ExpressionEvaluator.Evaluator Build(Type resultType, Expression expression)
        //        {
        //            if (!(expression.Type == resultType))
        //            {
        //                expression = Expression.Convert(expression, resultType);
        //            }
        //            return this.Build(expression);
        //        }

        //        internal static ExpressionEvaluator.Evaluator Build(ExpressionEvaluator.EvaluatorBuilder outer, IEnumerable<ParameterExpression> parameters, Expression expression)
        //        {
        //            List<ParameterExpression> list = parameters.ToList<ParameterExpression>();
        //            list.AddRange(VariableFinder.Find(expression));
        //            return new ExpressionEvaluator.EvaluatorBuilder(outer, list).Build(expression);
        //        }

        //        public ExpressionEvaluator.Evaluator Call(MethodCallExpression mc)
        //        {
        //            ExpressionEvaluator.Evaluator opInst = (mc.Object != null) ? this.Build(mc.Object) : null;
        //            ExpressionEvaluator.Evaluator[] opArgs = Enumerable.Select<Expression, ExpressionEvaluator.Evaluator>(mc.Arguments, new Func<Expression, ExpressionEvaluator.Evaluator>(this, (IntPtr) this.<Call>b__0)).ToArray<ExpressionEvaluator.Evaluator>();
        //            return this.GetMethodCallOperator(mc.Method, opInst, opArgs);
        //        }

        //        public ExpressionEvaluator.Evaluator Conditional(ConditionalExpression c)
        //        {
        //            ExpressionEvaluator.Evaluator evaluator = this.Build(c.Test);
        //            ExpressionEvaluator.Evaluator evaluator2 = this.Build(c.IfTrue);
        //            ExpressionEvaluator.Evaluator evaluator3 = this.Build(c.IfFalse);
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ConditionalEvaluator<T><>).MakeGenericType(new Type[] { c.Type }), new object[] { evaluator, evaluator2, evaluator3 });
        //        }

        //        public ExpressionEvaluator.Evaluator Constant(ConstantExpression c)
        //        {
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ConstantEvaluator<T><>).MakeGenericType(new Type[] { c.Type }), new object[] { c.Value });
        //        }

        //        private ExpressionEvaluator.Initializer Element(Type type, ElementInit elem)
        //        {
        //            Type[] typeArray;
        //            ExpressionEvaluator.Evaluator[] evaluatorArray = Enumerable.Select<Expression, ExpressionEvaluator.Evaluator>(elem.Arguments, new Func<Expression, ExpressionEvaluator.Evaluator>(this, (IntPtr) this.<Element>b__1d)).ToArray<ExpressionEvaluator.Evaluator>();
        //            if (evaluatorArray.Length == 1)
        //            {
        //                typeArray = new Type[] { type, evaluatorArray[0].ReturnType };
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ElementInitializer<T, A><,>).MakeGenericType(typeArray), new object[] { elem.AddMethod, evaluatorArray[0] });
        //            }
        //            if (evaluatorArray.Length == 2)
        //            {
        //                typeArray = new Type[] { type, evaluatorArray[0].ReturnType, evaluatorArray[1].ReturnType };
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ElementInitializer<T, A1, A2><,,>).MakeGenericType(typeArray), new object[] { elem.AddMethod, evaluatorArray[0], evaluatorArray[1] });
        //            }
        //            return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ElementInitializer<T><>).MakeGenericType(new Type[] { type }), new object[] { elem.AddMethod, evaluatorArray });
        //        }

        //        private MethodInfo FindBestMethod(IEnumerable<MethodInfo> methods, Type[] argTypes, Type returnType)
        //        {
        //            <>c__DisplayClass20 class2;
        //            MethodInfo info = Enumerable.FirstOrDefault<MethodInfo>(methods, new Func<MethodInfo, bool>(class2, (IntPtr) this.<FindBestMethod>b__1e));
        //            if (info == null)
        //            {
        //                info = Enumerable.FirstOrDefault<MethodInfo>(methods, new Func<MethodInfo, bool>(class2, (IntPtr) this.<FindBestMethod>b__1f));
        //            }
        //            return info;
        //        }

        //        private int FindParameterIndex(ParameterExpression p)
        //        {
        //            for (ExpressionEvaluator.EvaluatorBuilder builder = this; builder != null; builder = builder.outer)
        //            {
        //                if (this.parameters != null)
        //                {
        //                    int num = 0;
        //                    int count = this.parameters.Count;
        //                    while (num < count)
        //                    {
        //                        if (this.parameters[num] == p)
        //                        {
        //                            return (num + ((this.outer != null) ? this.outer.Count : 0));
        //                        }
        //                        num++;
        //                    }
        //                }
        //            }
        //            throw new InvalidOperationException(string.Format("Parameter '{0}' not in scope", p.Name));
        //        }

        //        public ExpressionEvaluator.Evaluator GetBinaryOperator(BinaryExpression b, Type sourceType, Type targetType, MethodInfo method, ExpressionEvaluator.Evaluator opLeft, ExpressionEvaluator.Evaluator opRight)
        //        {
        //            Delegate delegate2 = Delegate.CreateDelegate(typeof(Func<,,>).MakeGenericType(new Type[] { sourceType, sourceType, targetType }), null, method);
        //            if (b.IsLiftedToNull)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LiftToNullBinaryEvaluator<S, T><,>).MakeGenericType(new Type[] { sourceType, targetType }), new object[] { opLeft, opRight, delegate2 });
        //            }
        //            if (b.IsLifted)
        //            {
        //                if (b.NodeType == ExpressionType.Equal)
        //                {
        //                    return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LiftToEqualBinaryEvaluator<S><>).MakeGenericType(new Type[] { sourceType }), new object[] { opLeft, opRight, delegate2 });
        //                }
        //                if (b.NodeType == ExpressionType.NotEqual)
        //                {
        //                    return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LiftToNotEqualBinaryEvaluator<S><>).MakeGenericType(new Type[] { sourceType }), new object[] { opLeft, opRight, delegate2 });
        //                }
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LiftToFalseBinaryEvaluator<S><>).MakeGenericType(new Type[] { sourceType }), new object[] { opLeft, opRight, delegate2 });
        //            }
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.BinaryEvaluator<S, T><,>).MakeGenericType(new Type[] { sourceType, targetType }), new object[] { opLeft, opRight, delegate2 });
        //        }

        //        public ExpressionEvaluator.Evaluator GetMethodCallOperator(MethodInfo method, ExpressionEvaluator.Evaluator opInst, ExpressionEvaluator.Evaluator[] opArgs)
        //        {
        //            Type[] typeArray;
        //            Delegate delegate2;
        //            ParameterInfo[] parameters = method.GetParameters();
        //            if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
        //            {
        //                CS$<>9__CachedAnonymousMethodDelegate2 = new Func<ParameterInfo, bool>(null, (IntPtr) <GetMethodCallOperator>b__1);
        //            }
        //            if (Enumerable.Any<ParameterInfo>(parameters, CS$<>9__CachedAnonymousMethodDelegate2))
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.MethodCallWithRefArgsEvaluator<T><>).MakeGenericType(new Type[] { method.ReturnType }), new object[] { method, opInst, opArgs });
        //            }
        //            if (method.IsStatic && (opArgs.Length == 1))
        //            {
        //                typeArray = new Type[] { parameters[0].ParameterType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A, R><,>).MakeGenericType(typeArray), new object[] { opArgs[0], delegate2 });
        //            }
        //            if (!(method.IsStatic || (opArgs.Length != 0)))
        //            {
        //                typeArray = new Type[] { opInst.ReturnType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A, R><,>).MakeGenericType(typeArray), new object[] { opInst, delegate2 });
        //            }
        //            if (method.IsStatic && (opArgs.Length == 2))
        //            {
        //                typeArray = new Type[] { parameters[0].ParameterType, parameters[1].ParameterType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A1, A2, R><,,>).MakeGenericType(typeArray), new object[] { opArgs[0], opArgs[1], delegate2 });
        //            }
        //            if (!(method.IsStatic || (opArgs.Length != 1)))
        //            {
        //                typeArray = new Type[] { opInst.ReturnType, parameters[0].ParameterType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A1, A2, R><,,>).MakeGenericType(typeArray), new object[] { opInst, opArgs[0], delegate2 });
        //            }
        //            if (method.IsStatic && (opArgs.Length == 3))
        //            {
        //                typeArray = new Type[] { parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,,,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A1, A2, A3, R><,,,>).MakeGenericType(typeArray), new object[] { opArgs[0], opArgs[1], opArgs[2], delegate2 });
        //            }
        //            if (!(method.IsStatic || (opArgs.Length != 2)))
        //            {
        //                typeArray = new Type[] { opInst.ReturnType, parameters[0].ParameterType, parameters[1].ParameterType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,,,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A1, A2, A3, R><,,,>).MakeGenericType(typeArray), new object[] { opInst, opArgs[0], opArgs[1], delegate2 });
        //            }
        //            if (method.IsStatic && (opArgs.Length == 4))
        //            {
        //                typeArray = new Type[] { parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType, parameters[3].ParameterType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,,,,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A1, A2, A3, A4, R><,,,,>).MakeGenericType(typeArray), new object[] { opArgs[0], opArgs[1], opArgs[2], opArgs[3], delegate2 });
        //            }
        //            if (!(method.IsStatic || (opArgs.Length != 3)))
        //            {
        //                typeArray = new Type[] { opInst.ReturnType, parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType, method.ReturnType };
        //                delegate2 = Delegate.CreateDelegate(typeof(Func<,,,,>).MakeGenericType(typeArray), null, method);
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FuncEvaluator<A1, A2, A3, A4, R><,,,,>).MakeGenericType(typeArray), new object[] { opInst, opArgs[0], opArgs[1], opArgs[2], delegate2 });
        //            }
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.MethodCallEvaluator<T><>).MakeGenericType(new Type[] { method.ReturnType }), new object[] { method, opInst, opArgs });
        //        }

        //        private static Type GetNonNullType(Type type)
        //        {
        //            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
        //            {
        //                return type.GetGenericArguments()[0];
        //            }
        //            return type;
        //        }

        //        private ExpressionEvaluator.Evaluator GetUnaryOperator(UnaryExpression u, Type sourceType, Type targetType, MethodInfo method, ExpressionEvaluator.Evaluator operand)
        //        {
        //            Delegate delegate2 = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(new Type[] { sourceType, targetType }), null, method);
        //            if (u.IsLiftedToNull)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LiftToNullUnaryEvaluator<S, T><,>).MakeGenericType(new Type[] { sourceType, targetType }), new object[] { operand, delegate2 });
        //            }
        //            if (u.IsLifted)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LiftToFalseUnaryEvaluator<S><>).MakeGenericType(new Type[] { sourceType }), new object[] { operand, delegate2 });
        //            }
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.UnaryEvaluator<S, T><,>).MakeGenericType(new Type[] { sourceType, targetType }), new object[] { operand, delegate2 });
        //        }

        //        public ExpressionEvaluator.Evaluator Invoke(InvocationExpression inv)
        //        {
        //            Func<Expression, ExpressionEvaluator.Evaluator> func = null;
        //            LambdaExpression expression = inv.Expression as LambdaExpression;
        //            if (expression != null)
        //            {
        //                ExpressionEvaluator.Evaluator evaluator = this.Build(expression.Body);
        //                for (int i = expression.Parameters.Count - 1; i >= 0; i--)
        //                {
        //                    ParameterExpression p = expression.Parameters[i];
        //                    int num2 = this.FindParameterIndex(p);
        //                    ExpressionEvaluator.Evaluator evaluator2 = this.Build(inv.Arguments[i]);
        //                    evaluator = (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LetEvaluator<V, E><,>).MakeGenericType(new Type[] { p.Type, expression.Body.Type }), new object[] { num2, evaluator2, evaluator });
        //                }
        //                return evaluator;
        //            }
        //            ExpressionEvaluator.Evaluator evaluator3 = new ExpressionEvaluator.EvaluatorBuilder(this, null).Build(inv.Expression);
        //            if (func == null)
        //            {
        //                func = new Func<Expression, ExpressionEvaluator.Evaluator>(this, (IntPtr) this.<Invoke>b__8);
        //            }
        //            ExpressionEvaluator.Evaluator[] evaluatorArray = Enumerable.Select<Expression, ExpressionEvaluator.Evaluator>(inv.Arguments, func).ToArray<ExpressionEvaluator.Evaluator>();
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.InvokeEvaluator<T><>).MakeGenericType(new Type[] { inv.Type }), new object[] { evaluator3, evaluatorArray });
        //        }

        //        private static bool IsNullable(Type type)
        //        {
        //            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        //        }

        //        public ExpressionEvaluator.Evaluator Lambda(LambdaExpression lambda)
        //        {
        //            ExpressionEvaluator.Evaluator evaluator = Build(this, lambda.Parameters, lambda.Body);
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.LambdaEvaluator<T><>).MakeGenericType(new Type[] { lambda.Type }), new object[] { lambda.Parameters.Count, evaluator });
        //        }

        //        private ExpressionEvaluator.Evaluator ListInit(ListInitExpression lini)
        //        {
        //            <>c__DisplayClassf classf;
        //            ExpressionEvaluator.Evaluator evaluator = this.Build(lini.NewExpression);
        //            ExpressionEvaluator.Initializer[] initializerArray = Enumerable.Select<ElementInit, ExpressionEvaluator.Initializer>(lini.Initializers, new Func<ElementInit, ExpressionEvaluator.Initializer>(classf, (IntPtr) this.<ListInit>b__e)).ToArray<ExpressionEvaluator.Initializer>();
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.InitializerEvaluator<T><>).MakeGenericType(new Type[] { lini.Type }), new object[] { evaluator, initializerArray });
        //        }

        //        public ExpressionEvaluator.Evaluator MemberAccess(MemberExpression m)
        //        {
        //            ExpressionEvaluator.Evaluator evaluator = (m.Expression != null) ? this.Build(m.Expression) : null;
        //            FieldInfo member = m.Member as FieldInfo;
        //            if (member != null)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.FieldAccessEvaluator<T><>).MakeGenericType(new Type[] { member.FieldType }), new object[] { evaluator, member });
        //            }
        //            PropertyInfo info2 = m.Member as PropertyInfo;
        //            if (info2 == null)
        //            {
        //                throw new NotSupportedException();
        //            }
        //            Type type = (evaluator != null) ? evaluator.ReturnType : m.Member.DeclaringType;
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.PropertyAccessEvaluator<T, V><,>).MakeGenericType(new Type[] { type, info2.PropertyType }), new object[] { evaluator, info2 });
        //        }

        //        private ExpressionEvaluator.Initializer MemberAssignment(Type type, System.Linq.Expressions.MemberAssignment ma)
        //        {
        //            ExpressionEvaluator.Evaluator evaluator = this.Build(ma.Expression);
        //            if (ma.Member is FieldInfo)
        //            {
        //                if (type.IsValueType)
        //                {
        //                    return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ValueTypeFieldAssignmentInitializer<T><>).MakeGenericType(new Type[] { type }), new object[] { ma.Member, evaluator });
        //                }
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.FieldAssignmentInitializer<T><>).MakeGenericType(new Type[] { type }), new object[] { ma.Member, evaluator });
        //            }
        //            PropertyInfo member = (PropertyInfo) ma.Member;
        //            if (type.IsValueType)
        //            {
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ValueTypePropertyAssignmentInitializer<T, V><,>).MakeGenericType(new Type[] { type, member.PropertyType }), new object[] { member, evaluator });
        //            }
        //            return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.PropertyAssignmentInitializer<T, V><,>).MakeGenericType(new Type[] { type, member.PropertyType }), new object[] { member, evaluator });
        //        }

        //        private ExpressionEvaluator.Initializer MemberBinding(Type type, System.Linq.Expressions.MemberBinding mb)
        //        {
        //            switch (mb.BindingType)
        //            {
        //                case MemberBindingType.Assignment:
        //                    return this.MemberAssignment(type, (System.Linq.Expressions.MemberAssignment) mb);

        //                case MemberBindingType.MemberBinding:
        //                    return this.MemberMemberBinding(type, (System.Linq.Expressions.MemberMemberBinding) mb);

        //                case MemberBindingType.ListBinding:
        //                    return this.MemberListBinding(type, (System.Linq.Expressions.MemberListBinding) mb);
        //            }
        //            throw new NotImplementedException();
        //        }

        //        private ExpressionEvaluator.Evaluator MemberInit(MemberInitExpression mini)
        //        {
        //            <>c__DisplayClassc classc;
        //            ExpressionEvaluator.Evaluator evaluator = this.Build(mini.NewExpression);
        //            ExpressionEvaluator.Initializer[] initializerArray = Enumerable.Select<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer>(mini.Bindings, new Func<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer>(classc, (IntPtr) this.<MemberInit>b__b)).ToArray<ExpressionEvaluator.Initializer>();
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.InitializerEvaluator<T><>).MakeGenericType(new Type[] { mini.Type }), new object[] { evaluator, initializerArray });
        //        }

        //        private ExpressionEvaluator.Initializer MemberListBinding(Type type, System.Linq.Expressions.MemberListBinding mb)
        //        {
        //            ExpressionEvaluator.Initializer[] initializerArray;
        //            Func<ElementInit, ExpressionEvaluator.Initializer> func = null;
        //            Func<ElementInit, ExpressionEvaluator.Initializer> func2 = null;
        //            <>c__DisplayClass1b classb;
        //            FieldInfo fi = mb.Member as FieldInfo;
        //            if (fi != null)
        //            {
        //                if (func == null)
        //                {
        //                    func = new Func<ElementInit, ExpressionEvaluator.Initializer>(classb, (IntPtr) this.<MemberListBinding>b__17);
        //                }
        //                initializerArray = Enumerable.Select<ElementInit, ExpressionEvaluator.Initializer>(mb.Initializers, func).ToArray<ExpressionEvaluator.Initializer>();
        //                if (type.IsValueType)
        //                {
        //                    return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ValueTypeFieldMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, fi.FieldType }), new object[] { fi, initializerArray });
        //                }
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.FieldMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, fi.FieldType }), new object[] { fi, initializerArray });
        //            }
        //            PropertyInfo pi = mb.Member as PropertyInfo;
        //            if (pi == null)
        //            {
        //                throw new InvalidOperationException();
        //            }
        //            if (func2 == null)
        //            {
        //                func2 = new Func<ElementInit, ExpressionEvaluator.Initializer>(classb, (IntPtr) this.<MemberListBinding>b__18);
        //            }
        //            initializerArray = Enumerable.Select<ElementInit, ExpressionEvaluator.Initializer>(mb.Initializers, func2).ToArray<ExpressionEvaluator.Initializer>();
        //            if (type.IsValueType)
        //            {
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ValueTypePropertyMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, pi.PropertyType }), new object[] { pi, initializerArray });
        //            }
        //            return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.PropertyMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, pi.PropertyType }), new object[] { pi, initializerArray });
        //        }

        //        private ExpressionEvaluator.Initializer MemberMemberBinding(Type type, System.Linq.Expressions.MemberMemberBinding mb)
        //        {
        //            ExpressionEvaluator.Initializer[] initializerArray;
        //            Func<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer> func = null;
        //            Func<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer> func2 = null;
        //            <>c__DisplayClass15 class2;
        //            FieldInfo fi = mb.Member as FieldInfo;
        //            if (fi != null)
        //            {
        //                if (func == null)
        //                {
        //                    func = new Func<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer>(class2, (IntPtr) this.<MemberMemberBinding>b__11);
        //                }
        //                initializerArray = Enumerable.Select<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer>(mb.Bindings, func).ToArray<ExpressionEvaluator.Initializer>();
        //                if (type.IsValueType)
        //                {
        //                    return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ValueTypeFieldMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, fi.FieldType }), new object[] { fi, initializerArray });
        //                }
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.FieldMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, fi.FieldType }), new object[] { fi, initializerArray });
        //            }
        //            PropertyInfo pi = mb.Member as PropertyInfo;
        //            if (pi == null)
        //            {
        //                throw new InvalidOperationException();
        //            }
        //            if (func2 == null)
        //            {
        //                func2 = new Func<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer>(class2, (IntPtr) this.<MemberMemberBinding>b__12);
        //            }
        //            initializerArray = Enumerable.Select<System.Linq.Expressions.MemberBinding, ExpressionEvaluator.Initializer>(mb.Bindings, func2).ToArray<ExpressionEvaluator.Initializer>();
        //            if (type.IsValueType)
        //            {
        //                return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.ValueTypePropertyMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, pi.PropertyType }), new object[] { pi, initializerArray });
        //            }
        //            return (ExpressionEvaluator.Initializer) Activator.CreateInstance(typeof(ExpressionEvaluator.PropertyMemberInitializer<T, V><,>).MakeGenericType(new Type[] { type, pi.PropertyType }), new object[] { pi, initializerArray });
        //        }

        //        private bool MethodMatches(MethodInfo method, Type[] argTypes, Type returnType)
        //        {
        //            if (!((returnType == method.ReturnType) || method.ReflectedType.IsSubclassOf(returnType)))
        //            {
        //                return false;
        //            }
        //            ParameterInfo[] parameters = method.GetParameters();
        //            if (parameters.Length != argTypes.Length)
        //            {
        //                return false;
        //            }
        //            int index = 0;
        //            int length = parameters.Length;
        //            while (index < length)
        //            {
        //                if (!((parameters[index].ParameterType == argTypes[index]) || argTypes[index].IsSubclassOf(parameters[index].ParameterType)))
        //                {
        //                    return false;
        //                }
        //                index++;
        //            }
        //            return true;
        //        }

        //        private bool MethodMatchesExact(MethodInfo method, Type[] argTypes, Type returnType)
        //        {
        //            if (!(method.ReturnType == returnType))
        //            {
        //                return false;
        //            }
        //            ParameterInfo[] parameters = method.GetParameters();
        //            if (parameters.Length != argTypes.Length)
        //            {
        //                return false;
        //            }
        //            int index = 0;
        //            int length = parameters.Length;
        //            while (index < length)
        //            {
        //                if (!(parameters[index].ParameterType == argTypes[index]))
        //                {
        //                    return false;
        //                }
        //                index++;
        //            }
        //            return true;
        //        }

        //        public ExpressionEvaluator.Evaluator New(NewExpression n)
        //        {
        //            ExpressionEvaluator.Evaluator[] evaluatorArray = Enumerable.Select<Expression, ExpressionEvaluator.Evaluator>(n.Arguments, new Func<Expression, ExpressionEvaluator.Evaluator>(this, (IntPtr) this.<New>b__3)).ToArray<ExpressionEvaluator.Evaluator>();
        //            if (evaluatorArray.Length == 0)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewEvaluator<T><>).MakeGenericType(new Type[] { n.Type }), new object[] { n.Constructor });
        //            }
        //            if (evaluatorArray.Length == 1)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewEvaluator<T, A><,>).MakeGenericType(new Type[] { n.Type, n.Arguments[0].Type }), new object[] { n.Constructor, evaluatorArray[0] });
        //            }
        //            if (evaluatorArray.Length == 2)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewEvaluator<T, A1, A2><,,>).MakeGenericType(new Type[] { n.Type, n.Arguments[0].Type, n.Arguments[1].Type }), new object[] { n.Constructor, evaluatorArray[0], evaluatorArray[1] });
        //            }
        //            if (evaluatorArray.Length == 3)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewEvaluator<T, A1, A2, A3><,,,>).MakeGenericType(new Type[] { n.Type, n.Arguments[0].Type, n.Arguments[1].Type, n.Arguments[2].Type }), new object[] { n.Constructor, evaluatorArray[0], evaluatorArray[1], evaluatorArray[2] });
        //            }
        //            if (evaluatorArray.Length == 4)
        //            {
        //                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewEvaluator<T, A1, A2, A3, A4><,,,,>).MakeGenericType(new Type[] { n.Type, n.Arguments[0].Type, n.Arguments[1].Type, n.Arguments[2].Type, n.Arguments[3].Type }), new object[] { n.Constructor, evaluatorArray[0], evaluatorArray[1], evaluatorArray[2], evaluatorArray[3] });
        //            }
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewEvaluatorN<T><>).MakeGenericType(new Type[] { n.Type }), new object[] { n.Constructor, evaluatorArray });
        //        }

        //        public ExpressionEvaluator.Evaluator NewArrayBounds(NewArrayExpression n)
        //        {
        //            ExpressionEvaluator.Evaluator[] evaluatorArray = Enumerable.Select<Expression, ExpressionEvaluator.Evaluator>(n.Expressions, new Func<Expression, ExpressionEvaluator.Evaluator>(this, (IntPtr) this.<NewArrayBounds>b__7)).ToArray<ExpressionEvaluator.Evaluator>();
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewArrayBoundsEvaluator<T><>).MakeGenericType(new Type[] { n.Type.GetElementType() }), new object[] { evaluatorArray });
        //        }

        //        public ExpressionEvaluator.Evaluator NewArrayInit(NewArrayExpression n)
        //        {
        //            <>c__DisplayClass5 class2;
        //            Type elementType = n.Type.GetElementType();
        //            ExpressionEvaluator.Evaluator[] evaluatorArray = Enumerable.Select<Expression, ExpressionEvaluator.Evaluator>(n.Expressions, new Func<Expression, ExpressionEvaluator.Evaluator>(class2, (IntPtr) this.<NewArrayInit>b__4)).ToArray<ExpressionEvaluator.Evaluator>();
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.NewArrayInitEvaluator<T><>).MakeGenericType(new Type[] { elementType }), new object[] { evaluatorArray });
        //        }

        //        public ExpressionEvaluator.Evaluator Parameter(ParameterExpression p)
        //        {
        //            int num = this.FindParameterIndex(p);
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ParameterEvaluator<T><>).MakeGenericType(new Type[] { p.Type }), new object[] { num });
        //        }

        //        private ExpressionEvaluator.Evaluator Quote(UnaryExpression u)
        //        {
        //            Dictionary<int, ParameterExpression> dictionary = Enumerable.ToDictionary<ParameterExpression, int>(ExternalReferenceGatherer.Gather(this, u.Operand), new Func<ParameterExpression, int>(this, (IntPtr) this.<Quote>b__a));
        //            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.QuoteEvaluator<T><>).MakeGenericType(new Type[] { u.Type }), new object[] { u.Operand, dictionary });
        //        }

        //        public ExpressionEvaluator.Evaluator TypeIs(TypeBinaryExpression t)
        //        {
        //            return new ExpressionEvaluator.TypeIsEvaluator(this.Build(t.Expression), t.TypeOperand);
        //        }

        //        private ExpressionEvaluator.Evaluator Unary(UnaryExpression u)
        //        {
        //            MethodInfo info;
        //            ExpressionEvaluator.Evaluator operand = this.Build(u.Operand);
        //            bool flag = IsNullable(u.Operand.Type);
        //            bool flag2 = IsNullable(u.Type);
        //            Type nonNullType = GetNonNullType(u.Operand.Type);
        //            Type targetType = GetNonNullType(u.Type);
        //            if (u.Method != null)
        //            {
        //                return this.GetUnaryOperator(u, nonNullType, targetType, u.Method, operand);
        //            }
        //            switch (u.NodeType)
        //            {
        //                case ExpressionType.Negate:
        //                case ExpressionType.NegateChecked:
        //                case ExpressionType.Not:
        //                    info = this.FindBestMethod(ExpressionEvaluator.Operators.GetOperatorMethods(u.NodeType.ToString()), new Type[] { nonNullType }, targetType);
        //                    return this.GetUnaryOperator(u, nonNullType, targetType, info, operand);

        //                case ExpressionType.UnaryPlus:
        //                    return operand;

        //                case ExpressionType.Quote:
        //                    return this.Quote(u);

        //                case ExpressionType.TypeAs:
        //                    return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.TypeAsEvaluator<S, T><,>).MakeGenericType(new Type[] { u.Operand.Type, u.Type }), new object[] { operand });

        //                case ExpressionType.Convert:
        //                case ExpressionType.ConvertChecked:
        //                    if (!(u.Type == u.Operand.Type))
        //                    {
        //                        if (!(nonNullType.IsValueType && targetType.IsValueType))
        //                        {
        //                            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.Convert<S, T><,>).MakeGenericType(new Type[] { u.Operand.Type, u.Type }), new object[] { operand });
        //                        }
        //                        if (nonNullType == targetType)
        //                        {
        //                            if (!(!flag || flag2))
        //                            {
        //                                return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ConvertNtoNN<T><>).MakeGenericType(new Type[] { nonNullType }), new object[] { operand });
        //                            }
        //                            Debug.Assert(!flag && flag2);
        //                            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ConvertNNtoN<T><>).MakeGenericType(new Type[] { nonNullType }), new object[] { operand });
        //                        }
        //                        info = this.FindBestMethod(ExpressionEvaluator.Operators.GetOperatorMethods(u.NodeType + "To" + targetType.Name), new Type[] { nonNullType }, targetType);
        //                        Delegate delegate2 = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(new Type[] { nonNullType, targetType }), null, info);
        //                        if (!(flag || flag2))
        //                        {
        //                            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.Convert<S, T><,>).MakeGenericType(new Type[] { nonNullType, targetType }), new object[] { operand, delegate2 });
        //                        }
        //                        if (!(!flag || flag2))
        //                        {
        //                            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ConvertNtoNN<S, T><,>).MakeGenericType(new Type[] { nonNullType, targetType }), new object[] { operand, delegate2 });
        //                        }
        //                        if (!(flag || !flag2))
        //                        {
        //                            return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ConvertNNtoN<S, T><,>).MakeGenericType(new Type[] { nonNullType, targetType }), new object[] { operand, delegate2 });
        //                        }
        //                        Debug.Assert(flag && flag2);
        //                        return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ConvertNtoN<S, T><,>).MakeGenericType(new Type[] { nonNullType, targetType }), new object[] { operand, delegate2 });
        //                    }
        //                    return operand;

        //                case ExpressionType.ArrayLength:
        //                    return (ExpressionEvaluator.Evaluator) Activator.CreateInstance(typeof(ExpressionEvaluator.ArrayLengthEvaluator<T><>).MakeGenericType(new Type[] { u.Operand.Type.GetElementType() }), new object[] { operand });
        //            }
        //            throw new InvalidOperationException();
        //        }

        //        private int Count
        //        {
        //            get
        //            {
        //                if (this.count == -1)
        //                {
        //                    this.count = ((this.outer != null) ? this.outer.Count : 0) + ((this.parameters != null) ? this.parameters.Count : 0);
        //                }
        //                return this.count;
        //            }
        //        }

        //        private class ExternalReferenceGatherer : OldExpressionVisitor
        //        {
        //            private ExpressionEvaluator.EvaluatorBuilder builder;
        //            private HashSet<ParameterExpression> external = new HashSet<ParameterExpression>();

        //            private ExternalReferenceGatherer(ExpressionEvaluator.EvaluatorBuilder builder)
        //            {
        //                this.builder = builder;
        //            }

        //            internal static IEnumerable<ParameterExpression> Gather(ExpressionEvaluator.EvaluatorBuilder builder, Expression expression)
        //            {
        //                ExpressionEvaluator.EvaluatorBuilder.ExternalReferenceGatherer gatherer = new ExpressionEvaluator.EvaluatorBuilder.ExternalReferenceGatherer(builder);
        //                gatherer.Visit(expression);
        //                return gatherer.external;
        //            }

        //            internal override Expression VisitParameter(ParameterExpression p)
        //            {
        //                if (!this.builder.parameters.Contains(p))
        //                {
        //                    this.external.Add(p);
        //                }
        //                return base.VisitParameter(p);
        //            }
        //        }

        //        private class VariableFinder : OldExpressionVisitor
        //        {
        //            private List<ParameterExpression> variables = new List<ParameterExpression>();

        //            internal static List<ParameterExpression> Find(Expression expression)
        //            {
        //                ExpressionEvaluator.EvaluatorBuilder.VariableFinder finder = new ExpressionEvaluator.EvaluatorBuilder.VariableFinder();
        //                finder.Visit(expression);
        //                return finder.variables;
        //            }

        //            internal override Expression VisitInvocation(InvocationExpression iv)
        //            {
        //                LambdaExpression expression = iv.Expression as LambdaExpression;
        //                if (expression != null)
        //                {
        //                    this.variables.AddRange(expression.Parameters);
        //                }
        //                return base.VisitInvocation(iv);
        //            }
        //        }
        //    }

        //    public class EvaluatorHost
        //    {
        //        private ExpressionEvaluator.Evaluator evaluator;
        //        private int nArgs;
        //        private ExpressionEvaluator.EvaluatorState outer;

        //        public EvaluatorHost(ExpressionEvaluator.EvaluatorState outer, int nArgs, ExpressionEvaluator.Evaluator evaluator)
        //        {
        //            this.outer = outer;
        //            this.nArgs = nArgs;
        //            this.evaluator = evaluator;
        //        }

        //        public object Eval(object[] args)
        //        {
        //            int length = (args != null) ? args.Length : 0;
        //            if (length != this.nArgs)
        //            {
        //                object[] destinationArray = new object[this.nArgs];
        //                if (args != null)
        //                {
        //                    Array.Copy(args, destinationArray, length);
        //                }
        //                args = destinationArray;
        //            }
        //            return this.evaluator.EvalBoxed(new ExpressionEvaluator.EvaluatorState(this.outer, args));
        //        }
        //    }

        //    public class EvaluatorState
        //    {
        //        private ExpressionEvaluator.EvaluatorState outer;
        //        private int start;
        //        private object[] values;

        //        public EvaluatorState(ExpressionEvaluator.EvaluatorState outer, object[] values)
        //        {
        //            this.outer = outer;
        //            this.values = values;
        //            this.start = (outer != null) ? (outer.start + ((outer.values != null) ? outer.values.Length : 0)) : 0;
        //        }

        //        public object GetBoxedValue(int index)
        //        {
        //            ExpressionEvaluator.EvaluatorState outer = this;
        //            while (index < outer.start)
        //            {
        //                outer = outer.outer;
        //            }
        //            return outer.values[index - outer.start];
        //        }

        //        public T GetValue<T>(int index)
        //        {
        //            ExpressionEvaluator.EvaluatorState outer = this;
        //            while (index < outer.start)
        //            {
        //                outer = outer.outer;
        //            }
        //            return (T) outer.values[index - outer.start];
        //        }

        //        public void SetValue<T>(int index, T value)
        //        {
        //            ExpressionEvaluator.EvaluatorState outer = this;
        //            while (index < outer.start)
        //            {
        //                outer = outer.outer;
        //            }
        //            outer.values[index - outer.start] = value;
        //        }
        //    }

        //    public class FieldAccessEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private FieldInfo field;
        //        private ExpressionEvaluator.Evaluator operand;

        //        public FieldAccessEvaluator(ExpressionEvaluator.Evaluator operand, FieldInfo field)
        //        {
        //            this.operand = operand;
        //            this.field = field;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            object obj2 = (this.operand != null) ? this.operand.EvalBoxed(state) : null;
        //            return (T) this.field.GetValue(obj2);
        //        }

        //        public override ExpressionEvaluator.Address<T> EvalAddress(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return new FieldAddress<T>((this.operand != null) ? this.operand.EvalAddressBoxed(state) : null, this.field);
        //        }

        //        private class FieldAddress : ExpressionEvaluator.Address<T>
        //        {
        //            private FieldInfo field;
        //            private ExpressionEvaluator.Address instance;

        //            public FieldAddress(ExpressionEvaluator.Address instance, FieldInfo field)
        //            {
        //                this.instance = instance;
        //                this.field = field;
        //            }

        //            public override T GetValue(ExpressionEvaluator.EvaluatorState state)
        //            {
        //                object obj2 = (this.instance != null) ? this.instance.GetBoxedValue(state) : null;
        //                return (T) this.field.GetValue(obj2);
        //            }

        //            public override void SetValue(ExpressionEvaluator.EvaluatorState state, T value)
        //            {
        //                object obj2 = (this.instance != null) ? this.instance.GetBoxedValue(state) : null;
        //                this.field.SetValue(obj2, value);
        //                if (this.instance != null)
        //                {
        //                    this.instance.SetBoxedValue(state, obj2);
        //                }
        //            }
        //        }
        //    }

        //    public class FieldAssignmentInitializer<T> : ExpressionEvaluator.Initializer<T> where T: class
        //    {
        //        private ExpressionEvaluator.Evaluator evaluator;
        //        private FieldInfo field;

        //        public FieldAssignmentInitializer(FieldInfo field, ExpressionEvaluator.Evaluator evaluator)
        //        {
        //            this.field = field;
        //            this.evaluator = evaluator;
        //        }

        //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
        //        {
        //            object obj2 = this.evaluator.EvalBoxed(state);
        //            this.field.SetValue((T) instance, obj2);
        //        }
        //    }

        //    public class FieldMemberInitializer<T, V> : ExpressionEvaluator.Initializer<T> where T: class
        //    {
        //        private FieldInfo field;
        //        private ExpressionEvaluator.Initializer<V>[] initializers;

        //        public FieldMemberInitializer(FieldInfo field, ExpressionEvaluator.Initializer[] initializers)
        //        {
        //            this.field = field;
        //            if (ExpressionEvaluator.FieldMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate48 == null)
        //            {
        //                ExpressionEvaluator.FieldMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate48 = new Func<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(null, (IntPtr) ExpressionEvaluator.FieldMemberInitializer<T, V>.<.ctor>b__47);
        //            }
        //            this.initializers = Enumerable.Select<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(initializers, ExpressionEvaluator.FieldMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate48).ToArray<ExpressionEvaluator.Initializer<V>>();
        //        }

        //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
        //        {
        //            V local = (V) this.field.GetValue((T) instance);
        //            int index = 0;
        //            int length = this.initializers.Length;
        //            while (index < length)
        //            {
        //                this.initializers[index].Init(state, ref local);
        //                index++;
        //            }
        //            this.field.SetValue((T) instance, local);
        //        }
        //    }

        //    public class FuncEvaluator<R> : ExpressionEvaluator.Evaluator<R>
        //    {
        //        private Func<R> method;

        //        public FuncEvaluator(Func<R> method)
        //        {
        //            this.method = method;
        //        }

        //        public override R Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.method.Invoke();
        //        }
        //    }

        //    public class FuncEvaluator<A, R> : ExpressionEvaluator.Evaluator<R>
        //    {
        //        private ExpressionEvaluator.Evaluator<A> arg;
        //        private Func<A, R> method;

        //        public FuncEvaluator(ExpressionEvaluator.Evaluator<A> arg, Func<A, R> method)
        //        {
        //            this.arg = arg;
        //            this.method = method;
        //        }

        //        public override R Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            A local = this.arg.Eval(state);
        //            return this.method.Invoke(local);
        //        }
        //    }

        //    public class FuncEvaluator<A1, A2, R> : ExpressionEvaluator.Evaluator<R>
        //    {
        //        private ExpressionEvaluator.Evaluator<A1> arg1;
        //        private ExpressionEvaluator.Evaluator<A2> arg2;
        //        private Func<A1, A2, R> method;

        //        public FuncEvaluator(ExpressionEvaluator.Evaluator<A1> arg1, ExpressionEvaluator.Evaluator<A2> arg2, Func<A1, A2, R> method)
        //        {
        //            this.arg1 = arg1;
        //            this.arg2 = arg2;
        //            this.method = method;
        //        }

        //        public override R Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.method.Invoke(this.arg1.Eval(state), this.arg2.Eval(state));
        //        }
        //    }

        //    public class FuncEvaluator<A1, A2, A3, R> : ExpressionEvaluator.Evaluator<R>
        //    {
        //        private ExpressionEvaluator.Evaluator<A1> arg1;
        //        private ExpressionEvaluator.Evaluator<A2> arg2;
        //        private ExpressionEvaluator.Evaluator<A3> arg3;
        //        private Func<A1, A2, A3, R> method;

        //        public FuncEvaluator(ExpressionEvaluator.Evaluator<A1> arg1, ExpressionEvaluator.Evaluator<A2> arg2, ExpressionEvaluator.Evaluator<A3> arg3, Func<A1, A2, A3, R> method)
        //        {
        //            this.arg1 = arg1;
        //            this.arg2 = arg2;
        //            this.arg3 = arg3;
        //            this.method = method;
        //        }

        //        public override R Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.method.Invoke(this.arg1.Eval(state), this.arg2.Eval(state), this.arg3.Eval(state));
        //        }
        //    }

        //    public class FuncEvaluator<A1, A2, A3, A4, R> : ExpressionEvaluator.Evaluator<R>
        //    {
        //        private ExpressionEvaluator.Evaluator<A1> arg1;
        //        private ExpressionEvaluator.Evaluator<A2> arg2;
        //        private ExpressionEvaluator.Evaluator<A3> arg3;
        //        private ExpressionEvaluator.Evaluator<A4> arg4;
        //        private Func<A1, A2, A3, A4, R> method;

        //        public FuncEvaluator(ExpressionEvaluator.Evaluator<A1> arg1, ExpressionEvaluator.Evaluator<A2> arg2, ExpressionEvaluator.Evaluator<A3> arg3, ExpressionEvaluator.Evaluator<A4> arg4, Func<A1, A2, A3, A4, R> method)
        //        {
        //            this.arg1 = arg1;
        //            this.arg2 = arg2;
        //            this.arg3 = arg3;
        //            this.arg4 = arg4;
        //            this.method = method;
        //        }

        //        public override R Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return this.method.Invoke(this.arg1.Eval(state), this.arg2.Eval(state), this.arg3.Eval(state), this.arg4.Eval(state));
        //        }
        //    }

        //    public abstract class Initializer
        //    {
        //        protected Initializer()
        //        {
        //        }
        //    }

        //    public abstract class Initializer<T> : ExpressionEvaluator.Initializer
        //    {
        //        protected Initializer()
        //        {
        //        }

        //        public abstract void Init(ExpressionEvaluator.EvaluatorState state, ref T instance);
        //    }

        //    public class InitializerEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private ExpressionEvaluator.Initializer<T>[] initializers;
        //        private ExpressionEvaluator.Evaluator<T> opNew;

        //        public InitializerEvaluator(ExpressionEvaluator.Evaluator<T> opNew, ExpressionEvaluator.Initializer[] initializers)
        //        {
        //            this.opNew = opNew;
        //            if (ExpressionEvaluator.InitializerEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate46 == null)
        //            {
        //                ExpressionEvaluator.InitializerEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate46 = new Func<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<T>>(null, (IntPtr) ExpressionEvaluator.InitializerEvaluator<T>.<.ctor>b__45);
        //            }
        //            this.initializers = Enumerable.Select<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<T>>(initializers, ExpressionEvaluator.InitializerEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate46).ToArray<ExpressionEvaluator.Initializer<T>>();
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            T instance = this.opNew.Eval(state);
        //            int index = 0;
        //            int length = this.initializers.Length;
        //            while (index < length)
        //            {
        //                this.initializers[index].Init(state, ref instance);
        //                index++;
        //            }
        //            return instance;
        //        }
        //    }

        //    public class InvokeEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private ExpressionEvaluator.Evaluator[] opArgs;
        //        private ExpressionEvaluator.Evaluator opFunction;

        //        public InvokeEvaluator(ExpressionEvaluator.Evaluator opFunction, ExpressionEvaluator.Evaluator[] opArgs)
        //        {
        //            this.opFunction = opFunction;
        //            this.opArgs = opArgs;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            <>c__DisplayClass43<T> class2;
        //            T local;
        //            object obj2 = this.opFunction.EvalBoxed(state);
        //            IEnumerable<object> enumerable = Enumerable.Select<ExpressionEvaluator.Evaluator, object>(this.opArgs, new Func<ExpressionEvaluator.Evaluator, object>(class2, (IntPtr) this.<Eval>b__42));
        //            try
        //            {
        //                local = (T) ((Delegate) obj2).DynamicInvoke(new object[] { enumerable });
        //            }
        //            catch (TargetInvocationException exception)
        //            {
        //                throw exception.InnerException;
        //            }
        //            return local;
        //        }
        //    }

        //    public class LambdaEvaluator<T> : ExpressionEvaluator.Evaluator<T> where T: class
        //    {
        //        private ExpressionEvaluator.Evaluator evaluator;
        //        private int nArgs;

        //        public LambdaEvaluator(int nArgs, ExpressionEvaluator.Evaluator evaluator)
        //        {
        //            this.nArgs = nArgs;
        //            this.evaluator = evaluator;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            return (T) ExpressionEvaluator.CreateDelegate(typeof(T), state, this.nArgs, this.evaluator);
        //        }
        //    }

        //    public class LetEvaluator<V, E> : ExpressionEvaluator.Evaluator<E>
        //    {
        //        private ExpressionEvaluator.Evaluator<E> evExpression;
        //        private ExpressionEvaluator.Evaluator<V> evValue;
        //        private int index;

        //        public LetEvaluator(int index, ExpressionEvaluator.Evaluator<V> evValue, ExpressionEvaluator.Evaluator<E> evExpression)
        //        {
        //            this.index = index;
        //            this.evValue = evValue;
        //            this.evExpression = evExpression;
        //        }

        //        public override E Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            V local = this.evValue.Eval(state);
        //            state.SetValue<V>(this.index, local);
        //            return this.evExpression.Eval(state);
        //        }
        //    }

        //    public class LiftToEqualBinaryEvaluator<S> : ExpressionEvaluator.Evaluator<bool> where S: struct
        //    {
        //        private ExpressionEvaluator.Evaluator<S?> left;
        //        private Func<S, S, bool> op;
        //        private ExpressionEvaluator.Evaluator<S?> right;

        //        public LiftToEqualBinaryEvaluator(ExpressionEvaluator.Evaluator<S?> left, ExpressionEvaluator.Evaluator<S?> right, Func<S, S, bool> op)
        //        {
        //            this.left = left;
        //            this.right = right;
        //            this.op = op;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S? nullable = this.left.Eval(state);
        //            S? nullable2 = this.right.Eval(state);
        //            if (!(nullable.HasValue && nullable2.HasValue))
        //            {
        //                return (!nullable.HasValue && !nullable2.HasValue);
        //            }
        //            return this.op.Invoke(nullable.GetValueOrDefault(), nullable2.GetValueOrDefault());
        //        }
        //    }

        //    public class LiftToFalseAndAlsoEvaluator : ExpressionEvaluator.Evaluator<bool>
        //    {
        //        private ExpressionEvaluator.Evaluator<bool?> left;
        //        private ExpressionEvaluator.Evaluator<bool?> right;

        //        public LiftToFalseAndAlsoEvaluator(ExpressionEvaluator.Evaluator<bool?> left, ExpressionEvaluator.Evaluator<bool?> right)
        //        {
        //            this.left = left;
        //            this.right = right;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            bool? nullable = this.left.Eval(state);
        //            if (!nullable.HasValue)
        //            {
        //                return false;
        //            }
        //            bool? nullable2 = this.right.Eval(state);
        //            if (!nullable2.HasValue)
        //            {
        //                return false;
        //            }
        //            return (!nullable.GetValueOrDefault() ? false : nullable2.GetValueOrDefault());
        //        }
        //    }

        //    public class LiftToFalseBinaryEvaluator<S> : ExpressionEvaluator.Evaluator<bool> where S: struct
        //    {
        //        private ExpressionEvaluator.Evaluator<S?> left;
        //        private Func<S, S, bool> op;
        //        private ExpressionEvaluator.Evaluator<S?> right;

        //        public LiftToFalseBinaryEvaluator(ExpressionEvaluator.Evaluator<S?> left, ExpressionEvaluator.Evaluator<S?> right, Func<S, S, bool> op)
        //        {
        //            this.left = left;
        //            this.right = right;
        //            this.op = op;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S? nullable = this.left.Eval(state);
        //            S? nullable2 = this.right.Eval(state);
        //            if (!(nullable.HasValue && nullable2.HasValue))
        //            {
        //                return false;
        //            }
        //            return this.op.Invoke(nullable.GetValueOrDefault(), nullable2.GetValueOrDefault());
        //        }
        //    }

        //    public class LiftToFalseMethodCallEvaluator : ExpressionEvaluator.Evaluator<bool>
        //    {
        //        private MethodInfo method;
        //        private ExpressionEvaluator.Evaluator[] opArgs;
        //        private ExpressionEvaluator.Evaluator opInst;

        //        public LiftToFalseMethodCallEvaluator(MethodInfo method, ExpressionEvaluator.Evaluator opInst, ExpressionEvaluator.Evaluator[] opArgs)
        //        {
        //            this.method = method;
        //            this.opInst = opInst;
        //            this.opArgs = opArgs;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            <>c__DisplayClass36 class2;
        //            bool flag;
        //            object obj2 = (this.opInst != null) ? this.opInst.EvalBoxed(state) : null;
        //            object[] parameters = Enumerable.Select<ExpressionEvaluator.Evaluator, object>(this.opArgs, new Func<ExpressionEvaluator.Evaluator, object>(class2, (IntPtr) this.<Eval>b__33)).ToArray<object>();
        //            if (obj2 == null)
        //            {
        //            }
        //            if ((CS$<>9__CachedAnonymousMethodDelegate35 != null) || Enumerable.Any<object>(parameters, CS$<>9__CachedAnonymousMethodDelegate35))
        //            {
        //                return false;
        //            }
        //            try
        //            {
        //                flag = (bool) this.method.Invoke(obj2, parameters);
        //            }
        //            catch (TargetInvocationException exception)
        //            {
        //                throw exception.InnerException;
        //            }
        //            return flag;
        //        }
        //    }

        //    public class LiftToFalseOrElseEvaluator : ExpressionEvaluator.Evaluator<bool>
        //    {
        //        private ExpressionEvaluator.Evaluator<bool?> left;
        //        private ExpressionEvaluator.Evaluator<bool?> right;

        //        public LiftToFalseOrElseEvaluator(ExpressionEvaluator.Evaluator<bool?> left, ExpressionEvaluator.Evaluator<bool?> right)
        //        {
        //            this.left = left;
        //            this.right = right;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            bool? nullable = this.left.Eval(state);
        //            if (!nullable.HasValue)
        //            {
        //                return false;
        //            }
        //            if (nullable.GetValueOrDefault())
        //            {
        //                return true;
        //            }
        //            bool? nullable2 = this.right.Eval(state);
        //            if (!nullable2.HasValue)
        //            {
        //                return false;
        //            }
        //            return nullable2.GetValueOrDefault();
        //        }
        //    }

        //    public class LiftToFalseUnaryEvaluator<S> : ExpressionEvaluator.Evaluator<bool> where S: struct
        //    {
        //        private Func<S, bool> op;
        //        private ExpressionEvaluator.Evaluator<S?> operand;

        //        public LiftToFalseUnaryEvaluator(ExpressionEvaluator.Evaluator<S?> operand, Func<S, bool> op)
        //        {
        //            this.operand = operand;
        //            this.op = op;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S? nullable = this.operand.Eval(state);
        //            if (!nullable.HasValue)
        //            {
        //                return false;
        //            }
        //            return this.op.Invoke(nullable.GetValueOrDefault());
        //        }
        //    }

        //    public class LiftToNotEqualBinaryEvaluator<S> : ExpressionEvaluator.Evaluator<bool> where S: struct
        //    {
        //        private ExpressionEvaluator.Evaluator<S?> left;
        //        private Func<S, S, bool> op;
        //        private ExpressionEvaluator.Evaluator<S?> right;

        //        public LiftToNotEqualBinaryEvaluator(ExpressionEvaluator.Evaluator<S?> left, ExpressionEvaluator.Evaluator<S?> right, Func<S, S, bool> op)
        //        {
        //            this.left = left;
        //            this.right = right;
        //            this.op = op;
        //        }

        //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S? nullable = this.left.Eval(state);
        //            S? nullable2 = this.right.Eval(state);
        //            if (!(nullable.HasValue && nullable2.HasValue))
        //            {
        //                return (nullable.HasValue || nullable2.HasValue);
        //            }
        //            return this.op.Invoke(nullable.GetValueOrDefault(), nullable2.GetValueOrDefault());
        //        }
        //    }

        //    public class LiftToNullAndAlsoEvaluator : ExpressionEvaluator.Evaluator<bool?>
        //    {
        //        private ExpressionEvaluator.Evaluator<bool?> left;
        //        private ExpressionEvaluator.Evaluator<bool?> right;

        //        public LiftToNullAndAlsoEvaluator(ExpressionEvaluator.Evaluator<bool?> left, ExpressionEvaluator.Evaluator<bool?> right)
        //        {
        //            this.left = left;
        //            this.right = right;
        //        }

        //        public override bool? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            bool? nullable = this.left.Eval(state);
        //            if (!nullable.HasValue)
        //            {
        //                return null;
        //            }
        //            if (!nullable.GetValueOrDefault())
        //            {
        //                return false;
        //            }
        //            bool? nullable2 = this.right.Eval(state);
        //            if (!nullable2.HasValue)
        //            {
        //                return null;
        //            }
        //            return new bool?(nullable2.GetValueOrDefault());
        //        }
        //    }

        //    public class LiftToNullBinaryEvaluator<S, T> : ExpressionEvaluator.Evaluator<T?> where S: struct where T: struct
        //    {
        //        private ExpressionEvaluator.Evaluator<S?> left;
        //        private Func<S, S, T> op;
        //        private ExpressionEvaluator.Evaluator<S?> right;

        //        public LiftToNullBinaryEvaluator(ExpressionEvaluator.Evaluator<S?> left, ExpressionEvaluator.Evaluator<S?> right, Func<S, S, T> op)
        //        {
        //            this.left = left;
        //            this.right = right;
        //            this.op = op;
        //        }

        //        public override T? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S? nullable = this.left.Eval(state);
        //            S? nullable2 = this.right.Eval(state);
        //            if (!(nullable.HasValue && nullable2.HasValue))
        //            {
        //                return null;
        //            }
        //            return new T?(this.op.Invoke(nullable.GetValueOrDefault(), nullable2.GetValueOrDefault()));
        //        }
        //    }

        //    public class LiftToNullMethodCallEvaluator<T> : ExpressionEvaluator.Evaluator<T?> where T: struct
        //    {
        //        private MethodInfo method;
        //        private ExpressionEvaluator.Evaluator[] opArgs;
        //        private ExpressionEvaluator.Evaluator opInst;

        //        public LiftToNullMethodCallEvaluator(MethodInfo method, ExpressionEvaluator.Evaluator opInst, ExpressionEvaluator.Evaluator[] opArgs)
        //        {
        //            this.method = method;
        //            this.opInst = opInst;
        //            this.opArgs = opArgs;
        //        }

        //        public override T? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            <>c__DisplayClass31<T> class2;
        //            T? nullable;
        //            object obj2 = (this.opInst != null) ? this.opInst.EvalBoxed(state) : null;
        //            object[] parameters = Enumerable.Select<ExpressionEvaluator.Evaluator, object>(this.opArgs, new Func<ExpressionEvaluator.Evaluator, object>(class2, (IntPtr) this.<Eval>b__2e)).ToArray<object>();
        //            if (obj2 == null)
        //            {
        //            }
        //            if ((ExpressionEvaluator.LiftToNullMethodCallEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate30 != null) || Enumerable.Any<object>(parameters, ExpressionEvaluator.LiftToNullMethodCallEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate30))
        //            {
        //                return null;
        //            }
        //            try
        //            {
        //                nullable = (T?) this.method.Invoke(obj2, parameters);
        //            }
        //            catch (TargetInvocationException exception)
        //            {
        //                throw exception.InnerException;
        //            }
        //            return nullable;
        //        }
        //    }

        //    public class LiftToNullOrElseEvaluator : ExpressionEvaluator.Evaluator<bool?>
        //    {
        //        private ExpressionEvaluator.Evaluator<bool?> left;
        //        private ExpressionEvaluator.Evaluator<bool?> right;

        //        public LiftToNullOrElseEvaluator(ExpressionEvaluator.Evaluator<bool?> left, ExpressionEvaluator.Evaluator<bool?> right)
        //        {
        //            this.left = left;
        //            this.right = right;
        //        }

        //        public override bool? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            bool? nullable = this.left.Eval(state);
        //            if (!nullable.HasValue)
        //            {
        //                return null;
        //            }
        //            if (nullable.GetValueOrDefault())
        //            {
        //                return true;
        //            }
        //            bool? nullable2 = this.right.Eval(state);
        //            if (!nullable2.HasValue)
        //            {
        //                return null;
        //            }
        //            return new bool?(nullable2.GetValueOrDefault());
        //        }
        //    }

        //    public class LiftToNullUnaryEvaluator<S, T> : ExpressionEvaluator.Evaluator<T?> where S: struct where T: struct
        //    {
        //        private Func<S, T> op;
        //        private ExpressionEvaluator.Evaluator<S?> operand;

        //        public LiftToNullUnaryEvaluator(ExpressionEvaluator.Evaluator<S?> operand, Func<S, T> op)
        //        {
        //            this.operand = operand;
        //            this.op = op;
        //        }

        //        public override T? Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            S? nullable = this.operand.Eval(state);
        //            if (!nullable.HasValue)
        //            {
        //                return null;
        //            }
        //            return new T?(this.op.Invoke(nullable.GetValueOrDefault()));
        //        }
        //    }

        //    public class MethodCallEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private MethodInfo method;
        //        private ExpressionEvaluator.Evaluator[] opArgs;
        //        private ExpressionEvaluator.Evaluator opInst;

        //        public MethodCallEvaluator(MethodInfo method, ExpressionEvaluator.Evaluator opInst, ExpressionEvaluator.Evaluator[] opArgs)
        //        {
        //            this.method = method;
        //            this.opInst = opInst;
        //            this.opArgs = opArgs;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            Func<ExpressionEvaluator.Evaluator, object> func = null;
        //            Func<ExpressionEvaluator.Evaluator, object> func2 = null;
        //            T local;
        //            try
        //            {
        //                object obj2;
        //                object boxedValue;
        //                object[] objArray;
        //                <>c__DisplayClass26<T> class2;
        //                if ((this.opInst != null) && this.opInst.ReturnType.IsValueType)
        //                {
        //                    ExpressionEvaluator.Address address = this.opInst.EvalAddressBoxed(state);
        //                    boxedValue = address.GetBoxedValue(state);
        //                    if (func == null)
        //                    {
        //                        func = new Func<ExpressionEvaluator.Evaluator, object>(class2, (IntPtr) this.<Eval>b__22);
        //                    }
        //                    objArray = Enumerable.Select<ExpressionEvaluator.Evaluator, object>(this.opArgs, func).ToArray<object>();
        //                    obj2 = this.method.Invoke(boxedValue, objArray);
        //                    address.SetBoxedValue(state, boxedValue);
        //                }
        //                else
        //                {
        //                    boxedValue = (this.opInst != null) ? this.opInst.EvalBoxed(state) : null;
        //                    if (func2 == null)
        //                    {
        //                        func2 = new Func<ExpressionEvaluator.Evaluator, object>(class2, (IntPtr) this.<Eval>b__23);
        //                    }
        //                    objArray = Enumerable.Select<ExpressionEvaluator.Evaluator, object>(this.opArgs, func2).ToArray<object>();
        //                    obj2 = this.method.Invoke(boxedValue, objArray);
        //                }
        //                local = (T) obj2;
        //            }
        //            catch (TargetInvocationException exception)
        //            {
        //                throw exception.InnerException;
        //            }
        //            return local;
        //        }
        //    }

        //    public class MethodCallWithRefArgsEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        //    {
        //        private MethodInfo method;
        //        private ExpressionEvaluator.Evaluator[] opArgs;
        //        private ExpressionEvaluator.Evaluator opInst;
        //        private ParameterInfo[] parameters;

        //        public MethodCallWithRefArgsEvaluator(MethodInfo method, ExpressionEvaluator.Evaluator opInst, ExpressionEvaluator.Evaluator[] opArgs)
        //        {
        //            this.method = method;
        //            this.parameters = method.GetParameters();
        //            this.opInst = opInst;
        //            this.opArgs = opArgs;
        //        }

        //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            Func<ExpressionEvaluator.Evaluator, ExpressionEvaluator.Address> func = null;
        //            Func<ExpressionEvaluator.Address, object> func2 = null;
        //            T local;
        //            try
        //            {
        //                object boxedValue;
        //                <>c__DisplayClass2c<T> classc;
        //                ExpressionEvaluator.Address address = null;
        //                if ((this.opInst != null) && this.opInst.ReturnType.IsValueType)
        //                {
        //                    address = this.opInst.EvalAddressBoxed(state);
        //                    boxedValue = address.GetBoxedValue(state);
        //                }
        //                else
        //                {
        //                    boxedValue = (this.opInst != null) ? this.opInst.EvalBoxed(state) : null;
        //                }
        //                if (func == null)
        //                {
        //                    func = new Func<ExpressionEvaluator.Evaluator, ExpressionEvaluator.Address>(classc, (IntPtr) this.<Eval>b__28);
        //                }
        //                ExpressionEvaluator.Address[] addressArray = Enumerable.Select<ExpressionEvaluator.Evaluator, ExpressionEvaluator.Address>(this.opArgs, func).ToArray<ExpressionEvaluator.Address>();
        //                if (func2 == null)
        //                {
        //                    func2 = new Func<ExpressionEvaluator.Address, object>(classc, (IntPtr) this.<Eval>b__29);
        //                }
        //                object[] parameters = Enumerable.Select<ExpressionEvaluator.Address, object>(addressArray, func2).ToArray<object>();
        //                object obj3 = this.method.Invoke(boxedValue, parameters);
        //                int index = 0;
        //                int length = parameters.Length;
        //                while (index < length)
        //                {
        //                    if (this.parameters[index].ParameterType.IsByRef)
        //                    {
        //                        addressArray[index].SetBoxedValue(state, parameters[index]);
        //                    }
        //                    index++;
        //                }
        //                if (address != null)
        //                {
        //                    address.SetBoxedValue(state, boxedValue);
        //                }
        //                local = (T) obj3;
        //            }
        //            catch (TargetInvocationException exception)
        //            {
        //                throw exception.InnerException;
        //            }
        //            return local;
        //        }
        //    }

        //    public class NewArrayBoundsEvaluator<T> : ExpressionEvaluator.Evaluator<T[]>
        //    {
        //        private ExpressionEvaluator.Evaluator<int>[] bounds;

        //        public NewArrayBoundsEvaluator(ExpressionEvaluator.Evaluator[] bounds)
        //        {
        //            if (ExpressionEvaluator.NewArrayBoundsEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate3e == null)
        //            {
        //                ExpressionEvaluator.NewArrayBoundsEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate3e = new Func<ExpressionEvaluator.Evaluator, ExpressionEvaluator.Evaluator<int>>(null, (IntPtr) ExpressionEvaluator.NewArrayBoundsEvaluator<T>.<.ctor>b__3d);
        //            }
        //            this.bounds = Enumerable.Select<ExpressionEvaluator.Evaluator, ExpressionEvaluator.Evaluator<int>>(bounds, ExpressionEvaluator.NewArrayBoundsEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate3e).ToArray<ExpressionEvaluator.Evaluator<int>>();
        //        }

        //        public override T[] Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            <>c__DisplayClass40<T> class2;
        //            int[] lengths = Enumerable.Select<ExpressionEvaluator.Evaluator<int>, int>(this.bounds, new Func<ExpressionEvaluator.Evaluator<int>, int>(class2, (IntPtr) this.<Eval>b__3f)).ToArray<int>();
        //            return (T[]) Array.CreateInstance(typeof(T), lengths);
        //        }
        //    }

        //    public class NewArrayInitEvaluator<T> : ExpressionEvaluator.Evaluator<T[]>
        //    {
        //        private ExpressionEvaluator.Evaluator<T>[] initializers;

        //        public NewArrayInitEvaluator(ExpressionEvaluator.Evaluator[] initializers)
        //        {
        //            if (ExpressionEvaluator.NewArrayInitEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate3c == null)
        //            {
        //                ExpressionEvaluator.NewArrayInitEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate3c = new Func<ExpressionEvaluator.Evaluator, ExpressionEvaluator.Evaluator<T>>(null, (IntPtr) ExpressionEvaluator.NewArrayInitEvaluator<T>.<.ctor>b__3b);
        //            }
        //            this.initializers = Enumerable.Select<ExpressionEvaluator.Evaluator, ExpressionEvaluator.Evaluator<T>>(initializers, ExpressionEvaluator.NewArrayInitEvaluator<T>.CS$<>9__CachedAnonymousMethodDelegate3c).ToArray<ExpressionEvaluator.Evaluator<T>>();
        //        }

        //        public override T[] Eval(ExpressionEvaluator.EvaluatorState state)
        //        {
        //            T[] localArray = new T[this.initializers.Length];
        //            int index = 0;
        //            int length = this.initializers.Length;
        //            while (index < length)
        //            {
        //                localArray[index] = this.initializers[index].Eval(state);
        //                index++;
        //            }
        //            return localArray;
        //        }
        //    }

        public class NewEvaluator<T> : ExpressionEvaluator.Evaluator<T>
        {
            private Action<T> fnConstructor;

            public NewEvaluator(ConstructorInfo constructor)
            {
                if (constructor != null)
                {
                    this.fnConstructor = (Action<T>)ExpressionEvaluator.CreateDelegate(typeof(Action<T>), constructor);
                }
            }

            public override T Eval(ExpressionEvaluator.EvaluatorState state)
            {
                T local = default(T);
                if (this.fnConstructor != null)
                {
                    this.fnConstructor(local);
                }
                return local;
            }
        }

        public class NewEvaluator<T, A> : ExpressionEvaluator.Evaluator<T>
        {
            private Action<T, A> fnConstructor;
            private ExpressionEvaluator.Evaluator<A> opArg;

            public NewEvaluator(ConstructorInfo constructor, ExpressionEvaluator.Evaluator<A> opArg)
            {
                this.fnConstructor = (Action<T, A>)ExpressionEvaluator.CreateDelegate(typeof(Action<T, A>), constructor);
                this.opArg = opArg;
            }

            public override T Eval(ExpressionEvaluator.EvaluatorState state)
            {
                A local = this.opArg.Eval(state);
                T local2 = default(T);
                this.fnConstructor(local2, local);
                return local2;
            }
        }

        public class NewEvaluator<T, A1, A2> : ExpressionEvaluator.Evaluator<T>
        {
            private Action<T, A1, A2> fnConstructor;
            private ExpressionEvaluator.Evaluator<A1> opArg1;
            private ExpressionEvaluator.Evaluator<A2> opArg2;

            public NewEvaluator(ConstructorInfo constructor, ExpressionEvaluator.Evaluator<A1> opArg1, ExpressionEvaluator.Evaluator<A2> opArg2)
            {
                this.fnConstructor = (Action<T, A1, A2>)ExpressionEvaluator.CreateDelegate(typeof(Action<T, A1, A2>), constructor);
                this.opArg1 = opArg1;
                this.opArg2 = opArg2;
            }

            public override T Eval(ExpressionEvaluator.EvaluatorState state)
            {
                A1 local = this.opArg1.Eval(state);
                A2 local2 = this.opArg2.Eval(state);
                T local3 = default(T);
                this.fnConstructor(local3, local, local2);
                return local3;
            }
        }

        public class NewEvaluator<T, A1, A2, A3> : ExpressionEvaluator.Evaluator<T>
        {
            private Action<T, A1, A2, A3> fnConstructor;
            private ExpressionEvaluator.Evaluator<A1> opArg1;
            private ExpressionEvaluator.Evaluator<A2> opArg2;
            private ExpressionEvaluator.Evaluator<A3> opArg3;

            public NewEvaluator(ConstructorInfo constructor, ExpressionEvaluator.Evaluator<A1> opArg1, ExpressionEvaluator.Evaluator<A2> opArg2, ExpressionEvaluator.Evaluator<A3> opArg3)
            {
                this.fnConstructor = (Action<T, A1, A2, A3>)ExpressionEvaluator.CreateDelegate(typeof(Action<T, A1, A2, A3>), constructor);
                this.opArg1 = opArg1;
                this.opArg2 = opArg2;
                this.opArg3 = opArg3;
            }

            public override T Eval(ExpressionEvaluator.EvaluatorState state)
            {
                A1 local = this.opArg1.Eval(state);
                A2 local2 = this.opArg2.Eval(state);
                A3 local3 = this.opArg3.Eval(state);
                T local4 = default(T);
                this.fnConstructor(local4, local, local2, local3);
                return local4;
            }
        }

        public class NewEvaluator<T, A1, A2, A3, A4> : ExpressionEvaluator.Evaluator<T>
        {
            private Action<T, A1, A2, A3, A4> fnConstructor;
            private ExpressionEvaluator.Evaluator<A1> opArg1;
            private ExpressionEvaluator.Evaluator<A2> opArg2;
            private ExpressionEvaluator.Evaluator<A3> opArg3;
            private ExpressionEvaluator.Evaluator<A4> opArg4;

            public NewEvaluator(ConstructorInfo constructor, ExpressionEvaluator.Evaluator<A1> opArg1, ExpressionEvaluator.Evaluator<A2> opArg2, ExpressionEvaluator.Evaluator<A3> opArg3, ExpressionEvaluator.Evaluator<A4> opArg4)
            {
                this.fnConstructor = (Action<T, A1, A2, A3, A4>)ExpressionEvaluator.CreateDelegate(typeof(Action<T, A1, A2, A3, A4>), constructor);
                this.opArg1 = opArg1;
                this.opArg2 = opArg2;
                this.opArg3 = opArg3;
                this.opArg4 = opArg4;
            }

            public override T Eval(ExpressionEvaluator.EvaluatorState state)
            {
                A1 local = this.opArg1.Eval(state);
                A2 local2 = this.opArg2.Eval(state);
                A3 local3 = this.opArg3.Eval(state);
                A4 local4 = this.opArg4.Eval(state);
                T t = default(T);
                this.fnConstructor(t, local, local2, local3, local4);
                return t;
            }

            private delegate void Action<T, A1, A2, A3, A4>(T t, A1 a1, A2 a2, A3 a3, A4 a4);
        }
    }
    //public class NewEvaluatorN<T> : ExpressionEvaluator.Evaluator<T>
    //{
    //    private ConstructorInfo constructor;
    //    private ExpressionEvaluator.Evaluator[] opArgs;

    //    public NewEvaluatorN(ConstructorInfo constructor, ExpressionEvaluator.Evaluator[] opArgs)
    //    {
    //        this.constructor = constructor;
    //        this.opArgs = opArgs;
    //    }

    //    public override T Eval(ExpressionEvaluator.EvaluatorState state)
    //        {

    //            T local;
    //            object[] parameters = Enumerable.Select<ExpressionEvaluator.Evaluator, object>(this.opArgs, new Func<ExpressionEvaluator.Evaluator, object>(class2, (IntPtr) this.<Eval>b__38)).ToArray<object>();
    //            try
    //            {
    //                local = (T) this.constructor.Invoke(parameters);
    //            }
    //            catch (TargetInvocationException exception)
    //            {
    //                throw exception.InnerException;
    //            }
    //            return local;
    //        }
    //}

    //    public static class Operators
    //    {
    //        private static ILookup<string, MethodInfo> _methods;

    //        static Operators()
    //        {
    //            if (CS$<>9__CachedAnonymousMethodDelegate54 == null)
    //            {
    //                CS$<>9__CachedAnonymousMethodDelegate54 = new Func<MethodInfo, string>(null, (IntPtr) <.cctor>b__53);
    //            }
    //            _methods = Enumerable.ToLookup<MethodInfo, string>(typeof(ExpressionEvaluator.Operators).GetMethods(BindingFlags.Public | BindingFlags.Static), CS$<>9__CachedAnonymousMethodDelegate54);
    //        }

    //        public static byte Add(byte left, byte right)
    //        {
    //            return (byte) (left + right);
    //        }

    //        public static decimal Add(decimal left, decimal right)
    //        {
    //            return (left + right);
    //        }

    //        public static double Add(double left, double right)
    //        {
    //            return (left + right);
    //        }

    //        public static short Add(short left, short right)
    //        {
    //            return (short) (left + right);
    //        }

    //        public static int Add(int left, int right)
    //        {
    //            return (left + right);
    //        }

    //        public static long Add(long left, long right)
    //        {
    //            return (left + right);
    //        }

    //        public static sbyte Add(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left + right);
    //        }

    //        public static float Add(float left, float right)
    //        {
    //            return (left + right);
    //        }

    //        public static ushort Add(ushort left, ushort right)
    //        {
    //            return (ushort) (left + right);
    //        }

    //        public static uint Add(uint left, uint right)
    //        {
    //            return (left + right);
    //        }

    //        public static ulong Add(ulong left, ulong right)
    //        {
    //            return (left + right);
    //        }

    //        public static byte AddChecked(byte left, byte right)
    //        {
    //            return (byte) (left + right);
    //        }

    //        public static decimal AddChecked(decimal left, decimal right)
    //        {
    //            return (left + right);
    //        }

    //        public static double AddChecked(double left, double right)
    //        {
    //            return (left + right);
    //        }

    //        public static short AddChecked(short left, short right)
    //        {
    //            return (short) (left + right);
    //        }

    //        public static int AddChecked(int left, int right)
    //        {
    //            return (left + right);
    //        }

    //        public static long AddChecked(long left, long right)
    //        {
    //            return (left + right);
    //        }

    //        public static sbyte AddChecked(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left + right);
    //        }

    //        public static float AddChecked(float left, float right)
    //        {
    //            return (left + right);
    //        }

    //        public static ushort AddChecked(ushort left, ushort right)
    //        {
    //            return (ushort) (left + right);
    //        }

    //        public static uint AddChecked(uint left, uint right)
    //        {
    //            return (left + right);
    //        }

    //        public static ulong AddChecked(ulong left, ulong right)
    //        {
    //            return (left + right);
    //        }

    //        public static bool And(bool left, bool right)
    //        {
    //            return (left && right);
    //        }

    //        public static byte And(byte left, byte right)
    //        {
    //            return (byte) (left & right);
    //        }

    //        public static short And(short left, short right)
    //        {
    //            return (short) (left & right);
    //        }

    //        public static int And(int left, int right)
    //        {
    //            return (left & right);
    //        }

    //        public static long And(long left, long right)
    //        {
    //            return (left & right);
    //        }

    //        public static sbyte And(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left & right);
    //        }

    //        public static ushort And(ushort left, ushort right)
    //        {
    //            return (ushort) (left & right);
    //        }

    //        public static uint And(uint left, uint right)
    //        {
    //            return (left & right);
    //        }

    //        public static ulong And(ulong left, ulong right)
    //        {
    //            return (left & right);
    //        }

    //        public static byte ConvertCheckedToByte(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static byte ConvertCheckedToByte(decimal operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(double operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(short operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(int operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(long operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(sbyte operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(float operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(ushort operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(uint operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertCheckedToByte(ulong operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(decimal operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(double operand)
    //        {
    //            return (decimal) operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(short operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(int operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(long operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(float operand)
    //        {
    //            return (decimal) operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(uint operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertCheckedToDecimal(ulong operand)
    //        {
    //            return operand;
    //        }

    //        public static double ConvertCheckedToDouble(byte operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(decimal operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(double operand)
    //        {
    //            return operand;
    //        }

    //        public static double ConvertCheckedToDouble(short operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(int operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(long operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(sbyte operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(float operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(ushort operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(uint operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertCheckedToDouble(ulong operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static short ConvertCheckedToInt16(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static short ConvertCheckedToInt16(decimal operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertCheckedToInt16(double operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertCheckedToInt16(short operand)
    //        {
    //            return operand;
    //        }

    //        public static short ConvertCheckedToInt16(int operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertCheckedToInt16(long operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertCheckedToInt16(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static short ConvertCheckedToInt16(float operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertCheckedToInt16(ushort operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertCheckedToInt16(uint operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertCheckedToInt16(ulong operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static int ConvertCheckedToInt32(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertCheckedToInt32(decimal operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertCheckedToInt32(double operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertCheckedToInt32(short operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertCheckedToInt32(int operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertCheckedToInt32(long operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertCheckedToInt32(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertCheckedToInt32(float operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertCheckedToInt32(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertCheckedToInt32(uint operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertCheckedToInt32(ulong operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static long ConvertCheckedToInt64(byte operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(decimal operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(double operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(short operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(int operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(long operand)
    //        {
    //            return operand;
    //        }

    //        public static long ConvertCheckedToInt64(sbyte operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(float operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(ushort operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(uint operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertCheckedToInt64(ulong operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(byte operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(decimal operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(double operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(short operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(int operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(long operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(float operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(ushort operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(uint operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertCheckedToSByte(ulong operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static float ConvertCheckedToSingle(byte operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(decimal operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(double operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(short operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(int operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(long operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(sbyte operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(float operand)
    //        {
    //            return operand;
    //        }

    //        public static float ConvertCheckedToSingle(ushort operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(uint operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertCheckedToSingle(ulong operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(decimal operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(double operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(short operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(int operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(long operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(sbyte operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(float operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(uint operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertCheckedToUInt16(ulong operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(decimal operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(double operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(short operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(int operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(long operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(sbyte operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(float operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(uint operand)
    //        {
    //            return operand;
    //        }

    //        public static uint ConvertCheckedToUInt32(ulong operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(byte operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(decimal operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(double operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(short operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(int operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(long operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(sbyte operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(float operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(ushort operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(uint operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertCheckedToUInt64(ulong operand)
    //        {
    //            return operand;
    //        }

    //        public static byte ConvertToByte(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static byte ConvertToByte(decimal operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(double operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(short operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(int operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(long operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(sbyte operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(float operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(ushort operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(uint operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static byte ConvertToByte(ulong operand)
    //        {
    //            return (byte) operand;
    //        }

    //        public static decimal ConvertToDecimal(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(decimal operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(double operand)
    //        {
    //            return (decimal) operand;
    //        }

    //        public static decimal ConvertToDecimal(short operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(int operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(long operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(float operand)
    //        {
    //            return (decimal) operand;
    //        }

    //        public static decimal ConvertToDecimal(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(uint operand)
    //        {
    //            return operand;
    //        }

    //        public static decimal ConvertToDecimal(ulong operand)
    //        {
    //            return operand;
    //        }

    //        public static double ConvertToDouble(byte operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(decimal operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(double operand)
    //        {
    //            return operand;
    //        }

    //        public static double ConvertToDouble(short operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(int operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(long operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(sbyte operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(float operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(ushort operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(uint operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static double ConvertToDouble(ulong operand)
    //        {
    //            return (double) operand;
    //        }

    //        public static short ConvertToInt16(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static short ConvertToInt16(decimal operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertToInt16(double operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertToInt16(short operand)
    //        {
    //            return operand;
    //        }

    //        public static short ConvertToInt16(int operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertToInt16(long operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertToInt16(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static short ConvertToInt16(float operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertToInt16(ushort operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertToInt16(uint operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static short ConvertToInt16(ulong operand)
    //        {
    //            return (short) operand;
    //        }

    //        public static int ConvertToInt32(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertToInt32(decimal operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertToInt32(double operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertToInt32(short operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertToInt32(int operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertToInt32(long operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertToInt32(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertToInt32(float operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertToInt32(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static int ConvertToInt32(uint operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static int ConvertToInt32(ulong operand)
    //        {
    //            return (int) operand;
    //        }

    //        public static long ConvertToInt64(byte operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(decimal operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(double operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(short operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(int operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(long operand)
    //        {
    //            return operand;
    //        }

    //        public static long ConvertToInt64(sbyte operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(float operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(ushort operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(uint operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static long ConvertToInt64(ulong operand)
    //        {
    //            return (long) operand;
    //        }

    //        public static sbyte ConvertToSByte(byte operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(decimal operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(double operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(short operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(int operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(long operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(sbyte operand)
    //        {
    //            return operand;
    //        }

    //        public static sbyte ConvertToSByte(float operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(ushort operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(uint operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static sbyte ConvertToSByte(ulong operand)
    //        {
    //            return (sbyte) operand;
    //        }

    //        public static float ConvertToSingle(byte operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(decimal operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(double operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(short operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(int operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(long operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(sbyte operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(float operand)
    //        {
    //            return operand;
    //        }

    //        public static float ConvertToSingle(ushort operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(uint operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static float ConvertToSingle(ulong operand)
    //        {
    //            return (float) operand;
    //        }

    //        public static ushort ConvertToUInt16(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static ushort ConvertToUInt16(decimal operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(double operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(short operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(int operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(long operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(sbyte operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(float operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static ushort ConvertToUInt16(uint operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static ushort ConvertToUInt16(ulong operand)
    //        {
    //            return (ushort) operand;
    //        }

    //        public static uint ConvertToUInt32(byte operand)
    //        {
    //            return operand;
    //        }

    //        public static uint ConvertToUInt32(decimal operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertToUInt32(double operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertToUInt32(short operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertToUInt32(int operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertToUInt32(long operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertToUInt32(sbyte operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertToUInt32(float operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static uint ConvertToUInt32(ushort operand)
    //        {
    //            return operand;
    //        }

    //        public static uint ConvertToUInt32(uint operand)
    //        {
    //            return operand;
    //        }

    //        public static uint ConvertToUInt32(ulong operand)
    //        {
    //            return (uint) operand;
    //        }

    //        public static ulong ConvertToUInt64(byte operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(decimal operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(double operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(short operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(int operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(long operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(sbyte operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(float operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(ushort operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(uint operand)
    //        {
    //            return (ulong) operand;
    //        }

    //        public static ulong ConvertToUInt64(ulong operand)
    //        {
    //            return operand;
    //        }

    //        public static byte Divide(byte left, byte right)
    //        {
    //            return (byte) (left / right);
    //        }

    //        public static decimal Divide(decimal left, decimal right)
    //        {
    //            return (left / right);
    //        }

    //        public static double Divide(double left, double right)
    //        {
    //            return (left / right);
    //        }

    //        public static short Divide(short left, short right)
    //        {
    //            return (short) (left / right);
    //        }

    //        public static int Divide(int left, int right)
    //        {
    //            return (left / right);
    //        }

    //        public static long Divide(long left, long right)
    //        {
    //            return (left / right);
    //        }

    //        public static sbyte Divide(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left / right);
    //        }

    //        public static float Divide(float left, float right)
    //        {
    //            return (left / right);
    //        }

    //        public static ushort Divide(ushort left, ushort right)
    //        {
    //            return (ushort) (left / right);
    //        }

    //        public static uint Divide(uint left, uint right)
    //        {
    //            return (left / right);
    //        }

    //        public static ulong Divide(ulong left, ulong right)
    //        {
    //            return (left / right);
    //        }

    //        public static bool Equal(byte left, byte right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(decimal left, decimal right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(double left, double right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(short left, short right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(int left, int right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(long left, long right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(object left, object right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(sbyte left, sbyte right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(float left, float right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(ushort left, ushort right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(uint left, uint right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool Equal(ulong left, ulong right)
    //        {
    //            return (left == right);
    //        }

    //        public static bool ExclusiveOr(bool left, bool right)
    //        {
    //            return (left ^ right);
    //        }

    //        public static byte ExclusiveOr(byte left, byte right)
    //        {
    //            return (byte) (left ^ right);
    //        }

    //        public static short ExclusiveOr(short left, short right)
    //        {
    //            return (short) (left ^ right);
    //        }

    //        public static int ExclusiveOr(int left, int right)
    //        {
    //            return (left ^ right);
    //        }

    //        public static long ExclusiveOr(long left, long right)
    //        {
    //            return (left ^ right);
    //        }

    //        public static sbyte ExclusiveOr(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left ^ right);
    //        }

    //        public static ushort ExclusiveOr(ushort left, ushort right)
    //        {
    //            return (ushort) (left ^ right);
    //        }

    //        public static uint ExclusiveOr(uint left, uint right)
    //        {
    //            return (left ^ right);
    //        }

    //        public static ulong ExclusiveOr(ulong left, ulong right)
    //        {
    //            return (left ^ right);
    //        }

    //        public static IEnumerable<MethodInfo> GetOperatorMethods(string name)
    //        {
    //            return _methods[name];
    //        }

    //        public static bool GreaterThan(byte left, byte right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(decimal left, decimal right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(double left, double right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(short left, short right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(int left, int right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(long left, long right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(sbyte left, sbyte right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(float left, float right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(ushort left, ushort right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(uint left, uint right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThan(ulong left, ulong right)
    //        {
    //            return (left > right);
    //        }

    //        public static bool GreaterThanOrEqual(byte left, byte right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(decimal left, decimal right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(double left, double right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(short left, short right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(int left, int right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(long left, long right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(sbyte left, sbyte right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(float left, float right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(ushort left, ushort right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(uint left, uint right)
    //        {
    //            return (left >= right);
    //        }

    //        public static bool GreaterThanOrEqual(ulong left, ulong right)
    //        {
    //            return (left >= right);
    //        }

    //        public static byte LeftShift(byte left, byte right)
    //        {
    //            return (byte) (left << right);
    //        }

    //        public static short LeftShift(short left, short right)
    //        {
    //            return (short) (left << right);
    //        }

    //        public static int LeftShift(int left, int right)
    //        {
    //            return (left << right);
    //        }

    //        public static long LeftShift(long left, long right)
    //        {
    //            return (left << ((int) right));
    //        }

    //        public static sbyte LeftShift(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left << right);
    //        }

    //        public static ushort LeftShift(ushort left, ushort right)
    //        {
    //            return (ushort) (left << right);
    //        }

    //        public static uint LeftShift(uint left, uint right)
    //        {
    //            return (left << right);
    //        }

    //        public static ulong LeftShift(ulong left, ulong right)
    //        {
    //            return (left << ((int) right));
    //        }

    //        public static bool LessThan(byte left, byte right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(decimal left, decimal right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(double left, double right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(short left, short right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(int left, int right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(long left, long right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(sbyte left, sbyte right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(float left, float right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(ushort left, ushort right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(uint left, uint right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThan(ulong left, ulong right)
    //        {
    //            return (left < right);
    //        }

    //        public static bool LessThanOrEqual(byte left, byte right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(decimal left, decimal right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(double left, double right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(short left, short right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(int left, int right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(long left, long right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(sbyte left, sbyte right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(float left, float right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(ushort left, ushort right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(uint left, uint right)
    //        {
    //            return (left <= right);
    //        }

    //        public static bool LessThanOrEqual(ulong left, ulong right)
    //        {
    //            return (left <= right);
    //        }

    //        public static byte Modulo(byte left, byte right)
    //        {
    //            return (byte) (left % right);
    //        }

    //        public static decimal Modulo(decimal left, decimal right)
    //        {
    //            return decimal.op_Modulus(left, right);
    //        }

    //        public static double Modulo(double left, double right)
    //        {
    //            return (left % right);
    //        }

    //        public static short Modulo(short left, short right)
    //        {
    //            return (short) (left % right);
    //        }

    //        public static int Modulo(int left, int right)
    //        {
    //            return (left % right);
    //        }

    //        public static long Modulo(long left, long right)
    //        {
    //            return (left % right);
    //        }

    //        public static sbyte Modulo(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left % right);
    //        }

    //        public static float Modulo(float left, float right)
    //        {
    //            return (left % right);
    //        }

    //        public static ushort Modulo(ushort left, ushort right)
    //        {
    //            return (ushort) (left % right);
    //        }

    //        public static uint Modulo(uint left, uint right)
    //        {
    //            return (left % right);
    //        }

    //        public static ulong Modulo(ulong left, ulong right)
    //        {
    //            return (left % right);
    //        }

    //        public static byte Multiply(byte left, byte right)
    //        {
    //            return (byte) (left * right);
    //        }

    //        public static decimal Multiply(decimal left, decimal right)
    //        {
    //            return (left * right);
    //        }

    //        public static double Multiply(double left, double right)
    //        {
    //            return (left * right);
    //        }

    //        public static short Multiply(short left, short right)
    //        {
    //            return (short) (left * right);
    //        }

    //        public static int Multiply(int left, int right)
    //        {
    //            return (left * right);
    //        }

    //        public static long Multiply(long left, long right)
    //        {
    //            return (left * right);
    //        }

    //        public static sbyte Multiply(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left * right);
    //        }

    //        public static float Multiply(float left, float right)
    //        {
    //            return (left * right);
    //        }

    //        public static ushort Multiply(ushort left, ushort right)
    //        {
    //            return (ushort) (left * right);
    //        }

    //        public static uint Multiply(uint left, uint right)
    //        {
    //            return (left * right);
    //        }

    //        public static ulong Multiply(ulong left, ulong right)
    //        {
    //            return (left * right);
    //        }

    //        public static byte MultiplyChecked(byte left, byte right)
    //        {
    //            return (byte) (left * right);
    //        }

    //        public static decimal MultiplyChecked(decimal left, decimal right)
    //        {
    //            return (left * right);
    //        }

    //        public static double MultiplyChecked(double left, double right)
    //        {
    //            return (left * right);
    //        }

    //        public static short MultiplyChecked(short left, short right)
    //        {
    //            return (short) (left * right);
    //        }

    //        public static int MultiplyChecked(int left, int right)
    //        {
    //            return (left * right);
    //        }

    //        public static long MultiplyChecked(long left, long right)
    //        {
    //            return (left * right);
    //        }

    //        public static sbyte MultiplyChecked(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left * right);
    //        }

    //        public static float MultiplyChecked(float left, float right)
    //        {
    //            return (left * right);
    //        }

    //        public static ushort MultiplyChecked(ushort left, ushort right)
    //        {
    //            return (ushort) (left * right);
    //        }

    //        public static uint MultiplyChecked(uint left, uint right)
    //        {
    //            return (left * right);
    //        }

    //        public static ulong MultiplyChecked(ulong left, ulong right)
    //        {
    //            return (left * right);
    //        }

    //        public static byte Negate(byte operand)
    //        {
    //            return -operand;
    //        }

    //        public static decimal Negate(decimal operand)
    //        {
    //            return decimal.op_UnaryNegation(operand);
    //        }

    //        public static double Negate(double operand)
    //        {
    //            return -operand;
    //        }

    //        public static short Negate(short operand)
    //        {
    //            return -operand;
    //        }

    //        public static int Negate(int operand)
    //        {
    //            return -operand;
    //        }

    //        public static long Negate(long operand)
    //        {
    //            return -operand;
    //        }

    //        public static sbyte Negate(sbyte operand)
    //        {
    //            return -operand;
    //        }

    //        public static float Negate(float operand)
    //        {
    //            return -operand;
    //        }

    //        public static ushort Negate(ushort operand)
    //        {
    //            return -operand;
    //        }

    //        public static uint Negate(uint operand)
    //        {
    //            return (uint) -((ulong) operand);
    //        }

    //        public static ulong Negate(ulong operand)
    //        {
    //            return -operand;
    //        }

    //        public static decimal NegateChecked(decimal operand)
    //        {
    //            return decimal.op_UnaryNegation(operand);
    //        }

    //        public static double NegateChecked(double operand)
    //        {
    //            return -operand;
    //        }

    //        public static short NegateChecked(short operand)
    //        {
    //            return (short) (0 - operand);
    //        }

    //        public static int NegateChecked(int operand)
    //        {
    //            return (0 - operand);
    //        }

    //        public static long NegateChecked(long operand)
    //        {
    //            return (0L - operand);
    //        }

    //        public static sbyte NegateChecked(sbyte operand)
    //        {
    //            return (sbyte) (0 - operand);
    //        }

    //        public static float NegateChecked(float operand)
    //        {
    //            return -operand;
    //        }

    //        public static bool Not(bool operand)
    //        {
    //            return !operand;
    //        }

    //        public static byte Not(byte operand)
    //        {
    //            return ~operand;
    //        }

    //        public static short Not(short operand)
    //        {
    //            return ~operand;
    //        }

    //        public static int Not(int operand)
    //        {
    //            return ~operand;
    //        }

    //        public static long Not(long operand)
    //        {
    //            return ~operand;
    //        }

    //        public static sbyte Not(sbyte operand)
    //        {
    //            return ~operand;
    //        }

    //        public static ushort Not(ushort operand)
    //        {
    //            return ~operand;
    //        }

    //        public static uint Not(uint operand)
    //        {
    //            return ~operand;
    //        }

    //        public static ulong Not(ulong operand)
    //        {
    //            return ~operand;
    //        }

    //        public static bool NotEqual(byte left, byte right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(decimal left, decimal right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(double left, double right)
    //        {
    //            return !(left == right);
    //        }

    //        public static bool NotEqual(short left, short right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(int left, int right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(long left, long right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(object left, object right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(sbyte left, sbyte right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(float left, float right)
    //        {
    //            return !(left == right);
    //        }

    //        public static bool NotEqual(ushort left, ushort right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(uint left, uint right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool NotEqual(ulong left, ulong right)
    //        {
    //            return (left != right);
    //        }

    //        public static bool Or(bool left, bool right)
    //        {
    //            return (left || right);
    //        }

    //        public static byte Or(byte left, byte right)
    //        {
    //            return (byte) (left | right);
    //        }

    //        public static short Or(short left, short right)
    //        {
    //            return (short) (left | right);
    //        }

    //        public static int Or(int left, int right)
    //        {
    //            return (left | right);
    //        }

    //        public static long Or(long left, long right)
    //        {
    //            return (left | right);
    //        }

    //        public static sbyte Or(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left | right);
    //        }

    //        public static ushort Or(ushort left, ushort right)
    //        {
    //            return (ushort) (left | right);
    //        }

    //        public static uint Or(uint left, uint right)
    //        {
    //            return (left | right);
    //        }

    //        public static ulong Or(ulong left, ulong right)
    //        {
    //            return (left | right);
    //        }

    //        public static byte Power(byte left, byte right)
    //        {
    //            return (byte) Math.Pow((double) left, (double) right);
    //        }

    //        public static decimal Power(decimal left, decimal right)
    //        {
    //            return (decimal) Math.Pow((double) left, (double) right);
    //        }

    //        public static double Power(double left, double right)
    //        {
    //            return Math.Pow(left, right);
    //        }

    //        public static short Power(short left, short right)
    //        {
    //            return (short) Math.Pow((double) left, (double) right);
    //        }

    //        public static int Power(int left, int right)
    //        {
    //            return (int) Math.Pow((double) left, (double) right);
    //        }

    //        public static long Power(long left, long right)
    //        {
    //            return (long) Math.Pow((double) left, (double) right);
    //        }

    //        public static sbyte Power(sbyte left, sbyte right)
    //        {
    //            return (sbyte) Math.Pow((double) left, (double) right);
    //        }

    //        public static float Power(float left, float right)
    //        {
    //            return (float) Math.Pow((double) left, (double) right);
    //        }

    //        public static ushort Power(ushort left, ushort right)
    //        {
    //            return (ushort) Math.Pow((double) left, (double) right);
    //        }

    //        public static uint Power(uint left, uint right)
    //        {
    //            return (uint) Math.Pow((double) left, (double) right);
    //        }

    //        public static ulong Power(ulong left, ulong right)
    //        {
    //            return (ulong) Math.Pow((double) left, (double) right);
    //        }

    //        public static byte RightShift(byte left, byte right)
    //        {
    //            return (byte) (left >> right);
    //        }

    //        public static short RightShift(short left, short right)
    //        {
    //            return (short) (left >> right);
    //        }

    //        public static int RightShift(int left, int right)
    //        {
    //            return (left >> right);
    //        }

    //        public static long RightShift(long left, long right)
    //        {
    //            return (left >> ((int) right));
    //        }

    //        public static sbyte RightShift(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left >> right);
    //        }

    //        public static ushort RightShift(ushort left, ushort right)
    //        {
    //            return (ushort) (left >> right);
    //        }

    //        public static uint RightShift(uint left, uint right)
    //        {
    //            return (left >> right);
    //        }

    //        public static ulong RightShift(ulong left, ulong right)
    //        {
    //            return (left >> ((int) right));
    //        }

    //        public static byte Subtract(byte left, byte right)
    //        {
    //            return (byte) (left - right);
    //        }

    //        public static decimal Subtract(decimal left, decimal right)
    //        {
    //            return (left - right);
    //        }

    //        public static double Subtract(double left, double right)
    //        {
    //            return (left - right);
    //        }

    //        public static short Subtract(short left, short right)
    //        {
    //            return (short) (left - right);
    //        }

    //        public static int Subtract(int left, int right)
    //        {
    //            return (left - right);
    //        }

    //        public static long Subtract(long left, long right)
    //        {
    //            return (left - right);
    //        }

    //        public static sbyte Subtract(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left - right);
    //        }

    //        public static float Subtract(float left, float right)
    //        {
    //            return (left - right);
    //        }

    //        public static ushort Subtract(ushort left, ushort right)
    //        {
    //            return (ushort) (left - right);
    //        }

    //        public static uint Subtract(uint left, uint right)
    //        {
    //            return (left - right);
    //        }

    //        public static ulong Subtract(ulong left, ulong right)
    //        {
    //            return (left - right);
    //        }

    //        public static byte SubtractChecked(byte left, byte right)
    //        {
    //            return (byte) (left - right);
    //        }

    //        public static decimal SubtractChecked(decimal left, decimal right)
    //        {
    //            return (left - right);
    //        }

    //        public static double SubtractChecked(double left, double right)
    //        {
    //            return (left - right);
    //        }

    //        public static short SubtractChecked(short left, short right)
    //        {
    //            return (short) (left - right);
    //        }

    //        public static int SubtractChecked(int left, int right)
    //        {
    //            return (left - right);
    //        }

    //        public static long SubtractChecked(long left, long right)
    //        {
    //            return (left - right);
    //        }

    //        public static sbyte SubtractChecked(sbyte left, sbyte right)
    //        {
    //            return (sbyte) (left - right);
    //        }

    //        public static float SubtractChecked(float left, float right)
    //        {
    //            return (left - right);
    //        }

    //        public static ushort SubtractChecked(ushort left, ushort right)
    //        {
    //            return (ushort) (left - right);
    //        }

    //        public static uint SubtractChecked(uint left, uint right)
    //        {
    //            return (left - right);
    //        }

    //        public static ulong SubtractChecked(ulong left, ulong right)
    //        {
    //            return (left - right);
    //        }
    //    }

    //    public class OrElseEvaluator : ExpressionEvaluator.Evaluator<bool>
    //    {
    //        private ExpressionEvaluator.Evaluator<bool> left;
    //        private ExpressionEvaluator.Evaluator<bool> right;

    //        public OrElseEvaluator(ExpressionEvaluator.Evaluator<bool> left, ExpressionEvaluator.Evaluator<bool> right)
    //        {
    //            this.left = left;
    //            this.right = right;
    //        }

    //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            return (this.left.Eval(state) ? true : this.right.Eval(state));
    //        }
    //    }

    //    public class ParameterEvaluator<T> : ExpressionEvaluator.Evaluator<T>
    //    {
    //        private int index;

    //        public ParameterEvaluator(int index)
    //        {
    //            this.index = index;
    //        }

    //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            return state.GetValue<T>(this.index);
    //        }

    //        public override ExpressionEvaluator.Address<T> EvalAddress(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            return new ParameterAddress<T>(this.index);
    //        }

    //        private class ParameterAddress : ExpressionEvaluator.Address<T>
    //        {
    //            private int index;

    //            public ParameterAddress(int index)
    //            {
    //                this.index = index;
    //            }

    //            public override T GetValue(ExpressionEvaluator.EvaluatorState state)
    //            {
    //                return state.GetValue<T>(this.index);
    //            }

    //            public override void SetValue(ExpressionEvaluator.EvaluatorState state, T value)
    //            {
    //                state.SetValue<T>(this.index, value);
    //            }
    //        }
    //    }

    //    public class PropertyAccessEvaluator<T, V> : ExpressionEvaluator.Evaluator<V>
    //    {
    //        private Func<T, V> fnGetter;
    //        private ExpressionEvaluator.Evaluator<T> operand;

    //        public PropertyAccessEvaluator(ExpressionEvaluator.Evaluator<T> operand, PropertyInfo property)
    //        {
    //            this.operand = operand;
    //            this.fnGetter = (Func<T, V>) Delegate.CreateDelegate(typeof(Func<T, V>), property.GetGetMethod(true));
    //        }

    //        public override V Eval(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            T local = (this.operand != null) ? this.operand.Eval(state) : default(T);
    //            return this.fnGetter.Invoke(local);
    //        }
    //    }

    //    public class PropertyAssignmentInitializer<T, V> : ExpressionEvaluator.Initializer<T> where T: class
    //    {
    //        private ExpressionEvaluator.Evaluator<V> evaluator;
    //        private Action<T, V> fnSetter;

    //        public PropertyAssignmentInitializer(PropertyInfo property, ExpressionEvaluator.Evaluator<V> evaluator)
    //        {
    //            this.fnSetter = (Action<T, V>) Delegate.CreateDelegate(typeof(Action<T, V>), property.GetSetMethod(true));
    //            this.evaluator = evaluator;
    //        }

    //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
    //        {
    //            V local = this.evaluator.Eval(state);
    //            this.fnSetter(instance, local);
    //        }
    //    }

    //    public class PropertyMemberInitializer<T, V> : ExpressionEvaluator.Initializer<T> where T: class
    //    {
    //        private Func<T, V> fnGetter;
    //        private ExpressionEvaluator.Initializer<V>[] initializers;

    //        public PropertyMemberInitializer(PropertyInfo property, ExpressionEvaluator.Initializer[] initializers)
    //        {
    //            this.fnGetter = (Func<T, V>) Delegate.CreateDelegate(typeof(Func<T, V>), property.GetGetMethod(true));
    //            if (ExpressionEvaluator.PropertyMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4c == null)
    //            {
    //                ExpressionEvaluator.PropertyMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4c = new Func<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(null, (IntPtr) ExpressionEvaluator.PropertyMemberInitializer<T, V>.<.ctor>b__4b);
    //            }
    //            this.initializers = Enumerable.Select<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(initializers, ExpressionEvaluator.PropertyMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4c).ToArray<ExpressionEvaluator.Initializer<V>>();
    //        }

    //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
    //        {
    //            V local = this.fnGetter.Invoke(instance);
    //            int index = 0;
    //            int length = this.initializers.Length;
    //            while (index < length)
    //            {
    //                this.initializers[index].Init(state, ref local);
    //                index++;
    //            }
    //        }
    //    }

    //    public class QuoteEvaluator<T> : ExpressionEvaluator.Evaluator<T> where T: Expression
    //    {
    //        private Expression expression;
    //        private Dictionary<ParameterExpression, int> external;

    //        public QuoteEvaluator(Expression expression, Dictionary<ParameterExpression, int> external)
    //        {
    //            this.expression = expression;
    //            this.external = external;
    //        }

    //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            if (this.external.Count > 0)
    //            {
    //                return (T) QuoteRewriter<T>.Rewrite(this.external, state, this.expression);
    //            }
    //            return (T) this.expression;
    //        }

    //        private class QuoteRewriter : OldExpressionVisitor
    //        {
    //            private Dictionary<ParameterExpression, int> external;
    //            private ExpressionEvaluator.EvaluatorState state;

    //            private QuoteRewriter(Dictionary<ParameterExpression, int> external, ExpressionEvaluator.EvaluatorState state)
    //            {
    //                this.external = external;
    //                this.state = state;
    //            }

    //            internal static Expression Rewrite(Dictionary<ParameterExpression, int> external, ExpressionEvaluator.EvaluatorState state, Expression expression)
    //            {
    //                return new ExpressionEvaluator.QuoteEvaluator<T>.QuoteRewriter(external, state).Visit(expression);
    //            }

    //            internal override Expression VisitParameter(ParameterExpression p)
    //            {
    //                int num;
    //                if (this.external.TryGetValue(p, out num))
    //                {
    //                    return Expression.Constant(this.state.GetBoxedValue(num), p.Type);
    //                }
    //                return p;
    //            }
    //        }
    //    }

    //    public class TypeAsEvaluator<S, T> : ExpressionEvaluator.Evaluator<T> where S: class where T: class
    //    {
    //        private ExpressionEvaluator.Evaluator<S> operand;

    //        public TypeAsEvaluator(ExpressionEvaluator.Evaluator<S> operand)
    //        {
    //            this.operand = operand;
    //        }

    //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            return (this.operand.Eval(state) as T);
    //        }
    //    }

    //    public class TypeIsEvaluator : ExpressionEvaluator.Evaluator<bool>
    //    {
    //        private ExpressionEvaluator.Evaluator thing;
    //        private Type type;

    //        public TypeIsEvaluator(ExpressionEvaluator.Evaluator thing, Type type)
    //        {
    //            this.thing = thing;
    //            this.type = type;
    //        }

    //        public override bool Eval(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            object obj2 = this.thing.EvalBoxed(state);
    //            if (obj2 == null)
    //            {
    //                return false;
    //            }
    //            Type type = obj2.GetType();
    //            return ((type == this.type) || type.IsSubclassOf(this.type));
    //        }
    //    }

    //    public class UnaryEvaluator<S, T> : ExpressionEvaluator.Evaluator<T>
    //    {
    //        private Func<S, T> op;
    //        private ExpressionEvaluator.Evaluator<S> operand;

    //        public UnaryEvaluator(ExpressionEvaluator.Evaluator<S> operand, Func<S, T> op)
    //        {
    //            this.operand = operand;
    //            this.op = op;
    //        }

    //        public override T Eval(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            return this.op.Invoke(this.operand.Eval(state));
    //        }
    //    }

    //    public class ValueAddress<T> : ExpressionEvaluator.Address<T>
    //    {
    //        private T value;

    //        public ValueAddress(T value)
    //        {
    //            this.value = value;
    //        }

    //        public override T GetValue(ExpressionEvaluator.EvaluatorState state)
    //        {
    //            return this.value;
    //        }

    //        public override void SetValue(ExpressionEvaluator.EvaluatorState state, T value)
    //        {
    //            this.value = value;
    //        }
    //    }

    //    public class ValueTypeFieldAssignmentInitializer<T> : ExpressionEvaluator.Initializer<T> where T: struct
    //    {
    //        private ExpressionEvaluator.Evaluator evaluator;
    //        private FieldInfo field;

    //        public ValueTypeFieldAssignmentInitializer(FieldInfo field, ExpressionEvaluator.Evaluator evaluator)
    //        {
    //            this.field = field;
    //            this.evaluator = evaluator;
    //        }

    //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
    //        {
    //            object obj2 = this.evaluator.EvalBoxed(state);
    //            object obj3 = (T) instance;
    //            this.field.SetValue(obj3, obj2);
    //            instance = (T) obj3;
    //        }
    //    }

    //    public class ValueTypeFieldMemberInitializer<T, V> : ExpressionEvaluator.Initializer<T> where T: struct
    //    {
    //        private FieldInfo field;
    //        private ExpressionEvaluator.Initializer<V>[] initializers;
    //        private bool valueIsValueType;

    //        public ValueTypeFieldMemberInitializer(FieldInfo field, ExpressionEvaluator.Initializer[] initializers)
    //        {
    //            this.field = field;
    //            if (ExpressionEvaluator.ValueTypeFieldMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4a == null)
    //            {
    //                ExpressionEvaluator.ValueTypeFieldMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4a = new Func<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(null, (IntPtr) ExpressionEvaluator.ValueTypeFieldMemberInitializer<T, V>.<.ctor>b__49);
    //            }
    //            this.initializers = Enumerable.Select<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(initializers, ExpressionEvaluator.ValueTypeFieldMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4a).ToArray<ExpressionEvaluator.Initializer<V>>();
    //            this.valueIsValueType = field.FieldType.IsValueType;
    //        }

    //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
    //        {
    //            object obj2 = (T) instance;
    //            V local = (V) this.field.GetValue(obj2);
    //            int index = 0;
    //            int length = this.initializers.Length;
    //            while (index < length)
    //            {
    //                this.initializers[index].Init(state, ref local);
    //                index++;
    //            }
    //            if (this.valueIsValueType)
    //            {
    //                this.field.SetValue(obj2, local);
    //            }
    //            instance = (T) obj2;
    //        }
    //    }

    //    public class ValueTypePropertyAssignmentInitializer<T, V> : ExpressionEvaluator.Initializer<T> where T: struct
    //    {
    //        private ExpressionEvaluator.Evaluator<V> evaluator;
    //        private Assigner<T, V> fnSetter;

    //        public ValueTypePropertyAssignmentInitializer(PropertyInfo property, ExpressionEvaluator.Evaluator<V> evaluator)
    //        {
    //            this.fnSetter = (Assigner<T, V>) Delegate.CreateDelegate(typeof(Assigner<T, V>), property.GetSetMethod(true));
    //            this.evaluator = evaluator;
    //        }

    //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
    //        {
    //            V local = this.evaluator.Eval(state);
    //            this.fnSetter(ref instance, local);
    //        }

    //        private delegate void Assigner(ref T instance, V value);
    //    }

    //    public class ValueTypePropertyMemberInitializer<T, V> : ExpressionEvaluator.Initializer<T> where T: struct
    //    {
    //        private Getter<T, V> fnGetter;
    //        private ExpressionEvaluator.Initializer<V>[] initializers;

    //        public ValueTypePropertyMemberInitializer(PropertyInfo property, ExpressionEvaluator.Initializer[] initializers)
    //        {
    //            this.fnGetter = (Getter<T, V>) Delegate.CreateDelegate(typeof(Getter<T, V>), property.GetGetMethod(true));
    //            if (ExpressionEvaluator.ValueTypePropertyMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4e == null)
    //            {
    //                ExpressionEvaluator.ValueTypePropertyMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4e = new Func<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(null, (IntPtr) ExpressionEvaluator.ValueTypePropertyMemberInitializer<T, V>.<.ctor>b__4d);
    //            }
    //            this.initializers = Enumerable.Select<ExpressionEvaluator.Initializer, ExpressionEvaluator.Initializer<V>>(initializers, ExpressionEvaluator.ValueTypePropertyMemberInitializer<T, V>.CS$<>9__CachedAnonymousMethodDelegate4e).ToArray<ExpressionEvaluator.Initializer<V>>();
    //        }

    //        public override void Init(ExpressionEvaluator.EvaluatorState state, ref T instance)
    //        {
    //            V local = this.fnGetter(ref instance);
    //            int index = 0;
    //            int length = this.initializers.Length;
    //            while (index < length)
    //            {
    //                this.initializers[index].Init(state, ref local);
    //                index++;
    //            }
    //        }

    //        private delegate V Getter(ref T instance);
    //    }
    //}
}

