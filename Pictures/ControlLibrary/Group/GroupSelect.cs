using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public partial class GroupSelect : Form
    {
        public GroupSelect()
        {
            InitializeComponent();
        }

        public GroupPicker GroupPicker
        {
            get
            {
                return this.groupPicker1;
            }
        }
    }
}