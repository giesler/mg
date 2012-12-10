using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Configuration;
using System.Text;
using System.Threading;

namespace msn2.net.BarMonkey.RelayController
{
    public class ProXRRelayController : IRelayController
    {
        private SerialPort serialPort = null;
        private static object lockObject = new object();

        ~ProXRRelayController()
        {
            if (serialPort != null)
            {
                CloseSerialPort();
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

        #region IRelayController Members

        public void ConnectTest()
        {
            this.OpenAndConfigureSerialPort();
            this.CloseSerialPort();            
        }

        public void SendBatch(List<BatchItem> batch)
        {
            try
            {
                this.OpenAndConfigureSerialPort();

                int currentGroup = batch[0].Group;
                List<BatchItem> pendingList = new List<BatchItem>();

                foreach (BatchItem item in batch)
                {
                    if (item.Group != currentGroup)
                    {
                        SendProcessedBatch(pendingList);
                        pendingList.Clear();
                        currentGroup = item.Group;
                    }

                    pendingList.Add(item);

                    if (pendingList.Count == 16)
                    {
                        SendProcessedBatch(pendingList);
                        pendingList.Clear();
                    }                 
                }

                if (pendingList.Count > 0)
                {
                    SendProcessedBatch(pendingList);
                }
            }
            finally
            {
                this.CloseSerialPort();
            }
        }

        #endregion

        private void SendProcessedBatch(List<BatchItem> batch)
        {
            int maxSeconds = 0;
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

                this.SendCommandAndFlush(50, selectedTimerIndex, 0, 0, item.Seconds, item.RelayNumber);

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

            this.SendCommandAndFlush(50, 131, lsb, usb);

            Thread.Sleep(1000 * maxSeconds + 500);
        }

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

        private void SendCommandAndFlush(params int[] charCodes)
        {
            SendCommand(charCodes);
            FlushBuffer();
        }

        private void SendCommand(params int[] charCodes)
        {
            char[] chars = new char[charCodes.Length + 1];
            chars[0] = (char)254;
            for (int i = 0; i < charCodes.Length; i++)
            {
                chars[i+1] = (char)charCodes[i];
            }

            this.serialPort.Write(chars, 0, chars.Length);
        }

        private void FlushBuffer()
        {
            Thread.Sleep(100);
            while (this.serialPort.BytesToRead > 0)
            {
                this.serialPort.ReadChar();
            }
        }
    }
}
