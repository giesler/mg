namespace System.Data.Services.Client
{
    using IQToolkit;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal class ProjectionPlanCompiler : ALinqExpressionVisitor
    {
        private readonly Dictionary<Expression, ExpressionAnnotation> annotations = new Dictionary<Expression, ExpressionAnnotation>(ReferenceEqualityComparer<Expression>.Instance);
        private int identifierId;
        private readonly ParameterExpression materializerExpression = Expression.Parameter(typeof(object), "mat");
        private readonly Dictionary<Expression, Expression> normalizerRewrites;
        private ProjectionPathBuilder pathBuilder;
        private bool topLevelProjectionFound;

        private ProjectionPlanCompiler(Dictionary<Expression, Expression> normalizerRewrites)
        {
            this.normalizerRewrites = normalizerRewrites;
            this.pathBuilder = new ProjectionPathBuilder();
        }

        private Expression CallCheckValueForPathIsNull(Expression entry, Expression entryType, ProjectionPath path)
        {
            Expression key = CallMaterializer("ProjectionCheckValueForPathIsNull", new Expression[] { entry, entryType, Expression.Constant(path, typeof(object)) });
            ExpressionAnnotation annotation = new ExpressionAnnotation();
            annotation.Segment = path[path.Count - 1];
            this.annotations.Add(key, annotation);
            return key;
        }

        private static Expression CallMaterializer(string methodName, params Expression[] arguments)
        {
            return CallMaterializerWithType(methodName, null, arguments);
        }

        private static Expression CallMaterializerWithType(string methodName, Type[] typeArguments, params Expression[] arguments)
        {
            Debug.Assert(methodName != null, "methodName != null");
            Debug.Assert(arguments != null, "arguments != null");
            MethodInfo method = typeof(AtomMaterializerInvoker).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            Debug.Assert(method != null, "method != null - found " + methodName);
            if (typeArguments != null)
            {
                method = method.MakeGenericMethod(typeArguments);
            }
            return Expression.Call(method, arguments);
        }

        private Expression CallValueForPath(Expression entry, Expression entryType, ProjectionPath path)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(path != null, "path != null");
            Expression key = CallMaterializer("ProjectionValueForPath", new Expression[] { this.materializerExpression, entry, entryType, Expression.Constant(path, typeof(object)) });
            ExpressionAnnotation annotation = new ExpressionAnnotation();
            annotation.Segment = path[path.Count - 1];
            this.annotations.Add(key, annotation);
            return key;
        }

        private Expression CallValueForPathWithType(Expression entry, Expression entryType, ProjectionPath path, Type type)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(path != null, "path != null");
            Expression key = Expression.Convert(this.CallValueForPath(entry, entryType, path), type);
            ExpressionAnnotation annotation = new ExpressionAnnotation();
            annotation.Segment = path[path.Count - 1];
            this.annotations.Add(key, annotation);
            return key;
        }

        internal static ProjectionPlan CompilePlan(LambdaExpression projection, Dictionary<Expression, Expression> normalizerRewrites)
        {
            Debug.Assert(projection != null, "projection != null");
            Debug.Assert(projection.Parameters.Count == 1, "projection.Parameters.Count == 1");
            Debug.Assert(((((projection.Body.NodeType == ExpressionType.Constant) || (projection.Body.NodeType == ExpressionType.MemberInit)) || ((projection.Body.NodeType == ExpressionType.MemberAccess) || (projection.Body.NodeType == ExpressionType.Convert))) || (projection.Body.NodeType == ExpressionType.ConvertChecked)) || (projection.Body.NodeType == ExpressionType.New), "projection.Body.NodeType == Constant, MemberInit, MemberAccess, Convert(Checked) New");
            Expression expression = new ProjectionPlanCompiler(normalizerRewrites).Visit(projection);
            ProjectionPlan plan = new ProjectionPlan();
            plan.Plan = (Func<object, object, Type, object>) ExpressionEvaluator.CreateDelegate((LambdaExpression) expression);
            plan.ProjectedType = projection.Body.Type;
            plan.SourceProjection = projection;
            plan.TargetProjection = expression;
            return plan;
        }

        private Expression GetDeepestEntry(Expression[] path)
        {
            Debug.Assert(path.Length > 1, "path.Length > 1");
            Expression expression = null;
            int index = 1;
            do
            {
                Expression[] arguments = new Expression[] { expression ?? this.pathBuilder.ParameterEntryInScope, Expression.Constant(((MemberExpression) path[index]).Member.Name, typeof(string)) };
                expression = CallMaterializer("ProjectionGetEntry", arguments);
                index++;
            }
            while (index < path.Length);
            return expression;
        }

        private Expression GetExpressionBeforeNormalization(Expression expression)
        {
            Expression expression2;
            Debug.Assert(expression != null, "expression != null");
            if ((this.normalizerRewrites != null) && this.normalizerRewrites.TryGetValue(expression, out expression2))
            {
                expression = expression2;
            }
            return expression;
        }

        private Expression RebindConditionalNullCheck(ConditionalExpression conditional, ResourceBinder.PatternRules.MatchNullCheckResult nullCheck)
        {
            ExpressionAnnotation annotation;
            Debug.Assert(conditional != null, "conditional != null");
            Debug.Assert(nullCheck.Match, "nullCheck.Match -- otherwise no reason to call this rebind method");
            Expression key = this.Visit(nullCheck.TestToNullExpression);
            Expression expression2 = this.Visit(nullCheck.AssignExpression);
            if (!this.annotations.TryGetValue(key, out annotation))
            {
                return base.VisitConditional(conditional);
            }
            ProjectionPathSegment segment = annotation.Segment;
            Expression test = this.CallCheckValueForPathIsNull(segment.StartPath.RootEntry, segment.StartPath.ExpectedRootType, segment.StartPath);
            Expression ifTrue = Expression.Constant(null, expression2.Type);
            Expression ifFalse = expression2;
            return Expression.Condition(test, ifTrue, ifFalse);
        }

        private Expression RebindEntityMemberInit(MemberInitExpression init)
        {
              Expression[] expressionsToTargetEntity;
    Expression deepestEntry;
    Expression expectedParamTypeInScope;
    ParameterExpression expression5;
    ParameterExpression expression6;
    ProjectionPath path;
    Debug.Assert(init != null, "init != null");
    Debug.Assert(init.Bindings.Count > 0, "init.Bindings.Count > 0 -- otherwise this is just empty construction");
    if (!this.pathBuilder.HasRewrites)
    {
        expressionsToTargetEntity = MemberAssignmentAnalysis.Analyze(this.pathBuilder.LambdaParameterInScope, ((MemberAssignment) init.Bindings[0]).Expression).GetExpressionsToTargetEntity();
        Debug.Assert(expressionsToTargetEntity.Length != 0, "expressions.Length != 0 -- otherwise there is no correlation to parameter in entity member init");
    }
    else
    {
        expressionsToTargetEntity = MemberAssignmentAnalysis.EmptyExpressionArray;
    }
    Expression parameterEntryInScope = this.pathBuilder.ParameterEntryInScope;
    List<string> list = new List<string>();
    List<Func<object, object, Type, object>> list2 = new List<Func<object, object, Type, object>>();
    Type type = init.NewExpression.Type;
    Expression expression2 = Expression.Constant(type, typeof(Type));
    string[] names = expressionsToTargetEntity.Skip<Expression>(1).Select<Expression, string>(delegate (Expression e) {
        return ((MemberExpression) e).Member.Name;
    }).ToArray<string>();
    if (expressionsToTargetEntity.Length <= 1)
    {
        deepestEntry = this.pathBuilder.ParameterEntryInScope;
        expectedParamTypeInScope = this.pathBuilder.ExpectedParamTypeInScope;
        expression5 = (ParameterExpression) this.pathBuilder.ParameterEntryInScope;
        expression6 = (ParameterExpression) this.pathBuilder.ExpectedParamTypeInScope;
    }
    else
    {
        deepestEntry = this.GetDeepestEntry(expressionsToTargetEntity);
        expectedParamTypeInScope = expression2;
        expression5 = Expression.Parameter(typeof(object), "subentry" + this.identifierId++);
        expression6 = (ParameterExpression) this.pathBuilder.ExpectedParamTypeInScope;
        path = new ProjectionPath((ParameterExpression) this.pathBuilder.LambdaParameterInScope, this.pathBuilder.ExpectedParamTypeInScope, this.pathBuilder.ParameterEntryInScope, expressionsToTargetEntity.Skip<Expression>(1));
        ExpressionAnnotation annotation = new ExpressionAnnotation();
        annotation.Segment = path[path.Count - 1];
        this.annotations.Add(deepestEntry, annotation);
        ExpressionAnnotation annotation2 = new ExpressionAnnotation();
        annotation2.Segment = path[path.Count - 1];
        this.annotations.Add(expression5, annotation2);
        this.pathBuilder.RegisterRewrite(this.pathBuilder.LambdaParameterInScope, names, expression5);
    }
    int num = 0;
    while (num < init.Bindings.Count)
    {
        LambdaExpression expression7;
        Expression expression10;
        ParameterExpression[] expressionArray2;
        MemberAssignment assignment = (MemberAssignment) init.Bindings[num];
        list.Add(assignment.Member.Name);
        if (ClientType.CheckElementTypeIsEntity(assignment.Member.ReflectedType) && (assignment.Expression.NodeType == ExpressionType.MemberInit))
        {
            ExpressionAnnotation annotation3;
            Expression expression8 = CallMaterializer("ProjectionGetEntry", new Expression[] { parameterEntryInScope, Expression.Constant(assignment.Member.Name, typeof(string)) });
            ParameterExpression key = Expression.Parameter(typeof(object), "subentry" + this.identifierId++);
            if (this.annotations.TryGetValue(this.pathBuilder.ParameterEntryInScope, out annotation3))
            {
                path = new ProjectionPath((ParameterExpression) this.pathBuilder.LambdaParameterInScope, this.pathBuilder.ExpectedParamTypeInScope, parameterEntryInScope);
                path.AddRange(annotation3.Segment.StartPath);

               
            }
            else
            {
                path = new ProjectionPath((ParameterExpression) this.pathBuilder.LambdaParameterInScope, this.pathBuilder.ExpectedParamTypeInScope, parameterEntryInScope, expressionsToTargetEntity.Skip<Expression>(1));
            }
            ProjectionPathSegment item = new ProjectionPathSegment(path, assignment.Member.Name, assignment.Member.ReflectedType);
            path.Add(item);
            string[] strArray2 = path.Where<ProjectionPathSegment>(delegate (ProjectionPathSegment m) {
                return (m.Member != null);
            }).Select<ProjectionPathSegment, string>(delegate (ProjectionPathSegment m) {
                return m.Member;
            }).ToArray<string>();
            ExpressionAnnotation annotation4 = new ExpressionAnnotation();
            annotation4.Segment = item;
            this.annotations.Add(key, annotation4);
            this.pathBuilder.RegisterRewrite(this.pathBuilder.LambdaParameterInScope, strArray2, key);
            expression10 = this.Visit(assignment.Expression);
            this.pathBuilder.RevokeRewrite(this.pathBuilder.LambdaParameterInScope, strArray2);
            this.annotations.Remove(key);
            expression10 = Expression.Convert(expression10, typeof(object));
            expressionArray2 = new ParameterExpression[] { this.materializerExpression, key, expression6 };
            expression7 = Expression.Lambda(expression10, expressionArray2);
            Expression[] arguments = new Expression[] { this.materializerExpression, expression8, expression6 };
            ParameterExpression[] parameters = new ParameterExpression[] { this.materializerExpression, (ParameterExpression) parameterEntryInScope, expression6 };
            expression7 = Expression.Lambda(Expression.Invoke(expression7, arguments), parameters);
        }
        else
        {
            expression10 = Expression.Convert(this.Visit(assignment.Expression), typeof(object));
            expressionArray2 = new ParameterExpression[] { this.materializerExpression, expression5, expression6 };
            expression7 = Expression.Lambda(expression10, expressionArray2);
        }
        list2.Add((Func<object, object, Type, object>) ExpressionEvaluator.CreateDelegate(expression7));
        num++;
    }
    for (num = 1; num < expressionsToTargetEntity.Length; num++)
    {
        this.pathBuilder.RevokeRewrite(this.pathBuilder.LambdaParameterInScope, names);
        this.annotations.Remove(deepestEntry);
        this.annotations.Remove(expression5);
    }
    return Expression.Convert(CallMaterializer("ProjectionInitializeEntity", new Expression[] { this.materializerExpression, deepestEntry, expectedParamTypeInScope, expression2, Expression.Constant(list.ToArray()), Expression.Constant(list2.ToArray()) }), type);

        }

        private Expression RebindMemberAccess(MemberExpression m, ExpressionAnnotation baseAnnotation)
        {
            Debug.Assert(m != null, "m != null");
            Debug.Assert(baseAnnotation != null, "baseAnnotation != null");
            Expression expression = m.Expression;
            Expression rewrite = this.pathBuilder.GetRewrite(expression);
            if (rewrite != null)
            {
                Expression expectedRootType = Expression.Constant(expression.Type, typeof(Type));
                ProjectionPath startPath = new ProjectionPath(rewrite as ParameterExpression, expectedRootType, rewrite);
                ProjectionPathSegment segment2 = new ProjectionPathSegment(startPath, m.Member.Name, m.Type);
                startPath.Add(segment2);
                return this.CallValueForPathWithType(rewrite, expectedRootType, startPath, m.Type);
            }
            ProjectionPathSegment item = new ProjectionPathSegment(baseAnnotation.Segment.StartPath, m.Member.Name, m.Type);
            baseAnnotation.Segment.StartPath.Add(item);
            return this.CallValueForPathWithType(baseAnnotation.Segment.StartPath.RootEntry, baseAnnotation.Segment.StartPath.ExpectedRootType, baseAnnotation.Segment.StartPath, m.Type);
        }

        private Expression RebindMethodCallForMemberSelect(MethodCallExpression call)
        {
            ExpressionAnnotation annotation;
            Debug.Assert(call != null, "call != null");
            Debug.Assert(call.Method.Name == "Select", "call.Method.Name == 'Select'");
            Debug.Assert(call.Object == null, "call.Object == null -- otherwise this isn't a call to a static Select method");
            Debug.Assert(call.Arguments.Count == 2, "call.Arguments.Count == 2 -- otherwise this isn't the expected Select() call on IQueryable");
            Expression key = null;
            Expression expression2 = this.Visit(call.Arguments[0]);
            this.annotations.TryGetValue(expression2, out annotation);
            if (annotation != null)
            {
                Expression expression3 = this.Visit(call.Arguments[1]);
                Type type = call.Method.ReturnType.GetGenericArguments()[0];
                key = CallMaterializer("ProjectionSelect", new Expression[] { this.materializerExpression, this.pathBuilder.ParameterEntryInScope, this.pathBuilder.ExpectedParamTypeInScope, Expression.Constant(type, typeof(Type)), Expression.Constant(annotation.Segment.StartPath, typeof(object)), expression3 });
                this.annotations.Add(key, annotation);
                key = CallMaterializerWithType("EnumerateAsElementType", new Type[] { type }, new Expression[] { key });
                this.annotations.Add(key, annotation);
            }
            if (key == null)
            {
                key = base.VisitMethodCall(call);
            }
            return key;
        }

        private Expression RebindMethodCallForMemberToList(MethodCallExpression call)
        {
            ExpressionAnnotation annotation;
            Debug.Assert(call != null, "call != null");
            Debug.Assert(call.Object == null, "call.Object == null -- otherwise this isn't a call to a static ToList method");
            Debug.Assert(call.Method.Name == "ToList", "call.Method.Name == 'ToList'");
            Debug.Assert(call.Arguments.Count == 1, "call.Arguments.Count == 1 -- otherwise this isn't the expected ToList() call on IEnumerable");
            Expression key = this.Visit(call.Arguments[0]);
            if (this.annotations.TryGetValue(key, out annotation))
            {
                key = this.TypedEnumerableToList(key, call.Type);
                this.annotations.Add(key, annotation);
            }
            return key;
        }

        private Expression RebindMethodCallForNewSequence(MethodCallExpression call)
        {
            ExpressionAnnotation annotation;
            Debug.Assert(call != null, "call != null");
            Debug.Assert(ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(call), "ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(call)");
            Debug.Assert(call.Object == null, "call.Object == null -- otherwise this isn't the supported Select or ToList methods");
            Expression key = null;
            if (call.Method.Name == "Select")
            {
                Debug.Assert(call.Arguments.Count == 2, "call.Arguments.Count == 2 -- otherwise this isn't the argument we expected");
                Expression expression2 = this.Visit(call.Arguments[0]);
                this.annotations.TryGetValue(expression2, out annotation);
                if (annotation != null)
                {
                    Expression expression3 = this.Visit(call.Arguments[1]);
                    Type type = call.Method.ReturnType.GetGenericArguments()[0];
                    key = CallMaterializer("ProjectionSelect", new Expression[] { this.materializerExpression, this.pathBuilder.ParameterEntryInScope, this.pathBuilder.ExpectedParamTypeInScope, Expression.Constant(type, typeof(Type)), Expression.Constant(annotation.Segment.StartPath, typeof(object)), expression3 });
                    this.annotations.Add(key, annotation);
                    key = CallMaterializerWithType("EnumerateAsElementType", new Type[] { type }, new Expression[] { key });
                    this.annotations.Add(key, annotation);
                }
            }
            else
            {
                Debug.Assert(call.Method.Name == "ToList", "call.Method.Name == 'ToList'");
                Expression expression4 = this.Visit(call.Arguments[0]);
                if (this.annotations.TryGetValue(expression4, out annotation))
                {
                    key = this.TypedEnumerableToList(expression4, call.Type);
                    this.annotations.Add(key, annotation);
                }
            }
            if (key == null)
            {
                key = base.VisitMethodCall(call);
            }
            return key;
        }

        private NewExpression RebindNewExpressionForDataServiceCollectionOfT(NewExpression nex)
        {
            Debug.Assert(nex != null, "nex != null");
            Debug.Assert(ResourceBinder.PatternRules.MatchNewDataServiceCollectionOfT(nex), "Called should have checked that the 'new' was for our collection type");
            NewExpression key = base.VisitNew(nex);
            ExpressionAnnotation annotation = null;
            if (key != null)
            {
                ConstructorInfo constructor = nex.Type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First<ConstructorInfo>(delegate(ConstructorInfo c)
                {
                    return (c.GetParameters().Length == 7) && (c.GetParameters()[0].ParameterType == typeof(object));
                });
                Type type = typeof(IEnumerable<>).MakeGenericType(new Type[] { nex.Type.GetGenericArguments()[0] });
                if (((key.Arguments.Count == 1) && (key.Constructor == nex.Type.GetConstructor(new Type[] { type }))) && this.annotations.TryGetValue(key.Arguments[0], out annotation))
                {
                    key = Expression.New(constructor, new Expression[] { this.materializerExpression, Expression.Constant(null, typeof(DataServiceContext)), key.Arguments[0], Expression.Constant(TrackingMode.AutoChangeTracking, typeof(TrackingMode)), Expression.Constant(null, typeof(string)), Expression.Constant(null, typeof(Func<EntityChangedParams, bool>)), Expression.Constant(null, typeof(Func<EntityCollectionChangedParams, bool>)) });
                }
                else if ((key.Arguments.Count == 2) && this.annotations.TryGetValue(key.Arguments[0], out annotation))
                {
                    key = Expression.New(constructor, new Expression[] { this.materializerExpression, Expression.Constant(null, typeof(DataServiceContext)), key.Arguments[0], key.Arguments[1], Expression.Constant(null, typeof(string)), Expression.Constant(null, typeof(Func<EntityChangedParams, bool>)), Expression.Constant(null, typeof(Func<EntityCollectionChangedParams, bool>)) });
                }
                else if ((key.Arguments.Count == 5) && this.annotations.TryGetValue(key.Arguments[0], out annotation))
                {
                    key = Expression.New(constructor, new Expression[] { this.materializerExpression, Expression.Constant(null, typeof(DataServiceContext)), key.Arguments[0], key.Arguments[1], key.Arguments[2], key.Arguments[3], key.Arguments[4] });
                }
                else if (((key.Arguments.Count == 6) && typeof(DataServiceContext).IsAssignableFrom(key.Arguments[0].Type)) && this.annotations.TryGetValue(key.Arguments[1], out annotation))
                {
                    key = Expression.New(constructor, new Expression[] { this.materializerExpression, key.Arguments[0], key.Arguments[1], key.Arguments[2], key.Arguments[3], key.Arguments[4], key.Arguments[5] });
                }
            }
            if (annotation != null)
            {
                this.annotations.Add(key, annotation);
            }
            return key;

        }

        private Expression RebindParameter(Expression expression, ExpressionAnnotation annotation)
        {
            Debug.Assert(expression != null, "expression != null");
            Debug.Assert(annotation != null, "annotation != null");
            Expression expression2 = this.CallValueForPathWithType(annotation.Segment.StartPath.RootEntry, annotation.Segment.StartPath.ExpectedRootType, annotation.Segment.StartPath, expression.Type);
            ProjectionPath startPath = new ProjectionPath(annotation.Segment.StartPath.Root, annotation.Segment.StartPath.ExpectedRootType, annotation.Segment.StartPath.RootEntry);
            ProjectionPathSegment item = new ProjectionPathSegment(startPath, null, null);
            startPath.Add(item);
            ExpressionAnnotation annotation2 = new ExpressionAnnotation();
            annotation2.Segment = item;
            this.annotations[expression] = annotation2;
            return expression2;
        }

        private Expression TypedEnumerableToList(Expression source, Type targetType)
        {
            Debug.Assert(source != null, "source != null");
            Debug.Assert(targetType != null, "targetType != null");
            Type type = source.Type.GetGenericArguments()[0];
            Type type2 = targetType.GetGenericArguments()[0];
            return CallMaterializerWithType("ListAsElementType", new Type[] { type, type2 }, new Expression[] { this.materializerExpression, source });
        }

        internal override Expression VisitBinary(BinaryExpression b)
        {
            Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(b);
            if (expressionBeforeNormalization == b)
            {
                return base.VisitBinary(b);
            }
            return this.Visit(expressionBeforeNormalization);
        }

        internal override Expression VisitConditional(ConditionalExpression conditional)
        {
            Debug.Assert(conditional != null, "conditional != null");
            Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(conditional);
            if (expressionBeforeNormalization != conditional)
            {
                return this.Visit(expressionBeforeNormalization);
            }
            ResourceBinder.PatternRules.MatchNullCheckResult nullCheck = ResourceBinder.PatternRules.MatchNullCheck(this.pathBuilder.LambdaParameterInScope, conditional);
            if (!(nullCheck.Match && ClientType.CheckElementTypeIsEntity(nullCheck.AssignExpression.Type)))
            {
                return base.VisitConditional(conditional);
            }
            return this.RebindConditionalNullCheck(conditional, nullCheck);
        }

        internal override Expression VisitLambda(LambdaExpression lambda)
        {
            Debug.Assert(lambda != null, "lambda != null");
            if (!(this.topLevelProjectionFound && ((lambda.Parameters.Count != 1) || !ClientType.CheckElementTypeIsEntity(lambda.Parameters[0].Type))))
            {
                this.topLevelProjectionFound = true;
                ParameterExpression expectedType = Expression.Parameter(typeof(Type), "type" + this.identifierId);
                ParameterExpression entry = Expression.Parameter(typeof(object), "entry" + this.identifierId);
                this.identifierId++;
                this.pathBuilder.EnterLambdaScope(lambda, entry, expectedType);
                ProjectionPath startPath = new ProjectionPath(lambda.Parameters[0], expectedType, entry);
                ProjectionPathSegment item = new ProjectionPathSegment(startPath, null, null);
                startPath.Add(item);
                ExpressionAnnotation annotation = new ExpressionAnnotation();
                annotation.Segment = item;
                this.annotations[lambda.Parameters[0]] = annotation;
                Expression expression4 = this.Visit(lambda.Body);
                if (expression4.Type.IsValueType)
                {
                    expression4 = Expression.Convert(expression4, typeof(object));
                }
                Expression expression = Expression.Lambda<Func<object, object, Type, object>>(expression4, new ParameterExpression[] { this.materializerExpression, entry, expectedType });
                this.pathBuilder.LeaveLambdaScope();
                return expression;
            }
            return base.VisitLambda(lambda);
        }

        internal override Expression VisitMemberAccess(MemberExpression m)
        {
            ExpressionAnnotation annotation;
            Debug.Assert(m != null, "m != null");
            Expression expression = m.Expression;
            if (ClientConvert.IsKnownNullableType(expression.Type))
            {
                return base.VisitMemberAccess(m);
            }
            Expression key = this.Visit(expression);
            if (this.annotations.TryGetValue(key, out annotation))
            {
                return this.RebindMemberAccess(m, annotation);
            }
            return Expression.MakeMemberAccess(key, m.Member);
        }

        internal override Expression VisitMemberInit(MemberInitExpression init)
        {
            this.pathBuilder.EnterMemberInit(init);
            Expression expression = null;
            if (this.pathBuilder.CurrentIsEntity && (init.Bindings.Count > 0))
            {
                expression = this.RebindEntityMemberInit(init);
            }
            else
            {
                expression = base.VisitMemberInit(init);
            }
            this.pathBuilder.LeaveMemberInit();
            return expression;
        }

        internal override Expression VisitMethodCall(MethodCallExpression m)
        {
            Expression expression2;
            Debug.Assert(m != null, "m != null");
            Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(m);
            if (expressionBeforeNormalization != m)
            {
                return this.Visit(expressionBeforeNormalization);
            }
            if (this.pathBuilder.CurrentIsEntity)
            {
                Debug.Assert(ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m) || ResourceBinder.PatternRules.MatchReferenceEquals(m), "ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m) || ResourceBinder.PatternRules.MatchReferenceEquals(m) -- otherwise ProjectionAnalyzer should have blocked this for entities");
                if (m.Method.Name == "Select")
                {
                    expression2 = this.RebindMethodCallForMemberSelect(m);
                }
                else if (m.Method.Name == "ToList")
                {
                    expression2 = this.RebindMethodCallForMemberToList(m);
                }
                else
                {
                    Debug.Assert(m.Method.Name == "ReferenceEquals", "We don't know how to handle this method, ProjectionAnalyzer updated?");
                    expression2 = base.VisitMethodCall(m);
                }
                return expression2;
            }
            if (ProjectionAnalyzer.IsMethodCallAllowedEntitySequence(m))
            {
                expression2 = this.RebindMethodCallForNewSequence(m);
            }
            else
            {
                expression2 = base.VisitMethodCall(m);
            }
            return expression2;
        }

        internal override NewExpression VisitNew(NewExpression nex)
        {
            Debug.Assert(nex != null, "nex != null");
            if (ResourceBinder.PatternRules.MatchNewDataServiceCollectionOfT(nex))
            {
                return this.RebindNewExpressionForDataServiceCollectionOfT(nex);
            }
            return base.VisitNew(nex);
        }

        internal override Expression VisitParameter(ParameterExpression p)
        {
            ExpressionAnnotation annotation;
            Debug.Assert(p != null, "p != null");
            if (this.annotations.TryGetValue(p, out annotation))
            {
                return this.RebindParameter(p, annotation);
            }
            return base.VisitParameter(p);
        }

        internal override Expression VisitUnary(UnaryExpression u)
        {
            Expression expressionBeforeNormalization = this.GetExpressionBeforeNormalization(u);
            if (expressionBeforeNormalization == u)
            {
                ExpressionAnnotation annotation;
                Expression expression2 = base.VisitUnary(u);
                UnaryExpression expression3 = expression2 as UnaryExpression;
                if ((expression3 != null) && this.annotations.TryGetValue(expression3.Operand, out annotation))
                {
                    this.annotations[expression2] = annotation;
                }
                return expression2;
            }
            return this.Visit(expressionBeforeNormalization);
        }

        internal class ExpressionAnnotation
        {
            public ProjectionPathSegment Segment { get; set; }
            //[CompilerGenerated]
            //private ProjectionPathSegment <Segment>k__BackingField;

            //internal ProjectionPathSegment Segment
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Segment>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<Segment>k__BackingField = value;
            //    }
            //}
        }
    }
}

