namespace System.Data.Services.Client
{
    using IQToolkit;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Xml;

    internal static class Util
    {
        internal const string CodeGeneratorToolName = "System.Data.Services.Design";
        internal static readonly Version DataServiceVersion1 = new Version(1, 0);
        internal static readonly Version DataServiceVersion2 = new Version(2, 0);
        internal static readonly Version DataServiceVersionEmpty = new Version(0, 0);
        private static Action<string> DebugFaultInjector = delegate (string s) {
        };
        private static Func<string, string> dereferenceIdentity;
        internal static readonly char[] ForwardSlash = new char[] { '/' };
        internal static readonly Version MaxResponseVersion = DataServiceVersion2;
        private static Func<string, string> referenceIdentity;
        internal static readonly Version[] SupportedResponseVersions = new Version[] { DataServiceVersion1, DataServiceVersion2 };
        internal const string VersionSuffix = ";NetFx";
        private static char[] whitespaceForTracing = new char[] { '\r', '\n', ' ', ' ', ' ', ' ', ' ' };

        static Util()
        {
            DataServiceVersionEmpty = new Version(0, 0);
            DataServiceVersion1 = new Version(1, 0);
            DataServiceVersion2 = new Version(2, 0);
            MaxResponseVersion = DataServiceVersion2;
            SupportedResponseVersions = new Version[] { DataServiceVersion1, DataServiceVersion2 };
            ForwardSlash = new char[] { '/' };
            whitespaceForTracing = new char[] { '\r', '\n', ' ', ' ', ' ', ' ', ' ' };
            DebugFaultInjector = delegate(string s)
            {
            };
            referenceIdentity = delegate(string identity)
            {
                return identity;
            };
            dereferenceIdentity = delegate(string identity)
            {
                return identity;
            };

        }

        internal static object ActivatorCreateInstance(Type type, params object[] arguments)
        {
            Debug.Assert(type != null, "type != null");
            int num = (arguments == null) ? 0 : arguments.Length;
            ConstructorInfo[] constructors = type.GetConstructors();
            ConstructorInfo constructor = null;
            for (int i = 0; i < constructors.Length; i++)
            {
                if (constructors[i].GetParameters().Length == num)
                {
                    Debug.Assert(constructor == null, "Make sure that the specific type has only one constructor with specified argument count");
                    constructor = constructors[i];
                }
            }
            if (constructor == null)
            {
                throw new MissingMethodException();
            }
            return ConstructorInvoke(constructor, arguments);
        }

        internal static bool AreSame(string value1, string value2)
        {
            return (value1 == value2);
        }

        internal static bool AreSame(XmlReader reader, string localName, string namespaceUri)
        {
            Debug.Assert(((reader != null) && (localName != null)) && (null != namespaceUri), "null");
            return ((((XmlNodeType.Element == reader.NodeType) || (XmlNodeType.EndElement == reader.NodeType)) && AreSame(reader.LocalName, localName)) && AreSame(reader.NamespaceURI, namespaceUri));
        }

        internal static void CheckArgumentNotEmpty(string value, string parameterName)
        {
            CheckArgumentNull<string>(value, parameterName);
            if (0 == value.Length)
            {
                throw Error.Argument(Strings.Util_EmptyString, parameterName);
            }
        }

        internal static void CheckArgumentNotEmpty<T>(T[] value, string parameterName) where T: class
        {
            CheckArgumentNull<T[]>(value, parameterName);
            if (0 == value.Length)
            {
                throw Error.Argument(Strings.Util_EmptyArray, parameterName);
            }
            for (int i = 0; i < value.Length; i++)
            {
                if (object.ReferenceEquals(value[i], null))
                {
                    throw Error.Argument(Strings.Util_NullArrayElement, parameterName);
                }
            }
        }

        internal static T CheckArgumentNull<T>(T value, string parameterName) where T: class
        {
            if (null == value)
            {
                throw Error.ArgumentNull(parameterName);
            }
            return value;
        }

