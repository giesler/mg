namespace mn2.net.ShoppingList
{
    partial class SettingsDialog
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
            this.password = new System.Windows.Forms.Label();
            this.pin = new System.Windows.Forms.TextBox();
            this.deviceNameLabel = new System.Windows.Forms.Label();
            this.deviceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(4, 9);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.Text = "House PIN:";
            // 
            // pin
            // 
            this.pin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pin.Location = new System.Drawing.Point(4, 33);
            this.pin.Name = "pin";
            this.pin.PasswordChar = '*';
            this.pin.Size = new System.Drawing.Size(233, 21);
            this.pin.TabIndex = 3;
            this.pin.TextChanged += new System.EventHandler(this.pin_TextChanged);
            // 
            // deviceNameLabel
            // 
            this.deviceNameLabel.Location = new System.Drawing.Point(4, 71);
            this.deviceNameLabel.Name = "deviceNameLabel";
            this.deviceNameLabel.Size = new System.Drawing.Size(100, 20);
            this.deviceNameLabel.Text = "Device Name";
            // 
            // deviceName
            // 
            this.deviceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceName.Location = new System.Drawing.Point(4, 95);
            this.deviceName.Name = "deviceName";
            this.deviceName.ReadOnly = true;
            this.deviceName.Size = new System.Drawing.Size(233, 21);
            this.deviceName.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(4, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 53);
            this.label1.Text = "To change: \r\nStart -> Settings -> System -> About";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deviceName);
            this.Controls.Add(this.deviceNameLabel);
            this.Controls.Add(this.pin);
            this.Controls.Add(this.password);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem ok;
        private System.Windows.Forms.MenuItem cancel;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.TextBox pin;
        private System.Windows.Forms.Label deviceNameLabel;
        private System.Windows.Forms.TextBox deviceName;
        private System.Windows.Forms.Label label1;
    }
}