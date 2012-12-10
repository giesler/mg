using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class IngredientManager: ManagerBase
    {
        internal IngredientManager(BarMonkeyContext context)
            : base(context, typeof(Ingredient))
        {
        }

        public List<Ingredient> GetAvailableIngredients()
        {
            var q = from i in base.Context.Data.Ingredients
                    orderby i.Name
                    select i;

            return q.ToList<Ingredient>();
        }
    }
}
