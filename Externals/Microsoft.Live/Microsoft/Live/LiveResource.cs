namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Data.Services.Client;
    using System.Data.Services.Common;
    using System.Runtime.CompilerServices;


    [EntityPropertyMappingEx("Updated", SyndicationItemPropertyEx.Updated, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Contributor/Name", SyndicationItemPropertyEx.ContributorName, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Contributor/Uri", SyndicationItemPropertyEx.ContributorUri, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Id", SyndicationItemPropertyEx.Id, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Title", SyndicationItemPropertyEx.Title, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Contributor/Email", SyndicationItemPropertyEx.ContributorEmail, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Published", SyndicationItemPropertyEx.Published, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Author/Name", SyndicationItemPropertyEx.AuthorName, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Author/Email", SyndicationItemPropertyEx.AuthorEmail, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMappingEx("Author/Uri", SyndicationItemPropertyEx.AuthorUri, SyndicationTextContentKind.Plaintext, false)]

 


    [EntityPropertyMapping("Title", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, false), 
    EntityPropertyMapping("Contributor/Email", SyndicationItemProperty.ContributorEmail, SyndicationTextContentKind.Plaintext, false), 
    EntityPropertyMapping("Contributor/Uri", SyndicationItemProperty.ContributorUri, SyndicationTextContentKind.Plaintext, false), 
    EntityPropertyMapping("Contributor/Name", SyndicationItemProperty.ContributorName, SyndicationTextContentKind.Plaintext, false), 
    DataServiceKey("Id"), 
    //EntityPropertyMapping("Id",SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, false), 
    EntityPropertyMapping("Updated", SyndicationItemProperty.Updated, SyndicationTextContentKind.Plaintext, false), 
    EntityPropertyMapping("Published", SyndicationItemProperty.Published, SyndicationTextContentKind.Plaintext, false), 
    EntityPropertyMapping("Author/Name", SyndicationItemProperty.AuthorName, SyndicationTextContentKind.Plaintext, false), 
    EntityPropertyMapping("Author/Email", SyndicationItemProperty.AuthorEmail, SyndicationTextContentKind.Plaintext, false),
    EntityPropertyMapping("Author/Uri", SyndicationItemProperty.AuthorUri, SyndicationTextContentKind.Plaintext, false)]
    public class LiveResource : INotifyPropertyChanged
    {
        private Person _Author;
        private DataServiceCollection<Profile> _AuthorProfiles = new DataServiceCollection<Profile>(null, TrackingMode.None);
        private Person _Contributor;
        private DataServiceCollection<Profile> _ContributorProfiles = new DataServiceCollection<Profile>(null, TrackingMode.None);
        private string _Id;
        private bool _IsDeleted;
        private DateTime _Published;
        private string _Title;
        private DateTime _Updated;
        private LiveDataContext dataContext;

        public event PropertyChangedEventHandler PropertyChanged;

        public LiveDataContext GetDataContext()
        {
            return this.dataContext;
        }

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        internal void SetDataContext(LiveDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public Person Author
        {
            get
            {
                return this._Author;
            }
            set
            {
                this._Author = value;
                this.OnPropertyChanged("Author");
            }
        }

        public DataServiceCollection<Profile> AuthorProfiles
        {
            get
            {
                return this._AuthorProfiles;
            }
            set
            {
                this._AuthorProfiles = value;
                this.OnPropertyChanged("AuthorProfiles");
            }
        }

        public Person Contributor
        {
            get
            {
                return this._Contributor;
            }
            set
            {
                this._Contributor = value;
                this.OnPropertyChanged("Contributor");
            }
        }

        public DataServiceCollection<Profile> ContributorProfiles
        {
            get
            {
                return this._ContributorProfiles;
            }
            set
            {
                this._ContributorProfiles = value;
                this.OnPropertyChanged("ContributorProfiles");
            }
        }

        public string Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public bool IsDeleted
        {
            get
            {
                return this._IsDeleted;
            }
            set
            {
                this._IsDeleted = value;
                this.OnPropertyChanged("IsDeleted");
            }
        }

        public DateTime Published
        {
            get
            {
                return this._Published;
            }
            set
            {
                this._Published = value;
                this.OnPropertyChanged("Published");
            }
        }

        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
                this.OnPropertyChanged("Title");
            }
        }

        public DateTime Updated
        {
            get
            {
                return this._Updated;
            }
            set
            {
                this._Updated = value;
                this.OnPropertyChanged("Updated");
            }
        }
    }
}

