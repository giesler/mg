using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for PictureFilter.
	/// </summary>
	public class PictureFilter : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected Crownwood.Magic.Docking.DockingManager dockingManager;
		protected CategoryTree categoryTree;

		public PictureFilter()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			dockingManager = new DockingManager(this, VisualStyle.IDE);

			Zone leftZone  = dockingManager.CreateZoneForContent(State.DockLeft);

			categoryTree = new CategoryTree();
			Content categoryTreeContent = dockingManager.Contents.Add(categoryTree, "Folder");
			dockingManager.AddContentToZone(categoryTreeContent, leftZone, 0);

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
			// 
			// PictureFilter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 558);
			this.Name = "PictureFilter";
			this.Text = "PictureFilter";

		}
		#endregion

		private void label2_Click(object sender, System.EventArgs e)
		{
		
		}

		private void dateTimePicker1_ValueChanged(object sender, System.EventArgs e)
		{
		
		}

		private void label1_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
