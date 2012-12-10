﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace MobileTimer
{
    class PowerManager
    {
        private IntPtr m_hEvtQuit;
        private Thread m_th;
        private IntPtr m_hQueue;
        private string m_MessageQueueName = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);

        public event PowerStateChangedEventHandler PowerStateChanged;

        public PowerManager()
        {
            // Create event to stop the thread
            m_hEvtQuit = CreateEvent(0, true, false, null);

            // Create listener thread
            m_th = new Thread(new ThreadStart(WaitThreadProc));

            // Create message queue for power broadcasts
            m_hQueue = CreateMsgQueue(m_MessageQueueName, new MSGQUEUEOPTIONS());

            // Start listening
            m_th.Start();

            // Register for power notifications.
            IntPtr hNotif = RequestPowerNotifications(m_hQueue, 
                PowerEventType.PBT_POWERSTATUSCHANGE | PowerEventType.PBT_RESUME 
                | PowerEventType.PBT_TRANSITION | PowerEventType.PBT_POWERINFOCHANGE);
        }

        public void Close()
        {
            try
            {
                // Tell listener thread to stop 
                EventModify(m_hEvtQuit, EVENT_SET);

                // Clean up
                CloseHandle(m_hQueue);
            }
            catch
            {
            }
        }


        // Listener thread
        void WaitThreadProc()
        {
            // Open our message queue for reading
            MSGQUEUEOPTIONS opt = new MSGQUEUEOPTIONS();
            opt.bReadAccess = 1;
            IntPtr hQueue = CreateMsgQueue(m_MessageQueueName, opt);

            // Start waiting for Message Queue to fill up, while checking for a termination request
            while (WaitForSingleObject(m_hEvtQuit, 0) != WAIT_OBJECT_0)
            {
                // Did queue get some data?
                if (WaitForSingleObject(hQueue, 100) == WAIT_OBJECT_0)
                {
                    // Yes, let's read it
                    // 1024 byte buffer should be well enough. 
                    POWER_BROADCAST pb = new POWER_BROADCAST(1024);
                    int nRead, Flags;
                    ReadMsgQueue(hQueue, pb.Data, pb.Data.Length, out nRead, 0, out Flags);

                    if (this.PowerStateChanged != null)
                    {
                        PowerStateEventArgs e = new PowerStateEventArgs() { PowerEventType = pb.Message, PowerState = pb.Flags };
                        this.PowerStateChanged(this, e);
                    }                    
                    //// Is this an event we wanted?
                    //if ((pb.Message & PowerEventType.PBT_TRANSITION) == PowerEventType.PBT_TRANSITION &&
                    //    (pb.Flags & PowerState.POWER_STATE_ON) == PowerState.POWER_STATE_ON)
                    //{
                    //    //Notify parent - launch login dialog
                    //    this.Invoke(new EventHandler(Login));
                    //}

                }
            }
            CloseHandle(hQueue);
            CloseHandle(m_hEvtQuit);
        }

        #region P/Invoke Stuff

        [DllImport("coredll")]
        static extern IntPtr CreateMsgQueue(string Name, MSGQUEUEOPTIONS Options);

        [DllImport("coredll")]
        static extern bool ReadMsgQueue(
            IntPtr hMsgQ,
            byte[] lpBuffer,
            int cbBufferSize,
            out int lpNumberOfBytesRead,
            int dwTimeout,
            out int pdwFlags
            );


        [DllImport("coredll")]
        static extern IntPtr CreateEvent(int dwReserved1, bool bManualReset, bool bInitialState, string Name);

        [DllImport("coredll")]
        static extern bool EventModify(IntPtr hEvent, int func);

        [DllImport("coredll")]
        static extern bool CloseHandle(IntPtr h);

        [DllImport("coredll")]
        static extern IntPtr RequestPowerNotifications(IntPtr hMsgQ, PowerEventType Flags);

        [DllImport("coredll")]
        static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

        const int WAIT_OBJECT_0 = 0;
        const int EVENT_SET = 3;

        #endregion

    }
    
    public delegate void PowerStateChangedEventHandler(object sender, PowerStateEventArgs e);

    public class PowerStateEventArgs : EventArgs
    {
        public PowerEventType PowerEventType { get; set; }
        public PowerState PowerState { get; set; }
    }

    [Flags]
    public enum PowerEventType
    {
        PBT_TRANSITION = 0x00000001,
        PBT_RESUME = 0x00000002,
        PBT_POWERSTATUSCHANGE = 0x00000004,
        PBT_POWERINFOCHANGE = 0x00000008,
    }

    [Flags]
    public enum PowerState
    {
        POWER_STATE_ON = (0x00010000),
        POWER_STATE_OFF = (0x00020000),

        POWER_STATE_CRITICAL = (0x00040000),
        POWER_STATE_BOOT = (0x00080000),
        POWER_STATE_IDLE = (0x00100000),
        POWER_STATE_SUSPEND = (0x00200000),
        POWER_STATE_RESET = (0x00800000),
    }

    class MSGQUEUEOPTIONS
    {
        public MSGQUEUEOPTIONS()
        {
            dwSize = Marshal.SizeOf(typeof(MSGQUEUEOPTIONS));
            dwFlags = 0;
            dwMaxMessages = 20;
            cbMaxMessage = 100;
            bReadAccess = 0;
        }
        public int dwSize;
        public int dwFlags;
        public int dwMaxMessages;
        public int cbMaxMessage;
        public int bReadAccess;
    }

    class POWER_BROADCAST
    {
        public POWER_BROADCAST(int size)
        {
            m_data = new byte[size];
        }
        byte[] m_data;
        public byte[] Data { get { return m_data; } }
        public PowerEventType Message { get { return (PowerEventType)BitConverter.ToInt32(m_data, 0); } }
        public PowerState Flags { get { return (PowerState)BitConverter.ToInt32(m_data, 4); } }
        public int Length { get { return BitConverter.ToInt32(m_data, 8); } }
        public byte[] SystemPowerState { get { byte[] data = new byte[Length]; Buffer.BlockCopy(m_data, 12, data, 0, Length); return data; } }
    }
}
