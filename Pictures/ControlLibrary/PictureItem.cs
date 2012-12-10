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

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class PictureItem : UserControl
    {
        public PictureItem(int pictureId)
        {
            InitializeComponent();
            this.Resize += new EventHandler(PictureItem_Resize);
            this.Paint  += new PaintEventHandler(PictureItem_Paint);
            this.Disposed += new EventHandler(PictureItem_Disposed);

            this.DoubleBuffered = true;
            this.pictureId = pictureId;
        }

        private int pictureId;
        public int PictureId
        {
            get
            {
                return pictureId;
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

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // no background
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

        private void LoadImage(object o)
        {
            try
            {
                // See if there is a resized image
                if (this.Width < 125 && null == smallImage)
                {
                    smallImage = PicContext.Current.PictureManager.GetPictureImage(pictureId, 125, 125);
                }
                else if (null == image)
                {
                    image = PicContext.Current.PictureManager.GetPictureImage(pictureId, 750, 700);
                }

                RepaintImage();
            }
            catch (System.IO.FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.ToString());
                return;
            }
        }

        void RepaintImage()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(RepaintImage));
                return;
            }

            if (this.CustomPaint)
            {
                if (null != RequestPaint)
                {
                    RequestPaint(this, EventArgs.Empty);
                }
            }
            else
            {
                this.Refresh();
            }
        }

        public event EventHandler RequestPaint;

        void PictureItem_Resize(object sender, EventArgs e)
        {
            resized = true;
            Refresh();
        }

        void PrintLoading(Graphics g)
        {
//            using (Brush brush = Brushes.Black)
//            {
//                StringFormat format = new StringFormat();
//                format.Alignment = StringAlignment.Center;
//                format.LineAlignment = StringAlignment.Center;
//                Font font = new Font("Arial", 6);
//                RectangleF rect = new RectangleF(0, 0, this.Width, this.Height);
//                try
//                {
//                    
//                    g.DrawString("Loading...", font, brush, this.Top, this.Left, format);
//                }
//                catch (Exception ex)
//                {
//                    Trace.WriteLine("Exception writing Loading... for pic " + pictureId.ToString() + ": " + ex.Message);
//                }
//            }
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
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage));
                PrintLoading(e.Graphics);

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
                SizeImage(useSmallImage);
            }

            if (null != sizedImage)
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
                else
                {
                    using (Brush brush = new SolidBrush(this.BackColor))
                    {
                        e.Graphics.FillRectangle(brush, e.ClipRectangle);
                    }
                }

                e.Graphics.DrawImage(sizedImage,
                    new Rectangle(xOffset, yOffset, sizedImage.Width, sizedImage.Height),
                    0, 0, sizedImage.Width, sizedImage.Height, GraphicsUnit.Pixel, imageAtt);

            }
            else
            {
                PrintLoading(e.Graphics);
            }
        }

        private int sizedX;
        private int sizedY;

        private void SizeImage(bool useSamllImage)
        {
            float imageHeight = (useSamllImage ? smallImage.Height : image.Height);
            float imageWidth = (useSamllImage ? smallImage.Width : image.Width);

            int newX = 0;
            int newY = 0;
            int newHeight = 0;
            int newWidth = 0;

            int offset = (drawShadow ? (this.Height > 150 ? 4 : 2) : 0);

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

            sizedImage = new Bitmap(this.Width, this.Height);
            using (Graphics g = Graphics.FromImage(sizedImage))
            {
                int padding = (this.Width > 75 ? 3 : 2);

                // Draw drop shadow
                sizedImageRectangle = new Rectangle(newX + padding, newY + padding, newWidth - (padding * 2), newHeight - (padding * 2));

                if (drawShadow)
                {
                    Rectangle dropRectangle = new Rectangle(newX + padding, newY + padding, newWidth + offset + imageBorder - (padding * 2), newHeight + offset + imageBorder - (padding * 2));
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
                if (useSamllImage)
                {
                    g.DrawImage(smallImage, newX + imageBorder + padding, newY + imageBorder + padding,
                        newWidth - offset - imageBorder - (padding * 2), newHeight - offset - imageBorder - (padding * 2));
                }
                else
                {
                    g.DrawImage(image, newX + imageBorder + padding, newY + imageBorder + padding,
                        newWidth - offset - imageBorder - (padding * 2), newHeight - offset - imageBorder - (padding * 2));
                }
            }

            sizedX = newX;
            sizedY = newY;
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

        public void RectangleDropShadow(Graphics tg, Rectangle rc, Color 
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
                Refresh();
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
