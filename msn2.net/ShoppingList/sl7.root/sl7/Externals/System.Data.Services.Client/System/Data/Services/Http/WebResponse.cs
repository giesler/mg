namespace System.Data.Services.Http
{
    using System;
    using System.IO;

    internal abstract class WebResponse : IDisposable
    {
        protected WebResponse()
        {
        }

        public abstract void Close();
        protected abstract void Dispose(bool disposing);
        public abstract Stream GetResponseStream();
        void IDisposable.Dispose()
        {
            this.Dispose(false);
        }

        public abstract long ContentLength { get; }

        public abstract string ContentType { get; }
    }
}

