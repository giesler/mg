using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PicAdminCS
{
	/// <summary>
	/// Summary description for fPersonSelect.
	/// </summary>
	public class fPersonSelect : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private PicAdminCS.PeopleCtl peopleCtl1;

		private bool mblnCancel = false;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fPersonSelect()
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
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.peopleCtl1 = new PicAdminCS.PeopleCtl();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(320, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select a person:";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOK.Location = new System.Drawing.Point(104, 264);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// peopleCtl1
			// 
			this.peopleCtl1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.peopleCtl1.Location = new System.Drawing.Point(16, 40);
			this.peopleCtl1.Name = "peopleCtl1";
			this.peopleCtl1.Size = new System.Drawing.Size(240, 216);
			this.peopleCtl1.TabIndex = 3;
			this.peopleCtl1.DoubleClickPerson += new PicAdminCS.DoubleClickPersonEventHandler(this.peopleCtl1_DoubleClickPerson);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Location = new System.Drawing.Point(184, 264);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// fPersonSelect
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(272, 294);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.peopleCtl1,
																		  this.btnCancel,
																		  this.btnOK,
																		  this.label1});
			this.Name = "fPersonSelect";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select...";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{

			// make sure someone was selected
			if (peopleCtl1.SelectedPerson == null) 
			{
				MessageBox.Show("You must select a person!", "Select", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			} 
			else 
			{
				Visible = false;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			mblnCancel = true;
			Visible = false;
		}

		private void peopleCtl1_DoubleClickPerson(object sender, PicAdminCS.PersonCtlEventArgs e)
		{
			btnOK_Click(sender, e);
		}

		public bool Cancel
		{
			get 
			{
				return mblnCancel;
			}
		}

		public DataSetPerson.PersonRow SelectedPerson 
		{
			get 
			{
				return (peopleCtl1.SelectedPerson);
			}
		}
	}
}
