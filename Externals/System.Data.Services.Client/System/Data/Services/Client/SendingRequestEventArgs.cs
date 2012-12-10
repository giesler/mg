namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Data.Services.Http;

    public class SendingRequestEventArgs : EventArgs
    {
        private System.Net.WebRequest request;
        private System.Net.WebHeaderCollection requestHeaders;

        internal SendingRequestEventArgs(System.Net.WebRequest request, System.Net.WebHeaderCollection requestHeaders)
        {
            Debug.Assert(null == request, "non-null request in SL.");
            Debug.Assert(null != requestHeaders, "null requestHeaders");
            this.request = request;
            this.requestHeaders = requestHeaders;
        }

        public System.Net.WebHeaderCollection RequestHeaders
        {
            get
            {
                return this.requestHeaders;
            }
        }
    }
}

