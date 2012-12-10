namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ActivityReview : INotifyPropertyChanged
    {
        private string _ActivityObjectType;
        private Uri _AlternateLink;
        private string _Content;
        private string _Id;
        private Uri _PermaLink;
        private Uri _PreviewLink;
        private int _Rating;
        private string _Title;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnActivityObjectTypeChanged()
        {
        }

        private void OnActivityObjectTypeChanging(string value)
        {
        }

        private void OnAlternateLinkChanged()
        {
        }

        private void OnAlternateLinkChanging(Uri value)
        {
        }

        private void OnContentChanged()
        {
        }

        private void OnContentChanging(string value)
        {
        }

        private void OnIdChanged()
        {
        }

        private void OnIdChanging(string value)
        {
        }

        private void OnPermaLinkChanged()
        {
        }

        private void OnPermaLinkChanging(Uri value)
        {
        }

        private void OnPreviewLinkChanged()
        {
        }

        private void OnPreviewLinkChanging(Uri value)
        {
        }

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void OnRatingChanged()
        {
        }

        private void OnRatingChanging(int value)
        {
        }

        private void OnReviewedObjectChanged()
        {
        }

        private void OnReviewedObjectChanging(Uri value)
        {
        }

        private void OnTitleChanged()
        {
        }

        private void OnTitleChanging(string value)
        {
        }

        public string ActivityObjectType
        {
            get
            {
                return this._ActivityObjectType;
            }
            set
            {
                this.OnActivityObjectTypeChanging(value);
                this._ActivityObjectType = value;
                this.OnActivityObjectTypeChanged();
                this.OnPropertyChanged("ActivityObjectType");
            }
        }

        public Uri AlternateLink
        {
            get
            {
                return this._AlternateLink;
            }
            set
            {
                this.OnAlternateLinkChanging(value);
                this._AlternateLink = value;
                this.OnAlternateLinkChanged();
                this.OnPropertyChanged("AlternateLink");
            }
        }

        public string Content
        {
            get
            {
                return this._Content;
            }
            set
            {
                this.OnContentChanging(value);
                this._Content = value;
                this.OnContentChanged();
                this.OnPropertyChanged("Content");
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
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }

        public Uri PermaLink
        {
            get
            {
                return this._PermaLink;
            }
            set
            {
                this.OnPermaLinkChanging(value);
                this._PermaLink = value;
                this.OnPermaLinkChanged();
                this.OnPropertyChanged("PermaLink");
            }
        }

        public Uri PreviewLink
        {
            get
            {
                return this._PreviewLink;
            }
            set
            {
                this.OnPreviewLinkChanging(value);
                this._PreviewLink = value;
                this.OnPreviewLinkChanged();
                this.OnPropertyChanged("PreviewLink");
            }
        }

        public int Rating
        {
            get
            {
                return this._Rating;
            }
            set
            {
                this.OnRatingChanging(value);
                this._Rating = value;
                this.OnRatingChanged();
                this.OnPropertyChanged("Rating");
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
                this.OnTitleChanging(value);
                this._Title = value;
                this.OnTitleChanged();
                this.OnPropertyChanged("Title");
            }
        }
    }
}

