using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.ShoppingList
{
    public class ListDataService : IListDataService
    {

        SLSDataDataContext context = null;

        public ListDataService()
        {
            this.context = new SLSDataDataContext();
        }


        #region IListDataService Members

        public AddListReturnValue AddList(ClientAuthenticationData auth, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }

            AddListReturnValue returnVal = new AddListReturnValue();

            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);

            Guid listUniqueId = Guid.NewGuid();
            bool? isDupe = false;
            int? listId = 0;
            this.context.AddList(person.Id, name.Trim(), listUniqueId, ref isDupe, ref listId);

            if (isDupe.HasValue && isDupe.Value)
            {
                returnVal.IsDuplicate = true;
            }
            else
            {
                returnVal.List = new List { Id = listId.Value, Name = name.Trim(), UniqueId = listUniqueId };
            }

            return returnVal;
        }

        public UpdateListReturnValue UpdateList(ClientAuthenticationData auth, List list)
        {
            UpdateListReturnValue returnVal = new UpdateListReturnValue();

            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);

            bool? isDuplicate = false;
            bool? isInvalidId = false;
            this.context.UpdateList(person.Id, list.Name, list.UniqueId, ref isDuplicate, ref isInvalidId);

            if (isDuplicate.HasValue && isDuplicate.Value)
            {
                returnVal.IsDuplicate = true;
            }
            else if (isInvalidId.HasValue && isInvalidId.Value)
            {
                returnVal.IsInvalidId = true;
            }

            return returnVal;
        }

        public void DeleteList(ClientAuthenticationData auth, Guid uniqueId)
        {
            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
            this.context.DeleteList(person.Id, uniqueId);
        }

        public List<GetListsResult> GetLists(ClientAuthenticationData auth)
        {
            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
            var q = this.context.GetLists(person.Id);
            return q.ToList();
        }

        public List<GetAllListItemsResult> GetListItems(ClientAuthenticationData auth, Guid listUniqueId)
        {
            List<GetAllListItemsResult> items = null;

            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
            var q = this.context.GetAllListItems(person.Id).Where(l => l.ListUniqueId == listUniqueId);
            items = q.ToList();
            this.context.Dispose();

            return items;
        }

        public List<GetAllListItemsResult> GetAllListItems(ClientAuthenticationData auth)
        {
            List<GetAllListItemsResult> items = null;

            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
            var q = this.context.GetAllListItems(person.Id);
            items = q.ToList();

            return items;
        }

        public AddListItemReturnValue AddListItem(ClientAuthenticationData auth, Guid listUniqueId, string name)
        {
            AddListItemReturnValue returnVal = new AddListItemReturnValue();

            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
            bool? isDuplicate = false;
            Guid itemUniqueId = Guid.NewGuid();
            int? listItemId = 0;

            this.context.AddListItem(person.Id, auth.DeviceUniqueId, name, listUniqueId, itemUniqueId, ref isDuplicate, ref listItemId);

            if (isDuplicate.HasValue && isDuplicate.Value)
            {
                returnVal.IsDuplicate = true;
            }
            else
            {
                returnVal.ListItem = new ListItem { Name = name, UniqueId = itemUniqueId };
            }

            return returnVal;
        }

        public UpdateListItemReturnData UpdateListItem(ClientAuthenticationData auth, ListItem item)
        {
            UpdateListItemReturnData data = new UpdateListItemReturnData();

            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
            this.context.UpdateListItem(person.Id, 0, item.UniqueId, item.Name);

            return data;
        }

        public DeleteListItemReturnData DeleteListItem(ClientAuthenticationData auth, Guid listItemUniqueId)
        {
            DeleteListItemReturnData data = new DeleteListItemReturnData();

            Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
            this.context.DeleteListItem(person.Id, listItemUniqueId);

            return data;
        }

        public DateTime GetLastChangeTime(ClientAuthenticationData auth)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

