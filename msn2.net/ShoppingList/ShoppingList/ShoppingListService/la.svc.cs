using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace msn2.net.ShoppingList
{
    public class ListAuthentication : IListAuthentication
    {
        SLSDataDataContext context = null;

        public ListAuthentication()
        {
            this.context = new SLSDataDataContext();
        }

        #region IListAuthentication Members

        public Person GetPerson(string liveUserId, string name)
        {
            if (string.IsNullOrEmpty(liveUserId))
            {
                throw new ArgumentNullException("liveUserId");
            }

            Person person = null;

            try
            {
                person = this.context.Persons.FirstOrDefault(p => p.LiveUserId == liveUserId);

                if (person == null)
                {
                    person = new Person { Name = name, LiveUserId = liveUserId };
                    person.UniqueId = Guid.NewGuid();
                    this.context.Persons.InsertOnSubmit(person);
                    this.context.SubmitChanges();
                }
            }
            finally
            {
                this.context.Dispose();
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

            try
            {
                Person person = ValidateAuth(this.context, auth, false);

                device = new PersonDevice { Name = deviceName, LastConnectTime = DateTime.UtcNow };
                device.PersonId = person.Id;
                device.UniqueId = Guid.NewGuid();
                this.context.PersonDevices.InsertOnSubmit(device);
                this.context.SubmitChanges();
            }
            finally
            {
                this.context.Dispose();
            }

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

            try
            {
                Person person = ValidateAuth(this.context, auth, true);

                PersonDevice device = this.context.PersonDevices.FirstOrDefault(d => d.UniqueId == deviceId && d.PersonId == person.Id);
                if (deviceId != null)
                {
                    this.context.PersonDevices.DeleteOnSubmit(device);
                    this.context.SubmitChanges();
                }
            }
            finally
            {
                this.context.Dispose();
            }
        }

        #endregion

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
    }
}
