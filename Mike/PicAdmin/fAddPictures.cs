using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for fAddPictures.
	/// </summary>
	public class fAddPictures : System.Windows.Forms.Form
	{

		private bool mblnCancel;

		private System.Windows.Forms.Label lblFiles;
		private System.Windows.Forms.ListBox lstFiles;
		private System.Windows.Forms.Button btnAddPictures;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog openFileDialogPic;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Data.SqlClient.SqlDataAdapter daPictureCategory;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private msn2.net.Pictures.Controls.DataSetPicture dsPicture;
		private System.Windows.Forms.Button btnRemovePictures;
		private System.Data.SqlClient.SqlDataAdapter daPicture;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private msn2.net.Pictures.Controls.CategoryPicker categoryPicker1;
		private msn2.net.Pictures.Controls.GroupPicker groupPicker1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DateTimePicker dtPictureDate;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.RadioButton radioCustomDate;
		private System.Windows.Forms.RadioButton radioPictureDate;
		private System.Windows.Forms.Label label1;
		private msn2.net.Pictures.Controls.PersonSelect personSelect1;
		private System.Data.SqlClient.SqlDataAdapter daPictureGroup;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand3;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand3;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand3;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Windows.Forms.RadioButton radioFilenameDate;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand3;
		private System.Windows.Forms.CheckBox checkboxSortList;
		private System.Windows.Forms.CheckBox publishPictures;
		private System.Windows.Forms.CheckBox checkBoxFilenameTitle;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private fStatus stat;

		public fAddPictures()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set the connection string
			cn.ConnectionString = Config.ConnectionString;

			// add everyone group by default
			groupPicker1.AddSelectedGroup(1);

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
			this.btnCancel = new System.Windows.Forms.Button();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.personSelect1 = new msn2.net.Pictures.Controls.PersonSelect();
			this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
			this.btnAdd = new System.Windows.Forms.Button();
			this.groupPicker1 = new msn2.net.Pictures.Controls.GroupPicker();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.checkBoxFilenameTitle = new System.Windows.Forms.CheckBox();
			this.publishPictures = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioFilenameDate = new System.Windows.Forms.RadioButton();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.dtPictureDate = new System.Windows.Forms.DateTimePicker();
			this.radioCustomDate = new System.Windows.Forms.RadioButton();
			this.radioPictureDate = new System.Windows.Forms.RadioButton();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.categoryPicker1 = new msn2.net.Pictures.Controls.CategoryPicker();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.lstFiles = new System.Windows.Forms.ListBox();
			this.lblFiles = new System.Windows.Forms.Label();
			this.btnAddPictures = new System.Windows.Forms.Button();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.daPictureGroup = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand3 = new System.Data.SqlClient.SqlCommand();
			this.daPictureCategory = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.daPicture = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.dsPicture = new msn2.net.Pictures.Controls.DataSetPicture();
			this.btnRemovePictures = new System.Windows.Forms.Button();
			this.openFileDialogPic = new System.Windows.Forms.OpenFileDialog();
			this.checkboxSortList = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dsPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCancel.Location = new System.Drawing.Point(400, 384);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// sqlDeleteCommand2
			// 
			this.sqlDeleteCommand2.CommandText = "DELETE FROM PictureCategory WHERE (CategoryID = @CategoryID) AND (PictureID = @Pi" +
				"ctureID)";
			this.sqlDeleteCommand2.Connection = this.cn;
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// personSelect1
			// 
			this.personSelect1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.personSelect1.Location = new System.Drawing.Point(96, 112);
			this.personSelect1.Name = "personSelect1";
			this.personSelect1.SelectedPerson = null;
			this.personSelect1.SelectedPersonID = 0;
			this.personSelect1.Size = new System.Drawing.Size(352, 21);
			this.personSelect1.TabIndex = 9;
			// 
			// sqlInsertCommand2
			// 
			this.sqlInsertCommand2.CommandText = "INSERT INTO PictureCategory(PictureID, CategoryID) VALUES (@PictureID, @CategoryI" +
				"D); SELECT PictureID, CategoryID FROM PictureCategory WHERE (CategoryID = @Selec" +
				"t_CategoryID) AND (PictureID = @Select_PictureID)";
			this.sqlInsertCommand2.Connection = this.cn;
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// sqlInsertCommand3
			// 
			this.sqlInsertCommand3.CommandText = "INSERT INTO PictureGroup(PictureID, GroupID) VALUES (@PictureID, @GroupID); SELEC" +
				"T PictureGroupID, PictureID, GroupID FROM PictureGroup WHERE (PictureGroupID = @" +
				"@IDENTITY)";
			this.sqlInsertCommand3.Connection = this.cn;
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAdd.Location = new System.Drawing.Point(320, 384);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 7;
			this.btnAdd.Text = "&Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// groupPicker1
			// 
			this.groupPicker1.AllowRemoveEveryone = true;
			this.groupPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupPicker1.Location = new System.Drawing.Point(0, 0);
			this.groupPicker1.Name = "groupPicker1";
			this.groupPicker1.Size = new System.Drawing.Size(464, 190);
			this.groupPicker1.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(8, 160);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(472, 216);
			this.tabControl1.TabIndex = 10;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.checkBoxFilenameTitle);
			this.tabPage3.Controls.Add(this.publishPictures);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this.personSelect1);
			this.tabPage3.Controls.Add(this.groupBox1);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(464, 190);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Details";
			// 
			// checkBoxFilenameTitle
			// 
			this.checkBoxFilenameTitle.Checked = true;
			this.checkBoxFilenameTitle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxFilenameTitle.Location = new System.Drawing.Point(24, 160);
			this.checkBoxFilenameTitle.Name = "checkBoxFilenameTitle";
			this.checkBoxFilenameTitle.Size = new System.Drawing.Size(416, 24);
			this.checkBoxFilenameTitle.TabIndex = 11;
			this.checkBoxFilenameTitle.Text = "Use filename as title of picture";
			// 
			// publishPictures
			// 
			this.publishPictures.Checked = true;
			this.publishPictures.CheckState = System.Windows.Forms.CheckState.Checked;
			this.publishPictures.Location = new System.Drawing.Point(24, 136);
			this.publishPictures.Name = "publishPictures";
			this.publishPictures.Size = new System.Drawing.Size(416, 24);
			this.publishPictures.TabIndex = 10;
			this.publishPictures.Text = "Publish all pictures to site now";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 112);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Picture By:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.radioFilenameDate);
			this.groupBox1.Controls.Add(this.dateTimePicker1);
			this.groupBox1.Controls.Add(this.dtPictureDate);
			this.groupBox1.Controls.Add(this.radioCustomDate);
			this.groupBox1.Controls.Add(this.radioPictureDate);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(448, 96);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Picture Date";
			// 
			// radioFilenameDate
			// 
			this.radioFilenameDate.Location = new System.Drawing.Point(16, 64);
			this.radioFilenameDate.Name = "radioFilenameDate";
			this.radioFilenameDate.Size = new System.Drawing.Size(408, 24);
			this.radioFilenameDate.TabIndex = 5;
			this.radioFilenameDate.Text = "Use filename format:  yyyymmdd[-hhmm][-xx].xxx";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePicker1.Location = new System.Drawing.Point(168, 16);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(264, 20);
			this.dateTimePicker1.TabIndex = 4;
			// 
			// dtPictureDate
			// 
			this.dtPictureDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dtPictureDate.Location = new System.Drawing.Point(88, 160);
			this.dtPictureDate.Name = "dtPictureDate";
			this.dtPictureDate.Size = new System.Drawing.Size(248, 20);
			this.dtPictureDate.TabIndex = 4;
			// 
			// radioCustomDate
			// 
			this.radioCustomDate.Checked = true;
			this.radioCustomDate.Location = new System.Drawing.Point(16, 16);
			this.radioCustomDate.Name = "radioCustomDate";
			this.radioCustomDate.Size = new System.Drawing.Size(144, 24);
			this.radioCustomDate.TabIndex = 1;
			this.radioCustomDate.TabStop = true;
			this.radioCustomDate.Text = "Use the following date:";
			this.radioCustomDate.CheckedChanged += new System.EventHandler(this.radioCustomDate_CheckedChanged);
			// 
			// radioPictureDate
			// 
			this.radioPictureDate.Location = new System.Drawing.Point(16, 40);
			this.radioPictureDate.Name = "radioPictureDate";
			this.radioPictureDate.Size = new System.Drawing.Size(408, 24);
			this.radioPictureDate.TabIndex = 0;
			this.radioPictureDate.Text = "Use date on picture file (Date Picture Taken / Last Modified date)";
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.categoryPicker1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(464, 190);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Categories";
			// 
			// categoryPicker1
			// 
			this.categoryPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.categoryPicker1.Location = new System.Drawing.Point(0, 0);
			this.categoryPicker1.Name = "categoryPicker1";
			this.categoryPicker1.Size = new System.Drawing.Size(464, 190);
			this.categoryPicker1.TabIndex = 5;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupPicker1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(464, 190);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Security";
			// 
			// lstFiles
			// 
			this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lstFiles.Location = new System.Drawing.Point(8, 24);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstFiles.Size = new System.Drawing.Size(472, 95);
			this.lstFiles.TabIndex = 1;
			// 
			// lblFiles
			// 
			this.lblFiles.Location = new System.Drawing.Point(8, 8);
			this.lblFiles.Name = "lblFiles";
			this.lblFiles.TabIndex = 0;
			this.lblFiles.Text = "Pictures to add:";
			// 
			// btnAddPictures
			// 
			this.btnAddPictures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddPictures.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddPictures.Location = new System.Drawing.Point(320, 128);
			this.btnAddPictures.Name = "btnAddPictures";
			this.btnAddPictures.Size = new System.Drawing.Size(72, 23);
			this.btnAddPictures.TabIndex = 2;
			this.btnAddPictures.Text = "Add";
			this.btnAddPictures.Click += new System.EventHandler(this.btnAddPictures_Click);
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = "DELETE FROM PictureGroup WHERE (PictureGroupID = @PictureGroupID) AND (GroupID = " +
				"@GroupID OR @GroupID1 IS NULL AND GroupID IS NULL) AND (PictureID = @PictureID O" +
				"R @PictureID1 IS NULL AND PictureID IS NULL)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureGroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			// 
			// daPictureGroup
			// 
			this.daPictureGroup.DeleteCommand = this.sqlDeleteCommand1;
			this.daPictureGroup.InsertCommand = this.sqlInsertCommand3;
			this.daPictureGroup.SelectCommand = this.sqlSelectCommand3;
			this.daPictureGroup.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																									 new System.Data.Common.DataTableMapping("Table", "PictureGroup", new System.Data.Common.DataColumnMapping[] {
																																																					 new System.Data.Common.DataColumnMapping("PictureGroupID", "PictureGroupID"),
																																																					 new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																					 new System.Data.Common.DataColumnMapping("GroupID", "GroupID")})});
			this.daPictureGroup.UpdateCommand = this.sqlUpdateCommand3;
			// 
			// sqlSelectCommand3
			// 
			this.sqlSelectCommand3.CommandText = "SELECT PictureGroupID, PictureID, GroupID FROM PictureGroup WHERE (PictureID = @P" +
				"ictureID)";
			this.sqlSelectCommand3.Connection = this.cn;
			this.sqlSelectCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlUpdateCommand3
			// 
			this.sqlUpdateCommand3.CommandText = @"UPDATE PictureGroup SET PictureID = @PictureID, GroupID = @GroupID WHERE (PictureGroupID = @Original_PictureGroupID) AND (GroupID = @Original_GroupID OR @Original_GroupID1 IS NULL AND GroupID IS NULL) AND (PictureID = @Original_PictureID OR @Original_PictureID1 IS NULL AND PictureID IS NULL); SELECT PictureGroupID, PictureID, GroupID FROM PictureGroup WHERE (PictureGroupID = @Select_PictureGroupID)";
			this.sqlUpdateCommand3.Connection = this.cn;
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureGroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureGroupID", System.Data.SqlDbType.Int, 4, "PictureGroupID"));
			// 
			// daPictureCategory
			// 
			this.daPictureCategory.DeleteCommand = this.sqlDeleteCommand2;
			this.daPictureCategory.InsertCommand = this.sqlInsertCommand2;
			this.daPictureCategory.SelectCommand = this.sqlSelectCommand2;
			this.daPictureCategory.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "PictureCategory", new System.Data.Common.DataColumnMapping[] {
																																																						   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																						   new System.Data.Common.DataColumnMapping("CategoryID", "CategoryID")})});
			this.daPictureCategory.UpdateCommand = this.sqlUpdateCommand2;
			// 
			// sqlSelectCommand2
			// 
			this.sqlSelectCommand2.CommandText = "SELECT PictureID, CategoryID FROM PictureCategory";
			this.sqlSelectCommand2.Connection = this.cn;
			// 
			// sqlUpdateCommand2
			// 
			this.sqlUpdateCommand2.CommandText = @"UPDATE PictureCategory SET PictureID = @PictureID, CategoryID = @CategoryID WHERE (CategoryID = @Original_CategoryID) AND (PictureID = @Original_PictureID); SELECT PictureID, CategoryID FROM PictureCategory WHERE (CategoryID = @Select_CategoryID) AND (PictureID = @Select_PictureID)";
			this.sqlUpdateCommand2.Connection = this.cn;
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// daPicture
			// 
			this.daPicture.DeleteCommand = this.sqlDeleteCommand3;
			this.daPicture.InsertCommand = this.sqlInsertCommand1;
			this.daPicture.SelectCommand = this.sqlSelectCommand1;
			this.daPicture.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																								new System.Data.Common.DataTableMapping("Table", "Picture", new System.Data.Common.DataColumnMapping[] {
																																																		   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																		   new System.Data.Common.DataColumnMapping("Filename", "Filename"),
																																																		   new System.Data.Common.DataColumnMapping("PictureDate", "PictureDate"),
																																																		   new System.Data.Common.DataColumnMapping("Title", "Title"),
																																																		   new System.Data.Common.DataColumnMapping("Description", "Description"),
																																																		   new System.Data.Common.DataColumnMapping("Publish", "Publish"),
																																																		   new System.Data.Common.DataColumnMapping("PictureBy", "PictureBy"),
																																																		   new System.Data.Common.DataColumnMapping("PictureSort", "PictureSort"),
																																																		   new System.Data.Common.DataColumnMapping("PictureAddDate", "PictureAddDate"),
																																																		   new System.Data.Common.DataColumnMapping("PictureUpdateDate", "PictureUpdateDate"),
																																																		   new System.Data.Common.DataColumnMapping("Rating", "Rating")})});
			this.daPicture.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand3
			// 
			this.sqlDeleteCommand3.CommandText = @"DELETE FROM Picture WHERE (PictureID = @Original_PictureID) AND (Description = @Original_Description OR @Original_Description IS NULL AND Description IS NULL) AND (Filename = @Original_Filename OR @Original_Filename IS NULL AND Filename IS NULL) AND (PictureAddDate = @Original_PictureAddDate OR @Original_PictureAddDate IS NULL AND PictureAddDate IS NULL) AND (PictureBy = @Original_PictureBy OR @Original_PictureBy IS NULL AND PictureBy IS NULL) AND (PictureDate = @Original_PictureDate OR @Original_PictureDate IS NULL AND PictureDate IS NULL) AND (PictureSort = @Original_PictureSort) AND (PictureUpdateDate = @Original_PictureUpdateDate OR @Original_PictureUpdateDate IS NULL AND PictureUpdateDate IS NULL) AND (Publish = @Original_Publish OR @Original_Publish IS NULL AND Publish IS NULL) AND (Rating = @Original_Rating OR @Original_Rating IS NULL AND Rating IS NULL) AND (Title = @Original_Title OR @Original_Title IS NULL AND Title IS NULL)";
			this.sqlDeleteCommand3.Connection = this.cn;
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureAddDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureAddDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureSort", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureSort", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureUpdateDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Rating", System.Data.SqlDbType.TinyInt, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Rating", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = @"INSERT INTO Picture(Filename, PictureDate, Title, Description, Publish, PictureBy, PictureSort, PictureAddDate, PictureUpdateDate, Rating) VALUES (@Filename, @PictureDate, @Title, @Description, @Publish, @PictureBy, @PictureSort, @PictureAddDate, @PictureUpdateDate, @Rating); SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureBy, PictureSort, PictureAddDate, PictureUpdateDate, Rating FROM Picture WHERE (PictureID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 150, "Filename"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, "PictureDate"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, "Title"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, "Description"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, "PictureBy"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureSort", System.Data.SqlDbType.Int, 4, "PictureSort"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureAddDate", System.Data.SqlDbType.DateTime, 4, "PictureAddDate"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, "PictureUpdateDate"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rating", System.Data.SqlDbType.TinyInt, 1, "Rating"));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureBy, " +
				"PictureSort, PictureAddDate, PictureUpdateDate, Rating FROM Picture";
			this.sqlSelectCommand1.Connection = this.cn;
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Picture SET Filename = @Filename, PictureDate = @PictureDate, Title = @Title, Description = @Description, Publish = @Publish, PictureBy = @PictureBy, PictureSort = @PictureSort, PictureAddDate = @PictureAddDate, PictureUpdateDate = @PictureUpdateDate, Rating = @Rating WHERE (PictureID = @Original_PictureID) AND (Description = @Original_Description OR @Original_Description IS NULL AND Description IS NULL) AND (Filename = @Original_Filename OR @Original_Filename IS NULL AND Filename IS NULL) AND (PictureAddDate = @Original_PictureAddDate OR @Original_PictureAddDate IS NULL AND PictureAddDate IS NULL) AND (PictureBy = @Original_PictureBy OR @Original_PictureBy IS NULL AND PictureBy IS NULL) AND (PictureDate = @Original_PictureDate OR @Original_PictureDate IS NULL AND PictureDate IS NULL) AND (PictureSort = @Original_PictureSort) AND (PictureUpdateDate = @Original_PictureUpdateDate OR @Original_PictureUpdateDate IS NULL AND PictureUpdateDate IS NULL) AND (Publish = @Original_Publish OR @Original_Publish IS NULL AND Publish IS NULL) AND (Rating = @Original_Rating OR @Original_Rating IS NULL AND Rating IS NULL) AND (Title = @Original_Title OR @Original_Title IS NULL AND Title IS NULL); SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureBy, PictureSort, PictureAddDate, PictureUpdateDate, Rating FROM Picture WHERE (PictureID = @PictureID)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 150, "Filename"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, "PictureDate"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, "Title"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, "Description"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, "PictureBy"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureSort", System.Data.SqlDbType.Int, 4, "PictureSort"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureAddDate", System.Data.SqlDbType.DateTime, 4, "PictureAddDate"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, "PictureUpdateDate"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rating", System.Data.SqlDbType.TinyInt, 1, "Rating"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureAddDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureAddDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureSort", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureSort", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureUpdateDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Rating", System.Data.SqlDbType.TinyInt, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Rating", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// dsPicture
			// 
			this.dsPicture.DataSetName = "DataSetPicture";
			this.dsPicture.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// btnRemovePictures
			// 
			this.btnRemovePictures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemovePictures.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRemovePictures.Location = new System.Drawing.Point(400, 128);
			this.btnRemovePictures.Name = "btnRemovePictures";
			this.btnRemovePictures.Size = new System.Drawing.Size(72, 23);
			this.btnRemovePictures.TabIndex = 2;
			this.btnRemovePictures.Text = "Remove";
			this.btnRemovePictures.Click += new System.EventHandler(this.btnRemovePictures_Click);
			// 
			// openFileDialogPic
			// 
			this.openFileDialogPic.DefaultExt = "jpg";
			this.openFileDialogPic.Filter = "Supported Graphics Files (*.jpg, *.tif)|*.tif;*.jpg|JPEG Files (*.jpg)|*.jpg|TIF " +
				"Files (*.tif)|*.tif";
			this.openFileDialogPic.Multiselect = true;
			this.openFileDialogPic.Title = "Select picture(s):";
			// 
			// checkboxSortList
			// 
			this.checkboxSortList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.checkboxSortList.Checked = true;
			this.checkboxSortList.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkboxSortList.Location = new System.Drawing.Point(16, 128);
			this.checkboxSortList.Name = "checkboxSortList";
			this.checkboxSortList.Size = new System.Drawing.Size(224, 24);
			this.checkboxSortList.TabIndex = 11;
			this.checkboxSortList.Text = "Sort list by filename before adding";
			// 
			// fAddPictures
			// 
			this.AcceptButton = this.btnAdd;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(496, 414);
			this.Controls.Add(this.checkboxSortList);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnRemovePictures);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnAddPictures);
			this.Controls.Add(this.lstFiles);
			this.Controls.Add(this.lblFiles);
			this.Name = "fAddPictures";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Pictures";
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dsPicture)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			mblnCancel = true;
			Visible = false;
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			
			// make sure we have files
			if (lstFiles.Items.Count == 0) 
			{
				MessageBox.Show("You must add files to add them to the database.", "Add Pictures", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			// sort files if we are supposed to
			if (checkboxSortList.Checked)
				SortPictureList();

			// Validate the filenames if we need to
			if (radioFilenameDate.Checked) 
			{
				foreach (string file in lstFiles.Items) 
				{
					try 
					{
						ParseFilenameDateTime(file);
					} 
					catch (Exception ex) 
					{
						MessageBox.Show("The filename '" + file + "' cannot be parsed to determine the picture date/time.\n\t" + ex.Message, "Error in filename", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}

			}

			// disable controls
			btnAdd.Enabled = false;
			btnAddPictures.Enabled = false;

			// open a status window, file copying could take time
			stat = new fStatus("Adding pictures...", lstFiles.Items.Count);

			this.Visible = false;

			Thread t	= new Thread(new ThreadStart(ImportFile));
			t.Start();

		}

		private void ImportFile()
		{
			// figure out the current max PictureSort val
			cn.Open();
			SqlCommand cmdMaxVal = new SqlCommand("select Max(PictureSort) from Picture", cn);
			SqlDataReader drMaxVal = cmdMaxVal.ExecuteReader(CommandBehavior.SingleResult);
			int intCurPicSort = 1;
			if (drMaxVal.Read())
				intCurPicSort = drMaxVal.GetInt32(0);
			drMaxVal.Close();

			// add pictures to dataset
			int intFile = 0;
			foreach (string file in lstFiles.Items) 
			{				
				// Figure out the directory based on either file date/time or custom date
				DateTime date = DateTime.Now;
				if (radioCustomDate.Checked) 
					date = dateTimePicker1.Value;
				else if (radioPictureDate.Checked)
				{
					// Default to last modified time
					date = File.GetLastWriteTime(file);

					try
					{
						// Get 'date picture taken' if available
						Utilities.ExifMetadata ex				= new Utilities.ExifMetadata();
						Utilities.ExifMetadata.Metadata data	= ex.GetExifMetadata(file);

						date = DateTime.Parse(data.DatePictureTaken.DisplayValue);
					}
					catch (Exception ex)
					{						
						Trace.WriteLine(ex.Message);
					}
				}
				else
					date = ParseFilenameDateTime(file);
				string dateString = date.Year.ToString("0000") + "\\" 
					+ date.Month.ToString("00") + "\\" + date.Day.ToString("00") + "\\";

				// Build the full directory to use, and create it if not there
				string targetDirectory = PicContext.Current.Config.PictureDirectory + dateString;
				if (!Directory.Exists(targetDirectory))
					Directory.CreateDirectory(targetDirectory);

				// increment sort val
				intCurPicSort++;

				// get filename extension
				String fileExtension = file.Substring(file.LastIndexOf("."));
				String targetFile = "";

				// figure out the target filename
				int i = 0;
				for (i = 0; i<1000; i++) 
				{
					targetFile = "tmp" + i.ToString("0000") + fileExtension;
                    if (!File.Exists(targetDirectory + targetFile))
						break;
				}
                
				// copy file to target
				File.Copy(file, targetDirectory + targetFile);

				// add row to dataset
				targetFile = dateString + targetFile;
                DataSetPicture.PictureRow pictureRow = dsPicture.Picture.NewPictureRow();
				
				pictureRow.Filename		= targetFile;
				pictureRow.PictureDate = date;
				if (checkBoxFilenameTitle.Checked)
				{
					string fileTitle	= file.Substring(file.LastIndexOf(@"\")+1);
					fileTitle			= fileTitle.Substring(0, fileTitle.LastIndexOf("."));  // strip off extension
					pictureRow.Title	= fileTitle;
				}
				else
				{
					pictureRow.Title	= "(new picture)";
				}
				pictureRow.Publish		= publishPictures.Checked;
				pictureRow.Rating		= 50;
				pictureRow.PictureSort  = intCurPicSort;
				pictureRow.PictureAddDate = DateTime.Now;
				pictureRow.PictureUpdateDate = DateTime.Now;
				if (personSelect1.SelectedPerson != null)
					pictureRow.PictureBy = personSelect1.SelectedPersonID;
				
				dsPicture.Picture.AddPictureRow(pictureRow);

				// add categories to dataset
				foreach(int CategoryID in categoryPicker1.SelectedCategoryIds) 
				{
					dsPicture.PictureCategory.AddPictureCategoryRow(pictureRow,	CategoryID);
				}

				// add groups to dataset
				foreach (int groupId in groupPicker1.SelectedGroupIds) 
				{
					dsPicture.PictureGroup.AddPictureGroupRow(pictureRow, groupId);
				}

				// update progress bar
				intFile++;
				stat.Current = intFile;
                
			}

			// save the dataset
			stat.StatusText = "Writing to database...";
			SqlTransaction tr = cn.BeginTransaction();
			daPicture.InsertCommand.Transaction = tr;
			daPicture.UpdateCommand.Transaction = tr;
			daPicture.Update(dsPicture, "Picture");

			daPictureCategory.InsertCommand.Transaction = tr;
			daPictureCategory.Update(dsPicture, "PictureCategory");
			
			daPictureGroup.InsertCommand.Transaction = tr;
			daPictureGroup.Update(dsPicture, "PictureGroup");
			
			daPicture.Update(dsPicture, "Picture");

			// workaround concurrency error
			//SqlCommand cmd = new SqlCommand("update Picture set Filename=@Filename where PictureID = @PictureID", cn, tr);
			//cmd.Parameters.Add("@Filename", SqlDbType.NVarChar, 100);
			//cmd.Parameters.Add("@PictureID", SqlDbType.Int);

			// now update picture names
			stat.StatusText = "Updating picture names...";
//			foreach (DataRow dr in dsPicture.Picture) 
			foreach (DataSetPicture.PictureRow pr in dsPicture.Picture.Rows)
			{
                //DataSetPicture.PictureRow pr = (DataSetPicture.PictureRow) dr;
				
				// figure out the current directory portion of filename
				String directory = pr.Filename.Substring(0, pr.Filename.LastIndexOf("\\")+1);
				String extension = pr.Filename.Substring(pr.Filename.LastIndexOf("."));

				// figure out new name
				string oldFilename = pr.Filename;
				string newFilename = directory + pr.PictureID.ToString("000000") + extension;

				// rename the file
				string picDirectory = PicContext.Current.Config.PictureDirectory;
				File.Move(picDirectory + oldFilename, picDirectory + newFilename);

				pr.Filename = newFilename;

				// workaround concurrency error1
				//cmd.Parameters["@Filename"].Value = newFilename;
				//cmd.Parameters["@PictureID"].Value = pr.PictureID;
				//cmd.ExecuteNonQuery();

			}

			// Write final changes
			daPicture.Update(dsPicture, "Picture");

			// Commit to db
			stat.StatusText = "Saving to database...";
			tr.Commit();
			cn.Close();
			
			// Now create the thumbnails
			ImageUtilities utils = new ImageUtilities();
			stat.Current = 0;
			stat.StatusText = "Creating thumbnail images...";
			foreach (DataSetPicture.PictureRow pictureRow in dsPicture.Picture.Rows) 
			{
				utils.CreateUpdateCache(pictureRow.PictureID);
				stat.Current = stat.Current + 1;				
			}

			// all done
			stat.StatusText = "Done.";

			// close dialog
			stat.Hide();
			stat.Dispose();
			mblnCancel = false;
			Visible = false;
		}

		public void AddCategory(int categoryId)
		{
			categoryPicker1.AddSelectedCategory(categoryId);
		}

		private void btnAddPictures_Click(object sender, System.EventArgs e)
		{
			// show the browse dialog
			if (openFileDialogPic.ShowDialog() == DialogResult.Cancel)
				return;

			// add files if they aren't already added
			foreach(String strFile in openFileDialogPic.FileNames) 
			{
				if (!lstFiles.Items.Contains(strFile))
					lstFiles.Items.Add(strFile);
			}

		}

		private void btnRemovePictures_Click(object sender, System.EventArgs e)
		{
			while (lstFiles.SelectedItems.Count > 0)
				lstFiles.Items.Remove(lstFiles.SelectedItems[0]);
		}

		private void radioCustomDate_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioCustomDate.Checked) 
			{
				dateTimePicker1.Enabled = true;
			} 
			else 
			{
				dateTimePicker1.Enabled = false;
			}
		}

		private DateTime ParseFilenameDateTime(string filename) 
		{
            
			//yyyymmdd[-hhmm][-xx].xxx

			// get the actual filename
			string file = filename.Substring(filename.LastIndexOf("\\")+1);

			if (file.Length < 12) 
				throw new Exception("The filename is less than 12 characters.");

			string year  = file.Substring(0, 4);
			string month = file.Substring(4, 2);
			string day	 = file.Substring(6, 2);

			// see if this is a valid date
			try 
			{
				DateTime myd = Convert.ToDateTime(month + "/" + day + "/" + year);
			}
			catch (Exception) 
			{
				throw new Exception("The month/day/year was not a valid date.");
			}

			// see if time is given
			string hm = "";
			if (file.Length >= 16 && file.Substring(8, 1).Equals("-") && file.Substring(13,1).Equals("-")) 
			{
                hm = file.Substring(9, 2) + ":" + file.Substring(11, 2);
			}

            // Now try to create a datetime
			DateTime finalValue = DateTime.Now;
			try 
			{
				finalValue = Convert.ToDateTime(month + "/" + day + "/" + year + " " + hm);
			}
			catch (Exception)
			{
				throw new Exception("The time portion of the date/time was not recognized.");
			}

			return finalValue;

		}

		private void SortPictureList()
		{
			ArrayList files = new ArrayList(lstFiles.Items.Count);
			foreach (string file in lstFiles.Items)
				files.Add(file);
            
			lstFiles.Items.Clear();
			files.Sort();

			foreach (string file in files) 
				lstFiles.Items.Add(file);
		}

		public bool Cancel
		{
			get
			{
				return mblnCancel;
			}
			set
			{
				mblnCancel = value;
			}
		}
	
	}
}
