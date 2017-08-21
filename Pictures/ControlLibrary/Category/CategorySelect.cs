using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public partial class CategorySelect : PropertyForm
    {
        public CategorySelect()
        {
            InitializeComponent();
        }

        public CategoryPicker CategoryPicker
        {
            get
            {
                return this.categoryPicker1;
            }
        }
    }
}