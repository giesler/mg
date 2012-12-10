using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.Pictures;

namespace msn2.net.Pictures.Controls
{
    /// <summary>
    /// Summary description for AddPicturesToCategory.
    /// </summary>
    public class AddPicturesToCategory : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private int[] pictures;
        private CategoryInfo category;

        public AddPicturesToCategory(int[] pictures, CategoryInfo category)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.pictures = pictures;
            this.category = category;
            label2.Text = string.Format("{0} picutres", pictures.Length);
            label3.Text = category.Name;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(208, 160);
            this.ok.Name = "ok";
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(288, 160);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Would you like to add these pictures to the following category?";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(56, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(264, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(56, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(264, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "label3";
            // 
            // AddPicturesToCategory
            // 
            this.AcceptButton = this.ok;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(368, 190);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Name = "AddPicturesToCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddPicturesToCategory";
            this.ResumeLayout(false);

        }
        #endregion

        private void ok_Click(object sender, System.EventArgs e)
        {
            PictureManager picMan = PicContext.Current.PictureManager;

            foreach (int pictureId in this.pictures)
            {
                picMan.AddToCategory(pictureId, this.category.CategoryId);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
