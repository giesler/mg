using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace XMAdmin
{
	/// <summary>
	/// Summary description for main.
	/// </summary>
	public class formMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MdiClient mdiClient1;
		private System.Windows.Forms.TabControl tabWindows;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public formMain()
		{
			//
			// Required for Windows Form Designer support
			//
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
			this.tabWindows = new System.Windows.Forms.TabControl();
			this.mdiClient1 = new System.Windows.Forms.MdiClient();
			this.SuspendLayout();
			// 
			// tabWindows
			// 
			this.tabWindows.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabWindows.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabWindows.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tabWindows.ItemSize = new System.Drawing.Size(0, 32);
			this.tabWindows.Location = new System.Drawing.Point(0, 449);
			this.tabWindows.Name = "tabWindows";
			this.tabWindows.SelectedIndex = 0;
			this.tabWindows.Size = new System.Drawing.Size(640, 36);
			this.tabWindows.TabIndex = 3;
			this.tabWindows.SelectedIndexChanged += new System.EventHandler(this.tabWindows_SelectedIndexChanged);
			// 
			// mdiClient1
			// 
			this.mdiClient1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mdiClient1.Name = "mdiClient1";
			this.mdiClient1.TabIndex = 2;
			// 
			// formMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 485);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabWindows,
																		  this.mdiClient1});
			this.IsMdiContainer = true;
			this.Name = "formMain";
			this.Text = "XMAdmin";
			this.Load += new System.EventHandler(this.formMain_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void formMain_Load(object sender, System.EventArgs e)
		{
			//load a child forms
			new formPictures().MdiParent = this;
			new formUsers().MdiParent = this;

			//show all forms
			foreach(Form childForm in MdiChildren)
				childForm.Show();
		}

		public void MdiChildActivated(CustomMdiChild childForm)
		{
			//look for this tab
			foreach(TabPage tab in tabWindows.TabPages)
			{
				//same window?
				if (tab.Tag == childForm)
				{
					tabWindows.SelectedTab = tab;
				}
			}
		}

		public void MdiChildCreate(CustomMdiChild childForm)
		{
			//maximize all child forms
			childForm.WindowState = FormWindowState.Maximized;

			//insert tab
			TabPage tab;
			tab = new TabPage(childForm.Text);
			tab.Tag = childForm;
			tabWindows.TabPages.Add(tab);
		}

		private void tabWindows_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//bring the new window to the foreground
			((CustomMdiChild)tabWindows.SelectedTab.Tag).Activate();
		}
	}

	/// <summary>
	/// Provides support for the tab control in the main window.
	/// </summary>
	public class CustomMdiChild : Form
	{
		public CustomMdiChild()
		{
			//install events
			this.Load += new EventHandler(CustomMdiChild_Load);
			this.Enter += new EventHandler(CustomMdiChild_Activated);
		}

		public void CustomMdiChild_Load(object sender, EventArgs e)
		{
			if (MdiParent == null)
				return;

			((formMain)MdiParent).MdiChildCreate(this);
		}

		public void CustomMdiChild_Activated(object sender, EventArgs e)
		{
			if (MdiParent == null)
				return;

			((formMain)MdiParent).MdiChildActivated(this);
		}
	}
}
