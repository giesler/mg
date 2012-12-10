namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [EntitySet("Profiles"), DataServiceKey("Id")]
    public class Profile : LiveResource
    {
        private string _ProfileType;

        public Profile()
        {
        }

        public string ProfileType
        {
            get
            {
                return this._ProfileType;
            }
             set
            {
                this._ProfileType = value;
                this.OnPropertyChanged("ProfileType");
            }
        }
    }
}

