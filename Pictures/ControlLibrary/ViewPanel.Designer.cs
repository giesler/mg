namespace msn2.net.Pictures.Controls
{
    partial class ViewPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewSelection = new System.Windows.Forms.ComboBox();
            this.viewLabel = new System.Windows.Forms.Label();
            this.currentFilterView = new System.Windows.Forms.Panel();
            this.SuspendLayout();
// 
// viewSelection
// 
            this.viewSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewSelection.FormattingEnabled = true;
            this.viewSelection.Items.AddRange(new object[] {
            "Category",
            "Date picture taken",
            "Date picture added",
            "Person"});
            this.viewSelection.Location = new System.Drawing.Point(55, 4);
            this.viewSelection.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.viewSelection.Name = "viewSelection";
            this.viewSelection.Size = new System.Drawing.Size(140, 21);
            this.viewSelection.TabIndex = 3;
            this.viewSelection.SelectedIndexChanged += new System.EventHandler(this.viewSelection_SelectedIndexChanged);
// 
// viewLabel
// 
            this.viewLabel.AutoSize = true;
            this.viewLabel.Location = new System.Drawing.Point(5, 8);
            this.viewLabel.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(47, 14);
            this.viewLabel.TabIndex = 2;
            this.viewLabel.Text = "View by:";
// 
// currentFilterView
// 
            this.currentFilterView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFilterView.Location = new System.Drawing.Point(4, 29);
            this.currentFilterView.Name = "currentFilterView";
            this.currentFilterView.Size = new System.Drawing.Size(191, 302);
            this.currentFilterView.TabIndex = 4;
// 
// ViewPanel
// 
            this.Controls.Add(this.currentFilterView);
            this.Controls.Add(this.viewSelection);
            this.Controls.Add(this.viewLabel);
            this.Name = "ViewPanel";
            this.Size = new System.Drawing.Size(198, 334);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox viewSelection;
        private System.Windows.Forms.Label viewLabel;
        private System.Windows.Forms.Panel currentFilterView;
    }
}
