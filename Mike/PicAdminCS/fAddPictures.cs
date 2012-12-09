using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PicAdminCS
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
		private System.Windows.Forms.Label lblPictureDate;
		private System.Windows.Forms.DateTimePicker dtPictureDate;
		private PicAdminCS.CategoryPicker categoryPicker1;
		private System.Windows.Forms.Label lblPictureCategories;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog openFileDialogPic;
		private System.Data.SqlClient.SqlDataAdapter daPicture;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Data.SqlClient.SqlDataAdapter daPictureCategory;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private PicAdminCS.DataSetPicture dsPicture;
		private PicAdminCS.PersonSelect personSelect1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnRemovePictures;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fAddPictures()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set the connection string
			cn.ConnectionString = "data source=kyle;initial catalog=picdb;user id=sa;password=too;persist security info=False";

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.personSelect1 = new PicAdminCS.PersonSelect();
			this.categoryPicker1 = new PicAdminCS.CategoryPicker();
			this.label1 = new System.Windows.Forms.Label();
			this.daPicture = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this.daPictureCategory = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.lblPictureCategories = new System.Windows.Forms.Label();
			this.lblFiles = new System.Windows.Forms.Label();
			this.dtPictureDate = new System.Windows.Forms.DateTimePicker();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnRemovePictures = new System.Windows.Forms.Button();
			this.lstFiles = new System.Windows.Forms.ListBox();
			this.openFileDialogPic = new System.Windows.Forms.OpenFileDialog();
			this.btnAddPictures = new System.Windows.Forms.Button();
			this.dsPicture = new PicAdminCS.DataSetPicture();
			this.lblPictureDate = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dsPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = @"INSERT INTO Picture (Filename, PictureDate, Title, Description, Publish, PictureSort) VALUES (@Filename, @PictureDate, @Title, @Description, @Publish, @PictureSort); SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureSort FROM Picture WHERE (PictureID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureSort", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureSort", System.Data.DataRowVersion.Current, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// sqlInsertCommand2
			// 
			this.sqlInsertCommand2.CommandText = "INSERT INTO PictureCategory(PictureID, CategoryID) VALUES (@PictureID, @CategoryI" +
				"D); SELECT PictureID, CategoryID FROM PictureCategory WHERE (CategoryID = @Selec" +
				"t_CategoryID) AND (PictureID = @Select_PictureID)";
			this.sqlInsertCommand2.Connection = this.cn;
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand2
			// 
			this.sqlSelectCommand2.CommandText = "SELECT PictureID, CategoryID FROM PictureCategory";
			this.sqlSelectCommand2.Connection = this.cn;
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT PictureID, Filename, PictureDate, Title, Description, Publish FROM Picture" +
				"";
			this.sqlSelectCommand1.Connection = this.cn;
			// 
			// personSelect1
			// 
			this.personSelect1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.personSelect1.Location = new System.Drawing.Point(88, 184);
			this.personSelect1.Name = "personSelect1";
			this.personSelect1.SelectedPerson = null;
			this.personSelect1.SelectedPersonID = 0;
			this.personSelect1.Size = new System.Drawing.Size(336, 21);
			this.personSelect1.TabIndex = 9;
			// 
			// categoryPicker1
			// 
			this.categoryPicker1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.categoryPicker1.Location = new System.Drawing.Point(8, 232);
			this.categoryPicker1.Name = "categoryPicker1";
			this.categoryPicker1.Size = new System.Drawing.Size(560, 160);
			this.categoryPicker1.TabIndex = 5;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 184);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Picture By:";
			// 
			// daPicture
			// 
			this.daPicture.DeleteCommand = this.sqlDeleteCommand1;
			this.daPicture.InsertCommand = this.sqlInsertCommand1;
			this.daPicture.SelectCommand = this.sqlSelectCommand1;
			this.daPicture.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																								new System.Data.Common.DataTableMapping("Table", "Picture", new System.Data.Common.DataColumnMapping[] {
																																																		   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																		   new System.Data.Common.DataColumnMapping("Filename", "Filename"),
																																																		   new System.Data.Common.DataColumnMapping("PictureDate", "PictureDate"),
																																																		   new System.Data.Common.DataColumnMapping("Title", "Title"),
																																																		   new System.Data.Common.DataColumnMapping("Description", "Description"),
																																																		   new System.Data.Common.DataColumnMapping("Publish", "Publish")})});
			this.daPicture.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = @"DELETE FROM Picture WHERE (PictureID = @PictureID) AND (Description = @Description OR @Description1 IS NULL AND Description IS NULL) AND (Filename = @Filename OR @Filename1 IS NULL AND Filename IS NULL) AND (PictureDate = @PictureDate OR @PictureDate1 IS NULL AND PictureDate IS NULL) AND (Publish = @Publish OR @Publish1 IS NULL AND Publish IS NULL) AND (Title = @Title OR @Title1 IS NULL AND Title IS NULL)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description1", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate1", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish1", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title1", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Picture SET Filename = @Filename, PictureDate = @PictureDate, Title = @Title, Description = @Description, Publish = @Publish WHERE (PictureID = @Original_PictureID) AND (Description = @Original_Description OR @Original_Description1 IS NULL AND Description IS NULL) AND (Filename = @Original_Filename OR @Original_Filename1 IS NULL AND Filename IS NULL) AND (PictureDate = @Original_PictureDate OR @Original_PictureDate1 IS NULL AND PictureDate IS NULL) AND (Publish = @Original_Publish OR @Original_Publish1 IS NULL AND Publish IS NULL) AND (Title = @Original_Title OR @Original_Title1 IS NULL AND Title IS NULL); SELECT PictureID, Filename, PictureDate, Title, Description, Publish FROM Picture WHERE (PictureID = @Select_PictureID)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.Timestamp, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description1", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate1", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish1", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title1", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlDeleteCommand2
			// 
			this.sqlDeleteCommand2.CommandText = "DELETE FROM PictureCategory WHERE (CategoryID = @CategoryID) AND (PictureID = @Pi" +
				"ctureID)";
			this.sqlDeleteCommand2.Connection = this.cn;
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
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
			// sqlUpdateCommand2
			// 
			this.sqlUpdateCommand2.CommandText = @"UPDATE PictureCategory SET PictureID = @PictureID, CategoryID = @CategoryID WHERE (CategoryID = @Original_CategoryID) AND (PictureID = @Original_PictureID); SELECT PictureID, CategoryID FROM PictureCategory WHERE (CategoryID = @Select_CategoryID) AND (PictureID = @Select_PictureID)";
			this.sqlUpdateCommand2.Connection = this.cn;
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// lblPictureCategories
			// 
			this.lblPictureCategories.Location = new System.Drawing.Point(16, 216);
			this.lblPictureCategories.Name = "lblPictureCategories";
			this.lblPictureCategories.Size = new System.Drawing.Size(272, 23);
			this.lblPictureCategories.TabIndex = 6;
			this.lblPictureCategories.Text = "Picture Categories:";
			// 
			// lblFiles
			// 
			this.lblFiles.Location = new System.Drawing.Point(8, 8);
			this.lblFiles.Name = "lblFiles";
			this.lblFiles.TabIndex = 0;
			this.lblFiles.Text = "Pictures to add:";
			// 
			// dtPictureDate
			// 
			this.dtPictureDate.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.dtPictureDate.Location = new System.Drawing.Point(88, 160);
			this.dtPictureDate.Name = "dtPictureDate";
			this.dtPictureDate.Size = new System.Drawing.Size(336, 20);
			this.dtPictureDate.TabIndex = 4;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Location = new System.Drawing.Point(408, 400);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 7;
			this.btnAdd.Text = "&Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Location = new System.Drawing.Point(488, 400);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnRemovePictures
			// 
			this.btnRemovePictures.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnRemovePictures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRemovePictures.Location = new System.Drawing.Point(488, 128);
			this.btnRemovePictures.Name = "btnRemovePictures";
			this.btnRemovePictures.Size = new System.Drawing.Size(72, 23);
			this.btnRemovePictures.TabIndex = 2;
			this.btnRemovePictures.Text = "Remove";
			this.btnRemovePictures.Click += new System.EventHandler(this.btnRemovePictures_Click);
			// 
			// lstFiles
			// 
			this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lstFiles.Location = new System.Drawing.Point(8, 24);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstFiles.Size = new System.Drawing.Size(560, 95);
			this.lstFiles.TabIndex = 1;
			// 
			// openFileDialogPic
			// 
			this.openFileDialogPic.DefaultExt = "jpg";
			this.openFileDialogPic.Filter = "Supported Graphics Files (*.jpg, *.tif)|*.tif;*.jpg|JPEG Files (*.jpg)|*.jpg|TIF " +
				"Files (*.tif)|*.tif";
			this.openFileDialogPic.Multiselect = true;
			this.openFileDialogPic.Title = "Select picture(s):";
			// 
			// btnAddPictures
			// 
			this.btnAddPictures.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnAddPictures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddPictures.Location = new System.Drawing.Point(408, 128);
			this.btnAddPictures.Name = "btnAddPictures";
			this.btnAddPictures.Size = new System.Drawing.Size(72, 23);
			this.btnAddPictures.TabIndex = 2;
			this.btnAddPictures.Text = "Add";
			this.btnAddPictures.Click += new System.EventHandler(this.btnAddPictures_Click);
			// 
			// dsPicture
			// 
			this.dsPicture.DataSetName = "DataSetPicture";
			this.dsPicture.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsPicture.Namespace = "http://www.tempuri.org/DataSetPicture.xsd";
			// 
			// lblPictureDate
			// 
			this.lblPictureDate.Location = new System.Drawing.Point(8, 160);
			this.lblPictureDate.Name = "lblPictureDate";
			this.lblPictureDate.Size = new System.Drawing.Size(80, 23);
			this.lblPictureDate.TabIndex = 3;
			this.lblPictureDate.Text = "Picture Date:";
			// 
			// fAddPictures
			// 
			this.AcceptButton = this.btnAdd;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(584, 430);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnRemovePictures,
																		  this.label1,
																		  this.personSelect1,
																		  this.btnCancel,
																		  this.btnAdd,
																		  this.categoryPicker1,
																		  this.dtPictureDate,
																		  this.lblPictureDate,
																		  this.btnAddPictures,
																		  this.lstFiles,
																		  this.lblFiles,
																		  this.lblPictureCategories});
			this.Name = "fAddPictures";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Pictures";
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

			// disable controls
			btnAdd.Enabled = false;
			btnAddPictures.Enabled = false;
			
			// open a status window, file copying could take time
			fStatus stat = new fStatus(this, "Adding pictures...", lstFiles.Items.Count);

			// make sure directory for date exists
			String strTargetDir = "\\\\kenny\\inetpub\\pictures\\" + 
				dtPictureDate.Value.Year + "\\" + dtPictureDate.Value.Month.ToString("00") + "\\" + 
				dtPictureDate.Value.Day.ToString("00") + "\\";
			if (!Directory.Exists(strTargetDir))
				Directory.CreateDirectory(strTargetDir);

			// figure out the current max PictureSort val
			cn.Open();
			SqlCommand cmdMaxVal = new SqlCommand("select Max(PictureSort) from Picture", cn);
			SqlDataReader drMaxVal = cmdMaxVal.ExecuteReader(CommandBehavior.SingleResult);
			int intCurPicSort = 1;
			if (drMaxVal.Read())
				intCurPicSort = drMaxVal.GetInt32(0);
			drMaxVal.Close();
			cn.Close();

			// add pictures to dataset
			int intFile = 0;
			foreach (String strFile in lstFiles.Items) 
			{
				// increment sort val
				intCurPicSort++;

				// get filename extension
				String strExtension = strFile.Substring(strFile.LastIndexOf("."));
				String strTargetFile = "";

				// figure out the target filename
				int i = 0;
				for (i = 0; i<100; i++) 
				{
					strTargetFile = i.ToString("0000") + strExtension;
                    if (!File.Exists(strTargetDir + strTargetFile))
						break;
				}
                
				// copy file to target
				File.Copy(strFile, strTargetDir + strTargetFile);

				// add row to dataset
				strTargetFile = dtPictureDate.Value.Year + "\\" 
					+ dtPictureDate.Value.Month.ToString("00") + "\\" 
					+ dtPictureDate.Value.Day.ToString("00") + "\\" + strTargetFile;
                DataSetPicture.PictureRow pictureRow = dsPicture.Picture.NewPictureRow();
				
				pictureRow.Filename		= strTargetFile;
				pictureRow.PictureDate	= dtPictureDate.Value;
				pictureRow.Title		= "(new picture)";
				pictureRow.Publish		= false;
				pictureRow.PictureSort  = intCurPicSort;
				if (personSelect1.SelectedPerson != null)
					pictureRow.PictureBy = personSelect1.SelectedPersonID;
				
				dsPicture.Picture.AddPictureRow(pictureRow);

				// add categories to dataset
				foreach(int CategoryID in categoryPicker1.selectedCategories) 
				{
					dsPicture.PictureCategory.AddPictureCategoryRow(
						pictureRow,
						CategoryID);
				}

				// update progress bar
				intFile++;
				stat.Current = intFile;
                
			}

			// save the dataset
			stat.StatusText = "Saving to database...";
            daPicture.Update(dsPicture, "Picture");
			daPictureCategory.Update(dsPicture, "PictureCategory");
			stat.StatusText = "Done.";

			// close dialog
			stat.Visible = false;
			mblnCancel = false;
			Visible = false;
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
