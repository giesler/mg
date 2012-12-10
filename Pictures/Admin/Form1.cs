using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using msn2.net.Pictures.Controls;

namespace msn2.net.Pictures
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private msn2.net.Pictures.Controls.EditPictureLink editPictureLink1;
		private msn2.net.Pictures.Controls.EditCategoryLink editCategoryLink1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
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
				if (components != null) 
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
			this.button1 = new System.Windows.Forms.Button();
			this.editPictureLink1 = new msn2.net.Pictures.Controls.EditPictureLink();
			this.editCategoryLink1 = new msn2.net.Pictures.Controls.EditCategoryLink();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(72, 56);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "Main Form";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// editPictureLink1
			// 
			this.editPictureLink1.Location = new System.Drawing.Point(40, 160);
			this.editPictureLink1.Name = "editPictureLink1";
			this.editPictureLink1.PictureId = 2120;
			this.editPictureLink1.Size = new System.Drawing.Size(48, 16);
			this.editPictureLink1.TabIndex = 1;
			this.editPictureLink1.Load += new System.EventHandler(this.editPictureLink1_Load);
			// 
			// editCategoryLink1
			// 
			this.editCategoryLink1.CategoryId = 165;
			this.editCategoryLink1.Location = new System.Drawing.Point(176, 160);
			this.editCategoryLink1.Name = "editCategoryLink1";
			this.editCategoryLink1.SignalRefresh = false;
			this.editCategoryLink1.Size = new System.Drawing.Size(48, 16);
			this.editCategoryLink1.TabIndex = 2;
			this.editCategoryLink1.Load += new System.EventHandler(this.editCategoryLink1_Load);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(176, 128);
			this.label1.Name = "label1";
			this.label1.TabIndex = 3;
			this.label1.Text = "Category";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 128);
			this.label2.Name = "label2";
			this.label2.TabIndex = 4;
			this.label2.Text = "Picture";
			// 
			// Form1
			// 
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.editCategoryLink1);
			this.Controls.Add(this.editPictureLink1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
		//	Application.Run(new Form1());

            try
            {
                msn2.net.Pictures.Controls.fMain f = new msn2.net.Pictures.Controls.fMain();
                Application.Run(f);
            }
            catch (Exception ex)
            {
                ExceptionDialog dialog = new ExceptionDialog(ex);
                dialog.ShowDialog();
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
		{
			msn2.net.Pictures.Controls.fMain f = new msn2.net.Pictures.Controls.fMain();
			f.Show();
		}

		private void editPictureLink1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void editCategoryLink1_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
