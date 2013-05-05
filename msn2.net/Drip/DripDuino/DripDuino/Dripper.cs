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
            DefaultDuration = new TimeSpan(0, 15, 0); // 15 minutes
        }

        public static void Run()
        {
            InputPort switchPort = new InputPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled);
            OutputPort ledPort = new OutputPort(Pins.ONBOARD_LED, false);
            OutputPort externalLedPort = new OutputPort(Pins.GPIO_PIN_D7, false);
            OutputPort switchPort1 = new OutputPort(Pins.GPIO_PIN_D9, true);
            OutputPort switchPort2 = new OutputPort(Pins.GPIO_PIN_D10, true);

            bool wasClosed = false;

            while (true)
            {
                bool isClosed = switchPort.Read();

                // If on and time to turn off
                if (IsOn && OffTime < DateTime.Now)
                {
                    ledPort.Write(false);
                    externalLedPort.Write(false);
                    switchPort1.Write(true);
                    switchPort2.Write(true);
                    IsOn = false;
                    OnTime = DateTime.MinValue;
                }
                // If switch is pressed but wasn't on previous loop
                else if (isClosed && !wasClosed)
                {
                    ledPort.Write(true);
                    externalLedPort.Write(true);
                    switchPort1.Write(false);
                    switchPort2.Write(false);
                    OffTime = DateTime.Now.Add(Dripper.DefaultDuration);
                    OnTime = DateTime.Now;
                    IsOn = true;
                }
                else if (IsOn && isClosed && OnTime.AddSeconds(1) < DateTime.Now)
                {
                    ledPort.Write(false);
                    externalLedPort.Write(false);
                    switchPort1.Write(true);
                    switchPort2.Write(true);
                    OffTime = DateTime.Now;
                }

                wasClosed = isClosed;

                Thread.Sleep(100);
            }
        }
    }
}
