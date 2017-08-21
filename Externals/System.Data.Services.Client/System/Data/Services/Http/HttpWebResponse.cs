namespace System.Data.Services.Http
{
    using System;

    internal abstract class HttpWebResponse : WebResponse, IDisposable
    {
        protected HttpWebResponse()
        {
        }

        public abstract string GetResponseHeader(string headerName);

        public abstract WebHeaderCollection Headers { get; }

        public abstract HttpWebRequest Request { get; }

        public abstract HttpStatusCode StatusCode { get; }
    }
}

