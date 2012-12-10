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
using System.Windows.Media.Imaging;
using System.Collections.Generic;

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
        
        public static event StatusUpdateEventHandler StatusUpdate;
        public delegate void StatusUpdateEventHandler(string message, int current, int max);
        static int count = 0;
        static int totalCount = 0;

        public static string[] GetTags(string filename)
        {
            // open a filestream for the file we wish to look at
            using (Stream fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                // create a decoder to parse the file
                BitmapDecoder decoder = BitmapDecoder.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.Default);
                // grab the bitmap frame, which contains the metadata
                BitmapFrame frame = decoder.Frames[0];
                // get the metadata as BitmapMetadata
                BitmapMetadata metadata = frame.Metadata as BitmapMetadata;
                // close the stream before returning
                fs.Close();
                // System.Keywords is the same as using the above method.  this particular metadata property
                // will return an array of strings, though we still have to cast it as such
                string[] tags = null;

                if (metadata.Format.ToLower() == "jpg")
                {
                    tags = metadata.GetQuery("System.Keywords") as string[];
                }
                return tags;
            }
        }

        public static void AddTags(string filename, string[] tags)
        {
            SetTags(filename, tags, false);
        }

        public static void SetTags(string filename, string[] tags)
        {
            SetTags(filename, tags, true);
        }

        private static void SetTags(string filename, string[] tags, bool ignoreCurrent)
        {
            bool writeFailure = false;
            string[] keys = null;
            bool invalidFormat = false;

            if ((File.GetAttributes(filename) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(filename, FileAttributes.Normal);
            }

            // open a filestream for the file we wish to look at
            using (Stream fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                // create a decoder to parse the file
                BitmapDecoder decoder = BitmapDecoder.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.Default);
                // grab the bitmap frame, which contains the metadata
                BitmapFrame frame = decoder.Frames[0];
                // get the metadata as BitmapMetadata
                BitmapMetadata metadata = frame.Metadata as BitmapMetadata;
                // instantiate InPlaceBitmapMetadataWriter to write the metadata to the file
                InPlaceBitmapMetadataWriter writer = frame.CreateInPlaceBitmapMetadataWriter();
                if (writer.Format.ToLower() != "jpg")
                {
                    invalidFormat = true;
                    Trace.WriteLine("Invalid format (" + writer.Format + "): " + filename);
                }
                else
                {
                    if (ignoreCurrent == false && metadata.Keywords != null)      // tags exist - include them when saving
                    {
                        // build the complete list of tags - new and old
                        List<string> list = new List<string>();
                        foreach (string keyword in metadata.Keywords)
                        {
                            if (keyword.Trim().Length > 0)
                            {
                                list.Add(CleanTag(keyword));
                            }
                        }

                        foreach (string tag in tags)
                        {
                            bool found = false;
                            foreach (string s in metadata.Keywords)
                            {
                                if (s.ToLower() == tag.ToLower())
                                {
                                    found = true;
                                }
                            }

                            if (found == false)
                            {
                                list.Add(CleanTag(tag));
                            }
                        }

                        keys = new string[list.Count];
                        list.CopyTo(keys);

                        // associate the tags with the writer
                        // the type of variable to pass (here, an array of strings) depends on
                        // which metadata property you are using.  Since we are modifying the
                        // Keywords property, we use the array.  If you use the author property,
                        // it will simply be a string.
                        writer.SetQuery("System.Keywords", keys);
                    }
                    else     // no old tags - just use the new ones
                    {
                        keys = tags;
                        // associate the tags with the writer
                        // the type of variable to pass (here, an array of strings) depends on
                        // which metadata property you are using.  Since we are modifying the
                        // Keywords property, we use the array.  If you use the author property,
                        // it will simply be a string.
                        writer.SetQuery("System.Keywords", tags);
                    }

                    // try to save the metadata to the file
                    if (!writer.TrySave())
                    {
                        writeFailure = true;
                    }
                }
            }

            if (writeFailure && !invalidFormat)
            {
                // if it fails, there is no room for the metadata to be written to.
                // we must add room to the file using SetUpMetadataOnImage (defined below)
                //this.Invoke(new CloneImageDelegate(this.SetUpMetadataOnImage), filename, keys);
                CloneImage(filename, keys);
            }
        }

        static string CleanTag(string tag)
        {
            if (tag.StartsWith(";") || tag.StartsWith(@"\") || tag.StartsWith("/"))
            {
                tag = tag.Substring(1);
            }

            if (tag.EndsWith(";") || tag.EndsWith(@"\") || tag.EndsWith("/"))
            {
                tag = tag.Substring(tag.Length - 1);
            }

            return tag;
        }

        static void CloneImage(string filename, string[] tags)
        {
            // padding amount, using 2Kb.  don't need much here; metadata is rather small
            uint paddingAmount = 2048;

            // open image file to read
            using (Stream file = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                // create the decoder for the original file.  The BitmapCreateOptions and BitmapCacheOption denote
                // a lossless transocde.  We want to preserve the pixels and cache it on load.  Otherwise, we will lose
                // quality or even not have the file ready when we save, resulting in 0b of data written
                BitmapDecoder original = BitmapDecoder.Create(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                // create an encoder for the output file
                JpegBitmapEncoder output = new JpegBitmapEncoder();

                // add padding and tags to the file, as well as clone the data to another object
                if (original.Frames[0] != null && original.Frames[0].Metadata != null)
                {
                    // Because the file is in use, the BitmapMetadata object is frozen.
                    // So, we clone the object and add in the padding.
                    BitmapFrame frameCopy = (BitmapFrame)original.Frames[0].Clone();
                    BitmapMetadata metadata = original.Frames[0].Metadata.Clone() as BitmapMetadata;

                    // we use the same method described in AddTags() as saving tags to save an amount of padding
                    metadata.SetQuery("/app1/ifd/PaddingSchema:Padding", paddingAmount);
                    metadata.SetQuery("/app1/ifd/exif/PaddingSchema:Padding", paddingAmount);
                    metadata.SetQuery("/xmp/PaddingSchema:Padding", paddingAmount);
                    // we add the tags we want as well.  Again, using the same method described above
                    metadata.SetQuery("System.Keywords", tags);


                    // finally, we create a new frame that has all of this new metadata, along with the data that was in the original message
                    output.Frames.Add(BitmapFrame.Create(frameCopy, frameCopy.Thumbnail, metadata, original.Frames[0].ColorContexts));
                    //original.Frames[0].ColorContexts));
                    file.Close();  // close the file to ready for overwrite
                }
                // finally, save the new file over the old file
                string tempFile = filename + ".tmp";
                using (FileStream outputFile = File.Open(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    output.Save(outputFile);
                    outputFile.Close();
                }

                File.Delete(filename);
                File.Move(tempFile, filename);
            }
        }
	}
}
