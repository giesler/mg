using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace msn2.net.ShoppingList
{
    [DataContract]
    public class UpdateListItemReturnData
    {
        public UpdateListItemReturnData() { }

        [DataMember]
        public DateTime LastItemChangeTime { get; set; }
    }
}