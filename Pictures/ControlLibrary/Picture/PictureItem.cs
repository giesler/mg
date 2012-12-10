#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class PictureItem : UserControl
    {
        private Picture picture;
        private bool loading = false;
        private bool sizing = false;
        private object lockObject = new object();
        private bool displayPicInfo = false;

        public PictureItem(): this(null)
        {
        }

        public PictureItem(Picture picture): base()
        {
            this.picture = picture;
            this.DoubleBuffered = true;
            this.PaintBackground = true;
            InitializeComponent();
            InternalConstructor();
        }

        private void InternalConstructor()
        {
            this.Resize += new EventHandler(PictureItem_Resize);
            this.Paint += new PaintEventHandler(PictureItem_Paint);
            this.Disposed += new EventHandler(PictureItem_Disposed);
        }

        public Picture Picture
        {
            get
            {
                return this.picture;
            }
        }

        public bool DisplayPictureInfo
        {
            get
            {
                return this.displayPicInfo;
            }
            set
            {
                this.displayPicInfo = value;

                Point infoLocation = new Point(10, this.Height - 40);

                Rectangle rect = new Rectangle(infoLocation, new Size(200, 30));

                this.Invalidate(rect);
            }
        }

        public int PictureId
        {
            get
            {
                if (picture != null)
                {
                    return picture.Id;
                }

                return 0;
            }
        }

        private float pictureOpacity = 1.0f;
        public float PictureOpacity
        {
            get
            {
                return pictureOpacity;
            }
            set
            {
                pictureOpacity = value;
                Refresh();
            }
        }

        public bool PaintBackground { get; set; }
        public bool PaintFullControlArea { get; set; }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (this.PaintBackground == true)
            {
                base.OnPaintBackground(pevent);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        private bool resized;
        private Image image;
        private Image smallImage;
        private Image sizedImage;
        private bool selected;
        private bool drawShadow = true;
        private Rectangle sizedImageRectangle;
        private bool drawBorder = true;

        public bool DrawBorder
        {
            get
            {
                return drawBorder;
            }
            set
            {
                drawBorder = value;
            }
        }

        private bool imageReadError = false;

        private void LoadImage(object o)
        {
            try
            {
                this.imageReadError = false;

                // See if there is a resized image
                if (this.Width < 125 && null == smallImage)
                {
                    smallImage = PicContext.Current.PictureManager.GetPictureImage(this.picture, 125, 125);
                }
                else if (null == image)
                {
                    image = PicContext.Current.PictureManager.GetPictureImage(this.picture, 750, 700);
                }

                RepaintImage();
            }
            catch (FileNotFoundException fnfe)
            {
                this.imageReadError = true;
                Trace.WriteLine(fnfe.ToString());
                RepaintImage();
            }
            catch (DirectoryNotFoundException dnf)
            {
                this.imageReadError = true;
                Trace.WriteLine(dnf.ToString());
                RepaintImage();
            }
            finally
            {
                loading = false;
            }
        }

        public void SetPicture(Picture item)
        {
            lock (this.lockObject)
            {
                ReleaseImage();

                this.picture = item;
            }

            this.RepaintImage();
        }

        public void ReleaseImage()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
            if (smallImage != null)
            {
                smallImage.Dispose();
                smallImage = null;
            }
            if (sizedImage != null)
            {
                sizedImage.Dispose();
                sizedImage = null;
            }
        }

        void RepaintImage()
        {
            this.resized = false;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(RepaintImage));
            }
            else
            {
                this.Invalidate(true);
            }
        }

        //public event EventHandler RequestPaint;

        void PictureItem_Resize(object sender, EventArgs e)
        {
            resized = true;
            Refresh();
        }

        public bool HideLoadingImage { get; set; }

        void PrintLoading(PaintEventArgs e)
        {
            if (this.HideLoadingImage == false)
            {
                int imageHeight = 16;
                if (e.ClipRectangle.Height > 500)
                {
                    imageHeight = 32;
                }

                DrawCenteredImage(e, imageHeight, CommonImages.Refresh);
            }
        }

        private void DrawCenteredImage(PaintEventArgs e, int imageHeight, Image image)
        {
            RectangleF rect = new RectangleF(0, 0, this.Width, this.Height);
            if (rect.Width > imageHeight && rect.Height > imageHeight)
            {
                rect.X = this.Width / 2 - (imageHeight / 2);
                rect.Y = this.Height / 2 - (imageHeight / 2);
                rect.Width = imageHeight;
                rect.Height = imageHeight;
                try
                {
                    e.Graphics.DrawImage(image, rect);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Exception writing Loading: " + ex.Message);
                }
            }
        }

        bool InsideParentDisplay()
        {
            if (null != this.Parent)
            {
                // Check if inside viewable area
                if (this.Parent.Height > this.Top && this.Parent.Width > this.Left)
                {
                    return true;
                }
            }

            return false;
        }

        bool customPaint = false;
        public bool CustomPaint
        {
            get
            {
                return customPaint;
            }

            set
            {
                customPaint = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!customPaint)
            {
                PictureItem_Paint(this, e);
            }
        }

        public void CustomPaintControl(PaintEventArgs e, int xOffset, int yOffset)
        {
            PaintItem(e, xOffset, yOffset);
        }

        void PictureItem_Paint(object sender, PaintEventArgs e)
        {
            PaintItem(e, 0, 0);
        }

        public void PaintPicture(PaintEventArgs e)
        {
            PaintItem(e, 0, 0);
        }

        void PaintItem(PaintEventArgs e, int xOffset, int yOffset)
        {
            if (this.DesignMode)
            {
                base.OnPaint(e);
                return;
            }

            // Make sure inside parent display
            if (null != this.Parent)
            {
                if (!InsideParentDisplay())
                {
                    return;
                }
            }

            bool useSmallImage = this.Width < 125;

            // Load image
            if (null == smallImage && useSmallImage
                || null == image && !useSmallImage)
            {
                if (loading == false)
                {
                    lock (this.lockObject)
                    {
                        if (loading == false)
                        {
                            loading = true;
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage));
                        }
                    }
                }

                Trace.WriteLine("Printing loading...");
                PrintLoading(e);

                if (null == smallImage)
                {
                    return;
                }
            }

            // Use the small image if loaded and large one not avail
            if (!useSmallImage && null != smallImage && null == image)
            {
                useSmallImage = true;
            }

            // Resize image to fit bounds
            if ((null != image || null != smallImage) && 
                (resized || null == sizedImage))
            {
                Trace.WriteLine("Printing loading 2...");
                PrintLoading(e);

                if (sizing == false)
                {
                    sizing = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SizeImage), useSmallImage);
                }

                return;
            }

            if (this.imageReadError == true)
            {
                this.DrawCenteredImage(e, 32, CommonImages.Error);
            }
            else if (null != sizedImage)
            {
                // Initialize the color matrix.
                // Note the value 0.8 in row 4, column 4.
                float[][] matrixItems ={ 
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, pictureOpacity, 0}, 
                    new float[] {0, 0, 0, 0, 1}};
                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

                // Create an ImageAttributes object and set its color matrix.
                ImageAttributes imageAtt = new ImageAttributes();
                imageAtt.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                if (selected)
                {
                    RectangleDropShadow(e.Graphics, e.ClipRectangle, SystemColors.Highlight, 1, 150);
                }
                else if (this.drawBorder == true)
                {
                    using (Brush brush = new SolidBrush(this.BackColor))
                    {
                        e.Graphics.FillRectangle(brush, e.ClipRectangle);
                    }
                }

                if (this.drawBorder == true && this.drawShadow == true)
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        using (Pen pen = new Pen(brush))
                        {
                            e.Graphics.DrawRectangle(pen, xOffset, yOffset, sizedImage.Width, sizedImage.Height);
                        }
                    }
                }

                if (this.PaintFullControlArea == true)
                {
                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        e.Graphics.FillRectangle(brush, e.ClipRectangle);
                    }
                }

                e.Graphics.DrawImage(sizedImage,
                    new Rectangle(xOffset, yOffset, sizedImage.Width, sizedImage.Height),
                    0, 0, sizedImage.Width, sizedImage.Height, GraphicsUnit.Pixel, imageAtt);


                AddPictureInfo(e);
            }
            else
            {
                PrintLoading(e);
            }
        }

        private void AddPictureInfo(PaintEventArgs e)
        {
            if (this.displayPicInfo == true && this.Picture != null)
            {
                Point infoLocation = new Point(10, this.Height - 40);

                Rectangle rect = new Rectangle(infoLocation, new Size(200, 30));
                e.Graphics.FillRectangle(Brushes.Black, rect);

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;

                string topText = string.Format("{0}", this.Picture.Title);
                Rectangle topTextRect = new Rectangle(10, this.Height - 40, rect.Width, 12);
                e.Graphics.DrawString(topText, new Font("Arial", 10, FontStyle.Bold), Brushes.White, topTextRect, format);

                string bottomText = string.Format("{0}: #{1}", this.Picture.PictureDate.ToShortDateString(), this.Picture.Id);
                Rectangle bottomTextRect = new Rectangle(10, this.Height - 25, rect.Width, 10);
                e.Graphics.DrawString(bottomText, new Font("Arial", 10, FontStyle.Regular), Brushes.White, bottomTextRect, format);
            }
        }

        private int sizedX;
        private int sizedY;

        private void SizeImage(object oUseSmallImage)
        {
            try
            {
                bool useSmallImage = bool.Parse(oUseSmallImage.ToString());
                float imageHeight = 0F;
                float imageWidth = 0F;

                lock (this.lockObject)
                {
                    if ((this.smallImage != null && useSmallImage) || this.image != null)
                    {
                        imageHeight = (useSmallImage ? smallImage.Height : image.Height);
                        imageWidth = (useSmallImage ? smallImage.Width : image.Width);
                    }
                }

                if (imageHeight > 0 && imageWidth > 0)
                {
                    int newX = 0;
                    int newY = 0;
                    int newHeight = 0;
                    int newWidth = 0;

                    int offset = (drawShadow ? (this.Height > 150 ? 2 : 1) : 0);

                    // Add image border
                    int imageBorder = (drawBorder | selected ? (this.Height < 75 ? 1 : 2) : 0);

                    // Check if we need to fit the picture horizontally or vertically
                    if (imageWidth / this.Width > imageHeight / this.Height)
                    {
                        // Image is wider then tall
                        newWidth = this.Width - offset - imageBorder;
                        newHeight = Convert.ToInt32(newWidth * imageHeight / imageWidth);
                        newY = (this.Height - newHeight) / 2;
                    }
                    else
                    {
                        newHeight = this.Height - offset - imageBorder;
                        newWidth = Convert.ToInt32(newHeight * imageWidth / imageHeight);
                        newX = (this.Width - newWidth) / 2;
                    }

                    bool drewImage = false;
                    sizedImage = new Bitmap(this.Width, this.Height);
                    using (Graphics g = Graphics.FromImage(sizedImage))
                    {
                        int padding = (this.Width > 75 ? 3 : 2);

                        if (this.drawBorder == false)
                        {
                            padding = 0;
                        }

                        // Draw drop shadow
                        sizedImageRectangle = new Rectangle(newX + padding, newY + padding, newWidth - (padding * 2), newHeight - (padding * 2));

                        if (drawShadow)
                        {
                            Rectangle dropRectangle = new Rectangle(
                                newX + padding, 
                                newY + padding, 
                                newWidth + offset + imageBorder - (padding * 2), 
                                newHeight + offset + imageBorder - (padding * 2));
                            RectangleDropShadow(g, dropRectangle, Color.DarkGray, 4, 200);
                        }

                        if (imageBorder > 0)
                        {
                            using (Brush b = new SolidBrush(Color.White))
                            {
                                g.FillRectangle(b, newX + padding, newY + padding, newWidth - (padding * 2), newHeight - (padding * 2));
                            }
                        }

                        // Draw actual image
                        lock (this.lockObject)
                        {
                            if (useSmallImage)
                            {
                                if (this.smallImage != null)
                                {
                                    g.DrawImage(smallImage, newX + imageBorder + padding, newY + imageBorder + padding,
                                        newWidth - offset - imageBorder - (padding * 2), newHeight - offset - imageBorder - (padding * 2));
                                    drewImage = true;
                                }
                            }
                            else
                            {
                                if (this.image != null)
                                {
                                    g.DrawImage(image, newX + imageBorder + padding, newY + imageBorder + padding,
                                        newWidth - offset - imageBorder - (padding * 2), newHeight - offset - imageBorder - (padding * 2));
                                    drewImage = true;
                                }
                            }
                        }
                    }

                    sizedX = newX;
                    sizedY = newY;

                    if (drewImage == true)
                    {
                        this.RepaintImage();
                    }
                    else
                    {
                        Trace.WriteLine("Image repaint from size skipped - no image");
                    }
                }
            }
            finally
            {
                sizing = false;
            }
        }

        void PictureItem_Disposed(object sender, EventArgs e)
        {
            if (null != image)
            {
                image.Dispose();
            }
            if (null != smallImage)
            {
                smallImage.Dispose();
            }
            if (null != sizedImage)
            {
                sizedImage.Dispose();
            }
        }

        public static void RectangleDropShadow(Graphics tg, Rectangle rc, Color 
            shadowColor, int depth, int maxOpacity)
        {
            //calculate the opacities
            Color darkShadow=Color.FromArgb(maxOpacity,shadowColor);
            Color lightShadow=Color.FromArgb(0,shadowColor);

            Bitmap patternbm = new Bitmap(2 * depth, 2 * depth);

            //Create a brush that will create a softshadow circle
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(0, 0, 2 * depth, 2 * depth);

                using (PathGradientBrush pgb = new PathGradientBrush(gp))
                {
                    pgb.CenterColor = darkShadow;
                    pgb.SurroundColors = new Color[] { lightShadow };

                    //generate a softshadow pattern that can be used to paint the shadow
                    using (Graphics g = Graphics.FromImage(patternbm))
                    {
                        g.FillEllipse(pgb, 0, 0, 2 * depth, 2 * depth);
                    }
                }
            }

            using (SolidBrush sb = new SolidBrush(Color.FromArgb(maxOpacity, shadowColor)))
            {
                tg.FillRectangle(sb, rc.Left + depth, rc.Top + depth, rc.Width - (2 * depth), rc.Height - (2 * depth));
            }

            //top left corner
            tg.DrawImage(patternbm,new Rectangle(rc.Left,rc.Top,depth,depth),0,0,depth,depth,GraphicsUnit.Pixel);

            //top side
            tg.DrawImage(patternbm,new Rectangle(rc.Left+depth,rc.Top,rc.Width-(2*depth),depth),
                depth,0,1,depth,GraphicsUnit.Pixel);

            //top right corner
            tg.DrawImage(patternbm, new Rectangle(rc.Right-depth,rc.Top,depth,depth),
                depth,0,depth,depth,GraphicsUnit.Pixel);

            //right side
            tg.DrawImage(patternbm,new Rectangle(rc.Right-depth,rc.Top+depth,depth,rc.Height-(2*depth)),
                depth,depth,depth,1,GraphicsUnit.Pixel);

            //bottom left corner
            tg.DrawImage(patternbm,new Rectangle(rc.Right-depth,rc.Bottom-depth,depth,depth),
                depth,depth,depth,depth,GraphicsUnit.Pixel);

            //bottom side
            tg.DrawImage(patternbm,new Rectangle(rc.Left+depth,rc.Bottom-depth,rc.Width-(2*depth),depth),
                depth,depth,1,depth,GraphicsUnit.Pixel);

            //bottom left corner
            tg.DrawImage(patternbm,new Rectangle(rc.Left,rc.Bottom-depth,depth,depth),
                        0,depth,depth,depth,GraphicsUnit.Pixel);

            //left side
            tg.DrawImage(patternbm,new Rectangle(rc.Left,rc.Top+depth,depth,rc.Height-(2*depth)),
                        0,depth,depth,1,GraphicsUnit.Pixel);

            patternbm.Dispose();

        }

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                this.Invalidate(true);
            }
        }

        public bool DrawShadow
        {
            get
            {
                return drawShadow;
            }
            set
            {
                drawShadow = value;
            }
        }

    }
}
