namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Message}")]
    public sealed class DataServiceQueryException : InvalidOperationException
    {
        private readonly QueryOperationResponse response;

        public DataServiceQueryException() : base(Strings.DataServiceException_GeneralError)
        {
        }

        public DataServiceQueryException(string message) : base(message)
        {
        }

        public DataServiceQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DataServiceQueryException(string message, Exception innerException, QueryOperationResponse response) : base(message, innerException)
        {
            this.response = response;
        }

        public QueryOperationResponse Response
        {
            get
            {
                return this.response;
            }
        }
    }
}

