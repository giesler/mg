using System;
using System.Collections.Generic;
using System.Text;
using msn2.net.Pictures;

namespace msn2.net.Pictures.Controls
{
    public class GroupLinkItem: BaseLinkItem
    {
        private PersonGroup group;

        public GroupLinkItem(PersonGroup group)
        {
            this.group = group;
            this.Text = group.Name;
        }

        public PersonGroup Group
        {
            get
            {
                return this.group;
            }
        }
    }
}
