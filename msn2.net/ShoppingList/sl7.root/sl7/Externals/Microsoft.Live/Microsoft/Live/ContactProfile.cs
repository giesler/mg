namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class ContactProfile : Profile
    {
        private DateTime _Anniversary;
        private byte _BirthDay;
        private byte _BirthMonth;
        private short _BirthYear;
        private string _City;
        private string _CountryRegion;
        private string _HomeAddress;
        private string _HomeAddress2;
        private string _HomeFax;
        private string _HomePhone;
        private string _MobilePhone;
        private string _PersonalEmail;
        private string _PersonalIM;
        private string _PostalCode;
        private string _SignificantOther;
        private string _State;

        public DateTime Anniversary
        {
            get
            {
                return this._Anniversary;
            }
             set
            {
                this._Anniversary = value;
                this.OnPropertyChanged("Anniversary");
            }
        }

        public byte BirthDay
        {
            get
            {
                return this._BirthDay;
            }
             set
            {
                this._BirthDay = value;
                this.OnPropertyChanged("BirthDay");
            }
        }

        public byte BirthMonth
        {
            get
            {
                return this._BirthMonth;
            }
             set
            {
                this._BirthMonth = value;
                this.OnPropertyChanged("BirthMonth");
            }
        }

        public short BirthYear
        {
            get
            {
                return this._BirthYear;
            }
             set
            {
                this._BirthYear = value;
                this.OnPropertyChanged("BirthYear");
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

        public string HomeAddress
        {
            get
            {
                return this._HomeAddress;
            }
             set
            {
                this._HomeAddress = value;
                this.OnPropertyChanged("HomeAddress");
            }
        }

        public string HomeAddress2
        {
            get
            {
                return this._HomeAddress2;
            }
             set
            {
                this._HomeAddress2 = value;
                this.OnPropertyChanged("HomeAddress2");
            }
        }

        public string HomeFax
        {
            get
            {
                return this._HomeFax;
            }
             set
            {
                this._HomeFax = value;
                this.OnPropertyChanged("HomeFax");
            }
        }

        public string HomePhone
        {
            get
            {
                return this._HomePhone;
            }
             set
            {
                this._HomePhone = value;
                this.OnPropertyChanged("HomePhone");
            }
        }

        public string MobilePhone
        {
            get
            {
                return this._MobilePhone;
            }
             set
            {
                this._MobilePhone = value;
                this.OnPropertyChanged("MobilePhone");
            }
        }

        public string PersonalEmail
        {
            get
            {
                return this._PersonalEmail;
            }
             set
            {
                this._PersonalEmail = value;
                this.OnPropertyChanged("PersonalEmail");
            }
        }

        public string PersonalIM
        {
            get
            {
                return this._PersonalIM;
            }
             set
            {
                this._PersonalIM = value;
                this.OnPropertyChanged("PersonalIM");
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

        public string SignificantOther
        {
            get
            {
                return this._SignificantOther;
            }
             set
            {
                this._SignificantOther = value;
                this.OnPropertyChanged("SignificantOther");
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
    }
}

