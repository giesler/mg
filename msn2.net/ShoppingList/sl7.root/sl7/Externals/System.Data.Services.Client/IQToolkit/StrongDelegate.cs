namespace IQToolkit
{
    using System;
    using System.Reflection;

    public class StrongDelegate
    {
        Func<object[], object> fn;

        private StrongDelegate(Func<object[], object> fn)
        {
            this.fn = fn;
        }

        private static MethodInfo[] _meths;

        static StrongDelegate()
        {
            _meths = new MethodInfo[9];

            var meths = typeof(StrongDelegate).GetMethods();
            for (int i = 0, n = meths.Length; i < n; i++)
            {
                var gm = meths[i];
                if (gm.Name.StartsWith("M"))
                {
                    var tas = gm.GetGenericArguments();
                    _meths[tas.Length - 1] = gm;
                }
            }
        }

        /// <summary>
        /// Create a strongly typed delegate over a method with a weak signature
        /// </summary>
        /// <param name="delegateType">The strongly typed delegate's type</param>
        /// <param name="target"></param>
        /// <param name="method">Any method that takes a single array of objects and returns an object.</param>
        /// <returns></returns>
        public static Delegate CreateDelegate(Type delegateType, object target, MethodInfo method)
        {
            return CreateDelegate(delegateType, (Func<object[], object>)Delegate.CreateDelegate(typeof(Func<object[], object>), target, method));
        }

        /// <summary>
        /// Create a strongly typed delegate over a Func delegate with weak signature
        /// </summary>
        /// <param name="delegateType"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static Delegate CreateDelegate(Type delegateType, Func<object[], object> fn)
        {
            MethodInfo invoke = delegateType.GetMethod("Invoke");
            var parameters = invoke.GetParameters();
            Type[] typeArgs = new Type[1 + parameters.Length];
            for (int i = 0, n = parameters.Length; i < n; i++)
            {
                typeArgs[i] = parameters[i].ParameterType;
            }
            typeArgs[typeArgs.Length - 1] = invoke.ReturnType;
            if (typeArgs.Length <= _meths.Length)
            {
                var gm = _meths[typeArgs.Length - 1];
                var m = gm.MakeGenericMethod(typeArgs);
                return Delegate.CreateDelegate(delegateType, new StrongDelegate(fn), m);
            }
            throw new NotSupportedException("Delegate has too many arguments");
        }

        public R M<R>()
        {
            return (R)fn(null);
        }

        public R M<A1, R>(A1 a1)
        {
            return (R)fn(new object[] { a1 });
        }

        public R M<A1, A2, R>(A1 a1, A2 a2)
        {
            return (R)fn(new object[] { a1, a2 });
        }

        public R M<A1, A2, A3, R>(A1 a1, A2 a2, A3 a3)
        {
            return (R)fn(new object[] { a1, a2, a3 });
        }

        public R M<A1, A2, A3, A4, R>(A1 a1, A2 a2, A3 a3, A4 a4)
        {
            return (R)fn(new object[] { a1, a2, a3, a4 });
        }

        public R M<A1, A2, A3, A4, A5, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5)
        {
            return (R)fn(new object[] { a1, a2, a3, a4, a5 });
        }

        public R M<A1, A2, A3, A4, A5, A6, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6)
        {
            return (R)fn(new object[] { a1, a2, a3, a4, a5, a6 });
        }

        public R M<A1, A2, A3, A4, A5, A6, A7, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7)
        {
            return (R)fn(new object[] { a1, a2, a3, a4, a5, a6, a7 });
        }

        public R M<A1, A2, A3, A4, A5, A6, A7, A8, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8)
        {
            return (R)fn(new object[] { a1, a2, a3, a4, a5, a6, a7, a8 });
        }
    }

    //public class StrongDelegate
    //{
    //    private static MethodInfo[] _meths = new MethodInfo[9];
    //    private Func<object[], object> fn;

    //    static StrongDelegate()
    //    {
    //        MethodInfo[] methods = typeof(StrongDelegate).GetMethods();
    //        int index = 0;
    //        int length = methods.Length;
    //        while (index < length)
    //        {
    //            MethodInfo info = methods[index];
    //            if (info.Name.StartsWith("M"))
    //            {
    //                Type[] genericArguments = info.GetGenericArguments();
    //                _meths[genericArguments.Length - 1] = info;
    //            }
    //            index++;
    //        }
    //    }

    //    private StrongDelegate(Func<object[], object> fn)
    //    {
    //        this.fn = fn;
    //    }

    //    public static Delegate CreateDelegate(Type delegateType, Func<object[], object> fn)
    //    {
    //        MethodInfo method = delegateType.GetMethod("Invoke");
    //        ParameterInfo[] parameters = method.GetParameters();
    //        Type[] typeArguments = new Type[1 + parameters.Length];
    //        int index = 0;
    //        int length = parameters.Length;
    //        while (index < length)
    //        {
    //            typeArguments[index] = parameters[index].ParameterType;
    //            index++;
    //        }
    //        typeArguments[typeArguments.Length - 1] = method.ReturnType;
    //        if (typeArguments.Length > _meths.Length)
    //        {
    //            throw new NotSupportedException("Delegate has too many arguments");
    //        }
    //        MethodInfo info3 = _meths[typeArguments.Length - 1].MakeGenericMethod(typeArguments);
    //        return Delegate.CreateDelegate(delegateType, new StrongDelegate(fn), info3);
    //    }

    //    public static Delegate CreateDelegate(Type delegateType, object target, MethodInfo method)
    //    {
    //        return CreateDelegate(delegateType, (Func<object[], object>) Delegate.CreateDelegate(typeof(Func<object[], object>), target, method));
    //    }

    //    public R M<R>()
    //    {
    //        return (R) this.fn.Invoke(null);
    //    }

    //    public R M<A1, R>(A1 a1)
    //    {
    //        return this.fn.Invoke(new object[] { a1 });
    //    }

    //    public R M<A1, A2, R>(A1 a1, A2 a2)
    //    {
    //        return (R) this.fn.Invoke(new object[] { a1, a2 });
    //    }

    //    public R M<A1, A2, A3, R>(A1 a1, A2 a2, A3 a3)
    //    {
    //        return (R) this.fn.Invoke(new object[] { a1, a2, a3 });
    //    }

    //    public R M<A1, A2, A3, A4, R>(A1 a1, A2 a2, A3 a3, A4 a4)
    //    {
    //        return (R) this.fn.Invoke(new object[] { a1, a2, a3, a4 });
    //    }

    //    public R M<A1, A2, A3, A4, A5, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5)
    //    {
    //        return (R) this.fn.Invoke(new object[] { a1, a2, a3, a4, a5 });
    //    }

    //    public R M<A1, A2, A3, A4, A5, A6, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6)
    //    {
    //        return (R) this.fn.Invoke(new object[] { a1, a2, a3, a4, a5, a6 });
    //    }

    //    public R M<A1, A2, A3, A4, A5, A6, A7, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7)
    //    {
    //        return (R) this.fn.Invoke(new object[] { a1, a2, a3, a4, a5, a6, a7 });
    //    }

    //    public R M<A1, A2, A3, A4, A5, A6, A7, A8, R>(A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8)
    //    {
    //        return (R) this.fn.Invoke(new object[] { a1, a2, a3, a4, a5, a6, a7, a8 });
    //    }
    //}
}

