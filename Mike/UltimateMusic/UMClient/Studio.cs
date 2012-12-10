using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace UMClient
{
	/// <summary>
	/// Summary description for Studio.
	/// </summary>
	public class Studio : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected Crownwood.Magic.Docking.IDockingManager dockManager = null;

		public Studio()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Init the docking component
			dockManager = new Crownwood.Magic.Docking.DockingManagerIDE(this);
			

			// Add the notepad sample
			Crownwood.Magic.Docking.Content notepad = dockManager.CreateContent(new RichTextBox(),	"Notepad");
			dockManager.AddSingleContent(notepad, Crownwood.Magic.Docking.State.Floating);
            


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
			// Studio
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(480, 334);
			this.Name = "Studio";
			this.Text = "Studio";
			this.Load += new System.EventHandler(this.Studio_Load);

		}
		#endregion

		private void Studio_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
