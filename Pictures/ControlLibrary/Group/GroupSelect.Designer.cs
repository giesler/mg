namespace msn2.net.Pictures.Controls
{
    partial class GroupSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.groupPicker1 = new msn2.net.Pictures.Controls.GroupPicker();
            this.SuspendLayout();
            // 
            // groupPicker1
            // 
            this.groupPicker1.AllowRemoveEveryone = true;
            this.groupPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPicker1.Location = new System.Drawing.Point(0, 0);
            this.groupPicker1.Name = "groupPicker1";
            this.groupPicker1.Size = new System.Drawing.Size(421, 161);
            this.groupPicker1.TabIndex = 0;
            // 
            // GroupSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 161);
            this.Controls.Add(this.groupPicker1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "GroupSelect";
            this.Text = "Groups";
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPicker groupPicker1;

    }
}