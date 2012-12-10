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
        public TimeSpan Duration { get; private set; }

        public NewTimer()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.timer;
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
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
            this.blockDigits1.DisplayTime(null, null, null);

            this.current = 0;
            this.ok.Enabled = false;

            this.hours = 0;
            this.minutes = 0;
            this.seconds = 0;
            this.Duration = TimeSpan.Zero;
        }

        private int hours = 0;
        private int minutes = 0;
        private int seconds = 0;

        private void AddKey(int key)
        {
            if (current == 0)
            {
                this.seconds = key;
            }
            else if (current >= 1 && current < 6)
            {
                if (current >= 2)
                {
                    if (current >= 4)
                    {
                        this.hours = (this.hours * 10) + (this.minutes / 10);
                    }
                    
                    this.minutes = ((this.minutes % 10) * 10) + (this.seconds / 10);
                }

                this.seconds = ((this.seconds % 10) * 10) + key;
            }

            this.blockDigits1.DisplayTime(this.hours, this.minutes, this.seconds);

            this.Duration = new TimeSpan(this.hours, this.minutes, this.seconds);

            current++;
            this.ok.Enabled = true;
        }
    }
}