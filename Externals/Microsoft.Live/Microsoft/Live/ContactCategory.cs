namespace Microsoft.Live
{
    using System.Data.Services.Common;

    [EntitySet("Categories"), DataServiceKey("Id")]
    public class ContactCategory : LiveResource
    {
        private LiveDataServiceCollection<Contact> _Contacts;

        public LiveDataServiceCollection<Contact> Contacts
        {
            get
            {
                if (this._Contacts == null)
                {
                    this._Contacts = new LiveDataServiceCollection<Contact>(this);
                }
                return this._Contacts;
            }
        }
    }
}

