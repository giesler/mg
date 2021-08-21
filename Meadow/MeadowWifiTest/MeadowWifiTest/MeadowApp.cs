using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace MeadowWifiTest
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        public MeadowApp()
        {
            Initialize();
            CycleColors(1000);
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            Console.WriteLine("\n\ninit wifi");
            Device.InitWiFiAdapter().Wait();

            Console.WriteLine("sleep 30");
            Thread.Sleep(TimeSpan.FromSeconds(30));

            Console.WriteLine("reading wifi");
            try
            {
                var wifi = Device.WiFiAdapter;

                //                var nets = Device.WiFiAdapter.IpAddress;
                //              Console.WriteLine("Networks: " + nets.Count.ToString());

                Device.WiFiAdapter.SetAntenna(Meadow.Gateways.AntennaType.External);
                Console.WriteLine("Wifi antenna " + Device.WiFiAdapter.Antenna.ToString());
                byte[] m = Device.WiFiAdapter.MacAddress;
                Console.WriteLine($"MAC size {m.Length}");
                var ms = Encoding.UTF8.GetString(m); // BitConverter.ToString(m);
                Console.WriteLine($"MAC {ms}");

                Console.WriteLine("connecting");
                
                var result = Device.WiFiAdapter.Connect("MS2N", "ABunchOfWords");
                Console.WriteLine("Connection status - " + result.Result.ConnectionStatus.ToString());

                var wr = WebRequest.CreateHttp("http://hs2:8080/");
                var res = wr.GetResponse();

                Console.WriteLine($"Res: {res.ContentType} - len: {res.ContentLength}");

                if (result.Result.ConnectionStatus != Meadow.Gateway.WiFi.ConnectionStatus.Success)
                {
                    throw new Exception("Unable to connect - " + result.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: " + ex.ToString());
            }
        }

        void CycleColors(int duration)
        {
            Console.WriteLine("Cycle colors...");

            while (true)
            {
                ShowColorPulse(Color.Blue, duration);
                ShowColorPulse(Color.Cyan, duration);
                ShowColorPulse(Color.Green, duration);
                ShowColorPulse(Color.GreenYellow, duration);
                ShowColorPulse(Color.Yellow, duration);
                ShowColorPulse(Color.Orange, duration);
                ShowColorPulse(Color.OrangeRed, duration);
                ShowColorPulse(Color.Red, duration);
                ShowColorPulse(Color.MediumVioletRed, duration);
                ShowColorPulse(Color.Purple, duration);
                ShowColorPulse(Color.Magenta, duration);
                ShowColorPulse(Color.Pink, duration);
            }
        }

        void ShowColorPulse(Color color, int duration = 1000)
        {
            onboardLed.StartPulse(color, (duration / 2));
            Thread.Sleep(duration);
            onboardLed.Stop();
        }

        void ShowColor(Color color, int duration = 1000)
        {
            Console.WriteLine($"Color: {color}");
            onboardLed.SetColor(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }
    }
}
