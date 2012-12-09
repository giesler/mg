namespace WindowsApplication1
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.WinForms;

    /// <summary>
    ///    Summary description for StartupForm.
    /// </summary>
    public class StartupForm : System.WinForms.Form
    {
        /// <summary>
        ///    Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components;
		private System.WinForms.NumericUpDown MaxPlayersUpDown;
		private System.WinForms.Label MaxPlayersLabel;
		private System.WinForms.TextBox PrivatePasswordBox;
		private System.WinForms.CheckBox checkBox1;
		private System.WinForms.Button JoinButton;
		private System.WinForms.ListView ServersListView;
		private System.WinForms.ColumnHeader NameColumn;
		private System.WinForms.ColumnHeader PrivateColumn;
		private System.WinForms.ColumnHeader MaxPlayersColumn;
		private System.WinForms.ColumnHeader CurrentPlayersColumn;
		private System.WinForms.Button HostButton;
		private System.WinForms.GroupBox JoinBox;
		private System.WinForms.GroupBox HostBox;

        public StartupForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
#region FormDesigner Code

        /// <summary>
        ///    Clean up any resources being used.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            components.Dispose();
        }

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container ();
			this.ServersListView = new System.WinForms.ListView ();
			this.MaxPlayersUpDown = new System.WinForms.NumericUpDown ();
			this.HostBox = new System.WinForms.GroupBox ();
			this.PrivateColumn = new System.WinForms.ColumnHeader ();
			this.JoinButton = new System.WinForms.Button ();
			this.JoinBox = new System.WinForms.GroupBox ();
			this.HostButton = new System.WinForms.Button ();
			this.NameColumn = new System.WinForms.ColumnHeader ();
			this.checkBox1 = new System.WinForms.CheckBox ();
			this.PrivatePasswordBox = new System.WinForms.TextBox ();
			this.MaxPlayersLabel = new System.WinForms.Label ();
			this.CurrentPlayersColumn = new System.WinForms.ColumnHeader ();
			this.MaxPlayersColumn = new System.WinForms.ColumnHeader ();
			MaxPlayersUpDown.BeginInit ();
			//@this.TrayHeight = 0;
			//@this.TrayLargeIcon = false;
			//@this.TrayAutoArrange = true;
			ServersListView.Location = new System.Drawing.Point (16, 24);
			ServersListView.Size = new System.Drawing.Size (352, 97);
			ServersListView.View = System.WinForms.View.Report;
			ServersListView.ForeColor = System.Drawing.SystemColors.WindowText;
			ServersListView.TabIndex = 8;
			ServersListView.Columns.All = new System.WinForms.ColumnHeader[4] {this.NameColumn, this.PrivateColumn, this.MaxPlayersColumn, this.CurrentPlayersColumn};
			MaxPlayersUpDown.Value = new decimal (16);
			MaxPlayersUpDown.Location = new System.Drawing.Point (136, 56);
			MaxPlayersUpDown.Size = new System.Drawing.Size (48, 20);
			MaxPlayersUpDown.TabIndex = 5;
			HostBox.Location = new System.Drawing.Point (16, 16);
			HostBox.TabIndex = 6;
			HostBox.TabStop = false;
			HostBox.Text = "Host";
			HostBox.Size = new System.Drawing.Size (384, 88);
			PrivateColumn.Text = "Private";
			PrivateColumn.TextAlign = System.WinForms.HorizontalAlignment.Left;
			JoinButton.Location = new System.Drawing.Point (280, 136);
			JoinButton.Size = new System.Drawing.Size (88, 24);
			JoinButton.TabIndex = 9;
			JoinButton.Text = "Join";
			JoinBox.Visible = false;
			JoinBox.Location = new System.Drawing.Point (16, 112);
			JoinBox.TabIndex = 7;
			JoinBox.TabStop = false;
			JoinBox.Text = "Join";
			JoinBox.Size = new System.Drawing.Size (384, 176);
			HostButton.Visible = false;
			HostButton.Location = new System.Drawing.Point (280, 48);
			HostButton.Size = new System.Drawing.Size (88, 24);
			HostButton.TabIndex = 1;
			HostButton.Text = "Host";
			NameColumn.Text = "Name";
			NameColumn.Width = 100;
			NameColumn.TextAlign = System.WinForms.HorizontalAlignment.Left;
			checkBox1.Location = new System.Drawing.Point (24, 24);
			checkBox1.Text = "Private";
			checkBox1.Size = new System.Drawing.Size (72, 16);
			checkBox1.TabIndex = 2;
			PrivatePasswordBox.Location = new System.Drawing.Point (104, 24);
			PrivatePasswordBox.PasswordChar = '*';
			PrivatePasswordBox.TabIndex = 3;
			PrivatePasswordBox.Size = new System.Drawing.Size (112, 20);
			MaxPlayersLabel.Location = new System.Drawing.Point (32, 60);
			MaxPlayersLabel.Text = "Maximum Players";
			MaxPlayersLabel.Size = new System.Drawing.Size (104, 16);
			MaxPlayersLabel.TabIndex = 4;
			CurrentPlayersColumn.Text = "Current Players";
			CurrentPlayersColumn.Width = 90;
			CurrentPlayersColumn.TextAlign = System.WinForms.HorizontalAlignment.Left;
			MaxPlayersColumn.Text = "Max Players";
			MaxPlayersColumn.Width = 80;
			MaxPlayersColumn.TextAlign = System.WinForms.HorizontalAlignment.Left;
			this.Text = "Cards";
			this.MaximizeBox = false;
			this.StartPosition = System.WinForms.FormStartPosition.CenterScreen;
			this.AutoScaleBaseSize = new System.Drawing.Size (5, 13);
			this.BorderStyle = System.WinForms.FormBorderStyle.FixedDialog;
			this.MinimizeBox = false;
			this.ClientSize = new System.Drawing.Size (410, 303);
			HostBox.Controls.Add (this.MaxPlayersUpDown);
			HostBox.Controls.Add (this.MaxPlayersLabel);
			HostBox.Controls.Add (this.PrivatePasswordBox);
			HostBox.Controls.Add (this.checkBox1);
			HostBox.Controls.Add (this.HostButton);
			this.Controls.Add (this.JoinBox);
			this.Controls.Add (this.HostBox);
			JoinBox.Controls.Add (this.JoinButton);
			JoinBox.Controls.Add (this.ServersListView);
			MaxPlayersUpDown.EndInit ();
		}
#endregion
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args) 
        {
            Application.Run(new StartupForm());
        }
    }
}
