using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Net;
using System.Diagnostics;
using msn2.net.Pictures;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for SaveSlideshow.
	/// </summary>
	public class SaveSlideshow : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBoxSaveLocation;
		private System.Windows.Forms.Button save;
		private System.Windows.Forms.Button cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private PicContext picContext;

		public SaveSlideshow(PicContext picContext, int categoryId)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.picContext = picContext;
			this.categoryId	= categoryId;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxSaveLocation = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.save = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(368, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Where do you want to save the slideshow?";
			// 
			// textBoxSaveLocation
			// 
			this.textBoxSaveLocation.Location = new System.Drawing.Point(8, 32);
			this.textBoxSaveLocation.Name = "textBoxSaveLocation";
			this.textBoxSaveLocation.Size = new System.Drawing.Size(264, 20);
			this.textBoxSaveLocation.TabIndex = 1;
			this.textBoxSaveLocation.Text = "d:\\ss";
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(288, 32);
			this.button1.Name = "button1";
			this.button1.TabIndex = 2;
			this.button1.Text = "&Browse";
			// 
			// save
			// 
			this.save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.save.Location = new System.Drawing.Point(200, 80);
			this.save.Name = "save";
			this.save.TabIndex = 3;
			this.save.Text = "&Save";
			this.save.Click += new System.EventHandler(this.save_Click);
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancel.Location = new System.Drawing.Point(288, 80);
			this.cancel.Name = "cancel";
			this.cancel.TabIndex = 4;
			this.cancel.Text = "&Cancel";
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// SaveSlideshow
			// 
			this.AcceptButton = this.save;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(376, 110);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cancel,
																		  this.save,
																		  this.button1,
																		  this.textBoxSaveLocation,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SaveSlideshow";
			this.Opacity = 0.96;
			this.Text = "Save Slideshow";
			this.ResumeLayout(false);

		}
		#endregion

		private void cancel_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
		}

		private void save_Click(object sender, System.EventArgs e)
		{
			saveStatus = new fStatus("Saving slideshow...");
			saveStatus.Show();

			Thread t = new Thread(new ThreadStart(SaveSlideshowToDisk));
			t.Start();
		}

		private int categoryId;

		private fStatus saveStatus;

		private void SaveSlideshowToDisk()
		{
			try
			{
				string folder				= textBoxSaveLocation.Text;

				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
				if (!Directory.Exists(folder + @"\pics"))
				{
					Directory.CreateDirectory(folder + @"\pics");
				}

				// Create objects
				PictureDataSet ds			= new PictureDataSet();
				PictureManager pm			= picContext.PictureManager;

				// Figure out how many pictures we have
				ds							= pm.GetPicturesByCategory(categoryId);

				saveStatus.StatusText		= "Creating slideshow...";
				saveStatus.Max				= ds.Picture.Rows.Count;

				// Loop through items
				int recNumber				= 0;
				foreach (PictureDataSet.PictureRow picture in ds.Picture.Rows)
				{
					try
					{
						recNumber++;
						saveStatus.Current++;

						// Figure out filename
						string filename			= folder + Path.DirectorySeparatorChar + "pic" + String.Format("{0:00}", recNumber) + ".html";
						string imageFilename	= folder + Path.DirectorySeparatorChar + @"pics\" + String.Format("{0:00}", recNumber) + ".jpg";

						if (File.Exists(filename))
						{
							File.Delete(filename);
						}

						if (File.Exists(imageFilename))
						{
							File.Delete(imageFilename);
						}

						// Build request for page
						string url				= String.Format(@"http://pics.msn2.net/picviewss.aspx?r={0}&c={1}&type=category", recNumber, categoryId);
//						MessageBox.Show("request url = " + url);
						StringBuilder page		= new StringBuilder(GetUrl(url));

						// Replace image source URL
						int startPos			= page.ToString().IndexOf(@"/GetImage.axd");
//						MessageBox.Show("image source url startpos: " + startPos.ToString());
						int endPos				= page.ToString().IndexOf("\"", startPos);
						page.Remove(startPos, endPos - startPos);
						page.Insert(startPos, String.Format(@"pics\{0:00}.jpg", recNumber));

						// Check for next link
						startPos				= page.ToString().IndexOf("lnkNext");
//						MessageBox.Show("new link startPos = " + startPos.ToString());
						if (startPos > 0)
						{
							startPos			= page.ToString().IndexOf(@"/picviewss.aspx", startPos);
							endPos				= page.ToString().IndexOf("\"", startPos);
							page.Remove(startPos, endPos - startPos);
							page.Insert(startPos, String.Format("pic{0:00}.html", recNumber+1));
						}

						// Check for prev link
						startPos				= page.ToString().IndexOf("lnkPrevious");
//						MessageBox.Show("prev link startPos = " + startPos.ToString());
						if (startPos > 0)
						{
							startPos			= page.ToString().IndexOf(@"/picviewss.aspx", startPos);
							endPos				= page.ToString().IndexOf("\"", startPos);
							page.Remove(startPos, endPos - startPos);
							page.Insert(startPos, String.Format("pic{0:00}.html", recNumber-1));
						}
						//src="/pics/GetImage.axd?p=426&amp;mw=750&amp;mh=700"

						// Remove the close link
						startPos				= page.ToString().IndexOf("<a id=\"lnkReturn\"");
//						MessageBox.Show("return link startPos = " + startPos.ToString());
						if (startPos > 0)
						{
							endPos				= page.ToString().IndexOf("</a>", startPos) + 4;
							page.Remove(startPos, endPos - startPos);

							// Move the picture position to this position
							int picStartIndex	= page.ToString().IndexOf("<span id=\"lblPicture\">");
							int picEndIndex		= page.ToString().IndexOf("</span>", picStartIndex + 35) + 7;

							string temp			= page.ToString().Substring(picStartIndex, picEndIndex - picStartIndex);
							page.Insert(startPos,  temp);

							//					page.Remove(picStartIndex, picEndIndex - picStartIndex);
						}

						// Remove the first row of info panel
						startPos				= page.ToString().IndexOf("<div class=\"infoPanel\">");
//						MessageBox.Show("info panel first row startPos = " + startPos.ToString());
						startPos				= page.ToString().IndexOf("<tr>", startPos);
						if (startPos > 0)
						{
							endPos				= page.ToString().IndexOf("</table>", startPos);
							endPos				= page.ToString().IndexOf("</tr>", endPos) + 5;
							page.Remove(startPos, endPos - startPos);
						}

						StreamWriter writer		= File.CreateText(filename);
						writer.Write(page);
						writer.Close();

						// Request image
						string imgSource		= GetPictureFilename(picture.PictureID, 750, 700);
						File.Copy(@"\\ike\" + imgSource, imageFilename);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error processing image id #" + picture.PictureID.ToString() + ": " + ex.Message + "\n" + ex.StackTrace, "Error adding slideshow image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				
				}		
			
				// Get stylesheet
				if (File.Exists(folder + @"\msn2.css"))
				{
					File.Delete(folder + @"\msn2.css");
				}
				string url1		= String.Format(@"http://pics.msn2.net/msn2.css");
				StringBuilder pg		= new StringBuilder(GetUrl(url1));
				StreamWriter css = File.CreateText(folder + @"\msn2.css");
				css.Write(pg.ToString());
                css.Close();

				saveStatus.Close();
				saveStatus.Dispose();
				saveStatus					= null;
		
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + " " + ex.StackTrace);
			}
		}

		private string GetUrl(string url)
		{
			StringBuilder sb			= new StringBuilder();
			WebResponse response		= null;
			try
			{
				WebRequest request		= WebRequest.Create(url);
				response				= request.GetResponse();
				Stream receiveStream	= response.GetResponseStream();
				Encoding encoding		= System.Text.Encoding.UTF8;
				StreamReader sr			= new StreamReader(receiveStream, encoding);

				char[] read				= new char[256];
				int count				= sr.Read(read, 0, 256);

				while (count > 0)
				{
					sb.Append(read, 0, count);
					count				= sr.Read(read, 0, 256);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (response != null)
				{
					response.Close();
				}
			}

			return sb.ToString();
		}

		public string GetPictureFilename(int pictureId, int maxWidth, int maxHeight)
		{
			DataSet ds = picContext.PictureManager.GetPicture(pictureId, maxWidth, maxHeight);


			String strAppPath = @"/";

			string filename = ds.Tables[0].Rows[0]["FileName"].ToString();

			filename = strAppPath + "piccache/" + filename.Replace(@"\", @"/");

			return filename;
			
		}

	}
}
