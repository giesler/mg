using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Configuration;
using System.Text;
using System.Threading;

namespace msn2.net.BarMonkey.RelayController
{
    public class ProXRRelayPulseController : ProXRRelayController
    {
        protected override void SendBatchGroup(List<BatchItem> batch)
        {
            int maxSeconds = 0;
            int index = 0;
            int sendMilliseconds = 100;
            
            
            foreach (BatchItem item in batch)
            {
                if (item.Seconds > maxSeconds)
                {
                    maxSeconds = item.Seconds;
                }

                int selectedTimerIndex = 90 + index;

                this.SendCommandAndFlush(50, selectedTimerIndex, 0, 0, item.Seconds, item.RelayNumber);

                index++;
            }

//            this.SendCommandAndFlush(50, 131, lsb, usb);

            Thread.Sleep(1000 * maxSeconds + 500);
        }
    }
}
