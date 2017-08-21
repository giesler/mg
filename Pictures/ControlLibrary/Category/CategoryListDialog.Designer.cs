namespace msn2.net.Pictures.Controls
{
    partial class CategoryListDialog
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
// 
// listBox1
// 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(333, 238);
            this.listBox1.TabIndex = 0;
// 
// ok
// 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(182, 258);
            this.ok.Name = "ok";
            this.ok.TabIndex = 1;
            this.ok.Text = "OK";
// 
// cancel
// 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(264, 258);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 2;
            this.cancel.Text = "&Cancel";
// 
// CategoryListDialog
// 
            this.AcceptButton = this.ok;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(358, 288);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.listBox1);
            this.Name = "CategoryListDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Categories";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
    }
}