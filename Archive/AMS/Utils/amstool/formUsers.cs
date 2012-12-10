using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace XMAdmin
{
	/// <summary>
	/// Summary description for formUsers.
	/// </summary>
	public class formUsers : CustomMdiChild
	{
		private System.Windows.Forms.Panel panelSearch;
		private System.Windows.Forms.ListView listUsers;
		private System.Windows.Forms.ColumnHeader columnLogin;
		private System.Windows.Forms.Button buttonAll;
		private System.Windows.Forms.Button buttonOnline;
		private System.Windows.Forms.GroupBox groupName;
		private System.Windows.Forms.TextBox textLogin;
		private System.Windows.Forms.Button buttonLogin;
		private System.Windows.Forms.ColumnHeader columnEmail;
		private System.Windows.Forms.ContextMenu contextUsers;
		private System.Windows.Forms.MenuItem menuIndexes;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public formUsers()
			: base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.panelSearch = new System.Windows.Forms.Panel();
			this.groupName = new System.Windows.Forms.GroupBox();
			this.textLogin = new System.Windows.Forms.TextBox();
			this.buttonLogin = new System.Windows.Forms.Button();
			this.buttonOnline = new System.Windows.Forms.Button();
			this.buttonAll = new System.Windows.Forms.Button();
			this.listUsers = new System.Windows.Forms.ListView();
			this.columnLogin = new System.Windows.Forms.ColumnHeader();
			this.columnEmail = new System.Windows.Forms.ColumnHeader();
			this.contextUsers = new System.Windows.Forms.ContextMenu();
			this.menuIndexes = new System.Windows.Forms.MenuItem();
			this.panelSearch.SuspendLayout();
			this.groupName.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelSearch
			// 
			this.panelSearch.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.groupName,
																					  this.buttonOnline,
																					  this.buttonAll});
			this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelSearch.DockPadding.All = 2;
			this.panelSearch.Location = new System.Drawing.Point(2, 2);
			this.panelSearch.Name = "panelSearch";
			this.panelSearch.Size = new System.Drawing.Size(500, 112);
			this.panelSearch.TabIndex = 0;
			// 
			// groupName
			// 
			this.groupName.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.textLogin,
																					this.buttonLogin});
			this.groupName.Location = new System.Drawing.Point(136, 16);
			this.groupName.Name = "groupName";
			this.groupName.Size = new System.Drawing.Size(200, 64);
			this.groupName.TabIndex = 2;
			this.groupName.TabStop = false;
			this.groupName.Text = "Find by Login";
			// 
			// textLogin
			// 
			this.textLogin.Location = new System.Drawing.Point(16, 26);
			this.textLogin.Name = "textLogin";
			this.textLogin.Size = new System.Drawing.Size(96, 20);
			this.textLogin.TabIndex = 1;
			this.textLogin.Text = "";
			// 
			// buttonLogin
			// 
			this.buttonLogin.Location = new System.Drawing.Point(128, 24);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(56, 24);
			this.buttonLogin.TabIndex = 2;
			this.buttonLogin.Text = "Go";
			this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
			// 
			// buttonOnline
			// 
			this.buttonOnline.Location = new System.Drawing.Point(16, 24);
			this.buttonOnline.Name = "buttonOnline";
			this.buttonOnline.Size = new System.Drawing.Size(96, 24);
			this.buttonOnline.TabIndex = 1;
			this.buttonOnline.Text = "Online Users";
			this.buttonOnline.Click += new System.EventHandler(this.buttonOnline_Click);
			// 
			// buttonAll
			// 
			this.buttonAll.Location = new System.Drawing.Point(16, 56);
			this.buttonAll.Name = "buttonAll";
			this.buttonAll.Size = new System.Drawing.Size(96, 24);
			this.buttonAll.TabIndex = 0;
			this.buttonAll.Text = "All Users";
			this.buttonAll.Click += new System.EventHandler(this.buttonAll_Click);
			// 
			// listUsers
			// 
			this.listUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnLogin,
																						this.columnEmail});
			this.listUsers.ContextMenu = this.contextUsers;
			this.listUsers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listUsers.FullRowSelect = true;
			this.listUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listUsers.Location = new System.Drawing.Point(2, 114);
			this.listUsers.Name = "listUsers";
			this.listUsers.Size = new System.Drawing.Size(500, 241);
			this.listUsers.TabIndex = 1;
			this.listUsers.View = System.Windows.Forms.View.Details;
			// 
			// columnLogin
			// 
			this.columnLogin.Text = "Login";
			this.columnLogin.Width = 104;
			// 
			// columnEmail
			// 
			this.columnEmail.Text = "Email";
			this.columnEmail.Width = 139;
			// 
			// contextUsers
			// 
			this.contextUsers.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuIndexes});
			// 
			// menuIndexes
			// 
			this.menuIndexes.Index = 0;
			this.menuIndexes.Text = "Indexes...";
			this.menuIndexes.Click += new System.EventHandler(this.menuIndexes_Click);
			// 
			// formUsers
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 357);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listUsers,
																		  this.panelSearch});
			this.DockPadding.All = 2;
			this.Name = "formUsers";
			this.Text = "Users";
			this.panelSearch.ResumeLayout(false);
			this.groupName.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Show the given list of users in the listview.
		/// </summary>
		/// <param name="u"></param>
		private void DisplayUsers(Users u)
		{
			//clear current listitem
			listUsers.Items.Clear();

			//add each user
			ListViewItem i;
			foreach(User user in u)
			{
				i = new ListViewItem(new string[] {
					user.Login,
					user.Email });
				i.Tag = user;
				listUsers.Items.Add(i);
			}
		}

		private void buttonOnline_Click(object sender, System.EventArgs e)
		{
			DisplayUsers(Users.FindByOnline());
		}

		private void buttonAll_Click(object sender, System.EventArgs e)
		{
			DisplayUsers(Users.FindAllUsers());
		}

		private void buttonLogin_Click(object sender, System.EventArgs e)
		{
			DisplayUsers(Users.FindByLogin(textLogin.Text));
		}

		private void menuIndexes_Click(object sender, System.EventArgs e)
		{
			if (listUsers.SelectedItems.Count < 1)
				return;

			User u = (User)listUsers.SelectedItems[0].Tag;
			formUserIndexes dlg = new formUserIndexes(u);
			dlg.ShowDialog();
		}
	}
}
