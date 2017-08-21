using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for PersonSelect.
	/// </summary>
	public class PersonSelect : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btnSelectPictureBy;
		private System.Windows.Forms.TextBox txtPictureBy;
		private System.Windows.Forms.TextBox txtPersonName;
		private System.Windows.Forms.Button btnSelectPerson;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Person pr;

		public PersonSelect()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonSelect));
            this.txtPersonName = new System.Windows.Forms.TextBox();
            this.txtPictureBy = new System.Windows.Forms.TextBox();
            this.btnSelectPictureBy = new System.Windows.Forms.Button();
            this.btnSelectPerson = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPersonName
            // 
            this.txtPersonName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPersonName.Location = new System.Drawing.Point(0, 0);
            this.txtPersonName.Name = "txtPersonName";
            this.txtPersonName.ReadOnly = true;
            this.txtPersonName.Size = new System.Drawing.Size(248, 20);
            this.txtPersonName.TabIndex = 8;
            // 
            // txtPictureBy
            // 
            this.txtPictureBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPictureBy.BackColor = System.Drawing.SystemColors.Control;
            this.txtPictureBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPictureBy.Location = new System.Drawing.Point(72, 56);
            this.txtPictureBy.Name = "txtPictureBy";
            this.txtPictureBy.Size = new System.Drawing.Size(320, 20);
            this.txtPictureBy.TabIndex = 6;
            this.txtPictureBy.TabStop = false;
            // 
            // btnSelectPictureBy
            // 
            this.btnSelectPictureBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectPictureBy.Location = new System.Drawing.Point(400, 56);
            this.btnSelectPictureBy.Name = "btnSelectPictureBy";
            this.btnSelectPictureBy.Size = new System.Drawing.Size(24, 20);
            this.btnSelectPictureBy.TabIndex = 7;
            this.btnSelectPictureBy.Text = "...";
            // 
            // btnSelectPerson
            // 
            this.btnSelectPerson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectPerson.Location = new System.Drawing.Point(255, 0);
            this.btnSelectPerson.Name = "btnSelectPerson";
            this.btnSelectPerson.Size = new System.Drawing.Size(57, 20);
            this.btnSelectPerson.TabIndex = 9;
            this.btnSelectPerson.Text = "Select";
            this.btnSelectPerson.Click += new System.EventHandler(this.btnSelectPerson_Click);
            // 
            // PersonSelect
            // 
            this.Controls.Add(this.btnSelectPerson);
            this.Controls.Add(this.txtPersonName);
            this.Controls.Add(this.btnSelectPictureBy);
            this.Controls.Add(this.txtPictureBy);
            this.Name = "PersonSelect";
            this.Size = new System.Drawing.Size(312, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnSelectPerson_Click(object sender, System.EventArgs e)
		{
			fPersonSelect fPS = new fPersonSelect();
			fPS.ShowDialog();

            if (!fPS.Cancel)
            {
                pr = fPS.SelectedPerson;
                if (pr != null)
                {
                    txtPersonName.Text = pr.FullName;
                }
            }

		}

		public Person SelectedPerson
		{
			get 
			{
				return pr;
			}
            set
            {
                if (value != null)
                {
                    pr = value;
                    txtPersonName.Text = pr.FullName;
                }
            }
		}

		public void ClearSelectedPerson() 
		{
			pr = null;
			txtPersonName.Text = "";
		}

		public int SelectedPersonID 
		{
			get 
			{
				if (pr == null)
					return 0;
				else
                    return pr.PersonID;
			}
			set 
			{
                if (value != 0)
                {
                    pr = PicContext.Current.UserManager.GetPersonA(value);
                    txtPersonName.Text = pr.FullName;
                }
            }
		}
	}
}
