using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using christec.windowsce.forms;
using System.Media;

namespace MobileTimer
{
    public partial class RunningTimer : UserControl
    {
        private TimeSpan initialDuration;
        private TimeSpan totalDuration;
        private DateTime startTime;
        
        public RunningTimer(TimeSpan duration)
        {
            InitializeComponent();

            this.initialDuration = duration;
            this.totalDuration = duration;

            this.Start();
        }

        public bool IsRunning
        {
            get
            {
                return this.updateTimer.Enabled;
            }
        }

        private void startStop_Click(object sender, EventArgs e)
        {
            if (this.updateTimer.Enabled == true)
            {
                this.Stop();
            }
            else
            {
                this.Start();
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTimeSinceStart = DateTime.Now - startTime;
            TimeSpan remainingTime = this.totalDuration - elapsedTimeSinceStart;

            if (remainingTime.TotalSeconds < 0)
            {
                this.Stop();
                this.totalDuration = this.initialDuration;
                
                NotificationWithSoftKeys notice = new NotificationWithSoftKeys();
                notice.Caption = "Timer Alert";
                notice.Critical = true;
                notice.DisplayOn = true;
                notice.Icon = Properties.Resources.timer;
                notice.LeftSoftKey = new NotificationSoftKey(SoftKeyType.Dismiss, "Dismiss");
                notice.RightSoftKey = new NotificationSoftKey(SoftKeyType.Dismiss, "Restart");
                notice.InitialDuration = 60;
                notice.Text = string.Format("Timer {0:00}:{1:00}:{2:00} elapsed!", this.initialDuration.Hours, this.initialDuration.Minutes, this.initialDuration.Seconds);
                notice.Visible = true;
                notice.RightSoftKeyClick += new EventHandler(notice_RightSoftKeyClick);
                notice.LeftSoftKeyClick += new EventHandler(notice_LeftSoftKeyClick);

                if (this.noticeTimer == null)
                {
                    this.noticeTimer = new Timer();
                    this.noticeTimer.Tick += new EventHandler(timer_Tick);
                    this.noticeTimer.Interval = 5000;
                }

                this.noticeStartTime = DateTime.Now;
                this.noticeTimer.Enabled = true;
            }
            else
            {
                int elapsedSeconds = (int) this.initialDuration.TotalSeconds - (int)remainingTime.TotalSeconds;
                if (elapsedSeconds != this.progress.Value)
                {
                    this.progress.Value = elapsedSeconds;

                    this.blockDigits1.DisplayTime(remainingTime);
                }
            }
        }

        private Timer noticeTimer = null;
        private DateTime noticeStartTime = DateTime.MinValue;

        void timer_Tick(object sender, EventArgs e)
        {
            SystemSounds.Exclamation.Play();

            if (this.noticeStartTime.AddMinutes(1) < DateTime.Now)
            {
                this.noticeTimer.Enabled = false;
            }
        }

        void notice_LeftSoftKeyClick(object sender, EventArgs e)
        {
            this.noticeTimer.Enabled = false;
        }

        void notice_RightSoftKeyClick(object sender, EventArgs e)
        {
            this.noticeTimer.Enabled = false;

            this.totalDuration = this.initialDuration;
            this.Start();
        }

        void Start()
        {
            this.updateTimer.Enabled = true;
            this.delete.Enabled = false;

            this.startStop.Text = "Stop";
            this.startTime = DateTime.Now;
            
            this.progress.Value = 0;
            this.progress.Maximum = (int)this.initialDuration.TotalSeconds;

            this.blockDigits1.DisplayTime(totalDuration);
        }

        void Stop()
        {
            this.updateTimer.Enabled = false;
            this.delete.Enabled = true;

            TimeSpan elapsedTimeSinceStart = DateTime.Now - this.startTime;
            this.totalDuration = this.totalDuration - elapsedTimeSinceStart;

            this.startStop.Text = "Start";
        }

        private void delete_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }
    }
}
