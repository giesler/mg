namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class StatusProfile : Profile
    {
        private string _StatusLink;
        private string _StatusText;

        public string StatusLink
        {
            get
            {
                return this._StatusLink;
            }
            set
            {
                this._StatusLink = value;
                this.OnPropertyChanged("StatusLink");
            }
        }

        public string StatusText
        {
            get
            {
                return this._StatusText;
            }
            set
            {
                this._StatusText = value;
                this.OnPropertyChanged("StatusText");
            }
        }
    }
}

