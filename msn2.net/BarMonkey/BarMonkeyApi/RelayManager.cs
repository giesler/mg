using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class RelayManager : ManagerBase
    {
        internal RelayManager(BarMonkeyContext context)
            : base(context, typeof(Relay))
        {
        }

        public List<Relay> GetRelays()
        {
            var q = from r in base.Context.Data.Relays
                    orderby r.Id
                    select r;

            return q.ToList<Relay>();
        }

        public void SetIngredient(Relay relay, Ingredient ingredient)
        {
            Ingredient current = BarMonkeyContext.Current.Ingredients.GetIngredientOnRelay(ingredient.Relay);
            if (current != null)
            {
                current.Relay = null;
            }

            ingredient.Relay = relay;
            base.Context.Data.SubmitChanges();
        }
    }
}