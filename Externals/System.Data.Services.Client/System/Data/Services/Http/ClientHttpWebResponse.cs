namespace System.Data.Services.Http
{
    using System;
    using System.Diagnostics;
    using System.IO;

    internal sealed class ClientHttpWebResponse : HttpWebResponse
    {
        private ClientWebHeaderCollection headerCollection;
        private System.Net.HttpWebResponse innerResponse;
        private ClientHttpWebRequest request;

        internal ClientHttpWebResponse(System.Net.HttpWebResponse innerResponse, ClientHttpWebRequest request)
        {
            Debug.Assert(innerResponse != null, "innerResponse can't be null.");
            this.innerResponse = innerResponse;
            this.request = request;
            int num = (int)this.innerResponse.StatusCode;
            if ((num > 0x257) || (num < 100))
            {
                throw WebException.CreateInternal("HttpWebResponse.NormalizeResponseStatus");
            }
        }

        public override void Close()
        {
            this.innerResponse.Close();
        }

        protected override void Dispose(bool disposing)
        {
            ((IDisposable)this.innerResponse).Dispose();

        }

        public override string GetResponseHeader(string headerName)
        {
            return this.innerResponse.Headers[headerName];
        }

        public override Stream GetResponseStream()
        {
            return this.innerResponse.GetResponseStream();
        }

        public override long ContentLength
        {
            get
            {
                return this.innerResponse.ContentLength;
            }
        }

        public override string ContentType
        {
            get
            {
                return this.innerResponse.ContentType;
            }
        }

        public override WebHeaderCollection Headers
        {
            get
            {
                if (this.headerCollection == null)
                {
                    this.headerCollection = new ClientWebHeaderCollection(this.innerResponse.Headers);
                }
                return this.headerCollection;
            }
        }

        public override HttpWebRequest Request
        {
            get
            {
                return this.request;
            }
        }

        public override HttpStatusCode StatusCode
        {
            get
            {
                return (HttpStatusCode)this.innerResponse.StatusCode;
            }
        }
    }
}

