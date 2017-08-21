using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public abstract class BaseLinkItem: UserControl
    {
        private string text;

        public override string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Font font = this.Font;

            SizeF stringSize = e.Graphics.MeasureString(text, font);
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
                e.Graphics.DrawString(text, font, SystemBrushes.HighlightText, textRect);
            }
            else
            {
                e.Graphics.DrawString(text, font, SystemBrushes.WindowText, textRect);
            }
        }
    }
}
