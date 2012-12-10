#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

#endregion

namespace msn2.net.Pictures.Controls.UserControls
{
    [DefaultEvent("TextChanged")]
    public partial class MultiItemTextBox : UserControl
    {
        private List<StringItem> items;
        private string textBeforeEdit = string.Empty;
        private bool displayMultipleItems;
        private bool hasFocus = false;

        public MultiItemTextBox()
        {
            InitializeComponent();

            items = new List<StringItem>();
            displayMultipleItems = true;

            if (this.DesignMode)
            {
                this.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (items.Count > 0)
            {
                textBox1.BackColor = SystemColors.Control;
                textBox1.BorderStyle = BorderStyle.None;
                
                if (0 != string.Compare(textBox1.Text, textBeforeEdit))
                {
                    if (StringItemChanged != null)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            string newValue = textBox1.Text;
                            if (items.Count > 1)
                            {
                                newValue += " " + (i + 1).ToString();
                            }
                            StringItemChangedEventArgs ev = new StringItemChangedEventArgs(items[i].Id, textBeforeEdit, newValue);
                            items[i].Text = newValue;
                            StringItemChanged(this, ev);
                        }
                    }
                }
            }

            UpdateDisplay();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
                hasFocus = true;

                if (items.Count > 0)
                {
                    textBox1.BackColor = SystemColors.Window;
                    textBox1.BorderStyle = BorderStyle.FixedSingle;

                    textBox1.Text = items[items.Count - 1].Text;
                    textBeforeEdit = textBox1.Text;

                    textBox1.SelectAll();
                }
        }

        public void FinishEdit()
        {
            textBox1_Leave(this, EventArgs.Empty);
        }

        public void ClearItems()
        {
            items.Clear();
            UpdateDisplay();
        }

        public void AddItem(int id, string value)
        {
            StringItem item = new StringItem();
            item.Id = id;
            item.Text = value;

            items.Add(item);

            UpdateDisplay();
        }

        public void RemoveItem(int id)
        {
            foreach (StringItem item in items)
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
            textBox1.ForeColor = SystemColors.WindowText;

            if (items.Count == 0)
            {
                textBox1.Text = "";
            }
            else if (items.Count == 1)
            {
                textBox1.Text = items[0].Text;
            }
            else if (displayMultipleItems)
            {
                string temp = "";

                foreach (StringItem item in items)
                {
                    if (item.Text.Length > 0)
                    {
                        if (temp.Length > 0)
                        {
                            temp += ", ";
                        }
                        temp += item.Text;
                    }
                }
                textBox1.Text = temp;
            }
            else
            {
                textBox1.ForeColor = SystemColors.GrayText;
                textBox1.Text = "<multiple items selected>";
            }
        }

        public bool MultiLine
        {
            get
            {
                return textBox1.Multiline;
            }
            set
            {
                textBox1.Multiline = value;
            }
        }

        public bool AcceptsReturn
        {
            get
            {
                return textBox1.AcceptsReturn;
            }
            set
            {
                textBox1.AcceptsReturn = value;
            }
        }

        public delegate void StringItemChangedEventHandler(object sender, StringItemChangedEventArgs e);
        
        [Browsable(true), Description("Fired when the text is changed.")]
        public event StringItemChangedEventHandler StringItemChanged;

        class StringItem
        {
            public int Id;
            public string Text;
        }

        /// <summary>
        /// True to display multiple items seperated by commas
        /// </summary>
        /// <value></value>
        public bool DisplayMultipleItems
        {
            get
            {
                return displayMultipleItems;
            }
            set
            {
                displayMultipleItems = value;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t')
            {
                textBox1_Leave(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void MultiItemTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Trace.WriteLine("Form-KeyPress: " + e.KeyChar.ToString());
        }

        private void MultiItemTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Trace.WriteLine("Form-KeyUp: " + e.KeyCode.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("TextChanged: " + textBox1.Text);
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
//            textBox1_Enter(sender, EventArgs.Empty);
        }
    }

    public class StringItemChangedEventArgs : EventArgs
    {
        private int id;
        private string oldValue;
        private string newValue;

        public StringItemChangedEventArgs(int id, string oldValue, string newValue)
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

        public string OldValue
        {
            get
            {
                return oldValue;
            }
        }

        public string NewValue
        {
            get
            {
                return newValue;
            }
        }
    }
}
