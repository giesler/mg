namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Person : INotifyPropertyChanged
    {
        private string _Cid;
        private string _DeviceName;
        private string _Email;
        private string _Name;
        private System.Uri _Uri;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
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

        public string DeviceName
        {
            get
            {
                return this._DeviceName;
            }
            set
            {
                this._DeviceName = value;
                this.OnPropertyChanged("DeviceName");
            }
        }

        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = value;
                this.OnPropertyChanged("Email");
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

        public System.Uri Uri
        {
            get
            {
                return this._Uri;
            }
            set
            {
                this._Uri = value;
                this.OnPropertyChanged("Uri");
            }
        }
    }
}

