namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ContactPhone : INotifyPropertyChanged
    {
        private bool _IsIMEnabled;
        private bool _IsPrimary;
        private string _Number;
        private string _Type = "Personal";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
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

        public bool IsPrimary
        {
            get
            {
                return this._IsPrimary;
            }
            set
            {
                this._IsPrimary = value;
                this.OnPropertyChanged("IsPrimary");
            }
        }

        public string Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                this._Number = value;
                this.OnPropertyChanged("Number");
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

