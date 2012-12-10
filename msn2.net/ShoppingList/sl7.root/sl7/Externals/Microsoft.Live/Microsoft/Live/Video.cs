namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    //[DataServiceKey("Id"), EntityPropertyMapping("Thumbnail_48X48C", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_48X48C"), EntitySet("Files"), EntityPropertyMapping("Thumbnail_800X600", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_800X600"), EntityPropertyMapping("Thumbnail_600X450", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_600X450"), EntityPropertyMapping("Thumbnail_320X320", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_320X320"), EntityPropertyMapping("Thumbnail_213X213C", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_213X213C"), EntityPropertyMapping("Thumbnail_176X176", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_176X176"), EntityPropertyMapping("Thumbnail_104X104C", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_104X104C"), EntityPropertyMapping("Thumbnail_96X96", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_96X96"), EntityPropertyMapping("Thumbnail_48X48", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_48X48")]
    [
    EntityPropertyMappingEx("Thumbnail_48X48C", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_48X48C"),
    EntityPropertyMappingEx("Thumbnail_800X600", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_800X600"),
    EntityPropertyMappingEx("Thumbnail_600X450", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_600X450"),
    EntityPropertyMappingEx("Thumbnail_320X320", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_320X320"),
    EntityPropertyMappingEx("Thumbnail_213X213C", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_213X213C"),
    EntityPropertyMappingEx("Thumbnail_176X176", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_176X176"),
    EntityPropertyMappingEx("Thumbnail_104X104C", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_104X104C"),
    EntityPropertyMappingEx("Thumbnail_96X96", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_96X96"),
    EntityPropertyMappingEx("Thumbnail_48X48", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Thumbnail_48X48")]
    [DataServiceKey("Id"), 
    //EntityPropertyMapping("Thumbnail_48X48C", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), EntitySet("Files"), 
    //EntityPropertyMapping("Thumbnail_800X600", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    //EntityPropertyMapping("Thumbnail_600X450", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false),
    //EntityPropertyMapping("Thumbnail_320X320", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false),
    //EntityPropertyMapping("Thumbnail_213X213C", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false),
    //EntityPropertyMapping("Thumbnail_176X176", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false),
    //EntityPropertyMapping("Thumbnail_104X104C", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false),
    //EntityPropertyMapping("Thumbnail_96X96", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false),
    //EntityPropertyMapping("Thumbnail_48X48", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false)
    ]
    public class Video : FileEntry
    {
        private FileMediaContent _MediaContent;
        private bool _MediaContentInitialized;
        private int _TagCount;
        private LiveDataServiceCollection<Profile> _TaggerProfiles;
        private bool _TaggingEnabled;
        private LiveDataServiceCollection<Tag> _Tags;
        private Uri _Thumbnail_104X104C;
        private Uri _Thumbnail_176X176;
        private Uri _Thumbnail_213X213C;
        private Uri _Thumbnail_320X320;
        private Uri _Thumbnail_48X48;
        private Uri _Thumbnail_48X48C;
        private Uri _Thumbnail_600X450;
        private Uri _Thumbnail_800X600;
        private Uri _Thumbnail_96X96;

        public FileMediaContent MediaContent
        {
            get
            {
                if ((this._MediaContent == null) && !this._MediaContentInitialized)
                {
                    this._MediaContent = new FileMediaContent();
                    this._MediaContentInitialized = true;
                }
                return this._MediaContent;
            }
            internal set
            {
                this._MediaContent = value;
                this._MediaContentInitialized = true;
            }
        }

        public int TagCount
        {
            get
            {
                return this._TagCount;
            }
            internal set
            {
                this._TagCount = value;
                this.OnPropertyChanged("TagCount");
            }
        }

        public LiveDataServiceCollection<Profile> TaggerProfiles
        {
            get
            {
                if (this._TaggerProfiles == null)
                {
                    this._TaggerProfiles = new LiveDataServiceCollection<Profile>(this);
                }
                return this._TaggerProfiles;
            }
        }

        public bool TaggingEnabled
        {
            get
            {
                return this._TaggingEnabled;
            }
            internal set
            {
                this._TaggingEnabled = value;
                this.OnPropertyChanged("TaggingEnabled");
            }
        }

        public LiveDataServiceCollection<Tag> Tags
        {
            get
            {
                if (this._Tags == null)
                {
                    this._Tags = new LiveDataServiceCollection<Tag>(this);
                }
                return this._Tags;
            }
        }

        public Uri Thumbnail_104X104C
        {
            get
            {
                return this._Thumbnail_104X104C;
            }
            internal set
            {
                this._Thumbnail_104X104C = value;
                this.OnPropertyChanged("Thumbnail_104X104C");
            }
        }

        public Uri Thumbnail_176X176
        {
            get
            {
                return this._Thumbnail_176X176;
            }
            internal set
            {
                this._Thumbnail_176X176 = value;
                this.OnPropertyChanged("Thumbnail_176X176");
            }
        }

        public Uri Thumbnail_213X213C
        {
            get
            {
                return this._Thumbnail_213X213C;
            }
            internal set
            {
                this._Thumbnail_213X213C = value;
                this.OnPropertyChanged("Thumbnail_213X213C");
            }
        }

        public Uri Thumbnail_320X320
        {
            get
            {
                return this._Thumbnail_320X320;
            }
            internal set
            {
                this._Thumbnail_320X320 = value;
                this.OnPropertyChanged("Thumbnail_320X320");
            }
        }

        public Uri Thumbnail_48X48
        {
            get
            {
                return this._Thumbnail_48X48;
            }
            internal set
            {
                this._Thumbnail_48X48 = value;
                this.OnPropertyChanged("Thumbnail_48X48");
            }
        }

        public Uri Thumbnail_48X48C
        {
            get
            {
                return this._Thumbnail_48X48C;
            }
            internal set
            {
                this._Thumbnail_48X48C = value;
                this.OnPropertyChanged("Thumbnail_48X48C");
            }
        }

        public Uri Thumbnail_600X450
        {
            get
            {
                return this._Thumbnail_600X450;
            }
            internal set
            {
                this._Thumbnail_600X450 = value;
                this.OnPropertyChanged("Thumbnail_600X450");
            }
        }

        public Uri Thumbnail_800X600
        {
            get
            {
                return this._Thumbnail_800X600;
            }
            internal set
            {
                this._Thumbnail_800X600 = value;
                this.OnPropertyChanged("Thumbnail_800X600");
            }
        }

        public Uri Thumbnail_96X96
        {
            get
            {
                return this._Thumbnail_96X96;
            }
            internal set
            {
                this._Thumbnail_96X96 = value;
                this.OnPropertyChanged("Thumbnail_96X96");
            }
        }
    }
}

