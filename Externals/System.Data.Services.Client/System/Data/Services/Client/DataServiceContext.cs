namespace System.Data.Services.Client
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Services.Common;
    using System.Data.Services.Http;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using System.Xml.Linq;

    public class DataServiceContext
    {
        private bool applyingChanges;
        private Uri baseUri;
        private Uri baseUriWithSlash;
        private Dictionary<LinkDescriptor, LinkDescriptor> bindings = new Dictionary<LinkDescriptor, LinkDescriptor>(LinkDescriptor.EquivalenceComparer);
        internal EventHandler<SaveChangesEventArgs> ChangesSaved;
        private string dataNamespace;
        private Dictionary<object, EntityDescriptor> entityDescriptors = new Dictionary<object, EntityDescriptor>(EqualityComparer<object>.Default);
        private System.Data.Services.Client.HttpStack httpStack;
        private Dictionary<string, EntityDescriptor> identityToDescriptor;
        private bool ignoreMissingProperties;
        private bool ignoreResourceNotFoundException;
        private System.Data.Services.Client.MergeOption mergeOption;
        private static readonly string NewLine = Environment.NewLine;
        private uint nextChange;
        private bool postTunneling;
        public EventHandler<ReadingWritingEntityEventArgs> ReadingEntity;
        private Func<Type, string> resolveName;
        private Func<string, Type> resolveType;
        private Func<string, Uri> resolveSet;
 
        private SaveChangesOptions saveChangesDefaultOptions;
        public EventHandler<SendingRequestEventArgs> SendingRequest;
        private Uri typeScheme;
        public EventHandler<ReadingWritingEntityEventArgs> WritingEntity;

        //internal event EventHandler<SaveChangesEventArgs> ChangesSaved
        //{
        //    add
        //    {
        //        EventHandler<SaveChangesEventArgs> handler2;
        //        EventHandler<SaveChangesEventArgs> changesSaved = this.ChangesSaved;
        //        do
        //        {
        //            handler2 = changesSaved;
        //            EventHandler<SaveChangesEventArgs> handler3 = (EventHandler<SaveChangesEventArgs>) Delegate.Combine(handler2, value);
        //            changesSaved = Interlocked.CompareExchange<EventHandler<SaveChangesEventArgs>>(ref this.ChangesSaved, handler3, handler2);
        //        }
        //        while (changesSaved != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<SaveChangesEventArgs> handler2;
        //        EventHandler<SaveChangesEventArgs> changesSaved = this.ChangesSaved;
        //        do
        //        {
        //            handler2 = changesSaved;
        //            EventHandler<SaveChangesEventArgs> handler3 = (EventHandler<SaveChangesEventArgs>) Delegate.Remove(handler2, value);
        //            changesSaved = Interlocked.CompareExchange<EventHandler<SaveChangesEventArgs>>(ref this.ChangesSaved, handler3, handler2);
        //        }
        //        while (changesSaved != handler2);
        //    }
        //}

        //public event EventHandler<ReadingWritingEntityEventArgs> ReadingEntity
        //{
        //    add
        //    {
        //        EventHandler<ReadingWritingEntityEventArgs> handler2;
        //        EventHandler<ReadingWritingEntityEventArgs> readingEntity = this.ReadingEntity;
        //        do
        //        {
        //            handler2 = readingEntity;
        //            EventHandler<ReadingWritingEntityEventArgs> handler3 = (EventHandler<ReadingWritingEntityEventArgs>) Delegate.Combine(handler2, value);
        //            readingEntity = Interlocked.CompareExchange<EventHandler<ReadingWritingEntityEventArgs>>(ref this.ReadingEntity, handler3, handler2);
        //        }
        //        while (readingEntity != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<ReadingWritingEntityEventArgs> handler2;
        //        EventHandler<ReadingWritingEntityEventArgs> readingEntity = this.ReadingEntity;
        //        do
        //        {
        //            handler2 = readingEntity;
        //            EventHandler<ReadingWritingEntityEventArgs> handler3 = (EventHandler<ReadingWritingEntityEventArgs>) Delegate.Remove(handler2, value);
        //            readingEntity = Interlocked.CompareExchange<EventHandler<ReadingWritingEntityEventArgs>>(ref this.ReadingEntity, handler3, handler2);
        //        }
        //        while (readingEntity != handler2);
        //    }
        //}

        //public event EventHandler<SendingRequestEventArgs> SendingRequest
        //{
        //    add
        //    {
        //        EventHandler<SendingRequestEventArgs> handler2;
        //        EventHandler<SendingRequestEventArgs> sendingRequest = this.SendingRequest;
        //        do
        //        {
        //            handler2 = sendingRequest;
        //            EventHandler<SendingRequestEventArgs> handler3 = (EventHandler<SendingRequestEventArgs>) Delegate.Combine(handler2, value);
        //            sendingRequest = Interlocked.CompareExchange<EventHandler<SendingRequestEventArgs>>(ref this.SendingRequest, handler3, handler2);
        //        }
        //        while (sendingRequest != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<SendingRequestEventArgs> handler2;
        //        EventHandler<SendingRequestEventArgs> sendingRequest = this.SendingRequest;
        //        do
        //        {
        //            handler2 = sendingRequest;
        //            EventHandler<SendingRequestEventArgs> handler3 = (EventHandler<SendingRequestEventArgs>) Delegate.Remove(handler2, value);
        //            sendingRequest = Interlocked.CompareExchange<EventHandler<SendingRequestEventArgs>>(ref this.SendingRequest, handler3, handler2);
        //        }
        //        while (sendingRequest != handler2);
        //    }
        //}

        //public event EventHandler<ReadingWritingEntityEventArgs> WritingEntity
        //{
        //    add
        //    {
        //        EventHandler<ReadingWritingEntityEventArgs> handler2;
        //        EventHandler<ReadingWritingEntityEventArgs> writingEntity = this.WritingEntity;
        //        do
        //        {
        //            handler2 = writingEntity;
        //            EventHandler<ReadingWritingEntityEventArgs> handler3 = (EventHandler<ReadingWritingEntityEventArgs>) Delegate.Combine(handler2, value);
        //            writingEntity = Interlocked.CompareExchange<EventHandler<ReadingWritingEntityEventArgs>>(ref this.WritingEntity, handler3, handler2);
        //        }
        //        while (writingEntity != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<ReadingWritingEntityEventArgs> handler2;
        //        EventHandler<ReadingWritingEntityEventArgs> writingEntity = this.WritingEntity;
        //        do
        //        {
        //            handler2 = writingEntity;
        //            EventHandler<ReadingWritingEntityEventArgs> handler3 = (EventHandler<ReadingWritingEntityEventArgs>) Delegate.Remove(handler2, value);
        //            writingEntity = Interlocked.CompareExchange<EventHandler<ReadingWritingEntityEventArgs>>(ref this.WritingEntity, handler3, handler2);
        //        }
        //        while (writingEntity != handler2);
        //    }
        //}

        public DataServiceContext(Uri serviceRoot)
        {
            Util.CheckArgumentNull<Uri>(serviceRoot, "serviceRoot");
            if (!serviceRoot.IsAbsoluteUri)
            {
                System.Net.WebClient client = new System.Net.WebClient();
                serviceRoot = new Uri(new Uri(client.BaseAddress), serviceRoot);
            }
            if (((!serviceRoot.IsAbsoluteUri || !Uri.IsWellFormedUriString(serviceRoot.OriginalString, UriKind.Absolute)) || (!string.IsNullOrEmpty(serviceRoot.Query) || !string.IsNullOrEmpty(serviceRoot.Fragment))) || ((serviceRoot.Scheme != "http") && (serviceRoot.Scheme != "https")))
            {
                throw Error.Argument(Strings.Context_BaseUri, "serviceRoot");
            }
            this.BaseUri = serviceRoot;
            this.mergeOption = System.Data.Services.Client.MergeOption.AppendOnly;
            this.DataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            this.UsePostTunneling = true;
            this.typeScheme = new Uri("http://schemas.microsoft.com/ado/2007/08/dataservices/scheme");
            this.httpStack = System.Data.Services.Client.HttpStack.Auto;
        }

        public void AddLink(object source, string sourceProperty, object target)
        {
            this.EnsureRelatable(source, sourceProperty, target, EntityStates.Added);
            LinkDescriptor key = new LinkDescriptor(source, sourceProperty, target);
            if (this.bindings.ContainsKey(key))
            {
                throw Error.InvalidOperation(Strings.Context_RelationAlreadyContained);
            }
            key.State = EntityStates.Added;
            this.bindings.Add(key, key);
            this.IncrementChange(key);
        }

        public void AddObject(string entitySetName, object entity)
        {
            this.ValidateEntitySetName(ref entitySetName);
            ValidateEntityType(entity);
            Uri editLink = this.ResolveEntitySetName(entitySetName);
            EntityDescriptor descriptor = new EntityDescriptor(null, null, editLink, entity, null, null, entitySetName, null, EntityStates.Added);
            try
            {
                this.entityDescriptors.Add(entity, descriptor);
            }
            catch (ArgumentException)
            {
                throw Error.InvalidOperation(Strings.Context_EntityAlreadyContained);
            }
            this.IncrementChange(descriptor);
            this.WireUpRelationships(entity);

            //this.ValidateEntitySetName(ref entitySetName);
            //ValidateEntityType(entity);
            //EntityDescriptor descriptor = new EntityDescriptor(null, null, null, entity, null, null, entitySetName, null, EntityStates.Added);
            //try
            //{
            //    this.entityDescriptors.Add(entity, descriptor);
            //}
            //catch (ArgumentException)
            //{
            //    throw Error.InvalidOperation(Strings.Context_EntityAlreadyContained);
            //}
            //this.IncrementChange(descriptor);
        }

        private void WireUpRelationships(object entity)
        {
            IQueryable<ClientType.ClientProperty> queryable = Queryable.AsQueryable<ClientType.ClientProperty>(ClientType.Create(entity.GetType()).Properties);
            foreach (ClientType.ClientProperty property in queryable)
            {
                if ((null != property.CollectionType) && typeof(DataServiceCollection<>).MakeGenericType(new Type[] { property.CollectionType }).IsAssignableFrom(property.PropertyType))
                {
                    ClientType.Create(property.PropertyType).GetProperty("RelationshipInformation", false).SetValue(property.GetValue(entity), new RelationshipInformation(this, entity, property.PropertyName), "RelationshipInformation", false);
                }
            }
        }

 

 


        public void AddRelatedObject(object source, string sourceProperty, object target)
        {
            Util.CheckArgumentNull<object>(source, "source");
            Util.CheckArgumentNotEmpty(sourceProperty, "propertyName");
            Util.CheckArgumentNull<object>(target, "target");
            ValidateEntityType(source);
            EntityDescriptor parentEntity = this.EnsureContained(source, "source");
            if (parentEntity.State == EntityStates.Deleted)
            {
                throw Error.InvalidOperation(Strings.Context_AddRelatedObjectSourceDeleted);
            }
            ClientType.ClientProperty property = ClientType.Create(source.GetType()).GetProperty(sourceProperty, false);
            if (property.IsKnownType || (property.CollectionType == null))
            {
                throw Error.InvalidOperation(Strings.Context_AddRelatedObjectCollectionOnly);
            }
            ClientType type2 = ClientType.Create(target.GetType());
            ValidateEntityType(target);
            if (!ClientType.Create(property.CollectionType).ElementType.IsAssignableFrom(type2.ElementType))
            {
                throw Error.Argument(Strings.Context_RelationNotRefOrCollection, "target");
            }
            EntityDescriptor descriptor2 = new EntityDescriptor(null, null, null, target, parentEntity, sourceProperty, null, null, EntityStates.Added);
            try
            {
                this.entityDescriptors.Add(target, descriptor2);
            }
            catch (ArgumentException)
            {
                throw Error.InvalidOperation(Strings.Context_EntityAlreadyContained);
            }
            LinkDescriptor relatedEnd = descriptor2.GetRelatedEnd();
            relatedEnd.State = EntityStates.Added;
            this.bindings.Add(relatedEnd, relatedEnd);
            this.IncrementChange(descriptor2);
        }

        internal void AttachIdentity(string identity, Uri selfLink, Uri editLink, object entity, string etag)
        {
            Debug.Assert(null != identity, "must have identity");
            this.EnsureIdentityToResource();
            EntityDescriptor resource = this.entityDescriptors[entity];
            this.DetachResourceIdentity(resource);
            if (resource.IsDeepInsert)
            {
                LinkDescriptor descriptor2 = this.bindings[resource.GetRelatedEnd()];
                descriptor2.State = EntityStates.Unchanged;
            }
            resource.ETag = etag;
            resource.Identity = identity;
            resource.SelfLink = selfLink;
            resource.EditLink = editLink;
            resource.State = EntityStates.Unchanged;
            this.identityToDescriptor[identity] = resource;
        }

        public void AttachLink(object source, string sourceProperty, object target)
        {
            this.AttachLink(source, sourceProperty, target, System.Data.Services.Client.MergeOption.NoTracking);
        }

        internal void AttachLink(object source, string sourceProperty, object target, System.Data.Services.Client.MergeOption linkMerge)
        {
            this.EnsureRelatable(source, sourceProperty, target, EntityStates.Unchanged);
            LinkDescriptor descriptor = null;
            LinkDescriptor key = new LinkDescriptor(source, sourceProperty, target);
            if (this.bindings.TryGetValue(key, out descriptor))
            {
                switch (linkMerge)
                {
                    case System.Data.Services.Client.MergeOption.AppendOnly:
                        goto Label_010B;

                    case System.Data.Services.Client.MergeOption.OverwriteChanges:
                        key = descriptor;
                        goto Label_010B;

                    case System.Data.Services.Client.MergeOption.PreserveChanges:
                        if (((EntityStates.Added == descriptor.State) || (EntityStates.Unchanged == descriptor.State)) || ((EntityStates.Modified == descriptor.State) && (null != descriptor.Target)))
                        {
                            key = descriptor;
                        }
                        goto Label_010B;

                    case System.Data.Services.Client.MergeOption.NoTracking:
                        throw Error.InvalidOperation(Strings.Context_RelationAlreadyContained);
                }
            }
            else if ((null != ClientType.Create(source.GetType()).GetProperty(sourceProperty, false).CollectionType) || (null == (descriptor = this.DetachReferenceLink(source, sourceProperty, target, linkMerge))))
            {
                this.bindings.Add(key, key);
                this.IncrementChange(key);
            }
            else if ((linkMerge != System.Data.Services.Client.MergeOption.AppendOnly) && ((System.Data.Services.Client.MergeOption.PreserveChanges != linkMerge) || (EntityStates.Modified != descriptor.State)))
            {
                key = descriptor;
            }
        Label_010B:
            key.State = EntityStates.Unchanged;
        }

        internal void AttachLocation(object entity, string location)
        {
            Debug.Assert(null != entity, "null != entity");
            Uri uri = new Uri(location, UriKind.Absolute);
            string str = Util.ReferenceIdentity(uri.ToString());
            this.EnsureIdentityToResource();
            EntityDescriptor resource = this.entityDescriptors[entity];
            this.DetachResourceIdentity(resource);
            if (resource.IsDeepInsert)
            {
                LinkDescriptor descriptor2 = this.bindings[resource.GetRelatedEnd()];
                descriptor2.State = EntityStates.Unchanged;
            }
            resource.Identity = str;
            resource.EditLink = uri;
            this.identityToDescriptor[str] = resource;
        }

        public void AttachTo(string entitySetName, object entity)
        {
            this.AttachTo(entitySetName, entity, null);
        }

        public void AttachTo(string entitySetName, object entity, string etag)
        {
            this.ValidateEntitySetName(ref entitySetName);
            Uri editLink = GenerateEditLinkUri(this.ResolveEntitySetName(entitySetName), entity);
            EntityDescriptor descriptor = new EntityDescriptor(Util.ReferenceIdentity(editLink.ToString()), null, editLink, entity, null, null, null, etag, EntityStates.Unchanged);
            this.InternalAttachEntityDescriptor(descriptor, true);
            this.WireUpRelationships(entity);

            //this.ValidateEntitySetName(ref entitySetName);
            //Uri editLink = GenerateEditLinkUri(this.baseUriWithSlash, entitySetName, entity);
            //EntityDescriptor descriptor = new EntityDescriptor(Util.ReferenceIdentity(editLink.ToString()), null, editLink, entity, null, null, null, etag, EntityStates.Unchanged);
            //this.InternalAttachEntityDescriptor(descriptor, true);
        }

        public IAsyncResult BeginExecute<T>(DataServiceQueryContinuation<T> continuation, AsyncCallback callback, object state)
        {
            Util.CheckArgumentNull<DataServiceQueryContinuation<T>>(continuation, "continuation");
            return new DataServiceRequest<T>(continuation.CreateQueryComponents(), continuation.Plan).BeginExecute(this, this, callback, state);
        }

        public IAsyncResult BeginExecute<TElement>(Uri requestUri, AsyncCallback callback, object state)
        {
            requestUri = Util.CreateUri(this.baseUriWithSlash, requestUri);
            QueryComponents queryComponents = new QueryComponents(requestUri, Util.DataServiceVersionEmpty, typeof(TElement), null, null);
            return new DataServiceRequest<TElement>(queryComponents, null).BeginExecute(this, this, callback, state);
        }

        public IAsyncResult BeginExecuteBatch(AsyncCallback callback, object state, params DataServiceRequest[] queries)
        {
            Util.CheckArgumentNotEmpty<DataServiceRequest>(queries, "queries");
            SaveResult result = new SaveResult(this, "ExecuteBatch", queries, SaveChangesOptions.Batch, callback, state, true);
            result.BatchBeginRequest(false);
            return result;
        }

        public IAsyncResult BeginGetReadStream(object entity, DataServiceRequestArgs args, AsyncCallback callback, object state)
        {
            GetReadStreamResult result = this.CreateGetReadStreamResult(entity, args, callback, state);
            result.Begin();
            return result;
        }

        public IAsyncResult BeginLoadProperty(object entity, string propertyName, AsyncCallback callback, object state)
        {
            return this.BeginLoadProperty(entity, propertyName, (Uri) null, callback, state);
        }

        public IAsyncResult BeginLoadProperty(object entity, string propertyName, DataServiceQueryContinuation continuation, AsyncCallback callback, object state)
        {
            Util.CheckArgumentNull<DataServiceQueryContinuation>(continuation, "continuation");
            LoadPropertyResult result = this.CreateLoadPropertyRequest(entity, propertyName, callback, state, null, continuation);
            result.BeginExecute();
            return result;
        }

        public IAsyncResult BeginLoadProperty(object entity, string propertyName, Uri nextLinkUri, AsyncCallback callback, object state)
        {
            LoadPropertyResult result = this.CreateLoadPropertyRequest(entity, propertyName, callback, state, nextLinkUri, null);
            result.BeginExecute();
            return result;
        }

        public IAsyncResult BeginSaveChanges(AsyncCallback callback, object state)
        {
            return this.BeginSaveChanges(this.SaveChangesDefaultOptions, callback, state);
        }

        public IAsyncResult BeginSaveChanges(SaveChangesOptions options, AsyncCallback callback, object state)
        {
            ValidateSaveChangesOptions(options);
            SaveResult result = new SaveResult(this, "SaveChanges", null, options, callback, state, true);
            bool replaceOnUpdate = IsFlagSet(options, SaveChangesOptions.ReplaceOnUpdate);
            if (IsFlagSet(options, SaveChangesOptions.Batch))
            {
                result.BatchBeginRequest(replaceOnUpdate);
                return result;
            }
            result.BeginNextChange(replaceOnUpdate);
            return result;
        }

        public void CancelRequest(IAsyncResult asyncResult)
        {
            Util.CheckArgumentNull<IAsyncResult>(asyncResult, "asyncResult");
            BaseAsyncResult result = asyncResult as BaseAsyncResult;
            if ((result == null) || (this != result.Source))
            {
                object context = null;
                DataServiceQuery source = null;
                if (null != result)
                {
                    source = result.Source as DataServiceQuery;
                    if (null != source)
                    {
                        DataServiceQueryProvider provider = source.Provider as DataServiceQueryProvider;
                        if (null != provider)
                        {
                            context = provider.Context;
                        }
                    }
                }
                if (this != context)
                {
                    throw Error.Argument(Strings.Context_DidNotOriginateAsync, "asyncResult");
                }
            }
            if (!result.IsCompletedInternally)
            {
                result.SetAborted();
                WebRequest abortable = result.Abortable;
                if (null != abortable)
                {
                    abortable.Abort();
                }
            }
        }

        private static bool CanHandleResponseVersion(string responseVersion)
        {
            if (!string.IsNullOrEmpty(responseVersion))
            {
                KeyValuePair<Version, string> pair;
                if (!HttpProcessUtility.TryReadVersion(responseVersion, out pair))
                {
                    return false;
                }
                if (!Util.SupportedResponseVersions.Contains<Version>(pair.Key))
                {
                    return false;
                }
            }
            return true;
        }

        private GetReadStreamResult CreateGetReadStreamResult(object entity, DataServiceRequestArgs args, AsyncCallback callback, object state)
        {
            EntityDescriptor descriptor = this.EnsureContained(entity, "entity");
            Util.CheckArgumentNull<DataServiceRequestArgs>(args, "args");
            Uri mediaResourceUri = descriptor.GetMediaResourceUri(this.baseUriWithSlash);
            if (mediaResourceUri == null)
            {
                throw new ArgumentException(Strings.Context_EntityNotMediaLinkEntry, "entity");
            }
            HttpWebRequest request = this.CreateRequest(mediaResourceUri, "GET", true, null, null, false, System.Data.Services.Client.HttpStack.ClientHttp);
            WebUtil.ApplyHeadersToRequest(args.Headers, request, false);
            return new GetReadStreamResult(this, "GetReadStream", request, callback, state);
        }

        private LoadPropertyResult CreateLoadPropertyRequest(object entity, string propertyName, AsyncCallback callback, object state, Uri requestUri, DataServiceQueryContinuation continuation)
        {
            ProjectionPlan plan;
            Version dataServiceVersionEmpty;
            Debug.Assert((continuation == null) || (requestUri == null), "continuation == null || requestUri == null -- only one or the either (or neither) may be passed in");
            EntityDescriptor descriptor = this.EnsureContained(entity, "entity");
            Util.CheckArgumentNotEmpty(propertyName, "propertyName");
            ClientType type = ClientType.Create(entity.GetType());
            Debug.Assert(type.IsEntityType, "must be entity type to be contained");
            if (EntityStates.Added == descriptor.State)
            {
                throw Error.InvalidOperation(Strings.Context_NoLoadWithInsertEnd);
            }
            ClientType.ClientProperty property = type.GetProperty(propertyName, false);
            Debug.Assert(null != property, "should have thrown if propertyName didn't exist");
            if (continuation == null)
            {
                plan = null;
            }
            else
            {
                plan = continuation.Plan;
                requestUri = continuation.NextLinkUri;
            }
            bool allowAnyType = (type.MediaDataMember != null) && (propertyName == type.MediaDataMember.PropertyName);
            if (requestUri == null)
            {
                Uri uri;
                if (allowAnyType)
                {
                    uri = Util.CreateUri("$value", UriKind.Relative);
                }
                else
                {
                    uri = Util.CreateUri(propertyName + ((property.CollectionType != null) ? "()" : string.Empty), UriKind.Relative);
                }
                requestUri = Util.CreateUri(descriptor.GetResourceUri(this.baseUriWithSlash, true), uri);
                dataServiceVersionEmpty = Util.DataServiceVersion1;
            }
            else
            {
                dataServiceVersionEmpty = Util.DataServiceVersionEmpty;
            }
            HttpWebRequest request = this.CreateRequest(requestUri, "GET", allowAnyType, null, dataServiceVersionEmpty, false);
            return new LoadPropertyResult(entity, propertyName, this, request, callback, state, DataServiceRequest.GetInstance(property.PropertyType, requestUri), plan);
        }

        public DataServiceQuery<T> CreateQuery<T>(string entitySetName)
        {
            Util.CheckArgumentNotEmpty(entitySetName, "entitySetName");
            this.ValidateEntitySetName(ref entitySetName);
            return new DataServiceQuery<T>.DataServiceOrderedQuery(new ResourceSetExpression(typeof(IOrderedQueryable<T>), null, Expression.Constant(entitySetName), typeof(T), null, CountOption.None, null, null), new DataServiceQueryProvider(this));
        }

        private HttpWebRequest CreateRequest(LinkDescriptor binding)
        {
            Debug.Assert(null != binding, "null binding");
            if (binding.ContentGeneratedForSave)
            {
                return null;
            }
            EntityDescriptor sourceResource = this.entityDescriptors[binding.Source];
            EntityDescriptor descriptor2 = (binding.Target != null) ? this.entityDescriptors[binding.Target] : null;
            if (null == sourceResource.Identity)
            {
                Debug.Assert(!binding.ContentGeneratedForSave, "already saved link");
                binding.ContentGeneratedForSave = true;
                Debug.Assert(EntityStates.Added == sourceResource.State, "expected added state");
                throw Error.InvalidOperation(Strings.Context_LinkResourceInsertFailure, sourceResource.SaveError);
            }
            if ((descriptor2 != null) && (null == descriptor2.Identity))
            {
                Debug.Assert(!binding.ContentGeneratedForSave, "already saved link");
                binding.ContentGeneratedForSave = true;
                Debug.Assert(EntityStates.Added == descriptor2.State, "expected added state");
                throw Error.InvalidOperation(Strings.Context_LinkResourceInsertFailure, descriptor2.SaveError);
            }
            Debug.Assert(null != sourceResource.Identity, "missing sourceResource.Identity");
            return this.CreateRequest(this.CreateRequestUri(sourceResource, binding), GetLinkHttpMethod(binding), false, "application/xml", Util.DataServiceVersion1, false);
        }

        private HttpWebRequest CreateRequest(EntityDescriptor box, EntityStates state, bool replaceOnUpdate)
        {
            Debug.Assert((box != null) && (((EntityStates.Added == state) || (EntityStates.Modified == state)) || (EntityStates.Deleted == state)), "unexpected entity ResourceState");
            string entityHttpMethod = GetEntityHttpMethod(state, replaceOnUpdate);
            Uri resourceUri = box.GetResourceUri(this.baseUriWithSlash, false);
            Version requestVersion = ClientType.Create(box.Entity.GetType()).EpmIsV1Compatible ? Util.DataServiceVersion1 : Util.DataServiceVersion2;
            HttpWebRequest request = this.CreateRequest(resourceUri, entityHttpMethod, false, "application/atom+xml", requestVersion, false);
            if ((box.ETag != null) && ((EntityStates.Deleted == state) || (EntityStates.Modified == state)))
            {
                request.Headers.Set(HttpRequestHeader.IfMatch, box.ETag);
            }
            return request;
        }

        internal HttpWebRequest CreateRequest(Uri requestUri, string method, bool allowAnyType, string contentType, Version requestVersion, bool sendChunked)
        {
            return this.CreateRequest(requestUri, method, allowAnyType, contentType, requestVersion, sendChunked, System.Data.Services.Client.HttpStack.Auto);
        }

        internal HttpWebRequest CreateRequest(Uri requestUri, string method, bool allowAnyType, string contentType, Version requestVersion, bool sendChunked, System.Data.Services.Client.HttpStack httpStackArg)
        {
            Debug.Assert(null != requestUri, "request uri is null");
            Debug.Assert(requestUri.IsAbsoluteUri, "request uri is not absolute uri");
            Debug.Assert(requestUri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) || requestUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase), "request uri is not for HTTP");
            Debug.Assert(((object.ReferenceEquals("DELETE", method) || object.ReferenceEquals("GET", method)) || (object.ReferenceEquals("POST", method) || object.ReferenceEquals("PUT", method))) || object.ReferenceEquals("MERGE", method), "unexpected http method string reference");
            if (httpStackArg == System.Data.Services.Client.HttpStack.Auto)
            {
                httpStackArg = this.httpStack;
            }
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri, httpStackArg);
            if (!((!this.UsePostTunneling || object.ReferenceEquals("POST", method)) || object.ReferenceEquals("GET", method)))
            {
                request.Headers["X-HTTP-Method"] = method;
                request.Method = "POST";
            }
            else
            {
                request.Method = method;
            }
            if ((requestVersion != null) && (requestVersion.Major > 0))
            {
                request.Headers["DataServiceVersion"] = requestVersion.ToString() + ";NetFx";
            }
            request.Headers["MaxDataServiceVersion"] = Util.MaxResponseVersion.ToString() + ";NetFx";
            if (this.SendingRequest != null)
            {
                System.Net.WebHeaderCollection requestHeaders = request.CreateEmptyWebHeaderCollection();
                SendingRequestEventArgs e = new SendingRequestEventArgs(null, requestHeaders);
                this.SendingRequest(this, e);
                foreach (string str in requestHeaders.AllKeys)
                {
                    request.Headers[str] = requestHeaders[str];
                }
            }
            request.Accept = allowAnyType ? "*/*" : "application/atom+xml,application/xml";
            request.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
            if (!object.ReferenceEquals("GET", method))
            {
                Debug.Assert(!string.IsNullOrEmpty(contentType), "Content-Type must be specified for non get operation");
                request.ContentType = contentType;
                if (object.ReferenceEquals("DELETE", method))
                {
                    request.ContentLength = 0L;
                }
                if (!(!this.UsePostTunneling || object.ReferenceEquals("POST", method)))
                {
                    request.Headers["X-HTTP-Method"] = method;
                    method = "POST";
                }
            }
            else
            {
                Debug.Assert(contentType == null, "Content-Type for get methods should be null");
            }
            ICollection<string> allKeys = request.Headers.AllKeys;
            request.Method = method;
            return request;
        }

        private void CreateRequestBatch(LinkDescriptor binding, StreamWriter text)
        {
            string absoluteUri;
            EntityDescriptor sourceResource = this.entityDescriptors[binding.Source];
            if (null != sourceResource.Identity)
            {
                absoluteUri = this.CreateRequestUri(sourceResource, binding).AbsoluteUri;
            }
            else
            {
                Uri uri = this.CreateRequestRelativeUri(binding);
                absoluteUri = "$" + sourceResource.ChangeOrder.ToString(CultureInfo.InvariantCulture) + "/" + uri.OriginalString;
            }
            WriteOperationRequestHeaders(text, GetLinkHttpMethod(binding), absoluteUri, Util.DataServiceVersion1);
            text.WriteLine("{0}: {1}", "Content-ID", binding.ChangeOrder);
            if ((EntityStates.Added == binding.State) || ((EntityStates.Modified == binding.State) && (null != binding.Target)))
            {
                text.WriteLine("{0}: {1}", "Content-Type", "application/xml");
            }

            //string absoluteUri;
            //EntityDescriptor sourceResource = this.entityDescriptors[binding.Source];
            //if (null != sourceResource.Identity)
            //{
            //    absoluteUri = this.CreateRequestUri(sourceResource, binding).AbsoluteUri;
            //}
            //else
            //{
            //    Uri uri = this.CreateRequestRelativeUri(binding);
            //    absoluteUri = "$" + sourceResource.ChangeOrder.ToString(CultureInfo.InvariantCulture) + "/" + uri.OriginalString;
            //}
            //WriteOperationRequestHeaders(text, GetLinkHttpMethod(binding), absoluteUri, Util.DataServiceVersion1);
            //text.WriteLine("{0}: {1}", "Content-ID", binding.ChangeOrder);
            //if ((EntityStates.Added == binding.State) || ((EntityStates.Modified == binding.State) && (null != binding.Target)))
            //{
            //    text.WriteLine("{0}: {1}", "Content-Type", "application/xml");
            //}
        }

        private void CreateRequestBatch(EntityDescriptor box, StreamWriter text, bool replaceOnUpdate)
        {
            Debug.Assert(null != box, "null box");
            Debug.Assert(null != text, "null text");
            Debug.Assert(((box.State == EntityStates.Added) || (box.State == EntityStates.Deleted)) || (box.State == EntityStates.Modified), "the entity must be in one of the 3 possible states");
            Uri resourceUri = box.GetResourceUri(this.baseUriWithSlash, false);
            Debug.Assert(null != resourceUri, "request uri is null");
            Debug.Assert(resourceUri.IsAbsoluteUri, "request uri is not absolute uri");
            Version requestVersion = ClientType.Create(box.Entity.GetType()).EpmIsV1Compatible ? Util.DataServiceVersion1 : Util.DataServiceVersion2;
            WriteOperationRequestHeaders(text, GetEntityHttpMethod(box.State, replaceOnUpdate), resourceUri.AbsoluteUri, requestVersion);
            text.WriteLine("{0}: {1}", "Content-ID", box.ChangeOrder);
            if (EntityStates.Deleted != box.State)
            {
                text.WriteLine("{0}: {1}", "Content-Type", "application/atom+xml;type=entry");
            }
            if ((box.ETag != null) && ((EntityStates.Deleted == box.State) || (EntityStates.Modified == box.State)))
            {
                text.WriteLine("{0}: {1}", "If-Match", box.ETag);
            }
        }

        private MemoryStream CreateRequestData(EntityDescriptor box, bool newline)
        {
            Debug.Assert(null != box, "null box");
            MemoryStream stream = null;
            EntityStates state = box.State;
            if (state != EntityStates.Added)
            {
                if (state == EntityStates.Deleted)
                {
                    goto Label_0042;
                }
                if (state != EntityStates.Modified)
                {
                    Error.ThrowInternalError(InternalError.UnvalidatedEntityState);
                    goto Label_0042;
                }
            }
            stream = new MemoryStream();
        Label_0042:
            if (null != stream)
            {
                XmlWriter writer;
                bool flag;
                XDocument document = null;
                if (this.WritingEntity != null)
                {
                    document = new XDocument();
                    writer = document.CreateWriter();
                }
                else
                {
                    writer = XmlUtil.CreateXmlWriterAndWriteProcessingInstruction(stream, HttpProcessUtility.EncodingUtf8NoPreamble);
                }
                ClientType type = ClientType.Create(box.Entity.GetType());
                string serverTypeName = this.GetServerTypeName(box);
                writer.WriteStartElement("entry", "http://www.w3.org/2005/Atom");
                writer.WriteAttributeString("d", "http://www.w3.org/2000/xmlns/", this.DataNamespace);
                writer.WriteAttributeString("m", "http://www.w3.org/2000/xmlns/", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
                if (!string.IsNullOrEmpty(serverTypeName))
                {
                    writer.WriteStartElement("category", "http://www.w3.org/2005/Atom");
                    writer.WriteAttributeString("scheme", this.typeScheme.OriginalString);
                    writer.WriteAttributeString("term", serverTypeName);
                    writer.WriteEndElement();
                }
                if (type.HasEntityPropertyMappings)
                {
                    using (EpmSyndicationContentSerializer serializer = new EpmSyndicationContentSerializer(type.EpmTargetTree, box.Entity, writer))
                    {
                        serializer.Serialize();
                    }
                }
                else
                {
                    writer.WriteElementString("title", "http://www.w3.org/2005/Atom", string.Empty);
                    writer.WriteStartElement("author", "http://www.w3.org/2005/Atom");
                    writer.WriteElementString("name", "http://www.w3.org/2005/Atom", string.Empty);
                    writer.WriteEndElement();
                    writer.WriteElementString("updated", "http://www.w3.org/2005/Atom", XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.RoundtripKind));
                }
                if (EntityStates.Modified == box.State)
                {
                    writer.WriteElementString("id", Util.DereferenceIdentity(box.Identity));
                }
                else
                {
                    writer.WriteElementString("id", "http://www.w3.org/2005/Atom", string.Empty);
                }
                if (EntityStates.Added == box.State)
                {
                    this.CreateRequestDataLinks(box, writer);
                }
                if (!(type.IsMediaLinkEntry || box.IsMediaLinkEntry))
                {
                    writer.WriteStartElement("content", "http://www.w3.org/2005/Atom");
                    writer.WriteAttributeString("type", "application/xml");
                }
                writer.WriteStartElement("properties", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
                this.WriteContentProperties(writer, type, box.Entity, type.HasEntityPropertyMappings ? type.EpmSourceTree.Root : null, out flag);
                writer.WriteEndElement();
                if (!(type.IsMediaLinkEntry || box.IsMediaLinkEntry))
                {
                    writer.WriteEndElement();
                }
                if (type.HasEntityPropertyMappings)
                {
                    using (EpmCustomContentSerializer serializer2 = new EpmCustomContentSerializer(type.EpmTargetTree, box.Entity, writer))
                    {
                        serializer2.Serialize();
                    }
                }
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
                if (this.WritingEntity != null)
                {
                    ReadingWritingEntityEventArgs e = new ReadingWritingEntityEventArgs(box.Entity, document.Root);
                    this.WritingEntity(this, e);
                    XmlWriterSettings settings = XmlUtil.CreateXmlWriterSettings(HttpProcessUtility.EncodingUtf8NoPreamble);
                    settings.ConformanceLevel = ConformanceLevel.Auto;
                    using (XmlWriter writer2 = XmlWriter.Create(stream, settings))
                    {
                        document.Save(writer2);
                    }
                }
                if (newline)
                {
                    for (int i = 0; i < NewLine.Length; i++)
                    {
                        stream.WriteByte((byte) NewLine[i]);
                    }
                }
                stream.Position = 0L;
            }
            return stream;
        }

        private MemoryStream CreateRequestData(LinkDescriptor binding, bool newline)
        {
            string originalString;
            Debug.Assert((binding.State == EntityStates.Added) || ((binding.State == EntityStates.Modified) && (null != binding.Target)), "This method must be called only when a binding is added or put");
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlUtil.CreateXmlWriterAndWriteProcessingInstruction(stream, HttpProcessUtility.EncodingUtf8NoPreamble);
            EntityDescriptor descriptor = this.entityDescriptors[binding.Target];
            writer.WriteStartElement("uri", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
            if (null != descriptor.Identity)
            {
                originalString = descriptor.GetResourceUri(this.baseUriWithSlash, false).OriginalString;
            }
            else
            {
                originalString = "$" + descriptor.ChangeOrder.ToString(CultureInfo.InvariantCulture);
            }
            writer.WriteValue(originalString);
            writer.WriteEndElement();
            writer.Flush();
            if (newline)
            {
                for (int i = 0; i < NewLine.Length; i++)
                {
                    stream.WriteByte((byte) NewLine[i]);
                }
            }
            stream.Position = 0L;
            return stream;
        }

        private void CreateRequestDataLinks(EntityDescriptor box, XmlWriter writer)
        {
            Debug.Assert(EntityStates.Added == box.State, "entity not added state");
            ClientType type = null;
            foreach (LinkDescriptor descriptor in this.RelatedLinks(box))
            {
                string str;
                Debug.Assert(!descriptor.ContentGeneratedForSave, "already saved link");
                descriptor.ContentGeneratedForSave = true;
                if (null == type)
                {
                    type = ClientType.Create(box.Entity.GetType());
                }
                if (null != type.GetProperty(descriptor.SourceProperty, false).CollectionType)
                {
                    str = "application/atom+xml;type=feed";
                }
                else
                {
                    str = "application/atom+xml;type=entry";
                }
                Debug.Assert(null != descriptor.Target, "null is DELETE");
                string str2 = this.entityDescriptors[descriptor.Target].EditLink.ToString();
                writer.WriteStartElement("link", "http://www.w3.org/2005/Atom");
                writer.WriteAttributeString("href", str2);
                writer.WriteAttributeString("rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/related/" + descriptor.SourceProperty);
                writer.WriteAttributeString("type", str);
                writer.WriteEndElement();
            }
        }

        private Uri CreateRequestRelativeUri(LinkDescriptor binding)
        {
            Uri uri;
            if ((null != ClientType.Create(binding.Source.GetType()).GetProperty(binding.SourceProperty, false).CollectionType) && (EntityStates.Added != binding.State))
            {
                Debug.Assert(null != binding.Target, "null target in collection");
                EntityDescriptor descriptor = this.entityDescriptors[binding.Target];
                Uri uri2 = this.ResolveEntitySetNameBase(binding.SourceProperty, binding.SourceProperty).MakeRelativeUri(GenerateEditLinkUri(this.ResolveEntitySetName(binding.SourceProperty), descriptor.Entity));
                uri = Util.CreateUri("$links/" + uri2.OriginalString, UriKind.Relative);
            }
            else
            {
                uri = Util.CreateUri("$links/" + binding.SourceProperty, UriKind.Relative);
            }
            Debug.Assert(!uri.IsAbsoluteUri, "should be relative uri");
            return uri;


            //Uri uri;
            //if ((null != ClientType.Create(binding.Source.GetType()).GetProperty(binding.SourceProperty, false).CollectionType) && (EntityStates.Added != binding.State))
            //{
            //    Debug.Assert(null != binding.Target, "null target in collection");
            //    EntityDescriptor descriptor = this.entityDescriptors[binding.Target];
            //    Uri uri2 = this.BaseUriWithSlash.MakeRelativeUri(GenerateEditLinkUri(this.BaseUriWithSlash, binding.SourceProperty, descriptor.Entity));
            //    uri = Util.CreateUri("$links/" + uri2.OriginalString, UriKind.Relative);
            //}
            //else
            //{
            //    uri = Util.CreateUri("$links/" + binding.SourceProperty, UriKind.Relative);
            //}
            //Debug.Assert(!uri.IsAbsoluteUri, "should be relative uri");
            //return uri;
        }

        internal Uri ResolveEntitySetNameBase(string entitySetNamePath, string entitySetName)
        {
            Debug.Assert(null != entitySetNamePath, "null entitySetName");
            Uri uri = this.ResolveEntitySetName(entitySetNamePath);
            if (null == uri)
            {
                uri = Util.CreateUri(entitySetNamePath, UriKind.RelativeOrAbsolute);
            }
            return ((null == uri) ? null : new Uri(uri.OriginalString.Substring(0, uri.OriginalString.LastIndexOf(entitySetName, StringComparison.Ordinal))));
        }

        internal Uri ResolveEntitySetName(string entitySetName)
        {
            Debug.Assert(null != entitySetName, "null entitySetName");
            Func<string, Uri> resolveSet = this.ResolveSet;
            if (null != resolveSet)
            {
                return resolveSet(entitySetName);
            }
            return ((resolveSet != null) ? resolveSet(entitySetName) : new Uri(this.baseUriWithSlash + entitySetName, UriKind.Absolute));
        }

 




        private Uri CreateRequestUri(EntityDescriptor sourceResource, LinkDescriptor binding)
        {
            Uri uri;
            sourceResource.RelationshipLinks.TryGetValue(binding.SourceProperty, out uri);
            if (null == uri)
            {
                Util.CreateUri(sourceResource.GetResourceUri(this.baseUriWithSlash, false), this.CreateRequestRelativeUri(binding));
            }
            return uri;

            //return Util.CreateUri(sourceResource.GetResourceUri(this.baseUriWithSlash, false), this.CreateRequestRelativeUri(binding));
        }

        public void DeleteLink(object source, string sourceProperty, object target)
        {
            bool flag = this.EnsureRelatable(source, sourceProperty, target, EntityStates.Deleted);
            LinkDescriptor descriptor = null;
            LinkDescriptor key = new LinkDescriptor(source, sourceProperty, target);
            if (this.bindings.TryGetValue(key, out descriptor) && (EntityStates.Added == descriptor.State))
            {
                this.DetachExistingLink(descriptor, false);
            }
            else
            {
                if (flag)
                {
                    throw Error.InvalidOperation(Strings.Context_NoRelationWithInsertEnd);
                }
                if (null == descriptor)
                {
                    this.bindings.Add(key, key);
                    descriptor = key;
                }
                if (EntityStates.Deleted != descriptor.State)
                {
                    descriptor.State = EntityStates.Deleted;
                    this.IncrementChange(descriptor);
                }
            }
        }

        public void DeleteObject(object entity)
        {
            Util.CheckArgumentNull<object>(entity, "entity");
            EntityDescriptor descriptor = null;
            if (!this.entityDescriptors.TryGetValue(entity, out descriptor))
            {
                throw Error.InvalidOperation(Strings.Context_EntityNotContained);
            }
            EntityStates state = descriptor.State;
            if (EntityStates.Added == state)
            {
                this.DetachResource(descriptor);
            }
            else if (EntityStates.Deleted != state)
            {
                Debug.Assert(IncludeLinkState(state), "bad state transition to deleted");
                descriptor.State = EntityStates.Deleted;
                this.IncrementChange(descriptor);
            }
        }

        public bool Detach(object entity)
        {
            Util.CheckArgumentNull<object>(entity, "entity");
            EntityDescriptor descriptor = null;
            return (this.entityDescriptors.TryGetValue(entity, out descriptor) && this.DetachResource(descriptor));
        }

        private void DetachExistingLink(LinkDescriptor existingLink, bool targetDelete)
        {
            if (existingLink.Target != null)
            {
                EntityDescriptor descriptor = this.entityDescriptors[existingLink.Target];
                if (descriptor.IsDeepInsert && !targetDelete)
                {
                    EntityDescriptor parentForInsert = descriptor.ParentForInsert;
                    if (object.ReferenceEquals(descriptor.ParentEntity, existingLink.Source) && ((parentForInsert.State != EntityStates.Deleted) || (parentForInsert.State != EntityStates.Detached)))
                    {
                        throw new InvalidOperationException(Strings.Context_ChildResourceExists);
                    }
                }
            }
            if (this.bindings.Remove(existingLink))
            {
                existingLink.State = EntityStates.Detached;
            }
        }

        public bool DetachLink(object source, string sourceProperty, object target)
        {
            LinkDescriptor descriptor;
            Util.CheckArgumentNull<object>(source, "source");
            Util.CheckArgumentNotEmpty(sourceProperty, "sourceProperty");
            LinkDescriptor key = new LinkDescriptor(source, sourceProperty, target);
            if (!this.bindings.TryGetValue(key, out descriptor))
            {
                return false;
            }
            this.DetachExistingLink(descriptor, false);
            return true;
        }

        private LinkDescriptor DetachReferenceLink(object source, string sourceProperty, object target, System.Data.Services.Client.MergeOption linkMerge)
        {
            LinkDescriptor existing = this.GetLinks(source, sourceProperty).FirstOrDefault();
            if (null != existing)
            {
                if ((target == existing.Target) ||
                    (MergeOption.AppendOnly == linkMerge) ||
                    (MergeOption.PreserveChanges == linkMerge && EntityStates.Modified == existing.State))
                {
                    return existing;
                }

                this.DetachExistingLink(existing, false);
                Debug.Assert(!this.bindings.Values.Any(o => (o.Source == source) && (o.SourceProperty == sourceProperty)), "only expecting one");
            }

            return null;
        }

        private bool DetachResource(EntityDescriptor resource)
        {
            foreach (LinkDescriptor end in this.bindings.Values.Where(resource.IsRelatedEntity).ToList())
            {
                this.DetachExistingLink(
                        end,
                        end.Target == resource.Entity && resource.State == EntityStates.Added);
            }

            resource.ChangeOrder = UInt32.MaxValue;
            resource.State = EntityStates.Detached;
            bool flag = this.entityDescriptors.Remove(resource.Entity);
            Debug.Assert(flag, "should have removed existing entity");
            this.DetachResourceIdentity(resource);

            return true;
        }

        private void DetachResourceIdentity(EntityDescriptor resource)
        {
            EntityDescriptor descriptor = null;
            if (((resource.Identity != null) && this.identityToDescriptor.TryGetValue(resource.Identity, out descriptor)) && object.ReferenceEquals(descriptor, resource))
            {
                Debug.Assert(this.identityToDescriptor.Remove(resource.Identity), "should have removed existing identity");
            }
        }

        public IEnumerable<TElement> EndExecute<TElement>(IAsyncResult asyncResult)
        {
            Util.CheckArgumentNull<IAsyncResult>(asyncResult, "asyncResult");
            return DataServiceRequest.EndExecute<TElement>(this, this, asyncResult);
        }

        public DataServiceResponse EndExecuteBatch(IAsyncResult asyncResult)
        {
            return BaseAsyncResult.EndExecute<SaveResult>(this, "ExecuteBatch", asyncResult).EndRequest();
        }

        public DataServiceStreamResponse EndGetReadStream(IAsyncResult asyncResult)
        {
            return BaseAsyncResult.EndExecute<GetReadStreamResult>(this, "GetReadStream", asyncResult).End();
        }

        public QueryOperationResponse EndLoadProperty(IAsyncResult asyncResult)
        {
            return BaseAsyncResult.EndExecute<LoadPropertyResult>(this, "LoadProperty", asyncResult).LoadProperty();
        }

        public DataServiceResponse EndSaveChanges(IAsyncResult asyncResult)
        {
            DataServiceResponse response = BaseAsyncResult.EndExecute<SaveResult>(this, "SaveChanges", asyncResult).EndRequest();
            if (this.ChangesSaved != null)
            {
                this.ChangesSaved(this, new SaveChangesEventArgs(response));
            }
            return response;
        }

        private EntityDescriptor EnsureContained(object resource, string parameterName)
        {
            Util.CheckArgumentNull<object>(resource, parameterName);
            EntityDescriptor descriptor = null;
            if (!this.entityDescriptors.TryGetValue(resource, out descriptor))
            {
                throw Error.InvalidOperation(Strings.Context_EntityNotContained);
            }
            return descriptor;
        }

        private void EnsureIdentityToResource()
        {
            if (null == this.identityToDescriptor)
            {
                Interlocked.CompareExchange<Dictionary<string, EntityDescriptor>>(ref this.identityToDescriptor, new Dictionary<string, EntityDescriptor>(EqualityComparer<string>.Default), null);
            }
        }

        private bool EnsureRelatable(object source, string sourceProperty, object target, EntityStates state)
        {
            EntityDescriptor descriptor = this.EnsureContained(source, "source");
            EntityDescriptor descriptor2 = null;
            if ((target != null) || ((EntityStates.Modified != state) && (EntityStates.Unchanged != state)))
            {
                descriptor2 = this.EnsureContained(target, "target");
            }
            Util.CheckArgumentNotEmpty(sourceProperty, "sourceProperty");
            ClientType type = ClientType.Create(source.GetType());
            Debug.Assert(type.IsEntityType, "should be enforced by just adding an object");
            ClientType.ClientProperty property = type.GetProperty(sourceProperty, false);
            if (property.IsKnownType)
            {
                throw Error.InvalidOperation(Strings.Context_RelationNotRefOrCollection);
            }
            if (((EntityStates.Unchanged == state) && (target == null)) && (null != property.CollectionType))
            {
                descriptor2 = this.EnsureContained(target, "target");
            }
            if (((EntityStates.Added == state) || (EntityStates.Deleted == state)) && (null == property.CollectionType))
            {
                throw Error.InvalidOperation(Strings.Context_AddLinkCollectionOnly);
            }
            if ((EntityStates.Modified == state) && (null != property.CollectionType))
            {
                throw Error.InvalidOperation(Strings.Context_SetLinkReferenceOnly);
            }
            type = ClientType.Create(property.CollectionType ?? property.PropertyType);
            Debug.Assert(type.IsEntityType, "should be enforced by just adding an object");
            if (!((target == null) || type.ElementType.IsInstanceOfType(target)))
            {
                throw Error.Argument(Strings.Context_RelationNotRefOrCollection, "target");
            }
            if (((EntityStates.Added == state) || (EntityStates.Unchanged == state)) && ((descriptor.State == EntityStates.Deleted) || ((descriptor2 != null) && (descriptor2.State == EntityStates.Deleted))))
            {
                throw Error.InvalidOperation(Strings.Context_NoRelationWithDeleteEnd);
            }
            if (((EntityStates.Deleted == state) || (EntityStates.Unchanged == state)) && ((descriptor.State == EntityStates.Added) || ((descriptor2 != null) && (descriptor2.State == EntityStates.Added))))
            {
                if (EntityStates.Deleted != state)
                {
                    throw Error.InvalidOperation(Strings.Context_NoRelationWithInsertEnd);
                }
                return true;
            }
            return false;
        }

        internal void FireReadingEntityEvent(object entity, XElement data)
        {
            Debug.Assert(entity != null, "entity != null");
            Debug.Assert(data != null, "data != null");
            ReadingWritingEntityEventArgs e = new ReadingWritingEntityEventArgs(entity, data);
            this.ReadingEntity(this, e);
        }

        private static Uri GenerateEditLinkUri(Uri entitySetUri, object entity)
        {
            Debug.Assert((null != entitySetUri) && entitySetUri.IsAbsoluteUri, "baseUriWithSlash");
            Debug.Assert(null != entity, "entity");
            ValidateEntityTypeHasKeys(entity);
            StringBuilder builder = new StringBuilder();
            builder.Append(entitySetUri.AbsoluteUri);
            builder.Append("(");
            string str = string.Empty;
            ClientType.ClientProperty[] propertyArray = ClientType.Create(entity.GetType()).Properties.Where<ClientType.ClientProperty>(new Func<ClientType.ClientProperty, bool>(ClientType.ClientProperty.GetKeyProperty)).ToArray<ClientType.ClientProperty>();
            foreach (ClientType.ClientProperty property in propertyArray)
            {
                string str2;
                builder.Append(str);
                if (1 < propertyArray.Length)
                {
                    builder.Append(property.PropertyName).Append("=");
                }
                object obj2 = property.GetValue(entity);
                if (null == obj2)
                {
                    throw Error.InvalidOperation(Strings.Serializer_NullKeysAreNotSupported(property.PropertyName));
                }
                if (!ClientConvert.TryKeyPrimitiveToString(obj2, out str2))
                {
                    throw Error.InvalidOperation(Strings.Context_CannotConvertKey(obj2));
                }
                builder.Append(Uri.EscapeDataString(str2));
                str = ",";
            }
            builder.Append(")");
            return Util.CreateUri(builder.ToString(), UriKind.Absolute);
        }

 


        private static Uri GenerateEditLinkUri(Uri baseUriWithSlash, string entitySetName, object entity)
        {
            Debug.Assert(((null != baseUriWithSlash) && baseUriWithSlash.IsAbsoluteUri) && baseUriWithSlash.OriginalString.EndsWith("/", StringComparison.Ordinal), "baseUriWithSlash");
            Debug.Assert(!string.IsNullOrEmpty(entitySetName) && !entitySetName.StartsWith("/", StringComparison.Ordinal), "entitySetName");
            ValidateEntityTypeHasKeys(entity);
            StringBuilder builder = new StringBuilder();
            builder.Append(baseUriWithSlash.AbsoluteUri);
            builder.Append(entitySetName);
            builder.Append("(");
            string str = string.Empty;
            //ClientType.ClientProperty[] propertyArray = Enumerable.Where<ClientType.ClientProperty>(ClientType.Create(entity.GetType()).Properties, new Func<ClientType.ClientProperty, bool>(null, (IntPtr) ClientType.ClientProperty.GetKeyProperty)).ToArray<ClientType.ClientProperty>();
            ClientType.ClientProperty[] propertyArray = ClientType.Create(entity.GetType()).Properties.Where<ClientType.ClientProperty>(ClientType.ClientProperty.GetKeyProperty).ToArray();
            foreach (ClientType.ClientProperty property in propertyArray)
            {
                string str2;
                builder.Append(str);
                if (1 < propertyArray.Length)
                {
                    builder.Append(property.PropertyName).Append("=");
                }
                object obj2 = property.GetValue(entity);
                if (null == obj2)
                {
                    throw Error.InvalidOperation(Strings.Serializer_NullKeysAreNotSupported(property.PropertyName));
                }
                if (!ClientConvert.TryKeyPrimitiveToString(obj2, out str2))
                {
                    throw Error.InvalidOperation(Strings.Context_CannotConvertKey(obj2));
                }
                builder.Append(Uri.EscapeDataString(str2));
                str = ",";
            }
            builder.Append(")");
            return Util.CreateUri(builder.ToString(), UriKind.Absolute);
        }

        public EntityDescriptor GetEntityDescriptor(object entity)
        {
            EntityDescriptor descriptor;
            Util.CheckArgumentNull<object>(entity, "entity");
            if (this.entityDescriptors.TryGetValue(entity, out descriptor))
            {
                return descriptor;
            }
            return null;
        }

        private static string GetEntityHttpMethod(EntityStates state, bool replaceOnUpdate)
        {
            EntityStates states = state;
            if (states != EntityStates.Added)
            {
                if (states != EntityStates.Deleted)
                {
                    if (states != EntityStates.Modified)
                    {
                        throw Error.InternalError(InternalError.UnvalidatedEntityState);
                    }
                }
                else
                {
                    return "DELETE";
                }
                if (replaceOnUpdate)
                {
                    return "PUT";
                }
                return "MERGE";
            }
            return "POST";
        }

        public LinkDescriptor GetLinkDescriptor(object source, string sourceProperty, object target)
        {
            LinkDescriptor descriptor;
            Util.CheckArgumentNull<object>(source, "source");
            Util.CheckArgumentNotEmpty(sourceProperty, "sourceProperty");
            Util.CheckArgumentNull<object>(target, "target");
            if (this.bindings.TryGetValue(new LinkDescriptor(source, sourceProperty, target), out descriptor))
            {
                return descriptor;
            }
            return null;
        }

        private static string GetLinkHttpMethod(LinkDescriptor link)
        {
            if (null == ClientType.Create(link.Source.GetType()).GetProperty(link.SourceProperty, false).CollectionType)
            {
                Debug.Assert(EntityStates.Modified == link.State, "not Modified state");
                if (null == link.Target)
                {
                    return "DELETE";
                }
                return "PUT";
            }
            if (EntityStates.Deleted == link.State)
            {
                return "DELETE";
            }
            Debug.Assert(EntityStates.Added == link.State, "not Added state");
            return "POST";
        }

        internal IEnumerable<LinkDescriptor> GetLinks(object source, string sourceProperty)
        {
            return this.bindings.Values.Where(o => (o.Source == source) && (o.SourceProperty == sourceProperty));
        }

        public Uri GetMetadataUri()
        {
            return Util.CreateUri(this.baseUriWithSlash.OriginalString + "$metadata", UriKind.Absolute);
        }

        public Uri GetReadStreamUri(object entity)
        {
            return this.EnsureContained(entity, "entity").GetMediaResourceUri(this.baseUriWithSlash);
        }

        internal static DataServiceClientException GetResponseText(Func<Stream> getResponseStream, HttpStatusCode statusCode)
        {
            string str = null;
            using (Stream stream = getResponseStream.Invoke())
            {
                if ((stream != null) && stream.CanRead)
                {
                    str = new StreamReader(stream).ReadToEnd();
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                str = statusCode.ToString();
            }
            return new DataServiceClientException(str, (int) statusCode);
        }

        internal string GetServerTypeName(EntityDescriptor descriptor)
        {
            Debug.Assert((descriptor != null) && (descriptor.Entity != null), "Null descriptor or no entity in descriptor");
            if (this.resolveName != null)
            {
                Type type = descriptor.Entity.GetType();
                GeneratedCodeAttribute attribute = this.resolveName.Method.GetCustomAttributes(false).OfType<GeneratedCodeAttribute>().FirstOrDefault<GeneratedCodeAttribute>();
                if ((attribute == null) || (attribute.Tool != "System.Data.Services.Design"))
                {
                    return (this.resolveName.Invoke(type) ?? descriptor.ServerTypeName);
                }
                return (descriptor.ServerTypeName ?? this.resolveName.Invoke(type));
            }
            return descriptor.ServerTypeName;
        }

        internal static Exception HandleResponse(HttpStatusCode statusCode, string responseVersion, Func<Stream> getResponseStream, bool throwOnFailure)
        {
            InvalidOperationException responseText = null;
            if (!CanHandleResponseVersion(responseVersion))
            {
                responseText = Error.InvalidOperation(Strings.Context_VersionNotSupported(responseVersion, SerializeSupportedVersions()));
            }
            if (!((responseText != null) || WebUtil.SuccessStatusCode(statusCode)))
            {
                responseText = GetResponseText(getResponseStream, statusCode);
            }
            if ((responseText != null) && throwOnFailure)
            {
                throw responseText;
            }
            return responseText;
        }

        private void HandleResponseDelete(Descriptor entry)
        {
            if (EntityStates.Deleted != entry.State)
            {
                Error.ThrowBatchUnexpectedContent(InternalError.EntityNotDeleted);
            }
            if (entry.IsResource)
            {
                EntityDescriptor resource = (EntityDescriptor) entry;
                this.DetachResource(resource);
            }
            else
            {
                this.DetachExistingLink((LinkDescriptor) entry, false);
            }
        }

        private static void HandleResponsePost(LinkDescriptor entry)
        {
            if ((EntityStates.Added != entry.State) && ((EntityStates.Modified != entry.State) || (null == entry.Target)))
            {
                Error.ThrowBatchUnexpectedContent(InternalError.LinkNotAddedState);
            }
            entry.State = EntityStates.Unchanged;
        }

        private void HandleResponsePost(EntityDescriptor entry, MaterializeAtom materializer, Uri editLink, string etag)
        {
            Debug.Assert(editLink != null, "location header must be specified in POST responses.");
            if ((EntityStates.Added != entry.State) && (StreamStates.Added != entry.StreamState))
            {
                Error.ThrowBatchUnexpectedContent(InternalError.EntityNotAddedState);
            }
            if (materializer == null)
            {
                string identity = Util.ReferenceIdentity(editLink.ToString());
                this.AttachIdentity(identity, null, editLink, entry.Entity, etag);
            }
            else
            {
                materializer.SetInsertingObject(entry.Entity);
                foreach (object obj2 in materializer)
                {
                    Debug.Assert(null != entry.Identity, "updated inserted should always gain an identity");
                    Debug.Assert(obj2 == entry.Entity, "x == box.Entity, should have same object generated by response");
                    Debug.Assert(EntityStates.Unchanged == entry.State, "should have moved out of insert");
                    Debug.Assert((this.identityToDescriptor != null) && this.identityToDescriptor.ContainsKey(entry.Identity), "should have identity tracked");
                    if (entry.EditLink == null)
                    {
                        entry.EditLink = editLink;
                    }
                    if (entry.ETag == null)
                    {
                        entry.ETag = etag;
                    }
                }
            }
            foreach (LinkDescriptor descriptor in this.RelatedLinks(entry))
            {
                Debug.Assert(0 != descriptor.SaveResultWasProcessed, "link should have been saved with the enty");
                if (IncludeLinkState(descriptor.SaveResultWasProcessed) || (descriptor.SaveResultWasProcessed == EntityStates.Added))
                {
                    HandleResponsePost(descriptor);
                }
            }
        }

        private static void HandleResponsePut(Descriptor entry, string etag)
        {
            if (entry.IsResource)
            {
                EntityDescriptor descriptor = (EntityDescriptor) entry;
                if ((EntityStates.Modified != descriptor.State) && (StreamStates.Modified != descriptor.StreamState))
                {
                    Error.ThrowBatchUnexpectedContent(InternalError.EntryNotModified);
                }
                if (descriptor.StreamState == StreamStates.Modified)
                {
                    descriptor.StreamETag = etag;
                    descriptor.StreamState = StreamStates.NoStream;
                }
                else
                {
                    Debug.Assert(descriptor.State == EntityStates.Modified, "descriptor.State == EntityStates.Modified");
                    descriptor.ETag = etag;
                    descriptor.State = EntityStates.Unchanged;
                }
            }
            else
            {
                LinkDescriptor descriptor2 = (LinkDescriptor) entry;
                if ((EntityStates.Added == entry.State) || (EntityStates.Modified == entry.State))
                {
                    descriptor2.State = EntityStates.Unchanged;
                }
                else if (EntityStates.Detached != entry.State)
                {
                    Error.ThrowBatchUnexpectedContent(InternalError.LinkBadState);
                }
            }
        }

        private static bool IncludeLinkState(EntityStates x)
        {
            return ((EntityStates.Modified == x) || (EntityStates.Unchanged == x));
        }

        private void IncrementChange(Descriptor descriptor)
        {
            descriptor.ChangeOrder = ++this.nextChange;
        }

        internal EntityDescriptor InternalAttachEntityDescriptor(EntityDescriptor descriptor, bool failIfDuplicated)
        {
            EntityDescriptor descriptor2;
            EntityDescriptor descriptor3;
            Debug.Assert(null != descriptor.Identity, "must have identity");
            Debug.Assert((descriptor.Entity != null) && ClientType.Create(descriptor.Entity.GetType()).IsEntityType, "must be entity type to attach");
            this.EnsureIdentityToResource();
            this.entityDescriptors.TryGetValue(descriptor.Entity, out descriptor2);
            this.identityToDescriptor.TryGetValue(descriptor.Identity, out descriptor3);
            if (failIfDuplicated && (null != descriptor2))
            {
                throw Error.InvalidOperation(Strings.Context_EntityAlreadyContained);
            }
            if (descriptor2 != descriptor3)
            {
                throw Error.InvalidOperation(Strings.Context_DifferentEntityAlreadyContained);
            }
            if (null == descriptor2)
            {
                descriptor2 = descriptor;
                this.IncrementChange(descriptor);
                this.entityDescriptors.Add(descriptor.Entity, descriptor);
                this.identityToDescriptor.Add(descriptor.Identity, descriptor);
            }
            return descriptor2;
        }

        private static bool IsFlagSet(SaveChangesOptions options, SaveChangesOptions flag)
        {
            return ((options & flag) == flag);
        }

        private IEnumerable<LinkDescriptor> RelatedLinks(EntityDescriptor box)
        {
             foreach (LinkDescriptor end in this.bindings.Values)
            {
                if (end.Source == box.Entity)
                {
                    if (null != end.Target)
                    {   
                        EntityDescriptor target = this.entityDescriptors[end.Target];


                        if (IncludeLinkState(target.SaveResultWasProcessed) || ((0 == target.SaveResultWasProcessed) && IncludeLinkState(target.State)) ||
                            ((null != target.Identity) && (target.ChangeOrder < box.ChangeOrder) &&
                             ((0 == target.SaveResultWasProcessed && EntityStates.Added == target.State) ||
                              (EntityStates.Added == target.SaveResultWasProcessed))))
                        {
                            Debug.Assert(box.ChangeOrder < end.ChangeOrder, "saving is out of order");
                            yield return end;
                        }
                    }
                }
            }
            //<RelatedLinks>d__7 d__ = new <RelatedLinks>d__7(-2);
            //d__.<>4__this = this;
            //d__.<>3__box = box;
            //return d__;
        }

        internal string ResolveNameFromType(Type type)
        {
            Debug.Assert(null != type, "null type");
            Func<Type, string> resolveName = this.ResolveName;
            return ((resolveName != null) ? resolveName.Invoke(type) : null);
        }

        internal Type ResolveTypeFromName(string wireName, Type userType, bool checkAssignable)
        {
            Type type;
            Debug.Assert(null != userType, "null != baseType");
            if (string.IsNullOrEmpty(wireName))
            {
                return userType;
            }
            if (!ClientConvert.ToNamedType(wireName, out type))
            {
                type = null;
                Func<string, Type> resolveType = this.ResolveType;
                if (null != resolveType)
                {
                    type = resolveType.Invoke(wireName);
                }
                if (null == type)
                {
                    type = ClientType.ResolveFromName(wireName, userType, base.GetType());
                }
                if (!((!checkAssignable || (type == null)) || userType.IsAssignableFrom(type)))
                {
                    throw Error.InvalidOperation(Strings.Deserialize_Current(userType, type));
                }
            }
            return (type ?? userType);
        }

        private int SaveResultProcessed(Descriptor entry)
        {
            entry.SaveResultWasProcessed = entry.State;
            int num = 0;
            if (entry.IsResource && (EntityStates.Added == entry.State))
            {
                foreach (LinkDescriptor descriptor in this.RelatedLinks((EntityDescriptor) entry))
                {
                    Debug.Assert(descriptor.ContentGeneratedForSave, "link should have been saved with the enty");
                    if (descriptor.ContentGeneratedForSave)
                    {
                        Debug.Assert(0 == descriptor.SaveResultWasProcessed, "this link already had a result");
                        descriptor.SaveResultWasProcessed = descriptor.State;
                        num++;
                    }
                }
            }
            return num;
        }

        private static string SerializeSupportedVersions()
        {
            Debug.Assert(Util.SupportedResponseVersions.Length > 0, "At least one supported version must exist.");
            StringBuilder builder = new StringBuilder("'").Append(Util.SupportedResponseVersions[0].ToString());
            for (int i = 1; i < Util.SupportedResponseVersions.Length; i++)
            {
                builder.Append("', '");
                builder.Append(Util.SupportedResponseVersions[i].ToString());
            }
            builder.Append("'");
            return builder.ToString();
        }

        public void SetLink(object source, string sourceProperty, object target)
        {
            this.EnsureRelatable(source, sourceProperty, target, EntityStates.Modified);
            LinkDescriptor key = this.DetachReferenceLink(source, sourceProperty, target, System.Data.Services.Client.MergeOption.NoTracking);
            if (null == key)
            {
                key = new LinkDescriptor(source, sourceProperty, target);
                this.bindings.Add(key, key);
            }
            Debug.Assert((key.State == 0) || IncludeLinkState(key.State), "set link entity state");
            if (EntityStates.Modified != key.State)
            {
                key.State = EntityStates.Modified;
                this.IncrementChange(key);
            }
        }

        public void SetSaveStream(object entity, Stream stream, bool closeStream, DataServiceRequestArgs args)
        {
            EntityDescriptor descriptor = this.EnsureContained(entity, "entity");
            Util.CheckArgumentNull<Stream>(stream, "stream");
            Util.CheckArgumentNull<DataServiceRequestArgs>(args, "args");
            ClientType type = ClientType.Create(entity.GetType());
            if (type.MediaDataMember != null)
            {
                throw new ArgumentException(Strings.Context_SetSaveStreamOnMediaEntryProperty(type.ElementTypeName), "entity");
            }
            descriptor.SaveStream = new DataServiceSaveStream(stream, closeStream, args);
            Debug.Assert(descriptor.State != EntityStates.Detached, "We should never have a detached entity in the entityDescriptor dictionary.");
            switch (descriptor.State)
            {
                case EntityStates.Unchanged:
                case EntityStates.Modified:
                    descriptor.StreamState = StreamStates.Modified;
                    return;

                case EntityStates.Added:
                    descriptor.StreamState = StreamStates.Added;
                    return;
            }
            throw new DataServiceClientException(Strings.DataServiceException_GeneralError);
        }

        public void SetSaveStream(object entity, Stream stream, bool closeStream, string contentType, string slug)
        {
            Util.CheckArgumentNull<string>(contentType, "contentType");
            Util.CheckArgumentNull<string>(slug, "slug");
            DataServiceRequestArgs args = new DataServiceRequestArgs();
            args.ContentType = contentType;
            args.Slug = slug;
            this.SetSaveStream(entity, stream, closeStream, args);
        }

        public bool TryGetEntity<TEntity>(Uri identity, out TEntity entity) where TEntity: class
        {
            EntityStates states;
            entity = default(TEntity);
            Util.CheckArgumentNull<Uri>(identity, "relativeUri");
            entity = (TEntity) this.TryGetEntity(Util.ReferenceIdentity(identity.ToString()), null, System.Data.Services.Client.MergeOption.AppendOnly, out states);
            return (null != ((TEntity) entity));
        }

        internal object TryGetEntity(string resourceUri, string etag, System.Data.Services.Client.MergeOption merger, out EntityStates state)
        {
            Debug.Assert(null != resourceUri, "null uri");
            state = EntityStates.Detached;
            EntityDescriptor descriptor = null;
            if ((this.identityToDescriptor != null) && this.identityToDescriptor.TryGetValue(resourceUri, out descriptor))
            {
                state = descriptor.State;
                if ((etag != null) && (System.Data.Services.Client.MergeOption.AppendOnly != merger))
                {
                    descriptor.ETag = etag;
                }
                Debug.Assert(null != descriptor.Entity, "null entity");
                return descriptor.Entity;
            }
            return null;
        }

        public bool TryGetUri(object entity, out Uri identity)
        {
            identity = null;
            Util.CheckArgumentNull<object>(entity, "entity");
            EntityDescriptor descriptor = null;
            if ((((this.identityToDescriptor != null) && this.entityDescriptors.TryGetValue(entity, out descriptor)) && (descriptor.Identity != null)) && object.ReferenceEquals(descriptor, this.identityToDescriptor[descriptor.Identity]))
            {
                string str = Util.DereferenceIdentity(descriptor.Identity);
                identity = Util.CreateUri(str, UriKind.Absolute);
            }
            return (null != identity);
        }

        public void UpdateObject(object entity)
        {
            Util.CheckArgumentNull<object>(entity, "entity");
            EntityDescriptor descriptor = null;
            if (!this.entityDescriptors.TryGetValue(entity, out descriptor))
            {
                throw Error.Argument(Strings.Context_EntityNotContained, "entity");
            }
            if (EntityStates.Unchanged == descriptor.State)
            {
                descriptor.State = EntityStates.Modified;
                this.IncrementChange(descriptor);
            }
        }

        private void ValidateEntitySetName(ref string entitySetName)
        {
            Util.CheckArgumentNotEmpty(entitySetName, "entitySetName");
            entitySetName = entitySetName.Trim(Util.ForwardSlash);
            Util.CheckArgumentNotEmpty(entitySetName, "entitySetName");
            Uri requestUri = Util.CreateUri(entitySetName, UriKind.RelativeOrAbsolute);
            if (!(!requestUri.IsAbsoluteUri && string.IsNullOrEmpty(Util.CreateUri(this.baseUriWithSlash, requestUri).GetComponents(UriComponents.Fragment | UriComponents.Query, UriFormat.SafeUnescaped))))
            {
                throw Error.Argument(Strings.Context_EntitySetName, "entitySetName");
            }
        }

        private static void ValidateEntityType(object entity)
        {
            Util.CheckArgumentNull<object>(entity, "entity");
            if (!ClientType.Create(entity.GetType()).IsEntityType)
            {
                throw Error.Argument(Strings.Content_EntityIsNotEntityType, "entity");
            }
        }

        private static void ValidateEntityTypeHasKeys(object entity)
        {
            Util.CheckArgumentNull<object>(entity, "entity");
            if (ClientType.Create(entity.GetType()).KeyCount <= 0)
            {
                throw Error.Argument(Strings.Content_EntityWithoutKey, "entity");
            }
        }

        private static void ValidateSaveChangesOptions(SaveChangesOptions options)
        {
            if ((options | (SaveChangesOptions.ReplaceOnUpdate | SaveChangesOptions.ContinueOnError | SaveChangesOptions.Batch)) != (SaveChangesOptions.ReplaceOnUpdate | SaveChangesOptions.ContinueOnError | SaveChangesOptions.Batch))
            {
                throw Error.ArgumentOutOfRange("options");
            }
            if (IsFlagSet(options, SaveChangesOptions.ContinueOnError | SaveChangesOptions.Batch))
            {
                throw Error.ArgumentOutOfRange("options");
            }
        }

        private void WriteContentProperties(XmlWriter writer, ClientType type, object resource, EpmSourcePathSegment currentSegment, out bool propertiesWritten)
        {
            propertiesWritten = false;
            using (IEnumerator<ClientType.ClientProperty> enumerator = type.Properties.GetEnumerator())
            {
                Func<EpmSourcePathSegment, bool> func = null;
                while (enumerator.MoveNext())
                {
                    ClientType.ClientProperty property = enumerator.Current;
                    if ((property != type.MediaDataMember) && ((type.MediaDataMember == null) || (type.MediaDataMember.MimeTypeProperty != property)))
                    {
                        object propertyValue = property.GetValue(resource);

                        EpmSourcePathSegment segment = currentSegment != null ? currentSegment.SubProperties.SingleOrDefault(s => s.PropertyName == property.PropertyName) : null;
                        if (property.IsKnownType)
                        {
                            if (((propertyValue == null) || (segment == null)) || segment.EpmInfo.Attribute.KeepInContent)
                            {
                                WriteContentProperty(writer, this.DataNamespace, property, propertyValue);
                                propertiesWritten = true;
                            }
                        }
                        else if (null == property.CollectionType)
                        {
                            ClientType type2 = ClientType.Create(property.PropertyType);
                            if (!type2.IsEntityType)
                            {
                                XElement element = new XElement((XName) (this.DataNamespace + property.PropertyName));
                                bool flag = false;
                                string str = this.ResolveNameFromType(type2.ElementType);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    element.Add(new XAttribute((XName) ("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata" + "type"), str));
                                }
                                if (null == propertyValue)
                                {
                                    element.Add(new XAttribute((XName) ("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata" + "null"), "true"));
                                    flag = true;
                                }
                                else
                                {
                                    using (XmlWriter writer2 = element.CreateWriter())
                                    {
                                        this.WriteContentProperties(writer2, type2, propertyValue, segment, out flag);
                                    }
                                }
                                if (flag)
                                {
                                    element.WriteTo(writer);
                                    propertiesWritten = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void WriteContentProperty(XmlWriter writer, string namespaceName, ClientType.ClientProperty property, object propertyValue)
        {
            writer.WriteStartElement(property.PropertyName, namespaceName);
            string edmType = ClientConvert.GetEdmType(property.PropertyType);
            if (null != edmType)
            {
                writer.WriteAttributeString("type", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", edmType);
            }
            if (null == propertyValue)
            {
                writer.WriteAttributeString("null", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", "true");
                if (property.KeyProperty)
                {
                    throw Error.InvalidOperation(Strings.Serializer_NullKeysAreNotSupported(property.PropertyName));
                }
            }
            else
            {
                string str2 = ClientConvert.ToString(propertyValue, false);
                if (0 == str2.Length)
                {
                    writer.WriteAttributeString("null", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", "false");
                }
                else
                {
                    if (char.IsWhiteSpace(str2[0]) || char.IsWhiteSpace(str2[str2.Length - 1]))
                    {
                        writer.WriteAttributeString("space", "http://www.w3.org/2000/xmlns/", "preserve");
                    }
                    writer.WriteValue(str2);
                }
            }
            writer.WriteEndElement();
        }

        private static void WriteOperationRequestHeaders(StreamWriter writer, string methodName, string uri, Version requestVersion)
        {
            writer.WriteLine("{0}: {1}", "Content-Type", "application/http");
            writer.WriteLine("{0}: {1}", "Content-Transfer-Encoding", "binary");
            writer.WriteLine();
            writer.WriteLine("{0} {1} {2}", new object[] { methodName, uri, "HTTP/1.1" });
            if ((requestVersion != Util.DataServiceVersion1) && (requestVersion != Util.DataServiceVersionEmpty))
            {
                writer.WriteLine("{0}: {1}{2}", new object[] { "DataServiceVersion", requestVersion, ";NetFx" });
            }
        }

        private static void WriteOperationResponseHeaders(StreamWriter writer, int statusCode)
        {
            writer.WriteLine("{0}: {1}", "Content-Type", "application/http");
            writer.WriteLine("{0}: {1}", "Content-Transfer-Encoding", "binary");
            writer.WriteLine();
            writer.WriteLine("{0} {1} {2}", new object[] { "HTTP/1.1", statusCode, (HttpStatusCode) statusCode });
        }

        public bool ApplyingChanges
        {
            get
            {
                return this.applyingChanges;
            }
            internal set
            {
                this.applyingChanges = value;
            }
        }

        public Uri BaseUri
        {
            get
            {
                return this.baseUri;
            }
            set
            {
                if (this.baseUri == value) return;
                this.baseUri = value;
                this.baseUriWithSlash = value;
                if (!value.OriginalString.EndsWith("/", StringComparison.Ordinal))
                {
                    this.baseUriWithSlash = Util.CreateUri(value.OriginalString + "/", UriKind.Absolute);
                }
            }
        }

        internal Uri BaseUriWithSlash
        {
            get
            {
                return this.baseUriWithSlash;
            }
        }

        public string DataNamespace
        {
            get
            {
                return this.dataNamespace;
            }
            set
            {
                Util.CheckArgumentNull<string>(value, "value");
                this.dataNamespace = value;
            }
        }

        public ReadOnlyCollection<EntityDescriptor> Entities
        {
            get
            {
                       return this.entityDescriptors.Values.OrderBy<EntityDescriptor, uint>(delegate (EntityDescriptor d) {
            return d.ChangeOrder;
        }).ToList<EntityDescriptor>().AsReadOnly();

            }
        }

        internal bool HasReadingEntityHandlers
        {
            [DebuggerStepThrough]
            get
            {
                return (this.ReadingEntity != null);
            }
        }

        public System.Data.Services.Client.HttpStack HttpStack
        {
            get
            {
                return this.httpStack;
            }
            set
            {
                this.httpStack = Util.CheckEnumerationValue(value, "HttpStack");
            }
        }

        public bool IgnoreMissingProperties
        {
            get
            {
                return this.ignoreMissingProperties;
            }
            set
            {
                this.ignoreMissingProperties = value;
            }
        }

        public bool IgnoreResourceNotFoundException
        {
            get
            {
                return this.ignoreResourceNotFoundException;
            }
            set
            {
                this.ignoreResourceNotFoundException = value;
            }
        }

        public ReadOnlyCollection<LinkDescriptor> Links
        {
            get
            {
                        return this.bindings.Values.OrderBy<LinkDescriptor, uint>(delegate (LinkDescriptor l) {
            return l.ChangeOrder;
        }).ToList<LinkDescriptor>().AsReadOnly();

            }
        }

        public System.Data.Services.Client.MergeOption MergeOption
        {
            get
            {
                return this.mergeOption;
            }
            set
            {
                this.mergeOption = Util.CheckEnumerationValue(value, "MergeOption");
            }
        }

        public Func<Type, string> ResolveName
        {
            get
            {
                return this.resolveName;
            }
            set
            {
                this.resolveName = value;
            }
        }

        public Func<string, Type> ResolveType
        {
            get
            {
                return this.resolveType;
            }
            set
            {
                this.resolveType = value;
            }
        }

        public Func<string, Uri> ResolveSet
        {
            get
            {
                return this.resolveSet;
            }
            set
            {
                this.resolveSet = value;
            }
        }
 

 


        public SaveChangesOptions SaveChangesDefaultOptions
        {
            get
            {
                return this.saveChangesDefaultOptions;
            }
            set
            {
                ValidateSaveChangesOptions(value);
                this.saveChangesDefaultOptions = value;
            }
        }

        public Uri TypeScheme
        {
            get
            {
                return this.typeScheme;
            }
            set
            {
                Util.CheckArgumentNull<Uri>(value, "value");
                this.typeScheme = value;
            }
        }

        public bool UsePostTunneling
        {
            get
            {
                return this.postTunneling;
            }
            set
            {
                this.postTunneling = value;
            }
        }

       

        private class SaveResult : BaseAsyncResult
        {
            private readonly string batchBoundary;
            private HttpWebResponse batchResponse;
            private byte[] buildBatchBuffer;
            private StreamWriter buildBatchWriter;
            private readonly List<Descriptor> ChangedEntries;
            private int changesCompleted;
            private string changesetBoundary;
            private bool changesetStarted;
            private readonly DataServiceContext Context;
            private long copiedContentLength;
            private int entryIndex;
            private readonly bool executeAsync;
            private Stream httpWebResponseStream;
            private Stream mediaResourceRequestStream;
            private readonly SaveChangesOptions options;
            private bool processingMediaLinkEntry;
            private bool processingMediaLinkEntryPut;
            private readonly DataServiceRequest[] Queries;
            private PerRequest request;
            private BatchStream responseBatchStream;
            private readonly List<OperationResponse> Responses;
            private System.Data.Services.Client.DataServiceResponse service;

            internal SaveResult(DataServiceContext context, string method, DataServiceRequest[] queries, SaveChangesOptions options, AsyncCallback callback, object state, bool async) : base(context, method, callback, state)
            {
                  this.entryIndex = -1;
    this.executeAsync = async;
    this.Context = context;
    this.Queries = queries;
    this.options = options;
    this.Responses = new List<OperationResponse>();
    if (null == queries)
    {
        this.ChangedEntries = context.entityDescriptors.Values.Cast<Descriptor>().Union<Descriptor>(context.bindings.Values.Cast<Descriptor>()).Where<Descriptor>(delegate (Descriptor o) {
            return (o.IsModified && (o.ChangeOrder != uint.MaxValue));
        }).OrderBy<Descriptor, uint>(delegate (Descriptor o) {
            return o.ChangeOrder;
        }).ToList<Descriptor>();
        foreach (Descriptor descriptor in this.ChangedEntries)
        {
            descriptor.ContentGeneratedForSave = false;
            descriptor.SaveResultWasProcessed = 0;
            descriptor.SaveError = null;
            if (!descriptor.IsResource)
            {
                object target = ((LinkDescriptor) descriptor).Target;
                if (null != target)
                {
                    Descriptor descriptor2 = context.entityDescriptors[target];
                    if (EntityStates.Unchanged == descriptor2.State)
                    {
                        descriptor2.ContentGeneratedForSave = false;
                        descriptor2.SaveResultWasProcessed = 0;
                        descriptor2.SaveError = null;
                    }
                }
            }
        }
    }
    else
    {
        this.ChangedEntries = new List<Descriptor>();
    }
    if (DataServiceContext.IsFlagSet(options, SaveChangesOptions.Batch))
    {
        this.batchBoundary = "batch_" + Guid.NewGuid().ToString();
    }
    else
    {
        this.batchBoundary = "batchresponse_" + Guid.NewGuid().ToString();
        this.DataServiceResponse = new DataServiceResponse(null, -1, this.Responses, false);
    }

            }

            private void AsyncEndGetRequestStream(IAsyncResult asyncResult)
            {
                Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult.IsCompleted");
                PerRequest request = (asyncResult == null) ? null : (asyncResult.AsyncState as PerRequest);
                try
                {
                    CompleteCheck(request, InternalError.InvalidEndGetRequestCompleted);
                    request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                    EqualRefCheck(this.request, request, InternalError.InvalidEndGetRequestStream);
                    Stream stream = Util.NullCheck<Stream>(Util.NullCheck<HttpWebRequest>(request.Request, InternalError.InvalidEndGetRequestStreamRequest).EndGetRequestStream(asyncResult), InternalError.InvalidEndGetRequestStreamStream);
                    request.RequestStream = stream;
                    PerRequest.ContentStream requestContentStream = request.RequestContentStream;
                    Util.NullCheck<PerRequest.ContentStream>(requestContentStream, InternalError.InvalidEndGetRequestStreamContent);
                    Util.NullCheck<Stream>(requestContentStream.Stream, InternalError.InvalidEndGetRequestStreamContent);
                    if (requestContentStream.IsKnownMemoryStream)
                    {
                        MemoryStream stream3 = requestContentStream.Stream as MemoryStream;
                        byte[] buffer = stream3.GetBuffer();
                        int position = (int) stream3.Position;
                        int num2 = ((int) stream3.Length) - position;
                        if ((buffer == null) || (0 == num2))
                        {
                            Error.ThrowInternalError(InternalError.InvalidEndGetRequestStreamContentLength);
                        }
                    }
                    request.RequestContentBufferValidLength = -1;
                    Util.DebugInjectFault("SaveAsyncResult::AsyncEndGetRequestStream_BeforeBeginRead");
                    Stream stream1 = requestContentStream.Stream;
                    asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream1.BeginRead), request.RequestContentBuffer, 0, request.RequestContentBuffer.Length, new AsyncCallback(this.AsyncRequestContentEndRead), request);
                    request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                }
                catch (Exception exception)
                {
                    if (this.HandleFailure(request, exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    this.HandleCompleted(request);
                }
            }

            private void AsyncEndGetResponse(IAsyncResult asyncResult)
            {
                Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult.IsCompleted");
                PerRequest request = (asyncResult == null) ? null : (asyncResult.AsyncState as PerRequest);
                try
                {
                    CompleteCheck(request, InternalError.InvalidEndGetResponseCompleted);
                    request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                    EqualRefCheck(this.request, request, InternalError.InvalidEndGetResponse);
                    HttpWebRequest request2 = Util.NullCheck<HttpWebRequest>(request.Request, InternalError.InvalidEndGetResponseRequest);
                    HttpWebResponse response = null;
                    try
                    {
                        Util.DebugInjectFault("SaveAsyncResult::AsyncEndGetResponse::BeforeEndGetResponse");
                        response = (HttpWebResponse) request2.EndGetResponse(asyncResult);
                    }
                    catch (WebException exception)
                    {
                        response = exception.Response;
                        if (null == response)
                        {
                            throw;
                        }
                    }
                    request.HttpWebResponse = Util.NullCheck<HttpWebResponse>(response, InternalError.InvalidEndGetResponseResponse);
                    if (!DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.Batch))
                    {
                        this.HandleOperationResponse(response);
                    }
                    this.copiedContentLength = 0L;
                    Util.DebugInjectFault("SaveAsyncResult::AsyncEndGetResponse_BeforeGetStream");
                    Stream responseStream = response.GetResponseStream();
                    request.ResponseStream = responseStream;
                    if ((responseStream != null) && responseStream.CanRead)
                    {
                        if (null != this.buildBatchWriter)
                        {
                            this.buildBatchWriter.Flush();
                        }
                        if (null == this.buildBatchBuffer)
                        {
                            this.buildBatchBuffer = new byte[0x1f40];
                        }
                        do
                        {
                            Util.DebugInjectFault("SaveAsyncResult::AsyncEndGetResponse_BeforeBeginRead");
                            asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(responseStream.BeginRead), this.buildBatchBuffer, 0, this.buildBatchBuffer.Length, new AsyncCallback(this.AsyncEndRead), request);
                            request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                        }
                        while (((asyncResult.CompletedSynchronously && !request.RequestCompleted) && !base.IsCompletedInternally) && responseStream.CanRead);
                    }
                    else
                    {
                        request.SetComplete();
                        if (!base.IsCompletedInternally)
                        {
                            this.SaveNextChange(request);
                        }
                    }
                }
                catch (Exception exception2)
                {
                    if (this.HandleFailure(request, exception2))
                    {
                        throw;
                    }
                }
                finally
                {
                    this.HandleCompleted(request);
                }
            }

            private void AsyncEndRead(IAsyncResult asyncResult)
            {
                Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult.IsCompleted");
                PerRequest asyncState = asyncResult.AsyncState as PerRequest;
                int count = 0;
                try
                {
                    CompleteCheck(asyncState, InternalError.InvalidEndReadCompleted);
                    asyncState.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                    EqualRefCheck(this.request, asyncState, InternalError.InvalidEndRead);
                    Stream stream = Util.NullCheck<Stream>(asyncState.ResponseStream, InternalError.InvalidEndReadStream);
                    Util.DebugInjectFault("SaveAsyncResult::AsyncEndRead_BeforeEndRead");
                    count = stream.EndRead(asyncResult);
                    if (0 < count)
                    {
                        Util.NullCheck<Stream>(this.httpWebResponseStream, InternalError.InvalidEndReadCopy).Write(this.buildBatchBuffer, 0, count);
                        this.copiedContentLength += count;
                        if (!asyncResult.CompletedSynchronously && stream.CanRead)
                        {
                            do
                            {
                                asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream.BeginRead), this.buildBatchBuffer, 0, this.buildBatchBuffer.Length, new AsyncCallback(this.AsyncEndRead), asyncState);
                                asyncState.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                            }
                            while (((asyncResult.CompletedSynchronously && !asyncState.RequestCompleted) && !base.IsCompletedInternally) && stream.CanRead);
                        }
                    }
                    else
                    {
                        asyncState.SetComplete();
                        if (!base.IsCompletedInternally)
                        {
                            this.SaveNextChange(asyncState);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (this.HandleFailure(asyncState, exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    this.HandleCompleted(asyncState);
                }
            }

            private void AsyncEndWrite(IAsyncResult asyncResult)
            {
                Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult.IsCompleted");
                PerRequest request = (asyncResult == null) ? null : (asyncResult.AsyncState as PerRequest);
                try
                {
                    CompleteCheck(request, InternalError.InvalidEndWriteCompleted);
                    request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                    EqualRefCheck(this.request, request, InternalError.InvalidEndWrite);
                    PerRequest.ContentStream requestContentStream = request.RequestContentStream;
                    Util.NullCheck<PerRequest.ContentStream>(requestContentStream, InternalError.InvalidEndWriteStream);
                    Util.NullCheck<Stream>(requestContentStream.Stream, InternalError.InvalidEndWriteStream);
                    Stream stream2 = Util.NullCheck<Stream>(request.RequestStream, InternalError.InvalidEndWriteStream);
                    Util.DebugInjectFault("SaveAsyncResult::AsyncEndWrite_BeforeEndWrite");
                    stream2.EndWrite(asyncResult);
                    if (!asyncResult.CompletedSynchronously)
                    {
                        Util.DebugInjectFault("SaveAsyncResult::AsyncEndWrite_BeforeBeginRead");
                        Stream stream = requestContentStream.Stream;
                        asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream.BeginRead), request.RequestContentBuffer, 0, request.RequestContentBuffer.Length, new AsyncCallback(this.AsyncRequestContentEndRead), request);
                        request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                    }
                }
                catch (Exception exception)
                {
                    if (this.HandleFailure(request, exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    this.HandleCompleted(request);
                }
            }

            private void AsyncRequestContentEndRead(IAsyncResult asyncResult)
            {
                Debug.Assert((asyncResult != null) && asyncResult.IsCompleted, "asyncResult.IsCompleted");
                PerRequest request = (asyncResult == null) ? null : (asyncResult.AsyncState as PerRequest);
                try
                {
                    CompleteCheck(request, InternalError.InvalidEndReadCompleted);
                    request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                    EqualRefCheck(this.request, request, InternalError.InvalidEndRead);
                    PerRequest.ContentStream requestContentStream = request.RequestContentStream;
                    Util.NullCheck<PerRequest.ContentStream>(requestContentStream, InternalError.InvalidEndReadStream);
                    Util.NullCheck<Stream>(requestContentStream.Stream, InternalError.InvalidEndReadStream);
                    Stream stream2 = Util.NullCheck<Stream>(request.RequestStream, InternalError.InvalidEndReadStream);
                    Util.DebugInjectFault("SaveAsyncResult::AsyncRequestContentEndRead_BeforeEndRead");
                    int num = requestContentStream.Stream.EndRead(asyncResult);
                    if (0 < num)
                    {
                        bool flag = request.RequestContentBufferValidLength == -1;
                        request.RequestContentBufferValidLength = num;
                        if (!asyncResult.CompletedSynchronously || flag)
                        {
                            do
                            {
                                Util.DebugInjectFault("SaveAsyncResult::AsyncRequestContentEndRead_BeforeBeginWrite");
                                asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream2.BeginWrite), request.RequestContentBuffer, 0, request.RequestContentBufferValidLength, new AsyncCallback(this.AsyncEndWrite), request);
                                request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                                if (!((!asyncResult.CompletedSynchronously || request.RequestCompleted) || base.IsCompletedInternally))
                                {
                                    Util.DebugInjectFault("SaveAsyncResult::AsyncRequestContentEndRead_BeforeBeginRead");
                                    Stream stream = requestContentStream.Stream;
                                    asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(stream.BeginRead), request.RequestContentBuffer, 0, request.RequestContentBuffer.Length, new AsyncCallback(this.AsyncRequestContentEndRead), request);
                                    request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                                }
                            }
                            while (((asyncResult.CompletedSynchronously && !request.RequestCompleted) && !base.IsCompletedInternally) && (request.RequestContentBufferValidLength > 0));
                        }
                    }
                    else
                    {
                        //request.RequestContentBufferValidLength = 0;
                        //request.RequestStream = null;
                        //stream2.Close();
                        //HttpWebRequest local1 = Util.NullCheck<HttpWebRequest>(request.Request, InternalError.InvalidEndWriteRequest);
                        //asyncResult = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(local1, (IntPtr) local1.BeginGetResponse), new AsyncCallback(this.AsyncEndGetResponse), request);
                        //request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;

                        request.RequestContentBufferValidLength = 0;
                        request.RequestStream = null;
                        stream2.Close();

                        HttpWebRequest httpWebRequest = Util.NullCheck(request.Request, InternalError.InvalidEndWriteRequest);
                        asyncResult = BaseAsyncResult.InvokeAsync(httpWebRequest.BeginGetResponse, this.AsyncEndGetResponse, request);
                        request.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously; 
                    }
                }
                catch (Exception exception)
                {
                    if (this.HandleFailure(request, exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    this.HandleCompleted(request);
                }
            }

            internal void BatchBeginRequest(bool replaceOnUpdate)
            {
                PerRequest pereq = null;
                try
                {
                    MemoryStream memory = this.GenerateBatchRequest(replaceOnUpdate);
                    if (null != memory)
                    {
                        HttpWebRequest httpWebRequest = this.CreateBatchRequest(memory);
                        this.Abortable = httpWebRequest;

                        this.request = pereq = new PerRequest();
                        pereq.Request = httpWebRequest;
                        pereq.RequestContentStream = new PerRequest.ContentStream(memory, true);

                        this.httpWebResponseStream = new MemoryStream();

                        IAsyncResult asyncResult = BaseAsyncResult.InvokeAsync(httpWebRequest.BeginGetRequestStream, this.AsyncEndGetRequestStream, pereq);
                        pereq.RequestCompletedSynchronously &= asyncResult.CompletedSynchronously;
                    }
                    else
                    {
                        Debug.Assert(this.CompletedSynchronously, "completedSynchronously");
                        Debug.Assert(this.IsCompletedInternally, "completed");
                    }
                }
                catch (Exception e)
                {
                    this.HandleFailure(pereq, e);
                    throw;
                }
                finally
                {
                    this.HandleCompleted(pereq);
                }

                Debug.Assert((this.CompletedSynchronously && this.IsCompleted) || !this.CompletedSynchronously, "sync without complete");
            }

            internal void BeginNextChange(bool replaceOnUpdate)
            {
                 Debug.Assert(!base.IsCompletedInternally, "why being called if already completed?");
    PerRequest state = null;
    IAsyncResult result = null;
    do
    {
        HttpWebRequest request2 = null;
        HttpWebResponse response = null;
        try
        {
            if (null != this.request)
            {
                base.SetCompleted();
                Error.ThrowInternalError(InternalError.InvalidBeginNextChange);
            }
            base.Abortable = request2 = this.CreateNextRequest(replaceOnUpdate);
            if ((request2 != null) || (this.entryIndex < this.ChangedEntries.Count))
            {
                if (this.ChangedEntries[this.entryIndex].ContentGeneratedForSave)
                {
                    Debug.Assert(this.ChangedEntries[this.entryIndex] is LinkDescriptor, "only expected RelatedEnd to presave");
                    Debug.Assert((this.ChangedEntries[this.entryIndex].State == EntityStates.Added) || (this.ChangedEntries[this.entryIndex].State == EntityStates.Modified), "only expected added to presave");
                }
                else
                {
                    PerRequest.ContentStream stream = this.CreateChangeData(this.entryIndex, false);
                    if (this.executeAsync)
                    {
                        this.request = state = new PerRequest();
                        state.Request = request2;
                        if ((stream == null) || (null == stream.Stream))
                        {
                            result = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(request2.BeginGetResponse), new AsyncCallback(this.AsyncEndGetResponse), state);
                        }
                        else
                        {
                            if (stream.IsKnownMemoryStream)
                            {
                                request2.ContentLength = stream.Stream.Length - stream.Stream.Position;
                            }
                            state.RequestContentStream = stream;
                            result = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(request2.BeginGetRequestStream), new AsyncCallback(this.AsyncEndGetRequestStream), state);
                        }
                        state.RequestCompletedSynchronously &= result.CompletedSynchronously;
                        base.CompletedSynchronously &= result.CompletedSynchronously;
                    }
                }
            }
            else
            {
                base.SetCompleted();
                if (base.CompletedSynchronously)
                {
                    this.HandleCompleted(state);
                }
            }
        }
        catch (InvalidOperationException exception)
        {
            WebUtil.GetHttpWebResponse(exception, ref response);
            this.HandleOperationException(exception, response);
            this.HandleCompleted(state);
        }
        finally
        {
            if (null != response)
            {
                response.Close();
            }
        }
    }
    while (((state == null) || ((state.RequestCompleted && (result != null)) && result.CompletedSynchronously)) && !base.IsCompletedInternally);
    Debug.Assert(this.executeAsync || base.CompletedSynchronously, "sync !CompletedSynchronously");
    Debug.Assert((base.CompletedSynchronously && base.IsCompleted) || !base.CompletedSynchronously, "sync without complete");
    Debug.Assert((this.entryIndex < this.ChangedEntries.Count) || this.ChangedEntries.All<Descriptor>(delegate (Descriptor o) {
        return o.ContentGeneratedForSave;
    }), "didn't generate content for all entities/links");

}

            private HttpWebRequest CheckAndProcessMediaEntryPost(EntityDescriptor entityDescriptor)
            {
                ClientType type = ClientType.Create(entityDescriptor.Entity.GetType());
                if (!(type.IsMediaLinkEntry || entityDescriptor.IsMediaLinkEntry))
                {
                    return null;
                }
                if ((type.MediaDataMember == null) && (entityDescriptor.SaveStream == null))
                {
                    throw Error.InvalidOperation(Strings.Context_MLEWithoutSaveStream(type.ElementTypeName));
                }
                Debug.Assert(((type.MediaDataMember != null) && (entityDescriptor.SaveStream == null)) || ((type.MediaDataMember == null) && (entityDescriptor.SaveStream != null)), "Only one way of specifying the MR content is allowed.");
                HttpWebRequest mediaResourceRequest = this.CreateMediaResourceRequest(entityDescriptor.GetResourceUri(this.Context.baseUriWithSlash, false), "POST", type.MediaDataMember == null);
                if (type.MediaDataMember != null)
                {
                    if (type.MediaDataMember.MimeTypeProperty == null)
                    {
                        mediaResourceRequest.ContentType = "application/octet-stream";
                    }
                    else
                    {
                        object obj2 = type.MediaDataMember.MimeTypeProperty.GetValue(entityDescriptor.Entity);
                        string str = (obj2 != null) ? obj2.ToString() : null;
                        if (string.IsNullOrEmpty(str))
                        {
                            throw Error.InvalidOperation(Strings.Context_NoContentTypeForMediaLink(type.ElementTypeName, type.MediaDataMember.MimeTypeProperty.PropertyName));
                        }
                        mediaResourceRequest.ContentType = str;
                    }
                    object propertyValue = type.MediaDataMember.GetValue(entityDescriptor.Entity);
                    if (propertyValue == null)
                    {
                        mediaResourceRequest.ContentLength = 0L;
                        this.mediaResourceRequestStream = null;
                    }
                    else
                    {
                        byte[] bytes = propertyValue as byte[];
                        if (bytes == null)
                        {
                            string str2;
                            Encoding encoding;
                            HttpProcessUtility.ReadContentType(mediaResourceRequest.ContentType, out str2, out encoding);
                            if (encoding == null)
                            {
                                encoding = Encoding.UTF8;
                                mediaResourceRequest.ContentType = mediaResourceRequest.ContentType + ";charset=UTF-8";
                            }
                            bytes = encoding.GetBytes(ClientConvert.ToString(propertyValue, false));
                        }
                        mediaResourceRequest.ContentLength = bytes.Length;
                        this.mediaResourceRequestStream = new MemoryStream(bytes, 0, bytes.Length, false, true);
                    }
                }
                else
                {
                    this.SetupMediaResourceRequest(mediaResourceRequest, entityDescriptor);
                }
                entityDescriptor.State = EntityStates.Modified;
                return mediaResourceRequest;
            }

            private HttpWebRequest CheckAndProcessMediaEntryPut(EntityDescriptor box)
            {
                if (box.SaveStream == null)
                {
                    return null;
                }
                Uri editMediaResourceUri = box.GetEditMediaResourceUri(this.Context.baseUriWithSlash);
                if (editMediaResourceUri == null)
                {
                    throw Error.InvalidOperation(Strings.Context_SetSaveStreamWithoutEditMediaLink);
                }
                HttpWebRequest mediaResourceRequest = this.CreateMediaResourceRequest(editMediaResourceUri, "PUT", true);
                this.SetupMediaResourceRequest(mediaResourceRequest, box);
                if (box.StreamETag != null)
                {
                    mediaResourceRequest.Headers.Set(HttpRequestHeader.IfMatch, box.StreamETag);
                }
                return mediaResourceRequest;
            }

            private static void CompleteCheck(PerRequest value, InternalError errorcode)
            {
                if ((value == null) || value.RequestCompleted)
                {
                    Error.ThrowInternalError(errorcode);
                }
            }

            protected override void CompletedRequest()
            {
                this.buildBatchBuffer = null;
                if (null != this.buildBatchWriter)
                {
                    Debug.Assert(!DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.Batch), "should be non-batch");
                    this.HandleOperationEnd();
                    this.buildBatchWriter.WriteLine("--{0}--", this.batchBoundary);
                    this.buildBatchWriter.Flush();
                    Debug.Assert(object.ReferenceEquals(this.httpWebResponseStream, this.buildBatchWriter.BaseStream), "expected different stream");
                    this.httpWebResponseStream.Position = 0L;
                    this.buildBatchWriter = null;
                    this.responseBatchStream = new BatchStream(this.httpWebResponseStream, this.batchBoundary, HttpProcessUtility.EncodingUtf8NoPreamble, false);
                }
            }

            private HttpWebRequest CreateBatchRequest(MemoryStream memory)
            {
                Uri requestUri = Util.CreateUri(this.Context.baseUriWithSlash, Util.CreateUri("$batch", UriKind.Relative));
                string contentType = "multipart/mixed; boundary=" + this.batchBoundary;
                HttpWebRequest request = this.Context.CreateRequest(requestUri, "POST", false, contentType, Util.DataServiceVersion1, false);
                request.ContentLength = memory.Length - memory.Position;
                return request;
            }

            private PerRequest.ContentStream CreateChangeData(int index, bool newline)
            {
                Descriptor descriptor = this.ChangedEntries[index];
                Debug.Assert(!descriptor.ContentGeneratedForSave, "already saved entity/link");
                if (descriptor.IsResource)
                {
                    EntityDescriptor box = (EntityDescriptor) descriptor;
                    if (this.processingMediaLinkEntry)
                    {
                        Debug.Assert(this.processingMediaLinkEntryPut || (descriptor.State == EntityStates.Modified), "We should have modified the MLE state to Modified when we've created the MR POST request.");
                        Debug.Assert(!this.processingMediaLinkEntryPut || ((descriptor.State == EntityStates.Unchanged) || (descriptor.State == EntityStates.Modified)), "If we're processing MR PUT the entity must be either in Unchanged or Modified state.");
                        Debug.Assert(this.mediaResourceRequestStream != null, "We should have precreated the MR stream already.");
                        return new PerRequest.ContentStream(this.mediaResourceRequestStream, false);
                    }
                    descriptor.ContentGeneratedForSave = true;
                    return new PerRequest.ContentStream(this.Context.CreateRequestData(box, newline), true);
                }
                descriptor.ContentGeneratedForSave = true;
                LinkDescriptor binding = (LinkDescriptor) descriptor;
                if ((EntityStates.Added == binding.State) || ((EntityStates.Modified == binding.State) && (null != binding.Target)))
                {
                    return new PerRequest.ContentStream(this.Context.CreateRequestData(binding, newline), true);
                }
                return null;
            }

            private HttpWebRequest CreateMediaResourceRequest(Uri requestUri, string method, bool sendChunked)
            {
                return this.Context.CreateRequest(requestUri, method, false, "*/*", Util.DataServiceVersion1, sendChunked, HttpStack.ClientHttp);
            }

            private HttpWebRequest CreateNextRequest(bool replaceOnUpdate)
            {
                EntityDescriptor descriptor;
                if (!this.processingMediaLinkEntry)
                {
                    this.entryIndex++;
                }
                else
                {
                    Debug.Assert(this.ChangedEntries[this.entryIndex].IsResource, "Only resources can have MR's.");
                    descriptor = (EntityDescriptor) this.ChangedEntries[this.entryIndex];
                    if (this.processingMediaLinkEntryPut && (EntityStates.Unchanged == descriptor.State))
                    {
                        descriptor.ContentGeneratedForSave = true;
                        this.entryIndex++;
                    }
                    this.processingMediaLinkEntry = false;
                    this.processingMediaLinkEntryPut = false;
                    descriptor.CloseSaveStream();
                }
                if (this.entryIndex < this.ChangedEntries.Count)
                {
                    HttpWebRequest request;
                    Descriptor descriptor2 = this.ChangedEntries[this.entryIndex];
                    if (!descriptor2.IsResource)
                    {
                        return this.Context.CreateRequest((LinkDescriptor) descriptor2);
                    }
                    descriptor = (EntityDescriptor) descriptor2;
                    if (((EntityStates.Unchanged == descriptor2.State) || (EntityStates.Modified == descriptor2.State)) && (null != (request = this.CheckAndProcessMediaEntryPut(descriptor))))
                    {
                        this.processingMediaLinkEntry = true;
                        this.processingMediaLinkEntryPut = true;
                        return request;
                    }
                    if ((EntityStates.Added == descriptor2.State) && (null != (request = this.CheckAndProcessMediaEntryPost(descriptor))))
                    {
                        this.processingMediaLinkEntry = true;
                        this.processingMediaLinkEntryPut = false;
                        return request;
                    }
                    Debug.Assert(!this.processingMediaLinkEntry || (descriptor2.State == EntityStates.Modified), "!this.processingMediaLinkEntry || entry.State == EntityStates.Modified");
                    return this.Context.CreateRequest(descriptor, descriptor2.State, replaceOnUpdate);
                }
                return null;
            }

            internal System.Data.Services.Client.DataServiceResponse EndRequest()
            {
                foreach (EntityDescriptor box in this.ChangedEntries.Where(e => e.IsResource).Cast<EntityDescriptor>())
                {
                    box.CloseSaveStream();
                }

                if ((null != this.responseBatchStream) || (null != this.httpWebResponseStream))
                {
                    this.HandleBatchResponse();
                }

                return this.DataServiceResponse;
            }

            private static void EqualRefCheck(PerRequest actual, PerRequest expected, InternalError errorcode)
            {
                if (!object.ReferenceEquals(actual, expected))
                {
                    Error.ThrowInternalError(errorcode);
                }
            }

            private MemoryStream GenerateBatchRequest(bool replaceOnUpdate)
            {
                 this.changesetBoundary = null;
                if (null != this.Queries)
                {
                }
                else if (0 == this.ChangedEntries.Count)
                {
                    this.DataServiceResponse = new DataServiceResponse(null, (int)WebExceptionStatus.Success, this.Responses, true );
                    this.SetCompleted();
                    return null;
                }
                else
                {
                    this.changesetBoundary = XmlConstants.HttpMultipartBoundaryChangeSet + "_" + Guid.NewGuid().ToString();
                }

                MemoryStream memory = new MemoryStream();
                StreamWriter text = new StreamWriter(memory);     

#if TESTUNIXNEWLINE
                text.NewLine = NewLine;
#endif

                if (null != this.Queries)
                {
                    for (int i = 0; i < this.Queries.Length; ++i)
                    {
                        Uri requestUri = Util.CreateUri(this.Context.baseUriWithSlash, this.Queries[i].QueryComponents.Uri);

                        Debug.Assert(null != requestUri, "request uri is null");
                        Debug.Assert(requestUri.IsAbsoluteUri, "request uri is not absolute uri");

                        text.WriteLine("--{0}", this.batchBoundary);
                        WriteOperationRequestHeaders(text, XmlConstants.HttpMethodGet, requestUri.AbsoluteUri, this.Queries[i].QueryComponents.Version);
                        text.WriteLine();
                    }
                }
                else if (0 < this.ChangedEntries.Count)
                {
                    text.WriteLine("--{0}", this.batchBoundary);
                    text.WriteLine("{0}: {1}; boundary={2}", XmlConstants.HttpContentType, XmlConstants.MimeMultiPartMixed, this.changesetBoundary);
                    text.WriteLine();

                    for (int i = 0; i < this.ChangedEntries.Count; ++i)
                    {
                        #region validate changeset boundary starts on newline
#if DEBUG
                        {
                            text.Flush();
                            for (int kk = 0; kk < NewLine.Length; ++kk)
                            {
                                Debug.Assert((char)memory.GetBuffer()[memory.Length - (NewLine.Length - kk)] == NewLine[kk], "boundary didn't start with newline");
                            }
                        }
#endif
                        #endregion

                        Descriptor entry = this.ChangedEntries[i];
                        if (entry.ContentGeneratedForSave)
                        {
                            continue;
                        }

                        text.WriteLine("--{0}", this.changesetBoundary);

                        EntityDescriptor entityDescriptor = entry as EntityDescriptor;
                        if (entry.IsResource)
                        {
                            if (entityDescriptor.State == EntityStates.Added)
                            {
                                ClientType type = ClientType.Create(entityDescriptor.Entity.GetType());
                                if (type.IsMediaLinkEntry || entityDescriptor.IsMediaLinkEntry)
                                {
                                    throw Error.NotSupported(Strings.Context_BatchNotSupportedForMediaLink);
                                }
                            }
                            else if (entityDescriptor.State == EntityStates.Unchanged || entityDescriptor.State == EntityStates.Modified)
                            {
                                if (entityDescriptor.SaveStream != null)
                                {
                                    throw Error.NotSupported(Strings.Context_BatchNotSupportedForMediaLink);
                                }
                            }
                        }

                        PerRequest.ContentStream contentStream = this.CreateChangeData(i, true);
                        MemoryStream stream = null;
                        if (null != contentStream)
                        {
                            Debug.Assert(contentStream.IsKnownMemoryStream, "Batch requests don't support MRs yet");
                            stream = contentStream.Stream as MemoryStream;
                        }

                        if (entry.IsResource)
                        {
                            this.Context.CreateRequestBatch(entityDescriptor, text, replaceOnUpdate);
                        }
                        else
                        {
                            this.Context.CreateRequestBatch((LinkDescriptor)entry, text);
                        }

                        byte[] buffer = null;
                        int bufferOffset = 0, bufferLength = 0;
                        if (null != stream)
                        {
                            buffer = stream.GetBuffer();
                            bufferOffset = checked((int)stream.Position);
                            bufferLength = checked((int)stream.Length) - bufferOffset;
                        }

                        if (0 < bufferLength)
                        {
                            text.WriteLine("{0}: {1}", XmlConstants.HttpContentLength, bufferLength);
                        }

                        text.WriteLine();

                        if (0 < bufferLength)
                        {
                            text.Flush();
                            text.BaseStream.Write(buffer, bufferOffset, bufferLength);
                        }
                    }

                    #region validate changeset boundary ended with newline
#if DEBUG
                    {
                        text.Flush();

                        for (int kk = 0; kk < NewLine.Length; ++kk)
                        {
                            Debug.Assert((char)memory.GetBuffer()[memory.Length - (NewLine.Length - kk)] == NewLine[kk], "post CreateRequest boundary didn't start with newline");
                        }
                    }
#endif
                    #endregion

                   text.WriteLine("--{0}--", this.changesetBoundary);
                }

                text.WriteLine("--{0}--", this.batchBoundary);

                text.Flush();
                Debug.Assert(Object.ReferenceEquals(text.BaseStream, memory), "should be same");
                Debug.Assert(this.ChangedEntries.All(o => o.ContentGeneratedForSave), "didn't generated content for all entities/links");

                #region Validate batch format
#if DEBUG
                int testGetCount = 0;
                int testOpCount = 0;
                int testBeginSetCount = 0;
                int testEndSetCount = 0;
                memory.Position = 0;
                BatchStream testBatch = new BatchStream(memory, this.batchBoundary, HttpProcessUtility.EncodingUtf8NoPreamble, true);
                while (testBatch.MoveNext())
                {
                    switch (testBatch.State)
                    {
                        case BatchStreamState.StartBatch:
                        case BatchStreamState.EndBatch:
                        default:
                            Debug.Assert(false, "shouldn't happen");
                            break;

                        case BatchStreamState.Get:
                            testGetCount++;
                            break;

                        case BatchStreamState.BeginChangeSet:
                            testBeginSetCount++;
                            break;
                        case BatchStreamState.EndChangeSet:
                            testEndSetCount++;
                            break;
                        case BatchStreamState.Post:
                        case BatchStreamState.Put:
                        case BatchStreamState.Delete:
                        case BatchStreamState.Merge:
                            testOpCount++;
                            break;
                    }
                }

                Debug.Assert((null == this.Queries && 1 == testBeginSetCount) || (0 == testBeginSetCount), "more than one BeginChangeSet");
                Debug.Assert(testBeginSetCount == testEndSetCount, "more than one EndChangeSet");
                Debug.Assert((null == this.Queries && testGetCount == 0) || this.Queries.Length == testGetCount, "too many get count");
                Debug.Assert(BatchStreamState.EndBatch == testBatch.State, "should have ended propertly");
#endif
                #endregion

                this.changesetBoundary = null;

                memory.Position = 0;
                return memory;
            }

            private void HandleBatchResponse()
            {
                Func<Stream> getResponseStream = null;
                string batchBoundary = this.batchBoundary;
                Encoding encoding = Encoding.UTF8;
                Dictionary<string, string> headers = null;
                Exception innerException = null;
                try
                {
                    if (DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.Batch))
                    {
                        if ((this.batchResponse == null) || (HttpStatusCode.NoContent == this.batchResponse.StatusCode))
                        {
                            throw Error.InvalidOperation(Strings.Batch_ExpectedResponse(1));
                        }
                        //headers = WebUtil.WrapResponseHeaders(this.batchResponse);
                        //if (getResponseStream == null)
                        //{
                        //    getResponseStream = new Func<Stream>(this, (IntPtr) this.<HandleBatchResponse>b__23);
                        //}
                        //DataServiceContext.HandleResponse(this.batchResponse.StatusCode, this.batchResponse.Headers["DataServiceVersion"], getResponseStream, true);
                        //if (!BatchStream.GetBoundaryAndEncodingFromMultipartMixedContentType(this.batchResponse.ContentType, out batchBoundary, out encoding))
                        headers = WebUtil.WrapResponseHeaders(this.batchResponse);
                        HandleResponse(
                            this.batchResponse.StatusCode,
                            this.batchResponse.Headers[XmlConstants.HttpDataServiceVersion],
                            delegate() { return this.httpWebResponseStream; },
                            true);

                        if (!BatchStream.GetBoundaryAndEncodingFromMultipartMixedContentType(this.batchResponse.ContentType, out batchBoundary, out encoding))
                        {
                            string mime;
                            Exception inner = null;
                            HttpProcessUtility.ReadContentType(this.batchResponse.ContentType, out mime, out encoding);
                            if (String.Equals(XmlConstants.MimeTextPlain, mime))
                            {
                                inner = GetResponseText(this.batchResponse.GetResponseStream, this.batchResponse.StatusCode);
                            }

                            throw Error.InvalidOperation(Strings.Batch_ExpectedContentType(this.batchResponse.ContentType), inner);
                        }
                        if (null == this.httpWebResponseStream)
                        {
                            Error.ThrowBatchExpectedResponse(InternalError.NullResponseStream);
                        }
                        this.DataServiceResponse = new System.Data.Services.Client.DataServiceResponse(headers, (int) this.batchResponse.StatusCode, this.Responses, true);
                    }
                    bool flag = true;
                    BatchStream batch = null;
                    try
                    {
                        batch = this.responseBatchStream ?? new BatchStream(this.httpWebResponseStream, batchBoundary, encoding, false);
                        this.httpWebResponseStream = null;
                        this.responseBatchStream = null;
                        IEnumerable<OperationResponse> enumerable = this.HandleBatchResponse(batch);
                        if (DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.Batch) && (null != this.Queries))
                        {
                            flag = false;
                            this.responseBatchStream = batch;
                            this.DataServiceResponse = new System.Data.Services.Client.DataServiceResponse((Dictionary<string, string>) this.DataServiceResponse.BatchHeaders, this.DataServiceResponse.BatchStatusCode, enumerable, true);
                        }
                        else
                        {
                            foreach (ChangeOperationResponse response in enumerable)
                            {
                                if ((innerException == null) && (response.Error != null))
                                {
                                    innerException = response.Error;
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (flag && (null != batch))
                        {
                            batch.Close();
                        }
                    }
                }
                catch (InvalidOperationException exception3)
                {
                    innerException = exception3;
                }
                if (innerException != null)
                {
                    if (this.DataServiceResponse == null)
                    {
                        int statusCode = (this.batchResponse == null) ? ((int) HttpStatusCode.InternalServerError) : ((int) this.batchResponse.StatusCode);
                        this.DataServiceResponse = new System.Data.Services.Client.DataServiceResponse(headers, statusCode, null, DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.Batch));
                    }
                    throw new DataServiceRequestException(Strings.DataServiceException_GeneralError, innerException, this.DataServiceResponse);
                }
            }

            private IEnumerable<OperationResponse> HandleBatchResponse(BatchStream batch)
            {
               
                if (!batch.CanRead)
                {
                    yield break;
                }

                string contentType;
                string location;
                string etag;

                Uri editLink = null;

                HttpStatusCode status;
                int changesetIndex = 0;
                int queryCount = 0;
                int operationCount = 0;
                this.entryIndex = 0;
                while (batch.MoveNext())
                {
                    var contentHeaders = batch.ContentHeaders; 

                    Descriptor entry;
                    switch (batch.State)
                    {
                        #region BeginChangeSet
                        case BatchStreamState.BeginChangeSet:
                            if ((IsFlagSet(this.options, SaveChangesOptions.Batch) && (0 != changesetIndex)) ||
                                (0 != operationCount))
                            {   
                                Error.ThrowBatchUnexpectedContent(InternalError.UnexpectedBeginChangeSet);
                            }

                            break;
                        #endregion

                        #region EndChangeSet
                        case BatchStreamState.EndChangeSet:
                            changesetIndex++;
                            operationCount = 0;
                            break;
                        #endregion

                        #region GetResponse
                        case BatchStreamState.GetResponse:
                            Debug.Assert(0 == operationCount, "missing an EndChangeSet 2");

                            contentHeaders.TryGetValue(XmlConstants.HttpContentType, out contentType);
                            status = (HttpStatusCode)(-1);

                            Exception ex = null;
                            QueryOperationResponse qresponse = null;
                            try
                            {
                                status = batch.GetStatusCode();

                                ex = HandleResponse(status, batch.GetResponseVersion(), batch.GetContentStream, false);
                                if (null == ex)
                                {
                                    DataServiceRequest query = this.Queries[queryCount];
                                    MaterializeAtom materializer = DataServiceRequest.Materialize(this.Context, query.QueryComponents, null, contentType, batch.GetContentStream());
                                    qresponse = QueryOperationResponse.GetInstance(query.ElementType, contentHeaders, query, materializer);
                                }
                            }
                            catch (ArgumentException e)
                            {
                                ex = e;
                            }
                            catch (FormatException e)
                            {
                                ex = e;
                            }
                            catch (InvalidOperationException e)
                            {
                                ex = e;
                            }

                            if (null == qresponse)
                            {
                                if (null != this.Queries)
                                {
                                     DataServiceRequest query = this.Queries[queryCount];

                                    if (this.Context.ignoreResourceNotFoundException && status == HttpStatusCode.NotFound)
                                    {
                                        qresponse = QueryOperationResponse.GetInstance(query.ElementType, contentHeaders, query, MaterializeAtom.EmptyResults);
                                    }
                                    else
                                    {
                                        qresponse = QueryOperationResponse.GetInstance(query.ElementType, contentHeaders, query, MaterializeAtom.EmptyResults);
                                        qresponse.Error = ex;
                                    }
                                }
                                else
                                {
                                   throw ex;
                                }
                            }

                            qresponse.StatusCode = (int)status;
                            queryCount++;
                            yield return qresponse;
                            break;
                        #endregion

                        #region ChangeResponse
                        case BatchStreamState.ChangeResponse:

                            HttpStatusCode statusCode = batch.GetStatusCode();
                            Exception error = HandleResponse(statusCode, batch.GetResponseVersion(), batch.GetContentStream, false);
                            int index = this.ValidateContentID(contentHeaders);

                            try
                            {
                                entry = this.ChangedEntries[index];
                                operationCount += this.Context.SaveResultProcessed(entry);

                                if (null != error)
                                {
                                    throw error;
                                }

                                StreamStates streamState = StreamStates.NoStream;
                                if (entry.IsResource)
                                {
                                    EntityDescriptor descriptor = (EntityDescriptor)entry;
                                    streamState = descriptor.StreamState;
#if DEBUG
                                    if (descriptor.StreamState == StreamStates.Added)
                                    {
                                        Debug.Assert(
                                            statusCode == HttpStatusCode.Created && entry.State == EntityStates.Modified && descriptor.IsMediaLinkEntry,
                                            "statusCode == HttpStatusCode.Created && entry.State == EntityStates.Modified && descriptor.IsMediaLinkEntry -- Processing Post MR");
                                    }
                                    else if (descriptor.StreamState == StreamStates.Modified)
                                    {
                                        Debug.Assert(
                                            statusCode == HttpStatusCode.NoContent && descriptor.IsMediaLinkEntry,
                                            "statusCode == HttpStatusCode.NoContent && descriptor.IsMediaLinkEntry -- Processing Put MR");
                                    }
#endif
                                }

                                if (streamState == StreamStates.Added || entry.State == EntityStates.Added)
                                {
                                    #region Post
                                    if (entry.IsResource)
                                    {
                                        string mime = null;
                                        Encoding postEncoding = null;
                                        contentHeaders.TryGetValue(XmlConstants.HttpContentType, out contentType);
                                        contentHeaders.TryGetValue(XmlConstants.HttpResponseLocation, out location);
                                        contentHeaders.TryGetValue(XmlConstants.HttpResponseETag, out etag);
                                        EntityDescriptor entityDescriptor = (EntityDescriptor)entry;

                                        if (location != null)
                                        {
                                            editLink = Util.CreateUri(location, UriKind.Absolute);
                                        }
                                        else
                                        {
                                            throw Error.NotSupported(Strings.Deserialize_NoLocationHeader);
                                        }

                                        Stream stream = batch.GetContentStream();
                                        if (null != stream)
                                        {
                                            HttpProcessUtility.ReadContentType(contentType, out mime, out postEncoding);
                                            if (!String.Equals(XmlConstants.MimeApplicationAtom, mime, StringComparison.OrdinalIgnoreCase))
                                            {
                                                throw Error.InvalidOperation(Strings.Deserialize_UnknownMimeTypeSpecified(mime));
                                            }

                                            XmlReader reader = XmlUtil.CreateXmlReader(stream, postEncoding);
                                            QueryComponents qc = new QueryComponents(null, Util.DataServiceVersionEmpty, entityDescriptor.Entity.GetType(), null, null);
                                            EntityDescriptor descriptor = (EntityDescriptor)entry;
                                            MergeOption mergeOption = MergeOption.OverwriteChanges;

                                            if (descriptor.StreamState == StreamStates.Added)
                                            {
                                                mergeOption = MergeOption.PreserveChanges;
                                                Debug.Assert(descriptor.State == EntityStates.Modified, "The MLE state must be Modified.");
                                            }

                                            try
                                            {
                                                using (MaterializeAtom atom = new MaterializeAtom(this.Context, reader, qc, null, mergeOption))
                                                {
                                                    this.Context.HandleResponsePost(entityDescriptor, atom, editLink, etag);
                                                }
                                            }
                                            finally
                                            {
                                                if (descriptor.StreamState == StreamStates.Added)
                                                {
                                                   Debug.Assert(descriptor.State == EntityStates.Unchanged, "The materializer should always set the entity state to Unchanged.");
                                                    descriptor.State = EntityStates.Modified;

                                                    descriptor.StreamState = StreamStates.NoStream;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            this.Context.HandleResponsePost(entityDescriptor, null, editLink, etag);
                                        }
                                    }
                                    else
                                    {
                                        HandleResponsePost((LinkDescriptor)entry);
                                    }
                                    #endregion
                                }
                                else if (streamState == StreamStates.Modified || entry.State == EntityStates.Modified)
                                {
                                    #region Put, Merge
                                    contentHeaders.TryGetValue(XmlConstants.HttpResponseETag, out etag);
                                    HandleResponsePut(entry, etag);
                                    #endregion
                                }
                                else if (entry.State == EntityStates.Deleted)
                                {
                                    #region Delete
                                    this.Context.HandleResponseDelete(entry);
                                    #endregion
                                }

                           }
                            catch (Exception e)
                            {
                                this.ChangedEntries[index].SaveError = e;
                                error = e;
                            }

                            ChangeOperationResponse changeOperationResponse = 
                                new ChangeOperationResponse(contentHeaders, this.ChangedEntries[index]);
                            changeOperationResponse.StatusCode = (int)statusCode;
                            if (error != null)
                            {
                                changeOperationResponse.Error = error;
                            }

                            this.Responses.Add(changeOperationResponse);
                            operationCount++;
                            this.entryIndex++;
                            yield return changeOperationResponse;
                            break;
                        #endregion

                        default:
                            Error.ThrowBatchExpectedResponse(InternalError.UnexpectedBatchState);
                            break;
                    }
                }

                Debug.Assert(batch.State == BatchStreamState.EndBatch, "unexpected batch state");

               if ((null == this.Queries && 
                    (0 == changesetIndex || 
                     0 < queryCount || 
                     this.ChangedEntries.Any(o => o.ContentGeneratedForSave && 0 == o.SaveResultWasProcessed) &&
                     (!IsFlagSet(this.options, SaveChangesOptions.Batch) || null == this.ChangedEntries.FirstOrDefault(o => null != o.SaveError)))) ||
                    (null != this.Queries && queryCount != this.Queries.Length))
                {
                    throw Error.InvalidOperation(Strings.Batch_IncompleteResponseCount);
                }

                batch.Dispose();
            }

            private void HandleCompleted(PerRequest pereq)
            {
                if (null != pereq)
                {
                    base.CompletedSynchronously &= pereq.RequestCompletedSynchronously;
                    if (pereq.RequestCompleted)
                    {
                        Interlocked.CompareExchange<PerRequest>(ref this.request, null, pereq);
                        if (DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.Batch))
                        {
                            Interlocked.CompareExchange<HttpWebResponse>(ref this.batchResponse, pereq.HttpWebResponse, null);
                            pereq.HttpWebResponse = null;
                        }
                        pereq.Dispose();
                    }
                }
                base.HandleCompleted();
            }

            private bool HandleFailure(PerRequest pereq, Exception e)
            {
                if (null != pereq)
                {
                    if (base.IsAborted)
                    {
                        pereq.SetAborted();
                    }
                    else
                    {
                        pereq.SetComplete();
                    }
                }
                return base.HandleFailure(e);
            }

            private void HandleOperationEnd()
            {
                if (this.changesetStarted)
                {
                    Debug.Assert(null != this.buildBatchWriter, "buildBatchWriter");
                    Debug.Assert(null != this.changesetBoundary, "changesetBoundary");
                    this.buildBatchWriter.WriteLine();
                    this.buildBatchWriter.WriteLine("--{0}--", this.changesetBoundary);
                    this.changesetStarted = false;
                }
            }

            private void HandleOperationException(Exception e, HttpWebResponse response)
            {
                if (null != response)
                {
                    this.HandleOperationResponse(response);
                    this.HandleOperationResponseData(response);
                    this.HandleOperationEnd();
                }
                else
                {
                    this.HandleOperationStart();
                    DataServiceContext.WriteOperationResponseHeaders(this.buildBatchWriter, 500);
                    this.buildBatchWriter.WriteLine("{0}: {1}", "Content-Type", "text/plain");
                    this.buildBatchWriter.WriteLine("{0}: {1}", "Content-ID", this.ChangedEntries[this.entryIndex].ChangeOrder);
                    this.buildBatchWriter.WriteLine();
                    this.buildBatchWriter.WriteLine(e.ToString());
                    this.HandleOperationEnd();
                }
                this.request = null;
                if (!DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.ContinueOnError))
                {
                    base.SetCompleted();
                    this.processingMediaLinkEntry = false;
                    this.ChangedEntries[this.entryIndex].ContentGeneratedForSave = true;
                }
            }

            private void HandleOperationResponse(HttpWebResponse response)
            {
                this.HandleOperationStart();
                Descriptor descriptor = this.ChangedEntries[this.entryIndex];
                if (descriptor.IsResource)
                {
                    EntityDescriptor descriptor2 = (EntityDescriptor) descriptor;
                    if ((descriptor.State == EntityStates.Added) || (((descriptor.State == EntityStates.Modified) && this.processingMediaLinkEntry) && !this.processingMediaLinkEntryPut))
                    {
                        string location = response.Headers["Location"];
                        if (WebUtil.SuccessStatusCode(response.StatusCode))
                        {
                            if (null == location)
                            {
                                throw Error.NotSupported(Strings.Deserialize_NoLocationHeader);
                            }
                            this.Context.AttachLocation(descriptor2.Entity, location);
                        }
                    }
                    if (this.processingMediaLinkEntry)
                    {
                        if (!WebUtil.SuccessStatusCode(response.StatusCode))
                        {
                            this.processingMediaLinkEntry = false;
                            if (!this.processingMediaLinkEntryPut)
                            {
                                Debug.Assert(descriptor.State == EntityStates.Modified, "Entity state should be set to Modified once we've sent the POST MR");
                                descriptor.State = EntityStates.Added;
                                this.processingMediaLinkEntryPut = false;
                            }
                            descriptor.ContentGeneratedForSave = true;
                        }
                        else if (response.StatusCode == HttpStatusCode.Created)
                        {
                            descriptor2.ETag = response.Headers["ETag"];
                        }
                    }
                }
                DataServiceContext.WriteOperationResponseHeaders(this.buildBatchWriter, (int) response.StatusCode);
                foreach (string str2 in response.Headers.AllKeys)
                {
                    if ("Content-Length" != str2)
                    {
                        this.buildBatchWriter.WriteLine("{0}: {1}", str2, response.Headers[str2]);
                    }
                }
                this.buildBatchWriter.WriteLine("{0}: {1}", "Content-ID", descriptor.ChangeOrder);
                this.buildBatchWriter.WriteLine();
            }

            private void HandleOperationResponseData(HttpWebResponse response)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    if (null != stream)
                    {
                        this.buildBatchWriter.Flush();
                        if (0L == WebUtil.CopyStream(stream, this.buildBatchWriter.BaseStream, ref this.buildBatchBuffer))
                        {
                            this.HandleOperationResponseNoData();
                        }
                    }
                }
            }

            private void HandleOperationResponseNoData()
            {
                Debug.Assert(null != this.buildBatchWriter, "null buildBatchWriter");
                this.buildBatchWriter.Flush();
                MemoryStream baseStream = this.buildBatchWriter.BaseStream as MemoryStream;
                Debug.Assert(null != baseStream, "expected MemoryStream");
                Debug.Assert(this.buildBatchWriter.NewLine == DataServiceContext.NewLine, "mismatch NewLine");
                for (int i = 0; i < DataServiceContext.NewLine.Length; i++)
                {
                    Debug.Assert(baseStream.GetBuffer()[(int) ((IntPtr) (baseStream.Length - (DataServiceContext.NewLine.Length - i)))] == DataServiceContext.NewLine[i], "didn't end with newline");
                }
                Stream stream1 = this.buildBatchWriter.BaseStream;
                stream1.Position -= DataServiceContext.NewLine.Length;
                this.buildBatchWriter.WriteLine("{0}: {1}", "Content-Length", 0);
                this.buildBatchWriter.WriteLine();
            }

            private void HandleOperationStart()
            {
                this.HandleOperationEnd();
                if (null == this.httpWebResponseStream)
                {
                    this.httpWebResponseStream = new MemoryStream();
                }
                if (null == this.buildBatchWriter)
                {
                    this.buildBatchWriter = new StreamWriter(this.httpWebResponseStream);
                }
                if (null == this.changesetBoundary)
                {
                    this.changesetBoundary = "changesetresponse_" + Guid.NewGuid().ToString();
                }
                this.changesetStarted = true;
                this.buildBatchWriter.WriteLine("--{0}", this.batchBoundary);
                this.buildBatchWriter.WriteLine("{0}: {1}; boundary={2}", new object[] { "Content-Type", "multipart/mixed", this.changesetBoundary });
                this.buildBatchWriter.WriteLine();
                this.buildBatchWriter.WriteLine("--{0}", this.changesetBoundary);
            }

            private void SaveNextChange(PerRequest pereq)
            {
                Debug.Assert(this.executeAsync, "should be async");
                if (!pereq.RequestCompleted)
                {
                    Error.ThrowInternalError(InternalError.SaveNextChangeIncomplete);
                }
                EqualRefCheck(this.request, pereq, InternalError.InvalidSaveNextChange);
                if (DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.Batch))
                {
                    this.httpWebResponseStream.Position = 0L;
                    this.request = null;
                    base.SetCompleted();
                }
                else
                {
                    if (0L == this.copiedContentLength)
                    {
                        this.HandleOperationResponseNoData();
                    }
                    this.HandleOperationEnd();
                    if (!this.processingMediaLinkEntry)
                    {
                        this.changesCompleted++;
                    }
                    pereq.Dispose();
                    this.request = null;
                    if (!pereq.RequestCompletedSynchronously && !base.IsCompletedInternally)
                    {
                        this.BeginNextChange(DataServiceContext.IsFlagSet(this.options, SaveChangesOptions.ReplaceOnUpdate));
                    }
                }
            }

            private void SetupMediaResourceRequest(HttpWebRequest mediaResourceRequest, EntityDescriptor box)
            {
                this.mediaResourceRequestStream = box.SaveStream.Stream;
                WebUtil.ApplyHeadersToRequest(box.SaveStream.Args.Headers, mediaResourceRequest, true);
            }

            private int ValidateContentID(Dictionary<string, string> contentHeaders)
            {
                string str;
                int result = 0;
                if (!(contentHeaders.TryGetValue("Content-ID", out str) && int.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result)))
                {
                    Error.ThrowBatchUnexpectedContent(InternalError.ChangeResponseMissingContentID);
                }
                for (int i = 0; i < this.ChangedEntries.Count; i++)
                {
                    if (this.ChangedEntries[i].ChangeOrder == result)
                    {
                        return i;
                    }
                }
                Error.ThrowBatchUnexpectedContent(InternalError.ChangeResponseUnknownContentID);
                return -1;
            }

            internal System.Data.Services.Client.DataServiceResponse DataServiceResponse
            {
                get
                {
                    return this.service;
                }
                set
                {
                    this.service = value;
                }
            }

            //[CompilerGenerated]
            //private sealed class <HandleBatchResponse>d__29 : IEnumerable<OperationResponse>, IEnumerable, IEnumerator<OperationResponse>, IEnumerator, IDisposable
            //{
            //    private int <>1__state;
            //    private OperationResponse <>2__current;
            //    public BatchStream <>3__batch;
            //    public DataServiceContext.SaveResult <>4__this;
            //    private int <>l__initialThreadId;
            //    public ChangeOperationResponse <changeOperationResponse>5__39;
            //    public int <changesetIndex>5__2f;
            //    public Dictionary<string, string> <contentHeaders>5__32;
            //    public string <contentType>5__2a;
            //    public Uri <editLink>5__2d;
            //    public Descriptor <entry>5__33;
            //    public Exception <error>5__37;
            //    public string <etag>5__2c;
            //    public Exception <ex>5__34;
            //    public int <index>5__38;
            //    public string <location>5__2b;
            //    public int <operationCount>5__31;
            //    public QueryOperationResponse <qresponse>5__35;
            //    public int <queryCount>5__30;
            //    public HttpStatusCode <status>5__2e;
            //    public HttpStatusCode <statusCode>5__36;
            //    public BatchStream batch;

            //    [DebuggerHidden]
            //    public <HandleBatchResponse>d__29(int <>1__state)
            //    {
            //        this.<>1__state = <>1__state;
            //        this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            //    }

            //    private bool MoveNext()
            //    {
            //        switch (this.<>1__state)
            //        {
            //            case 0:
            //                this.<>1__state = -1;
            //                if (this.batch.CanRead)
            //                {
            //                    this.<editLink>5__2d = null;
            //                    this.<changesetIndex>5__2f = 0;
            //                    this.<queryCount>5__30 = 0;
            //                    this.<operationCount>5__31 = 0;
            //                    this.<>4__this.entryIndex = 0;
            //                    while (this.batch.MoveNext())
            //                    {
            //                        this.<contentHeaders>5__32 = this.batch.ContentHeaders;
            //                        switch (this.batch.State)
            //                        {
            //                            case BatchStreamState.BeginChangeSet:
            //                                if ((DataServiceContext.IsFlagSet(this.<>4__this.options, SaveChangesOptions.Batch) && (this.<changesetIndex>5__2f != 0)) || (0 != this.<operationCount>5__31))
            //                                {
            //                                    Error.ThrowBatchUnexpectedContent(InternalError.UnexpectedBeginChangeSet);
            //                                }
            //                                goto Label_0858;

            //                            case BatchStreamState.EndChangeSet:
            //                                this.<changesetIndex>5__2f++;
            //                                this.<operationCount>5__31 = 0;
            //                                goto Label_0858;

            //                            case BatchStreamState.GetResponse:
            //                                DataServiceRequest request;
            //                                Debug.Assert(0 == this.<operationCount>5__31, "missing an EndChangeSet 2");
            //                                this.<contentHeaders>5__32.TryGetValue("Content-Type", out this.<contentType>5__2a);
            //                                this.<status>5__2e = (HttpStatusCode) (-1);
            //                                this.<ex>5__34 = null;
            //                                this.<qresponse>5__35 = null;
            //                                try
            //                                {
            //                                    this.<status>5__2e = this.batch.GetStatusCode();
            //                                    this.<ex>5__34 = DataServiceContext.HandleResponse(this.<status>5__2e, this.batch.GetResponseVersion(), new Func<Stream>(this.batch, (IntPtr) this.GetContentStream), false);
            //                                    if (null == this.<ex>5__34)
            //                                    {
            //                                        request = this.<>4__this.Queries[this.<queryCount>5__30];
            //                                        MaterializeAtom results = DataServiceRequest.Materialize(this.<>4__this.Context, request.QueryComponents, null, this.<contentType>5__2a, this.batch.GetContentStream());
            //                                        this.<qresponse>5__35 = QueryOperationResponse.GetInstance(request.ElementType, this.<contentHeaders>5__32, request, results);
            //                                    }
            //                                }
            //                                catch (ArgumentException exception)
            //                                {
            //                                    this.<ex>5__34 = exception;
            //                                }
            //                                catch (FormatException exception2)
            //                                {
            //                                    this.<ex>5__34 = exception2;
            //                                }
            //                                catch (InvalidOperationException exception3)
            //                                {
            //                                    this.<ex>5__34 = exception3;
            //                                }
            //                                if (null == this.<qresponse>5__35)
            //                                {
            //                                    if (null == this.<>4__this.Queries)
            //                                    {
            //                                        throw this.<ex>5__34;
            //                                    }
            //                                    request = this.<>4__this.Queries[this.<queryCount>5__30];
            //                                    if (this.<>4__this.Context.ignoreResourceNotFoundException && (this.<status>5__2e == HttpStatusCode.NotFound))
            //                                    {
            //                                        this.<qresponse>5__35 = QueryOperationResponse.GetInstance(request.ElementType, this.<contentHeaders>5__32, request, MaterializeAtom.EmptyResults);
            //                                    }
            //                                    else
            //                                    {
            //                                        this.<qresponse>5__35 = QueryOperationResponse.GetInstance(request.ElementType, this.<contentHeaders>5__32, request, MaterializeAtom.EmptyResults);
            //                                        this.<qresponse>5__35.Error = this.<ex>5__34;
            //                                    }
            //                                }
            //                                this.<qresponse>5__35.StatusCode = (int) this.<status>5__2e;
            //                                this.<queryCount>5__30++;
            //                                this.<>2__current = this.<qresponse>5__35;
            //                                this.<>1__state = 2;
            //                                return true;

            //                            case BatchStreamState.ChangeResponse:
            //                                this.<statusCode>5__36 = this.batch.GetStatusCode();
            //                                this.<error>5__37 = DataServiceContext.HandleResponse(this.<statusCode>5__36, this.batch.GetResponseVersion(), new Func<Stream>(this.batch, (IntPtr) this.GetContentStream), false);
            //                                this.<index>5__38 = this.<>4__this.ValidateContentID(this.<contentHeaders>5__32);
            //                                try
            //                                {
            //                                    EntityDescriptor descriptor;
            //                                    this.<entry>5__33 = this.<>4__this.ChangedEntries[this.<index>5__38];
            //                                    this.<operationCount>5__31 += this.<>4__this.Context.SaveResultProcessed(this.<entry>5__33);
            //                                    if (null != this.<error>5__37)
            //                                    {
            //                                        throw this.<error>5__37;
            //                                    }
            //                                    StreamStates noStream = StreamStates.NoStream;
            //                                    if (this.<entry>5__33.IsResource)
            //                                    {
            //                                        descriptor = (EntityDescriptor) this.<entry>5__33;
            //                                        noStream = descriptor.StreamState;
            //                                        if (descriptor.StreamState == StreamStates.Added)
            //                                        {
            //                                            Debug.Assert(((this.<statusCode>5__36 == HttpStatusCode.Created) && (this.<entry>5__33.State == EntityStates.Modified)) && descriptor.IsMediaLinkEntry, "statusCode == HttpStatusCode.Created && entry.State == EntityStates.Modified && descriptor.IsMediaLinkEntry -- Processing Post MR");
            //                                        }
            //                                        else if (descriptor.StreamState == StreamStates.Modified)
            //                                        {
            //                                            Debug.Assert((this.<statusCode>5__36 == HttpStatusCode.NoContent) && descriptor.IsMediaLinkEntry, "statusCode == HttpStatusCode.NoContent && descriptor.IsMediaLinkEntry -- Processing Put MR");
            //                                        }
            //                                    }
            //                                    if ((noStream == StreamStates.Added) || (this.<entry>5__33.State == EntityStates.Added))
            //                                    {
            //                                        if (this.<entry>5__33.IsResource)
            //                                        {
            //                                            string mime = null;
            //                                            Encoding encoding = null;
            //                                            this.<contentHeaders>5__32.TryGetValue("Content-Type", out this.<contentType>5__2a);
            //                                            this.<contentHeaders>5__32.TryGetValue("Location", out this.<location>5__2b);
            //                                            this.<contentHeaders>5__32.TryGetValue("ETag", out this.<etag>5__2c);
            //                                            EntityDescriptor entry = (EntityDescriptor) this.<entry>5__33;
            //                                            if (this.<location>5__2b == null)
            //                                            {
            //                                                throw Error.NotSupported(Strings.Deserialize_NoLocationHeader);
            //                                            }
            //                                            this.<editLink>5__2d = Util.CreateUri(this.<location>5__2b, UriKind.Absolute);
            //                                            Stream contentStream = this.batch.GetContentStream();
            //                                            if (null != contentStream)
            //                                            {
            //                                                HttpProcessUtility.ReadContentType(this.<contentType>5__2a, out mime, out encoding);
            //                                                if (!string.Equals("application/atom+xml", mime, StringComparison.OrdinalIgnoreCase))
            //                                                {
            //                                                    throw Error.InvalidOperation(Strings.Deserialize_UnknownMimeTypeSpecified(mime));
            //                                                }
            //                                                XmlReader reader = XmlUtil.CreateXmlReader(contentStream, encoding);
            //                                                QueryComponents queryComponents = new QueryComponents(null, Util.DataServiceVersionEmpty, entry.Entity.GetType(), null, null);
            //                                                descriptor = (EntityDescriptor) this.<entry>5__33;
            //                                                MergeOption overwriteChanges = MergeOption.OverwriteChanges;
            //                                                if (descriptor.StreamState == StreamStates.Added)
            //                                                {
            //                                                    overwriteChanges = MergeOption.PreserveChanges;
            //                                                    Debug.Assert(descriptor.State == EntityStates.Modified, "The MLE state must be Modified.");
            //                                                }
            //                                                try
            //                                                {
            //                                                    using (MaterializeAtom atom2 = new MaterializeAtom(this.<>4__this.Context, reader, queryComponents, null, overwriteChanges))
            //                                                    {
            //                                                        this.<>4__this.Context.HandleResponsePost(entry, atom2, this.<editLink>5__2d, this.<etag>5__2c);
            //                                                    }
            //                                                }
            //                                                finally
            //                                                {
            //                                                    if (descriptor.StreamState == StreamStates.Added)
            //                                                    {
            //                                                        Debug.Assert(descriptor.State == EntityStates.Unchanged, "The materializer should always set the entity state to Unchanged.");
            //                                                        descriptor.State = EntityStates.Modified;
            //                                                        descriptor.StreamState = StreamStates.NoStream;
            //                                                    }
            //                                                }
            //                                            }
            //                                            else
            //                                            {
            //                                                this.<>4__this.Context.HandleResponsePost(entry, null, this.<editLink>5__2d, this.<etag>5__2c);
            //                                            }
            //                                        }
            //                                        else
            //                                        {
            //                                            DataServiceContext.HandleResponsePost((LinkDescriptor) this.<entry>5__33);
            //                                        }
            //                                    }
            //                                    else if ((noStream == StreamStates.Modified) || (this.<entry>5__33.State == EntityStates.Modified))
            //                                    {
            //                                        this.<contentHeaders>5__32.TryGetValue("ETag", out this.<etag>5__2c);
            //                                        DataServiceContext.HandleResponsePut(this.<entry>5__33, this.<etag>5__2c);
            //                                    }
            //                                    else if (this.<entry>5__33.State == EntityStates.Deleted)
            //                                    {
            //                                        this.<>4__this.Context.HandleResponseDelete(this.<entry>5__33);
            //                                    }
            //                                }
            //                                catch (Exception exception4)
            //                                {
            //                                    this.<>4__this.ChangedEntries[this.<index>5__38].SaveError = exception4;
            //                                    this.<error>5__37 = exception4;
            //                                }
            //                                this.<changeOperationResponse>5__39 = new ChangeOperationResponse(this.<contentHeaders>5__32, this.<>4__this.ChangedEntries[this.<index>5__38]);
            //                                this.<changeOperationResponse>5__39.StatusCode = (int) this.<statusCode>5__36;
            //                                if (this.<error>5__37 != null)
            //                                {
            //                                    this.<changeOperationResponse>5__39.Error = this.<error>5__37;
            //                                }
            //                                this.<>4__this.Responses.Add(this.<changeOperationResponse>5__39);
            //                                this.<operationCount>5__31++;
            //                                this.<>4__this.entryIndex++;
            //                                this.<>2__current = this.<changeOperationResponse>5__39;
            //                                this.<>1__state = 6;
            //                                return true;

            //                            default:
            //                                Error.ThrowBatchExpectedResponse(InternalError.UnexpectedBatchState);
            //                                goto Label_0858;
            //                        }
            //                    Label_0334:
            //                        this.<>1__state = -1;
            //                        goto Label_0858;
            //                    Label_0845:
            //                        this.<>1__state = -1;
            //                    Label_0858:;
            //                    }
            //                    Debug.Assert(this.batch.State == BatchStreamState.EndBatch, "unexpected batch state");
            //                    if ((this.<>4__this.Queries == null) && ((this.<changesetIndex>5__2f != 0) && (0 >= this.<queryCount>5__30)))
            //                    {
            //                        if (DataServiceContext.SaveResult.CS$<>9__CachedAnonymousMethodDelegate27 == null)
            //                        {
            //                            DataServiceContext.SaveResult.CS$<>9__CachedAnonymousMethodDelegate27 = new Func<Descriptor, bool>(null, (IntPtr) DataServiceContext.SaveResult.<HandleBatchResponse>b__25);
            //                        }
            //                        if ((Enumerable.Any<Descriptor>(this.<>4__this.ChangedEntries, DataServiceContext.SaveResult.CS$<>9__CachedAnonymousMethodDelegate27) && DataServiceContext.IsFlagSet(this.<>4__this.options, SaveChangesOptions.Batch)) && (DataServiceContext.SaveResult.CS$<>9__CachedAnonymousMethodDelegate28 == null))
            //                        {
            //                            DataServiceContext.SaveResult.CS$<>9__CachedAnonymousMethodDelegate28 = new Func<Descriptor, bool>(null, (IntPtr) DataServiceContext.SaveResult.<HandleBatchResponse>b__26);
            //                        }
            //                    }
            //                    if ((Enumerable.FirstOrDefault<Descriptor>(this.<>4__this.ChangedEntries, DataServiceContext.SaveResult.CS$<>9__CachedAnonymousMethodDelegate28) == null) || ((this.<>4__this.Queries != null) && (this.<queryCount>5__30 != this.<>4__this.Queries.Length)))
            //                    {
            //                        throw Error.InvalidOperation(Strings.Batch_IncompleteResponseCount);
            //                    }
            //                    this.batch.Dispose();
            //                }
            //                break;

            //            case 2:
            //                goto Label_0334;

            //            case 6:
            //                goto Label_0845;
            //        }
            //        return false;
            //    }

            //    [DebuggerHidden]
            //    IEnumerator<OperationResponse> IEnumerable<OperationResponse>.GetEnumerator()
            //    {
            //        DataServiceContext.SaveResult.<HandleBatchResponse>d__29 d__;
            //        if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
            //        {
            //            this.<>1__state = 0;
            //            d__ = this;
            //        }
            //        else
            //        {
            //            d__ = new DataServiceContext.SaveResult.<HandleBatchResponse>d__29(0);
            //            d__.<>4__this = this.<>4__this;
            //        }
            //        d__.batch = this.<>3__batch;
            //        return d__;
            //    }

            //    [DebuggerHidden]
            //    IEnumerator IEnumerable.GetEnumerator()
            //    {
            //        return this.System.Collections.Generic.IEnumerable<System.Data.Services.Client.OperationResponse>.GetEnumerator();
            //    }

            //    [DebuggerHidden]
            //    void IEnumerator.Reset()
            //    {
            //        throw new NotSupportedException();
            //    }

            //    void IDisposable.Dispose()
            //    {
            //    }

            //    OperationResponse IEnumerator<OperationResponse>.Current
            //    {
            //        [DebuggerHidden]
            //        get
            //        {
            //            return this.<>2__current;
            //        }
            //    }

            //    object IEnumerator.Current
            //    {
            //        [DebuggerHidden]
            //        get
            //        {
            //            return this.<>2__current;
            //        }
            //    }
            //}

            private sealed class PerRequest
            {
                public HttpWebResponse HttpWebResponse { get; set; }
                public HttpWebRequest Request { get; set; }
                public bool RequestCompletedSynchronously { get; set; }
                public int RequestContentBufferValidLength { get; set; }
                public ContentStream RequestContentStream { get; set; }
                public Stream RequestStream { get; set; }
                public Stream ResponseStream { get; set; }

                //[CompilerGenerated]
                //private System.Data.Services.Http.HttpWebResponse <HttpWebResponse>k__BackingField;
                //[CompilerGenerated]
                //private HttpWebRequest <Request>k__BackingField;
                //[CompilerGenerated]
                //private bool <RequestCompletedSynchronously>k__BackingField;
                //[CompilerGenerated]
                //private int <RequestContentBufferValidLength>k__BackingField;
                //[CompilerGenerated]
                //private ContentStream <RequestContentStream>k__BackingField;
                //[CompilerGenerated]
                //private Stream <RequestStream>k__BackingField;
                //[CompilerGenerated]
                //private Stream <ResponseStream>k__BackingField;
                private byte[] requestContentBuffer;
                private int requestStatus;

                internal PerRequest()
                {
                    this.RequestCompletedSynchronously = true;
                }

                internal void Dispose()
                {
                    Stream stream = null;
                    if (null != (stream = this.ResponseStream))
                    {
                        this.ResponseStream = null;
                        stream.Dispose();
                    }
                    if (null != this.RequestContentStream)
                    {
                        if ((this.RequestContentStream.Stream != null) && this.RequestContentStream.IsKnownMemoryStream)
                        {
                            this.RequestContentStream.Stream.Dispose();
                        }
                        this.RequestContentStream = null;
                    }
                    if (null != (stream = this.RequestStream))
                    {
                        this.RequestStream = null;
                        try
                        {
                            Util.DebugInjectFault("PerRequest::Dispose_BeforeRequestStreamDisposed");
                            stream.Dispose();
                        }
                        catch (WebException)
                        {
                            if (!this.RequestAborted)
                            {
                                throw;
                            }
                            Util.DebugInjectFault("PerRequest::Dispose_WebExceptionThrown");
                        }
                    }
                    System.Data.Services.Http.HttpWebResponse httpWebResponse = this.HttpWebResponse;
                    if (null != httpWebResponse)
                    {
                        httpWebResponse.Close();
                    }
                    this.Request = null;
                    this.SetComplete();
                }

                internal void SetAborted()
                {
                    Interlocked.Exchange(ref this.requestStatus, 2);
                }

                internal void SetComplete()
                {
                    Interlocked.CompareExchange(ref this.requestStatus, 1, 0);
                }

                //internal System.Data.Services.Http.HttpWebResponse HttpWebResponse
                //{
                //    [CompilerGenerated]
                //    get
                //    {
                //        return this.<HttpWebResponse>k__BackingField;
                //    }
                //    [CompilerGenerated]
                //    set
                //    {
                //        this.<HttpWebResponse>k__BackingField = value;
                //    }
                //}

                //internal HttpWebRequest Request
                //{
                //    [CompilerGenerated]
                //    get
                //    {
                //        return this.<Request>k__BackingField;
                //    }
                //    [CompilerGenerated]
                //    set
                //    {
                //        this.<Request>k__BackingField = value;
                //    }
                //}

                internal bool RequestAborted
                {
                    get
                    {
                        return (this.requestStatus == 2);
                    }
                }

                internal bool RequestCompleted
                {
                    get
                    {
                        return (this.requestStatus != 0);
                    }
                }

                //internal bool RequestCompletedSynchronously
                //{
                //    [CompilerGenerated]
                //    get
                //    {
                //        return this.<RequestCompletedSynchronously>k__BackingField;
                //    }
                //    [CompilerGenerated]
                //    set
                //    {
                //        this.<RequestCompletedSynchronously>k__BackingField = value;
                //    }
                //}

                internal byte[] RequestContentBuffer
                {
                    get
                    {
                        if (this.requestContentBuffer == null)
                        {
                            this.requestContentBuffer = new byte[0x10000];
                        }
                        return this.requestContentBuffer;
                    }
                }

                //internal int RequestContentBufferValidLength
                //{
                //    [CompilerGenerated]
                //    get
                //    {
                //        return this.<RequestContentBufferValidLength>k__BackingField;
                //    }
                //    [CompilerGenerated]
                //    set
                //    {
                //        this.<RequestContentBufferValidLength>k__BackingField = value;
                //    }
                //}

                //internal ContentStream RequestContentStream
                //{
                //    [CompilerGenerated]
                //    get
                //    {
                //        return this.<RequestContentStream>k__BackingField;
                //    }
                //    [CompilerGenerated]
                //    set
                //    {
                //        this.<RequestContentStream>k__BackingField = value;
                //    }
                //}

                //internal Stream RequestStream
                //{
                //    [CompilerGenerated]
                //    get
                //    {
                //        return this.<RequestStream>k__BackingField;
                //    }
                //    [CompilerGenerated]
                //    set
                //    {
                //        this.<RequestStream>k__BackingField = value;
                //    }
                //}

                //internal Stream ResponseStream
                //{
                //    [CompilerGenerated]
                //    get
                //    {
                //        return this.<ResponseStream>k__BackingField;
                //    }
                //    [CompilerGenerated]
                //    set
                //    {
                //        this.<ResponseStream>k__BackingField = value;
                //    }
                //}

                internal class ContentStream
                {
                    private readonly bool isKnownMemoryStream;
                    private readonly System.IO.Stream stream;

                    public ContentStream(System.IO.Stream stream, bool isKnownMemoryStream)
                    {
                        this.stream = stream;
                        this.isKnownMemoryStream = isKnownMemoryStream;
                    }

                    public bool IsKnownMemoryStream
                    {
                        get
                        {
                            return this.isKnownMemoryStream;
                        }
                    }

                    public System.IO.Stream Stream
                    {
                        get
                        {
                            return this.stream;
                        }
                    }
                }
            }
        }

        private class LoadPropertyResult : QueryResult
{
    // Fields
    private readonly object entity;
    private readonly ProjectionPlan plan;
    private readonly string propertyName;

    // Methods
    internal LoadPropertyResult(object entity, string propertyName, DataServiceContext context, HttpWebRequest request, AsyncCallback callback, object state, DataServiceRequest dataServiceRequest, ProjectionPlan plan) : base(context, "LoadProperty", dataServiceRequest, request, callback, state)
    {
        this.entity = entity;
        this.propertyName = propertyName;
        this.plan = plan;
    }

    internal QueryOperationResponse LoadProperty()
    {
        MaterializeAtom results = null;
        QueryOperationResponse responseWithType;
        DataServiceContext source = (DataServiceContext) base.Source;
        ClientType type = ClientType.Create(this.entity.GetType());
        Debug.Assert(type.IsEntityType, "must be entity type to be contained");
        EntityDescriptor box = source.EnsureContained(this.entity, "entity");
        if (EntityStates.Added == box.State)
        {
            throw Error.InvalidOperation(Strings.Context_NoLoadWithInsertEnd);
        }
        ClientType.ClientProperty property = type.GetProperty(this.propertyName, false);
        Type elementType = property.CollectionType ?? property.NullablePropertyType;
        try
        {
            if (type.MediaDataMember == property)
            {
                results = this.ReadPropertyFromRawData(property);
            }
            else
            {
                results = this.ReadPropertyFromAtom(box, property);
            }
            responseWithType = base.GetResponseWithType(results, elementType);
        }
        catch (InvalidOperationException exception)
        {
            QueryOperationResponse response = base.GetResponseWithType(results, elementType);
            if (response != null)
            {
                response.Error = exception;
                throw new DataServiceQueryException(Strings.DataServiceException_GeneralError, exception, response);
            }
            throw;
        }
        return responseWithType;
    }

    private static byte[] ReadByteArrayChunked(Stream responseStream)
    {
        byte[] buffer = null;
        using (MemoryStream stream = new MemoryStream())
        {
            bool flag;
            byte[] buffer2 = new byte[0x1000];
            int count = 0;
            int num2 = 0;
            goto Label_0047;
        Label_001C:
            count = responseStream.Read(buffer2, 0, buffer2.Length);
            if (count <= 0)
            {
                goto Label_004C;
            }
            stream.Write(buffer2, 0, count);
            num2 += count;
        Label_0047:
            flag = true;
            goto Label_001C;
        Label_004C:
            buffer = new byte[num2];
            stream.Position = 0L;
            count = stream.Read(buffer, 0, buffer.Length);
        }
        return buffer;
    }

    private static byte[] ReadByteArrayWithContentLength(Stream responseStream, int totalLength)
    {
        int num2;
        byte[] buffer = new byte[totalLength];
        for (int i = 0; i < totalLength; i += num2)
        {
            num2 = responseStream.Read(buffer, i, totalLength - i);
            if (num2 <= 0)
            {
                throw Error.InvalidOperation(Strings.Context_UnexpectedZeroRawRead);
            }
        }
        return buffer;
    }

    private MaterializeAtom ReadPropertyFromAtom(EntityDescriptor box, ClientType.ClientProperty property)
    {
        MaterializeAtom atom2;
        DataServiceContext source = (DataServiceContext) base.Source;
        bool applyingChanges = source.ApplyingChanges;
        try
        {
            source.ApplyingChanges = true;
            bool flag2 = EntityStates.Deleted == box.State;
            Type type = property.CollectionType ?? property.NullablePropertyType;
            ClientType type2 = ClientType.Create(type);
            bool flag3 = false;
            object entity = this.entity;
            if (null != property.CollectionType)
            {
                entity = property.GetValue(this.entity);
                if (null == entity)
                {
                    flag3 = true;
                    if (BindingEntityInfo.IsDataServiceCollection(property.PropertyType))
                    {
                        Debug.Assert(WebUtil.GetDataServiceCollectionOfT(new Type[] { type }) != null, "DataServiceCollection<> must be available here.");
                        object[] args = new object[2];
                        args[1] = TrackingMode.None;
                        entity = Activator.CreateInstance(WebUtil.GetDataServiceCollectionOfT(new Type[] { type }), args);
                    }
                    else
                    {
                        entity = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type }));
                    }
                }
            }
            Type type3 = property.CollectionType ?? property.NullablePropertyType;
            IList results = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type3 }));
            DataServiceQueryContinuation continuation = null;
            using (MaterializeAtom atom = base.GetMaterializer(source, this.plan))
            {
                Debug.Assert(atom != null, "materializer != null -- otherwise GetMaterializer() returned null rather than empty");
                int num = 0;
                foreach (object obj3 in atom)
                {
                    results.Add(obj3);
                    num++;
                    property.SetValue(entity, obj3, this.propertyName, true);
                    if (((obj3 != null) && (MergeOption.NoTracking != atom.MergeOptionValue)) && type2.IsEntityType)
                    {
                        if (flag2)
                        {
                            source.DeleteLink(this.entity, this.propertyName, obj3);
                        }
                        else
                        {
                            source.AttachLink(this.entity, this.propertyName, obj3, atom.MergeOptionValue);
                        }
                    }
                }
                continuation = atom.GetContinuation(null);
                Util.SetNextLinkForCollection(entity, continuation);
            }
            if (flag3)
            {
                property.SetValue(this.entity, entity, this.propertyName, false);
            }
            atom2 = MaterializeAtom.CreateWrapper(results, continuation);
        }
        finally
        {
            source.ApplyingChanges = applyingChanges;
        }
        return atom2;
    }

    private MaterializeAtom ReadPropertyFromRawData(ClientType.ClientProperty property)
    {
        MaterializeAtom atom;
        DataServiceContext source = (DataServiceContext) base.Source;
        bool applyingChanges = source.ApplyingChanges;
        try
        {
            source.ApplyingChanges = true;
            string mime = null;
            Encoding encoding = null;
            Type type = property.CollectionType ?? property.NullablePropertyType;
            IList results = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type }));
            HttpProcessUtility.ReadContentType(base.ContentType, out mime, out encoding);
            using (Stream stream = base.GetResponseStream())
            {
                if (property.PropertyType == typeof(byte[]))
                {
                    int contentLength = (int) base.ContentLength;
                    byte[] buffer = null;
                    if (contentLength >= 0)
                    {
                        buffer = ReadByteArrayWithContentLength(stream, contentLength);
                    }
                    else
                    {
                        buffer = ReadByteArrayChunked(stream);
                    }
                    results.Add(buffer);
                    property.SetValue(this.entity, buffer, this.propertyName, false);
                }
                else
                {
                    StreamReader reader = new StreamReader(stream, encoding);
                    object obj2 = (property.PropertyType == typeof(string)) ? reader.ReadToEnd() : ClientConvert.ChangeType(reader.ReadToEnd(), property.PropertyType);
                    results.Add(obj2);
                    property.SetValue(this.entity, obj2, this.propertyName, false);
                }
            }
            if (property.MimeTypeProperty != null)
            {
                property.MimeTypeProperty.SetValue(this.entity, mime, null, false);
            }
            atom = MaterializeAtom.CreateWrapper(results);
        }
        finally
        {
            source.ApplyingChanges = applyingChanges;
        }
        return atom;
    }
}

internal class DataServiceSaveStream
{
    // Fields
    private readonly DataServiceRequestArgs args;
    private readonly bool close;
    private readonly Stream stream;

    // Methods
    internal DataServiceSaveStream(Stream stream, bool close, DataServiceRequestArgs args)
    {
        Debug.Assert(stream != null, "stream must not be null.");
        this.stream = stream;
        this.close = close;
        this.args = args;
    }

    internal void Close()
    {
        if ((this.stream != null) && this.close)
        {
            this.stream.Close();
        }
    }

    // Properties
    internal DataServiceRequestArgs Args
    {
        get
        {
            return this.args;
        }
    }

    internal Stream Stream
    {
        get
        {
            return this.stream;
        }
    }
}

 
 
 

    }
}

