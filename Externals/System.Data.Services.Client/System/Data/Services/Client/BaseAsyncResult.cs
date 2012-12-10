namespace System.Data.Services.Client
{
    using System;
    using System.Data.Services.Http;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal abstract class BaseAsyncResult : IAsyncResult
    {
        private WebRequest abortable;
        private ManualResetEvent asyncWait;
        private bool asyncWaitDisposed;
        private object asyncWaitDisposeLock;
        private int completed;
        private bool completedSynchronously = true;
        private int done;
        private Exception failure;
        internal readonly string Method;
        internal readonly object Source;
        private readonly AsyncCallback userCallback;
        private bool userCompleted;
        private int userNotified;
        private readonly object userState;

        internal BaseAsyncResult(object source, string method, AsyncCallback callback, object state)
        {
            Debug.Assert(null != source, "null source");
            this.Source = source;
            this.Method = method;
            this.userCallback = callback;
            this.userState = state;
        }

        protected abstract void CompletedRequest();
        internal static T EndExecute<T>(object source, string method, IAsyncResult asyncResult) where T: BaseAsyncResult
        {
            Util.CheckArgumentNull<IAsyncResult>(asyncResult, "asyncResult");
            T local = asyncResult as T;
            if (((local == null) || (source != local.Source)) || (local.Method != method))
            {
                throw Error.Argument(Strings.Context_DidNotOriginateAsync, "asyncResult");
            }
            Debug.Assert((local.CompletedSynchronously && local.IsCompleted) || !local.CompletedSynchronously, "CompletedSynchronously && !IsCompleted");
            if (!local.IsCompleted)
            {
                local.AsyncWaitHandle.WaitOne();
                Debug.Assert(local.IsCompleted, "not completed after waiting");
            }
            if (Interlocked.Exchange(ref local.done, 1) != 0)
            {
                throw Error.Argument(Strings.Context_AsyncAlreadyDone, "asyncResult");
            }
            if (null != local.asyncWait)
            {
                Interlocked.CompareExchange<object>(ref local.asyncWaitDisposeLock, new object(), null);
                lock (local.asyncWaitDisposeLock)
                {
                    local.asyncWaitDisposed = true;
                    Util.Dispose<ManualResetEvent>(local.asyncWait);
                }
            }
            if (local.IsAborted)
            {
                throw Error.InvalidOperation(Strings.Context_OperationCanceled);
            }
            if (null == local.Failure)
            {
                return local;
            }
            if (Util.IsKnownClientExcption(local.Failure))
            {
                throw local.Failure;
            }
            throw Error.InvalidOperation(Strings.DataServiceException_GeneralError, local.Failure);
        }

        private static AsyncCallback GetDataServiceAsyncCallback(AsyncCallback callback)
        {
            return delegate (IAsyncResult asyncResult) {
                Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult != null && asyncResult.IsCompleted");
                if (!asyncResult.CompletedSynchronously)
                {
                    callback(asyncResult);
                }
            };
        }

        internal void HandleCompleted()
        {
            if (this.IsCompletedInternally && (Interlocked.Exchange(ref this.userNotified, 1) == 0))
            {
                this.abortable = null;
                try
                {
                    if (!Util.DoNotHandleException(this.Failure))
                    {
                        this.CompletedRequest();
                    }
                }
                catch (Exception exception)
                {
                    if (this.HandleFailure(exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    this.userCompleted = true;
                    this.SetAsyncWaitHandle();
                    if (!(((this.userCallback == null) || (this.Failure is ThreadAbortException)) || (this.Failure is StackOverflowException)))
                    {
                        this.userCallback(this);
                    }
                }
            }
        }

        internal bool HandleFailure(Exception e)
        {
            Interlocked.CompareExchange<Exception>(ref this.failure, e, null);
            this.SetCompleted();
            return Util.DoNotHandleException(e);
        }

        internal static IAsyncResult InvokeAsync(Func<AsyncCallback, object, IAsyncResult> asyncAction, AsyncCallback callback, object state)
        {
            return PostInvokeAsync(asyncAction.Invoke(GetDataServiceAsyncCallback(callback), state), callback);
        }

        internal static IAsyncResult InvokeAsync(Func<byte[], int, int, AsyncCallback, object, IAsyncResult> asyncAction, byte[] buffer, int offset, int length, AsyncCallback callback, object state)
        {
            return PostInvokeAsync(asyncAction(buffer, offset, length, GetDataServiceAsyncCallback(callback), state), callback);
        }

        private static IAsyncResult PostInvokeAsync(IAsyncResult asyncResult, AsyncCallback callback)
        {
            Debug.Assert(asyncResult != null, "asyncResult != null");
            if (asyncResult.CompletedSynchronously)
            {
                Debug.Assert(asyncResult.IsCompleted, "asyncResult.IsCompleted");
                callback(asyncResult);
            }
            return asyncResult;
        }

        internal void SetAborted()
        {
            Interlocked.Exchange(ref this.completed, 2);
        }

        private void SetAsyncWaitHandle()
        {
            if (null != this.asyncWait)
            {
                Interlocked.CompareExchange<object>(ref this.asyncWaitDisposeLock, new object(), null);
                lock (this.asyncWaitDisposeLock)
                {
                    if (!this.asyncWaitDisposed)
                    {
                        this.asyncWait.Set();
                    }
                }
            }
        }

        internal void SetCompleted()
        {
            Interlocked.CompareExchange(ref this.completed, 1, 0);
        }

        internal WebRequest Abortable
        {
            get
            {
                return this.abortable;
            }
            set
            {
                this.abortable = value;
                if ((value != null) && this.IsAborted)
                {
                    value.Abort();
                }
            }
        }

        public object AsyncState
        {
            get
            {
                return this.userState;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (null == this.asyncWait)
                {
                    Interlocked.CompareExchange<ManualResetEvent>(ref this.asyncWait, new ManualResetEvent(this.IsCompleted), null);
                    if (this.IsCompleted)
                    {
                        this.SetAsyncWaitHandle();
                    }
                }
                return this.asyncWait;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return this.completedSynchronously;
            }
            internal set
            {
                this.completedSynchronously = value;
            }
        }

        internal Exception Failure
        {
            get
            {
                return this.failure;
            }
        }

        internal bool IsAborted
        {
            get
            {
                return (2 == this.completed);
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this.userCompleted;
            }
        }

        internal bool IsCompletedInternally
        {
            get
            {
                return (0 != this.completed);
            }
        }

        internal delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    }
}

