namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;
    using System.IO;

    //[EntityPropertyMapping("Ux", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Ux"), DataServiceKey("Id"), EntitySet("Files"), EntityPropertyMapping("Summary", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, false)]
    [EntityPropertyMappingEx("Ux", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Ux"), 
    EntityPropertyMappingEx("Summary", SyndicationItemPropertyEx.Summary, SyndicationTextContentKind.Plaintext, false)]
    
    [
    //EntityPropertyMapping("Ux", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext,false), 
    DataServiceKey("Id"), EntitySet("Files"), EntityPropertyMapping("Summary", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, false)]
    public abstract class FileEntry : LiveResource
    {
        private int _CommentCount;
        private LiveDataServiceCollection<Comment> _Comments;
        private bool _CommentsEnabled;
        private LiveResource _Parent;
        private string _ParentId;
        private int _Size;
        private string _Summary;
        private Uri _Ux;

        protected FileEntry()
        {
        }

        private void CopyStream(Stream source, Stream destination)
        {
            byte[] buffer = new byte[0x2000];
            int count = 0;
            if (source.CanSeek)
            {
                source.Seek(0L, SeekOrigin.Begin);
            }
            for (count = source.Read(buffer, 0, buffer.Length); count > 0; count = source.Read(buffer, 0, buffer.Length))
            {
                destination.Write(buffer, 0, count);
            }
        }

        public int CommentCount
        {
            get
            {
                return this._CommentCount;
            }
            internal set
            {
                this._CommentCount = value;
                this.OnPropertyChanged("CommentCount");
            }
        }

        public LiveDataServiceCollection<Comment> Comments
        {
            get
            {
                if (this._Comments == null)
                {
                    this._Comments = new LiveDataServiceCollection<Comment>(this);
                }
                return this._Comments;
            }
        }

        public bool CommentsEnabled
        {
            get
            {
                return this._CommentsEnabled;
            }
            internal set
            {
                this._CommentsEnabled = value;
                this.OnPropertyChanged("CommentsEnabled");
            }
        }

        public LiveResource Parent
        {
            get
            {
                return this._Parent;
            }
            internal set
            {
                this._Parent = value;
                this.OnPropertyChanged("Parent");
            }
        }

        public string ParentId
        {
            get
            {
                return this._ParentId;
            }
            internal set
            {
                this._ParentId = value;
                this.OnPropertyChanged("ParentId");
            }
        }

        public int Size
        {
            get
            {
                return this._Size;
            }
            internal set
            {
                this._Size = value;
                this.OnPropertyChanged("Size");
            }
        }

        public string Summary
        {
            get
            {
                return this._Summary;
            }
            set
            {
                this._Summary = value;
                this.OnPropertyChanged("Summary");
            }
        }

        public Uri Ux
        {
            get
            {
                return this._Ux;
            }
            internal set
            {
                this._Ux = value;
            }
        }
    }
}

