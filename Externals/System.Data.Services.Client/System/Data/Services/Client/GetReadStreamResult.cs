namespace System.Data.Services.Client
{
    using System;
    using System.Data.Services.Http;
    using System.Diagnostics;

    internal class GetReadStreamResult : BaseAsyncResult
    {
        private readonly HttpWebRequest request;
        private HttpWebResponse response;

        internal GetReadStreamResult(object source, string method, HttpWebRequest request, AsyncCallback callback, object state) : base(source, method, callback, state)
        {
            Debug.Assert(request != null, "Null request can't be wrapped to a result.");
            this.request = request;
            base.Abortable = request;
        }

        private static void AsyncEndGetResponse(IAsyncResult asyncResult)
        {
            GetReadStreamResult asyncState = asyncResult.AsyncState as GetReadStreamResult;
            Debug.Assert(asyncState != null, "Async callback got called for different request.");
            try
            {
                asyncState.CompletedSynchronously &= asyncResult.CompletedSynchronously;
                HttpWebRequest request = Util.NullCheck<HttpWebRequest>(asyncState.request, InternalError.InvalidEndGetResponseRequest);
                HttpWebResponse webResponse = null;
                try
                {
                    webResponse = (HttpWebResponse) request.EndGetResponse(asyncResult);
                }
                catch (WebException exception)
                {
                    webResponse = exception.Response;
                    if (null == webResponse)
                    {
                        throw;
                    }
                }
                asyncState.SetHttpWebResponse(webResponse);
                asyncState.SetCompleted();
            }
            catch (Exception exception2)
            {
                if (asyncState.HandleFailure(exception2))
                {
                    throw;
                }
            }
            finally
            {
                asyncState.HandleCompleted();
            }
        }

        internal void Begin()
        {
            try
            {
                IAsyncResult asyncResult;
                asyncResult = BaseAsyncResult.InvokeAsync(this.request.BeginGetResponse, GetReadStreamResult.AsyncEndGetResponse, this);

                this.CompletedSynchronously &= asyncResult.CompletedSynchronously;
            }
            catch (Exception e)
            {
                this.HandleFailure(e);
                throw;
            }
            finally
            {
                this.HandleCompleted();
            }

            Debug.Assert(!this.CompletedSynchronously || this.IsCompleted, "if CompletedSynchronously then MUST IsCompleted");
        }

        protected override void CompletedRequest()
        {
            Debug.Assert(null != this.response || null != this.Failure, "should have response or exception");
            if (null != this.response)
            {
                InvalidOperationException failure = null;
                if (!WebUtil.SuccessStatusCode(this.response.StatusCode))
                {
                    failure = DataServiceContext.GetResponseText(this.response.GetResponseStream, this.response.StatusCode);
                }

                if (failure != null)
                {
                    this.response.Close();
                    this.HandleFailure(failure);
                }
            }
        }

        internal DataServiceStreamResponse End()
        {
            if (this.response != null)
            {
                return new DataServiceStreamResponse(this.response);
            }
            return null;
        }

        private void SetHttpWebResponse(HttpWebResponse webResponse)
        {
            Debug.Assert(webResponse != null, "Can't set a null response.");
            this.response = webResponse;
        }
    }
}

