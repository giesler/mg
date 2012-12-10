using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public partial class DrinkIngredient
    {
        public Ingredient GetSubstitue(BarMonkeyContext context)
        {
            Ingredient di = null;

            if (this.IngredientId > 0)
            {
                var q = context.Data.IngredientSubstitutes.Where(i => i.IngredientId == this.IngredientId).OrderBy(i => i.Priority);
                if (q.Count() > 0)
                {
                    int subId = q.First().SubstitueIngredientId;
                    di = context.Ingredients.GetIngredient(subId);
                }
            }

            return di;
        }
    }
}
