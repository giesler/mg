using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Data.Linq;
using System.Linq;
using System.Diagnostics;

namespace msn2.net.Pictures
{
	/// <summary>
	/// Summary description for ImageUtilities.
	/// </summary>
	public class ImageUtilities
	{
		
		public ImageUtilities()
		{
		}

		public void CreateUpdateCache(int pictureId)
        {
            PicContext pc = PicContext.Current.Clone();
            
            Picture picture = pc.PictureManager.GetPicture(pictureId);

			CreateUpdateCachedImage(pc, picture, 125, 125);
			CreateUpdateCachedImage(pc, picture, 750, 700);
		}

        private void CreateUpdateCachedImage(PicContext pc, Picture picture, int maxWidth, int maxHeight)
        {
            PictureCache cacheRow = (from pca in picture.PictureCaches
                                     where pca.Picture == picture && pca.MaxWidth == maxWidth && pca.MaxHeight == maxHeight
                                     select pca).FirstOrDefault();

            if (cacheRow == null)
            {
                cacheRow = new PictureCache { Picture = picture, MaxWidth = maxWidth, MaxHeight = maxHeight };

                StringBuilder nameBuilder = new StringBuilder();

                // get the 10000's directory
                int temp = picture.Id - (picture.Id % 10000);
                nameBuilder.Append(temp.ToString("000000"));

                // get the 1000's directory
                temp = picture.Id - (picture.Id % 1000);
                nameBuilder.Append(@"\" + temp.ToString("000000"));

                // get the 100's directory
                temp = picture.Id - (picture.Id % 100);
                nameBuilder.Append(@"\" + temp.ToString("000000"));

                // Add the picture ID
                nameBuilder.Append(@"\" + picture.Id.ToString("000000"));

                // Add the height and width
                nameBuilder.Append("w" + maxWidth.ToString("0000"));
                nameBuilder.Append("h" + maxHeight.ToString("0000"));

                // Always output in JPG
                nameBuilder.Append(".jpg");

                // Set the row details and add to DS
                cacheRow.Filename = nameBuilder.ToString();
                cacheRow.Height = 0;
                cacheRow.Width = 0;

                pc.PictureManager.AddPictureCache(picture, cacheRow);
            }

            // figure out the filenames on the web server
            string sourceFile = pc.Config.PictureDirectory + picture.Filename;
            string targetFile = pc.Config.CacheDirectory + cacheRow.Filename;

            // Make sure the target directory exists first
            string path = targetFile.Substring(0, targetFile.LastIndexOf(@"\"));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // check datestamps - if the source file has changed get rid of the cached image
            if (File.Exists(targetFile))
            {
                if (File.GetLastWriteTime(sourceFile) > File.GetLastWriteTime(targetFile))
                {
                    try
                    {
                        File.Delete(targetFile);
                    }
                    catch (Exception ex)
                    {
                        // leave old image
                        Trace.WriteLine(string.Format("Deleting {0}: {1}", targetFile, ex));
                    }
                }
            }

            // if the targetFile doesn't exist, we need to create it
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

                    // since we know these values, might as well set them
                    cacheRow.Height = newHeight;
                    cacheRow.Width = newWidth;
                    pc.SubmitChanges();

                }	// done using source image, let it be disposed automatically

            }
        }
	}
}
