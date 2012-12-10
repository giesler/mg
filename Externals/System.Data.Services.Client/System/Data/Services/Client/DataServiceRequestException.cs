namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Message}")]
    public sealed class DataServiceRequestException : InvalidOperationException
    {
        private readonly DataServiceResponse response;

        public DataServiceRequestException() : base(Strings.DataServiceException_GeneralError)
        {
        }

        public DataServiceRequestException(string message) : base(message)
        {
        }

        public DataServiceRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DataServiceRequestException(string message, Exception innerException, DataServiceResponse response) : base(message, innerException)
        {
            this.response = response;
        }

        public DataServiceResponse Response
        {
            get
            {
                return this.response;
            }
        }
    }
}

