using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Data.Linq;
using System.Linq;

namespace msn2.net.BarMonkey.RelayController
{
    public class ProXRRelayPulseController : ProXRRelayController
    {
        int onCommandOffset = 108;
        int offCommandOffset = 100;

        protected override void ConfigureSerialPort()
        {
            this.SendCommandAndFlush("manual refresh", 26);

            this.SendCommandAndFlush("disable reporting", 28);
        }

        protected override void SendBatchGroup(List<BatchItem> batch)
        {
            double maxSeconds = 0.0;

            var sortedList = from l in batch
                             orderby l.Seconds ascending
                             select l;
        
            foreach (BatchItem item in sortedList)
            {
                if (item.Seconds > maxSeconds)
                {
                    maxSeconds = item.Seconds;
                }

                int bank = GetBank(item.RelayNumber);
                int bankRelayIndex = GetBankRelayIndex(item.RelayNumber);
                
                base.SendCommandAndFlush("turn on", bankRelayIndex + onCommandOffset, bank);
            }

            List<BatchItem> activeRelays = sortedList.ToList<BatchItem>();

            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now.AddSeconds(maxSeconds);

            try
            {
                WriteElapsedTime(startTime);
                base.SendCommandAndFlush("flush", 37);

                while (DateTime.Now < endTime)
                {
                    ProcessActiveList(activeRelays, startTime);
                }

                int loopCount = 0;
                while (activeRelays.Count > 0 && loopCount < 20)
                {
                    ProcessActiveList(activeRelays, startTime);

                    if (activeRelays.Count > 0)
                    {
                        Thread.Sleep(100);
                    }
                }

                if (activeRelays.Count > 0)
                {
                    Console.WriteLine("WARNING: {0} relays remained active.", activeRelays.Count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            finally
            {
                // Turn off all relays
                WriteElapsedTime(startTime);
                base.SendCommandAndFlush("all off", 29);
            }

        }

        private void ProcessActiveList(List<BatchItem> activeRelays, DateTime startTime)
        {
            List<BatchItem> offList = new List<BatchItem>();

            foreach (BatchItem item in activeRelays)
            {
                DateTime itemEndTime = startTime.AddSeconds(item.Seconds);
                if (itemEndTime < DateTime.Now.AddMilliseconds(100))
                {
                    offList.Add(item);
                }
            }

            if (offList.Count > 0)
            {
                foreach (BatchItem item in offList)
                {
                    int bank = GetBank(item.RelayNumber);
                    int bankRelayIndex = GetBankRelayIndex(item.RelayNumber);

                    WriteElapsedTime(startTime);
                    base.SendCommandAndFlush("turn off", offCommandOffset + bankRelayIndex, bank);

                    activeRelays.Remove(item);
                }

                WriteElapsedTime(startTime);
                base.SendCommandAndFlush("refresh", 37);
            }
            else
            {
                Thread.Sleep(100);
            }
        }

        private static void WriteElapsedTime(DateTime startTime)
        {
            TimeSpan currentDuration = DateTime.Now - startTime;
            Console.Write("{0:00}:{1:000}:", currentDuration.Seconds, (1000 * currentDuration.Milliseconds));
        }

        private int GetBank(int relay)
        {
            int bank = relay / 8 + 1;
            return bank;
        }

        private int GetBankRelayIndex(int relay)
        {
            return relay % 8;
        }
    }
}
