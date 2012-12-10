namespace Microsoft.Live
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Location : INotifyPropertyChanged
    {
        private string _City;
        private string _CompanyName;
        private string _CountryRegion;
        private string _Department;
        private string _Formatted;
        private bool _IsPrimary;
        private double _Latitude;
        private double _Longitude;
        private string _PostalCode;
        private string _State;
        private string _StreetAddress;
        private string _Type = "ContactLocationPersonal";
        private string _Value;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string City
        {
            get
            {
                return this._City;
            }
            set
            {
                this._City = value;
                this.OnPropertyChanged("City");
            }
        }

        public string CompanyName
        {
            get
            {
                return this._CompanyName;
            }
            set
            {
                this._CompanyName = value;
                this.OnPropertyChanged("CompanyName");
            }
        }

        public string CountryRegion
        {
            get
            {
                return this._CountryRegion;
            }
            set
            {
                this._CountryRegion = value;
                this.OnPropertyChanged("CountryRegion");
            }
        }

        public string Department
        {
            get
            {
                return this._Department;
            }
            set
            {
                this._Department = value;
                this.OnPropertyChanged("Department");
            }
        }

        public string Formatted
        {
            get
            {
                return this._Formatted;
            }
            set
            {
                this._Formatted = value;
                this.OnPropertyChanged("Formatted");
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

        public double Latitude
        {
            get
            {
                return this._Latitude;
            }
            set
            {
                this._Latitude = value;
                this.OnPropertyChanged("Latitude");
            }
        }

        public double Longitude
        {
            get
            {
                return this._Longitude;
            }
            set
            {
                this._Longitude = value;
                this.OnPropertyChanged("Longitude");
            }
        }

        public string PostalCode
        {
            get
            {
                return this._PostalCode;
            }
            set
            {
                this._PostalCode = value;
                this.OnPropertyChanged("PostalCode");
            }
        }

        public string State
        {
            get
            {
                return this._State;
            }
            set
            {
                this._State = value;
                this.OnPropertyChanged("State");
            }
        }

        public string StreetAddress
        {
            get
            {
                return this._StreetAddress;
            }
            set
            {
                this._StreetAddress = value;
                this.OnPropertyChanged("StreetAddress");
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

        public string Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
                this.OnPropertyChanged("Value");
            }
        }
    }
}

