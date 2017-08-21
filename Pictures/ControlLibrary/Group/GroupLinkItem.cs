using System;
using System.Collections.Generic;
using System.Text;
using msn2.net.Pictures;

namespace msn2.net.Pictures.Controls
{
    public class GroupLinkItem: BaseLinkItem
    {
        private PersonGroupInfo group;

        public GroupLinkItem(PersonGroupInfo group)
        {
            this.group = group;
            this.Text = group.Name;
        }

        public PersonGroupInfo Group
        {
            get
            {
                return this.group;
            }
        }
    }
}
