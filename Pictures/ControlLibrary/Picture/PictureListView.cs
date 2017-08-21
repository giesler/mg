using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace msn2.net.Pictures.Controls
{
    public class PictureListView: ListView
    {
        List<Picture> pictures = null;
        ImageList images = new ImageList();
        bool loadPending = false;
        System.Windows.Forms.Timer renderTimer;
        List<PictureListViewItem> renderList = new List<PictureListViewItem>();
        object lockObject = new object();

        public PictureListView()
        {
            base.View = View.SmallIcon;
            this.OwnerDraw = true;
            this.DoubleBuffered = true;
            base.AutoArrange = false;

            this.renderTimer = new System.Windows.Forms.Timer();
            this.renderTimer.Interval = 500;
            this.renderTimer.Tick += new EventHandler(renderTimer_Tick);
            this.renderTimer.Enabled = true;
        }

        void renderTimer_Tick(object sender, EventArgs e)
        {
            if (this.renderList.Count > 0)
            {
                this.SuspendLayout();
                lock (this.lockObject)
                {
                    foreach (PictureListViewItem item in this.renderList)
                    {
                        item.ImageIndex = 0;
                    }
                    this.renderList.Clear();
                }
                this.ResumeLayout();
            }
        }

        //public const int LVS_OWNERDRAWFIXED = 0x0400;

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.Style |= LVS_OWNERDRAWFIXED;
        //        return cp;
        //    }
        //}

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                //case (int)ReflectedMessages.OCM_DRAWITEM:
                //    {
                //        DrawItemStruct dis =
                //        (DrawItemStruct)m.GetLParam(typeof(DrawItemStruct));

                //        Graphics graph = Graphics.FromHdc(dis.hDC);
                //        Rectangle rect = new Rectangle(dis.rcItem.left, dis.rcItem.top, dis.rcItem.right - dis.rcItem.left, dis.rcItem.bottom - dis.rcItem.top);
                //        int index = dis.itemID;
                //        DrawItemState state = DrawItemState.None;

                //        System.Windows.Forms.DrawItemEventArgs e = new System.Windows.Forms.DrawItemEventArgs(graph, Font, rect, index, state, ForeColor, BackColor);
                //        if (this.DrawItem != null)
                //        {
                //            this.DrawItem(this, e);
                //        }
                //        OnDrawItem(e);

                //        graph.Dispose();
                //        break;
                //    }

                case 8236:
                    this.WmReflectMeasureItem(ref m);
                    break;

            }
        }

        private struct MEASUREITEMSTRUCT
        {
            public int CtlType;
            public int CtlID;
            public int itemID;
            public int itemWidth;
            public int itemHeight;
            public IntPtr itemData;
        }

        private void WmReflectMeasureItem(ref Message m)
        {
            MeasureItemEventArgs args1;

            MEASUREITEMSTRUCT measureitemstruct1 = (MEASUREITEMSTRUCT)m.GetLParam(typeof(MEASUREITEMSTRUCT));
            if (this.OwnerDraw && measureitemstruct1.itemID >= 0)
            {
                measureitemstruct1.itemHeight = 100;
                measureitemstruct1.itemWidth = 100;
            }

            Marshal.StructureToPtr(measureitemstruct1, m.LParam, false);
            m.Result = ((IntPtr)1);
        }

        public void LoadPictures(List<Picture> pictures)
        {
            this.pictures = pictures;

            foreach (Image image in this.images.Images)
            {
                image.Dispose();
            }
            
            this.Items.Clear();
            this.SuspendLayout();
            foreach (Picture picture in pictures)
            {
                this.Items.Add(new PictureListViewItem(picture));
            }

            if (this.Items.Count > 0)
            {
                this.Items[0].EnsureVisible();
            }
            this.ResumeLayout();

            this.images = new ImageList();
            this.images.ImageSize = new Size(100, 100);
            this.images.Images.Add(CommonImages.Refresh);
            this.SmallImageList = this.images;
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            base.OnDrawItem(e);

            PictureListViewItem item = (PictureListViewItem)e.Item;
            if (item.Loading == false && item.Loaded == false)
            {
                item.Loading = true;
                ThreadPool.QueueUserWorkItem(this.LoadPicture, item);
            }

            if (e.Bounds.Height > 100 && e.Bounds.Width > 100 && e.Bounds.X >= 0 && e.Bounds.Y >= 0
            && e.Bounds.Width == 134 && e.Bounds.X % 10 == 0)
            {
                Trace.WriteLine("Picture " + item.Picture.Id + ": " + e.Bounds.ToString());

                e.DrawBackground();

                if (item.Loaded)
                {
                    DrawPicture(e, item);
                }
                else
                {
                    if (e.Bounds.Width > 16 && e.Bounds.Height > 16)
                    {
                        Rectangle rect = new Rectangle(e.Bounds.Width / 2 - 8, e.Bounds.Height / 2 - 8, 16, 16);
                        e.Graphics.DrawImage(CommonImages.Refresh, rect);
                    }
                }

                if (item.Focused)
                {
                    e.DrawFocusRectangle();
                }
            }
        }

        private static void DrawPicture(DrawListViewItemEventArgs e, PictureListViewItem item)
        {
            // Initialize the color matrix.
            // Note the value 0.8 in row 4, column 4.
            float opacity = item.Selected ? 0.8F : 1.0F;
            float[][] matrixItems ={ 
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, opacity, 0}, 
                    new float[] {0, 0, 0, 0, 1}};
            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            // Create an ImageAttributes object and set its color matrix.
            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                using (Pen pen = new Pen(brush))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, item.SizedImage.Width, item.SizedImage.Height);
                }
            }

            e.Graphics.DrawImage(item.Image,
                new Rectangle(0, 0, item.SizedImage.Width, item.SizedImage.Height),
                0, 0, item.SizedImage.Width, item.SizedImage.Height, GraphicsUnit.Pixel, imageAtt);
            
            e.Graphics.DrawImage(item.Image, e.Bounds);
        }

        void LoadPicture(object sender)
        {
            PictureListViewItem item = (PictureListViewItem)sender;

            item.Image = PicContext.Current.PictureManager.GetPictureImage(item.Picture, 125, 125);

            int maxWidth = 100;
            int maxHeight = 100;

            float imageHeight = 0F;
            float imageWidth = 0F;
            int newX = 0;
            int newY = 0;
            int newHeight = 0;
            int newWidth = 0;

            int offset = this.Height > 150 ? 2 : 1;

            // Add image border
            int imageBorder = this.Height < 75 ? 1 : 2;

            // Check if we need to fit the picture horizontally or vertically
            if (item.Image.Width / maxWidth > item.Image.Height / maxHeight)
            {
                // Image is wider then tall
                newWidth = maxWidth - offset - imageBorder;
                newHeight = Convert.ToInt32(newWidth * item.Image.Height / item.Image.Width);
                newY = (maxHeight - newHeight) / 2;
            }
            else
            {
                newHeight = maxHeight - offset - imageBorder;
                newWidth = Convert.ToInt32(newHeight * item.Image.Width / item.Image.Height);
                newX = (maxWidth - newWidth) / 2;
            }

            bool drewImage = false;
            item.SizedImage = new Bitmap(maxWidth, maxHeight);
            using (Graphics g = Graphics.FromImage(item.SizedImage))
            {
                int padding = maxWidth > 75 ? 3 : 2;

                // Draw drop shadow
                Rectangle sizedImageRectangle = new Rectangle(newX + padding, newY + padding,
                    newWidth - (padding * 2), newHeight - (padding * 2));
                Rectangle dropRectangle = new Rectangle(newX + padding, newY + padding,
                        newWidth + offset + imageBorder - (padding * 2),
                        newHeight + offset + imageBorder - (padding * 2));
                PictureItem.RectangleDropShadow(g, dropRectangle, Color.DarkGray, 4, 200);

                if (imageBorder > 0)
                {
                    using (Brush b = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(b, newX + padding, newY + padding, newWidth - (padding * 2), newHeight - (padding * 2));
                    }
                }

                // Draw actual image
                g.DrawImage(item.Image, newX + imageBorder + padding, newY + imageBorder + padding,
                    newWidth - offset - imageBorder - (padding * 2), newHeight - offset - imageBorder - (padding * 2));
            }

            item.Loaded = true;
            this.renderList.Add(item);
            //this.BeginInvoke(new WaitCallback(this.RenderImage), sender);
        }

        //void RenderImage(object sender)
        //{
        //    PictureListViewItem item = (PictureListViewItem)sender;

        //    this.images.Images.Add(item.Image);

        //    item.ImageIndex = this.images.Images.Count - 1;
        //}
    }

    public class PictureListViewItem : ListViewItem
    {
        public Picture Picture { get; private set; }
        public Image Image { get; set; }
        public Image SizedImage { get; set; }
        internal bool Loading { get; set; }
        internal bool Loaded { get; set; }
        internal bool FirstPaint { get; set; }

        public PictureListViewItem(Picture picture)
        {
            this.Picture = picture;
            base.ImageIndex = 0;
            this.Text = this.Picture.Id.ToString();
        }
    }
}
