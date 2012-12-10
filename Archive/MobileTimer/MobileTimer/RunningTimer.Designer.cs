namespace MobileTimer
{
    partial class RunningTimer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progress = new System.Windows.Forms.ProgressBar();
            this.startStop = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer();
            this.delete = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.blockDigits1 = new MobileTimer.BlockDigits();
            this.SuspendLayout();
            // 
            // progress
            // 
            this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progress.Location = new System.Drawing.Point(4, 51);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(210, 10);
            // 
            // startStop
            // 
            this.startStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startStop.Location = new System.Drawing.Point(220, 4);
            this.startStop.Name = "startStop";
            this.startStop.Size = new System.Drawing.Size(38, 42);
            this.startStop.TabIndex = 2;
            this.startStop.Text = "start";
            this.startStop.Click += new System.EventHandler(this.startStop_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.delete.Location = new System.Drawing.Point(220, 51);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(38, 10);
            this.delete.TabIndex = 4;
            this.delete.Text = "delete";
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Enabled = false;
            this.splitter1.Location = new System.Drawing.Point(0, 64);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(261, 3);
            // 
            // blockDigits1
            // 
            this.blockDigits1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.blockDigits1.BackColor = System.Drawing.SystemColors.Window;
            this.blockDigits1.Location = new System.Drawing.Point(4, 4);
            this.blockDigits1.Name = "blockDigits1";
            this.blockDigits1.Size = new System.Drawing.Size(210, 42);
            this.blockDigits1.TabIndex = 0;
            // 
            // RunningTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.startStop);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.blockDigits1);
            this.Name = "RunningTimer";
            this.Size = new System.Drawing.Size(261, 67);
            this.ResumeLayout(false);

        }

        #endregion

        private BlockDigits blockDigits1;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Button startStop;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Splitter splitter1;
    }
}
