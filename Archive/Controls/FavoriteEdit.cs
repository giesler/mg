using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for FavoriteEdit.
	/// </summary>
	public class FavoriteEdit : msn2.net.Controls.ShellForm
	{
		#region Declares

		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.TextBox textBoxURL;
		private msn2.net.Controls.ShellButton buttonOK;
		private msn2.net.Controls.ShellButton buttonCancel;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.Label labelUrl;
		private System.Windows.Forms.TextBox textBoxIcon;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxNewTabRegEx;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxOpenRegEx;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBoxLinkBehavior;
		private System.Windows.Forms.Label label4;
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructors

		public FavoriteEdit()
		{
			InitializeComponent();
		}

		public FavoriteEdit(Data data): base(data)
		{
			InitializeComponent();

			this.Title	= data.Text;
			this.Url	= data.Url;

			if (data.ConfigData != null)
			{
				FavoriteConfigData favData		= (FavoriteConfigData) data.ConfigData;
                this.textBoxIcon.Text			= favData.IconLocation;
				this.textBoxOpenRegEx.Text		= favData.OpenLinkRegEx;
				this.textBoxNewTabRegEx.Text	= favData.NewTabRegEx;
			}
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
			this.labelName = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.textBoxURL = new System.Windows.Forms.TextBox();
			this.labelUrl = new System.Windows.Forms.Label();
			this.buttonOK = new msn2.net.Controls.ShellButton();
			this.buttonCancel = new msn2.net.Controls.ShellButton();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.textBoxIcon = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxNewTabRegEx = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxOpenRegEx = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBoxLinkBehavior = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
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
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(8, 8);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(56, 23);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name:";
			this.labelName.Visible = false;
			// 
			// textBoxName
			// 
			this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxName.Location = new System.Drawing.Point(64, 8);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(256, 20);
			this.textBoxName.TabIndex = 1;
			this.textBoxName.Text = "";
			this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
			this.textBoxName.Validated += new System.EventHandler(this.textBoxName_Validated);
			// 
			// textBoxURL
			// 
			this.textBoxURL.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxURL.Location = new System.Drawing.Point(64, 32);
			this.textBoxURL.Name = "textBoxURL";
			this.textBoxURL.Size = new System.Drawing.Size(256, 20);
			this.textBoxURL.TabIndex = 4;
			this.textBoxURL.Text = "";
			this.textBoxURL.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxURL_Validating);
			this.textBoxURL.Validated += new System.EventHandler(this.textBoxURL_Validated);
			// 
			// labelUrl
			// 
			this.labelUrl.Location = new System.Drawing.Point(8, 32);
			this.labelUrl.Name = "labelUrl";
			this.labelUrl.Size = new System.Drawing.Size(56, 23);
			this.labelUrl.TabIndex = 3;
			this.labelUrl.Text = "URL:";
			this.labelUrl.Visible = false;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.EndColor = System.Drawing.Color.Empty;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(168, 160);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.StartColor = System.Drawing.Color.LightGray;
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.CausesValidation = false;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.EndColor = System.Drawing.Color.Empty;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(248, 160);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.StartColor = System.Drawing.Color.LightGray;
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.DataMember = null;
			// 
			// textBoxIcon
			// 
			this.textBoxIcon.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxIcon.Location = new System.Drawing.Point(64, 56);
			this.textBoxIcon.Name = "textBoxIcon";
			this.textBoxIcon.Size = new System.Drawing.Size(256, 20);
			this.textBoxIcon.TabIndex = 8;
			this.textBoxIcon.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 7;
			this.label1.Text = "Icon:";
			this.label1.Visible = false;
			// 
			// textBoxNewTabRegEx
			// 
			this.textBoxNewTabRegEx.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxNewTabRegEx.Location = new System.Drawing.Point(64, 80);
			this.textBoxNewTabRegEx.Name = "textBoxNewTabRegEx";
			this.textBoxNewTabRegEx.Size = new System.Drawing.Size(256, 20);
			this.textBoxNewTabRegEx.TabIndex = 10;
			this.textBoxNewTabRegEx.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 9;
			this.label2.Text = "New Tab:";
			this.label2.Visible = false;
			// 
			// textBoxOpenRegEx
			// 
			this.textBoxOpenRegEx.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxOpenRegEx.Location = new System.Drawing.Point(64, 104);
			this.textBoxOpenRegEx.Name = "textBoxOpenRegEx";
			this.textBoxOpenRegEx.Size = new System.Drawing.Size(256, 20);
			this.textBoxOpenRegEx.TabIndex = 12;
			this.textBoxOpenRegEx.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 23);
			this.label3.TabIndex = 11;
			this.label3.Text = "Open:";
			this.label3.Visible = false;
			// 
			// comboBoxLinkBehavior
			// 
			this.comboBoxLinkBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxLinkBehavior.Items.AddRange(new object[] {
																	  "Open link",
																	  "Open link in new tab",
																	  "Open link in new window"});
			this.comboBoxLinkBehavior.Location = new System.Drawing.Point(64, 128);
			this.comboBoxLinkBehavior.Name = "comboBoxLinkBehavior";
			this.comboBoxLinkBehavior.Size = new System.Drawing.Size(256, 21);
			this.comboBoxLinkBehavior.TabIndex = 13;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 128);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 23);
			this.label4.TabIndex = 14;
			this.label4.Text = "Default:";
			this.label4.Visible = false;
			// 
			// FavoriteEdit
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(336, 190);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label4,
																		  this.comboBoxLinkBehavior,
																		  this.textBoxOpenRegEx,
																		  this.label3,
																		  this.textBoxNewTabRegEx,
																		  this.label2,
																		  this.textBoxIcon,
																		  this.label1,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.textBoxURL,
																		  this.labelUrl,
																		  this.textBoxName,
																		  this.labelName});
			this.Name = "FavoriteEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Favorite";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FavoriteEdit_Paint);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Buttons

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			// Save any data if it exists
			if (Data != null)
			{
				Data.Text	= this.Title;
				Data.Url	= this.Url;
				Data.Save();

				FavoriteConfigData favData	= (FavoriteConfigData) Data.ConfigData;
				favData.IconLocation		= this.textBoxIcon.Text;
				favData.OpenLinkRegEx		= textBoxOpenRegEx.Text;
				favData.NewTabRegEx			= textBoxNewTabRegEx.Text;
				
			}

			this.Visible = false;
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
		}

		#endregion

		#region Fields

		#region Name

		private void textBoxName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (textBoxName.Text.Length == 0)
			{
				errorProvider1.SetError(textBoxName, "You must enter a name!");
				e.Cancel = true;
			}
		}

		private void textBoxName_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(textBoxName, null);
		}

		#endregion

		#region Url

		private void textBoxURL_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (textBoxURL.Text.Length == 0)
			{
				errorProvider1.SetError(textBoxURL, "You must enter a URL!");
				e.Cancel = true;
			}
		}

		private void textBoxURL_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(textBoxURL, null);
		}

		#endregion

		#endregion

		#region Properties

		public string Title
		{
			get { return textBoxName.Text; }
			set { textBoxName.Text = value; }
		}

		public string Url
		{
			get { return textBoxURL.Text; }
			set {	 textBoxURL.Text = value; }
		}

		#endregion
		
		#region Paint

		private void FavoriteEdit_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);

			e.Graphics.DrawString(this.labelName.Text, this.labelName.Font, new SolidBrush(SystemColors.ControlText), new RectangleF(labelName.Location, labelName.Size));
			e.Graphics.DrawString(this.labelUrl.Text, this.labelUrl.Font, new SolidBrush(SystemColors.ControlText), new RectangleF(labelUrl.Location, labelUrl.Size));

		}

		#endregion

	}
}
