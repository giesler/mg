namespace msn2.net.Pictures
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.label1 = new System.Windows.Forms.Label();
            this.interval = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.path = new System.Windows.Forms.TextBox();
            this.changePath = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.group = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.interval)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Change picture every";
            // 
            // interval
            // 
            this.interval.Location = new System.Drawing.Point(164, 9);
            this.interval.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.interval.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.interval.Name = "interval";
            this.interval.Size = new System.Drawing.Size(52, 20);
            this.interval.TabIndex = 1;
            this.interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.interval.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "seconds";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(250, 128);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 3;
            this.ok.Text = "&OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(334, 128);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Show pictures under:";
            // 
            // path
            // 
            this.path.Location = new System.Drawing.Point(164, 35);
            this.path.Name = "path";
            this.path.ReadOnly = true;
            this.path.Size = new System.Drawing.Size(164, 20);
            this.path.TabIndex = 6;
            // 
            // changePath
            // 
            this.changePath.Location = new System.Drawing.Point(334, 35);
            this.changePath.Name = "changePath";
            this.changePath.Size = new System.Drawing.Size(75, 23);
            this.changePath.TabIndex = 7;
            this.changePath.Text = "Change";
            this.changePath.UseVisualStyleBackColor = true;
            this.changePath.Click += new System.EventHandler(this.changePath_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Show pictures shared with:";
            // 
            // group
            // 
            this.group.BackColor = System.Drawing.SystemColors.Window;
            this.group.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.group.FormattingEnabled = true;
            this.group.Location = new System.Drawing.Point(164, 62);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(164, 21);
            this.group.TabIndex = 9;
            // 
            // Options
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(421, 158);
            this.Controls.Add(this.group);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.changePath);
            this.Controls.Add(this.path);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.interval);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "msn2 Picture Screen Saver Options";
            ((System.ComponentModel.ISupportInitialize)(this.interval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown interval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox path;
        private System.Windows.Forms.Button changePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox group;
    }
}