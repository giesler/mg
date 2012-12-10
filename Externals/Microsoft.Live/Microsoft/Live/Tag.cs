namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Common;

    [EntitySet("Tags"), DataServiceKey("Id")]
    public class Tag : LiveResource
    {
        private string _Cid;
        private TagCoordinates _Coordinates;
        private LiveDataServiceCollection<Profile> _Profiles;

        public string Cid
        {
            get
            {
                return this._Cid;
            }
            set
            {
                this._Cid = value;
            }
        }

        public TagCoordinates Coordinates
        {
            get
            {
                return this._Coordinates;
            }
            set
            {
                this._Coordinates = value;
            }
        }

        public LiveDataServiceCollection<Profile> Profiles
        {
            get
            {
                if (this._Profiles == null)
                {
                    this._Profiles = new LiveDataServiceCollection<Profile>(this);
                }
                return this._Profiles;
            }
        }
    }
}

