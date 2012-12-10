namespace System.Data.Services.Http
{
    using System;
    using System.Net;

    internal abstract class HttpWebRequest : WebRequest, IDisposable
    {
        protected HttpWebRequest()
        {
        }

        public abstract System.Net.WebHeaderCollection CreateEmptyWebHeaderCollection();
        protected abstract void Dispose(bool disposing);
        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        public abstract string Accept { get; set; }

        public abstract bool AllowReadStreamBuffering { get; set; }

        public abstract long ContentLength { set; }
    }
}

