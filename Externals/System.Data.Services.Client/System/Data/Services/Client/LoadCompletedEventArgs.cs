namespace System.Data.Services.Client
{
    using System;
    using System.ComponentModel;

    public sealed class LoadCompletedEventArgs : AsyncCompletedEventArgs
    {
        private System.Data.Services.Client.QueryOperationResponse queryOperationResponse;

        internal LoadCompletedEventArgs(System.Data.Services.Client.QueryOperationResponse queryOperationResponse, Exception error) : base(error, false, null)
        {
            this.queryOperationResponse = queryOperationResponse;
        }

        public System.Data.Services.Client.QueryOperationResponse QueryOperationResponse
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return this.queryOperationResponse;
            }
        }
    }
}

