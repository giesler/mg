namespace System.Linq.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal static class Error
    {
        internal static Exception AccessorsCannotHaveByRefArgs()
        {
            return new ArgumentException(LinqStrings.AccessorsCannotHaveByRefArgs);
        }

        internal static Exception AccessorsCannotHaveVarArgs()
        {
            return new ArgumentException(LinqStrings.AccessorsCannotHaveVarArgs);
        }

        internal static Exception AmbiguousJump(object p0)
        {
            return new InvalidOperationException(LinqStrings.AmbiguousJump(p0));
        }

        internal static Exception AmbiguousMatchInExpandoObject(object p0)
        {
            return new AmbiguousMatchException(LinqStrings.AmbiguousMatchInExpandoObject(p0));
        }

        internal static Exception ArgCntMustBeGreaterThanNameCnt()
        {
            return new ArgumentException(LinqStrings.ArgCntMustBeGreaterThanNameCnt);
        }

        internal static Exception ArgumentCannotBeOfTypeVoid()
        {
            return new ArgumentException(LinqStrings.ArgumentCannotBeOfTypeVoid);
        }

        internal static Exception ArgumentMemberNotDeclOnType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ArgumentMemberNotDeclOnType(p0, p1));
        }

        internal static Exception ArgumentMustBeArray()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeArray);
        }

        internal static Exception ArgumentMustBeArrayIndexType()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeArrayIndexType);
        }

        internal static Exception ArgumentMustBeBoolean()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeBoolean);
        }

        internal static Exception ArgumentMustBeFieldInfoOrPropertInfo()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeFieldInfoOrPropertInfo);
        }

        internal static Exception ArgumentMustBeFieldInfoOrPropertInfoOrMethod()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeFieldInfoOrPropertInfoOrMethod);
        }

        internal static Exception ArgumentMustBeInstanceMember()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeInstanceMember);
        }

        internal static Exception ArgumentMustBeInteger()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeInteger);
        }

        internal static Exception ArgumentMustBeSingleDimensionalArrayType()
        {
            return new ArgumentException(LinqStrings.ArgumentMustBeSingleDimensionalArrayType);
        }

        internal static Exception ArgumentMustNotHaveValueType()
        {
            return new ArgumentException(LinqStrings.ArgumentMustNotHaveValueType);
        }

        internal static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        internal static Exception ArgumentOutOfRange(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName);
        }

        internal static Exception ArgumentTypeCannotBeVoid()
        {
            return new ArgumentException(LinqStrings.ArgumentTypeCannotBeVoid);
        }

        internal static Exception ArgumentTypeDoesNotMatchMember(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ArgumentTypeDoesNotMatchMember(p0, p1));
        }

        internal static Exception ArgumentTypesMustMatch()
        {
            return new ArgumentException(LinqStrings.ArgumentTypesMustMatch);
        }

        internal static Exception ArrayTypeMustBeArray()
        {
            return new ArgumentException(LinqStrings.ArrayTypeMustBeArray);
        }

        internal static Exception BinaryOperatorNotDefined(object p0, object p1, object p2)
        {
            return new InvalidOperationException(LinqStrings.BinaryOperatorNotDefined(p0, p1, p2));
        }

        internal static Exception BinderNotCompatibleWithCallSite(object p0, object p1, object p2)
        {
            return new InvalidOperationException(LinqStrings.BinderNotCompatibleWithCallSite(p0, p1, p2));
        }

        internal static Exception BindingCannotBeNull()
        {
            return new InvalidOperationException(LinqStrings.BindingCannotBeNull);
        }

        internal static Exception BodyOfCatchMustHaveSameTypeAsBodyOfTry()
        {
            return new ArgumentException(LinqStrings.BodyOfCatchMustHaveSameTypeAsBodyOfTry);
        }

        internal static Exception BothAccessorsMustBeStatic()
        {
            return new ArgumentException(LinqStrings.BothAccessorsMustBeStatic);
        }

        internal static Exception BoundsCannotBeLessThanOne()
        {
            return new ArgumentException(LinqStrings.BoundsCannotBeLessThanOne);
        }

        internal static Exception CannotAutoInitializeValueTypeElementThroughProperty(object p0)
        {
            return new InvalidOperationException(LinqStrings.CannotAutoInitializeValueTypeElementThroughProperty(p0));
        }

        internal static Exception CannotAutoInitializeValueTypeMemberThroughProperty(object p0)
        {
            return new InvalidOperationException(LinqStrings.CannotAutoInitializeValueTypeMemberThroughProperty(p0));
        }

        internal static Exception CannotCloseOverByRef(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.CannotCloseOverByRef(p0, p1));
        }

        internal static Exception CannotCompileConstant(object p0)
        {
            return new InvalidOperationException(LinqStrings.CannotCompileConstant(p0));
        }

        internal static Exception CannotCompileDynamic()
        {
            return new NotSupportedException(LinqStrings.CannotCompileDynamic);
        }

        internal static Exception CoalesceUsedOnNonNullType()
        {
            return new InvalidOperationException(LinqStrings.CoalesceUsedOnNonNullType);
        }

        internal static Exception CoercionOperatorNotDefined(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.CoercionOperatorNotDefined(p0, p1));
        }

        internal static Exception CollectionModifiedWhileEnumerating()
        {
            return new InvalidOperationException(LinqStrings.CollectionModifiedWhileEnumerating);
        }

        internal static Exception CollectionReadOnly()
        {
            return new NotSupportedException(LinqStrings.CollectionReadOnly);
        }

        internal static Exception ControlCannotEnterExpression()
        {
            return new InvalidOperationException(LinqStrings.ControlCannotEnterExpression);
        }

        internal static Exception ControlCannotEnterTry()
        {
            return new InvalidOperationException(LinqStrings.ControlCannotEnterTry);
        }

        internal static Exception ControlCannotLeaveFilterTest()
        {
            return new InvalidOperationException(LinqStrings.ControlCannotLeaveFilterTest);
        }

        internal static Exception ControlCannotLeaveFinally()
        {
            return new InvalidOperationException(LinqStrings.ControlCannotLeaveFinally);
        }

        internal static Exception ConversionIsNotSupportedForArithmeticTypes()
        {
            return new InvalidOperationException(LinqStrings.ConversionIsNotSupportedForArithmeticTypes);
        }

        internal static Exception CountCannotBeNegative()
        {
            return new ArgumentException(LinqStrings.CountCannotBeNegative);
        }

        internal static Exception DefaultBodyMustBeSupplied()
        {
            return new ArgumentException(LinqStrings.DefaultBodyMustBeSupplied);
        }

        internal static Exception DuplicateVariable(object p0)
        {
            return new ArgumentException(LinqStrings.DuplicateVariable(p0));
        }

        internal static Exception DynamicBinderResultNotAssignable(object p0, object p1, object p2)
        {
            return new InvalidCastException(LinqStrings.DynamicBinderResultNotAssignable(p0, p1, p2));
        }

        internal static Exception DynamicBindingNeedsRestrictions(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.DynamicBindingNeedsRestrictions(p0, p1));
        }

        internal static Exception DynamicObjectResultNotAssignable(object p0, object p1, object p2, object p3)
        {
            return new InvalidCastException(LinqStrings.DynamicObjectResultNotAssignable(p0, p1, p2, p3));
        }

        internal static Exception ElementInitializerMethodNoRefOutParam(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ElementInitializerMethodNoRefOutParam(p0, p1));
        }

        internal static Exception ElementInitializerMethodNotAdd()
        {
            return new ArgumentException(LinqStrings.ElementInitializerMethodNotAdd);
        }

        internal static Exception ElementInitializerMethodStatic()
        {
            return new ArgumentException(LinqStrings.ElementInitializerMethodStatic);
        }

        internal static Exception ElementInitializerMethodWithZeroArgs()
        {
            return new ArgumentException(LinqStrings.ElementInitializerMethodWithZeroArgs);
        }

        internal static Exception EnumerationIsDone()
        {
            return new InvalidOperationException(LinqStrings.EnumerationIsDone);
        }

        internal static Exception EqualityMustReturnBoolean(object p0)
        {
            return new ArgumentException(LinqStrings.EqualityMustReturnBoolean(p0));
        }

        internal static Exception ExpressionTypeCannotInitializeArrayType(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.ExpressionTypeCannotInitializeArrayType(p0, p1));
        }

        internal static Exception ExpressionTypeDoesNotMatchAssignment(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ExpressionTypeDoesNotMatchAssignment(p0, p1));
        }

        internal static Exception ExpressionTypeDoesNotMatchConstructorParameter(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ExpressionTypeDoesNotMatchConstructorParameter(p0, p1));
        }

        internal static Exception ExpressionTypeDoesNotMatchLabel(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ExpressionTypeDoesNotMatchLabel(p0, p1));
        }

        internal static Exception ExpressionTypeDoesNotMatchMethodParameter(object p0, object p1, object p2)
        {
            return new ArgumentException(LinqStrings.ExpressionTypeDoesNotMatchMethodParameter(p0, p1, p2));
        }

        internal static Exception ExpressionTypeDoesNotMatchParameter(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ExpressionTypeDoesNotMatchParameter(p0, p1));
        }

        internal static Exception ExpressionTypeDoesNotMatchReturn(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ExpressionTypeDoesNotMatchReturn(p0, p1));
        }

        internal static Exception ExpressionTypeNotInvocable(object p0)
        {
            return new ArgumentException(LinqStrings.ExpressionTypeNotInvocable(p0));
        }

        internal static Exception ExtensionNodeMustOverrideProperty(object p0)
        {
            return new InvalidOperationException(LinqStrings.ExtensionNodeMustOverrideProperty(p0));
        }

        internal static Exception ExtensionNotReduced()
        {
            return new InvalidOperationException(LinqStrings.ExtensionNotReduced);
        }

        internal static Exception FaultCannotHaveCatchOrFinally()
        {
            return new ArgumentException(LinqStrings.FaultCannotHaveCatchOrFinally);
        }

        internal static Exception FieldInfoNotDefinedForType(object p0, object p1, object p2)
        {
            return new ArgumentException(LinqStrings.FieldInfoNotDefinedForType(p0, p1, p2));
        }

        internal static Exception FieldNotDefinedForType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.FieldNotDefinedForType(p0, p1));
        }

        internal static Exception FirstArgumentMustBeCallSite()
        {
            return new ArgumentException(LinqStrings.FirstArgumentMustBeCallSite);
        }

        internal static Exception GenericMethodWithArgsDoesNotExistOnType(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.GenericMethodWithArgsDoesNotExistOnType(p0, p1));
        }

        internal static Exception HomogenousAppDomainRequired()
        {
            return new InvalidOperationException(LinqStrings.HomogenousAppDomainRequired);
        }

        internal static Exception IllegalNewGenericParams(object p0)
        {
            return new ArgumentException(LinqStrings.IllegalNewGenericParams(p0));
        }

        internal static Exception IncorrectNumberOfArgumentsForMembers()
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfArgumentsForMembers);
        }

        internal static Exception IncorrectNumberOfConstructorArguments()
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfConstructorArguments);
        }

        internal static Exception IncorrectNumberOfIndexes()
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfIndexes);
        }

        internal static Exception IncorrectNumberOfLambdaArguments()
        {
            return new InvalidOperationException(LinqStrings.IncorrectNumberOfLambdaArguments);
        }

        internal static Exception IncorrectNumberOfLambdaDeclarationParameters()
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfLambdaDeclarationParameters);
        }

        internal static Exception IncorrectNumberOfMembersForGivenConstructor()
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfMembersForGivenConstructor);
        }

        internal static Exception IncorrectNumberOfMethodCallArguments(object p0)
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfMethodCallArguments(p0));
        }

        internal static Exception IncorrectNumberOfTypeArgsForAction()
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfTypeArgsForAction);
        }

        internal static Exception IncorrectNumberOfTypeArgsForFunc()
        {
            return new ArgumentException(LinqStrings.IncorrectNumberOfTypeArgsForFunc);
        }

        internal static Exception IncorrectTypeForTypeAs(object p0)
        {
            return new ArgumentException(LinqStrings.IncorrectTypeForTypeAs(p0));
        }

        internal static Exception IndexesOfSetGetMustMatch()
        {
            return new ArgumentException(LinqStrings.IndexesOfSetGetMustMatch);
        }

        internal static Exception InstanceAndMethodTypeMismatch(object p0, object p1, object p2)
        {
            return new ArgumentException(LinqStrings.InstanceAndMethodTypeMismatch(p0, p1, p2));
        }

        internal static Exception InstanceFieldNotDefinedForType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.InstanceFieldNotDefinedForType(p0, p1));
        }

        internal static Exception InstancePropertyNotDefinedForType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.InstancePropertyNotDefinedForType(p0, p1));
        }

        internal static Exception InstancePropertyWithoutParameterNotDefinedForType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.InstancePropertyWithoutParameterNotDefinedForType(p0, p1));
        }

        internal static Exception InstancePropertyWithSpecifiedParametersNotDefinedForType(object p0, object p1, object p2)
        {
            return new ArgumentException(LinqStrings.InstancePropertyWithSpecifiedParametersNotDefinedForType(p0, p1, p2));
        }

        internal static Exception InvalidAsmNameOrExtension()
        {
            return new ArgumentException(LinqStrings.InvalidAsmNameOrExtension);
        }

        internal static Exception InvalidCast(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.InvalidCast(p0, p1));
        }

        internal static Exception InvalidLvalue(object p0)
        {
            return new InvalidOperationException(LinqStrings.InvalidLvalue(p0));
        }

        internal static Exception InvalidMemberType(object p0)
        {
            return new InvalidOperationException(LinqStrings.InvalidMemberType(p0));
        }

        internal static Exception InvalidMetaObjectCreated(object p0)
        {
            return new InvalidOperationException(LinqStrings.InvalidMetaObjectCreated(p0));
        }

        internal static Exception InvalidOperation(object p0)
        {
            return new ArgumentException(LinqStrings.InvalidOperation(p0));
        }

        internal static Exception InvalidOutputDir()
        {
            return new ArgumentException(LinqStrings.InvalidOutputDir);
        }

        internal static Exception InvalidUnboxType()
        {
            return new ArgumentException(LinqStrings.InvalidUnboxType);
        }

        internal static Exception KeyDoesNotExistInExpando(object p0)
        {
            return new KeyNotFoundException(LinqStrings.KeyDoesNotExistInExpando(p0));
        }

        internal static Exception LabelMustBeVoidOrHaveExpression()
        {
            return new ArgumentException(LinqStrings.LabelMustBeVoidOrHaveExpression);
        }

        internal static Exception LabelTargetAlreadyDefined(object p0)
        {
            return new InvalidOperationException(LinqStrings.LabelTargetAlreadyDefined(p0));
        }

        internal static Exception LabelTargetUndefined(object p0)
        {
            return new InvalidOperationException(LinqStrings.LabelTargetUndefined(p0));
        }

        internal static Exception LabelTypeMustBeVoid()
        {
            return new ArgumentException(LinqStrings.LabelTypeMustBeVoid);
        }

        internal static Exception LambdaTypeMustBeDerivedFromSystemDelegate()
        {
            return new ArgumentException(LinqStrings.LambdaTypeMustBeDerivedFromSystemDelegate);
        }

        internal static Exception ListInitializerWithZeroMembers()
        {
            return new ArgumentException(LinqStrings.ListInitializerWithZeroMembers);
        }

        internal static Exception LogicalOperatorMustHaveBooleanOperators(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.LogicalOperatorMustHaveBooleanOperators(p0, p1));
        }

        internal static Exception MemberNotFieldOrProperty(object p0)
        {
            return new ArgumentException(LinqStrings.MemberNotFieldOrProperty(p0));
        }

        internal static Exception MethodBuilderDoesNotHaveTypeBuilder()
        {
            return new ArgumentException(LinqStrings.MethodBuilderDoesNotHaveTypeBuilder);
        }

        internal static Exception MethodContainsGenericParameters(object p0)
        {
            return new ArgumentException(LinqStrings.MethodContainsGenericParameters(p0));
        }

        internal static Exception MethodDoesNotExistOnType(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.MethodDoesNotExistOnType(p0, p1));
        }

        internal static Exception MethodIsGeneric(object p0)
        {
            return new ArgumentException(LinqStrings.MethodIsGeneric(p0));
        }

        internal static Exception MethodNotPropertyAccessor(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.MethodNotPropertyAccessor(p0, p1));
        }

        internal static Exception MethodWithArgsDoesNotExistOnType(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.MethodWithArgsDoesNotExistOnType(p0, p1));
        }

        internal static Exception MethodWithMoreThanOneMatch(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.MethodWithMoreThanOneMatch(p0, p1));
        }

        internal static Exception MustBeReducible()
        {
            return new ArgumentException(LinqStrings.MustBeReducible);
        }

        internal static Exception MustReduceToDifferent()
        {
            return new ArgumentException(LinqStrings.MustReduceToDifferent);
        }

        internal static Exception MustRewriteChildToSameType(object p0, object p1, object p2)
        {
            return new InvalidOperationException(LinqStrings.MustRewriteChildToSameType(p0, p1, p2));
        }

        internal static Exception MustRewriteToSameNode(object p0, object p1, object p2)
        {
            return new InvalidOperationException(LinqStrings.MustRewriteToSameNode(p0, p1, p2));
        }

        internal static Exception MustRewriteWithoutMethod(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.MustRewriteWithoutMethod(p0, p1));
        }

        internal static Exception NonLocalJumpWithValue(object p0)
        {
            return new InvalidOperationException(LinqStrings.NonLocalJumpWithValue(p0));
        }

        internal static Exception NoOrInvalidRuleProduced()
        {
            return new InvalidOperationException(LinqStrings.NoOrInvalidRuleProduced);
        }

        internal static Exception NotAMemberOfType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.NotAMemberOfType(p0, p1));
        }

        internal static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        internal static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        internal static Exception OnlyStaticMethodsHaveNullInstance()
        {
            return new ArgumentException(LinqStrings.OnlyStaticMethodsHaveNullInstance);
        }

        internal static Exception OperandTypesDoNotMatchParameters(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.OperandTypesDoNotMatchParameters(p0, p1));
        }

        internal static Exception OperatorNotImplementedForType(object p0, object p1)
        {
            return new NotImplementedException(LinqStrings.OperatorNotImplementedForType(p0, p1));
        }

        internal static Exception OutOfRange(object p0, object p1)
        {
            return new ArgumentOutOfRangeException(LinqStrings.OutOfRange(p0, p1));
        }

        internal static Exception OverloadOperatorTypeDoesNotMatchConversionType(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.OverloadOperatorTypeDoesNotMatchConversionType(p0, p1));
        }

        internal static Exception ParameterExpressionNotValidAsDelegate(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.ParameterExpressionNotValidAsDelegate(p0, p1));
        }

        internal static Exception PdbGeneratorNeedsExpressionCompiler()
        {
            return new NotSupportedException(LinqStrings.PdbGeneratorNeedsExpressionCompiler);
        }

        internal static Exception PropertyCannotHaveRefType()
        {
            return new ArgumentException(LinqStrings.PropertyCannotHaveRefType);
        }

        internal static Exception PropertyDoesNotHaveAccessor(object p0)
        {
            return new ArgumentException(LinqStrings.PropertyDoesNotHaveAccessor(p0));
        }

        internal static Exception PropertyDoesNotHaveGetter(object p0)
        {
            return new ArgumentException(LinqStrings.PropertyDoesNotHaveGetter(p0));
        }

        internal static Exception PropertyDoesNotHaveSetter(object p0)
        {
            return new ArgumentException(LinqStrings.PropertyDoesNotHaveSetter(p0));
        }

        internal static Exception PropertyNotDefinedForType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.PropertyNotDefinedForType(p0, p1));
        }

        internal static Exception PropertyTyepMustMatchSetter()
        {
            return new ArgumentException(LinqStrings.PropertyTyepMustMatchSetter);
        }

        internal static Exception PropertyTypeCannotBeVoid()
        {
            return new ArgumentException(LinqStrings.PropertyTypeCannotBeVoid);
        }

        internal static Exception PropertyWithMoreThanOneMatch(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.PropertyWithMoreThanOneMatch(p0, p1));
        }

        internal static Exception QueueEmpty()
        {
            return new InvalidOperationException(LinqStrings.QueueEmpty);
        }

        internal static Exception QuotedExpressionMustBeLambda()
        {
            return new ArgumentException(LinqStrings.QuotedExpressionMustBeLambda);
        }

        internal static Exception ReducedNotCompatible()
        {
            return new ArgumentException(LinqStrings.ReducedNotCompatible);
        }

        internal static Exception ReducibleMustOverrideReduce()
        {
            return new ArgumentException(LinqStrings.ReducibleMustOverrideReduce);
        }

        internal static Exception ReferenceEqualityNotDefined(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.ReferenceEqualityNotDefined(p0, p1));
        }

        internal static Exception RethrowRequiresCatch()
        {
            return new InvalidOperationException(LinqStrings.RethrowRequiresCatch);
        }

        internal static Exception SameKeyExistsInExpando(object p0)
        {
            return new ArgumentException(LinqStrings.SameKeyExistsInExpando(p0));
        }

        internal static Exception SetterHasNoParams()
        {
            return new ArgumentException(LinqStrings.SetterHasNoParams);
        }

        internal static Exception SetterMustBeVoid()
        {
            return new ArgumentException(LinqStrings.SetterMustBeVoid);
        }

        internal static Exception StartEndMustBeOrdered()
        {
            return new ArgumentException(LinqStrings.StartEndMustBeOrdered);
        }

        internal static Exception SwitchValueTypeDoesNotMatchComparisonMethodParameter(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.SwitchValueTypeDoesNotMatchComparisonMethodParameter(p0, p1));
        }

        internal static Exception TestValueTypeDoesNotMatchComparisonMethodParameter(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.TestValueTypeDoesNotMatchComparisonMethodParameter(p0, p1));
        }

        internal static Exception TryMustHaveCatchFinallyOrFault()
        {
            return new ArgumentException(LinqStrings.TryMustHaveCatchFinallyOrFault);
        }

        internal static Exception TryNotAllowedInFilter()
        {
            return new InvalidOperationException(LinqStrings.TryNotAllowedInFilter);
        }

        internal static Exception TryNotSupportedForMethodsWithRefArgs(object p0)
        {
            return new NotSupportedException(LinqStrings.TryNotSupportedForMethodsWithRefArgs(p0));
        }

        internal static Exception TryNotSupportedForValueTypeInstances(object p0)
        {
            return new NotSupportedException(LinqStrings.TryNotSupportedForValueTypeInstances(p0));
        }

        internal static Exception TypeContainsGenericParameters(object p0)
        {
            return new ArgumentException(LinqStrings.TypeContainsGenericParameters(p0));
        }

        internal static Exception TypeDoesNotHaveConstructorForTheSignature()
        {
            return new ArgumentException(LinqStrings.TypeDoesNotHaveConstructorForTheSignature);
        }

        internal static Exception TypeIsGeneric(object p0)
        {
            return new ArgumentException(LinqStrings.TypeIsGeneric(p0));
        }

        internal static Exception TypeMissingDefaultConstructor(object p0)
        {
            return new ArgumentException(LinqStrings.TypeMissingDefaultConstructor(p0));
        }

        internal static Exception TypeMustBeDerivedFromSystemDelegate()
        {
            return new ArgumentException(LinqStrings.TypeMustBeDerivedFromSystemDelegate);
        }

        internal static Exception TypeMustNotBeByRef()
        {
            return new ArgumentException(LinqStrings.TypeMustNotBeByRef);
        }

        internal static Exception TypeNotIEnumerable(object p0)
        {
            return new ArgumentException(LinqStrings.TypeNotIEnumerable(p0));
        }

        internal static Exception TypeParameterIsNotDelegate(object p0)
        {
            return new InvalidOperationException(LinqStrings.TypeParameterIsNotDelegate(p0));
        }

        internal static Exception UnaryOperatorNotDefined(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.UnaryOperatorNotDefined(p0, p1));
        }

        internal static Exception UndefinedVariable(object p0, object p1, object p2)
        {
            return new InvalidOperationException(LinqStrings.UndefinedVariable(p0, p1, p2));
        }

        internal static Exception UnexpectedCoalesceOperator()
        {
            return new InvalidOperationException(LinqStrings.UnexpectedCoalesceOperator);
        }

        internal static Exception UnexpectedVarArgsCall(object p0)
        {
            return new InvalidOperationException(LinqStrings.UnexpectedVarArgsCall(p0));
        }

        internal static Exception UnhandledBinary(object p0)
        {
            return new ArgumentException(LinqStrings.UnhandledBinary(p0));
        }

        internal static Exception UnhandledBinding()
        {
            return new ArgumentException(LinqStrings.UnhandledBinding);
        }

        internal static Exception UnhandledBindingType(object p0)
        {
            return new ArgumentException(LinqStrings.UnhandledBindingType(p0));
        }

        internal static Exception UnhandledConvert(object p0)
        {
            return new ArgumentException(LinqStrings.UnhandledConvert(p0));
        }

        internal static Exception UnhandledExpressionType(object p0)
        {
            return new ArgumentException(LinqStrings.UnhandledExpressionType(p0));
        }

        internal static Exception UnhandledUnary(object p0)
        {
            return new ArgumentException(LinqStrings.UnhandledUnary(p0));
        }

        internal static Exception UnknownBindingType()
        {
            return new ArgumentException(LinqStrings.UnknownBindingType);
        }

        internal static Exception UnknownLiftType(object p0)
        {
            return new InvalidOperationException(LinqStrings.UnknownLiftType(p0));
        }

        internal static Exception UserDefinedOperatorMustBeStatic(object p0)
        {
            return new ArgumentException(LinqStrings.UserDefinedOperatorMustBeStatic(p0));
        }

        internal static Exception UserDefinedOperatorMustNotBeVoid(object p0)
        {
            return new ArgumentException(LinqStrings.UserDefinedOperatorMustNotBeVoid(p0));
        }

        internal static Exception UserDefinedOpMustHaveConsistentTypes(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.UserDefinedOpMustHaveConsistentTypes(p0, p1));
        }

        internal static Exception UserDefinedOpMustHaveValidReturnType(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.UserDefinedOpMustHaveValidReturnType(p0, p1));
        }

        internal static Exception VariableMustNotBeByRef(object p0, object p1)
        {
            return new ArgumentException(LinqStrings.VariableMustNotBeByRef(p0, p1));
        }
    }
}

