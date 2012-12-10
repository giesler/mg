namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Message}")]
    public sealed class DataServiceClientException : InvalidOperationException
    {
        private readonly int statusCode;

        public DataServiceClientException() : this(Strings.DataServiceException_GeneralError)
        {
        }

        public DataServiceClientException(string message) : this(message, (Exception) null)
        {
        }

        public DataServiceClientException(string message, Exception innerException) : this(message, innerException, 500)
        {
        }

        public DataServiceClientException(string message, int statusCode) : this(message, null, statusCode)
        {
        }

        public DataServiceClientException(string message, Exception innerException, int statusCode) : base(message, innerException)
        {
            this.statusCode = statusCode;
        }

        public int StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }
    }
}

