using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;

namespace msn2.net.ShoppingList
{
    public class ListDataService : IListDataService
    {
        SLSDataDataContext context = null;

        public ListDataService()
        {
            this.context = new SLSDataDataContext();
        }

        void WaitForServerCallSleepTime()
        {
            string sleepTime = ConfigurationManager.AppSettings["serverCallSleepTime"];
            if (!string.IsNullOrEmpty(sleepTime))
            {
                TimeSpan ts = TimeSpan.Parse(sleepTime);
                Thread.Sleep(ts);
            }
        }

        #region IListDataService Members

        public AddListReturnValue AddList(ClientAuthenticationData auth, string name)
        {
            this.WaitForServerCallSleepTime();
            
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }
                        
            AddListReturnValue returnVal = new AddListReturnValue();

            Person person = ValidateAuth(this.context, auth, true);

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
            this.WaitForServerCallSleepTime();

            UpdateListReturnValue returnVal = new UpdateListReturnValue();

            Person person = ValidateAuth(this.context, auth, true);

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
            this.WaitForServerCallSleepTime();

            Person person = ValidateAuth(this.context, auth, true);
            this.context.DeleteList(person.Id, uniqueId);
        }

        public List<GetListsResult> GetLists(ClientAuthenticationData auth)
        {
            this.WaitForServerCallSleepTime();

            Person person = ValidateAuth(this.context, auth, true);
            var q = this.context.GetLists(person.Id);
            return q.ToList();
        }

        public List<GetAllListItemsResult> GetListItems(ClientAuthenticationData auth, Guid listUniqueId)
        {
            this.WaitForServerCallSleepTime();

            List<GetAllListItemsResult> items = null;

            Person person = ValidateAuth(this.context, auth, true);
            var q = this.context.GetAllListItems(person.Id).Where(l => l.ListUniqueId == listUniqueId);
            items = q.ToList();
            this.context.Dispose();

            return items;
        }

        public List<GetAllListItemsResult> GetAllListItems(ClientAuthenticationData auth)
        {
            this.WaitForServerCallSleepTime();

            List<GetAllListItemsResult> items = null;

            Person person = ValidateAuth(this.context, auth, true);
            var q = this.context.GetAllListItems(person.Id);
            items = q.ToList();

            return items;
        }

        public List<GetAllResult> GetAll(ClientAuthenticationData auth)
        {
            this.WaitForServerCallSleepTime();

            List<GetAllResult> result = null;

            Person person = ValidateAuth(this.context, auth, true);
            var q = this.context.GetAll(person.Id);
            result = q.ToList();

            return result;        
        }

        public AddListItemReturnValue AddListItem(ClientAuthenticationData auth, Guid listUniqueId, string name)
        {
            this.WaitForServerCallSleepTime();

            AddListItemReturnValue returnVal = new AddListItemReturnValue();

            Person person = ValidateAuth(this.context, auth, true);
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
            this.WaitForServerCallSleepTime();

            UpdateListItemReturnData data = new UpdateListItemReturnData();

            Person person = ValidateAuth(this.context, auth, true);
            this.context.UpdateListItem(person.Id, 0, item.UniqueId, item.Name);

            return data;
        }

        public DeleteListItemReturnData DeleteListItem(ClientAuthenticationData auth, Guid listItemUniqueId)
        {
            this.WaitForServerCallSleepTime();

            DeleteListItemReturnData data = new DeleteListItemReturnData();

            Person person = ValidateAuth(this.context, auth, true);
            this.context.DeleteListItem(person.Id, listItemUniqueId);

            return data;
        }

        public DateTime GetLastChangeTime(ClientAuthenticationData auth)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Auth

        public Person GetPerson(string liveUserId, string name)
        {
            if (string.IsNullOrEmpty(liveUserId))
            {
                throw new ArgumentNullException("liveUserId");
            }

            Person person = this.context.Persons.FirstOrDefault(p => p.LiveUserId == liveUserId);

            if (person == null)
            {
                person = new Person { Name = name, LiveUserId = liveUserId };
                person.UniqueId = Guid.NewGuid();
                this.context.Persons.InsertOnSubmit(person);
                this.context.SubmitChanges();
            }

            return person;
        }

        public void UpdatePerson(ClientAuthenticationData auth, Person person)
        {
            if (auth == null || auth.PersonUniqueId == Guid.Empty || auth.DeviceUniqueId == Guid.Empty)
            {
                throw new ArgumentException("auth");
            }
            if (string.IsNullOrEmpty(person.LiveUserId) || person.Id <= 0)
            {
                throw new ArgumentException("person");
            }

            try
            {
                Person dbPerson = ValidateAuth(this.context, auth, true);

                if (dbPerson.UniqueId != person.UniqueId)
                {
                    throw new Exception("You cannot update a different person's information.");
                }

                if (dbPerson == null)
                {
                    throw new ArgumentException("Person not found or invalid ID.");
                }

                dbPerson.Name = person.Name;
                this.context.SubmitChanges();
            }
            finally
            {
                this.context.Dispose();
            }
        }

        public PersonDevice AddDevice(ClientAuthenticationData auth, string deviceName)
        {
            if (auth == null || auth.PersonUniqueId == Guid.Empty)
            {
                throw new ArgumentException("auth");
            }
            if (string.IsNullOrEmpty(deviceName))
            {
                throw new ArgumentException("deviceName");
            }

            PersonDevice device = null;
            Person person = ValidateAuth(this.context, auth, false);

            device = new PersonDevice { Name = deviceName, LastConnectTime = DateTime.UtcNow };
            device.PersonId = person.Id;
            device.UniqueId = Guid.NewGuid();
            this.context.PersonDevices.InsertOnSubmit(device);
            this.context.SubmitChanges();

            return device;
        }

        public void RemoveDevice(ClientAuthenticationData auth, Guid deviceId)
        {
            if (auth == null || auth.PersonUniqueId == Guid.Empty || auth.DeviceUniqueId == Guid.Empty)
            {
                throw new ArgumentException("auth");
            }
            if (deviceId == Guid.Empty)
            {
                throw new ArgumentException("deviceId");
            }

            Person person = ValidateAuth(this.context, auth, true);

            PersonDevice device = this.context.PersonDevices.FirstOrDefault(d => d.UniqueId == deviceId && d.PersonId == person.Id);
            if (deviceId != null)
            {
                this.context.PersonDevices.DeleteOnSubmit(device);
                this.context.SubmitChanges();
            }
        }
        
        internal static Person ValidateAuth(SLSDataDataContext context, ClientAuthenticationData auth, bool validateDevice)
        {
            Person person = null;

            if (validateDevice)
            {
                person = context.Persons.FirstOrDefault(p => p.UniqueId == auth.PersonUniqueId
                    && p.PersonDevices.Any(d => d.UniqueId == auth.DeviceUniqueId));
            }
            else
            {
                person = context.Persons.FirstOrDefault(p => p.UniqueId == auth.PersonUniqueId);
            }

            if (person == null)
            {
                throw new Exception("Invalid auth");
            }

            return person;
        }

        #endregion
    }
}

