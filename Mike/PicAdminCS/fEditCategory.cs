using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PicAdminCS
{
	/// <summary>
	/// Summary description for fEditCategory.
	/// </summary>
	public class fEditCategory : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCategoryName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;

		protected bool mblnCancel = false;
		protected int mintCategoryID = 0;
		protected int mintParentCategoryID = 0;
		protected DataSetCategory.CategoryRow curCategoryRow;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Data.SqlClient.SqlDataAdapter daCategory;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private PicAdminCS.DataSetCategory dsCategory;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fEditCategory()
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
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtCategoryName = new System.Windows.Forms.TextBox();
			this.dsCategory = new PicAdminCS.DataSetCategory();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.daCategory = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			((System.ComponentModel.ISupportInitialize)(this.dsCategory)).BeginInit();
			this.SuspendLayout();
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOK.Location = new System.Drawing.Point(184, 136);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// txtCategoryName
			// 
			this.txtCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtCategoryName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsCategory, "Category.CategoryName"));
			this.txtCategoryName.Location = new System.Drawing.Point(80, 8);
			this.txtCategoryName.Name = "txtCategoryName";
			this.txtCategoryName.Size = new System.Drawing.Size(264, 20);
			this.txtCategoryName.TabIndex = 1;
			this.txtCategoryName.Text = "";
			// 
			// dsCategory
			// 
			this.dsCategory.DataSetName = "DataSetCategory";
			this.dsCategory.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsCategory.Namespace = "http://www.tempuri.org/DataSetCategory.xsd";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Description:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Location = new System.Drawing.Point(264, 136);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsCategory, "Category.CategoryDescription"));
			this.txtDescription.Location = new System.Drawing.Point(80, 32);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(264, 96);
			this.txtDescription.TabIndex = 3;
			this.txtDescription.Text = "";
			// 
			// daCategory
			// 
			this.daCategory.DeleteCommand = this.sqlDeleteCommand1;
			this.daCategory.InsertCommand = this.sqlInsertCommand1;
			this.daCategory.SelectCommand = this.sqlSelectCommand1;
			this.daCategory.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																								 new System.Data.Common.DataTableMapping("Table", "Category", new System.Data.Common.DataColumnMapping[] {
																																																			 new System.Data.Common.DataColumnMapping("CategoryID", "CategoryID"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryParentID", "CategoryParentID"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryName", "CategoryName"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryPath", "CategoryPath"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryDescription", "CategoryDescription")})});
			this.daCategory.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath, CategoryDescript" +
				"ion FROM Category WHERE (CategoryID = @categoryid)";
			this.sqlSelectCommand1.Connection = this.cn;
			this.sqlSelectCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@categoryid", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = @"INSERT INTO Category(CategoryParentID, CategoryName, CategoryPath, CategoryDescription) VALUES (@CategoryParentID, @CategoryName, @CategoryPath, @CategoryDescription); SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath, CategoryDescription FROM Category WHERE (CategoryID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDescription", System.Data.SqlDbType.NText, 16, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Current, null));
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Category SET CategoryParentID = @CategoryParentID, CategoryName = @CategoryName, CategoryPath = @CategoryPath, CategoryDescription = @CategoryDescription WHERE (CategoryID = @Original_CategoryID) AND (CategoryDescription LIKE @Original_CategoryDescription OR @Original_CategoryDescription1 IS NULL AND CategoryDescription IS NULL) AND (CategoryName = @Original_CategoryName) AND (CategoryParentID = @Original_CategoryParentID) AND (CategoryPath = @Original_CategoryPath OR @Original_CategoryPath1 IS NULL AND CategoryPath IS NULL); SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath, CategoryDescription FROM Category WHERE (CategoryID = @Select_CategoryID)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDescription", System.Data.SqlDbType.NText, 16, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryDescription", System.Data.SqlDbType.NText, 16, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryDescription1", System.Data.SqlDbType.NText, 16, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryPath1", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = @"DELETE FROM Category WHERE (CategoryID = @CategoryID) AND (CategoryDescription LIKE @CategoryDescription OR @CategoryDescription1 IS NULL AND CategoryDescription IS NULL) AND (CategoryName = @CategoryName) AND (CategoryParentID = @CategoryParentID) AND (CategoryPath = @CategoryPath OR @CategoryPath1 IS NULL AND CategoryPath IS NULL)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDescription", System.Data.SqlDbType.NText, 16, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDescription1", System.Data.SqlDbType.NText, 16, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath1", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			// 
			// fEditCategory
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(352, 166);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnCancel,
																		  this.btnOK,
																		  this.label2,
																		  this.txtDescription,
																		  this.txtCategoryName,
																		  this.label1});
			this.Name = "fEditCategory";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Category";
			this.Load += new System.EventHandler(this.fEditCategory_Load);
			((System.ComponentModel.ISupportInitialize)(this.dsCategory)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void fEditCategory_Load(object sender, System.EventArgs e)
		{

		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{

			if (mintCategoryID != 0)
			{
				this.BindingContext[dsCategory, "Category"].EndCurrentEdit();
				daCategory.Update(dsCategory, "Category");
				Visible = false;
			}
			// otherwise we want to add a record
			else
			{
				DataSetCategory.CategoryRow cr = dsCategory.Category.NewCategoryRow();
				cr.CategoryName			= txtCategoryName.Text;
				cr.CategoryDescription  = txtDescription.Text;
				cr.CategoryParentID		= mintParentCategoryID;
				dsCategory.Category.AddCategoryRow(cr);
				daCategory.Update(dsCategory, "Category");
				curCategoryRow = cr;
			}
			Visible = false;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			mblnCancel = true;
			Visible = false;
		}

		public int CategoryID 
		{
			get 
			{
				return mintCategoryID;
			}
			set 
			{
				mintCategoryID = value;
				daCategory.SelectCommand.Parameters["@CategoryID"].Value = value;
                daCategory.Fill(dsCategory, "Category");
				curCategoryRow = (DataSetCategory.CategoryRow) dsCategory.Category.Rows[0];
			}
		}

		public int ParentCategoryID 
		{
			get 
			{
				return mintParentCategoryID;
			}
			set 
			{
				mintParentCategoryID = value;
			}
		}

		public bool Cancel 
		{
			get 
			{
				return mblnCancel;
			}
		}

		public DataSetCategory.CategoryRow SelectedCategory 
		{
			get 
			{
				return curCategoryRow;
			}
		}
	}
}
