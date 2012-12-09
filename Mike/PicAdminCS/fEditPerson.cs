using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PicAdminCS
{
	/// <summary>
	/// Summary description for fPromptText.
	/// </summary>
	public class fEditPerson : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblCaption;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFullName;
		private bool mblnCancel = false;

		public fEditPerson()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.btnOK = new System.Windows.Forms.Button();
			this.lblCaption = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFullName = new System.Windows.Forms.TextBox();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnOK.Location = new System.Drawing.Point(160, 136);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblCaption
			// 
			this.lblCaption.Location = new System.Drawing.Point(16, 8);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(312, 23);
			this.lblCaption.TabIndex = 0;
			this.lblCaption.Text = "First Name:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(312, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Last Name:";
			// 
			// txtFullName
			// 
			this.txtFullName.Location = new System.Drawing.Point(16, 104);
			this.txtFullName.Name = "txtFullName";
			this.txtFullName.Size = new System.Drawing.Size(312, 20);
			this.txtFullName.TabIndex = 5;
			this.txtFullName.Text = "";
			// 
			// txtFirstName
			// 
			this.txtFirstName.Location = new System.Drawing.Point(16, 24);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(312, 20);
			this.txtFirstName.TabIndex = 1;
			this.txtFirstName.Text = "";
			// 
			// txtLastName
			// 
			this.txtLastName.Location = new System.Drawing.Point(16, 64);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(312, 20);
			this.txtLastName.TabIndex = 3;
			this.txtLastName.Text = "";
			this.txtLastName.Leave += new System.EventHandler(this.txtLastName_Leave);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCancel.Location = new System.Drawing.Point(240, 136);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(312, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "Full Name:";
			// 
			// fEditPerson
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(354, 170);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtFullName,
																		  this.label2,
																		  this.txtLastName,
																		  this.label1,
																		  this.btnCancel,
																		  this.btnOK,
																		  this.txtFirstName,
																		  this.lblCaption});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "fEditPerson";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Person";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			mblnCancel = false;
			Visible = false;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			mblnCancel = true;
			Visible = false;
		}

		private void txtLastName_Leave(object sender, System.EventArgs e)
		{
			if (txtFullName.Text == "")
				txtFullName.Text = txtFirstName.Text + " " + txtLastName.Text;
		}

		public bool Cancel
		{
			get
			{
				return mblnCancel;
			}
			set
			{
				mblnCancel = value;
			}
		}

		public String FirstName 
		{
			get
			{
				return txtFirstName.Text;
			}
			set
			{
				txtFirstName.Text = value;
			}
		}

		public String LastName 
		{
			get
			{
				return txtLastName.Text;
			}
			set
			{
				txtLastName.Text = value;
			}
		}
	
		public String FullName 
		{
			get
			{
				return txtFullName.Text;
			}
			set
			{
				txtFullName.Text = value;
			}
		}

	}
}
