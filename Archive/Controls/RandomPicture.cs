using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using msn2.net.Configuration;
using System.Diagnostics;
using System.Data;
using System.Xml;
using System.Net;

namespace msn2.net.Controls
{
	public class RandomPicture : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components = null;
		private System.Drawing.Image image;
		private string pictureRootPath = "http://pics/piccache";

		public RandomPicture(Data data): base(data)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(119, 101);
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 10000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// RandomPicture
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(119, 101);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.pictureBox1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "RandomPicture";
			this.Text = "Random Picture";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.RandomPicture_Paint);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Enabled = false;

			pics.RandomPictureService randomPicture = new pics.RandomPictureService();
			randomPicture.BeginRandomImageData(1, new AsyncCallback(RandomImageCallback), randomPicture);

// TODO: add rnadom picture status
//			dev.RandomPictureService picService = new dev.RandomPictureService();
//			byte[] picBytes = picService.RandomImage(1);
//
//			MemoryStream memStream = new MemoryStream(picBytes);
//		
//			Image img = Image.FromStream(memStream);
//			pictureBox1.Image = img;
		}

		private void RandomImageCallback(System.IAsyncResult result)
		{
			if (result.IsCompleted)
			{
				pics.RandomPictureService randomPicture = (pics.RandomPictureService) result.AsyncState;
				DataSet ds = randomPicture.EndRandomImageData(result);
				
				try
				{
				
					Debug.WriteLine(ds.Tables[1].Rows.Count);

					string path = pictureRootPath + @"/" + ds.Tables[1].Rows[0]["Filename"].ToString().Replace(@"\", @"/");
					WebRequest req = WebRequest.Create(path);
					WebResponse response = req.GetResponse();
					image = Image.FromStream(response.GetResponseStream());
					response.Close();
					this.Invalidate();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					Debug.WriteLine(ex.StackTrace);					
				}
			}

			timer1.Enabled = true;
		}

		private void RandomPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (image != null)
			{				
				e.Graphics.DrawImage(image, 0, 0, 200, 200);
			}
		}
	}
}

