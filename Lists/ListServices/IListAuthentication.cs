using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace msn2.net.ShoppingList
{
    [ServiceContract]
    public interface IListAuthentication
    {
        [OperationContract]
        Person GetPerson(string liveUserId, string name);

        [OperationContract]
        void UpdatePerson(ClientAuthenticationData auth, Person person);

        [OperationContract]
        PersonDevice AddDevice(ClientAuthenticationData auth, string deviceName);

        [OperationContract]
        void RemoveDevice(ClientAuthenticationData auth, Guid deviceId);        
    }
}
