using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using msn2.net.Configuration;
using msn2.net.Controls;

namespace msn2.net.ProjectF
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	public class Login : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonLogin;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ListView listViewUsers;
		private System.Windows.Forms.Button buttonNew;
		private System.Windows.Forms.Button buttonNewConfig;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListView listViewConfig;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Login()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			LoadLogins();
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
			this.listViewUsers = new System.Windows.Forms.ListView();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonLogin = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonNew = new System.Windows.Forms.Button();
			this.listViewConfig = new System.Windows.Forms.ListView();
			this.buttonNewConfig = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// listViewUsers
			// 
			this.listViewUsers.FullRowSelect = true;
			this.listViewUsers.HideSelection = false;
			this.listViewUsers.Location = new System.Drawing.Point(16, 32);
			this.listViewUsers.MultiSelect = false;
			this.listViewUsers.Name = "listViewUsers";
			this.listViewUsers.Size = new System.Drawing.Size(264, 88);
			this.listViewUsers.TabIndex = 5;
			this.listViewUsers.SelectedIndexChanged += new System.EventHandler(this.listViewUsers_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "You are:";
			// 
			// buttonLogin
			// 
			this.buttonLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonLogin.Location = new System.Drawing.Point(120, 296);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(72, 24);
			this.buttonLogin.TabIndex = 7;
			this.buttonLogin.Text = "login";
			this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(200, 296);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(72, 24);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonNew
			// 
			this.buttonNew.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNew.Location = new System.Drawing.Point(232, 120);
			this.buttonNew.Name = "buttonNew";
			this.buttonNew.Size = new System.Drawing.Size(48, 16);
			this.buttonNew.TabIndex = 9;
			this.buttonNew.Text = "new";
			this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
			// 
			// listViewConfig
			// 
			this.listViewConfig.FullRowSelect = true;
			this.listViewConfig.Location = new System.Drawing.Point(16, 160);
			this.listViewConfig.MultiSelect = false;
			this.listViewConfig.Name = "listViewConfig";
			this.listViewConfig.Size = new System.Drawing.Size(264, 96);
			this.listViewConfig.TabIndex = 10;
			// 
			// buttonNewConfig
			// 
			this.buttonNewConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNewConfig.Location = new System.Drawing.Point(232, 256);
			this.buttonNewConfig.Name = "buttonNewConfig";
			this.buttonNewConfig.Size = new System.Drawing.Size(48, 16);
			this.buttonNewConfig.TabIndex = 11;
			this.buttonNewConfig.Text = "new";
			this.buttonNewConfig.Click += new System.EventHandler(this.buttonNewConfig_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(256, 16);
			this.label2.TabIndex = 12;
			this.label2.Text = "Load config:";
			// 
			// Login
			// 
			this.AcceptButton = this.buttonLogin;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(300, 336);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label2,
																		  this.buttonNewConfig,
																		  this.listViewConfig,
																		  this.buttonNew,
																		  this.buttonCancel,
																		  this.buttonLogin,
																		  this.label1,
																		  this.listViewUsers});
			this.Name = "Login";
			this.Text = "Project F Login";
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadLogins()
		{
            SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_User_List", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;

			listViewUsers.Items.Clear();

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			
			// Add listview items for each login
			while (dr.Read())
			{
				listViewUsers.Items.Add(new UserListViewItem((Guid) dr["UserId"], dr["UserName"].ToString()));
			}

			dr.Close();
			cn.Close();
		}

		private void buttonNew_Click(object sender, System.EventArgs e)
		{
            InputPrompt prompt = new InputPrompt("Enter a user name");
			prompt.Dialog = true;

			while (prompt.ShowShellDialog(this) != DialogResult.Cancel)
			{
				bool inUse = false;

				// check if user already in listview
				foreach (UserListViewItem userItem in listViewUsers.Items)
				{
					if (userItem.Text == prompt.Value)
						inUse = true;
				}

				if (!inUse)
					break;

				MessageBox.Show(this, "The name you entered is already in use.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}

			if (prompt.DialogResult == DialogResult.Cancel)
				return;

			SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_User_Add", cn);
			cmd.CommandType  = CommandType.StoredProcedure;

			cmd.Parameters.Add("@userName", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			cmd.Parameters["@userId"].Direction = ParameterDirection.Output;

			cmd.Parameters["@userName"].Value = prompt.Value;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			LoadLogins();

			Guid userId = (Guid) cmd.Parameters["@userId"].Value;

			foreach (UserListViewItem userItem in listViewUsers.Items)
			{
				if (userItem.UserId == userId)
				{
					userItem.Selected = true;
					break;
				}
			}

			
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
		}

		private void buttonLogin_Click(object sender, System.EventArgs e)
		{
			if (listViewUsers.SelectedItems.Count == 0)
				return;

			UserListViewItem userItem = (UserListViewItem) listViewUsers.SelectedItems[0];

			if (listViewConfig.SelectedItems.Count == 0)
				return;

			ConfigurationListViewItem configItem = (ConfigurationListViewItem) listViewConfig.SelectedItems[0];
            
			//ConfigurationSettings.Current.Login(userItem.UserId, configItem.ConfigId);

			this.Visible = false;
		}

		private void buttonNewConfig_Click(object sender, System.EventArgs e)
		{
			if (listViewUsers.SelectedItems.Count == 0)
				return;

			UserListViewItem userItem = (UserListViewItem) listViewUsers.SelectedItems[0];

			InputPrompt prompt = new InputPrompt("Enter a config name");
			prompt.Dialog = true;

			while (prompt.ShowShellDialog(this) != DialogResult.Cancel)
			{
				bool inUse = false;

//				// check if user already in listview
//				foreach (ConfigurationListViewItem configItem in listViewConfig.Items)
//				{
//					if (userItem.Text == prompt.Value)
//						inUse = true;
//				}

				if (!inUse)
					break;

				MessageBox.Show(this, "The name you entered is already in use.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}

			if (prompt.DialogResult == DialogResult.Cancel)
				return;

			SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_Configuration_Add", cn);
			cmd.CommandType  = CommandType.StoredProcedure;

			cmd.Parameters.Add("@configurationName", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
			cmd.Parameters.Add("@configurationId", SqlDbType.UniqueIdentifier);
			cmd.Parameters["@configurationId"].Direction = ParameterDirection.Output;

			cmd.Parameters["@configurationName"].Value = prompt.Value;
			cmd.Parameters["@userId"].Value		= userItem.UserId;

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			LoadConfigs();

			Guid configId = (Guid) cmd.Parameters["@configurationId"].Value;

			foreach (ConfigurationListViewItem configItem in listViewConfig.Items)
			{
				if (configItem.ConfigId == configId)
				{
					configItem.Selected = true;
					break;
				}
			}		
		}

		private void listViewUsers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			LoadConfigs();
		}

		private void LoadConfigs()
		{
			// verify something is selected
			if (listViewUsers.SelectedItems.Count == 0)
				return;

			UserListViewItem userItem = (UserListViewItem) listViewUsers.SelectedItems[0];

			SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_Configuration_List", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;

			cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier);
            cmd.Parameters["@userId"].Value = userItem.UserId;

			listViewConfig.Items.Clear();

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			
			// Add listview items for each login
			while (dr.Read())
			{
				listViewConfig.Items.Add(new ConfigurationListViewItem((Guid) dr["ConfigurationId"], dr["ConfigurationName"].ToString()));
			}

			dr.Close();
			cn.Close();
			
		}

		private class UserListViewItem: ListViewItem
		{
			private Guid userId;

			public UserListViewItem(Guid userId, string userName)
			{
				this.userId = userId;

				this.Text = userName;				
			}

			public Guid UserId
			{
				get { return userId; }
			}
		}

		private class ConfigurationListViewItem: ListViewItem
		{
			private Guid configId;

			public ConfigurationListViewItem(Guid configId, string configName)
			{
				this.Text = configName;
				this.configId = configId;
			}

			public Guid ConfigId
			{
				get { return configId; }
			}
		}
	}
}
