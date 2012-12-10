using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ShellSave.
	/// </summary>
	public class ShellSave : msn2.net.Controls.ShellForm
	{
		#region Declares

		private msn2.net.Controls.CategoryTreeView categoryTreeView;
		private msn2.net.Controls.ShellButton buttonOK;
		private msn2.net.Controls.ShellButton buttonCancel;
		private System.ComponentModel.Container components = null;
		private msn2.net.Common.ConfigData configData = null;

		#endregion

		#region Constructor

		public ShellSave()
		{
			InternalConstructor();
			categoryTreeView.RootData = ConfigurationSettings.Current.Data.Get("Favorites").Get("msn2.net");
		}

		public ShellSave(msn2.net.Common.ConfigData configData)
		{
			InternalConstructor();
			this.configData	= configData;
		}

		private void InternalConstructor()
		{
			InitializeComponent();

			// Set to current mouse position
			this.Left	= Cursor.Position.X - buttonOK.Left - (buttonOK.Width / 2);
			this.Top	= Cursor.Position.Y - buttonCancel.Top - (buttonCancel.Width / 2);

			if (this.Left < 0)
				this.Left = 0;

			if (this.Top < 0)
				this.Top = 0;
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
			this.categoryTreeView = new msn2.net.Controls.CategoryTreeView();
			this.buttonOK = new msn2.net.Controls.ShellButton();
			this.buttonCancel = new msn2.net.Controls.ShellButton();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// categoryTreeView
			// 
			this.categoryTreeView.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.categoryTreeView.Location = new System.Drawing.Point(8, 8);
			this.categoryTreeView.Name = "categoryTreeView";
			this.categoryTreeView.ParentShellForm = null;
			this.categoryTreeView.Size = new System.Drawing.Size(216, 168);
			this.categoryTreeView.TabIndex = 0;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(112, 184);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(48, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(168, 184);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(48, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// ShellSave
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(232, 214);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.categoryTreeView});
			this.Name = "ShellSave";
			this.ShadedBackground = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Save To...";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Buttons

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			this.Data		= categoryTreeView.Data;
            DialogResult	= DialogResult.OK;
			this.Visible	= false;
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Data		= null;
			DialogResult	= DialogResult.Cancel;
			this.Visible	= false;
		}

		#endregion

	}
}
