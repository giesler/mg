#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

#endregion

namespace msn2.net.Pictures
{
    public enum PictureCacheSize
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        FullSize = 3
    }

    public class PictureCacheInfo
    {
        public PictureCacheInfo()
        {
        }

        public string GetImageFileName(PictureData picture, int maxWidth, int maxHeight)
        {
            string cacheName = this.GetCacheName(picture.Id, maxWidth, maxHeight);
            string targetFile = PicContext.Current.Config.CacheDirectory + cacheName;

            if (File.Exists(targetFile) == false)
            {
                targetFile = null;
            }

            return targetFile;
        }

        public Image GetImage(PictureData picture, int maxWidth, int maxHeight)
        {
            string cacheName = this.GetCacheName(picture.Id, maxWidth, maxHeight);
            string sourceFile = PicContext.Current.Config.PictureDirectory + picture.Filename;
            string targetFile = PicContext.Current.Config.CacheDirectory   + cacheName;

            // Make sure target dir exists
            string targetPath = targetFile.Substring(0, targetFile.LastIndexOf(@"\"));
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            // Check if file is already cached but old
            if (File.Exists(targetFile))
            {
                if (File.GetLastWriteTime(sourceFile) > File.GetLastWriteTime(targetFile))
                {
                    File.Delete(targetFile);
                }
            }

            // Create file if needed
            if (!File.Exists(targetFile))
            {
                // load the current file
                using (Image img = Image.FromFile(sourceFile))
                {
                    // read the current height and width
                    int newWidth = img.Width;
                    int newHeight = img.Height;

                    // see if image is wider then we want, if so resize
                    if (newWidth > maxWidth)
                    {
                        newWidth = maxWidth;
                        newHeight = (int)((float)img.Height * ((float)newWidth / (float)img.Width));
                    }

                    // see if image height is greater then we want, if so, resize
                    if (newHeight > maxHeight)
                    {
                        newWidth = (int)((float)newWidth * ((float)maxHeight / (float)newHeight));
                        newHeight = maxHeight;
                    }

                    // if new width and height aren't the same as the image, resize, createing a new image, then save
                    if (newWidth != img.Width || newHeight != img.Height)
                    {
                        using (Bitmap bmp = new Bitmap(img, newWidth, newHeight))
                        {
                            bmp.Save(targetFile, ImageFormat.Jpeg);
                        }
                    }
                    else
                    {
                        // simply output the file as it is, since it is already an okay size
                        img.Save(targetFile, ImageFormat.Jpeg);
                    }
                }
            }

            // Read the targetFile and return
            Image image = Image.FromFile(targetFile);
            return image;
        }

        private string GetCacheName(int pictureId, int maxWidth, int maxHeight)
        {
            StringBuilder nameBuilder = new StringBuilder();

            // get the 10000's directory
            int temp = pictureId - (pictureId % 10000);
            nameBuilder.Append(temp.ToString("000000"));

            // get the 1000's directory
            temp = pictureId - (pictureId % 1000);
            nameBuilder.Append(@"\" + temp.ToString("000000"));

            // get the 100's directory
            temp = pictureId - (pictureId % 100);
            nameBuilder.Append(@"\" + temp.ToString("000000"));

            // Add the picture ID
            nameBuilder.Append(@"\" + pictureId.ToString("000000"));

            // Add the height and width
            nameBuilder.Append("w" + maxWidth.ToString("0000"));
            nameBuilder.Append("h" + maxHeight.ToString("0000"));

            // Always output in JPG
            nameBuilder.Append(".jpg");

            return nameBuilder.ToString();
        }

        public PictureCacheData GetPictureCacheData(PictureData picture, PictureCacheSize size)
        {
            int maxWidth = 125;
            int maxHeight = 125;

            if (size == PictureCacheSize.Medium)
            {
                maxWidth = 750;
                maxHeight = 700;
            }

            string cacheFile= GetImageFileName(picture, maxWidth, maxHeight);

            return new PictureCacheData {PictureData = picture, Filename = cacheFile};
        }
    }


    public class PictureCacheData
    {
        public PictureCacheData()
        {
        }

        public PictureData PictureData { get; set; }
        public string Filename { get; set; }
    }
}