        internal static HttpStack CheckEnumerationValue(HttpStack value, string parameterName)
        {
            switch (value)
            {
                case HttpStack.Auto:
                case HttpStack.ClientHttp:
                case HttpStack.XmlHttp:
                    return value;
            }
            throw Error.ArgumentOutOfRange(parameterName);
        }

        internal static MergeOption CheckEnumerationValue(MergeOption value, string parameterName)
        {
            switch (value)
            {
                case MergeOption.AppendOnly:
                case MergeOption.OverwriteChanges:
                case MergeOption.PreserveChanges:
                case MergeOption.NoTracking:
                    return value;
            }
            throw Error.ArgumentOutOfRange(parameterName);
        }

        internal static object ConstructorInvoke(ConstructorInfo constructor, object[] arguments)
        {
            ParameterExpression expression;
            if (constructor == null)
            {
                throw new MissingMethodException();
            }
            int num = (arguments == null) ? 0 : arguments.Length;
            Expression[] expressionArray = new Expression[num];
            ParameterInfo[] parameters = constructor.GetParameters();
            for (int i = 0; i < expressionArray.Length; i++)
            {
                expressionArray[i] = Expression.Constant(arguments[i], parameters[i].ParameterType);
            }
            Expression<Func<object[], object>> function = Expression.Lambda<Func<object[], object>>(Expression.Convert(Expression.New(constructor, expressionArray), typeof(object)), new ParameterExpression[] { expression = Expression.Parameter(typeof(object[]), "arguments") });
            return ExpressionEvaluator.Eval(function, arguments);
        }

        internal static bool ContainsReference<T>(T[] array, T value) where T: class
        {
            return (0 <= IndexOfReference<T>(array, value));
        }

        internal static Uri CreateUri(string value, UriKind kind)
        {
            return ((value == null) ? null : new Uri(value, kind));
        }

        internal static Uri CreateUri(Uri baseUri, Uri requestUri)
        {
            Debug.Assert((null != baseUri) && baseUri.IsAbsoluteUri, "baseUri !IsAbsoluteUri");
            Debug.Assert(string.IsNullOrEmpty(baseUri.Query) && string.IsNullOrEmpty(baseUri.Fragment), "baseUri has query or fragment");
            CheckArgumentNull<Uri>(requestUri, "requestUri");
            if (!requestUri.IsAbsoluteUri)
            {
                if (baseUri.OriginalString.EndsWith("/", StringComparison.Ordinal))
                {
                    if (requestUri.OriginalString.StartsWith("/", StringComparison.Ordinal))
                    {
                        requestUri = new Uri(baseUri, CreateUri(requestUri.OriginalString.TrimStart(ForwardSlash), UriKind.Relative));
                        return requestUri;
                    }
                    requestUri = new Uri(baseUri, requestUri);
                    return requestUri;
                }
                requestUri = CreateUri(baseUri.OriginalString + "/" + requestUri.OriginalString.TrimStart(ForwardSlash), UriKind.Absolute);
            }
            return requestUri;
        }

        [Conditional("DEBUG")]
        internal static void DebugInjectFault(string state)
        {
            DebugFaultInjector(state);
        }

        internal static string DereferenceIdentity(string uri)
        {
            return dereferenceIdentity.Invoke(uri);
        }

        internal static void Dispose<T>(T disposable) where T: class, IDisposable
        {
            if (null != disposable)
            {
                disposable.Dispose();
            }
        }

        internal static void Dispose<T>(ref T disposable) where T: class, IDisposable
        {
            Dispose<T>((T) disposable);
            disposable = default(T);
        }

