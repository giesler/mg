namespace MobileTimer
{
    partial class BlockDigits
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.minute0 = new MobileTimer.BlockDigit();
            this.minute1 = new MobileTimer.BlockDigit();
            this.second0 = new MobileTimer.BlockDigit();
            this.second1 = new MobileTimer.BlockDigit();
            this.hour0 = new MobileTimer.BlockDigit();
            this.hour1 = new MobileTimer.BlockDigit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(53, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 33);
            this.label1.Text = ":";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular);
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(117, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 33);
            this.label2.Text = ":";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // minute0
            // 
            this.minute0.BackColor = System.Drawing.SystemColors.Window;
            this.minute0.Location = new System.Drawing.Point(94, 0);
            this.minute0.Name = "minute0";
            this.minute0.Size = new System.Drawing.Size(22, 39);
            this.minute0.TabIndex = 9;
            // 
            // minute1
            // 
            this.minute1.BackColor = System.Drawing.SystemColors.Window;
            this.minute1.Location = new System.Drawing.Point(66, 0);
            this.minute1.Name = "minute1";
            this.minute1.Size = new System.Drawing.Size(22, 39);
            this.minute1.TabIndex = 8;
            // 
            // second0
            // 
            this.second0.BackColor = System.Drawing.SystemColors.Window;
            this.second0.Location = new System.Drawing.Point(157, 0);
            this.second0.Name = "second0";
            this.second0.Size = new System.Drawing.Size(22, 39);
            this.second0.TabIndex = 7;
            // 
            // second1
            // 
            this.second1.BackColor = System.Drawing.SystemColors.Window;
            this.second1.Location = new System.Drawing.Point(129, 0);
            this.second1.Name = "second1";
            this.second1.Size = new System.Drawing.Size(22, 39);
            this.second1.TabIndex = 6;
            // 
            // hour0
            // 
            this.hour0.BackColor = System.Drawing.SystemColors.Window;
            this.hour0.Location = new System.Drawing.Point(31, 0);
            this.hour0.Name = "hour0";
            this.hour0.Size = new System.Drawing.Size(22, 39);
            this.hour0.TabIndex = 5;
            // 
            // hour1
            // 
            this.hour1.BackColor = System.Drawing.SystemColors.Window;
            this.hour1.Location = new System.Drawing.Point(3, 0);
            this.hour1.Name = "hour1";
            this.hour1.Size = new System.Drawing.Size(22, 39);
            this.hour1.TabIndex = 4;
            // 
            // BlockDigits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.minute0);
            this.Controls.Add(this.minute1);
            this.Controls.Add(this.second0);
            this.Controls.Add(this.second1);
            this.Controls.Add(this.hour0);
            this.Controls.Add(this.hour1);
            this.Name = "BlockDigits";
            this.Size = new System.Drawing.Size(185, 42);
            this.ResumeLayout(false);

        }

        #endregion

        private BlockDigit second0;
        private BlockDigit second1;
        private BlockDigit hour0;
        private BlockDigit hour1;
        private BlockDigit minute0;
        private BlockDigit minute1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
