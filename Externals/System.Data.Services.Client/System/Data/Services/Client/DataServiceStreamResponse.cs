﻿namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Http;
    using System.Diagnostics;
    using System.IO;

    public sealed class DataServiceStreamResponse : IDisposable
    {
        private Dictionary<string, string> headers;
        private HttpWebResponse response;

        internal DataServiceStreamResponse(HttpWebResponse response)
        {
            Debug.Assert(response != null, "Can't create a stream response object from a null response.");
            this.response = response;
        }

        private void CheckDisposed()
        {
            if (this.response == null)
            {
                Error.ThrowObjectDisposed(base.GetType());
            }
        }

        public void Dispose()
        {
            Util.Dispose<HttpWebResponse>(ref this.response);
        }

        public string ContentDisposition
        {
            get
            {
                this.CheckDisposed();
                return this.response.Headers["Content-Disposition"];
            }
        }

        public string ContentType
        {
            get
            {
                this.CheckDisposed();
                return this.response.Headers["Content-Type"];
            }
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                this.CheckDisposed();
                if (this.headers == null)
                {
                    this.headers = WebUtil.WrapResponseHeaders(this.response);
                }
                return this.headers;
            }
        }

        public System.IO.Stream Stream
        {
            get
            {
                this.CheckDisposed();
                return this.response.GetResponseStream();
            }
        }
    }
}
