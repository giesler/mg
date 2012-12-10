using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace msn2.net.Controls
{
	public class MSNBCHeadlines : msn2.net.Controls.MiniBrowser
	{
		private System.ComponentModel.IContainer components = null;

		public MSNBCHeadlines()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.FixedSize   = new Size(300, 300);
			this.Text        = "MSNBC Headlines";

			this.BaseUrl = "http://dev/home/MSNBCHeadlines.aspx?page={0}";

			// Add items
			this.AddWebLink(new WebLinkPage("Cover", "1"));
			this.AddWebLink(new WebLinkPage("News", "2"));
			this.AddWebLink(new WebLinkPage("Business", "3"));
			this.AddWebLink(new WebLinkPage("Health", "4"));
			this.AddWebLink(new WebLinkPage("Technology", "5"));
			this.AddWebLink(new WebLinkPage("TV News", "6"));
			this.AddWebLink(new WebLinkPage("Opinions", "7"));

			this.Left = Screen.PrimaryScreen.Bounds.Right - this.Width - 50;
			this.Top  = Screen.PrimaryScreen.Bounds.Bottom  - this.Height - 50;
            
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
			// MSNBCHeadlines
			// 
			this.AutoLayout = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 216);
			this.Name = "MSNBCHeadlines";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.ResumeLayout(false);

		}
		#endregion
	}
}

