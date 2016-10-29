using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for SaveLocation.
	/// </summary>
	public class SaveLocation : System.Windows.Forms.UserControl
	{
		#region Declares

		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.ListView listViewLocation;
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor / Disposal

		public SaveLocation()
		{
			// This call is required by the Windows.Forms Form Designer.
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

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonAdd = new System.Windows.Forms.Button();
			this.listViewLocation = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonAdd.Location = new System.Drawing.Point(272, 8);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(32, 16);
			this.buttonAdd.TabIndex = 4;
			this.buttonAdd.Text = "...";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// listViewLocation
			// 
			this.listViewLocation.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listViewLocation.Location = new System.Drawing.Point(8, 8);
			this.listViewLocation.Name = "listViewLocation";
			this.listViewLocation.Size = new System.Drawing.Size(256, 16);
			this.listViewLocation.TabIndex = 2;
			// 
			// SaveLocation
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonAdd,
																		  this.listViewLocation});
			this.Name = "SaveLocation";
			this.Size = new System.Drawing.Size(312, 32);
			this.ResumeLayout(false);

		}
		#endregion

		#region Buttons

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
            CategoryBrowser browser = new CategoryBrowser(ConfigurationSettings.Current.Data.Get("Favorites.Category"));
			
			if (browser.ShowShellDialog(this) == DialogResult.OK)
			{
				ListViewItem item = listViewLocation.Items.Add(browser.SelectedCategory.Text);
				item.Tag = browser.SelectedCategory;
			}
		}

		#endregion
	}
}
