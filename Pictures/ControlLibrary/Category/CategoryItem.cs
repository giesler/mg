#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using msn2.net.Pictures;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class CategoryItem : UserControl
    {
        private Category category;

        public CategoryItem(Category category)
        {
            InitializeComponent();

            
            this.category = category;
        }

        public Category Category
        {
            get
            {
                return this.category;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            string name = this.category.Name;
            Font font = this.Font;

            SizeF stringSize = e.Graphics.MeasureString(name, font);
            if (this.Width != stringSize.Width)
            {
                this.Width = Convert.ToInt32(stringSize.Width);
            }

            RectangleF textRect = new RectangleF(0, 0, this.Width, this.Height);

            if (base.Focused)
            {
                // Draw highlight
                using (Pen pen = new Pen(SystemBrushes.Highlight))
                {
                    e.Graphics.DrawRectangle(pen, this.ClientRectangle);
                }
                e.Graphics.DrawString(name, font, SystemBrushes.HighlightText, textRect);
            }
            else
            {
                e.Graphics.DrawString(name, font, SystemBrushes.WindowText, textRect);
            }
        }

    }
}
