namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

//    [EntityPropertyMapping("Ux", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Ux"), DataServiceKey("Id"), EntityPropertyMapping("ThumbnailImage", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/ThumbnailImage")]
    [EntityPropertyMappingEx("Ux", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/Ux"), 
    //DataServiceKey("Id"), 
    EntityPropertyMappingEx("ThumbnailImage", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/ThumbnailImage")]

    [
    //EntityPropertyMapping("Ux", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    DataServiceKey("Id"), 
    //EntityPropertyMapping("ThumbnailImage", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false)
    ]
    public class AboutYouProfile : Profile
    {
        private LiveDataServiceCollection<Album> _Albums;
        private LiveDataServiceCollection<Contact> _AllContacts;
        private string _Cid;
        private string _FirstName;
        private LiveDataServiceCollection<FolderEntry> _Folders;
        private string _Gender;
        private string _Interests;
        private string _LastName;
        private string _Location;
        private string _MoreAboutMe;
        private LiveDataServiceCollection<Activity> _MyActivities;
        private string _Occupation;
        private Uri _ThumbnailImage;
        private Uri _Ux;
        private string _windowsLiveID;

        public LiveDataServiceCollection<Album> Albums
        {
            get
            {
                if (this._Albums == null)
                {
                    this._Albums = new LiveDataServiceCollection<Album>(this);
                }
                return this._Albums;
            }
        }

        public LiveDataServiceCollection<Contact> AllContacts
        {
            get
            {
                if (this._AllContacts == null)
                {
                    this._AllContacts = new LiveDataServiceCollection<Contact>(this);
                }
                return this._AllContacts;
            }
        }

        public string Cid
        {
            get
            {
                return this._Cid;
            }
             set
            {
                this._Cid = value;
                this.OnPropertyChanged("Cid");
            }
        }

        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
             set
            {
                this._FirstName = value;
                this.OnPropertyChanged("FirstName");
            }
        }

        public LiveDataServiceCollection<FolderEntry> Folders
        {
            get
            {
                if (this._Folders == null)
                {
                    this._Folders = new LiveDataServiceCollection<FolderEntry>(this);
                }
                return this._Folders;
            }
        }

        public string Gender
        {
            get
            {
                return this._Gender;
            }
             set
            {
                this._Gender = value;
                this.OnPropertyChanged("Gender");
            }
        }

        public string Interests
        {
            get
            {
                return this._Interests;
            }
             set
            {
                this._Interests = value;
                this.OnPropertyChanged("Interests");
            }
        }

        public string LastName
        {
            get
            {
                return this._LastName;
            }
             set
            {
                this._LastName = value;
                this.OnPropertyChanged("LastName");
            }
        }

        public string Location
        {
            get
            {
                return this._Location;
            }
             set
            {
                this._Location = value;
                this.OnPropertyChanged("Location");
            }
        }

        public string MoreAboutMe
        {
            get
            {
                return this._MoreAboutMe;
            }
             set
            {
                this._MoreAboutMe = value;
                this.OnPropertyChanged("MoreAboutMe");
            }
        }

        public LiveDataServiceCollection<Activity> MyActivities
        {
            get
            {
                if (this._MyActivities == null)
                {
                    this._MyActivities = new LiveDataServiceCollection<Activity>(this);
                }
                return this._MyActivities;
            }
        }

        public string Occupation
        {
            get
            {
                return this._Occupation;
            }
             set
            {
                this._Occupation = value;
                this.OnPropertyChanged("Occupation");
            }
        }

        public Uri ThumbnailImage
        {
            get
            {
                return this._ThumbnailImage;
            }
             set
            {
                this._ThumbnailImage = value;
                this.OnPropertyChanged("ThumbnailImage");
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

        public string WindowsLiveID
        {
            get
            {
                return this._windowsLiveID;
            }
             set
            {
                this._windowsLiveID = value;
                this.OnPropertyChanged("WindowsLiveID");
            }
        }
    }
}

