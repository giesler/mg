using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PicAdmin
{
	/// <summary>
	/// Summary description for fPromptText.
	/// </summary>
	public class fEditPerson : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblCaption;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFullName;
		private System.Data.SqlClient.SqlConnection cn;
		private PicAdmin.DataSetPerson dsPerson;
		private bool mblnCancel = false;
		protected int mintPersonID;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlDataAdapter daPerson;
		private PicAdmin.GroupPicker groupPicker1;
		private System.Data.SqlClient.SqlDataAdapter daPersonGroup;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		protected DataSetPerson.PersonRow curPersonRow;

		public fEditPerson()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			// we don't want to allow removal of everyone
			groupPicker1.AllowRemoveEveryone = false;
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
			this.label3 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtFullName = new System.Windows.Forms.TextBox();
			this.dsPerson = new PicAdmin.DataSetPerson();
			this.daPersonGroup = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.groupPicker1 = new PicAdmin.GroupPicker();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this.daPerson = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.lblCaption = new System.Windows.Forms.Label();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).BeginInit();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label3.Location = new System.Drawing.Point(16, 128);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(398, 23);
			this.label3.TabIndex = 6;
			this.label3.Text = "Email";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCancel.Location = new System.Drawing.Point(320, 320);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtFullName
			// 
			this.txtFullName.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtFullName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPerson, "Person.FullName"));
			this.txtFullName.Location = new System.Drawing.Point(16, 104);
			this.txtFullName.Name = "txtFullName";
			this.txtFullName.Size = new System.Drawing.Size(398, 20);
			this.txtFullName.TabIndex = 5;
			this.txtFullName.Text = "";
			this.txtFullName.Validating += new System.ComponentModel.CancelEventHandler(this.txtFullName_Validating);
			this.txtFullName.Validated += new System.EventHandler(this.txtFullName_Validated);
			// 
			// dsPerson
			// 
			this.dsPerson.DataSetName = "DataSetPerson";
			this.dsPerson.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsPerson.Namespace = "http://www.tempuri.org/DataSetPerson.xsd";
			// 
			// daPersonGroup
			// 
			this.daPersonGroup.DeleteCommand = this.sqlDeleteCommand1;
			this.daPersonGroup.InsertCommand = this.sqlInsertCommand1;
			this.daPersonGroup.SelectCommand = this.sqlSelectCommand1;
			this.daPersonGroup.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																									new System.Data.Common.DataTableMapping("Table", "PersonGroup", new System.Data.Common.DataColumnMapping[] {
																																																				   new System.Data.Common.DataColumnMapping("PersonGroupID", "PersonGroupID"),
																																																				   new System.Data.Common.DataColumnMapping("PersonID", "PersonID"),
																																																				   new System.Data.Common.DataColumnMapping("GroupID", "GroupID")})});
			this.daPersonGroup.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = "DELETE FROM PersonGroup WHERE (GroupID = @GroupID) AND (PersonID = @PersonID) AND" +
				" (PersonGroupID = @PersonGroupID)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonGroupID", System.Data.DataRowVersion.Original, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;password=tOO;persist security info=True;us" +
				"er id=sa;workstation id=CHEF;packet size=4096";
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO PersonGroup(PersonID, GroupID) VALUES (@PersonID, @GroupID); SELECT P" +
				"ersonGroupID, PersonID, GroupID FROM PersonGroup WHERE (GroupID = @Select_GroupI" +
				"D) AND (PersonID = @Select_PersonID)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT PersonGroupID, PersonID, GroupID FROM PersonGroup WHERE (PersonID = @Perso" +
				"nID)";
			this.sqlSelectCommand1.Connection = this.cn;
			this.sqlSelectCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE PersonGroup SET PersonID = @PersonID, GroupID = @GroupID WHERE (GroupID = @Original_GroupID) AND (PersonID = @Original_PersonID); SELECT PersonGroupID, PersonID, GroupID FROM PersonGroup WHERE (GroupID = @Select_GroupID) AND (PersonID = @Select_PersonID)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			// 
			// txtLastName
			// 
			this.txtLastName.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtLastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPerson, "Person.LastName"));
			this.txtLastName.Location = new System.Drawing.Point(16, 64);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(398, 20);
			this.txtLastName.TabIndex = 3;
			this.txtLastName.Text = "";
			this.txtLastName.Validating += new System.ComponentModel.CancelEventHandler(this.txtLastName_Validating);
			this.txtLastName.Validated += new System.EventHandler(this.txtLastName_Validated);
			this.txtLastName.Leave += new System.EventHandler(this.txtLastName_Leave);
			// 
			// groupPicker1
			// 
			this.groupPicker1.AllowRemoveEveryone = true;
			this.groupPicker1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupPicker1.Location = new System.Drawing.Point(16, 176);
			this.groupPicker1.Name = "groupPicker1";
			this.groupPicker1.Size = new System.Drawing.Size(398, 136);
			this.groupPicker1.TabIndex = 10;
			this.groupPicker1.RemovedGroup += new PicAdmin.RemovedGroupEventHandler(this.groupPicker1_RemovedGroup);
			this.groupPicker1.AddedGroup += new PicAdmin.AddedGroupEventHandler(this.groupPicker1_AddedGroup);
			// 
			// sqlDeleteCommand2
			// 
			this.sqlDeleteCommand2.CommandText = @"DELETE FROM Person WHERE (PersonID = @PersonID) AND (Email = @Email OR @Email1 IS NULL AND Email IS NULL) AND (FirstName = @FirstName OR @FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @FullName OR @FullName1 IS NULL AND FullName IS NULL) AND (LastName = @LastName OR @LastName1 IS NULL AND LastName IS NULL)";
			this.sqlDeleteCommand2.Connection = this.cn;
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Email", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email1", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Email", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			// 
			// daPerson
			// 
			this.daPerson.DeleteCommand = this.sqlDeleteCommand2;
			this.daPerson.InsertCommand = this.sqlInsertCommand2;
			this.daPerson.SelectCommand = this.sqlSelectCommand2;
			this.daPerson.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							   new System.Data.Common.DataTableMapping("Table", "Person", new System.Data.Common.DataColumnMapping[] {
																																																		 new System.Data.Common.DataColumnMapping("PersonID", "PersonID"),
																																																		 new System.Data.Common.DataColumnMapping("LastName", "LastName"),
																																																		 new System.Data.Common.DataColumnMapping("FirstName", "FirstName"),
																																																		 new System.Data.Common.DataColumnMapping("FullName", "FullName"),
																																																		 new System.Data.Common.DataColumnMapping("Email", "Email")})});
			this.daPerson.UpdateCommand = this.sqlUpdateCommand2;
			// 
			// sqlInsertCommand2
			// 
			this.sqlInsertCommand2.CommandText = "INSERT INTO Person(LastName, FirstName, FullName, Email) VALUES (@LastName, @Firs" +
				"tName, @FullName, @Email); SELECT PersonID, LastName, FirstName, FullName, Email" +
				" FROM Person WHERE (PersonID = @@IDENTITY)";
			this.sqlInsertCommand2.Connection = this.cn;
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Email", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand2
			// 
			this.sqlSelectCommand2.CommandText = "SELECT PersonID, LastName, FirstName, FullName, Email FROM Person WHERE (PersonID" +
				" = @PersonID)";
			this.sqlSelectCommand2.Connection = this.cn;
			this.sqlSelectCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlUpdateCommand2
			// 
			this.sqlUpdateCommand2.CommandText = @"UPDATE Person SET LastName = @LastName, FirstName = @FirstName, FullName = @FullName, Email = @Email WHERE (PersonID = @Original_PersonID) AND (Email = @Original_Email OR @Original_Email1 IS NULL AND Email IS NULL) AND (FirstName = @Original_FirstName OR @Original_FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @Original_FullName OR @Original_FullName1 IS NULL AND FullName IS NULL) AND (LastName = @Original_LastName OR @Original_LastName1 IS NULL AND LastName IS NULL); SELECT PersonID, LastName, FirstName, FullName, Email FROM Person WHERE (PersonID = @Select_PersonID)";
			this.sqlUpdateCommand2.Connection = this.cn;
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Email", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Email", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Email", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Email1", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Email", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			// 
			// lblCaption
			// 
			this.lblCaption.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lblCaption.Location = new System.Drawing.Point(16, 8);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(398, 24);
			this.lblCaption.TabIndex = 0;
			this.lblCaption.Text = "First Name:";
			// 
			// txtFirstName
			// 
			this.txtFirstName.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPerson, "Person.FirstName"));
			this.txtFirstName.Location = new System.Drawing.Point(16, 24);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(398, 20);
			this.txtFirstName.TabIndex = 1;
			this.txtFirstName.Text = "";
			this.txtFirstName.Validating += new System.ComponentModel.CancelEventHandler(this.txtFirstName_Validating);
			this.txtFirstName.Validated += new System.EventHandler(this.txtFirstName_Validated);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnOK.Location = new System.Drawing.Point(240, 320);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// txtEmail
			// 
			this.txtEmail.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPerson, "Person.Email"));
			this.txtEmail.Location = new System.Drawing.Point(16, 144);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(398, 20);
			this.txtEmail.TabIndex = 7;
			this.txtEmail.Text = "";
			this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
			this.txtEmail.Validated += new System.EventHandler(this.txtEmail_Validated);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label1.Location = new System.Drawing.Point(16, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(398, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Last Name:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label2.Location = new System.Drawing.Point(16, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(398, 24);
			this.label2.TabIndex = 4;
			this.label2.Text = "Full Name:";
			// 
			// fEditPerson
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(432, 352);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupPicker1,
																		  this.txtEmail,
																		  this.label3,
																		  this.txtFullName,
																		  this.label2,
																		  this.txtLastName,
																		  this.label1,
																		  this.btnCancel,
																		  this.btnOK,
																		  this.txtFirstName,
																		  this.lblCaption});
			this.Name = "fEditPerson";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Person";
			this.Load += new System.EventHandler(this.fEditPerson_Load);
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{

			// make sure fullname is set
			if (txtFullName.Text.Length == 0) 
			{
				return;
			}
			
			//			if (mintPersonID != 0)
//			{
				this.BindingContext[dsPerson, "Person"].EndCurrentEdit();
				daPerson.Update(dsPerson, "Person");
				daPersonGroup.Update(dsPerson, "PersonGroup");
				Visible = false;
/*			}
				// otherwise we want to add a record
			else
			{
				// add row
				DataSetPerson.PersonRow pr = dsPerson.Person.NewPersonRow();
				pr.FirstName = txtFirstName.Text;
				pr.LastName  = txtLastName.Text;
				pr.FullName  = txtFullName.Text;
				pr.Email	 = txtEmail.Text;
				dsPerson.Person.AddPersonRow(pr);
				daPerson.Update(dsPerson, "Person");
				daPersonGroup.Update(dsPerson, "PersonGroup");
				curPersonRow = pr;
			}
*/
			Visible = false;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			mblnCancel = true;
			Visible = false;
		}

		private void txtLastName_Leave(object sender, System.EventArgs e)
		{
			if (txtFullName.Text == "")
				txtFullName.Text = txtFirstName.Text + " " + txtLastName.Text;
		}

		private void groupPicker1_AddedGroup(object sender, PicAdmin.GroupPickerEventArgs e)
		{
			// add the group to the PictureGroup dataset
			dsPerson.PersonGroup.AddPersonGroupRow(
				(DataSetPerson.PersonRow) dsPerson.Person.Rows[0], e.GroupID);

		}

		private void groupPicker1_RemovedGroup(object sender, PicAdmin.GroupPickerEventArgs e)
		{
			DataSetPerson.PersonGroupRow row = 
				dsPerson.PersonGroup.FindByPersonIDGroupID(dsPerson.Person[0].PersonID, e.GroupID);

			if (row != null)
			{
				row.Delete();
			}

		}

		private void fEditPerson_Load(object sender, System.EventArgs e)
		{

		}

		public bool Cancel
		{
			get
			{
				return mblnCancel;
			}
		}

		public int PersonID 
		{
			get 
			{
				return mintPersonID;
			}
			set 
			{
				mintPersonID = value;
				daPerson.SelectCommand.Parameters["@PersonID"].Value = value;
				daPerson.Fill(dsPerson, "Person");
				curPersonRow = (DataSetPerson.PersonRow) dsPerson.Person.Rows[0];
				daPersonGroup.SelectCommand.Parameters["@PersonID"].Value = value;
				daPersonGroup.Fill(dsPerson, "PersonGroup");

				// load selected groups
				groupPicker1.ClearSelectedGroups();
				foreach (DataSetPerson.PersonGroupRow groupRow in dsPerson.PersonGroup.Rows) 
				{
					groupPicker1.AddSelectedGroup(groupRow.GroupID);
				}
			}
		}

		public void NewPerson() 
		{

			// create a new row
			curPersonRow = dsPerson.Person.NewPersonRow();
			dsPerson.Person.AddPersonRow(curPersonRow);
            
            // add everyone group
			groupPicker1.AddSelectedGroup(1);
			dsPerson.PersonGroup.AddPersonGroupRow(curPersonRow, 1);

		}

		private void txtFirstName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try 
			{
				if (txtFirstName.Text.Length == 0)
					throw new Exception("First name is a required field!");
			}
			catch (Exception ex) 
			{
				// cancel this event
				e.Cancel = true;
				errorProvider1.SetError(txtFirstName, ex.Message);
			}
		}

		private void txtFirstName_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(txtFirstName, "");
		}

		private void txtLastName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try 
			{
				if (txtLastName.Text.Length == 0)
					throw new Exception("Last name is a required field!");
			}
			catch (Exception ex) 
			{
				// cancel this event
				e.Cancel = true;
				errorProvider1.SetError(txtLastName, ex.Message);
			}
		}

		private void txtLastName_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(txtLastName, "");
		}

		private void txtFullName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try 
			{
				if (txtFullName.Text.Length == 0)
					throw new Exception("Full name is a required field!");
			}
			catch (Exception ex) 
			{
				// cancel this event
				e.Cancel = true;
				errorProvider1.SetError(txtFullName, ex.Message);
			}
		}

		private void txtFullName_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(txtFullName, "");
		}

		private void txtEmail_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try 
			{
				if (txtFirstName.Text.Length > 0 && 
					(txtFirstName.Text.IndexOf(".") == 0 || txtFirstName.Text.IndexOf("@") == 0) )
					throw new Exception("First name is a required field!");
			}
			catch (Exception ex) 
			{
				// cancel this event
				e.Cancel = true;
				errorProvider1.SetError(txtFirstName, ex.Message);
			}
		}

		private void txtEmail_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(txtEmail, "");
		}

		public DataSetPerson.PersonRow SelectedPerson 
		{
			get 
			{
				return curPersonRow;
			}
		}

	}
}
