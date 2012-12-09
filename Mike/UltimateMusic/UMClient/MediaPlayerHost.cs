using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace UMClient
{
	/// <summary>
	/// Summary description for MediaPlayerHost.
	/// </summary>
	public class MediaPlayerHost : System.Windows.Forms.Form
	{
		private AxMediaPlayer.AxMediaPlayer axMediaPlayer1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MediaPlayerHost()
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
			base.Dispose( disposing );
		}

		public string MediaFile 
		{
			set { axMediaPlayer1.FileName = value; }
		}

		public double Duration 
		{
			get { return axMediaPlayer1.Duration; }
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MediaPlayerHost));
			this.axMediaPlayer1 = new AxMediaPlayer.AxMediaPlayer();
			((System.ComponentModel.ISupportInitialize)(this.axMediaPlayer1)).BeginInit();
			this.SuspendLayout();
			// 
			// axMediaPlayer1
			// 
			this.axMediaPlayer1.Location = new System.Drawing.Point(8, 8);
			this.axMediaPlayer1.Name = "axMediaPlayer1";
			this.axMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMediaPlayer1.OcxState")));
			this.axMediaPlayer1.Size = new System.Drawing.Size(286, 225);
			this.axMediaPlayer1.TabIndex = 0;
			// 
			// MediaPlayerHost
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 246);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.axMediaPlayer1});
			this.Name = "MediaPlayerHost";
			this.Text = "MediaPlayerHost";
			((System.ComponentModel.ISupportInitialize)(this.axMediaPlayer1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
