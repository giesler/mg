namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Collections.Generic;

    [DebuggerDisplay("State = {state}, Uri = {editLink}, Element = {entity.GetType().ToString()}")]
    public sealed class EntityDescriptor : Descriptor
    {
        private Uri editLink;
        private Uri editMediaLink;
        private object entity;
        private string entitySetName;
        private string etag;
        private string identity;
        private bool mediaLinkEntry;
        private EntityDescriptor parentDescriptor;
        private string parentProperty;
        private Uri readStreamLink;
        private DataServiceContext.DataServiceSaveStream saveStream;
        private Uri selfLink;
        private string serverTypeName;
        private string streamETag;
        private StreamStates streamState;
        private Dictionary<string, Uri> relationshipLinks;
 


        internal EntityDescriptor(string identity, Uri selfLink, Uri editLink, object entity, EntityDescriptor parentEntity, string parentProperty, string entitySetName, string etag, EntityStates state) : base(state)
        {
            Debug.Assert(entity != null, "entity is null");
            Debug.Assert(((parentEntity == null) && (parentProperty == null)) || ((parentEntity != null) && (parentProperty != null)), "When parentEntity is specified, must also specify parentProperyName");
            if (state == EntityStates.Added)
            {
                Debug.Assert((((identity == null) && (selfLink == null)) && (editLink == null)) && (etag == null), "For objects in added state, identity, self-link, edit-link and etag must be null");
                Debug.Assert(((!string.IsNullOrEmpty(entitySetName) && (parentEntity == null)) && string.IsNullOrEmpty(parentProperty)) || ((string.IsNullOrEmpty(entitySetName) && (parentEntity != null)) && !string.IsNullOrEmpty(parentProperty)), "For entities in added state, entity set name or the insert path must be specified");
            }
            else
            {
                Debug.Assert(identity != null, "For objects in non-added state, identity must never be null");
                Debug.Assert((string.IsNullOrEmpty(entitySetName) && string.IsNullOrEmpty(parentProperty)) && (parentEntity == null), "For non-added entities, the entity set name and the insert path must be null");
            }
            this.identity = identity;
            this.selfLink = selfLink;
            this.editLink = editLink;
            this.parentDescriptor = parentEntity;
            this.parentProperty = parentProperty;
            this.entity = entity;
            this.etag = etag;
            this.entitySetName = entitySetName;
        }

        internal void CloseSaveStream()
        {
            if (this.saveStream != null)
            {
                DataServiceContext.DataServiceSaveStream saveStream = this.saveStream;
                this.saveStream = null;
                saveStream.Close();
            }
        }

        internal Uri GetEditMediaResourceUri(Uri serviceBaseUri)
        {
            return ((this.EditStreamUri == null) ? null : Util.CreateUri(serviceBaseUri, this.EditStreamUri));
        }

        private Uri GetLink(bool queryLink)
        {
            if (queryLink && (this.SelfLink != null))
            {
                return this.SelfLink;
            }
            if (this.EditLink != null)
            {
                return this.EditLink;
            }
            Debug.Assert(base.State == EntityStates.Added, "the entity must be in added state");
            if (!string.IsNullOrEmpty(this.entitySetName))
            {
                return Util.CreateUri(this.entitySetName, UriKind.Relative);
            }
            return Util.CreateUri(this.parentProperty, UriKind.Relative);
        }

        internal Uri GetMediaResourceUri(Uri serviceBaseUri)
        {
            return ((this.ReadStreamUri == null) ? null : Util.CreateUri(serviceBaseUri, this.ReadStreamUri));
        }

        internal LinkDescriptor GetRelatedEnd()
        {
            Debug.Assert(this.IsDeepInsert, "For related end, this must be a deep insert");
            Debug.Assert(this.Identity == null, "If the identity is set, it means that the edit link no longer has the property name");
            return new LinkDescriptor(this.parentDescriptor.entity, this.parentProperty, this.entity);
        }

        internal Uri GetResourceUri(Uri baseUriWithSlash, bool queryLink)
        {
            if (this.parentDescriptor != null)
            {
                if (this.parentDescriptor.Identity == null)
                {
                    return Util.CreateUri(Util.CreateUri(baseUriWithSlash, new Uri("$" + this.parentDescriptor.ChangeOrder.ToString(CultureInfo.InvariantCulture), UriKind.Relative)), Util.CreateUri(this.parentProperty, UriKind.Relative));
                }
                return Util.CreateUri(Util.CreateUri(baseUriWithSlash, this.parentDescriptor.GetLink(queryLink)), this.GetLink(queryLink));
            }
            return Util.CreateUri(baseUriWithSlash, this.GetLink(queryLink));
        }

        internal bool IsRelatedEntity(LinkDescriptor related)
        {
            return ((this.entity == related.Source) || (this.entity == related.Target));
        }

        public Uri EditLink
        {
            get
            {
                return this.editLink;
            }
            internal set
            {
                this.editLink = value;
            }
        }

        public Uri EditStreamUri
        {
            get
            {
                return this.editMediaLink;
            }
            internal set
            {
                this.editMediaLink = value;
                if (value != null)
                {
                    this.mediaLinkEntry = true;
                }
            }
        }

        public object Entity
        {
            get
            {
                return this.entity;
            }
        }

        public string ETag
        {
            get
            {
                return this.etag;
            }
            internal set
            {
                this.etag = value;
            }
        }

        public string Identity
        {
            get
            {
                return this.identity;
            }
            internal set
            {
                Util.CheckArgumentNotEmpty(value, "Identity");
                this.identity = value;
                this.parentDescriptor = null;
                this.parentProperty = null;
                this.entitySetName = null;
            }
        }

        internal bool IsDeepInsert
        {
            get
            {
                return (this.parentDescriptor != null);
            }
        }

        internal bool IsMediaLinkEntry
        {
            get
            {
                return this.mediaLinkEntry;
            }
        }

        internal override bool IsModified
        {
            get
            {
                return (base.IsModified || (this.saveStream != null));
            }
        }

        internal override bool IsResource
        {
            get
            {
                return true;
            }
        }

        internal object ParentEntity
        {
            get
            {
                return ((this.parentDescriptor != null) ? this.parentDescriptor.entity : null);
            }
        }

        public EntityDescriptor ParentForInsert
        {
            get
            {
                return this.parentDescriptor;
            }
        }

        public string ParentPropertyForInsert
        {
            get
            {
                return this.parentProperty;
            }
        }

        public Uri ReadStreamUri
        {
            get
            {
                return this.readStreamLink;
            }
            internal set
            {
                this.readStreamLink = value;
                if (value != null)
                {
                    this.mediaLinkEntry = true;
                }
            }
        }

        internal DataServiceContext.DataServiceSaveStream SaveStream
        {
            get
            {
                return this.saveStream;
            }
            set
            {
                this.saveStream = value;
                if (value != null)
                {
                    this.mediaLinkEntry = true;
                }
            }
        }

        public Uri SelfLink
        {
            get
            {
                return this.selfLink;
            }
            internal set
            {
                this.selfLink = value;
            }
        }

        public string ServerTypeName
        {
            get
            {
                return this.serverTypeName;
            }
            internal set
            {
                this.serverTypeName = value;
            }
        }

        public string StreamETag
        {
            get
            {
                return this.streamETag;
            }
            internal set
            {
                Debug.Assert(this.mediaLinkEntry, "this.mediaLinkEntry == true");
                this.streamETag = value;
            }
        }

        internal StreamStates StreamState
        {
            get
            {
                return this.streamState;
            }
            set
            {
                this.streamState = value;
                Debug.Assert((this.streamState == StreamStates.NoStream) || this.mediaLinkEntry, "this.streamState == StreamStates.NoStream || this.mediaLinkEntry");
                Debug.Assert(((this.saveStream == null) && (this.streamState == StreamStates.NoStream)) || ((this.saveStream != null) && (this.streamState != StreamStates.NoStream)), "(this.saveStream == null && this.streamState == StreamStates.NoStream) || (this.saveStream != null && this.streamState != StreamStates.NoStream)");
            }
        }

        public Dictionary<string, Uri> RelationshipLinks
        {
            get
            {
                return this.relationshipLinks;
            }
            set
            {
                this.relationshipLinks = value;
            }
        }
 

    }
}

