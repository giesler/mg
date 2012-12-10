namespace msn2.net.Pictures.Controls
{
    partial class PictureProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.multiPictureEdit1 = new msn2.net.Pictures.Controls.MultiPictureEdit();
            this.SuspendLayout();
// 
// multiPictureEdit1
// 
            this.multiPictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiPictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.multiPictureEdit1.Name = "multiPictureEdit1";
            this.multiPictureEdit1.Size = new System.Drawing.Size(326, 180);
            this.multiPictureEdit1.TabIndex = 0;
// 
// PictureProperties
// 
            this.ClientSize = new System.Drawing.Size(326, 180);
            this.Controls.Add(this.multiPictureEdit1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PictureProperties";
            this.ShowInTaskbar = false;
            this.Text = "Properties";
            this.ResumeLayout(false);

        }

        #endregion

        private MultiPictureEdit multiPictureEdit1;
    }
}