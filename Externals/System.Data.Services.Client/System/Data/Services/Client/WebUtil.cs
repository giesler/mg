namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Http;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal static class WebUtil
    {
        private static bool? dataServiceCollectionAvailable = null;

        internal static void ApplyHeadersToRequest(Dictionary<string, string> headers, HttpWebRequest request, bool ignoreAcceptHeader)
        {
            foreach (KeyValuePair<string, string> pair in headers)
            {
                if (string.Equals(pair.Key, "Accept", StringComparison.Ordinal))
                {
                    if (!ignoreAcceptHeader)
                    {
                        request.Accept = pair.Value;
                    }
                }
                else if (string.Equals(pair.Key, "Content-Type", StringComparison.Ordinal))
                {
                    request.ContentType = pair.Value;
                }
                else
                {
                    request.Headers[pair.Key] = pair.Value;
                }
            }
        }

        internal static long CopyStream(Stream input, Stream output, ref byte[] refBuffer)
        {
            Debug.Assert(null != input, "null input stream");
            Debug.Assert(null != output, "null output stream");
            long num = 0L;
            byte[] buffer = refBuffer;
            if (null == buffer)
            {
                refBuffer = buffer = new byte[0x3e8];
            }
            int count = 0;
            while (input.CanRead && (0 < (count = input.Read(buffer, 0, buffer.Length))))
            {
                output.Write(buffer, 0, count);
                num += count;
            }
            return num;
        }

        internal static Type GetDataServiceCollectionOfT(params Type[] typeArguments)
        {
            if (DataServiceCollectionAvailable)
            {
                Debug.Assert(GetDataServiceCollectionOfTType() != null, "DataServiceCollection is available so GetDataServiceCollectionOfTType() must not return null.");
                return GetDataServiceCollectionOfTType().MakeGenericType(typeArguments);
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Type GetDataServiceCollectionOfTType()
        {
            return typeof(DataServiceCollection<>);
        }

        internal static void GetHttpWebResponse(InvalidOperationException exception, ref HttpWebResponse response)
        {
            if (null == response)
            {
                WebException exception2 = exception as WebException;
                if (null != exception2)
                {
                    response = exception2.Response;
                }
            }
        }

        internal static bool IsDataServiceCollectionType(Type t)
        {
            return (DataServiceCollectionAvailable && (t == GetDataServiceCollectionOfTType()));
        }

        internal static bool SuccessStatusCode(HttpStatusCode status)
        {
            return ((HttpStatusCode.OK <= status) && (status < HttpStatusCode.Ambiguous));
        }

        internal static Dictionary<string, string> WrapResponseHeaders(HttpWebResponse response)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(EqualityComparer<string>.Default);
            if (null != response)
            {
                foreach (string str in response.Headers.AllKeys)
                {
                    dictionary.Add(str, response.Headers[str]);
                }
            }
            return dictionary;
        }

        private static bool DataServiceCollectionAvailable
        {
            get
            {
                if (!dataServiceCollectionAvailable.HasValue)
                {
                    try
                    {
                        dataServiceCollectionAvailable = new bool?(GetDataServiceCollectionOfTType() != null);
                    }
                    catch (FileNotFoundException)
                    {
                        dataServiceCollectionAvailable = false;
                    }
                }
                Debug.Assert(dataServiceCollectionAvailable.HasValue, "observableCollectionOfTAvailable must not be null here.");
                return dataServiceCollectionAvailable.Value;
            }
        }
    }
}

