using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public partial class Ingredient
    {
        public override string ToString()
        {
            if (this.RelayId.HasValue)
            {
                return this.Name + " (relay " + this.RelayId.Value.ToString() + ")";
            }
            else
            {
                return this.Name;
            }
        }
    }
}
