namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ActivityList : INotifyPropertyChanged
    {
        private string _ActivityObjectType;
        private Uri _AlternateLink;
        private string _Id;
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

