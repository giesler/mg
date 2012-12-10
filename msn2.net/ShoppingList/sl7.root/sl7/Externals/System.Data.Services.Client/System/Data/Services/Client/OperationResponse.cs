namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class OperationResponse
    {
        private Dictionary<string, string> headers;
        private Exception innerException;
        private int statusCode;

        internal OperationResponse(Dictionary<string, string> headers)
        {
            Debug.Assert(null != headers, "null headers");
            this.headers = headers;
        }

        public Exception Error
        {
            get
            {
                return this.innerException;
            }
            set
            {
                Debug.Assert(null != value, "should not set null");
                this.innerException = value;
            }
        }

        public IDictionary<string, string> Headers
        {
            get
            {
                return this.headers;
            }
        }

        public int StatusCode
        {
            get
            {
                return this.statusCode;
            }
            internal set
            {
                this.statusCode = value;
            }
        }
    }
}

