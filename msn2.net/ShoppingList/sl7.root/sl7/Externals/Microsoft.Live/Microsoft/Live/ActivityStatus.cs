namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ActivityStatus : INotifyPropertyChanged
    {
        private string _ActivityObjectType;
        private Uri _AlternateLink;
        private string _Content;
        private string _Id;

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

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
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
    }
}

