using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
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

            try
            {
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
            }
            finally
            {
                this.context.Dispose();
            }

            return returnVal;
        }

        public UpdateListReturnValue UpdateList(ClientAuthenticationData auth, List list)
        {
            UpdateListReturnValue returnVal = new UpdateListReturnValue();

            try
            {
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
            }
            finally
            {
                this.context.Dispose();
            }

            return returnVal;
        }

        public void DeleteList(ClientAuthenticationData auth, Guid uniqueId)
        {
            try
            {
                Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
                this.context.DeleteList(person.Id, uniqueId);
            }
            finally
            {
                this.context.Dispose();
            }
        }

        public List<GetListsResult> GetLists(ClientAuthenticationData auth)
        {
            try
            {
                Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
                var q = this.context.GetLists(person.Id);
                return q.ToList();
            }
            finally
            {
                this.context.Dispose();
            }
        }

        public List<GetAllListItemsResult> GetListItems(ClientAuthenticationData auth, Guid listUniqueId)
        {
            List<GetAllListItemsResult> items = null;

            try
            {
                Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
                var q = this.context.GetAllListItems(person.Id).Where(l => l.ListUniqueId == listUniqueId);
                items = q.ToList();
            }
            finally
            {
                this.context.Dispose();
            }

            return items;
        }

        public List<GetAllListItemsResult> GetAllListItems(ClientAuthenticationData auth)
        {
            List<GetAllListItemsResult> items = null;

            try
            {
                Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
                var q = this.context.GetAllListItems(person.Id);
                items = q.ToList();                
            }
            finally
            {
                this.context.Dispose();
            }

            return items;
        }

        public AddListItemReturnValue AddListItem(ClientAuthenticationData auth, Guid listUniqueId, string name)
        {
            AddListItemReturnValue returnVal = new AddListItemReturnValue();

            try
            {
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
            }
            finally
            {
                this.context.Dispose();
            }

            return returnVal;
        }

        public UpdateListItemReturnData UpdateListItem(ClientAuthenticationData auth, ListItem item)
        {
            UpdateListItemReturnData data = new UpdateListItemReturnData();

            try
            {
                Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
                this.context.UpdateListItem(person.Id, 0, item.UniqueId, item.Name);
            }
            finally
            {
                this.context.Dispose();
            }

            return data;
        }

        public DeleteListItemReturnData DeleteListItem(ClientAuthenticationData auth, Guid listItemUniqueId)
        {
            DeleteListItemReturnData data = new DeleteListItemReturnData();

            try
            {
                Person person = ListAuthentication.ValidateAuth(this.context, auth, true);
                this.context.DeleteListItem(person.Id, listItemUniqueId);
            }
            finally
            {
                this.context.Dispose();
            }

            return data;
        }

        public DateTime GetLastChangeTime(ClientAuthenticationData auth)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
