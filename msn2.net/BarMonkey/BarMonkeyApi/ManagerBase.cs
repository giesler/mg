using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public abstract class ManagerBase
    {
        private BarMonkeyContext context;

        protected BarMonkeyContext Context
        {
            get { return context; }
        }
	        
        private Type itemType;

        public Type ItemType
        {
            get { return itemType; }
        }

        internal ManagerBase(BarMonkeyContext context, Type itemType)
        {
            this.context = context;
            this.itemType = itemType;
        }

    }
}
