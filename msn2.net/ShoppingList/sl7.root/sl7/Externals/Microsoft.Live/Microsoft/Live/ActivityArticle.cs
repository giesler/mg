namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ActivityArticle : INotifyPropertyChanged
    {
        private string _ActivityObjectType;
        private string _Content;
        private string _Id;
        private Uri _PermaLink;
        private string _Summary;
        private string _Title;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnActivityObjectTypeChanged()
        {
        }

        private void OnActivityObjectTypeChanging(string value)
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

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void OnSummaryChanged()
        {
        }

        private void OnSummaryChanging(string value)
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

        public string Summary
        {
            get
            {
                return this._Summary;
            }
            set
            {
                this.OnSummaryChanging(value);
                this._Summary = value;
                this.OnSummaryChanged();
                this.OnPropertyChanged("Summary");
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

