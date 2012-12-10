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
        public RunningTimers()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.timer;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            NewTimer newTimer = new NewTimer();
            if (newTimer.ShowDialog() == DialogResult.OK && newTimer.Duration.TotalSeconds > 0)
            {
                AddTimer(newTimer.Duration);
            }
            else
            {
                this.Close();
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

        private void menuUpdateApp_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("http://home.msn2.net/cab/MobileTimerCab.cab", "");
            p.Start();

            this.Close();

            Application.Exit();
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

    }
}