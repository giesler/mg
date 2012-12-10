#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls.UserControls
{
    public partial class DateTimePicker : UserControl
    {
        private DateTime originalValue;
        private List<DateTimeItem> items;

        public DateTimePicker()
        {
            items = new List<DateTimeItem>();

            InitializeComponent();
            this.Height = datePicker.Height;
            this.SizeControls();

            for (int i = 0; i < 60; i++)
            {
                minutePicker.Items.Add(i.ToString("00"));
            }

            hourPicker.Value = (DateTime.Now.Hour > 12 ? DateTime.Now.Hour - 12 : DateTime.Now.Hour);
            minutePicker.SelectedItem = FindItem(minutePicker, DateTime.Now.Minute.ToString("00"));
            string ampm = (DateTime.Now.Hour > 12 ? "pm" : "am");
            ampmPicker.SelectedItem = FindItem(ampmPicker, ampm);

            UpdateDisplay();
        }

        private void DateTimePicker_Resize(object sender, EventArgs e)
        {
            SizeControls();
        }


        public void ClearItems()
        {
            items.Clear();
            UpdateDisplay();
        }

        public void AddItem(int id, DateTime value)
        {
            DateTimeItem item = new DateTimeItem();
            item.Id = id;
            item.DateTime = value;

            items.Add(item);

            UpdateDisplay();
        }

        public void RemoveItem(int id)
        {
            foreach (DateTimeItem item in items)
            {
                if (item.Id == id)
                {
                    items.Remove(item);
                    break;
                }
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (items.Count == 0)
            {
                dateLabel.Text = "";
            }
            else if (items.Count == 1)
            {
                dateLabel.Text = items[0].DateTime.ToString("ddd MMM d, yy  h:mm tt");
                SetDateTime(items[0].DateTime);
            }
            else
            {
                dateLabel.Text = GetDateLabel();
            }
        }

        private string GetDateLabel()
        {
            DateTime minDate = items[0].DateTime;
            DateTime maxDate = items[0].DateTime;

            foreach (DateTimeItem dt in items)
            {
                if (dt.DateTime < minDate)
                {
                    minDate = dt.DateTime;
                }
                if (dt.DateTime > maxDate)
                {
                    maxDate = dt.DateTime;
                }
            }

            SetDateTime(minDate);

            TimeSpan diff = maxDate - minDate;

            // Check if all pictures on same day
            if (diff.TotalDays < 1)
            {
                return minDate.ToString("ddd MMM d hh:mm tt") + " - " + maxDate.ToString("hh:mm tt");
            }
            else if (diff.TotalDays < 365)
            {
                return minDate.ToString("ddd MMM d") + " - " + maxDate.ToString("ddd MMM d, yy");
            }
            else
            {
                return minDate.ToString("ddd MMM d, yy") + " - " + maxDate.ToString("ddd MMM d, yy");
            }
        }

        public delegate void DateTimeItemChangedEventHandler(object sender, DateTimeItemChangedEventArgs e);
        public event DateTimeItemChangedEventHandler DateTimeItemChanged;

        public DateTime Value
        {
            get
            {
                string temp = datePicker.Value.ToShortDateString();
                temp += " " + hourPicker.Value.ToString();
                temp += ":" + minutePicker.SelectedItem.ToString();
                temp += " " + ampmPicker.SelectedItem.ToString();
                return DateTime.Parse(temp);
            }
        }

        private void SetDateTime(DateTime value)
        {
            datePicker.Value = value;

            int hour = (value.Hour > 12 ? value.Hour - 12 : value.Hour);
            if (hour == 0) hour = 12;
            hourPicker.Value = hour;
            minutePicker.SelectedItem = FindItem(minutePicker, value.Minute.ToString("00"));
            string ampm = (value.Hour > 12 ? "pm" : "am");
            ampmPicker.SelectedItem = FindItem(ampmPicker, ampm);

            originalValue = value;
        }

        private object FindItem(DomainUpDown upDown, string value)
        {
            foreach (string item in upDown.Items)
            {
                if (item == value)
                {
                    return item;
                }
            }

            return null;
        }

        private void SizeControls()
        {
            dateLabel.Top = datePicker.Top;
            dateLabel.Left = datePicker.Left;
            dateLabel.Width = this.Width;
            dateLabel.Height = datePicker.Height;

            SwitchEditMode(false);
        }

        private void dateLabel_Click(object sender, EventArgs e)
        {
            SwitchEditMode(true);
            datePicker.Focus();
        }

        private void timeLabel_Click(object sender, EventArgs e)
        {
            SwitchEditMode(true);
            hourPicker.Focus();
        }

        private void DateTimePicker_Enter(object sender, EventArgs e)
        {
            SwitchEditMode(true);
        }

        private void DateTimePicker_Leave(object sender, EventArgs e)
        {
            SwitchEditMode(false);

            DateTime newValue = Value;
            TimeSpan ts = originalValue - newValue;
            if (ts.TotalSeconds > 90 || ts.TotalSeconds < -90)
            {
                if (null != this.DateTimeItemChanged)
                {
                    foreach (DateTimeItem item in items)
                    {
                        this.DateTimeItemChanged(this, new DateTimeItemChangedEventArgs(item.Id, item.DateTime, newValue));
                        item.DateTime = newValue;
                    }
                }
            }
        }

        public void FinishEdit()
        {
            DateTimePicker_Leave(this, EventArgs.Empty);
        }

        private void SwitchEditMode(bool enableEdit)
        {
            dateLabel.Visible = !enableEdit;
            datePicker.Visible = enableEdit;

            hourPicker.Visible = enableEdit;
            minutePicker.Visible = enableEdit;
            ampmPicker.Visible = enableEdit;
        }

    }

    public class DateTimeItem
    {
        public int Id;
        public DateTime DateTime;
    }


    public class DateTimeItemChangedEventArgs : EventArgs
    {
        private int id;
        private DateTime oldValue;
        private DateTime newValue;

        public DateTimeItemChangedEventArgs(int id, DateTime oldValue, DateTime newValue)
        {
            this.id = id;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public DateTime OldValue
        {
            get
            {
                return oldValue;
            }
        }

        public DateTime NewValue
        {
            get
            {
                return newValue;
            }
        }
    }
}
