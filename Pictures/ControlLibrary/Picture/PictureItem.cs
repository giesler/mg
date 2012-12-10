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
        private bool sizing = false;
        private object lockObject = new object();
        private bool displayPicInfo = false;
        int loadingPictureId;
        
        enum ImageLoadSize
        {
            Small,
            Medium,
            Full
        }

        Dictionary<ImageLoadSize, Image> images = new Dictionary<ImageLoadSize, Image>();
        Dictionary<ImageLoadSize, Image> sizedImages = new Dictionary<ImageLoadSize, Image>();

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
/*        private Image image;
        private Image smallImage;
        private Image sizedImage;
  */      private bool selected;
        private bool drawShadow = true;
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
                Picture picture = this.picture;
                this.loadingPictureId = picture.Id;

                ImageLoadSize loadSize = (ImageLoadSize) o;

                this.imageReadError = false;
                Image loadedImage = null;

                if (loadSize == ImageLoadSize.Small)
                {
                        loadedImage = PicContext.Current.PictureManager.GetPictureImage(picture, 125, 125);
                        Trace.WriteLine("Loaded 125x125 for " + picture.Id.ToString());
                }
                else if (loadSize == ImageLoadSize.Medium)
                {
                    Thread.Sleep(100);
                    loadedImage = PicContext.Current.PictureManager.GetPictureImage(picture, 750, 700);
                    Trace.WriteLine("Loaded 750x700 for " + picture.Id.ToString());
                }
                else if (loadSize == ImageLoadSize.Full)
                {
                    Thread.Sleep(250);
                    string file = Path.Combine(PicContext.Current.Config.PictureDirectory, this.picture.Filename);
                    loadedImage = Image.FromFile(file);
                    Trace.WriteLine("Loaded " + file + " for " + this.picture.Id.ToString());
                }

                if (this.loadingPictureId == picture.Id)
                {
                    Image sizedImage = SizeImage(loadedImage, this.Width, this.Height);

                    if (this.loadingPictureId == picture.Id)
                    {
                        lock (this.lockObject)
                        {
                            if (this.loadingPictureId == picture.Id)
                            {
                                this.images[loadSize] = loadedImage;
                                this.sizedImages.Add(loadSize, sizedImage);
                            }
                        }

                        this.RepaintImage();
                    }
                }
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
        }

        public void SetPicture(Picture item)
        {
            ReleaseImage();

            lock (this.lockObject)
            {
                this.picture = item;
            }

            this.RepaintImage();
        }

        public void ReleaseImage()
        {
            lock (this.lockObject)
            {
                foreach (Image image in this.images.Values)
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }
                }
                this.images.Clear();

                foreach (Image image in this.sizedImages.Values)
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }
                }
                this.sizedImages.Clear();
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

            bool smallImageOnly = this.Width < 125;

            // Load image
            if (this.loadingPictureId != this.picture.Id)
            {
                lock (this.lockObject)
                {
                    if (this.loadingPictureId != this.picture.Id)
                    {
                        this.loadingPictureId = this.picture.Id;

                        this.ReleaseImage();

                        this.images.Add(ImageLoadSize.Small, null);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Small);
                        
                        if (this.Width > 125)
                        {
                            this.images.Add(ImageLoadSize.Medium, null);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Medium);

                            if (this.Width > 700)
                            {
                                this.images.Add(ImageLoadSize.Full, null);
                                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Full);
                            }
                        }
                    }
                }

                PrintLoading(e);
            }

            Image sizedImage = null;
            if (this.sizedImages.ContainsKey(ImageLoadSize.Small))
            {
                sizedImage = this.sizedImages[ImageLoadSize.Small];
            }
            if (this.sizedImages.ContainsKey(ImageLoadSize.Medium))
            {
                if (this.sizedImages[ImageLoadSize.Medium] != null)
                {
                    sizedImage = this.sizedImages[ImageLoadSize.Medium];
                }
            }
            if (this.sizedImages.ContainsKey(ImageLoadSize.Full))
            {
                if (this.sizedImages[ImageLoadSize.Full] != null)
                {
                    sizedImage = this.sizedImages[ImageLoadSize.Full];
                }
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

                lock (this.lockObject)
                {
                    e.Graphics.DrawImage(sizedImage,
                        new Rectangle(xOffset, yOffset, sizedImage.Width, sizedImage.Height),
                        0, 0, sizedImage.Width, sizedImage.Height, GraphicsUnit.Pixel, imageAtt);
                }

                AddPictureInfo(e);

                if (this.images.ContainsValue(null))
                {
                    PrintLoading(e);
                }
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

        Image SizeImage(Image image, int maxWidth, int maxHeight)
        {
            Image newSizedImage = null;

            if (image != null && image.Height > 0 && image.Width > 0)
            {
                float imageHeight = image.Height;
                float imageWidth = image.Width;

                int newX = 0;
                int newY = 0;
                int newHeight = 0;
                int newWidth = 0;

                int offset = (drawShadow ? (maxHeight > 150 ? 2 : 1) : 0);

                // Add image border
                int imageBorder = (drawBorder | selected ? (maxHeight < 75 ? 1 : 2) : 0);

                // Check if we need to fit the picture horizontally or vertically
                if (imageWidth / maxWidth > imageHeight / maxHeight)
                {
                    // Image is wider then tall
                    newWidth = maxWidth - offset - imageBorder;
                    newHeight = Convert.ToInt32(newWidth * imageHeight / imageWidth);
                    newY = (maxHeight - newHeight) / 2;
                }
                else
                {
                    newHeight = maxHeight - offset - imageBorder;
                    newWidth = Convert.ToInt32(newHeight * imageWidth / imageHeight);
                    newX = (maxWidth - newWidth) / 2;
                }

                Trace.WriteLine(string.Format("Sizing {0}x{1} to {2}x{3}", imageWidth, imageHeight, newWidth, newHeight));

                newSizedImage = new Bitmap(maxWidth, maxHeight);
                using (Graphics g = Graphics.FromImage(newSizedImage))
                {
                    int padding = (maxWidth > 75 ? 3 : 2);

                    if (this.drawBorder == false)
                    {
                        padding = 0;
                    }

                    // Draw drop shadow
                    Rectangle sizedImageRectangle = new Rectangle(newX + padding, newY + padding, newWidth - (padding * 2), newHeight - (padding * 2));

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
                    g.DrawImage(image, newX + imageBorder + padding, newY + imageBorder + padding,
                        newWidth - offset - imageBorder - (padding * 2), newHeight - offset - imageBorder - (padding * 2));
                }
            }

            return newSizedImage;
        }

        void PictureItem_Disposed(object sender, EventArgs e)
        {
            lock (this.lockObject)
            {
                this.ReleaseImage();
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
