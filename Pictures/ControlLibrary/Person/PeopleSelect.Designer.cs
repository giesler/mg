namespace msn2.net.Pictures.Controls
{
    partial class PeopleSelect
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
            this.personPicker1 = new msn2.net.Pictures.Controls.PersonPicker();
            this.SuspendLayout();
            // 
            // personPicker1
            // 
            this.personPicker1.Location = new System.Drawing.Point(2, 3);
            this.personPicker1.Name = "personPicker1";
            this.personPicker1.Size = new System.Drawing.Size(540, 283);
            this.personPicker1.TabIndex = 0;
            // 
            // PeopleSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 289);
            this.Controls.Add(this.personPicker1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PeopleSelect";
            this.Text = "People";
            this.ResumeLayout(false);

        }

        #endregion

        private PersonPicker personPicker1;
    }
}