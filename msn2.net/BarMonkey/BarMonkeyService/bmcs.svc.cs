using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using msn2.net.BarMonkey;

namespace BarMonkeyService
{
    public class BarMonkeyClientService : IBarMonkeyClientService
    {
        public BarMonkeyClientService()
        {
        }

        public List<Drink> GetTopDrinks(int count)
        {
            BarMonkeyContext bmc = new BarMonkeyContext();
            List<Drink> drinks = bmc.Drinks.GetTopDrinks(count);
            return drinks;
        }

        public List<Drink> GetAllDrinks()
        {
            BarMonkeyContext bmc = new BarMonkeyContext();
            List<Drink> drinks = bmc.Drinks.GetDrinks(string.Empty).OrderBy(i => i.Name).ToList();
            return drinks;
        }

        public void ConnectTest()
        {
            Dispenser disp = new Dispenser();
            disp.ConnectTest();
        }

        public void PourDrink(int drinkId, int containerId)
        {
            BarMonkeyContext bmc = new BarMonkeyContext();
            Drink drink = bmc.Drinks.GetDrink(drinkId);
            Container container = bmc.Containers.GetContainer(containerId);

            Dispenser disp = new Dispenser();
            disp.PourDrink(bmc, drink, container);
        }

        public void PourIngredient(int relayId, int seconds)
        {
            Dispenser disp = new Dispenser();
            disp.PourIngredient(relayId, seconds);
        }

        public void TurnOffAllRelays()
        {
            Dispenser disp = new Dispenser();
            disp.TurnOffAllRelays();
        }
    }
}
