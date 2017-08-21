using System;
using System.Collections.Generic;
using System.Text;
using msn2.net.Pictures;

namespace msn2.net.Pictures.Controls
{
    public class PersonLinkItem: BaseLinkItem
    {
        private PersonInfo person;

        public PersonLinkItem(PersonInfo person)
        {
            this.person = person;
            this.Text = person.Name;
        }

        public PersonInfo Person
        {
            get
            {
                return this.person;
            }
        }
    }
}
