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

        public Relay GetRelay(int id)
        {
            return base.Context.Data.Relays.FirstOrDefault(i => i.Id == id);
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
            if (ingredient.Relay != null)
            {
                Ingredient current = this.Context.Ingredients.GetIngredientOnRelay(relay);
                if (current != null)
                {
                    current.Relay = null;
                }
            }

            ingredient = this.Context.Ingredients.GetIngredient(ingredient.Id);
            Relay newRelay = this.GetRelay(relay.Id);
            ingredient.Relay = newRelay;
            base.Context.Data.SubmitChanges();
        }
    }
}