        internal static bool DoesNullAttributeSayTrue(XmlReader reader)
        {
            string attribute = reader.GetAttribute("null", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
            return ((attribute != null) && XmlConvert.ToBoolean(attribute));
        }

        internal static bool DoNotHandleException(Exception ex)
        {
            return ((ex != null) && (((ex is StackOverflowException) || (ex is OutOfMemoryException)) || (ex is ThreadAbortException)));
        }

        internal static Type GetTypeAllowingNull(Type type)
        {
            Debug.Assert(type != null, "type != null");
            return (TypeAllowsNull(type) ? type : typeof(Nullable<>).MakeGenericType(new Type[] { type }));
        }

        internal static char[] GetWhitespaceForTracing(int depth)
        {
            char[] whitespaceForTracing = Util.whitespaceForTracing;
            while (whitespaceForTracing.Length <= depth)
            {
                char[] chArray2 = new char[2 * whitespaceForTracing.Length];
                chArray2[0] = '\r';
                chArray2[1] = '\n';
                for (int i = 2; i < chArray2.Length; i++)
                {
                    chArray2[i] = ' ';
                }
                Interlocked.CompareExchange<char[]>(ref Util.whitespaceForTracing, chArray2, whitespaceForTracing);
                whitespaceForTracing = chArray2;
            }
            return whitespaceForTracing;
        }

        internal static int IndexOfReference<T>(T[] array, T value) where T: class
        {
            Debug.Assert(null != array, "null array");
            for (int i = 0; i < array.Length; i++)
            {
                if (object.ReferenceEquals(array[i], value))
                {
                    return i;
                }
            }
            return -1;
        }

        internal static bool IsKnownClientExcption(Exception ex)
        {
            return (((ex is DataServiceClientException) || (ex is DataServiceQueryException)) || (ex is DataServiceRequestException));
        }

        private static bool IsNullableType(Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        internal static T NullCheck<T>(T value, InternalError errorcode) where T: class
        {
            if (object.ReferenceEquals(value, null))
            {
                Error.ThrowInternalError(errorcode);
            }
            return value;
        }

        internal static string ReferenceIdentity(string uri)
        {
            return referenceIdentity.Invoke(uri);
        }

        internal static void SetNextLinkForCollection(object collection, DataServiceQueryContinuation continuation)
        {
            Debug.Assert(collection != null, "collection != null");
            foreach (PropertyInfo info in collection.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (((info.Name == "Continuation") && info.CanWrite) && typeof(DataServiceQueryContinuation).IsAssignableFrom(info.PropertyType))
                {
                    info.SetValue(collection, continuation, null);
                }
            }
        }

        [Conditional("TRACE")]
        internal static void TraceElement(XmlReader reader, TextWriter writer)
        {
            Debug.Assert(XmlNodeType.Element == reader.NodeType, "not positioned on Element");
            if (null != writer)
            {
                writer.Write(GetWhitespaceForTracing(2 + reader.Depth), 0, 2 + reader.Depth);
                writer.Write("<{0}", reader.Name);
                if (reader.MoveToFirstAttribute())
                {
                    do
                    {
                        writer.Write(" {0}=\"{1}\"", reader.Name, reader.Value);
                    }
                    while (reader.MoveToNextAttribute());
                    reader.MoveToElement();
                }
                writer.Write(reader.IsEmptyElement ? " />" : ">");
            }
        }

        [Conditional("TRACE")]
        internal static void TraceEndElement(XmlReader reader, TextWriter writer, bool indent)
        {
            if (null != writer)
            {
                if (indent)
                {
                    writer.Write(GetWhitespaceForTracing(2 + reader.Depth), 0, 2 + reader.Depth);
                }
                writer.Write("</{0}>", reader.Name);
            }
        }

        [Conditional("TRACE")]
        internal static void TraceText(TextWriter writer, string value)
        {
            if (null != writer)
            {
                writer.Write(value);
            }
        }

        internal static bool TypeAllowsNull(Type type)
        {
            Debug.Assert(type != null, "type != null");
            return (!type.IsValueType || IsNullableType(type));
        }
    }
}

