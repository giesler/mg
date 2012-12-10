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
		private Crownwood.Magic.Controls.TabControl tabControl1;
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
			this.tabControl1 = new Crownwood.Magic.Controls.TabControl();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "";
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm;
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.HotTextColor = System.Drawing.SystemColors.ActiveCaption;
			this.tabControl1.HotTrack = false;
			this.tabControl1.ImageList = null;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.PositionTop = true;
			this.tabControl1.SelectedIndex = -1;
			this.tabControl1.ShowArrows = false;
			this.tabControl1.ShowClose = false;
			this.tabControl1.ShrinkPagesToFit = false;
			this.tabControl1.Size = new System.Drawing.Size(432, 264);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.TextColor = System.Drawing.SystemColors.MenuText;
			this.tabControl1.SelectionChanged += new System.EventHandler(this.tabControl1_SelectionChanged);
			// 
			// MiniBrowser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 264);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "MiniBrowser";
			this.Text = "MiniBrowser";
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Methods

		protected void AddWebLink(WebLinkPage item)
		{
			tabControl1.TabPages.Add(item);
		}

		#endregion

		private void tabControl1_TabIndexChanged(object sender, System.EventArgs e)
		{
			WebLinkPage page = (WebLinkPage) tabControl1.TabPages[tabControl1.SelectedIndex];
			page.Navigate(baseUrl);
		}

		private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
		{
			WebLinkPage page = (WebLinkPage) tabControl1.TabPages[tabControl1.SelectedIndex];
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
