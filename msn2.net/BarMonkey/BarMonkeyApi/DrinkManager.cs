﻿using System;
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
                    where (d.Name.Contains(searchText) || d.Description.Contains(searchText))
                        && d.DrinkIngredients.Count > 0                        
                    orderby d.Name
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

        public void LogDrink(Drink drink, decimal offset)
        {
            UserDrinkHistory udh = new UserDrinkHistory();
            udh.DrinkId = drink.Id;
            udh.UserId = base.Context.CurrentUser.Id;
            udh.Timestamp = DateTime.Now;
            base.Context.Data.UserDrinkHistories.Add(udh);

            //foreach (DrinkIngredient ingredient in drink.DrinkIngredients)
            //{
            //    UserDrinkIngredientHistory udih = new UserDrinkIngredientHistory();
            //    udih.UserDrinkHistory = udh;
            //    udih.IngredientId = ingredient.Id;
            //    udih.AmountOunces = ingredient.AmountOunces * offset;
            //    udih.Sequence = sequence;
            //    base.Context.Data.UserDrinkIngredientHistories.Add(udih);
            //}

            base.Context.Data.SubmitChanges();
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
                    base.Context.Data.SubmitChanges();
                }
            }
            else
            {
                if (count > 0)
                {
                    foreach (UserFavorite fav in q)
                    {
                        base.Context.Data.UserFavorites.Remove(fav);
                        base.Context.Data.SubmitChanges();
                    }
                }
            }
        }

        public List<Drink> GetFavorites()
        {
            int userId = base.Context.CurrentUser.Id;

            var q = from f in base.Context.Data.UserFavorites
                    where f.UserId == userId
                    orderby f.Drink.Name
                    select f.Drink;

            return q.ToList<Drink>();
        }

        public List<Drink> GetLatest(int count)
        {
            int userId = base.Context.CurrentUser.Id;

            var q = (from d in base.Context.Data.UserDrinkHistories
                     where d.UserId == userId
                     orderby d.Timestamp descending
                     select d.Drink).Take<Drink>(count);

            return q.ToList<Drink>();
        }
    }
}