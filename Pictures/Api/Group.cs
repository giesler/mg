using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.Pictures
{
    public partial class Group
    {
        public override string ToString()
        {
            return this.GroupName;
        }

        public int Id
        {
            get
            {
                return this.GroupID;
            }
        }
    }
}
