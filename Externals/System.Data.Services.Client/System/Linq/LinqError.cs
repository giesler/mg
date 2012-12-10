namespace System.Linq
{
    using System;

    internal static class LinqError
    {
        internal static Exception ArgumentArrayHasTooManyElements(object p0)
        {
            return new ArgumentException(LinqStrings.ArgumentArrayHasTooManyElements(p0));
        }

        internal static Exception ArgumentNotIEnumerableGeneric(object p0)
        {
            return new ArgumentException(LinqStrings.ArgumentNotIEnumerableGeneric(p0));
        }

        internal static Exception ArgumentNotLambda(object p0)
        {
            return new ArgumentException(LinqStrings.ArgumentNotLambda(p0));
        }

        internal static Exception ArgumentNotSequence(object p0)
        {
            return new ArgumentException(LinqStrings.ArgumentNotSequence(p0));
        }

        internal static Exception ArgumentNotValid(object p0)
        {
            return new ArgumentException(LinqStrings.ArgumentNotValid(p0));
        }

        internal static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        internal static Exception ArgumentOutOfRange(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName);
        }

        internal static Exception IncompatibleElementTypes()
        {
            return new ArgumentException(LinqStrings.IncompatibleElementTypes);
        }

        internal static Exception MoreThanOneElement()
        {
            return new InvalidOperationException(LinqStrings.MoreThanOneElement);
        }

        internal static Exception MoreThanOneMatch()
        {
            return new InvalidOperationException(LinqStrings.MoreThanOneMatch);
        }

        internal static Exception NoArgumentMatchingMethodsInQueryable(object p0)
        {
            return new InvalidOperationException(LinqStrings.NoArgumentMatchingMethodsInQueryable(p0));
        }

        internal static Exception NoElements()
        {
            return new InvalidOperationException(LinqStrings.NoElements);
        }

        internal static Exception NoMatch()
        {
            return new InvalidOperationException(LinqStrings.NoMatch);
        }

        internal static Exception NoMethodOnType(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.NoMethodOnType(p0, p1));
        }

        internal static Exception NoMethodOnTypeMatchingArguments(object p0, object p1)
        {
            return new InvalidOperationException(LinqStrings.NoMethodOnTypeMatchingArguments(p0, p1));
        }

        internal static Exception NoNameMatchingMethodsInQueryable(object p0)
        {
            return new InvalidOperationException(LinqStrings.NoNameMatchingMethodsInQueryable(p0));
        }

        internal static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        internal static Exception NotSupported()
        {
            return new NotSupportedException();
        }
    }
}

