using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for GoogleResult.
	/// </summary>
	public class GoogleResultControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.LinkLabel linkToSite;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.LinkLabel linkCached;

		private ResultElement resultElement;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GoogleResultControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		public GoogleResultControl(ResultElement re)
		{
			InitializeComponent();

			resultElement			= re;
            labelTitle.Text			= re.title;
			labelDescription.Text	= re.summary;
			linkToSite.Text			= re.URL;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.linkToSite = new System.Windows.Forms.LinkLabel();
			this.labelTitle = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelDescription = new System.Windows.Forms.Label();
			this.linkCached = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// linkToSite
			// 
			this.linkToSite.Location = new System.Drawing.Point(32, 24);
			this.linkToSite.Name = "linkToSite";
			this.linkToSite.Size = new System.Drawing.Size(288, 23);
			this.linkToSite.TabIndex = 0;
			this.linkToSite.TabStop = true;
			this.linkToSite.Text = "Link Goes Here";
			// 
			// labelTitle
			// 
			this.labelTitle.Location = new System.Drawing.Point(32, 0);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(344, 24);
			this.labelTitle.TabIndex = 1;
			this.labelTitle.Text = "title";
			// 
			// label2
			// 
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "#";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelDescription
			// 
			this.labelDescription.Location = new System.Drawing.Point(32, 48);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(352, 48);
			this.labelDescription.TabIndex = 3;
			this.labelDescription.Text = "summary";
			// 
			// linkCached
			// 
			this.linkCached.Location = new System.Drawing.Point(344, 24);
			this.linkCached.Name = "linkCached";
			this.linkCached.Size = new System.Drawing.Size(48, 23);
			this.linkCached.TabIndex = 4;
			this.linkCached.TabStop = true;
			this.linkCached.Text = "Cached";
			this.linkCached.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// GoogleResultControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.linkCached,
																		  this.labelDescription,
																		  this.label2,
																		  this.linkToSite,
																		  this.labelTitle});
			this.Name = "GoogleResultControl";
			this.Size = new System.Drawing.Size(392, 96);
			this.ResumeLayout(false);

		}
		#endregion
	}

	
}
