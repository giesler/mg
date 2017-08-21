namespace System.Data.Services.Http
{
    using System;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.IO;

    internal abstract class WebRequest
    {
        protected WebRequest()
        {
        }

        public abstract void Abort();
        public abstract IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state);
        public abstract IAsyncResult BeginGetResponse(AsyncCallback callback, object state);
        public static WebRequest Create(Uri requestUri, HttpStack httpStack)
        {
            Debug.Assert(requestUri != null, "requestUri != null");
            if ((requestUri.Scheme != Uri.UriSchemeHttp) && (requestUri.Scheme != Uri.UriSchemeHttps))
            {
                throw new NotSupportedException();
            }
            if (httpStack == HttpStack.Auto)
            {
                if (UriRequiresClientHttpWebRequest(requestUri))
                {
                    httpStack = HttpStack.ClientHttp;
                }
                else
                {
                    httpStack = HttpStack.XmlHttp;
                }
            }
            Debug.Assert(httpStack == HttpStack.ClientHttp, "Only ClientHttp is supported for now.");
            return new ClientHttpWebRequest(requestUri);
        }

        public abstract Stream EndGetRequestStream(IAsyncResult asyncResult);
        public abstract WebResponse EndGetResponse(IAsyncResult asyncResult);
        private static bool UriRequiresClientHttpWebRequest(Uri uri)
        {
            return true;
        }

        public abstract string ContentType { get; set; }

        public abstract WebHeaderCollection Headers { get; }

        public abstract string Method { get; set; }

        public abstract Uri RequestUri { get; }
    }
}

