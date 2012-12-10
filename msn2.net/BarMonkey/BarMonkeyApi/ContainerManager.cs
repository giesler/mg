using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class ContainerManager: ManagerBase
    {
        internal ContainerManager(BarMonkeyContext context)
            : base(context, typeof(Container))
        {
        }

        public List<Container> GetContainers()
        {
            var q = from c in base.Context.Data.Containers
                    orderby c.Size
                    select c;
            return q.ToList<Container>();                    
        }

        public Container GetContainer(int id)
        {
            var q = from d in base.Context.Data.Containers
                    where d.Id == id
                    select d;
            List<Container> list = q.ToList<Container>();
            Container c = list.Count > 0 ? list[0] : null;
            return c;
        }
    }
}
