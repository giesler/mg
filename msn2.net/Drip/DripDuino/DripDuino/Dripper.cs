using System;
using Microsoft.SPOT;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace DripDuino
{
    class Dripper
    {
        public static bool IsOn { get; private set; }
        public static TimeSpan DefaultDuration { get; private set; }
        public static DateTime OffTime { get; private set; }
        public static DateTime OnTime { get; private set; }

        static Dripper()
        {
            DefaultDuration = new TimeSpan(0, 1, 0); // 1 minute
        }

        public static void Run()
        {
            InputPort switchPort = new InputPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled);
            OutputPort ledPort = new OutputPort(Pins.ONBOARD_LED, false);
            bool wasClosed = false;

            while (true)
            {
                bool isClosed = switchPort.Read();

                // If on and time to turn off
                if (IsOn && OffTime < DateTime.Now)
                {
                    ledPort.Write(false);
                    IsOn = false;
                    OnTime = DateTime.MinValue;
                }
                // If switch is pressed but wasn't on previous loop
                else if (isClosed && !wasClosed)
                {
                    ledPort.Write(true);
                    OffTime = DateTime.Now.Add(Dripper.DefaultDuration);
                    OnTime = DateTime.Now;
                    IsOn = true;
                }
                // If switch was pressed on last loop but is not pressed now
                else if (wasClosed && !isClosed)
                {
                    ledPort.Write(false);
                    OffTime = DateTime.Now;
                }

                wasClosed = isClosed;

                Thread.Sleep(100);
            }
        }
    }
}
