namespace MobileTimer
{
    partial class NewTimer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.ok = new System.Windows.Forms.MenuItem();
            this.cancel = new System.Windows.Forms.MenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button0 = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer();
            this.digit3 = new MobileTimer.BlockDigit();
            this.digit2 = new MobileTimer.BlockDigit();
            this.digit1 = new MobileTimer.BlockDigit();
            this.digit0 = new MobileTimer.BlockDigit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.ok);
            this.mainMenu1.MenuItems.Add(this.cancel);
            // 
            // ok
            // 
            this.ok.Text = "OK";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.Text = "&Cancel";
            this.cancel.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(5, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 48);
            this.button1.TabIndex = 1;
            this.button1.Text = "1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(86, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 48);
            this.button2.TabIndex = 2;
            this.button2.Text = "2";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(168, 45);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(70, 48);
            this.button3.TabIndex = 3;
            this.button3.Text = "3";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button6.Location = new System.Drawing.Point(168, 99);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(70, 48);
            this.button6.TabIndex = 6;
            this.button6.Text = "6";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button5.Location = new System.Drawing.Point(86, 99);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(70, 48);
            this.button5.TabIndex = 5;
            this.button5.Text = "5";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button4.Location = new System.Drawing.Point(5, 99);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(70, 48);
            this.button4.TabIndex = 4;
            this.button4.Text = "4";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button9.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button9.Location = new System.Drawing.Point(168, 153);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(70, 48);
            this.button9.TabIndex = 9;
            this.button9.Text = "9";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button8.Location = new System.Drawing.Point(86, 153);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(70, 48);
            this.button8.TabIndex = 8;
            this.button8.Text = "8";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button7.Location = new System.Drawing.Point(5, 153);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(70, 48);
            this.button7.TabIndex = 7;
            this.button7.Text = "7";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button0
            // 
            this.button0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button0.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button0.Location = new System.Drawing.Point(86, 207);
            this.button0.Name = "button0";
            this.button0.Size = new System.Drawing.Size(70, 48);
            this.button0.TabIndex = 11;
            this.button0.Text = "0";
            this.button0.Click += new System.EventHandler(this.button0_Click);
            // 
            // clear
            // 
            this.clear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clear.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.clear.Location = new System.Drawing.Point(5, 207);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(70, 48);
            this.clear.TabIndex = 10;
            this.clear.Text = "CLR";
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // digit3
            // 
            this.digit3.BackColor = System.Drawing.Color.White;
            this.digit3.Digit = null;
            this.digit3.Location = new System.Drawing.Point(40, 4);
            this.digit3.Name = "digit3";
            this.digit3.Size = new System.Drawing.Size(35, 35);
            this.digit3.TabIndex = 12;
            // 
            // digit2
            // 
            this.digit2.BackColor = System.Drawing.Color.White;
            this.digit2.Digit = null;
            this.digit2.Location = new System.Drawing.Point(86, 4);
            this.digit2.Name = "digit2";
            this.digit2.Size = new System.Drawing.Size(35, 35);
            this.digit2.TabIndex = 13;
            // 
            // digit1
            // 
            this.digit1.BackColor = System.Drawing.Color.White;
            this.digit1.Digit = null;
            this.digit1.Location = new System.Drawing.Point(138, 4);
            this.digit1.Name = "digit1";
            this.digit1.Size = new System.Drawing.Size(35, 35);
            this.digit1.TabIndex = 14;
            // 
            // digit0
            // 
            this.digit0.BackColor = System.Drawing.Color.White;
            this.digit0.Digit = null;
            this.digit0.Location = new System.Drawing.Point(179, 4);
            this.digit0.Name = "digit0";
            this.digit0.Size = new System.Drawing.Size(35, 35);
            this.digit0.TabIndex = 15;
            // 
            // NewTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.digit0);
            this.Controls.Add(this.digit1);
            this.Controls.Add(this.digit2);
            this.Controls.Add(this.digit3);
            this.Controls.Add(this.button0);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "NewTimer";
            this.Text = "New Timer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewTimer_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem ok;
        private System.Windows.Forms.MenuItem cancel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button0;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Timer timer1;
        private BlockDigit digit3;
        private BlockDigit digit2;
        private BlockDigit digit1;
        private BlockDigit digit0;
    }
}

