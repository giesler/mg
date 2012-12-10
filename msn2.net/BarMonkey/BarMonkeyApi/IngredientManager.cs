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

        public List<Ingredient> GetIngredients()
        {
            var q = from i in base.Context.Data.Ingredients
                    orderby i.Name
                    select i;

            return q.ToList<Ingredient>();
        }

        public Ingredient GetIngredientOnRelay(Relay relay)
        {
            var q = from i in base.Context.Data.Ingredients
                    where i.Relay == relay
                    select i;

            if (q.Count<Ingredient>() > 0)
            {
                return q.First<Ingredient>();
            }
            else
            {
                return null;
            }
        }

        public List<Ingredient> GetAvailableIngredients()
        {
            var q = from i in base.Context.Data.Ingredients
                    orderby i.Name
                    select i;

            return q.ToList<Ingredient>();
        }

        public List<Ingredient> GetByName()
        {
            var q = from i in BarMonkeyContext.Current.Data.Ingredients
                    orderby i.Name ascending
                    select i;

            return q.ToList<Ingredient>();
        }

        public List<Ingredient> GetByAmount()
        {
            var q = from i in BarMonkeyContext.Current.Data.Ingredients
                    orderby i.RemainingOunces ascending
                    select i;

            return q.ToList<Ingredient>();
        }
    }
}
