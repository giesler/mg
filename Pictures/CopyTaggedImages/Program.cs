using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace CopyTaggedImages
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> tags = new List<string>();
            //tags.Add("msn2.net/dome");
            //tags.Add("dome");
            tags.Add("msn2.net/Around the House");
            

            string sourcePath = @"D:\OneDrive\Pictures\Store";
            //string destPath = @"\\server0\Plex\MSN2\Dome";
            string destPath = @"\\server0\Plex\MSN2\Around The House";

            Console.WriteLine("Scanning files...");
            List<string> files = GetImageFiles(sourcePath, tags);
            int i = 0;
            
            foreach (string file in files)
            {
                i++;
                Console.WriteLine("{0}/{1} - {2}", i, files.Count, file);

                var bmpDec = BitmapDecoder.Create(new Uri(file), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                var bmpEnc = new JpegBitmapEncoder();
                bmpEnc.QualityLevel = 100;
                bmpEnc.Frames.Add(bmpDec.Frames[0]);

                using (MemoryStream ms = new MemoryStream())
                {
                    bmpEnc.Save(ms);

                    using (Image source = Image.FromStream(ms))
                    {
                        using (Image dest = SizeImage(source, 1920, 1200))
                        {
                            string newFile = file.ToLower().Replace(sourcePath.ToLower(), destPath.ToLower());
                            newFile = Path.ChangeExtension(newFile, "jpg");
                            newFile = newFile.Replace(@"200x\", "").Replace(@"201x\", "");

                            string dir = Path.GetDirectoryName(newFile);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            Console.WriteLine("{0}/{1} - {2}", i, files.Count, newFile);
                            dest.Save(newFile, ImageFormat.Jpeg);
                        }
                    }
                }
            }

        }

        static List<string> GetImageFiles(string path, List<string> tags)
        {
            List<string> files = new List<string>();

            //files.AddRange(Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(path, "*.cr2", SearchOption.AllDirectories));

            List<string> tagged = new List<string>();

            foreach (string file in files)
            {
                var data = MetadataExtractor.ImageMetadataReader.ReadMetadata(file);

                foreach (var item in data)
                {
                    Console.WriteLine(item.Name);
                }

                string xmpFile = Path.ChangeExtension(file, "xmp");
                if (File.Exists(xmpFile))
                {
                    string xmp = File.ReadAllText(xmpFile);
                    foreach (string tag in tags)
                    {
                        if (xmp.ToLower().IndexOf(@"<rdf:li>" + tag.ToLower() + "</rdf:li>") > 0)
                        {
                            tagged.Add(file);
                        }
                    }
                }
                else if (file.ToLower().EndsWith("jpg"))
                {
                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BitmapSource img = BitmapFrame.Create(fs);
                    BitmapMetadata md = (BitmapMetadata)img.Metadata;

                    try
                    {
                        if (md.Keywords != null)
                        {
                            foreach (string tag in md.Keywords)
                            {
                                foreach (string match in tags)
                                {
                                    if (string.Equals(tag.Trim(), match.Trim(), StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        tagged.Add(file);
                                    }
                                }
                            }
                        }
                    }
                    catch (NotSupportedException)
                    { }
                }

            }


            return tagged;
        }

        static void GetImageFiles(List<string> files, string path)
        {
            foreach (string subDir in Directory.GetDirectories(path))
            {
                GetImageFiles(files, subDir);
            }

            files.AddRange(Directory.GetFiles(path, " *.cr2|*.jpg"));
        }

        public static Image SizeImage(Image image, int maxWidth, int maxHeight)
        {
            Image newSizedImage = null;

            if (image != null && image.Height > 0 && image.Width > 0)
            {
                float imageHeight = image.Height;
                float imageWidth = image.Width;

                int newHeight = 0;
                int newWidth = 0;

                if (imageWidth < maxWidth && imageHeight < maxHeight)
                {
                    return image;
                }

                // Check if we need to fit the picture horizontally or vertically
                if (imageWidth / maxWidth > imageHeight / maxHeight)
                {
                    // Image is wider then tall
                    newWidth = maxWidth;
                    newHeight = Convert.ToInt32(newWidth * imageHeight / imageWidth);
                }
                else
                {
                    newHeight = maxHeight;
                    newWidth = Convert.ToInt32(newHeight * imageWidth / imageHeight);
                }

                Console.WriteLine(string.Format("Sizing {0}x{1} to {2}x{3}", imageWidth, imageHeight, newWidth, newHeight));

                newSizedImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(newSizedImage))
                {
                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                }
            }

            return newSizedImage;
        }
    }
}
