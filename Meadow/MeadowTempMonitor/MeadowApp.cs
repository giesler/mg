using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Atmospheric;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MeadowTempMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Color[] colors = new Color[4]
        {
            Color.FromHex("#008500"),
            Color.FromHex("#269926"),
            Color.FromHex("#00CC00"),
            Color.FromHex("#67E667")
        };

        AnalogTemperature analogTemperature;
        St7789 st7789;
        GraphicsLibrary graphics;
        int displayWidth, displayHeight;

        public MeadowApp()
        {
            Console.WriteLine("Initializing app...");

            analogTemperature = new AnalogTemperature(
                device: Device,
                analogPin: Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );
            analogTemperature.Updated += AnalogTemperatureUpdated;

            var config = new SpiClockConfiguration(
                6000,
                SpiClockConfiguration.Mode.Mode3);

            st7789 = new St7789(
                device: Device,
                spiBus: Device.CreateSpiBus(
                    Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );
            displayWidth = Convert.ToInt32(st7789.Width);
            displayHeight = Convert.ToInt32(st7789.Height);

            graphics = new GraphicsLibrary(st7789);
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;
            LoadScreen();

            //Console.WriteLine("init wifi");
            //Device.InitWiFiAdapter().Wait();
            //Thread.Sleep(TimeSpan.FromMinutes(2));

//            Console.WriteLine("Connecting...");
//            try
//            {
//                var wifi = Device.WiFiAdapter;

////                var nets = Device.WiFiAdapter.IpAddress;
//  //              Console.WriteLine("Networks: " + nets.Count.ToString());

//                var result = Device.WiFiAdapter.Connect("MS2N Guest", "domecamp");
//                Console.WriteLine("Connection status - " + result.ConnectionStatus.ToString());

//                if (result.ConnectionStatus != Meadow.Gateway.WiFi.ConnectionStatus.Success)
//                {
//                    throw new Exception("Unable to connect - " + result.ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Connection error: " + ex.ToString());
//            }
            
            analogTemperature.StartUpdating();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        void LoadScreen()
        {
            Console.WriteLine("LoadScreen...");

            graphics.Clear();

            int radius = 225;
            int originX = displayWidth / 2;
            int originY = displayHeight / 2 + 130;

            graphics.Stroke = 3;
            for (int i = 1; i < 2; i++)
            {
                graphics.DrawCircle(
                    centerX: originX,
                    centerY: originY,
                    radius: radius,
                    color: colors[i - 1],
                    filled: true);

                graphics.Show();
                radius -= 20;
            }

            graphics.DrawLine(0, 220, 240, 220, Color.White);
            graphics.DrawLine(0, 230, 240, 230, Color.White);

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(54, 130, "TEMP", Color.White);

            graphics.Show();
        }

        void AnalogTemperatureUpdated(object sender, AtmosphericConditionChangeResult e)
        {
            float oldTemp = e.Old.Temperature.Value / 1000;
            float newTemp = e.New.Temperature.Value / 1000;

            float oldTempF = oldTemp * 1.8f + 32;
            float newTempF = newTemp * 1.8f + 32;

            Console.WriteLine(newTempF.ToString());

            graphics.DrawText(
                x: 48, y: 160,
                text: newTempF.ToString("00.0"),
                color: colors[colors.Length - 1],
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);
            graphics.DrawText(
                x: 48, y: 160,
                text: newTempF.ToString("00.0"),
                color: Color.White,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();
        }
    }
}