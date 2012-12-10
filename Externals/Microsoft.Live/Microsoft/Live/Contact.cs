namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Services.Common;
    using System.Runtime.CompilerServices;

    //[EntityPropertyMapping("ThumbnailImage", SyndicationItemProperty.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/ThumbnailImage"), EntitySet("AllContacts"), DataServiceKey("Id")]
    [EntityPropertyMappingEx("ThumbnailImage", SyndicationItemPropertyEx.LinkHref, SyndicationTextContentKind.Plaintext, false, "rel", "http://schemas.microsoft.com/ado/2007/08/dataservices/mediaresource/ThumbnailImage")]
    [
    //EntityPropertyMapping("ThumbnailImage", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false), 
    EntitySet("AllContacts"), DataServiceKey("Id")]
    public class Contact : LiveResource
    {
        private DateTime _Birthday;
        private ObservableCollection<CategoryInfo> _Categories = new ObservableCollection<CategoryInfo>();
        private string _Cid;
        private ObservableCollection<ContactEmail> _Emails = new ObservableCollection<ContactEmail>();
        private string _FirstName;
        private string _FormattedName;
        private string _HonorificPrefix;
        private string _HonorificSuffix;
        private bool _IsFriend;
        private bool _IsIMEnabled;
        private string _JobTitle;
        private string _LastName;
        private ObservableCollection<Location> _Locations = new ObservableCollection<Location>();
        private string _MiddleName;
        private ObservableCollection<ContactPhone> _PhoneNumbers = new ObservableCollection<ContactPhone>();
        private LiveDataServiceCollection<Profile> _Profiles;
        private Uri _ThumbnailImage;
        private ObservableCollection<ContactUrl> _Urls = new ObservableCollection<ContactUrl>();
        private string _WindowsLiveID;

        public string UrcOffset { get; set; }
        //[CompilerGenerated]
        //private string <UtcOffset>k__BackingField;

        public DateTime Birthday
        {
            get
            {
                return this._Birthday;
            }
            set
            {
                this._Birthday = value;
                this.OnPropertyChanged("Birthday");
            }
        }

        public ObservableCollection<CategoryInfo> Categories
        {
            get
            {
                return this._Categories;
            }
            set
            {
                if (value != null)
                {
                    this._Categories = value;
                }
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

        public ObservableCollection<ContactEmail> Emails
        {
            get
            {
                return this._Emails;
            }
            set
            {
                if (value != null)
                {
                    this._Emails = value;
                }
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

        public string FormattedName
        {
            get
            {
                return this._FormattedName;
            }
            set
            {
                this._FormattedName = value;
                this.OnPropertyChanged("FormattedName");
            }
        }

        public string HonorificPrefix
        {
            get
            {
                return this._HonorificPrefix;
            }
            set
            {
                this._HonorificPrefix = value;
                this.OnPropertyChanged("HonorificPrefix");
            }
        }

        public string HonorificSuffix
        {
            get
            {
                return this._HonorificSuffix;
            }
            set
            {
                this._HonorificSuffix = value;
                this.OnPropertyChanged("HonorificSuffix");
            }
        }

        public bool IsFriend
        {
            get
            {
                return this._IsFriend;
            }
             set
            {
                this._IsFriend = value;
                this.OnPropertyChanged("IsFriend");
            }
        }

        public bool IsIMEnabled
        {
            get
            {
                return this._IsIMEnabled;
            }
            set
            {
                this._IsIMEnabled = value;
                this.OnPropertyChanged("IsIMEnabled");
            }
        }

        public string JobTitle
        {
            get
            {
                return this._JobTitle;
            }
            set
            {
                this._JobTitle = value;
                this.OnPropertyChanged("JobTitle");
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

        public ObservableCollection<Location> Locations
        {
            get
            {
                return this._Locations;
            }
            set
            {
                if (value != null)
                {
                    this._Locations = value;
                }
            }
        }

        public string MiddleName
        {
            get
            {
                return this._MiddleName;
            }
            set
            {
                this._MiddleName = value;
                this.OnPropertyChanged("MiddleName");
            }
        }

        public ObservableCollection<ContactPhone> PhoneNumbers
        {
            get
            {
                return this._PhoneNumbers;
            }
            set
            {
                if (value != null)
                {
                    this._PhoneNumbers = value;
                }
            }
        }

        public LiveDataServiceCollection<Profile> Profiles
        {
            get
            {
                if (this._Profiles == null)
                {
                    this._Profiles = new LiveDataServiceCollection<Profile>(this);
                }
                return this._Profiles;
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

        public ObservableCollection<ContactUrl> Urls
        {
            get
            {
                return this._Urls;
            }
            set
            {
                if (value != null)
                {
                    this._Urls = value;
                }
            }
        }

        //internal string UtcOffset
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<UtcOffset>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<UtcOffset>k__BackingField = value;
        //    }
        //}

        public string WindowsLiveID
        {
            get
            {
                return this._WindowsLiveID;
            }
            set
            {
                this._WindowsLiveID = value;
                this.OnPropertyChanged("WindowsLiveID");
            }
        }
    }
}

