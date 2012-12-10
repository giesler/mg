using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MobileTimer
{
    public class BlockDigit : UserControl
    {
        const int DigitLineWidth = 4;

        public BlockDigit()
        {
            this.BackColor = SystemColors.Window;
        }

        private Nullable<int> digit = null;

        public Nullable<int> Digit
        {
            get
            {
                return this.digit;
            }
            set
            {
                if (this.digit != value)
                {
                    this.digit = value;
                    this.Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Brush brush = new SolidBrush(SystemColors.ScrollBar))
            {
                PaintTop(brush, e);
                PaintBottom(brush, e);
                PaintMiddle(brush, e);
                PaintUpperLeft(brush, e);
                PaintUpperRight(brush, e);
                PaintLowerLeft(brush, e);
                PaintLowerRight(brush, e);
            }

            if (this.Digit.HasValue == true)
            {
                using (Brush brush = new SolidBrush(SystemColors.ActiveBorder))
                {
                    switch (this.Digit)
                    {
                        case 0:
                            PaintTop(brush, e);
                            PaintBottom(brush, e);
                            PaintUpperLeft(brush, e);
                            PaintUpperRight(brush, e);
                            PaintLowerLeft(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 1:
                            PaintUpperRight(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 2:
                            PaintTop(brush, e);
                            PaintBottom(brush, e);
                            PaintMiddle(brush, e);
                            PaintUpperRight(brush, e);
                            PaintLowerLeft(brush, e);
                            break;
                        case 3:
                            PaintTop(brush, e);
                            PaintBottom(brush, e);
                            PaintMiddle(brush, e);
                            PaintUpperRight(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 4:
                            PaintMiddle(brush, e);
                            PaintUpperLeft(brush, e);
                            PaintUpperRight(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 5:
                            PaintTop(brush, e);
                            PaintBottom(brush, e);
                            PaintMiddle(brush, e);
                            PaintUpperLeft(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 6:
                            PaintTop(brush, e);
                            PaintBottom(brush, e);
                            PaintMiddle(brush, e);
                            PaintUpperLeft(brush, e);
                            PaintLowerLeft(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 7:
                            PaintTop(brush, e);
                            PaintUpperRight(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 8:
                            PaintTop(brush, e);
                            PaintBottom(brush, e);
                            PaintMiddle(brush, e);
                            PaintUpperLeft(brush, e);
                            PaintUpperRight(brush, e);
                            PaintLowerLeft(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                        case 9:
                            PaintTop(brush, e);
                            PaintBottom(brush, e);
                            PaintMiddle(brush, e);
                            PaintUpperLeft(brush, e);
                            PaintUpperRight(brush, e);
                            PaintLowerRight(brush, e);
                            break;
                    }
                }
            }
        }

        void PaintTop(Brush brush, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, DigitLineWidth);
            e.Graphics.FillRectangle(brush, rect);
        }

        void PaintBottom(Brush brush, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, this.Height - DigitLineWidth, this.Width, DigitLineWidth);
            e.Graphics.FillRectangle(brush, rect);
        }

        void PaintMiddle(Brush brush, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, this.Height / 2 - 1, this.Width, DigitLineWidth);
            e.Graphics.FillRectangle(brush, rect);
        }

        void PaintUpperLeft(Brush brush, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, DigitLineWidth, this.Height / 2);
            e.Graphics.FillRectangle(brush, rect);
        }

        void PaintUpperRight(Brush brush, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(this.Width - DigitLineWidth, 0, DigitLineWidth, this.Height / 2);
            e.Graphics.FillRectangle(brush, rect);
        }

        void PaintLowerLeft(Brush brush, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, this.Height / 2, DigitLineWidth, this.Height - (this.Height / 2));
            e.Graphics.FillRectangle(brush, rect);
        }

        void PaintLowerRight(Brush brush, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(this.Width - DigitLineWidth, this.Height / 2, DigitLineWidth, this.Height - (this.Height / 2));
            e.Graphics.FillRectangle(brush, rect);
        }

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    // Don't want a background
        //    //base.OnPaintBackground(e);
        //}
    }
}