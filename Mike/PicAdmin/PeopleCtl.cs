using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace msn2.net.Pictures.Controls
{

	/// <summary>
	/// Summary description for PeopleCtl.
	/// </summary>
	public class PeopleCtl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Data.DataView dvPersonFullName;
		private System.Data.DataView dvPersonLastName;
		private System.Data.DataView dvPersonFirstName;
		private System.Windows.Forms.MenuItem menuAddPerson;
		private System.Windows.Forms.MenuItem menuEditPerson;
		private System.Windows.Forms.MenuItem menuDeletePerson;
		private msn2.net.Pictures.Controls.DataSetPerson dsPerson;
		private System.Data.DataView dvPersonFind;
		private System.Data.SqlClient.SqlDataAdapter daPerson;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Data.SqlClient.SqlConnection sqlConnection1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TextBox findString;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TreeView tvBrowse;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TreeView tvGroups;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.ListView lvFind;
		private System.Windows.Forms.ListView lvBrowse;
		private System.Windows.Forms.ListView lvGroups;
		private System.Windows.Forms.TabPage tabPageBrowse;
		private System.Windows.Forms.TabPage tabPageGroups;
		private System.Windows.Forms.TabPage tabPageFind;


		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public PeopleCtl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			try
			{
				// Set the connection string
				this.sqlConnection1.ConnectionString	= Config.ConnectionString;
				this.cn.ConnectionString				= Config.ConnectionString;

				// Load all people
				daPerson.Fill(dsPerson, "Person");
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message);

			}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.dvPersonFullName = new System.Data.DataView();
			this.dsPerson = new msn2.net.Pictures.Controls.DataSetPerson();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuAddPerson = new System.Windows.Forms.MenuItem();
			this.menuEditPerson = new System.Windows.Forms.MenuItem();
			this.menuDeletePerson = new System.Windows.Forms.MenuItem();
			this.dvPersonFirstName = new System.Data.DataView();
			this.dvPersonLastName = new System.Data.DataView();
			this.dvPersonFind = new System.Data.DataView();
			this.daPerson = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageFind = new System.Windows.Forms.TabPage();
			this.lvFind = new System.Windows.Forms.ListView();
			this.findString = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.tabPageBrowse = new System.Windows.Forms.TabPage();
			this.lvBrowse = new System.Windows.Forms.ListView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.tvBrowse = new System.Windows.Forms.TreeView();
			this.tabPageGroups = new System.Windows.Forms.TabPage();
			this.lvGroups = new System.Windows.Forms.ListView();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.tvGroups = new System.Windows.Forms.TreeView();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFullName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFirstName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonLastName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFind)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPageFind.SuspendLayout();
			this.tabPageBrowse.SuspendLayout();
			this.tabPageGroups.SuspendLayout();
			this.SuspendLayout();
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// dvPersonFullName
			// 
			this.dvPersonFullName.RowFilter = "FullName IS NOT NULL";
			this.dvPersonFullName.Sort = "FullName";
			this.dvPersonFullName.Table = this.dsPerson.Person;
			// 
			// dsPerson
			// 
			this.dsPerson.DataSetName = "DataSetPicture";
			this.dsPerson.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuAddPerson,
																						 this.menuEditPerson,
																						 this.menuDeletePerson});
			// 
			// menuAddPerson
			// 
			this.menuAddPerson.Index = 0;
			this.menuAddPerson.Text = "&Add Person";
			this.menuAddPerson.Click += new System.EventHandler(this.menuAddPerson_Click);
			// 
			// menuEditPerson
			// 
			this.menuEditPerson.Index = 1;
			this.menuEditPerson.Text = "&Edit Person";
			this.menuEditPerson.Click += new System.EventHandler(this.menuEditPerson_Click);
			// 
			// menuDeletePerson
			// 
			this.menuDeletePerson.Index = 2;
			this.menuDeletePerson.Text = "&Delete Person";
			this.menuDeletePerson.Click += new System.EventHandler(this.menuDeletePerson_Click);
			// 
			// dvPersonFirstName
			// 
			this.dvPersonFirstName.RowFilter = "FirstName IS NOT NULL";
			this.dvPersonFirstName.Sort = "FirstName";
			this.dvPersonFirstName.Table = this.dsPerson.Person;
			// 
			// dvPersonLastName
			// 
			this.dvPersonLastName.RowFilter = "LastName IS NOT NULL";
			this.dvPersonLastName.Sort = "LastName, FirstName";
			this.dvPersonLastName.Table = this.dsPerson.Person;
			// 
			// dvPersonFind
			// 
			this.dvPersonFind.Table = this.dsPerson.Person;
			// 
			// daPerson
			// 
			this.daPerson.DeleteCommand = this.sqlDeleteCommand1;
			this.daPerson.InsertCommand = this.sqlInsertCommand1;
			this.daPerson.SelectCommand = this.sqlSelectCommand1;
			this.daPerson.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							   new System.Data.Common.DataTableMapping("Table", "Person", new System.Data.Common.DataColumnMapping[] {
																																																		 new System.Data.Common.DataColumnMapping("PersonID", "PersonID"),
																																																		 new System.Data.Common.DataColumnMapping("LastName", "LastName"),
																																																		 new System.Data.Common.DataColumnMapping("FirstName", "FirstName"),
																																																		 new System.Data.Common.DataColumnMapping("FullName", "FullName")})});
			this.daPerson.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = @"DELETE FROM Person WHERE (PersonID = @PersonID) AND (FirstName = @FirstName OR @FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @FullName OR @FullName1 IS NULL AND FullName IS NULL) AND (LastName = @LastName OR @LastName1 IS NULL AND LastName IS NULL)";
			this.sqlDeleteCommand1.Connection = this.sqlConnection1;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			// 
			// sqlConnection1
			// 
			this.sqlConnection1.ConnectionString = "data source=kyle;integrated security=sspi;initial catalog=picdb;persist security " +
				"info=False";
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO Person(LastName, FirstName, FullName) VALUES (@LastName, @FirstName, " +
				"@FullName); SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (Pe" +
				"rsonID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.sqlConnection1;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT PersonID, LastName, FirstName, FullName FROM Person";
			this.sqlSelectCommand1.Connection = this.sqlConnection1;
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Person SET LastName = @LastName, FirstName = @FirstName, FullName = @FullName WHERE (PersonID = @Original_PersonID) AND (FirstName = @Original_FirstName OR @Original_FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @Original_FullName OR @Original_FullName1 IS NULL AND FullName IS NULL) AND (LastName = @Original_LastName OR @Original_LastName1 IS NULL AND LastName IS NULL); SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (PersonID = @Select_PersonID)";
			this.sqlUpdateCommand1.Connection = this.sqlConnection1;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, "PersonID"));
			// 
			// tabControl1
			// 
			this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControl1.Controls.Add(this.tabPageFind);
			this.tabControl1.Controls.Add(this.tabPageBrowse);
			this.tabControl1.Controls.Add(this.tabPageGroups);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(352, 288);
			this.tabControl1.TabIndex = 4;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPageFind
			// 
			this.tabPageFind.Controls.Add(this.lvFind);
			this.tabPageFind.Controls.Add(this.findString);
			this.tabPageFind.Controls.Add(this.button1);
			this.tabPageFind.Location = new System.Drawing.Point(4, 4);
			this.tabPageFind.Name = "tabPageFind";
			this.tabPageFind.Size = new System.Drawing.Size(344, 262);
			this.tabPageFind.TabIndex = 2;
			this.tabPageFind.Text = "Find";
			// 
			// lvFind
			// 
			this.lvFind.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lvFind.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvFind.HideSelection = false;
			this.lvFind.Location = new System.Drawing.Point(0, 32);
			this.lvFind.MultiSelect = false;
			this.lvFind.Name = "lvFind";
			this.lvFind.Size = new System.Drawing.Size(344, 230);
			this.lvFind.TabIndex = 7;
			this.lvFind.View = System.Windows.Forms.View.List;
			this.lvFind.DoubleClick += new System.EventHandler(this.lvFind_DoubleClick);
			this.lvFind.SelectedIndexChanged += new System.EventHandler(this.lvFind_SelectedIndexChanged);
			// 
			// findString
			// 
			this.findString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.findString.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.findString.Location = new System.Drawing.Point(0, 8);
			this.findString.Name = "findString";
			this.findString.Size = new System.Drawing.Size(288, 20);
			this.findString.TabIndex = 4;
			this.findString.Text = "<enter name>";
			this.findString.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.findString_KeyPress);
			this.findString.Enter += new System.EventHandler(this.findString_Enter);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button1.Location = new System.Drawing.Point(299, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(40, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Find";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tabPageBrowse
			// 
			this.tabPageBrowse.Controls.Add(this.lvBrowse);
			this.tabPageBrowse.Controls.Add(this.splitter1);
			this.tabPageBrowse.Controls.Add(this.tvBrowse);
			this.tabPageBrowse.Location = new System.Drawing.Point(4, 4);
			this.tabPageBrowse.Name = "tabPageBrowse";
			this.tabPageBrowse.Size = new System.Drawing.Size(344, 262);
			this.tabPageBrowse.TabIndex = 0;
			this.tabPageBrowse.Text = "Browse";
			// 
			// lvBrowse
			// 
			this.lvBrowse.ContextMenu = this.contextMenu1;
			this.lvBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvBrowse.FullRowSelect = true;
			this.lvBrowse.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvBrowse.HideSelection = false;
			this.lvBrowse.Location = new System.Drawing.Point(107, 0);
			this.lvBrowse.MultiSelect = false;
			this.lvBrowse.Name = "lvBrowse";
			this.lvBrowse.Size = new System.Drawing.Size(237, 262);
			this.lvBrowse.TabIndex = 2;
			this.lvBrowse.View = System.Windows.Forms.View.List;
			this.lvBrowse.DoubleClick += new System.EventHandler(this.lvBrowse_DoubleClick);
			this.lvBrowse.SelectedIndexChanged += new System.EventHandler(this.lvBrowse_SelectedIndexChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(104, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 262);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// tvBrowse
			// 
			this.tvBrowse.Dock = System.Windows.Forms.DockStyle.Left;
			this.tvBrowse.ImageIndex = -1;
			this.tvBrowse.Location = new System.Drawing.Point(0, 0);
			this.tvBrowse.Name = "tvBrowse";
			this.tvBrowse.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																				 new System.Windows.Forms.TreeNode("Full Name", new System.Windows.Forms.TreeNode[] {
																																										new System.Windows.Forms.TreeNode("A-D"),
																																										new System.Windows.Forms.TreeNode("E-H"),
																																										new System.Windows.Forms.TreeNode("I-L"),
																																										new System.Windows.Forms.TreeNode("M-P"),
																																										new System.Windows.Forms.TreeNode("Q-T"),
																																										new System.Windows.Forms.TreeNode("U-Z")}),
																				 new System.Windows.Forms.TreeNode("First Name", new System.Windows.Forms.TreeNode[] {
																																										 new System.Windows.Forms.TreeNode("A-D"),
																																										 new System.Windows.Forms.TreeNode("E-H"),
																																										 new System.Windows.Forms.TreeNode("I-L"),
																																										 new System.Windows.Forms.TreeNode("M-P"),
																																										 new System.Windows.Forms.TreeNode("Q-T"),
																																										 new System.Windows.Forms.TreeNode("U-Z")}),
																				 new System.Windows.Forms.TreeNode("Last Name", new System.Windows.Forms.TreeNode[] {
																																										new System.Windows.Forms.TreeNode("A-D"),
																																										new System.Windows.Forms.TreeNode("E-H"),
																																										new System.Windows.Forms.TreeNode("I-L"),
																																										new System.Windows.Forms.TreeNode("M-P"),
																																										new System.Windows.Forms.TreeNode("Q-T"),
																																										new System.Windows.Forms.TreeNode("U-Z")})});
			this.tvBrowse.SelectedImageIndex = -1;
			this.tvBrowse.Size = new System.Drawing.Size(104, 262);
			this.tvBrowse.TabIndex = 0;
			this.tvBrowse.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowse_AfterSelect);
			// 
			// tabPageGroups
			// 
			this.tabPageGroups.Controls.Add(this.lvGroups);
			this.tabPageGroups.Controls.Add(this.splitter2);
			this.tabPageGroups.Controls.Add(this.tvGroups);
			this.tabPageGroups.Location = new System.Drawing.Point(4, 4);
			this.tabPageGroups.Name = "tabPageGroups";
			this.tabPageGroups.Size = new System.Drawing.Size(344, 262);
			this.tabPageGroups.TabIndex = 1;
			this.tabPageGroups.Text = "Groups";
			// 
			// lvGroups
			// 
			this.lvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvGroups.Location = new System.Drawing.Point(91, 0);
			this.lvGroups.Name = "lvGroups";
			this.lvGroups.Size = new System.Drawing.Size(253, 262);
			this.lvGroups.TabIndex = 2;
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(88, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3, 262);
			this.splitter2.TabIndex = 1;
			this.splitter2.TabStop = false;
			// 
			// tvGroups
			// 
			this.tvGroups.Dock = System.Windows.Forms.DockStyle.Left;
			this.tvGroups.ImageIndex = -1;
			this.tvGroups.Location = new System.Drawing.Point(0, 0);
			this.tvGroups.Name = "tvGroups";
			this.tvGroups.SelectedImageIndex = -1;
			this.tvGroups.Size = new System.Drawing.Size(88, 262);
			this.tvGroups.TabIndex = 0;
			// 
			// PeopleCtl
			// 
			this.Controls.Add(this.tabControl1);
			this.Name = "PeopleCtl";
			this.Size = new System.Drawing.Size(352, 288);
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFullName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFirstName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonLastName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFind)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPageFind.ResumeLayout(false);
			this.tabPageBrowse.ResumeLayout(false);
			this.tabPageGroups.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void menuAddPerson_Click(object sender, System.EventArgs e)
		{
			fEditPerson p = new fEditPerson();
			p.NewPerson();
			p.ShowDialog();

			if (!p.Cancel) 
			{
				DataSetPerson.PersonRow pr = p.SelectedPerson;

				// add the new row to our ds
				DataSetPerson.PersonRow prNew = dsPerson.Person.NewPersonRow();
				if (!pr.IsLastNameNull())
					prNew.LastName = pr.LastName;
				if (!pr.IsFirstNameNull())
					prNew.FirstName = pr.FirstName;
				if (!pr.IsFullNameNull())
					prNew.FullName  = pr.FullName;
				prNew.PersonID  = pr.PersonID;
				dsPerson.Person.AddPersonRow(prNew);

			}


		}

		private void menuEditPerson_Click(object sender, System.EventArgs e)
		{
			// make sure a person is selected
			if (lvBrowse.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select a person to edit.", "Edit Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			ListViewItem person = lvBrowse.SelectedItems[0];
			
			DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) person.Tag;
			fEditPerson p = new fEditPerson();
			p.PersonID = pr.PersonID;

			p.ShowDialog();

			if (!p.Cancel) 
			{
				pr = p.SelectedPerson;
			}

		}

		private void menuDeletePerson_Click(object sender, System.EventArgs e)
		{
			if (lvBrowse.SelectedItems.Count == 0) 
			{
				MessageBox.Show("You must select a person to delete.", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			ListViewItem item = lvBrowse.SelectedItems[0];

			// make sure we want to delete
			if (MessageBox.Show("Would you like to delete '" + item.Text + "'?", 
				"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
			{
				DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) item.Tag;
				pr.Delete();
				daPerson.Update(dsPerson, "Person");

				lvBrowse.Items.Remove(lvBrowse.SelectedItems[0]);
			}
		}


		public DataSetPerson.PersonRow FindPersonInfo(int PersonID) 
		{
			return (dsPerson.Person.FindByPersonID(PersonID));
		}

		public DataSetPerson.PersonRow SelectedPerson
		{
			get 
			{
				if (tabControl1.SelectedTab == tabPageFind)
				{
					if (lvFind.SelectedItems.Count == 0)
					{
						return null;
					}
					else
					{
						return (DataSetPerson.PersonRow) lvFind.SelectedItems[0].Tag;
					}
				}
				else if (tabControl1.SelectedTab == tabPageBrowse)
				{
					if (lvBrowse.SelectedItems.Count == 0)
					{
						return null;
					}
					else
					{
						return (DataSetPerson.PersonRow) lvBrowse.SelectedItems[0].Tag;
					}
				} 
				else if (tabControl1.SelectedTab == tabPageGroups)
				{
					if (lvGroups.SelectedItems.Count == 0)
					{
						return null;
					}
					else
					{
						return (DataSetPerson.PersonRow) lvGroups.SelectedItems[0].Tag;
					}
				}			

				return null;
			}

		}

		// events
		public event ClickPersonEventHandler ClickPerson;
		public event DoubleClickPersonEventHandler DoubleClickPerson;


		private void findString_Enter(object sender, System.EventArgs e)
		{
			if (findString.Text.Equals("<enter name>"))
			{
				findString.Text = "";
				findString.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			string search = findString.Text;
			bool selectedPerson = false;

			dvPersonFind.RowFilter = "LastName like '%" + search + "%' OR FirstName like '%" + search + "%' OR FullName like '%" + search + "%'";
			foreach (DataRowView dr in dvPersonFind) 
			{
				// add this row as a listitem
				DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) dr.Row;
				ListViewItem child = lvFind.Items.Add(pr.FullName);
				child.Tag = pr;

				if (!selectedPerson) 
				{
					selectedPerson = true;
					child.Selected = true;
				}
			}
            
			findString.SelectAll();
			findString.Focus();
		
			// Select this person if the only one
			if (dvPersonFind.Count == 1)
			{
				if (DoubleClickPerson != null)
				{
					PersonCtlEventArgs ex = new PersonCtlEventArgs();
					ex.personRow = (DataSetPerson.PersonRow) dvPersonFind[0].Row;

					DoubleClickPerson(this, ex);
				}
			}
		}

		private void findString_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13 ) 
			{
				button1_Click(sender, e);
				e.Handled = true;
			}
		}

		private void lvBrowse_DoubleClick(object sender, System.EventArgs e)
		{

			// make sure someone is selected
			if (lvBrowse.SelectedItems.Count == 0)
				return;

			// Get the selected person
			ListViewItem item = lvBrowse.SelectedItems[0];

			// Fire event for other controls to catch if they want
			PersonCtlEventArgs ex = new PersonCtlEventArgs();
			ex.personRow = (DataSetPerson.PersonRow) item.Tag;
			
			if (DoubleClickPerson != null)
				DoubleClickPerson(this, ex);

		}

		private void tvBrowse_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			
			if (e.Action != TreeViewAction.Unknown)
			{

				TreeNode n = e.Node;

				// Check if we are on a letter range node
				if (n.Text.Length != 3 || !n.Text.Substring(1, 1).Equals("-"))
				{
					return;				
				}

				// Clear our listview
				lvBrowse.Items.Clear();

				string rowFilter = "FullName ";
				DataView dv = dvPersonFullName;
				TreeNode nParent = n.Parent;

				// select the appropriate view
				if (nParent.Text == "Full Name")
				{
					dv = dvPersonFullName;
				}
				else if (nParent.Text == "First Name")
				{
					dv = dvPersonFirstName;
					rowFilter = "FirstName ";
				}
				else if (nParent.Text == "Last Name")
				{
					dv = dvPersonLastName;
					rowFilter = "LastName ";
				}

				if (n.Text == "A-D")
					rowFilter = String.Format("{0} > 'A%' and {0} < 'E%'", rowFilter);
				else if (n.Text == "E-H")
					rowFilter = String.Format("{0} > 'E%' and {0} < 'I%'", rowFilter);
				else if (n.Text == "I-L")
					 rowFilter = String.Format("{0} > 'I%' and {0} < 'M%'", rowFilter);
				else if (n.Text == "M-P")
					 rowFilter = String.Format("{0} > 'M%' and {0} < 'Q%'", rowFilter);
				else if (n.Text == "Q-T")
					 rowFilter = String.Format("{0} > 'Q%' and {0} < 'U%'", rowFilter);
				else if (n.Text == "U-Z")
					 rowFilter = String.Format("{0} > 'U%'", rowFilter);

				dv.RowFilter = rowFilter;

				ListViewItem item;

				foreach (DataRowView dr in dv) 
				{
					// add this row as a node
					DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) dr.Row;
					if (nParent.Text == "Full Name")
						item = lvBrowse.Items.Add(pr.FullName);
					else if (nParent.Text == "First Name")
						item = lvBrowse.Items.Add(pr.FirstName + " " + pr.LastName);
					else //if (n.Tag.ToString() == "LastName")
						item = lvBrowse.Items.Add(pr.LastName + ", " + pr.FirstName);

					item.Tag = pr;
				}
		
			}
		}

		private void lvBrowse_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			// make sure someone is selected
			if (lvBrowse.SelectedItems.Count == 0)
				return;

			// Get the selected person
			ListViewItem item = lvBrowse.SelectedItems[0];

			// Fire event for other controls to catch if they want
			PersonCtlEventArgs ex = new PersonCtlEventArgs();
			ex.personRow = (DataSetPerson.PersonRow) item.Tag;
			
			if (ClickPerson != null)
				ClickPerson(this, ex);
		}

		private void lvFind_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// make sure someone is selected
			if (lvFind.SelectedItems.Count == 0)
				return;

			// Get the selected person
			ListViewItem item = lvFind.SelectedItems[0];

			// Fire event for other controls to catch if they want
			PersonCtlEventArgs ex = new PersonCtlEventArgs();
			ex.personRow = (DataSetPerson.PersonRow) item.Tag;
			
			if (ClickPerson != null)
				ClickPerson(this, ex);
		
		}

		private void lvFind_DoubleClick(object sender, System.EventArgs e)
		{
			// make sure someone is selected
			if (lvFind.SelectedItems.Count == 0)
				return;

			// Get the selected person
			ListViewItem item = lvFind.SelectedItems[0];

			// Fire event for other controls to catch if they want
			PersonCtlEventArgs ex = new PersonCtlEventArgs();
			ex.personRow = (DataSetPerson.PersonRow) item.Tag;
			
			if (DoubleClickPerson != null)
				DoubleClickPerson(this, ex);
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Check if we are on the find page
			if (tabControl1.SelectedIndex == 2)
			{
				findString.Focus();
			}
		
		}

	}

	// events
	public delegate void ClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	public delegate void DoubleClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	
	// class for passing events up
	public class PersonCtlEventArgs: EventArgs 
	{
		public DataSetPerson.PersonRow personRow;
	}

}
