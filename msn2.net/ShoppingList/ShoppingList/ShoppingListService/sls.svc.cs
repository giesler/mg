using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Net;

namespace msn2.net.ShoppingList
{
    public class ShoppingListService : IShoppingListService
    {
        private SLSDataDataContext dataContext;

        public ShoppingListService()
        {
            this.dataContext = new SLSDataDataContext();
        }

        public List<string> GetStores()
        {
            var q = from s in this.dataContext.Stores
                    orderby s.SortOrder
                    select s.Name;

            return q.ToList<string>();
        }

        public List<ShoppingListItem> GetShoppingListItems()
        {
            return this.GetShoppingListItemsForStore(null);
        }

        public List<ShoppingListItem> GetShoppingListItemsForStore(string store)
        {
            var q = from si in this.dataContext.StoreItems
                    where (store == null) || (si.Store.Name == store)
                    orderby si.Name
                    select si;

            List<ShoppingListItem> results = new List<ShoppingListItem>();

            foreach (var i in q)
            {
                ShoppingListItem item = new ShoppingListItem() { Id = i.Id, ListItem = i.Name, Store = i.Store.Name };
                results.Add(item);
            } 

            return results;
        }

        public ShoppingListItem AddShoppingListItem(string storeName, string listItem)
        {
            List list = (from l in this.dataContext.Lists
                         where l.Name == storeName && l.ListPersons.Any(p => p.PersonId == 1)
                         select l).First();

            ListItem item = new ListItem{ Name=listItem, ListId = list.Id, AddedByPersonId=1, UniqueId=Guid.NewGuid()};
            item.AddedTime = DateTime.UtcNow;
            item.ChangeDeviceId = 1;
            item.ChangedTime = DateTime.UtcNow;
            
            this.dataContext.ListItems.InsertOnSubmit(item);
            this.dataContext.SubmitChanges();

            return new ShoppingListItem { Id = item.Id, ListItem = item.Name, Store = item.List.Name };
        }

        public void UpdateShoppingListItem(ShoppingListItem listItem)
        {
            StoreItem item = (from si in this.dataContext.StoreItems
                             where si.Id == listItem.Id
                             select si).First<StoreItem>();

            item.Name = listItem.ListItem;
            this.dataContext.SubmitChanges();
        }

        public void DeleteShoppingListItem(ShoppingListItem listItem)
        {
            ListItem item = (from li in this.dataContext.ListItems
                             where li.Id == listItem.Id
                             select li).First();

            this.dataContext.ListItems.DeleteOnSubmit(item);
            this.dataContext.SubmitChanges();
        }
    }
}
