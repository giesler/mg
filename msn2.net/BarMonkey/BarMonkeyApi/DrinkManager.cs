using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class DrinkManager: ManagerBase
    {
        internal DrinkManager(BarMonkeyContext context): base(context, typeof(Drink))
        {
        }

        public List<Drink> GetDrinks(string searchText)
        {
            var q = from d in base.Context.Data.Drinks
                    where d.Name.Contains(searchText) || d.Description.Contains(searchText)
                    select d;
            return q.ToList<Drink>();                    
        }

        public Drink GetDrink(int id)
        {
            var q = from d in base.Context.Data.Drinks
                    where d.Id == id
                    select d;
            List<Drink> list = q.ToList<Drink>();
            Drink drink = list.Count > 0 ? list[0] : null;
            return drink;
        }
    }
}
