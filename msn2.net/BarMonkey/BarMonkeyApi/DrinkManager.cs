using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace msn2.net.BarMonkey
{
    public class DrinkManager : ManagerBase
    {
        internal DrinkManager(BarMonkeyContext context)
            : base(context, typeof(Drink))
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

        public bool IsFavorite(int drinkId)
        {
            int userId = base.Context.CurrentUser.Id;

            var q = from f in base.Context.Data.UserFavorites
                    where f.DrinkId == drinkId && f.UserId == userId
                    select f;
            int count = q.Count<UserFavorite>();

            return count > 0;
        }

        public void SetFavorite(int drinkId, bool? isFavorite)
        {
            int userId = base.Context.CurrentUser.Id;

            var q = from f in base.Context.Data.UserFavorites
                    where f.DrinkId == drinkId && f.UserId == userId
                    select f;
            int count = q.Count<UserFavorite>();

            if (isFavorite == true)
            {
                if (count == 0)
                {
                    UserFavorite favorite = new UserFavorite { DrinkId = drinkId, UserId = userId };
                    base.Context.Data.UserFavorites.Add(favorite);
                }
            }
            else
            {
                if (count > 0)
                {
                    foreach (UserFavorite fav in q)
                    {
                        base.Context.Data.UserFavorites.Remove(fav);
                    }
                }
            }
        }

        public List<UserFavorite> GetFavorites()
        {
            int userId = base.Context.CurrentUser.Id;

            var q = from f in base.Context.Data.UserFavorites
                    where f.UserId == userId
                    orderby f.Drink.Name
                    select f;
            
            return q.ToList<UserFavorite>();
        }
    }
}