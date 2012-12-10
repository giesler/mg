using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace msn2.net.BarMonkey.RelayController
{
    public class ConsoleRelayController: IRelayController
    {
        #region IRelayController Members

        public void ConnectTest()
        {
            Console.WriteLine("ConnectTest called.");
        }

        public void SendBatch(List<BatchItem> batch)
        {
            Console.WriteLine("SendBatch - {0} items", batch.Count);
            foreach (BatchItem item in batch)
            {
                Console.WriteLine(" - Group {0}, Relay {1} for {2} seconds", item.Group, item.RelayNumber, item.Seconds);
            }

            Thread.Sleep(3500);
        }

        public void TurnAllOff()
        {
            Console.WriteLine("Turn all off");
        }

        #endregion
    }
}
