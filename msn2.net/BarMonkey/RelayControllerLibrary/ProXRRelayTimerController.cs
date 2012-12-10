using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Configuration;
using System.Text;
using System.Threading;

namespace msn2.net.BarMonkey.RelayController
{
    public class ProXRRelayTimerController : ProXRRelayController
    {
        protected override void SendBatchGroup(List<BatchItem> batch)
        {
            double maxSeconds = 0.0;
            int index = 0;
            int lsb = 0;
            int usb = 0;

            foreach (BatchItem item in batch)
            {
                if (item.Seconds > maxSeconds)
                {
                    maxSeconds = item.Seconds;
                }

                int selectedTimerIndex = 90 + index;

                this.SendCommandAndFlush("schedule", 50, selectedTimerIndex, 0, 0, (int)item.Seconds, item.RelayNumber);

                if (index < 8)
                {
                    lsb = lsb + (int)Math.Pow(2, index);
                }
                else
                {
                    usb = usb + (int)Math.Pow(2, index);
                }

                index++;
            }

            this.SendCommandAndFlush("flush", 50, 131, lsb, usb);

            Thread.Sleep(1000 * (int)maxSeconds + 500);
        }

    }
}
