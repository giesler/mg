using System;
using Microsoft.SPOT;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.IO;

namespace DripDuino
{
    class Dripper
    {
        public static bool IsOn { get; private set; }
        public static TimeSpan DefaultDuration { get; private set; }
        public static DateTime OffTime { get; private set; }
        public static DateTime OnTime { get; private set; }

        private static InputPort switchPort = new InputPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled);
        private static OutputPort internalLedPort = new OutputPort(Pins.ONBOARD_LED, false);
        private static OutputPort externalLedPort = new OutputPort(Pins.GPIO_PIN_D7, false);
        private static OutputPort outPort1 = new OutputPort(Pins.GPIO_PIN_D9, true);
        private static OutputPort outPort2 = new OutputPort(Pins.GPIO_PIN_D10, true);
        
        static Dripper()
        {
            DefaultDuration = new TimeSpan(0, 15, 0); // 15 minutes
        }

        public static void Run()
        {
            bool wasClosed = false;

            while (true)
            {
                bool isClosed = switchPort.Read();

                // If on and time to turn off or button is pressed
                if (IsOn && (OffTime < DateTime.Now || (isClosed && OnTime.AddSeconds(1) < DateTime.Now)))
                {
                    Toggle(false);
                }
                // If switch is pressed but wasn't on previous loop
                else if (isClosed && !wasClosed)
                {
                    Toggle(true);
                }

                wasClosed = isClosed;

                Thread.Sleep(100);
            }
        }

        public static void Toggle(bool turnOn)
        {
            internalLedPort.Write(turnOn);
            externalLedPort.Write(turnOn);
            outPort1.Write(!turnOn);
            outPort2.Write(!turnOn);

            IsOn = turnOn;
            OnTime = turnOn ? DateTime.Now : DateTime.MinValue;
            OffTime = turnOn ? DateTime.Now.Add(Dripper.DefaultDuration) : OffTime;

            if (IsOn)
            {
                Log.AddEntry("Turned on for " + Dripper.DefaultDuration.Minutes + " minutes");
            }
            else
            {
                Log.AddEntry("Turned off");
            }
        }
    }
}
