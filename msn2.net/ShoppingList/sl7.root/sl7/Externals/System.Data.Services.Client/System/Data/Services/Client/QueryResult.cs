namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Http;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    internal class QueryResult : BaseAsyncResult
    {
        private Stream asyncResponseStream;
        private byte[] asyncStreamCopyBuffer;
        private long contentLength;
        private string contentType;
        private HttpWebResponse httpWebResponse;
        internal readonly HttpWebRequest Request;
        private Stream requestStream;
        private MemoryStream requestStreamContent;
        private Stream responseStream;
        private bool responseStreamOwner;
        private static byte[] reusableAsyncCopyBuffer;
        internal readonly DataServiceRequest ServiceRequest;
        private HttpStatusCode statusCode;
        private bool usingBuffer;

        internal QueryResult(object source, string method, DataServiceRequest serviceRequest, HttpWebRequest request, AsyncCallback callback, object state) : base(source, method, callback, state)
        {
            Debug.Assert(null != request, "null request");
            this.ServiceRequest = serviceRequest;
            this.Request = request;
            base.Abortable = request;
        }

        private static void AsyncEndGetResponse(IAsyncResult asyncResult)
        {
            Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult.IsCompleted");
            QueryResult asyncState = asyncResult.AsyncState as QueryResult;
            try
            {
                CompleteCheck(asyncState, InternalError.InvalidEndGetResponseCompleted);
                asyncState.CompletedSynchronously &= asyncResult.CompletedSynchronously;
                HttpWebRequest request = Util.NullCheck<HttpWebRequest>(asyncState.Request, InternalError.InvalidEndGetResponseRequest);
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse) request.EndGetResponse(asyncResult);
                }
                catch (WebException exception)
                {
                    response = exception.Response;
                    if (null == response)
                    {
                        throw;
                    }
                }
                asyncState.SetHttpWebResponse(Util.NullCheck<HttpWebResponse>(response, InternalError.InvalidEndGetResponseResponse));
                Debug.Assert(null == asyncState.asyncResponseStream, "non-null asyncResponseStream");
                Stream responseStream = null;
                if (HttpStatusCode.NoContent != response.StatusCode)
                {
                    responseStream = response.GetResponseStream();
                    asyncState.asyncResponseStream = responseStream;
                }
                if ((responseStream != null) && responseStream.CanRead)
                {
                    if (null == asyncState.responseStream)
                    {
                        asyncState.responseStream = Util.NullCheck<Stream>(asyncState.GetAsyncResponseStreamCopy(), InternalError.InvalidAsyncResponseStreamCopy);
                    }
                    if (null == asyncState.asyncStreamCopyBuffer)
                    {
                        asyncState.asyncStreamCopyBuffer = Util.NullCheck<byte[]>(asyncState.GetAsyncResponseStreamCopyBuffer(), InternalError.InvalidAsyncResponseStreamCopyBuffer);
                    }
                    ReadResponseStream(asyncState);
                }
                else
                {
                    asyncState.SetCompleted();
                }
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

        private static void AsyncEndRead(IAsyncResult asyncResult)
        {
            Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult.IsCompleted");
            QueryResult asyncState = asyncResult.AsyncState as QueryResult;
            int count = 0;
            try
            {
                CompleteCheck(asyncState, InternalError.InvalidEndReadCompleted);
                asyncState.CompletedSynchronously &= asyncResult.CompletedSynchronously;
                Stream stream = Util.NullCheck<Stream>(asyncState.asyncResponseStream, InternalError.InvalidEndReadStream);
                Stream stream2 = Util.NullCheck<Stream>(asyncState.responseStream, InternalError.InvalidEndReadCopy);
                byte[] buffer = Util.NullCheck<byte[]>(asyncState.asyncStreamCopyBuffer, InternalError.InvalidEndReadBuffer);
                count = stream.EndRead(asyncResult);
                asyncState.usingBuffer = false;
                if (0 < count)
                {
                    stream2.Write(buffer, 0, count);
                }
                if (((0 < count) && (0 < buffer.Length)) && stream.CanRead)
                {
                    if (!asyncResult.CompletedSynchronously)
                    {
                        ReadResponseStream(asyncState);
                    }
                }
                else
                {
                    if (stream2.Position < stream2.Length)
                    {
                        ((MemoryStream) stream2).SetLength(stream2.Position);
                    }
                    asyncState.SetCompleted();
                }
            }
            catch (Exception exception)
            {
                if (asyncState.HandleFailure(exception))
                {
                    throw;
                }
            }
            finally
            {
                asyncState.HandleCompleted();
            }
        }

        internal void BeginExecute()
        {
            try
            {
                IAsyncResult result = BaseAsyncResult.InvokeAsync(this.Request.BeginGetResponse, QueryResult.AsyncEndGetResponse, this);
                //BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(this.Request, (IntPtr) this.Request.BeginGetResponse), new AsyncCallback(QueryResult.AsyncEndGetResponse), this);
                base.CompletedSynchronously &= result.CompletedSynchronously;
            }
            catch (Exception exception)
            {
                base.HandleFailure(exception);
                throw;
            }
            finally
            {
                base.HandleCompleted();
            }
            Debug.Assert(!base.CompletedSynchronously || base.IsCompleted, "if CompletedSynchronously then MUST IsCompleted");
        }

        private static void CompleteCheck(QueryResult pereq, InternalError errorcode)
        {
            if ((pereq == null) || (pereq.IsCompletedInternally && !pereq.IsAborted))
            {
                Error.ThrowInternalError(errorcode);
            }
        }

        protected override void CompletedRequest()
        {
            Util.Dispose<Stream>(ref this.asyncResponseStream);
            Util.Dispose<Stream>(ref this.requestStream);
            Util.Dispose<MemoryStream>(ref this.requestStreamContent);
            byte[] asyncStreamCopyBuffer = this.asyncStreamCopyBuffer;
            this.asyncStreamCopyBuffer = null;
            if (!((asyncStreamCopyBuffer == null) || this.usingBuffer))
            {
                this.PutAsyncResponseStreamCopyBuffer(asyncStreamCopyBuffer);
            }
            if (this.responseStreamOwner && (null != this.responseStream))
            {
                this.responseStream.Position = 0L;
            }
            Debug.Assert((this.httpWebResponse != null) || (null != base.Failure), "should have response or exception");
            if (null != this.httpWebResponse)
            {
                this.httpWebResponse.Close();

                Exception ex = DataServiceContext.HandleResponse(this.StatusCode, this.httpWebResponse.Headers[XmlConstants.HttpDataServiceVersion], this.GetResponseStream, false);
                if (null != ex)
                {
                    this.HandleFailure(ex);
                }
            }
        }

        internal static QueryResult EndExecute<TElement>(object source, IAsyncResult asyncResult)
        {
            QueryResult result = null;
            try
            {
                result = BaseAsyncResult.EndExecute<QueryResult>(source, "Execute", asyncResult);
            }
            catch (InvalidOperationException exception)
            {
                result = asyncResult as QueryResult;
                Debug.Assert(result != null, "response != null, BaseAsyncResult.EndExecute() would have thrown a different exception otherwise.");
                QueryOperationResponse response = result.GetResponse<TElement>(MaterializeAtom.EmptyResults);
                if (response != null)
                {
                    response.Error = exception;
                    throw new DataServiceQueryException(Strings.DataServiceException_GeneralError, exception, response);
                }
                throw;
            }
            return result;
        }

        protected virtual Stream GetAsyncResponseStreamCopy()
        {
            this.responseStreamOwner = true;
            long contentLength = this.contentLength;
            if ((0L < contentLength) && (contentLength <= 0x7fffffffL))
            {
                Debug.Assert(null == this.asyncStreamCopyBuffer, "not expecting buffer");
                return new MemoryStream((int) contentLength);
            }
            return new MemoryStream();
        }

        protected virtual byte[] GetAsyncResponseStreamCopyBuffer()
        {
            Debug.Assert(null == this.asyncStreamCopyBuffer, "non-null this.asyncStreamCopyBuffer");
            return (Interlocked.Exchange<byte[]>(ref reusableAsyncCopyBuffer, null) ?? new byte[0x1f40]);
        }

        internal MaterializeAtom GetMaterializer(DataServiceContext context, ProjectionPlan plan)
        {
            Debug.Assert(base.IsCompletedInternally, "request hasn't completed yet");
            if (HttpStatusCode.NoContent != this.StatusCode)
            {
                return DataServiceRequest.Materialize(context, this.ServiceRequest.QueryComponents, plan, this.ContentType, this.GetResponseStream());
            }
            return MaterializeAtom.EmptyResults;
        }

        internal QueryOperationResponse<TElement> GetResponse<TElement>(MaterializeAtom results)
        {
            if (this.httpWebResponse != null)
            {
                QueryOperationResponse<TElement> response = new QueryOperationResponse<TElement>(WebUtil.WrapResponseHeaders(this.httpWebResponse), this.ServiceRequest, results);
                response.StatusCode = (int) this.httpWebResponse.StatusCode;
                return response;
            }
            return null;
        }

        internal Stream GetResponseStream()
        {
            return this.responseStream;
        }

        internal QueryOperationResponse GetResponseWithType(MaterializeAtom results, Type elementType)
        {
            if (this.httpWebResponse != null)
            {
                Dictionary<string, string> headers = WebUtil.WrapResponseHeaders(this.httpWebResponse);
                QueryOperationResponse response = QueryOperationResponse.GetInstance(elementType, headers, this.ServiceRequest, results);
                response.StatusCode = (int) this.httpWebResponse.StatusCode;
                return response;
            }
            return null;
        }

        internal QueryOperationResponse<TElement> ProcessResult<TElement>(DataServiceContext context, ProjectionPlan plan)
        {
            MaterializeAtom results = DataServiceRequest.Materialize(context, this.ServiceRequest.QueryComponents, plan, this.ContentType, this.GetResponseStream());
            return this.GetResponse<TElement>(results);
        }

        protected virtual void PutAsyncResponseStreamCopyBuffer(byte[] buffer)
        {
            reusableAsyncCopyBuffer = buffer;
        }

        private static void ReadResponseStream(QueryResult queryResult)
        {
            IAsyncResult result;
            byte[] asyncStreamCopyBuffer = queryResult.asyncStreamCopyBuffer;
            Stream asyncResponseStream = queryResult.asyncResponseStream;
            do
            {
                int offset = 0;
                int length = asyncStreamCopyBuffer.Length;
                queryResult.usingBuffer = true;
                result = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(asyncResponseStream.BeginRead), asyncStreamCopyBuffer, offset, length, new AsyncCallback(QueryResult.AsyncEndRead), queryResult);
                queryResult.CompletedSynchronously &= result.CompletedSynchronously;
            }
            while ((result.CompletedSynchronously && !queryResult.IsCompletedInternally) && asyncResponseStream.CanRead);
            Debug.Assert(!queryResult.CompletedSynchronously || queryResult.IsCompletedInternally, "AsyncEndGetResponse !IsCompleted");
        }

        protected virtual void SetHttpWebResponse(HttpWebResponse response)
        {
            this.httpWebResponse = response;
            this.statusCode = response.StatusCode;
            this.contentLength = response.ContentLength;
            this.contentType = response.ContentType;
        }

        internal long ContentLength
        {
            get
            {
                return this.contentLength;
            }
        }

        internal string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        internal HttpStatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }
    }
}

