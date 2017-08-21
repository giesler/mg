using System;
using System.Collections.Generic;
using System.Text;

namespace msn2.net.Pictures.Controls
{
    public class GroupLinkList: BaseLinkList
    {
        public GroupLinkList()
        {
        }

        public GroupLinkItem Add(PersonGroupInfo group)
        {
            GroupLinkItem item = new GroupLinkItem(group);
            this.Controls.Add(item);
            return item;
        }

        public bool Contains(PersonGroupInfo group)
        {
            GroupLinkItem item = this.Find(group);
            return (item != null);
        }

        public GroupLinkItem Find(PersonGroupInfo group)
        {
            foreach (GroupLinkItem item in this.Controls)
            {
                if (item.Group.Id == group.Id)
                {
                    return item;
                }
            }

            return null;
        }

        public void Remove(PersonGroupInfo group)
        {
            foreach (GroupLinkItem item in this.Controls)
            {
                if (item.Group.Id == group.Id)
                {
                    this.Controls.Remove(item);
                    break;
                }
            }
        }
    }
}
