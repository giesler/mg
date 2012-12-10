namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class CategoryInfo : INotifyPropertyChanged
    {
        private Uri _CategoryLink;
        private string _Id;
        private string _Name;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public Uri CategoryLink
        {
            get
            {
                return this._CategoryLink;
            }
             set
            {
                this._CategoryLink = value;
                this.OnPropertyChanged("CategoryLink");
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

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                this.OnPropertyChanged("Name");
            }
        }
    }
}

