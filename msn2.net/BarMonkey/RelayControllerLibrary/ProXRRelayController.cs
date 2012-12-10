using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using msn2.net.BarMonkey.RelayController;
using System.IO.Ports;
using System.Threading;
using System.Configuration;

namespace msn2.net.BarMonkey
{
    public abstract class ProXRRelayController: IRelayController
    {
        public enum RelayControllerRefreshMode
        {
            AutomaticRefresh = 0,
            ManualRefresh
        }

        private static object lockObject = new object();

        private SerialPort serialPort = null;
        private RelayControllerRefreshMode refreshMode = RelayControllerRefreshMode.AutomaticRefresh;

        ~ProXRRelayController()
        {
            if (serialPort != null)
            {
                CloseSerialPort();
            }
        }

        public void ConnectTest()
        {
            this.OpenAndConfigureSerialPort();
            this.CloseSerialPort();
        }

        public void SendBatch(List<BatchItem> batch)
        {
            Console.WriteLine("Sending batch...");

            try
            {
                int currentGroup = batch[0].Group;
                List<BatchItem> pendingList = new List<BatchItem>();

                foreach (BatchItem item in batch)
                {
                    if (item.Group != currentGroup)
                    {
                        SendBatchGroup(pendingList);
                        pendingList.Clear();
                        currentGroup = item.Group;
                    }

                    pendingList.Add(item);

                    if (pendingList.Count == 16)
                    {
                        SendBatchGroup(pendingList);
                        pendingList.Clear();
                    }
                }

                if (pendingList.Count > 0)
                {
                    SendBatchGroup(pendingList);
                }

                Console.WriteLine("Send complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending: " + ex.Message);
            }
            finally
            {
                this.CloseSerialPort();
            }
        }

        protected abstract void SendBatchGroup(List<BatchItem> batch);
        
        private void OpenAndConfigureSerialPort()
        {
            if (this.serialPort == null)
            {
                lock (ProXRRelayController.lockObject)
                {
                    if (this.serialPort == null)
                    {
                        string comPort = ConfigurationManager.AppSettings["RelayController.ComPort"];
                        if (comPort == null)
                        {
                            comPort = "COM1";
                        }
                        int baudRate = 115200;
                        string baudRateSetting = ConfigurationManager.AppSettings["RelayController.BaudRate"];
                        if (baudRateSetting != null)
                        {
                            baudRate = int.Parse(baudRateSetting);
                        }

                        this.serialPort = new SerialPort(
                                              comPort,
                                              baudRate,
                                              Parity.None,
                                              8,
                                              StopBits.One);
                        this.serialPort.Encoding = Encoding.GetEncoding("iso-8859-1");

                        this.serialPort.Open();

                        // Connect test
                        this.SendCommandAndFlush("connect", 33);

                        this.refreshMode = GetRefreshMode();
                        if (this.refreshMode == RelayControllerRefreshMode.AutomaticRefresh)
                        {
                            this.SendCommandAndFlush("auto refresh", 25);
                        }
                        else
                        {
                            this.SendCommandAndFlush("manual refresh", 26);
                        }

                        this.SendCommandAndFlush("disable reporting mode", 28);
                    }
                }
            }
        }

        protected virtual RelayControllerRefreshMode GetRefreshMode()
        {
            return RelayControllerRefreshMode.AutomaticRefresh;
        }

        private void CloseSerialPort()
        {
            if (this.serialPort != null && this.serialPort.IsOpen == true)
            {
                Console.WriteLine("Closing serial port..");

                this.SendCommandAndFlush("turn off all ports", 29);

                if (this.refreshMode == RelayControllerRefreshMode.ManualRefresh)
                {
                    this.SendCommandAndFlush("flush", 37);
                }

                // TODO: Manually turn off all timers too

                this.serialPort.Close();

                Console.WriteLine("Serial port closed.");
            }

            this.serialPort = null;
        }

        protected void SendCommandAndFlush(string commandName, params int[] charCodes)
        {
            this.OpenAndConfigureSerialPort();

            this.SendCommand(commandName, charCodes);
            this.FlushBuffer();
        }

        protected void SendCommand(string commandName, params int[] charCodes)
        {
            this.OpenAndConfigureSerialPort();

            Console.Write("Sending '{0}': ", commandName);

            char[] chars = new char[charCodes.Length + 1];
            chars[0] = (char)254;
            for (int i = 0; i < charCodes.Length; i++)
            {
                chars[i + 1] = (char)charCodes[i];
                Console.Write(charCodes[i].ToString());
                Console.Write(" ");
            }
            Console.WriteLine();

            this.serialPort.Write(chars, 0, chars.Length);
        }

        private void FlushBuffer()
        {
            this.OpenAndConfigureSerialPort();

            Thread.Sleep(50);
            int charsRead = 0;
            while (this.serialPort.BytesToRead > 0)
            {
                int newChar = this.serialPort.ReadChar();
                Console.Write(newChar + " ");
            }
            if (charsRead > 0)
            {
                Console.WriteLine();
            }
        }
    }
}
