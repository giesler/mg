namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    public class DataServiceCollection<T> : ObservableCollection<T>
    {
        private bool asyncOperationInProgress;
        private Func<EntityCollectionChangedParams, bool> collectionChangedCallback;
        private DataServiceQueryContinuation<T> continuation;
        private Func<EntityChangedParams, bool> entityChangedCallback;
        private string entitySetName;
        public EventHandler<LoadCompletedEventArgs> LoadCompleted;
        private BindingObserver observer;
        private bool rootCollection;
        private bool trackingOnLoad;

        //public event EventHandler<LoadCompletedEventArgs> LoadCompleted
        //{
        //    add
        //    {
        //        EventHandler<LoadCompletedEventArgs> handler2;
        //        EventHandler<LoadCompletedEventArgs> loadCompleted = this.LoadCompleted;
        //        do
        //        {
        //            handler2 = loadCompleted;
        //            EventHandler<LoadCompletedEventArgs> handler3 = (EventHandler<LoadCompletedEventArgs>) Delegate.Combine(handler2, value);
        //            loadCompleted = Interlocked.CompareExchange<EventHandler<LoadCompletedEventArgs>>(ref this.LoadCompleted, handler3, handler2);
        //        }
        //        while (loadCompleted != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<LoadCompletedEventArgs> handler2;
        //        EventHandler<LoadCompletedEventArgs> loadCompleted = this.LoadCompleted;
        //        do
        //        {
        //            handler2 = loadCompleted;
        //            EventHandler<LoadCompletedEventArgs> handler3 = (EventHandler<LoadCompletedEventArgs>) Delegate.Remove(handler2, value);
        //            loadCompleted = Interlocked.CompareExchange<EventHandler<LoadCompletedEventArgs>>(ref this.LoadCompleted, handler3, handler2);
        //        }
        //        while (loadCompleted != handler2);
        //    }
        //}

        public DataServiceCollection() : this(null, null, TrackingMode.AutoChangeTracking, null, null, null)
        {
        }

        public DataServiceCollection(IEnumerable<T> items) : this(null, items, TrackingMode.AutoChangeTracking, null, null, null)
        {
        }

        public DataServiceCollection(DataServiceContext context) : this(context, null, TrackingMode.AutoChangeTracking, null, null, null)
        {
        }

        public DataServiceCollection(IEnumerable<T> items, TrackingMode trackingMode) : this(null, items, trackingMode, null, null, null)
        {
        }

        public DataServiceCollection(DataServiceContext context, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) : this(context, null, TrackingMode.AutoChangeTracking, entitySetName, entityChangedCallback, collectionChangedCallback)
        {
        }

        public DataServiceCollection(IEnumerable<T> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) : this(null, items, trackingMode, entitySetName, entityChangedCallback, collectionChangedCallback)
        {
        }

        public DataServiceCollection(DataServiceContext context, IEnumerable<T> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback)
        {
            if (trackingMode == TrackingMode.AutoChangeTracking)
            {
                if (context == null)
                {
                    if (items == null)
                    {
                        this.trackingOnLoad = true;
                        this.entitySetName = entitySetName;
                        this.entityChangedCallback = entityChangedCallback;
                        this.collectionChangedCallback = collectionChangedCallback;
                    }
                    else
                    {
                        context = DataServiceCollection<T>.GetContextFromItems(items);
                    }
                }
                if (!this.trackingOnLoad)
                {
                    if (items != null)
                    {
                        DataServiceCollection<T>.ValidateIteratorParameter(items);
                    }
                    this.StartTracking(context, items, entitySetName, entityChangedCallback, collectionChangedCallback);
                }
            }
            else if (items != null)
            {
                this.Load(items);
            }
        }

        internal DataServiceCollection(object atomMaterializer, DataServiceContext context, IEnumerable<T> items, TrackingMode trackingMode, string entitySetName, Func<EntityChangedParams, bool> entityChangedCallback, Func<EntityCollectionChangedParams, bool> collectionChangedCallback) : this((context != null) ? context : ((AtomMaterializer) atomMaterializer).Context, items, trackingMode, entitySetName, entityChangedCallback, collectionChangedCallback)
        {
            Debug.Assert(atomMaterializer != null, "atomMaterializer != null");
            Debug.Assert(((AtomMaterializer) atomMaterializer).Context != null, "Context != null");
            if (items != null)
            {
                ((AtomMaterializer) atomMaterializer).PropagateContinuation<T>(items, this);
            }
        }

        public void Add(T item)
        {
            if (this.IsTracking)
            {
                INotifyPropertyChanged changed = item as INotifyPropertyChanged;
                if (changed == null)
                {
                    throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(item.GetType()));
                }
            }
            base.Add(item);
        }

        private void BeginLoadAsyncOperation(Func<AsyncCallback, IAsyncResult> beginCall, Func<IAsyncResult, QueryOperationResponse> endCall)
        {
            Debug.Assert(!this.asyncOperationInProgress, "Trying to start a new LoadAsync while another is still in progress. We should have thrown.");

            this.asyncOperationInProgress = true;
            try
            {
                IAsyncResult asyncResult = beginCall(
                    ar => System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            QueryOperationResponse result = endCall(ar);
                            this.asyncOperationInProgress = false;
                            if (this.LoadCompleted != null)
                            {
                                this.LoadCompleted(this, new LoadCompletedEventArgs(result, null));
                            }
                        }
                        catch (Exception ex)
                        {
                            this.asyncOperationInProgress = false;
                            if (this.LoadCompleted != null)
                            {
                                this.LoadCompleted(this, new LoadCompletedEventArgs(null, ex));
                            }
                        }
                    }));
            }
            catch (Exception)
            {
                this.asyncOperationInProgress = false;
                throw;
            }
        }

        public void Clear(bool stopTracking)
        {
            if (!this.IsTracking)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
            }
            if (!stopTracking)
            {
                base.Clear();
            }
            else
            {
                Debug.Assert(this.observer.Context != null, "Must have valid context when the collection is being observed.");
                try
                {
                    this.observer.DetachBehavior = true;
                    base.Clear();
                }
                finally
                {
                    this.observer.DetachBehavior = false;
                }
            }
        }

        public void Detach()
        {
            if (!this.IsTracking)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
            }
            if (!this.rootCollection)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_CannotStopTrackingChildCollection);
            }
            this.observer.StopTracking();
            this.observer = null;
            this.rootCollection = false;
        }

        private void FinishLoading()
        {
            if (this.IsTracking)
            {
                this.observer.AttachBehavior = false;
            }
        }

        private static DataServiceContext GetContextFromItems(IEnumerable<T> items)
        {
            DataServiceContext context;
            Debug.Assert(items != null, "items != null");
            DataServiceQuery<T> query = items as DataServiceQuery<T>;
            if (query != null)
            {
                DataServiceQueryProvider provider = query.Provider as DataServiceQueryProvider;
                Debug.Assert(provider != null, "Got DataServiceQuery with unknown query provider.");
                context = provider.Context;
                Debug.Assert(context != null, "Query provider must always have valid context.");
                return context;
            }
            QueryOperationResponse response = items as QueryOperationResponse;
            if (response == null)
            {
                throw new ArgumentException(Strings.DataServiceCollection_CannotDetermineContextFromItems);
            }
            Debug.Assert(response.Results != null, "Got QueryOperationResponse without valid results.");
            context = response.Results.Context;
            Debug.Assert(context != null, "Materializer must always have valid context.");
            return context;
        }

        protected override void InsertItem(int index, T item)
        {
            if (this.trackingOnLoad)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_InsertIntoTrackedButNotLoadedCollection);
            }
            base.InsertItem(index, item);
        }

        private void InternalLoadCollection(IEnumerable<T> items)
        {
            Debug.Assert(items != null, "items != null");
            Debug.Assert(!(items is DataServiceQuery), "SL Client using DSQ as items...should have been caught by ValidateIteratorParameter.");
            foreach (T local in items)
            {
                if (!base.Contains(local))
                {
                    this.Add(local);
                }
            }
            QueryOperationResponse<T> response = items as QueryOperationResponse<T>;
            if (response != null)
            {
                this.continuation = response.GetContinuation();
            }
            else
            {
                this.continuation = null;
            }
        }

        public void Load(IEnumerable<T> items)
        {
            DataServiceCollection<T>.ValidateIteratorParameter(items);
            if (this.trackingOnLoad)
            {
                DataServiceContext contextFromItems = DataServiceCollection<T>.GetContextFromItems(items);
                this.trackingOnLoad = false;
                this.StartTracking(contextFromItems, items, this.entitySetName, this.entityChangedCallback, this.collectionChangedCallback);
            }
            else
            {
                this.StartLoading();
                try
                {
                    this.InternalLoadCollection(items);
                }
                finally
                {
                    this.FinishLoading();
                }
            }
        }

        public void Load(T item)
        {
            if (item == null)
            {
                throw Error.ArgumentNull("item");
            }
            this.StartLoading();
            try
            {
                if (!base.Contains(item))
                {
                    this.Add(item);
                }
            }
            finally
            {
                this.FinishLoading();
            }
        }

        public void LoadAsync()
        {
            if (!this.IsTracking)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
            }

            object parent;
            string property;
            if (!this.observer.LookupParent(this, out parent, out property))
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_LoadAsyncNoParamsWithoutParentEntity);
            }

            if (this.asyncOperationInProgress)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime);
            }

            BeginLoadAsyncOperation(
                asyncCallback => this.observer.Context.BeginLoadProperty(parent, property, asyncCallback, null),
                asyncResult => (QueryOperationResponse)this.observer.Context.EndLoadProperty(asyncResult));
        }

        public void LoadAsync(IQueryable<T> query)
        {
            Util.CheckArgumentNull(query, "query");
            DataServiceQuery<T> dsq = query as DataServiceQuery<T>;
            if (dsq == null)
            {
                throw new ArgumentException(Strings.DataServiceCollection_LoadAsyncRequiresDataServiceQuery, "query");
            }

            if (this.asyncOperationInProgress)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime);
            }

            if (this.trackingOnLoad)
            {
                this.StartTracking(((DataServiceQueryProvider)dsq.Provider).Context,
                                   null,
                                   this.entitySetName,
                                   this.entityChangedCallback,
                                   this.collectionChangedCallback);
                this.trackingOnLoad = false;
            }

            BeginLoadAsyncOperation(
                asyncCallback => dsq.BeginExecute(asyncCallback, null),
                asyncResult =>
                {
                    QueryOperationResponse<T> response = (QueryOperationResponse<T>)dsq.EndExecute(asyncResult);
                    this.Load(response);
                    return response;
                });
        }

        public bool LoadNextPartialSetAsync()
        {
            if (!this.IsTracking)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_OperationForTrackedOnly);
            }

            if (this.asyncOperationInProgress)
            {
                throw new InvalidOperationException(Strings.DataServiceCollection_MultipleLoadAsyncOperationsAtTheSameTime);
            }

            if (this.Continuation == null)
            {
                if (this.LoadCompleted != null)
                {
                    this.LoadCompleted(this, new LoadCompletedEventArgs(null, null));
                }
                return false;
            }

            BeginLoadAsyncOperation(
                asyncCallback => this.observer.Context.BeginExecute(this.Continuation, asyncCallback, null),
                asyncResult =>
                {
                    QueryOperationResponse<T> response = (QueryOperationResponse<T>)this.observer.Context.EndExecute<T>(asyncResult);
                    this.Load(response);
                    return response;
                });

            return true;
        }

        private void StartLoading()
        {
            if (this.IsTracking)
            {
                if (this.observer.Context == null)
                {
                    throw new InvalidOperationException(Strings.DataServiceCollection_LoadRequiresTargetCollectionObserved);
                }
                this.observer.AttachBehavior = true;
            }
        }

        private void StartTracking(DataServiceContext context, IEnumerable<T> items, string entitySet, Func<EntityChangedParams, bool> entityChanged, Func<EntityCollectionChangedParams, bool> collectionChanged)
        {
            Debug.Assert(context != null, "Must have a valid context to initialize.");
            Debug.Assert(this.observer == null, "Must have no observer which implies Initialize should only be called once.");
            if (items != null)
            {
                this.InternalLoadCollection(items);
            }
            this.observer = new BindingObserver(context, entityChanged, collectionChanged);
            this.observer.StartTracking<T>(this, entitySet);
            this.rootCollection = true;
        }

        private static void ValidateIteratorParameter(IEnumerable<T> items)
        {
            Util.CheckArgumentNull<IEnumerable<T>>(items, "items");
            DataServiceQuery<T> query = items as DataServiceQuery<T>;
            if (query != null)
            {
                throw new ArgumentException(Strings.DataServiceCollection_DataServiceQueryCanNotBeEnumerated);
            }
        }

        public DataServiceQueryContinuation<T> Continuation
        {
            get
            {
                return this.continuation;
            }
            set
            {
                this.continuation = value;
            }
        }

        internal bool IsTracking
        {
            get
            {
                return (this.observer != null);
            }
        }

        internal BindingObserver Observer
        {
            get
            {
                return this.observer;
            }
            set
            {
                Debug.Assert(!this.rootCollection, "Must be a child collection to have the Observer setter called.");
                Debug.Assert(typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T)), "The entity type must be trackable (by implementing INotifyPropertyChanged interface)");
                this.observer = value;
            }
        }
    }
}

