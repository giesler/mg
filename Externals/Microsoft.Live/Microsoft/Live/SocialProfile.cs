namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class SocialProfile : Profile
    {
        private string _Fashion;
        private string _FavoriteQuote;
        private string _Hometown;
        private string _Humor;
        private string _InterestedIn;
        private string _PlacesLived;
        private string _RelationshipStatus;

        public string Fashion
        {
            get
            {
                return this._Fashion;
            }
             set
            {
                this._Fashion = value;
                this.OnPropertyChanged("Fashion");
            }
        }

        public string FavoriteQuote
        {
            get
            {
                return this._FavoriteQuote;
            }
             set
            {
                this._FavoriteQuote = value;
                this.OnPropertyChanged("FavoriteQuote");
            }
        }

        public string Hometown
        {
            get
            {
                return this._Hometown;
            }
             set
            {
                this._Hometown = value;
                this.OnPropertyChanged("Hometown");
            }
        }

        public string Humor
        {
            get
            {
                return this._Humor;
            }
             set
            {
                this._Humor = value;
                this.OnPropertyChanged("Humor");
            }
        }

        public string InterestedIn
        {
            get
            {
                return this._InterestedIn;
            }
             set
            {
                this._InterestedIn = value;
                this.OnPropertyChanged("InterestedIn");
            }
        }

        public string PlacesLived
        {
            get
            {
                return this._PlacesLived;
            }
             set
            {
                this._PlacesLived = value;
                this.OnPropertyChanged("PlacesLived");
            }
        }

        public string RelationshipStatus
        {
            get
            {
                return this._RelationshipStatus;
            }
            set
            {
                this._RelationshipStatus = value;
                this.OnPropertyChanged("RelationshipStatus");
            }
        }
    }
}

