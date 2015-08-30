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

namespace msn2.net.Pictures.Controls
{
    public partial class PictureStack : UserControl
    {
        private List<PictureItem> pictures;
        public List<PictureItem> Pictures
        {
            get
            {
                return pictures;
            }

            set
            {
                pictures = value;
            }
        }


        public PictureStack()
        {
            InitializeComponent();
            pictures = new List<PictureItem>();
            this.Resize += new EventHandler(PictureStack_Resize);
            UpdateControls();
        }

        public void AddPicture(Picture picture)
        {
            PictureItem item = new PictureItem(picture);
            //item.CustomPaint = true;
            //item.RequestPaint += new EventHandler(item_RequestPaint);
            pictures.Add(item);
            this.Controls.Add(item);
            UpdateControls();

            if (!this.suspendPaint)
            {
                SizePictures();
            }
        }

        public void RemovePicture(int pictureId)
        {
            foreach (PictureItem item in pictures)
            {
                if (item.PictureId == pictureId)
                {
                    pictures.Remove(item);
                    this.Controls.Remove(item);
                    item.Dispose();
                    UpdateControls();
                    break;
                }
            }

            if (!this.suspendPaint)
            {
                SizePictures();
            }
        }

        private void UpdateControls()
        {
        }

        private void SizePictures()
        {
            this.SuspendLayout();

            int pictureCount = (pictures.Count > 5 ? 5 : pictures.Count);
            if (pictureCount == 0)
            {
                return;
            }

            int sideOffset  = 2;
            int itemWidth = this.Width - (sideOffset * 2) - (pictureCount * 6);
            int itemHeight = this.Height - (sideOffset * 2) - (pictureCount * 6);

            // Draw pictures in reverse order
            int itemCount = 0;
            for (int pictureNumber = pictures.Count - pictureCount; 
                pictureNumber < pictures.Count; pictureNumber++)
            {
                PictureItem item = pictures[pictureNumber];
                item.Visible = true;

                item.Left = 2 + (itemCount * 6);
                item.Top = item.Left;

                item.Width = itemWidth;
                item.Height = itemHeight;

   //             item.BringToFront();
                itemCount++;
            }

            // Hide remaining images
/*            for (int pictureNumber = 0; pictureNumber < pictures.Count - pictureCount; pictureNumber++)
            {
                Trace.WriteLine(string.Format("Hiding image {0}, picture ID {1}", pictureNumber, pictures[pictureNumber].PictureId));
                this.pictures[pictureNumber].Visible = false;
                this.pictures[pictureNumber].SendToBack();
            }
            */
            this.ResumeLayout();

            this.Refresh();
        }

        private void PictureStack_Paint(object sender, PaintEventArgs e)
        {
            int pictureCount = (pictures.Count > 5 ? 5 : pictures.Count);
            for (int pictureNumber = pictures.Count - pictureCount;
                pictureNumber < pictures.Count; pictureNumber++)
            {
                try
                {
                    PictureItem item = pictures[pictureNumber];
                    item.CustomPaintControl(e, item.Left, item.Top);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("PictureStack_Paint exception: " + ex.Message);
                }
            }

        }

        //void item_RequestPaint(object sender, EventArgs e)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.BeginInvoke(new MethodInvoker(this.Refresh));
        //    }
        //    else
        //    {
        //        this.Refresh();
        //    }
        //}

        bool suspendPaint = false;
        public bool SuspendPaint
        {
            get
            {
                return suspendPaint;
            }

            set
            {
                suspendPaint = value;
                if (!suspendPaint)
                {
                    SizePictures();
                }
            }
        }


        void PictureStack_Resize(object sender, EventArgs e)
        {
            SizePictures();
            Refresh();
        }
    }
}
