using System;
using giesler.org.lists.ListData;

namespace giesler.org.lists
{
    public class ListItemEx: ListItem
    {
        public Guid ListUniqueId { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
