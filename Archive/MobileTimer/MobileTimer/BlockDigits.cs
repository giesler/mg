using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MobileTimer
{
    public partial class BlockDigits : UserControl
    {
        public BlockDigits()
        {
            InitializeComponent();
        }

        public void DisplayTime(TimeSpan timeSpan)
        {
            int hours = timeSpan.Hours;
            if (timeSpan.Days > 0)
            {
                hours += (timeSpan.Days * 24);
            }
            this.DisplayTime(hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public void DisplayTime(Nullable<int> hours, Nullable<int> minutes, Nullable<int> seconds)
        {
            this.hour1.Digit = null;
            this.hour0.Digit = null;

            if (hours.HasValue == true && hours.Value > 0)
            {
                if (hours.Value >= 10)
                {
                    this.hour1.Digit = hours.Value / 10;
                }
                this.hour0.Digit = hours.Value % 10;
            }

            this.minute1.Digit = null;
            this.minute0.Digit = null;

            if (minutes.HasValue == true && (minutes.Value > 0 || hours.Value > 0))
            {
                if (minutes.Value >= 10 || (hours.HasValue && hours.Value > 0))
                {
                    this.minute1.Digit = minutes.Value / 10;
                }
                this.minute0.Digit = minutes.Value % 10;
            }

            this.second1.Digit = null;
            this.second0.Digit = null;

            if (seconds.HasValue == true && (seconds.Value > 0 || (minutes.HasValue && minutes.Value > 0) || (hours.HasValue && hours.Value > 0)))
            {
                if (seconds.Value >= 10 || ((minutes.HasValue && minutes.Value > 0) || (hours.HasValue && hours.Value > 0)))
                {
                    this.second1.Digit = seconds.Value / 10;
                }
                this.second0.Digit = seconds.Value % 10;
            }
        }
    }
}
