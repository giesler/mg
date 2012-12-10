namespace System.Data.Services.Http
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Browser;

    internal sealed class ClientHttpWebRequest : HttpWebRequest
    {
        private ClientWebHeaderCollection headerCollection;
        private readonly System.Net.HttpWebRequest innerRequest;

        public ClientHttpWebRequest(Uri requestUri)
        {
            Debug.Assert(requestUri != null, "requestUri can't be null.");
            this.innerRequest = (System.Net.HttpWebRequest)WebRequestCreator.ClientHttp.Create(requestUri);
            Debug.Assert(this.innerRequest != null, "ClientHttp.Create failed to create a new request without throwing exception.");
        }

        public override void Abort()
        {
            this.innerRequest.Abort();
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return this.innerRequest.BeginGetRequestStream(callback, state);
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return this.innerRequest.BeginGetResponse(callback, state);
        }

        public override System.Net.WebHeaderCollection CreateEmptyWebHeaderCollection()
        {
            return new System.Net.WebHeaderCollection();
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return this.innerRequest.EndGetRequestStream(asyncResult);
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            WebResponse response3;
            try
            {
                System.Net.HttpWebResponse innerResponse = (System.Net.HttpWebResponse)this.innerRequest.EndGetResponse(asyncResult);
                response3 = new ClientHttpWebResponse(innerResponse, this);
            }
            catch (System.Net.WebException exception)
            {
                ClientHttpWebResponse response = new ClientHttpWebResponse((System.Net.HttpWebResponse)exception.Response, this);
                throw new WebException(exception.Message, exception, response);
            }
            return response3;
        }

        public override string Accept
        {
            get
            {
                return this.innerRequest.Accept;
            }
            set
            {
                this.innerRequest.Accept=value;
            }
        }

        public override bool AllowReadStreamBuffering
        {
            get
            {
                return this.innerRequest.AllowReadStreamBuffering;
            }
            set
            {
                this.innerRequest.AllowReadStreamBuffering = value;
            }
        }

        public override long ContentLength
        {
            set
            {
            }
        }

        public override string ContentType
        {
            get
            {
                return this.innerRequest.ContentType;
            }
            set
            {
                this.innerRequest.ContentType=value;
            }
        }

        public override WebHeaderCollection Headers
        {
            get
            {
                if (this.headerCollection == null)
                {
                    this.headerCollection = new ClientWebHeaderCollection(this.innerRequest.Headers, this.innerRequest);
                }
                return this.headerCollection;
            }
        }

        public override string Method
        {
            get
            {
                return this.innerRequest.Method;
            }
            set
            {
                this.innerRequest.Method = value;
            }
        }

        public override Uri RequestUri
        {
            get
            {
                return this.innerRequest.RequestUri;
            }
        }
    }
}

