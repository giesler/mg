using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class DrinkManager: ManagerBase
    {
        private BarMonkeyContext context = null;

        internal DrinkManager(BarMonkeyContext context): base(context, typeof(Drink))
        {
            this.context = context;
        }

    }
}
