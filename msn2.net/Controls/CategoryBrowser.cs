using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.Controls;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for CategoryBrowser.
	/// </summary>
	public class CategoryBrowser : msn2.net.Controls.ShellForm
	{
		#region Declares

		private msn2.net.Controls.ShellButton buttonOK;
		private msn2.net.Controls.ShellButton buttonCancel;
		private System.Windows.Forms.Panel panelTreeView;
		private System.ComponentModel.Container components = null;
		private CategoryTreeView categoryTreeView = null;

		#endregion

		#region Constructors

		public CategoryBrowser()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public CategoryBrowser(Data categoryData)
		{
			// Create tree view control and add to form
            categoryTreeView = new CategoryTreeView(this, categoryData);
			categoryTreeView.Dock = DockStyle.Fill;
			panelTreeView.Controls.Add(categoryTreeView);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonOK = new msn2.net.Controls.ShellButton();
			this.buttonCancel = new msn2.net.Controls.ShellButton();
			this.panelTreeView = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(128, 248);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(72, 24);
			this.buttonOK.StartColor = System.Drawing.Color.LightGray;
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(208, 248);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(72, 24);
			this.buttonCancel.StartColor = System.Drawing.Color.LightGray;
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// panelTreeView
			// 
			this.panelTreeView.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.panelTreeView.Location = new System.Drawing.Point(8, 8);
			this.panelTreeView.Name = "panelTreeView";
			this.panelTreeView.Size = new System.Drawing.Size(272, 232);
			this.panelTreeView.TabIndex = 3;
			// 
			// CategoryBrowser
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(292, 278);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelTreeView,
																		  this.buttonCancel,
																		  this.buttonOK});
			this.Name = "CategoryBrowser";
			this.Text = "CategoryBrowser";
			this.TitleVisible = true;
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.CategoryBrowser_Paint);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Buttons

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		#endregion

		#region Paint

		private void CategoryBrowser_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);
		}

		#endregion

		#region Properties

		public Data SelectedCategory
		{
			get
			{
				return categoryTreeView.Data;
			}
		}

		#endregion

	}
}
