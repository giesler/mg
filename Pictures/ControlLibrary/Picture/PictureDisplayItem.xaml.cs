using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using IO = System.IO;
using msn2.net.Pictures;
using Drawing = System.Drawing;
using WinForms = System.Windows.Forms;
using System.Threading;

namespace msn2.net.Pictures.Controls
{
    /// <summary>
    /// Interaction logic for PictureDisplayItem.xaml
    /// </summary>
    public partial class PictureDisplayItem : UserControl
    {
        public bool DrawShadow { get; set; }
        public bool DrawBorder { get; set; }
        public bool HideLoadingImage { get; set; }
        public bool CustomPaint { get; set; }
        public Picture Picture { get; private set; }

        bool selected = false;
        object lockObject = new object();
        int loadingPictureId;
        DateTime lastInvalidate = DateTime.Now;
        bool downloadingImage = false;
        
        ImageLoadSize currentImageSize = ImageLoadSize.None;
        Dictionary<ImageLoadSize, string> imagePaths = new Dictionary<ImageLoadSize,string>();

        public event EventHandler SelectedChanged;
        public event EventHandler ClearSelection;
        
        enum ImageLoadSize
        {
            Small,
            Medium,
            Full,
            None
        }

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    this.lastInvalidate = DateTime.Now;
                    this.dropEffect.Color = value ? SystemColors.HighlightColor : Colors.Black;

