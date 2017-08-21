namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ContactEmail : INotifyPropertyChanged
    {
        private string _Address;
        private bool _IsIMEnabled;
        private string _Type = "Personal";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                this._Address = value;
                this.OnPropertyChanged("Address");
            }
        }

        public bool IsIMEnabled
        {
            get
            {
                return this._IsIMEnabled;
            }
            set
            {
                this._IsIMEnabled = value;
                this.OnPropertyChanged("IsIMEnabled");
            }
        }

        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
                this.OnPropertyChanged("Type");
            }
        }
    }
}

