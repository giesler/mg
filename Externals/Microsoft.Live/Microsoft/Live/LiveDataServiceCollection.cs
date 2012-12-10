namespace Microsoft.Live
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Services.Client;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Reflection;

    public class LiveDataServiceCollection<T> : DataServiceCollection<T>
    {
        public LiveResource Owner { get; set; }
        //[CompilerGenerated]
        //private LiveResource <Owner>k__BackingField;
        private object asyncState;
        private LiveDataContext context;
        private int isLoading;
        private IEnumerable<T> items;
        private SynchronizationContext syncContext;

        public event EventHandler<AsyncCompletedEventArgs> LoadCompleted;

        internal LiveDataServiceCollection(LiveResource parent) : base(null, TrackingMode.None)
        {
            this.Owner = parent;
        }

        public LiveDataServiceCollection(LiveDataContext context, IEnumerable<T> items) : base(context)
        {
            this.context = context;
            this.items = items;
        }

        private void CheckLoadInProgress()
        {
            if (Interlocked.CompareExchange(ref this.isLoading, 1, 0) == 1)
            {
                throw new InvalidOperationException(StringResource.LoadInProgress);
            }
        }

        public DataServiceQuery<T> GetQuery()
        {
            if ((this.items != null) && (this.items is DataServiceQuery<T>))
            {
                return (this.items as DataServiceQuery<T>);
            }
            return this.DataContext.CreateRelatedQuery<T>(this.Owner, this);
        }

       





        public void LoadAsync()
        {
            this.LoadAsync(null);
        }

        public void LoadAsync(object state)
        {
            this.LoadAsync(null, state);
        }

        public void LoadAsync(IEnumerable<T> items, object state)
        {
            if (items != null)
            {
                this.items = items;
            }
            DataServiceQuery<T> query = null;
            if (this.items != null)
            {
                query = this.items as DataServiceQuery<T>;
            }
            else
            {
                query = this.GetQuery();
            }
            if (query == null)
            {
                throw new InvalidOperationException("Collection is not associated with a query. Use Load() for loading in-memory collections.");
            }
            this.CheckLoadInProgress();
            this.asyncState = state;
            this.syncContext = SynchronizationContext.Current;
            query.BeginExecute(delegate (IAsyncResult result) {
                if (result.IsCompleted)
                {
                    Exception error = null;
                    try
                    {
                        QueryOperationResponse<T> enditems = query.EndExecute(result) as QueryOperationResponse<T>;
                        if (items != null)
                        {
                            Load(enditems);
                            Continuation = enditems.GetContinuation();
                        }
                    }
                    catch (Exception exception2)
                    {
                        error = exception2;
                    }
                    finally
                    {
                        OnLoadCompleted(error);
                    }
                }
            }, null);
        }

        public void LoadNextPartialSetAsync(object state)
        {
            this.CheckLoadInProgress();
            this.asyncState = state;
            this.syncContext = SynchronizationContext.Current;
            if (base.Continuation != null)
            {
                this.OnLoadCompleted(null);
            }
            this.DataContext.BeginExecute<T>(base.Continuation, delegate (IAsyncResult result) {
                if (result.IsCompleted)
                {
                    Exception error = null;
                    try
                    {
                        QueryOperationResponse<T> items = DataContext.EndExecute<T>(result) as QueryOperationResponse<T>;
                        if (items != null)
                        {
                            base.Load(items);
                            base.Continuation = items.GetContinuation();
                        }
                    }
                    catch (Exception exception2)
                    {
                        error = exception2;
                    }
                    finally
                    {
                        OnLoadCompleted(error);
                    }
                }
            }, null);
        }

        private void OnLoadCompleted(Exception error)
        {
            SendOrPostCallback d = null;
            WaitCallback callBack = null;
            Interlocked.Exchange(ref this.isLoading, 0);
            if (this.LoadCompleted != null)
            {
                if (this.syncContext != null)
                {
                    if (d == null)
                    {

                        d = delegate(object asyncState){LoadCompleted(this,new AsyncCompletedEventArgs(error, false, asyncState));};
                        //d = delegate {
                        //    base.<>4__this.LoadCompleted(base.<>4__this, new AsyncCompletedEventArgs(base.error, false, base.<>4__this.asyncState));
                        //};
                    }
                    this.syncContext.Post(d, null);
                }
                else
                {
                    if (callBack == null)
                    {
                        callBack = delegate(object asyncState){
                            LoadCompleted(this, new AsyncCompletedEventArgs(error, false, asyncState));
                        };
                        //callBack = delegate {
                        //    base.<>4__this.LoadCompleted(base.<>4__this, new AsyncCompletedEventArgs(base.error, false, base.<>4__this.asyncState));
                        //};
                    }
                    ThreadPool.QueueUserWorkItem(callBack);
                }
            }
        }

        internal LiveDataContext DataContext
        {
            get
            {
                if ((this.context == null) && (this.Owner != null))
                {
                    this.context = this.Owner.GetDataContext();
                }
                return this.context;
            }
        }

        //public LiveResource Owner
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Owner>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Owner>k__BackingField = value;
        //    }
        //}
    }
}

