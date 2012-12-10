using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace msn2.net.Controls
{
	public class MSNBCWeather : msn2.net.Controls.MiniBrowser
	{
		private System.ComponentModel.IContainer components = null;

		public MSNBCWeather()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.FixedSize = new Size(500, 250);

			this.Text    = "MSNBC Weather";
			this.BaseUrl = "http://dev/home/weather.aspx?aid={0}";

			this.AddWebLink(new WebLinkPage("Kirkland", "WAKI"));
			this.AddWebLink(new WebLinkPage("Seattle", "SEA"));
			this.AddWebLink(new WebLinkPage("Madison", "MSN"));

			this.Left = Screen.PrimaryScreen.Bounds.Right - this.Width - 50;
			this.Top  = Screen.PrimaryScreen.Bounds.Bottom - this.Height - 300;

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
			this.SuspendLayout();
			// 
			// MSNBCWeather
			// 
			this.AutoLayout = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 176);
			this.Name = "MSNBCWeather";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.ResumeLayout(false);

		}
		#endregion
	}
}

