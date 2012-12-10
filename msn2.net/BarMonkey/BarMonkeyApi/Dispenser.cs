using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using msn2.net.BarMonkey.RelayControllerService;

namespace msn2.net.BarMonkey
{
    public class Dispenser
    {
        public Dispenser() {}

        public event EventHandler OnPourConnectCompleted;
        public event EventHandler OnPourStarted;
        public event EventHandler OnPourCompleted;

        public TimeSpan EstimatedDuration { get; private set; }

        public void PourDrink(BarMonkeyContext context, Drink drink, Container container)
        {
            RelayControllerClient relayClient = new RelayControllerClient();
            relayClient.ConnectTest();

            if (this.OnPourConnectCompleted != null)
            {
                this.OnPourConnectCompleted(this, EventArgs.Empty);
            }

            var q = from di in drink.DrinkActualIngredients
                    orderby di.Group, di.Sequence
                    select di;

            decimal totalOunces = (from di in drink.DrinkActualIngredients select di.AmountOunces).Sum();
            decimal offset = container.Size / totalOunces;

            List<BatchItem> items = new List<BatchItem>();

            decimal fullDuration = 0;
            decimal currentStageDuration = 0;
            int currentGroup = -1;
            foreach (DrinkActualIngredient di in q)
            {
                if (currentGroup != di.Group)
                {
                    fullDuration += currentStageDuration;
                    currentGroup = di.Group;
                    currentStageDuration = 0;
                }

                decimal duration = GetOutputDuration(offset, di);
                if (duration > currentStageDuration)
                {
                    currentStageDuration = duration;
                }
            }
            fullDuration += currentStageDuration + 3;  // 3 second offset

            this.EstimatedDuration = new TimeSpan(0, 0, (int)fullDuration);
            
            int lightRelayNumber = 36;
            items.Add(new BatchItem { Group = 0, RelayNumber = lightRelayNumber, Seconds = (double)fullDuration });

            List<Ingredient> actualIngredients = new List<Ingredient>();

            foreach (DrinkActualIngredient di in q)
            {
                decimal duration = GetOutputDuration(offset, di);

                int relayNumber = -1;
                var subs = context.Data.IngredientSubstitutes.Where(i => i.Ingredient.Id == di.IngredientId).OrderBy(i => i.Priority);
                if (subs.Count() > 0)
                {
                    int altId = subs.First().SubstitueIngredientId;
                    Ingredient sub = context.Ingredients.GetIngredient(altId);
                    relayNumber = sub.RelayId.Value;
                    actualIngredients.Add(sub);
                }
                else
                {
                    actualIngredients.Add(di.Ingredient);
                }

                if (relayNumber == -1)
                {
                    relayNumber = (int)di.Ingredient.RelayId;
                }

                items.Add(new BatchItem { Group = di.Group, RelayNumber = relayNumber, Seconds = (double)duration });
            }

            if (container.WaterFlushOunces > 0)
            {
                Ingredient waterIngredient = BarMonkeyContext.Current.Ingredients.GetIngredient("Water");
                decimal duration = container.WaterFlushOunces * waterIngredient.OuncesPerSecond;

                items.Add(new BatchItem { Group = 999, RelayNumber = (int)waterIngredient.RelayId, Seconds = (double)duration });
            }

            if (this.OnPourStarted != null)
            {
                this.OnPourStarted(this, EventArgs.Empty);
            }

            relayClient.SendBatch(items.ToArray<BatchItem>());

            if (BarMonkeyContext.Current.ImpersonateUser != null)
            {
                context.Drinks.LogDrink(drink, offset, BarMonkeyContext.Current.ImpersonateUser.Id);
            }
            else
            {
                context.Drinks.LogDrink(drink, offset, BarMonkeyContext.Current.CurrentUser.Id);
            }

            if (this.OnPourCompleted != null)
            {
                this.OnPourCompleted(this, EventArgs.Empty);
            }
        }

        private static decimal GetOutputDuration(decimal offset, DrinkActualIngredient di)
        {
            decimal outputAmount = di.AmountOunces * offset;
            decimal duration = outputAmount * di.Ingredient.OuncesPerSecond;
            return duration;
        }

        public void ConnectTest()
        {
            RelayControllerClient relay = new RelayControllerClient();
            relay.ConnectTest();
        }
    }
}
