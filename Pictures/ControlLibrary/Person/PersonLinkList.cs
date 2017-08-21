using System;
using System.Collections.Generic;
using System.Text;

namespace msn2.net.Pictures.Controls
{
    public class PersonLinkList: BaseLinkList
    {
        public PersonLinkList()
        {
        }

        public PersonLinkItem Add(PersonInfo person)
        {
            PersonLinkItem item = new PersonLinkItem(person);
            this.Controls.Add(item);
            return item;
        }

        public bool Contains(PersonInfo person)
        {
            PersonLinkItem item = this.Find(person);
            return (item != null);
        }

        public PersonLinkItem Find(PersonInfo person)
        {
            foreach (PersonLinkItem item in this.Controls)
            {
                if (item.Person.Id == person.Id)
                {
                    return item;
                }
            }

            return null;
        }

        public void Remove(PersonInfo person)
        {
            foreach (PersonLinkItem item in this.Controls)
            {
                if (item.Person.Id == person.Id)
                {
                    this.Controls.Remove(item);
                    break;
                }
            }
        }
    }
}
