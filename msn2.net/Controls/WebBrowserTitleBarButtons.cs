using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for WebBrowserTitleBarButtons.
	/// </summary>
	public class WebBrowserTitleBarButtons : System.Windows.Forms.UserControl
	{
		#region Declares

		private System.Windows.Forms.Button buttonRefresh;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button buttonSaveTo;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor

		public WebBrowserTitleBarButtons()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			this.components = new System.ComponentModel.Container();
			this.buttonRefresh = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.buttonSaveTo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonRefresh
			// 
			this.buttonRefresh.BackColor = System.Drawing.Color.Transparent;
			this.buttonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new System.Drawing.Size(14, 14);
			this.buttonRefresh.TabIndex = 0;
			this.buttonRefresh.Text = "r";
			this.toolTip1.SetToolTip(this.buttonRefresh, "Refresh web page");
			this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
			// 
			// buttonSaveTo
			// 
			this.buttonSaveTo.BackColor = System.Drawing.Color.Transparent;
			this.buttonSaveTo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonSaveTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonSaveTo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonSaveTo.Location = new System.Drawing.Point(16, 0);
			this.buttonSaveTo.Name = "buttonSaveTo";
			this.buttonSaveTo.Size = new System.Drawing.Size(14, 14);
			this.buttonSaveTo.TabIndex = 1;
			this.buttonSaveTo.Text = "+";
			this.toolTip1.SetToolTip(this.buttonSaveTo, "Save to...");
			this.buttonSaveTo.Click += new System.EventHandler(this.buttonSaveTo_Click);
			// 
			// WebBrowserTitleBarButtons
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonSaveTo,
																		  this.buttonRefresh});
			this.Name = "WebBrowserTitleBarButtons";
			this.Size = new System.Drawing.Size(32, 16);
			this.ResumeLayout(false);

		}
		#endregion

		#region Events

		private void buttonRefresh_Click(object sender, System.EventArgs e)
		{
			if (Refresh_Clicked != null)
				Refresh_Clicked(this, e);
		}

		private void buttonSaveTo_Click(object sender, System.EventArgs e)
		{
			if (SaveTo_Clicked != null)
				SaveTo_Clicked(this, e);
		}

		public event EventHandler Refresh_Clicked;
		public event EventHandler SaveTo_Clicked;

		#endregion

	}
}
