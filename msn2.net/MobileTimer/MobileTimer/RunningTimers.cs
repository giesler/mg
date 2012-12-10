using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MobileTimer
{
    public partial class RunningTimers : Form
    {
        private PowerManager powerManager = null;
        private Timer addItemTimer = null;

        public RunningTimers()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.timer;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.addItemTimer = new Timer();
            this.addItemTimer.Interval = 500;
            this.addItemTimer.Tick += new EventHandler(addItemTimer_Tick);
            this.addItemTimer.Enabled = true;
        }

        void addItemTimer_Tick(object sender, EventArgs e)
        {
            if (this.addItemTimer.Enabled == true)
            {
                this.addItemTimer.Enabled = false;

                NewTimer newTimer = new NewTimer();
                if (newTimer.ShowDialog() == DialogResult.OK && newTimer.Duration.TotalSeconds > 0)
                {
                    AddTimer(newTimer.Duration);

                    this.powerManager = new PowerManager();
                    this.powerManager.PowerStateChanged += new PowerStateChangedEventHandler(powerManager_PowerStateChanged);
                }
                else
                {
                    this.Close();
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            bool running = false;
            foreach (Control c in this.Controls)
            {
                RunningTimer timer = c as RunningTimer;
                if (timer != null)
                {
                    if (timer.IsRunning == true)
                    {
                        running = true;
                        break;
                    }
                }
            }

            if (running == true)
            {
                string msg = "There are still running timers.  Closing will stop the timers.  Do you want to close?";
                DialogResult result = MessageBox.Show(msg, "Running Timers", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            if (e.Cancel == false)
            {
                if (this.powerManager != null)
                {
                    this.powerManager.Close();
                    this.powerManager = null;
                }
            }
        }

        private void AddTimer(TimeSpan duration)
        {
            this.SuspendLayout();

            RunningTimer timer = new RunningTimer(duration);
            timer.Dock = DockStyle.Top;
            timer.Parent = this;
            timer.SendToBack();

            this.ResumeLayout();
        }

        void powerManager_PowerStateChanged(object sender, PowerStateEventArgs e)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new PowerStateChangedEventHandler(this.powerManager_PowerStateChanged), sender, e);
            }
            else
            {
                this.debugText.Text = string.Format(
                    "{0}{1}: PowerEventType: {2}, PowerState: {3}{4}",
                    this.debugText.Text,
                    DateTime.Now.ToLongTimeString(),
                    e.PowerEventType,
                    e.PowerState,
                    Environment.NewLine);
                this.debugText.SelectionStart = this.debugText.Text.Length;
            }
        }

        private void menuUpdateApp_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("http://home.msn2.net/cab/MobileTimerCab.cab", "");
            p.Start();

            this.Close();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            NewTimer newTimer = new NewTimer();
            if (newTimer.ShowDialog() == DialogResult.OK && newTimer.Duration.TotalSeconds > 0)
            {
                this.AddTimer(newTimer.Duration);
            }
        }

        private void menuDebug_Click(object sender, EventArgs e)
        {
            this.menuDebug.Checked = !this.menuDebug.Checked;
            this.debugText.Visible = this.menuDebug.Checked;
        }

    }
}