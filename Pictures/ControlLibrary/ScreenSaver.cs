using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public class PictureScreenSaver: RandomSlideshow
    {
        // Keep track of whether the screensaver has become active.
        private bool isActive = false;

        // Keep track of the location of the mouse
        private Point mouseLocation;

        public PictureScreenSaver()
        {
            base.ToolStip.Visible = false;

            SetupFormProperties();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Set IsActive and MouseLocation only the first time this event is called.
            if (!isActive)
            {
                mouseLocation = MousePosition;
                isActive = true;
            }
            else
            {
                // If the mouse has moved significantly since first call, close.
                if ((Math.Abs(MousePosition.X - mouseLocation.X) > 10) ||
                    (Math.Abs(MousePosition.Y - mouseLocation.Y) > 10))
                {
                    Close();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool returnValue = true;

            switch (keyData)
            {
                case Keys.Down:
                case Keys.Right:
                    base.Next();
                    break;
                case Keys.Left:
                case Keys.Up:
                    base.Previous();
                    break;
                default:
                    //Close();
                    returnValue = base.ProcessCmdKey(ref msg, keyData);
                    break;
            }

            return returnValue;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Close();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.PictureItem.PaintPicture(e);
        }

        private void SetupFormProperties()
        {
            // Use double buffering to improve drawing performance
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            // Set the application to full screen mode and hide the mouse
            Cursor.Hide();
            Bounds = Screen.PrimaryScreen.Bounds;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            BackgroundImageLayout = ImageLayout.Stretch;

            // Capture the mouse
            this.Capture = true;
        }
    }
}
