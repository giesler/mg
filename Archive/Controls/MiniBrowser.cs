using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for MiniBrowser.
	/// </summary>
	public class MiniBrowser : msn2.net.Controls.ShellForm
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
        private TabControl tabControl;
		private string baseUrl = "";
		
		public MiniBrowser()
		{
			InitializeComponent();
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
			this.components = new System.ComponentModel.Container();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl = new TabControl();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "";
			// 
			// tabControl1
			// 
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Name = "tabControl";
			this.tabControl.Size = new System.Drawing.Size(432, 264);
			this.tabControl.TabIndex = 1;
			// 
			// MiniBrowser
			// 
			this.ClientSize = new System.Drawing.Size(432, 264);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl});
			this.Name = "MiniBrowser";
			this.Text = "MiniBrowser";
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Methods

		protected void AddWebLink(WebLinkPage item)
		{
			tabControl.TabPages.Add(item);
		}

		#endregion

		private void tabControl1_TabIndexChanged(object sender, System.EventArgs e)
		{
			WebLinkPage page = (WebLinkPage) tabControl.TabPages[tabControl.SelectedIndex];
			page.Navigate(baseUrl);
		}

		private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
		{
			WebLinkPage page = (WebLinkPage) tabControl.TabPages[tabControl.SelectedIndex];
			page.Navigate(baseUrl);
		}

		#region Properties

		public string BaseUrl
		{
			get { return baseUrl; }
			set { baseUrl = value; }
		}

		#endregion

	}
}
