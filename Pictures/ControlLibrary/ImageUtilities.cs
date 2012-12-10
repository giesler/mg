using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for ImageUtilities.
	/// </summary>
	public class ImageUtilities
	{
		private SqlConnection sqlConnection;
        private SqlDataAdapter daPicture;
		private SqlDataAdapter daPictureCache;

		public ImageUtilities()
		{
			// Set up connection
            sqlConnection = new SqlConnection(PicContext.Current.Config.ConnectionString);

            // Data adapter for picture details
			daPicture = new SqlDataAdapter("select * from Picture where PictureID = @PictureID", sqlConnection);
			daPicture.SelectCommand.Parameters.Add("@PictureID", SqlDbType.Int);

			// Data adapter for picture cache details
			daPictureCache = new SqlDataAdapter("select * from PictureCache where PictureID = @PictureID", sqlConnection);
			daPictureCache.SelectCommand.Parameters.Add("@PictureID", SqlDbType.Int);

			// Insert command for Cache data
			string sql = "insert PictureCache (PictureID, Filename, Height, Width, MaxHeight, MaxWidth) "
							+ "values (@PictureID, @Filename, @Height, @Width, @MaxHeight, @MaxWidth); "
							+ "select * from PictureCache where PictureCacheID = @@IDENTITY";
			daPictureCache.InsertCommand = new SqlCommand(sql, sqlConnection);
			daPictureCache.InsertCommand.Parameters.Add("@PictureID", SqlDbType.Int);
			daPictureCache.InsertCommand.Parameters["@PictureID"].SourceColumn = "PictureID";
			daPictureCache.InsertCommand.Parameters.Add("@Filename", SqlDbType.NVarChar, 200);
			daPictureCache.InsertCommand.Parameters["@Filename"].SourceColumn = "Filename";
			daPictureCache.InsertCommand.Parameters.Add("@Height", SqlDbType.Int);
			daPictureCache.InsertCommand.Parameters["@Height"].SourceColumn = "Height";
			daPictureCache.InsertCommand.Parameters.Add("@Width", SqlDbType.Int);
			daPictureCache.InsertCommand.Parameters["@Width"].SourceColumn = "Width";
			daPictureCache.InsertCommand.Parameters.Add("@MaxHeight", SqlDbType.Int);
			daPictureCache.InsertCommand.Parameters["@MaxHeight"].SourceColumn = "MaxHeight";
			daPictureCache.InsertCommand.Parameters.Add("@MaxWidth", SqlDbType.Int);
			daPictureCache.InsertCommand.Parameters["@MaxWidth"].SourceColumn = "MaxWidth";
			daPictureCache.TableMappings.Add("PictureCache", "PictureCache");
			daPictureCache.TableMappings[0].ColumnMappings.Add("PictureID", "PictureID");
			daPictureCache.TableMappings[0].ColumnMappings.Add("Filename", "Filename");
			daPictureCache.TableMappings[0].ColumnMappings.Add("Height", "Height");
			daPictureCache.TableMappings[0].ColumnMappings.Add("Width", "Width");
			daPictureCache.TableMappings[0].ColumnMappings.Add("MaxHeight", "MaxHeight");
			daPictureCache.TableMappings[0].ColumnMappings.Add("MaxWidth", "MaxWidth");
			daPictureCache.TableMappings[0].ColumnMappings.Add("PictureCacheID", "PictureCacheID");

			// Insert command for Cache data
			sql = "update PictureCache set PictureID = @PictureID, Filename = @Filename, "
				+ "Height = @Height, Width = @Width, MaxHeight = @MaxHeight, MaxWidth = @MaxWidth "
				+ "where PictureCacheID = @PictureCacheID; "
				+ "select * from PictureCache where PictureCacheID = @PictureCacheID";
			daPictureCache.UpdateCommand = new SqlCommand(sql, sqlConnection);
			daPictureCache.UpdateCommand.Parameters.Add("@PictureCacheID", SqlDbType.Int);
			daPictureCache.UpdateCommand.Parameters["@PictureCacheID"].SourceColumn = "PictureCacheID";
			daPictureCache.UpdateCommand.Parameters.Add("@PictureID", SqlDbType.Int);
			daPictureCache.UpdateCommand.Parameters["@PictureID"].SourceColumn = "PictureID";
			daPictureCache.UpdateCommand.Parameters.Add("@Filename", SqlDbType.NVarChar, 200);
			daPictureCache.UpdateCommand.Parameters["@Filename"].SourceColumn = "Filename";
			daPictureCache.UpdateCommand.Parameters.Add("@Height", SqlDbType.Int);
			daPictureCache.UpdateCommand.Parameters["@Height"].SourceColumn = "Height";
			daPictureCache.UpdateCommand.Parameters.Add("@Width", SqlDbType.Int);
			daPictureCache.UpdateCommand.Parameters["@Width"].SourceColumn = "Width";
			daPictureCache.UpdateCommand.Parameters.Add("@MaxHeight", SqlDbType.Int);
			daPictureCache.UpdateCommand.Parameters["@MaxHeight"].SourceColumn = "MaxHeight";
			daPictureCache.UpdateCommand.Parameters.Add("@MaxWidth", SqlDbType.Int);
			daPictureCache.UpdateCommand.Parameters["@MaxWidth"].SourceColumn = "MaxWidth";

		}

		public void CreateUpdateCache(int pictureId) 
		{
			DataSetPicture dsPicture = new DataSetPicture();

			// Set the picture ID we want to load
			daPicture.SelectCommand.Parameters["@PictureID"].Value = pictureId;
			daPictureCache.SelectCommand.Parameters["@PictureID"].Value = pictureId;

			// Load the picture details.
			daPicture.Fill(dsPicture, "Picture");
			daPictureCache.Fill(dsPicture, "PictureCache");

			// Load the details about this specific picture.
			DataSetPicture.PictureRow picture = (DataSetPicture.PictureRow) dsPicture.Picture.Rows[0];
			
			// Update cached image for each size supported
			CreateUpdateCachedImage(ref dsPicture, picture, 125, 125);
			CreateUpdateCachedImage(ref dsPicture, picture, 750, 700);

			daPictureCache.Update(dsPicture, "PictureCache");
		}

		private void CreateUpdateCachedImage(ref DataSetPicture dsPicture, DataSetPicture.PictureRow pictureRow,
			int maxWidth, int maxHeight) {

			DataSetPicture.PictureCacheRow cacheRow = null;

			// Search for the cached image details
			foreach (DataSetPicture.PictureCacheRow tempRow in dsPicture.PictureCache.Rows) 
			{
				if (tempRow.MaxHeight == maxHeight && tempRow.MaxWidth == maxWidth) 
				{
					cacheRow = tempRow;
					break;
				}
			}

			// If the row wasn't found, add a new row
			if (cacheRow == null) 
			{
				int pictureId = pictureRow.PictureID;

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

				// Set the row details and add to DS
				cacheRow = dsPicture.PictureCache.NewPictureCacheRow();
				cacheRow.MaxWidth  = maxWidth;
				cacheRow.MaxHeight = maxHeight;
				cacheRow.PictureID = pictureRow.PictureID;
				cacheRow.Filename  = nameBuilder.ToString();
				dsPicture.PictureCache.AddPictureCacheRow(cacheRow);
			}
            
			// figure out the filenames on the web server
			string sourceFile = PicContext.Current.Config.PictureDirectory + pictureRow.Filename;
			string targetFile = PicContext.Current.Config.CacheDirectory   + cacheRow.Filename;

			// Make sure the target directory exists first
			string path = targetFile.Substring(0, targetFile.LastIndexOf(@"\"));
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			// check datestamps - if the source file has changed get rid of the cached image
			if (File.Exists(targetFile)) 
			{
				if (File.GetLastWriteTime(sourceFile) > File.GetLastWriteTime(targetFile))
					File.Delete(targetFile);
			}

			// if the targetFile doesn't exist, we need to create it
			if (!File.Exists(targetFile)) 
			{

				// load the current file
				using (Image img = Image.FromFile(sourceFile)) 
				{

					// read the current height and width
					int newWidth  = img.Width;
					int newHeight = img.Height;

					// see if image is wider then we want, if so resize
					if (newWidth > maxWidth) 
					{
						newWidth  = maxWidth;
						newHeight = (int) ( (float) img.Height * ( (float) newWidth / (float) img.Width) );
					}

					// see if image height is greater then we want, if so, resize
					if (newHeight > maxHeight)
					{
						newWidth  = (int) ( (float) newWidth * ( (float) maxHeight / (float) newHeight) );
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
					cacheRow.Width  = newWidth;

				}	// done using source image, let it be disposed automatically

			}
		}


        public static string GetDatePictureTaken(string fileName)
        {
            string dateTaken = null;

            using (StreamReader stream = new StreamReader(fileName))
            {
                BitmapSource source = BitmapFrame.Create(stream.BaseStream);
                BitmapMetadata metaData = source.Metadata as BitmapMetadata;
                if (metaData != null && metaData.DateTaken != null)
                {
                    dateTaken = metaData.DateTaken;
                }
            }

            return dateTaken;
        }
	}
}
