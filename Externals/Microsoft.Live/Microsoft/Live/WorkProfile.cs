namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class WorkProfile : Profile
    {
        private string _City;
        private string _Company;
        private string _CountryRegion;
        private string _JobTitle;
        private string _PostalCode;
        private string _Profession;
        private string _State;
        private string _WorkAddress;
        private string _WorkAddress2;
        private string _WorkEmail;
        private string _WorkFax;
        private string _WorkPager;
        private string _WorkPhone;

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

        public string Company
        {
            get
            {
                return this._Company;
            }
             set
            {
                this._Company = value;
                this.OnPropertyChanged("Company");
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

        public string JobTitle
        {
            get
            {
                return this._JobTitle;
            }
             set
            {
                this._JobTitle = value;
                this.OnPropertyChanged("JobTitle");
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

        public string Profession
        {
            get
            {
                return this._Profession;
            }
             set
            {
                this._Profession = value;
                this.OnPropertyChanged("Profession");
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

        public string WorkAddress
        {
            get
            {
                return this._WorkAddress;
            }
             set
            {
                this._WorkAddress = value;
                this.OnPropertyChanged("WorkAddress");
            }
        }

        public string WorkAddress2
        {
            get
            {
                return this._WorkAddress2;
            }
             set
            {
                this._WorkAddress2 = value;
                this.OnPropertyChanged("WorkAddress2");
            }
        }

        public string WorkEmail
        {
            get
            {
                return this._WorkEmail;
            }
             set
            {
                this._WorkEmail = value;
                this.OnPropertyChanged("WorkEmail");
            }
        }

        public string WorkFax
        {
            get
            {
                return this._WorkFax;
            }
             set
            {
                this._WorkFax = value;
                this.OnPropertyChanged("WorkFax");
            }
        }

        public string WorkPager
        {
            get
            {
                return this._WorkPager;
            }
             set
            {
                this._WorkPager = value;
                this.OnPropertyChanged("WorkPager");
            }
        }

        public string WorkPhone
        {
            get
            {
                return this._WorkPhone;
            }
             set
            {
                this._WorkPhone = value;
                this.OnPropertyChanged("WorkPhone");
            }
        }
    }
}

