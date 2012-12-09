using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace PicAdmin
{
	/// <summary>
	/// Summary description for fPictureViewer.
	/// </summary>
	public class fPictureViewer : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panelPic;
		private System.Windows.Forms.PictureBox pbPic;

		private Image imgCurImage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fPictureViewer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			if (imgCurImage != null) 
			{
				imgCurImage.Dispose();
				imgCurImage = null;
			}
			if (pbPic.Image != null)
				pbPic.Image = null;

			base.Dispose( disposing );
		}

		public void LoadImage(String strFile) 
		{

			try 
			{
				// get rid of current image if we have one
				if (imgCurImage != null) 
				{   
					imgCurImage.Dispose();
					imgCurImage = null;
				}
				pbPic.Image = null;
				strFile = strFile.Replace("/", "\\");

				String strFullFile = "\\\\kenny\\inetpub\\pictures\\" + strFile;

				// figure out the temp location, create directory if needed
				String strTempFile = System.Environment.GetEnvironmentVariable("TEMP")
					+ "\\_piccache\\" + strFile;
				strTempFile = strTempFile.Replace("/", "\\");
				String strTempDir = strTempFile.Substring(0, strTempFile.LastIndexOf("\\"));
				if (!Directory.Exists(strTempDir)) 
					Directory.CreateDirectory(strTempDir);

				// copy file to temp location
				if (!File.Exists(strTempFile) ||
					File.GetLastWriteTime(strTempFile) < File.GetLastWriteTime(strFullFile))
					File.Copy(strFullFile, strTempFile);

				// Load the file
				imgCurImage = Image.FromFile(strTempFile);
			
				SizeImage();

				return;
			}
			catch (System.IO.FileNotFoundException fnfe) 
			{
				Console.WriteLine(fnfe.ToString());
				pbPic.Image = null;
				return;
			}		
		}

		private void SizeImage() 
		{
			// figure out dimensions to use
			int intMaxWidth  = this.Width;
			int intMaxHeight = this.Height;
			int intNewWidth  = imgCurImage.Width;
			int intNewHeight = imgCurImage.Height;

			// see if image is wider then we want, if so resize
			if (intNewWidth > intMaxWidth) 
			{
				intNewWidth	 = intMaxWidth;
				intNewHeight = (int) ( (float) imgCurImage.Height * ( (float) intNewWidth / (float) imgCurImage.Width) );
			}

			// see if image height is greater then we want, if so, resize
			if (intNewHeight > intMaxHeight)
			{
				intNewWidth  = (int) ( (float) intNewWidth * ( (float) intMaxHeight / (float) intNewHeight) );
				intNewHeight = intMaxHeight;
			}

			// size the picture control to the size
			pbPic.Width  = intNewWidth;
			pbPic.Height = intNewHeight;
			pbPic.Left   = (this.Width / 2) - (pbPic.Width / 2);
			pbPic.Top    = (this.Top   / 2) - (pbPic.Top   / 2);

			//				panelPic.Width = (int) ( ( (float) imgCurImage.Width / (float) imgCurImage.Height) * (float) panelPic.Height );
			//				pbPic.Width = panelPic.Width;
			//				pbPic.Height = panelPic.Height;

			pbPic.Image = imgCurImage;
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelPic = new System.Windows.Forms.Panel();
			this.pbPic = new System.Windows.Forms.PictureBox();
			this.panelPic.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelPic
			// 
			this.panelPic.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.pbPic});
			this.panelPic.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelPic.Name = "panelPic";
			this.panelPic.Size = new System.Drawing.Size(560, 414);
			this.panelPic.TabIndex = 0;
			// 
			// pbPic
			// 
			this.pbPic.Location = new System.Drawing.Point(48, 64);
			this.pbPic.Name = "pbPic";
			this.pbPic.Size = new System.Drawing.Size(440, 264);
			this.pbPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbPic.TabIndex = 0;
			this.pbPic.TabStop = false;
			// 
			// fPictureViewer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 414);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelPic});
			this.Name = "fPictureViewer";
			this.Text = "fPictureViewer";
			this.Resize += new System.EventHandler(this.fPictureViewer_Resize);
			this.panelPic.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void fPictureViewer_Resize(object sender, System.EventArgs e)
		{
			SizeImage();
		}
	}
}