                    if (this.SelectedChanged != null)
                    {
                        this.SelectedChanged(this, EventArgs.Empty);
                    }
                }
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
                this.lastInvalidate = DateTime.Now;
            }
        }

        public PictureDisplayItem()
        {
            this.InitializeComponent();
        }

        private void LoadImage(object o)
        {
            try
            {
                Picture picture = this.Picture;
                if (picture != null)
                {
                    this.loadingPictureId = picture.Id;

                    ImageLoadSize loadSize = (ImageLoadSize)o;

                    Trace.WriteLine("Loading " + loadSize.ToString() + " for " + picture.Id.ToString());

                    string baseUri = "http://svc.msn2.net/picsws/getimage.axd?p={0}&mw={1}&mh={2}&sb=0";
                    string path = null;
                    if (loadSize == ImageLoadSize.Small)
                    {
                        //path = PicContext.Current.PictureCache.GetImagePath(picture, 125, 125);
                        path = string.Format(baseUri, picture.Id, 125, 125);
                    }
                    else if (loadSize == ImageLoadSize.Medium)
                    {
                        path = PicContext.Current.PictureCache.GetImagePath(picture, 750, 700);
                        path = string.Format(baseUri, picture.Id, 750, 700);
                    }
                    else if (loadSize == ImageLoadSize.Full)
                    {
                        path = "file://" + IO.Path.Combine(PicContext.Current.Config.PictureDirectory, this.Picture.Filename);
                    }

                    this.imagePaths[loadSize] = path;

                    this.UpdateImageUri();                    
                }
            }
            catch (IO.FileNotFoundException fnfe)
            {
                Trace.WriteLine(fnfe.ToString());
                this.UpdateImageUri();
            }
            catch (IO.DirectoryNotFoundException dnf)
            {
                Trace.WriteLine(dnf.ToString());
                this.UpdateImageUri();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                this.UpdateImageUri();
            }
        }

        void OnImageDownloadCompleted(object sender, EventArgs e)
        {
            if (this.Picture != null)
            {
                if (this.currentImageSize == ImageLoadSize.Small && this.imagePaths.ContainsKey(ImageLoadSize.Medium))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Medium);
                }
                if (this.currentImageSize == ImageLoadSize.Medium && this.imagePaths.ContainsKey(ImageLoadSize.Full))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Full);
                }

                if (this.currentImageSize == ImageLoadSize.Medium && this.picImageMedium.Visibility == System.Windows.Visibility.Collapsed)
                {
                    this.picImageMedium.Visibility = System.Windows.Visibility.Visible;
                    this.picImageSmall.Visibility = System.Windows.Visibility.Collapsed;
                }
                if (this.currentImageSize == ImageLoadSize.Full && this.picImageFull.Visibility == System.Windows.Visibility.Collapsed)
                {
                    this.picImageFull.Visibility = System.Windows.Visibility.Visible;
                    this.picImageMedium.Visibility = System.Windows.Visibility.Collapsed;
                }

                Trace.WriteLine("Download complete - picture " + this.Picture.Id.ToString() + ": " + this.currentImageSize.ToString());

                this.downloadingImage = false;
                this.UpdateImageUri();
                this.CheckForAdditionalLoads();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                //if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
                //{
                //    if (this.ClearSelection != null)
                //    {
                //        this.ClearSelection(this, EventArgs.Empty);
                //    }
                //}

                this.Selected = !this.Selected;
            }
        }

        void UpdateImageUri()
        {
            this.Dispatcher.BeginInvoke(new WinForms.MethodInvoker(delegate() { this.UpdateImageUriUiThread(); }));
        }

        void UpdateImageUriUiThread()
        {
            if (this.Picture != null)
            {
                string path = null;
                ImageLoadSize newSize = ImageLoadSize.None;
                Image image = null;

                if (!this.downloadingImage)
                {
                    if (this.currentImageSize == ImageLoadSize.None)
                    {
                        if (!string.IsNullOrEmpty(this.imagePaths[ImageLoadSize.Small]))
                        {
                            path = this.imagePaths[ImageLoadSize.Small];
                            newSize = ImageLoadSize.Small;
                            image = this.picImageSmall;
                        }
                    }

                    if (this.currentImageSize == ImageLoadSize.Small && this.imagePaths.ContainsKey(ImageLoadSize.Medium)
                        && !string.IsNullOrEmpty(this.imagePaths[ImageLoadSize.Medium]))
                    {
                        if (!string.IsNullOrEmpty(this.imagePaths[ImageLoadSize.Medium]))
                        {
                            path = this.imagePaths[ImageLoadSize.Medium];
                            newSize = ImageLoadSize.Medium;
                            image = this.picImageMedium;
                        }
                    }

                    if (this.currentImageSize == ImageLoadSize.Medium && this.imagePaths.ContainsKey(ImageLoadSize.Full)
                        && !string.IsNullOrEmpty(this.imagePaths[ImageLoadSize.Full]))
                    {
                        if (!string.IsNullOrEmpty(this.imagePaths[ImageLoadSize.Full]))
                        {
                            path = this.imagePaths[ImageLoadSize.Full];
                            newSize = ImageLoadSize.Full;
                            image = this.picImageFull;
                        }
                    }

                    if (!string.IsNullOrEmpty(path))
                    {
                        Trace.WriteLine("Setting image " + path);

                        Uri uri = new Uri(path, UriKind.Absolute);
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.DownloadCompleted += new EventHandler(OnImageDownloadCompleted);
                        img.DownloadFailed += new EventHandler<ExceptionEventArgs>(OnImageDownloadFailed);
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = uri;
                        img.EndInit();

                        image.Source = img;
                        this.currentImageSize = newSize;

                        if (!img.IsDownloading)
                        {
                            this.OnImageDownloadCompleted(null, null);
                        }
                    }
                }
            }
        }
        
        void OnImageDownloadFailed(object sender, ExceptionEventArgs e)
        {
            Trace.WriteLine("Failed: " + this.Picture.Id.ToString() + " - " + e.ErrorException.Message);
        }

        public void SetPicture(Picture item)
        {            
            this.ReleaseImages();

            lock (this.lockObject)
            {
                this.Picture = item;
            }

            if (item != null)
            {
                this.InvalidateVisual();
                this.QueueReloads();
            }
        }

        public void ReleaseImages()
        {
            this.imagePaths.Clear();
            this.currentImageSize = ImageLoadSize.None;
            this.picImageSmall.Visibility = System.Windows.Visibility.Visible;
            this.picImageMedium.Visibility = System.Windows.Visibility.Collapsed;
            this.picImageFull.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            this.InvalidateVisual();

            this.CheckForAdditionalLoads();
        }

        void DisplayIcon(string imagePath)
        {
            imagePath = "/msn2.net.Pictures.Controls;component/" + imagePath;
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(imagePath, UriKind.Relative);
            img.EndInit();
            // TODO: this.loadingImage.Source = img;
            this.picImageSmall.Source = null;
            this.picImageMedium.Source = null;
            this.picImageFull.Source = null;
        }

        public void QueueReloads()
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            this.CheckForAdditionalLoads();
        }

        void CheckForAdditionalLoads()
        {
            if (this.Picture != null)
            {
                lock (this.lockObject)
                {
                    if (this.imagePaths.ContainsKey(ImageLoadSize.Small) == false)
                    {
                        this.imagePaths.Add(ImageLoadSize.Small, null);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Small);
                    }

                    if (this.RenderSize.Height > 125)
                    {
                        if (this.imagePaths.ContainsKey(ImageLoadSize.Medium) == false)
                        {
                            this.imagePaths.Add(ImageLoadSize.Medium, null);
                            if (this.currentImageSize != ImageLoadSize.None)
                            {
                                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Medium);
                            }
                        }

                        if (this.RenderSize.Height > 700)
                        {
                            if (this.imagePaths.ContainsKey(ImageLoadSize.Full) == false)
                            {
                                this.imagePaths.Add(ImageLoadSize.Full, null);

                                if (this.currentImageSize != ImageLoadSize.None)
                                {
                                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), ImageLoadSize.Medium);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
