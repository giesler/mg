using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace msn2.net.ShoppingList
{
    [DataContract]
    public class ClientAuthenticationData
    {
        public ClientAuthenticationData() { }

        [DataMember]
        public Guid PersonUniqueId { get; set; }

        [DataMember]
        public Guid DeviceUniqueId { get; set; }
    }
}