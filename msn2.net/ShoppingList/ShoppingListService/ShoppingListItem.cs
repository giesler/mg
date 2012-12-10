using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace msn2.net.ShoppingList
{
    [DataContract]
    public class ShoppingListItem
    {
        string store = "";
        string listItem = "";
        int id = 0;

        [DataMember]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        [DataMember]
        public string Store
        {
            get { return this.store; }
            set { this.store = value; }
        }

        [DataMember]
        public string ListItem
        {
            get { return this.listItem; }
            set { this.listItem = value; }
        }
    }
}
