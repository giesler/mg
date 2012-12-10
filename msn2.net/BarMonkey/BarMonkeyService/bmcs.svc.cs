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
    }
}
