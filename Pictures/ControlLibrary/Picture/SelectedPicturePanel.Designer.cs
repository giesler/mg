namespace msn2.net.Pictures.Controls
{
    partial class SelectedPicturePanel
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
            this.taskLabel = new System.Windows.Forms.Label();
            this.taskList = new System.Windows.Forms.Panel();
            this.pictureDetailEditor = new msn2.net.Pictures.Controls.MultiPictureEdit();
            this.pictureStack1 = new msn2.net.Pictures.Controls.PictureStack();
            this.SuspendLayout();
// 
// taskLabel
// 
            this.taskLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.taskLabel.AutoSize = true;
            this.taskLabel.Location = new System.Drawing.Point(541, 4);
            this.taskLabel.Name = "taskLabel";
            this.taskLabel.Size = new System.Drawing.Size(34, 14);
            this.taskLabel.TabIndex = 2;
            this.taskLabel.Text = "Tasks";
// 
// taskList
// 
            this.taskList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.taskList.Location = new System.Drawing.Point(541, 24);
            this.taskList.Name = "taskList";
            this.taskList.Size = new System.Drawing.Size(152, 73);
            this.taskList.TabIndex = 3;
// 
// pictureDetailEditor
// 
            this.pictureDetailEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureDetailEditor.Location = new System.Drawing.Point(170, 4);
            this.pictureDetailEditor.Name = "pictureDetailEditor";
            this.pictureDetailEditor.Size = new System.Drawing.Size(364, 75);
            this.pictureDetailEditor.TabIndex = 4;
// 
// pictureStack1
// 
            this.pictureStack1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureStack1.Location = new System.Drawing.Point(4, 4);
            this.pictureStack1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.pictureStack1.Name = "pictureStack1";
            this.pictureStack1.Size = new System.Drawing.Size(159, 93);
            this.pictureStack1.SuspendPaint = false;
            this.pictureStack1.TabIndex = 0;
// 
// SelectedPicturePanel
// 
            this.Controls.Add(this.pictureDetailEditor);
            this.Controls.Add(this.taskList);
            this.Controls.Add(this.taskLabel);
            this.Controls.Add(this.pictureStack1);
            this.Name = "SelectedPicturePanel";
            this.Size = new System.Drawing.Size(696, 101);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureStack pictureStack1;
        private System.Windows.Forms.Label taskLabel;
        private System.Windows.Forms.Panel taskList;
        private MultiPictureEdit pictureDetailEditor;
    }
}
