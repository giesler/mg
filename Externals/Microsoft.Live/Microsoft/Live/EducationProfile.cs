namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class EducationProfile : Profile
    {
        private string _Degree;
        private string _GraduationYear;
        private string _GreekOrganization;
        private string _Major;
        private string _University;
        private string _UniversityEndYear;
        private string _UniversityStartYear;

        public string Degree
        {
            get
            {
                return this._Degree;
            }
            internal set
            {
                this._Degree = value;
                this.OnPropertyChanged("Degree");
            }
        }

        public string GraduationYear
        {
            get
            {
                return this._GraduationYear;
            }
            internal set
            {
                this._GraduationYear = value;
                this.OnPropertyChanged("GraduationYear");
            }
        }

        public string GreekOrganization
        {
            get
            {
                return this._GreekOrganization;
            }
            internal set
            {
                this._GreekOrganization = value;
                this.OnPropertyChanged("GreekOrganization");
            }
        }

        public string Major
        {
            get
            {
                return this._Major;
            }
            internal set
            {
                this._Major = value;
                this.OnPropertyChanged("Major");
            }
        }

        public string University
        {
            get
            {
                return this._University;
            }
            internal set
            {
                this._University = value;
                this.OnPropertyChanged("University");
            }
        }

        public string UniversityEndYear
        {
            get
            {
                return this._UniversityEndYear;
            }
            internal set
            {
                this._UniversityEndYear = value;
                this.OnPropertyChanged("UniversityEndYear");
            }
        }

        public string UniversityStartYear
        {
            get
            {
                return this._UniversityStartYear;
            }
            internal set
            {
                this._UniversityStartYear = value;
                this.OnPropertyChanged("UniversityStartYear");
            }
        }
    }
}

