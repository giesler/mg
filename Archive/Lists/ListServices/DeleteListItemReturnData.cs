using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msn2.net.ShoppingList
{
    [Serializable]
    public class DeleteListItemReturnData
    {
        public DeleteListItemReturnData() { }

        public DateTime LastItemChangeTime { get; set; }
    }        
}