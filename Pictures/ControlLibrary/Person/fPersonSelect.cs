using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for fPersonSelect.
	/// </summary>
	public class fPersonSelect : System.Windows.Forms.Form
    {
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private msn2.net.Pictures.Controls.PeopleCtl peopleCtl1;

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
            this.btnOK = new System.Windows.Forms.Button();
            this.peopleCtl1 = new msn2.net.Pictures.Controls.PeopleCtl();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(196, 368);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // peopleCtl1
            // 
            this.peopleCtl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.peopleCtl1.Location = new System.Drawing.Point(9, 4);
            this.peopleCtl1.Name = "peopleCtl1";
            this.peopleCtl1.Size = new System.Drawing.Size(343, 358);
            this.peopleCtl1.TabIndex = 0;
            this.peopleCtl1.DoubleClickPerson += new msn2.net.Pictures.Controls.DoubleClickPersonEventHandler(this.peopleCtl1_DoubleClickPerson);
            this.peopleCtl1.Load += new System.EventHandler(this.peopleCtl1_Load);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(276, 368);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // fPersonSelect
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(364, 398);
            this.Controls.Add(this.peopleCtl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "fPersonSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a person";
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
                this.Close();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			mblnCancel = true;
            this.Close();
        }

		private void peopleCtl1_DoubleClickPerson(object sender, msn2.net.Pictures.Controls.PersonCtlEventArgs e)
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

		public Person SelectedPerson 
		{
			get 
			{
				return (peopleCtl1.SelectedPerson);
			}
		}

        private void peopleCtl1_ClickPerson(object sender, PersonCtlEventArgs e)
        {
            this.UpdateControls();
        }

        private void UpdateControls()
        {
            bool personSelected = (this.peopleCtl1.SelectedPerson != null);

            this.btnOK.Enabled = personSelected;
        }

        private void peopleCtl1_Load(object sender, EventArgs e)
        {

        }
	}
}
