using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for fSelectCategory.
	/// </summary>
	public class fSelectCategory : System.Windows.Forms.Form
	{
        private static CategoryInfo lastSelected = null;
        private static fSelectCategory current = null;

        public static fSelectCategory GetSelectCategoryDialog()
        {
            if (current == null)
            {
                current = new fSelectCategory();
            }

            return current;
        }

        private msn2.net.Pictures.Controls.CategoryTree categoryTree1;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fSelectCategory()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            if (lastSelected != null)
            {
                categoryTree1.SetSelectedCategory(lastSelected);
            }
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
			this.categoryTree1 = new msn2.net.Pictures.Controls.CategoryTree();
			this.ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// categoryTree1
			// 
			this.categoryTree1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.categoryTree1.Location = new System.Drawing.Point(8, 8);
			this.categoryTree1.Name = "categoryTree1";
			this.categoryTree1.Size = new System.Drawing.Size(308, 288);
			this.categoryTree1.TabIndex = 0;
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ok.Location = new System.Drawing.Point(164, 304);
			this.ok.Name = "ok";
			this.ok.TabIndex = 1;
			this.ok.Text = "OK";
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(244, 304);
			this.cancel.Name = "cancel";
			this.cancel.TabIndex = 2;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// fSelectCategory
			// 
			this.AcceptButton = this.ok;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(328, 334);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.categoryTree1);
			this.Name = "fSelectCategory";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select a category";
			this.ResumeLayout(false);

		}
		#endregion

		private void cancel_Click(object sender, System.EventArgs e)
		{
			DialogResult	= DialogResult.Cancel;
            this.Close();
		}

		private void ok_Click(object sender, System.EventArgs e)
		{
			DialogResult	= DialogResult.OK;
            this.Close();

            lastSelected = this.SelectedCategory;
        }

		public CategoryInfo SelectedCategory
		{
			get
			{
				return categoryTree1.SelectedCategory;
			}
		}
	}
}
