namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ActivityBookmark : INotifyPropertyChanged
    {
        private string _ActivityObjectType;
        private string _Description;
        private string _Id;
        private Uri _PreviewLink;
        private Uri _TargetLink;
        private string _Title;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnActivityObjectTypeChanged()
        {
        }

        private void OnActivityObjectTypeChanging(string value)
        {
        }

        private void OnDescriptionChanged()
        {
        }

        private void OnDescriptionChanging(string value)
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

        private void OnTargetLinkChanged()
        {
        }

        private void OnTargetLinkChanging(Uri value)
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

        public Uri TargetLink
        {
            get
            {
                return this._TargetLink;
            }
            set
            {
                this.OnTargetLinkChanging(value);
                this._TargetLink = value;
                this.OnTargetLinkChanged();
                this.OnPropertyChanged("TargetLink");
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

