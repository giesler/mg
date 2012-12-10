using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public partial class Relay
    {
        public override string ToString()
        {
            if (this.Ingredients.Count > 0)
            {
                return this.Id.ToString() + " (" + this.Ingredients[0].Name + ")";
            }
            else
            {
                return this.Id.ToString();
            }
        }
    }
}
