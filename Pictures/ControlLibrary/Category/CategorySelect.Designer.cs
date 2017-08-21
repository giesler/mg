namespace msn2.net.Pictures.Controls
{
    partial class CategorySelect
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
            this.categoryPicker1 = new msn2.net.Pictures.Controls.CategoryPicker();
            this.SuspendLayout();
            // 
            // categoryPicker1
            // 
            this.categoryPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryPicker1.Location = new System.Drawing.Point(0, 0);
            this.categoryPicker1.Name = "categoryPicker1";
            this.categoryPicker1.Size = new System.Drawing.Size(489, 253);
            this.categoryPicker1.TabIndex = 0;
            // 
            // CategorySelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 253);
            this.Controls.Add(this.categoryPicker1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CategorySelect";
            this.Text = "Categories";
            this.ResumeLayout(false);

        }

        #endregion

        private CategoryPicker categoryPicker1;

    }
}