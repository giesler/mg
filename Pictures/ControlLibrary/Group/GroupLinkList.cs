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

        public GroupLinkItem Add(PersonGroup group)
        {
            GroupLinkItem item = new GroupLinkItem(group);
            this.Controls.Add(item);
            return item;
        }

        public bool Contains(PersonGroup group)
        {
            GroupLinkItem item = this.Find(group);
            return (item != null);
        }

        public GroupLinkItem Find(PersonGroup group)
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

        public void Remove(PersonGroup group)
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
