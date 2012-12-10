namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    internal static class CommonUtil
    {
        private static readonly Type[] unsupportedTypes = new Type[0];

        internal static bool IsUnsupportedType(Type type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }
            if (unsupportedTypes.Any<Type>(delegate(Type t)
            {
                return t.IsAssignableFrom(type);
            }))
            {
                return true;
            }
            Debug.Assert(!type.FullName.StartsWith("System.Tuple", StringComparison.Ordinal), "System.Tuple is not blocked by unsupported type check");
            return false;

        }
    }
}

