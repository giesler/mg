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
    [ServiceContract]
    public interface IBarMonkeyClientService
    {
        [OperationContract]
        List<Drink> GetTopDrinks(int count);
        
        [OperationContract]
        List<Drink> GetAllDrinks();

        [OperationContract]
        void PourDrink(int drinkId, int containerId);

        [OperationContract]
        void PourIngredient(int relayId, int seconds);

        [OperationContract]
        void TurnOffAllRelays();

        [OperationContract]
        void ConnectTest();
    }
}
