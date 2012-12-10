using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace msn2.net.ShoppingList
{
    [ServiceContract]
    public interface IShoppingListService
    {
        [OperationContract]
        List<string> GetStores();

        [OperationContract]
        List<ShoppingListItem> GetShoppingListItems();

        [OperationContract]
        List<ShoppingListItem> GetShoppingListItemsForStore(string store);

        [OperationContract]
        ShoppingListItem AddShoppingListItem(string store, string listItem);

        [OperationContract]
        void DeleteShoppingListItem(ShoppingListItem listItem);
    }
}
