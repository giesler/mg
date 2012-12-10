using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MobileTimer
{
    public partial class NewTimer : Form
    {
        int current = 0;

        public NewTimer()
        {
            InitializeComponent();
        }

        private void NewTimer_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            int minutes = this.digit3.Digit.HasValue ? this.digit3.Digit.Value * 10 : 0;
            minutes += this.digit2.Digit.HasValue ? this.digit2.Digit.Value : 0;
            int seconds = this.digit1.Digit.HasValue ? this.digit1.Digit.Value * 10 : 0;
            seconds += this.digit0.Digit.HasValue ? this.digit0.Digit.Value : 0;

            TimeSpan duration = new TimeSpan(0, minutes, seconds);

            this.timer1.Interval = (int)duration.TotalMilliseconds;
            this.timer1.Enabled = true;

            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddKey(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddKey(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddKey(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddKey(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddKey(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddKey(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddKey(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddKey(8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddKey(9);
        }

        private void button0_Click(object sender, EventArgs e)
        {
            AddKey(0);
        }

        private void clear_Click(object sender, EventArgs e)
        {
            this.digit0.Digit = null;
            this.digit1.Digit = null;
            this.digit2.Digit = null;
            this.digit3.Digit = null;

            this.current = 0;
            this.ok.Enabled = false;
        }

        private void AddKey(int key)
        {
            if (current == 0)
            {
                this.digit0.Digit = key;
            }
            else if (current == 1)
            {
                this.digit1.Digit = this.digit0.Digit;
                this.digit0.Digit = key;
            }
            else if (current == 2)
            {
                this.digit2.Digit = this.digit1.Digit;
                this.digit1.Digit = this.digit0.Digit;
                this.digit0.Digit = key;
            }
            else if (current == 3)
            {
                this.digit3.Digit = this.digit2.Digit;
                this.digit2.Digit = this.digit1.Digit;
                this.digit1.Digit = this.digit0.Digit;
                this.digit0.Digit = key;
            }

            current++;
            this.ok.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan interval = new TimeSpan(0, 0, 0, 0, this.timer1.Interval);
            string message = string.Format(
                "{0}:{1} timer elapsed.",
                interval.Minutes.ToString(),
                interval.Seconds.ToString("00"));

            this.timer1.Enabled = true;
            MessageBox.Show(message, "Timer", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            this.Close();
        }
    }
}