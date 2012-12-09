using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace PicAdminCS
{
	/// <summary>
	/// Summary description for fPicture.
	/// </summary>
	public class fPicture : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelPic;
		private System.Windows.Forms.PictureBox pbPic;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;

		public bool mblnCancel;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.CheckBox chkPublish;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private PicAdminCS.CategoryPicker categoryPicker1;
		private System.Data.SqlClient.SqlDataAdapter daPictureCategory;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlDataAdapter daPicturePerson;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand3;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand3;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand3;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand3;
		private PicAdminCS.PersonPicker personPicker1;
		private System.Windows.Forms.Label label3;
		private PicAdminCS.PersonSelect personSelect1;
		private System.Data.SqlClient.SqlDataAdapter daPicture;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand4;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand4;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand4;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand4;
		private PicAdminCS.DataSetPicture dsPicture;
		private System.Windows.Forms.TextBox txtPictureDate;


		public String Title 
		{
			get 
			{
				return txtTitle.Text;
			}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fPicture()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			this.label3 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.sqlDeleteCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand4 = new System.Data.SqlClient.SqlCommand();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.dsPicture = new PicAdminCS.DataSetPicture();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.categoryPicker1 = new PicAdminCS.CategoryPicker();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.personPicker1 = new PicAdminCS.PersonPicker();
			this.chkPublish = new System.Windows.Forms.CheckBox();
			this.personSelect1 = new PicAdminCS.PersonSelect();
			this.panelPic = new System.Windows.Forms.Panel();
			this.pbPic = new System.Windows.Forms.PictureBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.txtPictureDate = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.daPictureCategory = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.daPicture = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlInsertCommand4 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand4 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand4 = new System.Data.SqlClient.SqlCommand();
			this.daPicturePerson = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand3 = new System.Data.SqlClient.SqlCommand();
			this.btnOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dsPicture)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panelPic.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 1;
			this.label3.Text = "By:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Location = new System.Drawing.Point(456, 440);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 24);
			this.btnCancel.TabIndex = 4;
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
			// sqlDeleteCommand3
			// 
			this.sqlDeleteCommand3.CommandText = "DELETE FROM PicturePerson WHERE (PersonID = @PersonID) AND (PictureID = @PictureI" +
				"D)";
			this.sqlDeleteCommand3.Connection = this.cn;
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
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
			// sqlInsertCommand3
			// 
			this.sqlInsertCommand3.CommandText = "INSERT INTO PicturePerson(PictureID, PersonID) VALUES (@PictureID, @PersonID); SE" +
				"LECT PictureID, PersonID FROM PicturePerson WHERE (PersonID = @Select_PersonID) " +
				"AND (PictureID = @Select_PictureID)";
			this.sqlInsertCommand3.Connection = this.cn;
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlDeleteCommand4
			// 
			this.sqlDeleteCommand4.CommandText = @"DELETE FROM Picture WHERE (PictureID = @PictureID) AND (Description = @Description OR @Description1 IS NULL AND Description IS NULL) AND (Filename = @Filename OR @Filename1 IS NULL AND Filename IS NULL) AND (PictureBy = @PictureBy OR @PictureBy1 IS NULL AND PictureBy IS NULL) AND (PictureDate = @PictureDate OR @PictureDate1 IS NULL AND PictureDate IS NULL) AND (Publish = @Publish OR @Publish1 IS NULL AND Publish IS NULL) AND (Title = @Title OR @Title1 IS NULL AND Title IS NULL)";
			this.sqlDeleteCommand4.Connection = this.cn;
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description1", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate1", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish1", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title1", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage2,
																					  this.tabPage3});
			this.tabControl1.Location = new System.Drawing.Point(16, 104);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(536, 208);
			this.tabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.txtDescription});
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(528, 182);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Description";
			// 
			// txtDescription
			// 
			this.txtDescription.AcceptsReturn = true;
			this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPicture, "Picture.Description"));
			this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(528, 182);
			this.txtDescription.TabIndex = 1;
			this.txtDescription.Text = "";
			// 
			// dsPicture
			// 
			this.dsPicture.DataSetName = "DataSetPicture";
			this.dsPicture.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsPicture.Namespace = "http://www.tempuri.org/DataSetPicture.xsd";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.categoryPicker1});
			this.tabPage2.Location = new System.Drawing.Point(4, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(528, 200);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Categories";
			// 
			// categoryPicker1
			// 
			this.categoryPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.categoryPicker1.Name = "categoryPicker1";
			this.categoryPicker1.Size = new System.Drawing.Size(560, 182);
			this.categoryPicker1.TabIndex = 0;
			this.categoryPicker1.AddedCategory += new PicAdminCS.AddedCategoryEventHandler(this.categoryPicker1_AddedCategory);
			this.categoryPicker1.RemovedCategory += new PicAdminCS.RemovedCategoryEventHandler(this.categoryPicker1_RemovedCategory);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.personPicker1});
			this.tabPage3.Location = new System.Drawing.Point(4, 4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(528, 200);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "People";
			// 
			// personPicker1
			// 
			this.personPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.personPicker1.Name = "personPicker1";
			this.personPicker1.Size = new System.Drawing.Size(560, 182);
			this.personPicker1.TabIndex = 0;
			this.personPicker1.RemovedPerson += new PicAdminCS.RemovedPersonEventHandler(this.personPicker1_RemovedPerson);
			this.personPicker1.AddedPerson += new PicAdminCS.AddedPersonEventHandler(this.personPicker1_AddedPerson);
			// 
			// chkPublish
			// 
			this.chkPublish.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.dsPicture, "Picture.Publish"));
			this.chkPublish.Location = new System.Drawing.Point(72, 80);
			this.chkPublish.Name = "chkPublish";
			this.chkPublish.Size = new System.Drawing.Size(408, 24);
			this.chkPublish.TabIndex = 2;
			this.chkPublish.Text = "Publish";
			// 
			// personSelect1
			// 
			this.personSelect1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.personSelect1.Location = new System.Drawing.Point(72, 56);
			this.personSelect1.Name = "personSelect1";
			this.personSelect1.SelectedPerson = null;
			this.personSelect1.SelectedPersonID = 0;
			this.personSelect1.Size = new System.Drawing.Size(472, 21);
			this.personSelect1.TabIndex = 8;
			// 
			// panelPic
			// 
			this.panelPic.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.panelPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelPic.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.pbPic});
			this.panelPic.Location = new System.Drawing.Point(16, 320);
			this.panelPic.Name = "panelPic";
			this.panelPic.Size = new System.Drawing.Size(196, 136);
			this.panelPic.TabIndex = 3;
			// 
			// pbPic
			// 
			this.pbPic.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbPic.Name = "pbPic";
			this.pbPic.Size = new System.Drawing.Size(192, 132);
			this.pbPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbPic.TabIndex = 0;
			this.pbPic.TabStop = false;
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPicture, "Picture.Title"));
			this.txtTitle.Location = new System.Drawing.Point(72, 8);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(472, 20);
			this.txtTitle.TabIndex = 0;
			this.txtTitle.Text = "";
			// 
			// txtPictureDate
			// 
			this.txtPictureDate.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtPictureDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPicture, "Picture.PictureDate"));
			this.txtPictureDate.Location = new System.Drawing.Point(72, 32);
			this.txtPictureDate.Name = "txtPictureDate";
			this.txtPictureDate.ReadOnly = true;
			this.txtPictureDate.Size = new System.Drawing.Size(472, 20);
			this.txtPictureDate.TabIndex = 0;
			this.txtPictureDate.TabStop = false;
			this.txtPictureDate.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Date:";
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
			this.sqlSelectCommand2.CommandText = "SELECT PictureID, CategoryID FROM PictureCategory WHERE (PictureID = @PictureID)";
			this.sqlSelectCommand2.Connection = this.cn;
			this.sqlSelectCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
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
			// daPicture
			// 
			this.daPicture.DeleteCommand = this.sqlDeleteCommand4;
			this.daPicture.InsertCommand = this.sqlInsertCommand4;
			this.daPicture.SelectCommand = this.sqlSelectCommand4;
			this.daPicture.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																								new System.Data.Common.DataTableMapping("Table", "Picture", new System.Data.Common.DataColumnMapping[] {
																																																		   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																		   new System.Data.Common.DataColumnMapping("Filename", "Filename"),
																																																		   new System.Data.Common.DataColumnMapping("PictureDate", "PictureDate"),
																																																		   new System.Data.Common.DataColumnMapping("Title", "Title"),
																																																		   new System.Data.Common.DataColumnMapping("Description", "Description"),
																																																		   new System.Data.Common.DataColumnMapping("Publish", "Publish"),
																																																		   new System.Data.Common.DataColumnMapping("PictureBy", "PictureBy")})});
			this.daPicture.UpdateCommand = this.sqlUpdateCommand4;
			// 
			// sqlInsertCommand4
			// 
			this.sqlInsertCommand4.CommandText = @"INSERT INTO Picture(Filename, PictureDate, Title, Description, Publish, PictureBy) VALUES (@Filename, @PictureDate, @Title, @Description, @Publish, @PictureBy); SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureBy FROM Picture WHERE (PictureID = @@IDENTITY)";
			this.sqlInsertCommand4.Connection = this.cn;
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.Timestamp, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand4
			// 
			this.sqlSelectCommand4.CommandText = "SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureBy F" +
				"ROM Picture WHERE (PictureID = @PictureID)";
			this.sqlSelectCommand4.Connection = this.cn;
			this.sqlSelectCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlUpdateCommand4
			// 
			this.sqlUpdateCommand4.CommandText = @"UPDATE Picture SET Filename = @Filename, PictureDate = @PictureDate, Title = @Title, Description = @Description, Publish = @Publish, PictureBy = @PictureBy WHERE (PictureID = @Original_PictureID) AND (Description = @Original_Description OR @Original_Description1 IS NULL AND Description IS NULL) AND (Filename = @Original_Filename OR @Original_Filename1 IS NULL AND Filename IS NULL) AND (PictureBy = @Original_PictureBy OR @Original_PictureBy1 IS NULL AND PictureBy IS NULL) AND (PictureDate = @Original_PictureDate OR @Original_PictureDate1 IS NULL AND PictureDate IS NULL) AND (Publish = @Original_Publish OR @Original_Publish1 IS NULL AND Publish IS NULL) AND (Title = @Original_Title OR @Original_Title1 IS NULL AND Title IS NULL); SELECT PictureID, Filename, PictureDate, Title, Description, Publish, PictureBy FROM Picture WHERE (PictureID = @Select_PictureID)";
			this.sqlUpdateCommand4.Connection = this.cn;
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description1", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureBy1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate1", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish1", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title1", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// daPicturePerson
			// 
			this.daPicturePerson.DeleteCommand = this.sqlDeleteCommand3;
			this.daPicturePerson.InsertCommand = this.sqlInsertCommand3;
			this.daPicturePerson.SelectCommand = this.sqlSelectCommand3;
			this.daPicturePerson.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																									  new System.Data.Common.DataTableMapping("Table", "PicturePerson", new System.Data.Common.DataColumnMapping[] {
																																																					   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																					   new System.Data.Common.DataColumnMapping("PersonID", "PersonID")})});
			this.daPicturePerson.UpdateCommand = this.sqlUpdateCommand3;
			// 
			// sqlSelectCommand3
			// 
			this.sqlSelectCommand3.CommandText = "SELECT PictureID, PersonID FROM PicturePerson WHERE (PictureID = @PictureID)";
			this.sqlSelectCommand3.Connection = this.cn;
			this.sqlSelectCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlUpdateCommand3
			// 
			this.sqlUpdateCommand3.CommandText = @"UPDATE PicturePerson SET PictureID = @PictureID, PersonID = @PersonID WHERE (PersonID = @Original_PersonID) AND (PictureID = @Original_PictureID); SELECT PictureID, PersonID FROM PicturePerson WHERE (PersonID = @Select_PersonID) AND (PictureID = @Select_PictureID)";
			this.sqlUpdateCommand3.Connection = this.cn;
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// btnOK
			// 
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOK.Location = new System.Drawing.Point(368, 440);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 24);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Title:";
			// 
			// fPicture
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(560, 470);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.personSelect1,
																		  this.label3,
																		  this.txtPictureDate,
																		  this.chkPublish,
																		  this.btnCancel,
																		  this.btnOK,
																		  this.panelPic,
																		  this.label2,
																		  this.label1,
																		  this.txtTitle,
																		  this.tabControl1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fPicture";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Picture Detail";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dsPicture)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panelPic.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			mblnCancel = true;
			this.Visible = false;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			//SavePicture();
			dsPicture.Picture[0].PictureBy = personSelect1.SelectedPersonID;
			this.BindingContext[dsPicture, "Picture"].EndCurrentEdit();

			daPicture.Update(dsPicture, "Picture");
			daPictureCategory.Update(dsPicture, "PictureCategory");
			daPicturePerson.Update(dsPicture, "PicturePerson");

			mblnCancel = false;
			this.Visible = false;
		}

		public bool LoadPicture(int id) 
		{
            daPicture.SelectCommand.Parameters["@PictureID"].Value = id;
			daPicture.Fill(dsPicture, "Picture");
			
			daPicturePerson.SelectCommand.Parameters["@PictureID"].Value = id;
			daPicturePerson.Fill(dsPicture, "PicturePerson");
			
			daPictureCategory.SelectCommand.Parameters["@PictureID"].Value = id;
			daPictureCategory.Fill(dsPicture, "PictureCategory");

			personSelect1.SelectedPersonID = dsPicture.Picture[0].PictureBy;

			// load categories
			foreach(DataSetPicture.PictureCategoryRow pcr in dsPicture.PictureCategory.Rows)
			{
                categoryPicker1.AddSelectedCategory(pcr.CategoryID);
			}

			// load people
			foreach(DataSetPicture.PicturePersonRow ppr in dsPicture.PicturePerson.Rows)
			{
				personPicker1.AddSelectedPerson(ppr.PersonID);
			}

			// load picture if possible
			String strFileName = dsPicture.Picture[0].Filename.ToString();

			if (strFileName.Length > 0) 
			{
				LoadImage("\\\\stan\\c$\\Inetpub\\msn2.net\\Pictures\\" + 
					strFileName.Replace("/", "\\") );
			}

			return true;
		}

		public bool LoadImage(String strFilename) 
		{

			Image img = Image.FromFile(strFilename);
			
			panelPic.Width = (int) ( ( (float) img.Width / (float) img.Height) * (float) panelPic.Height );
			pbPic.Width = panelPic.Width;
			pbPic.Height = panelPic.Height;

			pbPic.Image = img;

			return true;

		}

		private void categoryPicker1_AddedCategory(object sender, PicAdminCS.CategoryPickerEventArgs e)
		{
			// add the category to the PictureCategory dataset
			dsPicture.PictureCategory.AddPictureCategoryRow
				((DataSetPicture.PictureRow) dsPicture.Picture.Rows[0], e.CategoryID);
		}

		private void categoryPicker1_RemovedCategory(object sender, PicAdminCS.CategoryPickerEventArgs e)
		{
			DataSetPicture.PictureCategoryRow pcr = 
				dsPicture.PictureCategory.FindByPictureIDCategoryID(dsPicture.Picture[0].PictureID, e.CategoryID);
			
			if (pcr != null) 
			{
                pcr.Delete();
			}

		}

		private void personPicker1_AddedPerson(object sender, PicAdminCS.PersonPickerEventArgs e)
		{
			// add the person to the PicturePerson dataset
			dsPicture.PicturePerson.AddPicturePersonRow
				((DataSetPicture.PictureRow) dsPicture.Picture.Rows[0], e.PersonID);
		}

		private void personPicker1_RemovedPerson(object sender, PicAdminCS.PersonPickerEventArgs e)
		{
			DataSetPicture.PicturePersonRow ppr = 
				dsPicture.PicturePerson.FindByPictureIDPersonID(dsPicture.Picture[0].PictureID, e.PersonID);

			if (ppr != null)
			{
				ppr.Delete();
			}
			
		}


	}
}
