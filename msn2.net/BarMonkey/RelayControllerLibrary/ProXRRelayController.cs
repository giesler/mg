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
        private static object lockObject = new object();

        private SerialPort serialPort = null;

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
                        this.SendCommandAndFlush(33);

                        // Set auto refresh
                        this.SendCommandAndFlush(25);

                        // Disable reporting mode
                        this.SendCommandAndFlush(28);
                    }
                }
            }
        }

        private void CloseSerialPort()
        {
            if (this.serialPort.IsOpen == true)
            {
                // Turn off all ports
                this.SendCommandAndFlush(29);

                // TODO: Manually turn off all timers too

                this.serialPort.Close();
            }

            this.serialPort = null;
        }

        protected void SendCommandAndFlush(params int[] charCodes)
        {
            this.OpenAndConfigureSerialPort();

            this.SendCommand(charCodes);
            this.FlushBuffer();
        }

        protected void SendCommand(params int[] charCodes)
        {
            this.OpenAndConfigureSerialPort();

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

            Thread.Sleep(100);
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
