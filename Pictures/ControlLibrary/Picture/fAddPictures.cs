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
        private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand3;
        private System.Windows.Forms.CheckBox checkboxSortList;
        /// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private CheckBox autoRotate;
        private fStatus stat;

		public fAddPictures()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set the connection string
            cn.ConnectionString = PicContext.Current.Config.ConnectionString;

            // add everyone group by default
			groupPicker1.AddSelectedGroup(1);

            this.AcceptButton = this.btnAdd;
            this.CancelButton = this.btnCancel;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAddPictures));
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
            this.autoRotate = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.btnCancel.Location = new System.Drawing.Point(421, 430);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // sqlDeleteCommand2
            // 
            this.sqlDeleteCommand2.CommandText = "DELETE FROM PictureCategory WHERE (CategoryID = @CategoryID) AND (PictureID = @Pi" +
                "ctureID)";
            this.sqlDeleteCommand2.Connection = this.cn;
            this.sqlDeleteCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null)});
            // 
            // cn
            // 
            this.cn.ConnectionString = "data source=picdbserver;initial catalog=picdb;integrated security=SSPI;persist se" +
                "curity info=False;workstation id=CHEF;packet size=4096";
            this.cn.FireInfoMessageEventOnUserErrors = false;
            // 
            // personSelect1
            // 
            this.personSelect1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personSelect1.Location = new System.Drawing.Point(96, 91);
            this.personSelect1.Name = "personSelect1";
            this.personSelect1.SelectedPerson = null;
            this.personSelect1.SelectedPersonID = 0;
            this.personSelect1.Size = new System.Drawing.Size(373, 21);
            this.personSelect1.TabIndex = 9;
            // 
            // sqlInsertCommand2
            // 
            this.sqlInsertCommand2.CommandText = resources.GetString("sqlInsertCommand2.CommandText");
            this.sqlInsertCommand2.Connection = this.cn;
            this.sqlInsertCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"),
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"),
            new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"),
            new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID")});
            // 
            // sqlInsertCommand3
            // 
            this.sqlInsertCommand3.CommandText = "INSERT INTO PictureGroup(PictureID, GroupID) VALUES (@PictureID, @GroupID); SELEC" +
                "T PictureGroupID, PictureID, GroupID FROM PictureGroup WHERE (PictureGroupID = @" +
                "@IDENTITY)";
            this.sqlInsertCommand3.Connection = this.cn;
            this.sqlInsertCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null)});
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(341, 430);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
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
            this.groupPicker1.Size = new System.Drawing.Size(485, 236);
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
            this.tabControl1.Size = new System.Drawing.Size(493, 262);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.autoRotate);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.personSelect1);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(485, 236);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Details";
            // 
            // autoRotate
            // 
            this.autoRotate.AutoSize = true;
            this.autoRotate.Location = new System.Drawing.Point(16, 121);
            this.autoRotate.Name = "autoRotate";
            this.autoRotate.Size = new System.Drawing.Size(154, 17);
            this.autoRotate.TabIndex = 10;
            this.autoRotate.Text = "&Automatically rotate pictures";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Picture By:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.dtPictureDate);
            this.groupBox1.Controls.Add(this.radioCustomDate);
            this.groupBox1.Controls.Add(this.radioPictureDate);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(469, 76);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Picture Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Location = new System.Drawing.Point(150, 43);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(285, 20);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // dtPictureDate
            // 
            this.dtPictureDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPictureDate.Location = new System.Drawing.Point(88, 160);
            this.dtPictureDate.Name = "dtPictureDate";
            this.dtPictureDate.Size = new System.Drawing.Size(269, 20);
            this.dtPictureDate.TabIndex = 4;
            // 
            // radioCustomDate
            // 
            this.radioCustomDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioCustomDate.Location = new System.Drawing.Point(17, 43);
            this.radioCustomDate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.radioCustomDate.Name = "radioCustomDate";
            this.radioCustomDate.Size = new System.Drawing.Size(165, 24);
            this.radioCustomDate.TabIndex = 1;
            this.radioCustomDate.Text = "Use the following date:";
            this.radioCustomDate.CheckedChanged += new System.EventHandler(this.radioCustomDate_CheckedChanged);
            // 
            // radioPictureDate
            // 
            this.radioPictureDate.Checked = true;
            this.radioPictureDate.Location = new System.Drawing.Point(16, 20);
            this.radioPictureDate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
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
            this.tabPage1.Size = new System.Drawing.Size(485, 236);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Categories";
            // 
            // categoryPicker1
            // 
            this.categoryPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryPicker1.Location = new System.Drawing.Point(0, 0);
            this.categoryPicker1.Name = "categoryPicker1";
            this.categoryPicker1.Size = new System.Drawing.Size(485, 236);
            this.categoryPicker1.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupPicker1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(485, 236);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Share With";
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(8, 24);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFiles.Size = new System.Drawing.Size(493, 95);
            this.lstFiles.TabIndex = 1;
            // 
            // lblFiles
            // 
            this.lblFiles.Location = new System.Drawing.Point(8, 8);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(100, 23);
            this.lblFiles.TabIndex = 0;
            this.lblFiles.Text = "Pictures to add:";
            // 
            // btnAddPictures
            // 
            this.btnAddPictures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPictures.Location = new System.Drawing.Point(341, 128);
            this.btnAddPictures.Name = "btnAddPictures";
            this.btnAddPictures.Size = new System.Drawing.Size(72, 23);
            this.btnAddPictures.TabIndex = 2;
            this.btnAddPictures.Text = "A&dd";
            this.btnAddPictures.Click += new System.EventHandler(this.btnAddPictures_Click);
            // 
            // sqlDeleteCommand1
            // 
            this.sqlDeleteCommand1.CommandText = resources.GetString("sqlDeleteCommand1.CommandText");
            this.sqlDeleteCommand1.Connection = this.cn;
            this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@PictureGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureGroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@PictureID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null)});
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
            this.sqlSelectCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null)});
            // 
            // sqlUpdateCommand3
            // 
            this.sqlUpdateCommand3.CommandText = resources.GetString("sqlUpdateCommand3.CommandText");
            this.sqlUpdateCommand3.Connection = this.cn;
            this.sqlUpdateCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureGroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Select_PictureGroupID", System.Data.SqlDbType.Int, 4, "PictureGroupID")});
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
            this.sqlUpdateCommand2.CommandText = resources.GetString("sqlUpdateCommand2.CommandText");
            this.sqlUpdateCommand2.Connection = this.cn;
            this.sqlUpdateCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"),
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"),
            new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID")});
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
            this.sqlDeleteCommand3.CommandText = resources.GetString("sqlDeleteCommand3.CommandText");
            this.sqlDeleteCommand3.Connection = this.cn;
            this.sqlDeleteCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Filename", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureAddDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureAddDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureSort", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureSort", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureUpdateDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Publish", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Rating", System.Data.SqlDbType.TinyInt, 1, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Rating", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Title", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
            this.sqlInsertCommand1.Connection = this.cn;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 150, "Filename"),
            new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, "PictureDate"),
            new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, "Title"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, "Description"),
            new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"),
            new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, "PictureBy"),
            new System.Data.SqlClient.SqlParameter("@PictureSort", System.Data.SqlDbType.Int, 4, "PictureSort"),
            new System.Data.SqlClient.SqlParameter("@PictureAddDate", System.Data.SqlDbType.DateTime, 4, "PictureAddDate"),
            new System.Data.SqlClient.SqlParameter("@PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, "PictureUpdateDate"),
            new System.Data.SqlClient.SqlParameter("@Rating", System.Data.SqlDbType.TinyInt, 1, "Rating")});
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureBy, " +
                "PictureSort, PictureAddDate, PictureUpdateDate, Rating FROM Picture";
            this.sqlSelectCommand1.Connection = this.cn;
            // 
            // sqlUpdateCommand1
            // 
            this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
            this.sqlUpdateCommand1.Connection = this.cn;
            this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 150, "Filename"),
            new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, "PictureDate"),
            new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, "Title"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, "Description"),
            new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"),
            new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, "PictureBy"),
            new System.Data.SqlClient.SqlParameter("@PictureSort", System.Data.SqlDbType.Int, 4, "PictureSort"),
            new System.Data.SqlClient.SqlParameter("@PictureAddDate", System.Data.SqlDbType.DateTime, 4, "PictureAddDate"),
            new System.Data.SqlClient.SqlParameter("@PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, "PictureUpdateDate"),
            new System.Data.SqlClient.SqlParameter("@Rating", System.Data.SqlDbType.TinyInt, 1, "Rating"),
            new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Filename", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureAddDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureAddDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureSort", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureSort", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_PictureUpdateDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PictureUpdateDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Publish", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Rating", System.Data.SqlDbType.TinyInt, 1, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Rating", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Title", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID")});
            // 
            // dsPicture
            // 
            this.dsPicture.DataSetName = "DataSetPicture";
            this.dsPicture.Locale = new System.Globalization.CultureInfo("en-US");
            // 
            // btnRemovePictures
            // 
            this.btnRemovePictures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePictures.Location = new System.Drawing.Point(421, 128);
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
            this.checkboxSortList.Size = new System.Drawing.Size(245, 24);
            this.checkboxSortList.TabIndex = 11;
            this.checkboxSortList.Text = "Sort list by filename before adding";
            // 
            // fAddPictures
            // 
            this.AcceptButton = this.btnAdd;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(517, 460);
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
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dsPicture)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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

            // disable controls
			foreach (Control c in this.Controls)
			{
				c.Enabled = false;
			}

			// open a status window, file copying could take time
			stat = new fStatus("Adding pictures...", lstFiles.Items.Count);
			stat.Show();
			stat.Refresh();

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
            Utilities.ExifMetadata ex = new Utilities.ExifMetadata();
            foreach (string file in lstFiles.Items)
            {				
				// Figure out the directory based on either file date/time or custom date
                Utilities.ExifMetadata.Metadata data;
                DateTime date = DateTime.Now;
                if (radioCustomDate.Checked)
                {
                    date = dateTimePicker1.Value;
                }
                else 
                {
                    // Try to read from metadata
                    string dateTakenString = ImageUtilities.GetDatePictureTaken(file);
                    if (dateTakenString != null)
                    {
                        date = DateTime.Parse(dateTakenString);
                    }
                    else
                    {
                        // Default to last modified time
                        date = File.GetLastWriteTime(file);

                        try
                        {
                            // Get 'date picture taken' if available
                            data = ex.GetExifMetadata(file);

                            if (data.DatePictureTaken.DisplayValue != null)
                            {
                                date = DateTime.Parse(data.DatePictureTaken.DisplayValue);
                            }
                        }
                        catch (Exception exc)
                        {
                            Trace.WriteLine(exc.Message);
                        }
                    }
                }

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
				pictureRow.PictureDate  = date;
				string fileTitle	    = file.Substring(file.LastIndexOf(@"\")+1);
				fileTitle			    = fileTitle.Substring(0, fileTitle.LastIndexOf("."));  // strip off extension
				pictureRow.Title	    = fileTitle;
                pictureRow.Publish      = true;
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
                this.MoveFile(picDirectory + oldFilename, picDirectory + newFilename);

				// Allow move to complete
				Thread.Sleep(500);

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

            this.autoRotateFlag = this.autoRotate.Checked;

            ThreadPool.QueueUserWorkItem(new WaitCallback(CreateThumbs), ex);

            FinishImport();
        }

        private bool autoRotateFlag = false;

        private void CreateThumbs(object o)
        {
            Utilities.ExifMetadata ex = (Utilities.ExifMetadata) o;

            // Now create the thumbnails
            ImageUtilities utils = new ImageUtilities();
            stat.Current = 0;
            stat.StatusText = "Creating thumbnail images...";
            foreach (DataSetPicture.PictureRow pictureRow in dsPicture.Picture.Rows)
            {
                // Check rotate
                if (this.autoRotateFlag == true)
                {
                    stat.StatusText = "Rotating image...";
                    string file = PicContext.Current.Config.PictureDirectory + pictureRow.Filename;
                    Utilities.ExifMetadata.Metadata data = ex.GetExifMetadata(file);
                    RotatePicture(pictureRow, data.Orientation.Orientation);
                    stat.StatusText = "Creating thumbnail images...";
                }

                utils.CreateUpdateCache(pictureRow.PictureID);
                stat.Current = stat.Current + 1;
            }

            // close dialog
            this.stat.Invoke(new MethodInvoker(CloseStatus));
        }

        private void CloseStatus()
        {
            stat.Close();
        }

        private void MoveFile(string source, string dest)
        {
            bool moved = false;
            int count = 0;

            while (!moved)
            {
                try
                {
                    File.Move(source, dest);
                    moved = true;
                }
                catch (IOException)
                {
                    if (count > 200)
                    {
                        MessageBox.Show("Unable to copy " + source + " to " + dest);
                        throw;
                    }
                    Thread.Sleep(100);
                }

                count++;
            }
        }

        private void FinishImport()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(FinishImport));
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void RotatePicture(DataSetPicture.PictureRow pictureRow, Utilities.PictureOrientation orientation)
        {
            PictureManager pm = PicContext.Current.PictureManager;

            switch (orientation)
            {
                case Utilities.PictureOrientation.Rotate90:
                    pm.RotateImage(pictureRow.PictureID, RotateFlipType.Rotate90FlipNone);
                    break;
                case Utilities.PictureOrientation.Rotate180:
                    pm.RotateImage(pictureRow.PictureID, RotateFlipType.Rotate180FlipNone);
                    break;
                case Utilities.PictureOrientation.Roteate270:
                    pm.RotateImage(pictureRow.PictureID, RotateFlipType.Rotate270FlipNone);
                    break;
            }
        }

        public void AddCategory(int categoryId)
		{
			categoryPicker1.AddSelectedCategory(categoryId);
		}

		private void btnAddPictures_Click(object sender, System.EventArgs e)
		{
			// show the browse dialog
			if (openFileDialogPic.ShowDialog(this) == DialogResult.Cancel)
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

        public Category ImportCategory
        {
            get
            {
                Category category = null;
                if (this.categoryPicker1.selectedCategories.Count > 0)
                {
                    category = PicContext.Current.CategoryManager.GetCategory(this.categoryPicker1.SelectedCategoryIds[0]);
                }
                return category;
            }
        }
	
	}
}
