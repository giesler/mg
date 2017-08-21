namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ActivityVideo : INotifyPropertyChanged
    {
        private string _ActivityObjectType;
        private Uri _AlternateLink;
        private Uri _AppletLink;
        private string _Description;
        private Uri _EnclosureLink;
        private string _Id;
        private Uri _PreviewLink;
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

        private void OnAppletLinkChanged()
        {
        }

        private void OnAppletLinkChanging(Uri value)
        {
        }

        private void OnDescriptionChanged()
        {
        }

        private void OnDescriptionChanging(string value)
        {
        }

        private void OnEnclosureLinkChanged()
        {
        }

        private void OnEnclosureLinkChanging(Uri value)
        {
        }

        private void OnIdChanged()
        {
        }

        private void OnIdChanging(string value)
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

        public Uri AppletLink
        {
            get
            {
                return this._AppletLink;
            }
            set
            {
                this.OnAppletLinkChanging(value);
                this._AppletLink = value;
                this.OnAppletLinkChanged();
                this.OnPropertyChanged("AppletLink");
            }
        }

        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this.OnDescriptionChanging(value);
                this._Description = value;
                this.OnDescriptionChanged();
                this.OnPropertyChanged("Description");
            }
        }

        public Uri EnclosureLink
        {
            get
            {
                return this._EnclosureLink;
            }
            set
            {
                this.OnEnclosureLinkChanging(value);
                this._EnclosureLink = value;
                this.OnEnclosureLinkChanged();
                this.OnPropertyChanged("EnclosureLink");
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

