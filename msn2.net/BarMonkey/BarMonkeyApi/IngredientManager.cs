﻿using System;
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

        public Ingredient GetIngredient(int id)
        {
            var q = from i in base.Context.Data.Ingredients
                    where i.Id == id
                    select i;

            return q.First<Ingredient>();
        }

        public Ingredient GetIngredient(string name)
        {
            var q = from i in base.Context.Data.Ingredients
                    where i.Name == name
                    select i;

            return q.First<Ingredient>();
        }

        public Ingredient GetIngredientOnRelay(Relay relay)
        {
            var q = from i in base.Context.Data.Ingredients
                    where i.Relay.Id == relay.Id
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

        public void UpdateIngredient(Ingredient ingredient)
        {
            base.Context.Data.SubmitChanges();
        }
    }
}
