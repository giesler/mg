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

        public List<Drink> GetDrinks(string searchText)
        {
            var q = from d in context.Data.Drinks
                    where d.Name.Contains(searchText) || d.Description.Contains(searchText)
                    select d;
            return q.ToList<Drink>();                    
        }

        public Drink GetDrink(int id)
        {
            var q = from d in context.Data.Drinks
                    where d.Id == id
                    select d;
            List<Drink> drinks = q.ToList<Drink>();
            Drink drink = drinks.Count > 0 ? drinks[0] : null;
            return drink;
        }
    }
}
