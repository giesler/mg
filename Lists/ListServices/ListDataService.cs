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
        public ListDataService()
        {
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

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);

                Guid listUniqueId = Guid.NewGuid();
                bool? isDupe = false;
                int? listId = 0;
                context.AddList(person.Id, name.Trim(), listUniqueId, ref isDupe, ref listId);

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
        }

        public UpdateListReturnValue UpdateList(ClientAuthenticationData auth, List list)
        {
            this.WaitForServerCallSleepTime();

            UpdateListReturnValue returnVal = new UpdateListReturnValue();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);

                bool? isDuplicate = false;
                bool? isInvalidId = false;
                context.UpdateList(person.Id, list.Name, list.UniqueId, ref isDuplicate, ref isInvalidId);

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
        }

        public void DeleteList(ClientAuthenticationData auth, Guid uniqueId)
        {
            this.WaitForServerCallSleepTime();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                context.DeleteList(person.Id, uniqueId);
            }
        }

        public List<GetListsResult> GetLists(ClientAuthenticationData auth)
        {
            this.WaitForServerCallSleepTime();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                var q = context.GetLists(person.Id);
                return q.ToList();
            }
        }

        public List<GetAllListItemsResult> GetListItems(ClientAuthenticationData auth, Guid listUniqueId)
        {
            this.WaitForServerCallSleepTime();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                return context.GetAllListItems(person.Id).Where(l => l.ListUniqueId == listUniqueId).ToList();
            }
        }

        public List<GetAllListItemsResult> GetAllListItems(ClientAuthenticationData auth)
        {
            this.WaitForServerCallSleepTime();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                return context.GetAllListItems(person.Id).ToList();
            }
        }

        public List<GetAllResult> GetAll(ClientAuthenticationData auth)
        {
            this.WaitForServerCallSleepTime();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                return context.GetAll(person.Id).ToList();
            }
        }

        public AddListItemReturnValue AddListItem(ClientAuthenticationData auth, Guid listUniqueId, string name)
        {
            this.WaitForServerCallSleepTime();

            AddListItemReturnValue returnVal = new AddListItemReturnValue();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                bool? isDuplicate = false;
                Guid itemUniqueId = Guid.NewGuid();
                int? listItemId = 0;

                context.AddListItem(person.Id, auth.DeviceUniqueId, name, listUniqueId, itemUniqueId, ref isDuplicate, ref listItemId);

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
        }

        public UpdateListItemReturnData UpdateListItem(ClientAuthenticationData auth, ListItem item)
        {
            this.WaitForServerCallSleepTime();

            UpdateListItemReturnData data = new UpdateListItemReturnData();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                context.UpdateListItem(person.Id, 0, item.UniqueId, item.Name);
                return data;
            }
        }

        public DeleteListItemReturnData DeleteListItem(ClientAuthenticationData auth, Guid listItemUniqueId)
        {
            this.WaitForServerCallSleepTime();

            DeleteListItemReturnData data = new DeleteListItemReturnData();

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);
                context.DeleteListItem(person.Id, listItemUniqueId);
                return data;
            }
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

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = context.Persons.FirstOrDefault(p => p.LiveUserId == liveUserId);

                if (person == null)
                {
                    person = new Person { Name = name, LiveUserId = liveUserId };
                    person.UniqueId = Guid.NewGuid();
                    context.Persons.InsertOnSubmit(person);
                    context.SubmitChanges();
                }

                return person;
            }
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

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person dbPerson = ValidateAuth(context, auth, true);

                if (dbPerson.UniqueId != person.UniqueId)
                {
                    throw new Exception("You cannot update a different person's information.");
                }

                if (dbPerson == null)
                {
                    throw new ArgumentException("Person not found or invalid ID.");
                }

                dbPerson.Name = person.Name;
                context.SubmitChanges();
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

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, false);

                PersonDevice device = new PersonDevice { Name = deviceName, LastConnectTime = DateTime.UtcNow };
                device.PersonId = person.Id;
                device.UniqueId = Guid.NewGuid();
                context.PersonDevices.InsertOnSubmit(device);
                context.SubmitChanges();
                return device;
            }
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

            using (SLSDataDataContext context = new SLSDataDataContext())
            {
                Person person = ValidateAuth(context, auth, true);

                PersonDevice device = context.PersonDevices.FirstOrDefault(d => d.UniqueId == deviceId && d.PersonId == person.Id);
                if (deviceId != null)
                {
                    context.PersonDevices.DeleteOnSubmit(device);
                    context.SubmitChanges();
                }
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

