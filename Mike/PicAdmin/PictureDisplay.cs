using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for PictureDisplay.
	/// </summary>
	public class PictureDisplay : System.Windows.Forms.UserControl
	{
		#region Declares
		private System.Windows.Forms.PictureBox pbPic;
		private System.ComponentModel.Container components = null;
		private Image imgCurImage;
		private string m_Filename;
		#endregion
		#region Constructor
		public PictureDisplay()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}
		#endregion
		#region Disposal
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

				if (imgCurImage != null)
				{
					imgCurImage.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		public void LoadImage(int pictureId)
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

				// See if there is a resized image
				string strFile = "";
				SqlConnection cn = new SqlConnection(Config.ConnectionString);
                cn.Open();
				SqlCommand cmd = new SqlCommand("select * from PictureCache where PictureID = @PictureID and MaxWidth = 750 and MaxHeight = 700", cn);
				cmd.Parameters.Add("@PictureID", Convert.ToInt32(pictureId));
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read()) 
				{
					strFile = @"\\ike\piccache\" + dr["Filename"].ToString();
				}
				dr.Close();
				cn.Close();

				// save the filename
				m_Filename = strFile;
				String strFullFile = "\\\\ike\\pictures\\" + strFile;

				// Load the file
				imgCurImage = Image.FromFile(strFile);
			
				// figure out dimensions to use
				this.Width			= (int) ( ( (float) imgCurImage.Width / (float) imgCurImage.Height) * (float) this.Height );
				pbPic.Width			= this.Width;
				pbPic.Height		= this.Height;

				pbPic.Image = imgCurImage;
				return;
			}
			catch (System.IO.FileNotFoundException fnfe) 
			{
				Console.WriteLine(fnfe.ToString());
				pbPic.Image = null;
				return;
			}		
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pbPic = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// pbPic
			// 
			this.pbPic.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbPic.Name = "pbPic";
			this.pbPic.Size = new System.Drawing.Size(280, 176);
			this.pbPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbPic.TabIndex = 1;
			this.pbPic.TabStop = false;
			// 
			// PictureDisplay
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.pbPic});
			this.Name = "PictureDisplay";
			this.Size = new System.Drawing.Size(280, 176);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
