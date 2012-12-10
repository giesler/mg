namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    //[EntitySet("Albums"), EntityPropertyMapping("Ux", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Ux"), DataServiceKey("Id"), EntityPropertyMapping("Summary", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, false), EntityPropertyMapping("Browse", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Browse")]
    [EntityPropertyMappingEx("Ux", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Ux"), 
    EntityPropertyMappingEx("Summary", SyndicationItemPropertyEx.Summary, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Browse", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Browse")]
    [EntitySet("Albums"), 
    //EntityPropertyMapping("Ux", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    DataServiceKey("Id"), 
    EntityPropertyMapping("Summary", SyndicationItemProperty.Summary, SyndicationTextContentKind.Plaintext, false), 
    //EntityPropertyMapping("Browse", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false)
    ]
    public abstract class TopLevelFolder : LiveResource
    {
        private Uri _Browse;
        private LiveDataServiceCollection<FileEntry> _Files;
        private string _SharingLevel;
        private int _Size;
        private string _Summary;
        private Uri _Ux;

        protected TopLevelFolder()
        {
        }

        public Uri Browse
        {
            get
            {
                return this._Browse;
            }
             set
            {
                this._Browse = value;
            }
        }

        public LiveDataServiceCollection<FileEntry> Files
        {
            get
            {
                if (this._Files == null)
                {
                    this._Files = new LiveDataServiceCollection<FileEntry>(this);
                }
                return this._Files;
            }
        }

        public string SharingLevel
        {
            get
            {
                if (this._SharingLevel == null)
                {
                    this._SharingLevel = "Public";
                }
                return this._SharingLevel;
            }
            set
            {
                this._SharingLevel = value;
            }
        }

        public int Size
        {
            get
            {
                return this._Size;
            }
             set
            {
                this._Size = value;
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
             set
            {
                this._Ux = value;
            }
        }
    }
}

