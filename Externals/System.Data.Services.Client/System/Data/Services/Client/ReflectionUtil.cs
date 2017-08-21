namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class ReflectionUtil
    {
        private static readonly Dictionary<SequenceMethod, MethodInfo> s_inverseMap;
        private static readonly Dictionary<MethodInfo, SequenceMethod> s_methodMap;

        static ReflectionUtil()
        {
            Dictionary<string, SequenceMethod> dictionary = new Dictionary<string, SequenceMethod>(EqualityComparer<string>.Default);
            dictionary.Add("AsQueryable(IEnumerable`1<T>)->IQueryable`1<T>", SequenceMethod.AsQueryableGeneric);
            dictionary.Add("Where(IQueryable`1<T>, Expression`1<TDelegate>)->IQueryable`1<T>", SequenceMethod.Where);
            dictionary.Add("OfType(IQueryable)->IQueryable`1<T>", SequenceMethod.OfType);
            dictionary.Add("Cast(IQueryable)->IQueryable`1<T>", SequenceMethod.Cast);
            dictionary.Add("Select(IQueryable`1<T>, Expression`1<TDelegate>)->IQueryable`1<T>", SequenceMethod.Select);
            dictionary.Add("SelectMany(IQueryable`1<T>, Expression`1<TDelegate>)->IQueryable`1<T>", SequenceMethod.SelectMany);
            dictionary.Add("SelectMany(IQueryable`1<T>, Expression`1<TDelegate>, Expression`1<TDelegate>)->IQueryable`1<T>", SequenceMethod.SelectManyOrdinalResultSelector);
            dictionary.Add("OrderBy(IQueryable`1<T>, Expression`1<TDelegate>)->IOrderedQueryable`1<T>", SequenceMethod.OrderBy);
            dictionary.Add("OrderBy(IQueryable`1<T>, Expression`1<TDelegate>, IComparer`1<T>)->IOrderedQueryable`1<T>", SequenceMethod.OrderByComparer);
            dictionary.Add("OrderByDescending(IQueryable`1<T>, Expression`1<TDelegate>)->IOrderedQueryable`1<T>", SequenceMethod.OrderByDescending);
            dictionary.Add("OrderByDescending(IQueryable`1<T>, Expression`1<TDelegate>, IComparer`1<T>)->IOrderedQueryable`1<T>", SequenceMethod.OrderByDescendingComparer);
            dictionary.Add("ThenBy(IOrderedQueryable`1<T>, Expression`1<TDelegate>)->IOrderedQueryable`1<T>", SequenceMethod.ThenBy);
            dictionary.Add("ThenBy(IOrderedQueryable`1<T>, Expression`1<TDelegate>, IComparer`1<T>)->IOrderedQueryable`1<T>", SequenceMethod.ThenByComparer);
            dictionary.Add("ThenByDescending(IOrderedQueryable`1<T>, Expression`1<TDelegate>)->IOrderedQueryable`1<T>", SequenceMethod.ThenByDescending);
            dictionary.Add("ThenByDescending(IOrderedQueryable`1<T>, Expression`1<TDelegate>, IComparer`1<T>)->IOrderedQueryable`1<T>", SequenceMethod.ThenByDescendingComparer);
            dictionary.Add("Take(IQueryable`1<T>, Int32)->IQueryable`1<T>", SequenceMethod.Take);
            dictionary.Add("Skip(IQueryable`1<T>, Int32)->IQueryable`1<T>", SequenceMethod.Skip);
            dictionary.Add("First(IQueryable`1<T>)->T0", SequenceMethod.First);
            dictionary.Add("FirstOrDefault(IQueryable`1<T>)->T0", SequenceMethod.FirstOrDefault);
            dictionary.Add("Single(IQueryable`1<T>)->T0", SequenceMethod.Single);
            dictionary.Add("SingleOrDefault(IQueryable`1<T>)->T0", SequenceMethod.SingleOrDefault);
            dictionary.Add("Count(IQueryable`1<T>)->Int32", SequenceMethod.Count);
            dictionary.Add("LongCount(IQueryable`1<T>)->Int64", SequenceMethod.LongCount);
            dictionary.Add("AsQueryable(IEnumerable)->IQueryable", SequenceMethod.AsQueryable);
            s_methodMap = new Dictionary<MethodInfo, SequenceMethod>(EqualityComparer<MethodInfo>.Default);
            s_inverseMap = new Dictionary<SequenceMethod, MethodInfo>(EqualityComparer<SequenceMethod>.Default);
            foreach (MethodInfo info in GetAllLinqOperators())
            {
                SequenceMethod method;
                string canonicalMethodDescription = GetCanonicalMethodDescription(info);
                if (dictionary.TryGetValue(canonicalMethodDescription, out method))
                {
                    s_methodMap.Add(info, method);
                    s_inverseMap[method] = info;
                }
            }
        }

        private static void AppendCanonicalTypeDescription(Type type, Dictionary<Type, int> genericArgumentOrdinals, StringBuilder description)
        {
            int num;
            if ((genericArgumentOrdinals != null) && genericArgumentOrdinals.TryGetValue(type, out num))
            {
                description.Append("T").Append(num.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                description.Append(type.Name);
                if (type.IsGenericType)
                {
                    description.Append("<");
                    bool flag = true;
                    foreach (Type type2 in type.GetGenericArguments())
                    {
                        if (flag)
                        {
                            flag = false;
                        }
                        else
                        {
                            description.Append(", ");
                        }
                        AppendCanonicalTypeDescription(type2, genericArgumentOrdinals, description);
                    }
                    description.Append(">");
                }
            }
        }

        internal static IEnumerable<MethodInfo> GetAllLinqOperators()
        {
            return typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).Concat<MethodInfo>(typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static));
        }

        internal static string GetCanonicalMethodDescription(MethodInfo method)
        {
            Dictionary<Type, int> genericArgumentOrdinals = null;
            if (method.IsGenericMethodDefinition)
            {
                genericArgumentOrdinals = method.GetGenericArguments().Where<Type>(delegate(Type t)
                {
                    return t.IsGenericParameter;
                }).Select<Type, KeyValuePair<Type, int>>(delegate(Type t, int i)
                {
                    return new KeyValuePair<Type, int>(t, i);
                }).ToDictionary<KeyValuePair<Type, int>, Type, int>(delegate(KeyValuePair<Type, int> r)
                {
                    return r.Key;
                }, delegate(KeyValuePair<Type, int> r)
                {
                    return r.Value;
                });
            }
            StringBuilder description = new StringBuilder();
            description.Append(method.Name).Append("(");
            bool flag = true;
            foreach (ParameterInfo info in method.GetParameters())
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    description.Append(", ");
                }
                AppendCanonicalTypeDescription(info.ParameterType, genericArgumentOrdinals, description);
            }
            description.Append(")");
            if (null != method.ReturnType)
            {
                description.Append("->");
                AppendCanonicalTypeDescription(method.ReturnType, genericArgumentOrdinals, description);
            }
            return description.ToString();

        }

        internal static bool IsSequenceMethod(MethodInfo method, SequenceMethod sequenceMethod)
        {
            SequenceMethod method2;
            return (TryIdentifySequenceMethod(method, out method2) && (method2 == sequenceMethod));
        }

        internal static bool TryIdentifySequenceMethod(MethodInfo method, out SequenceMethod sequenceMethod)
        {
            method = method.IsGenericMethod ? method.GetGenericMethodDefinition() : method;
            return s_methodMap.TryGetValue(method, out sequenceMethod);
        }
    }
}

