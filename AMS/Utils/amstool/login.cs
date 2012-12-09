using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace XMAdmin
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class formLogin : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.Button buttonMore;
		private System.Windows.Forms.TextBox textUsername;
		private System.Windows.Forms.TextBox textServer;
		private System.Windows.Forms.TextBox textDatabase;
		private System.Windows.Forms.Label labelInstructions;
		private System.Windows.Forms.Label labelUsername;
		private System.Windows.Forms.Label labelServer;
		private System.Windows.Forms.Label labelDatabase;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public formLogin()
		{
			//Required for Windows Form Designer support
			InitializeComponent();

			//load fields from config
			textUsername.Text = Config.Values[ConfigValues.ServerUsername];
			textServer.Text = Config.Values[ConfigValues.ServerServer];
			textDatabase.Text = Config.Values[ConfigValues.ServerDatabase];
			textPassword.Text = Config.Values[ConfigValues.ServerPassword];
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonMore = new System.Windows.Forms.Button();
			this.labelInstructions = new System.Windows.Forms.Label();
			this.textServer = new System.Windows.Forms.TextBox();
			this.labelPassword = new System.Windows.Forms.Label();
			this.labelUsername = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.labelServer = new System.Windows.Forms.Label();
			this.textDatabase = new System.Windows.Forms.TextBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.textUsername = new System.Windows.Forms.TextBox();
			this.labelDatabase = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonMore
			// 
			this.buttonMore.Location = new System.Drawing.Point(240, 80);
			this.buttonMore.Name = "buttonMore";
			this.buttonMore.Size = new System.Drawing.Size(88, 24);
			this.buttonMore.TabIndex = 5;
			this.buttonMore.Text = "More >>";
			this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
			// 
			// labelInstructions
			// 
			this.labelInstructions.Location = new System.Drawing.Point(8, 16);
			this.labelInstructions.Name = "labelInstructions";
			this.labelInstructions.Size = new System.Drawing.Size(216, 32);
			this.labelInstructions.TabIndex = 0;
			this.labelInstructions.Text = "Enter a server, database, username and password to connect to SQL Server.";
			// 
			// textServer
			// 
			this.textServer.Location = new System.Drawing.Point(80, 112);
			this.textServer.MaxLength = 32;
			this.textServer.Name = "textServer";
			this.textServer.Size = new System.Drawing.Size(128, 20);
			this.textServer.TabIndex = 6;
			this.textServer.Text = "";
			this.textServer.Visible = false;
			// 
			// labelPassword
			// 
			this.labelPassword.Location = new System.Drawing.Point(8, 92);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(72, 16);
			this.labelPassword.TabIndex = 2;
			this.labelPassword.Text = "Password";
			// 
			// labelUsername
			// 
			this.labelUsername.Location = new System.Drawing.Point(8, 68);
			this.labelUsername.Name = "labelUsername";
			this.labelUsername.Size = new System.Drawing.Size(72, 16);
			this.labelUsername.TabIndex = 2;
			this.labelUsername.Text = "Username";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(240, 48);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(88, 24);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(80, 88);
			this.textPassword.MaxLength = 32;
			this.textPassword.Name = "textPassword";
			this.textPassword.PasswordChar = '*';
			this.textPassword.Size = new System.Drawing.Size(128, 20);
			this.textPassword.TabIndex = 2;
			this.textPassword.Text = "";
			// 
			// labelServer
			// 
			this.labelServer.Location = new System.Drawing.Point(8, 116);
			this.labelServer.Name = "labelServer";
			this.labelServer.Size = new System.Drawing.Size(72, 16);
			this.labelServer.TabIndex = 2;
			this.labelServer.Text = "Server";
			this.labelServer.Visible = false;
			// 
			// textDatabase
			// 
			this.textDatabase.Location = new System.Drawing.Point(80, 136);
			this.textDatabase.MaxLength = 32;
			this.textDatabase.Name = "textDatabase";
			this.textDatabase.Size = new System.Drawing.Size(128, 20);
			this.textDatabase.TabIndex = 7;
			this.textDatabase.Text = "";
			this.textDatabase.Visible = false;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(240, 16);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(88, 24);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// textUsername
			// 
			this.textUsername.Location = new System.Drawing.Point(80, 64);
			this.textUsername.MaxLength = 32;
			this.textUsername.Name = "textUsername";
			this.textUsername.Size = new System.Drawing.Size(128, 20);
			this.textUsername.TabIndex = 1;
			this.textUsername.Text = "";
			// 
			// labelDatabase
			// 
			this.labelDatabase.Location = new System.Drawing.Point(8, 140);
			this.labelDatabase.Name = "labelDatabase";
			this.labelDatabase.Size = new System.Drawing.Size(72, 16);
			this.labelDatabase.TabIndex = 2;
			this.labelDatabase.Text = "Database";
			this.labelDatabase.Visible = false;
			// 
			// formLogin
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(344, 123);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.labelDatabase,
																		  this.textDatabase,
																		  this.labelServer,
																		  this.textServer,
																		  this.labelUsername,
																		  this.textUsername,
																		  this.buttonMore,
																		  this.textPassword,
																		  this.labelPassword,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.labelInstructions});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "formLogin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "XMAdmin Login";
			this.Activated += new System.EventHandler(this.formLogin_Activated);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			//load settings
			try
			{
				//load initial settings
				Config.Reset();

				//check for config file
				if (File.Exists(Config.Path))
					Config.Load();
			}
			catch(Exception e)
			{	
				MessageBox.Show(null, "Error loading configuration data:\n"+e.Message,
					"Config Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//load pictures
			try
			{
				//check for data file
				if (File.Exists(Pictures.Path))
					Pictures.Load();
			}
			catch(Exception e)
			{
				MessageBox.Show(null, "Error loading picture data:\n"+e.Message,
					"Picture Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//show the login window
			formLogin dlg = new formLogin();
			if (dlg.ShowDialog() == DialogResult.Cancel)
			{
				Config.Save();
				return;
			}
			dlg = null;

			//show the main window
			Application.Run(new formMain());

			//save config values
			Config.Save();
			Pictures.Save();
		}

		private void buttonMore_Click(object sender, System.EventArgs e)
		{
			//increate the form size
			this.Height = 200;

			//hide this button
			buttonMore.Hide();

			//show server, database controls
			labelServer.Show();
			labelDatabase.Show();
			textServer.Show();
			textDatabase.Show();
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			//update the config fields
			Config.Values[ConfigValues.ServerUsername] = textUsername.Text;
			Config.Values[ConfigValues.ServerPassword] = textPassword.Text;
			Config.Values[ConfigValues.ServerServer] = textServer.Text;
			Config.Values[ConfigValues.ServerDatabase] = textDatabase.Text;

			//try to connection first
			try
			{
				Data.Connect();
			}
			catch(Exception ex)
			{
				//show the error, but don't close the dialog box
				MessageBox.Show(this, "Error connecting to server:\n"+ex.Message,
					"Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//success
			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			//cancel pressed
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void formLogin_Activated(object sender, System.EventArgs e)
		{
			//if server or database is blank, expand
			if (textServer.Text.Length < 1 || textDatabase.Text.Length < 1)
			{
				buttonMore_Click(this, null);
				textServer.Focus();
			}
			else
			{
				//if the username field is blank, focus on it
				if (textUsername.Text.Length < 1)
					textUsername.Focus();
				else
					textPassword.Focus();
			}
		}
	}
}