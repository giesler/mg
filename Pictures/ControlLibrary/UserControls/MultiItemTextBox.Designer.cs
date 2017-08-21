namespace msn2.net.Pictures.Controls.UserControls
{
    partial class MultiItemTextBox
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

            this.textBox1.Leave -= new System.EventHandler(this.textBox1_Leave);
            this.textBox1.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseClick);
            this.textBox1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.Enter -= new System.EventHandler(this.textBox1_Enter);
            this.textBox1.TextChanged -= new System.EventHandler(this.textBox1_TextChanged);

        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
// 
// textBox1
// 
            this.textBox1.AcceptsTab = true;
            this.textBox1.AutoSize = false;
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(182, 18);
            this.textBox1.TabIndex = 0;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            this.textBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseClick);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
// 
// MultiItemTextBox
// 
            this.Controls.Add(this.textBox1);
            this.Name = "MultiItemTextBox";
            this.Size = new System.Drawing.Size(182, 18);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MultiItemTextBox_KeyUp);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MultiItemTextBox_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
    }
}
