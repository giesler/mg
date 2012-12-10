using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
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

        private Category category;

        protected bool mblnCancel = false;
		protected int mintCategoryID = 0;
		protected int mintParentCategoryID = 0;
		protected DataSetCategory.CategoryRow curCategoryRow;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Data.SqlClient.SqlDataAdapter daCategory;
		private msn2.net.Pictures.Controls.DataSetCategory dsCategory;
		private msn2.net.Pictures.Controls.GroupPicker groupPicker1;
		private System.Data.SqlClient.SqlDataAdapter daCategoryGroup;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlConnection sqlConnection1;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.CheckBox checkBoxPublish;
		private System.Windows.Forms.Label label3;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;

		private bool buttonPushed = false;
        private System.Windows.Forms.DateTimePicker categoryDatePicker;
        private IContainer components;

		public fEditCategory()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set the connection string
            if (PicContext.Current != null)
            {
                cn.ConnectionString = PicContext.Current.Config.ConnectionString;
                sqlConnection1.ConnectionString = PicContext.Current.Config.ConnectionString;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fEditCategory));
            this.cn = new System.Data.SqlClient.SqlConnection();
            this.dsCategory = new msn2.net.Pictures.Controls.DataSetCategory();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.daCategory = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.daCategoryGroup = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
            this.txtCategoryName = new System.Windows.Forms.TextBox();
            this.groupPicker1 = new msn2.net.Pictures.Controls.GroupPicker();
            this.checkBoxPublish = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.categoryDatePicker = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dsCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // cn
            // 
            this.cn.FireInfoMessageEventOnUserErrors = false;
            // 
            // dsCategory
            // 
            this.dsCategory.DataSetName = "DataSetCategory";
            this.dsCategory.Locale = new System.Globalization.CultureInfo("en-US");
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Description:";
            // 
            // sqlSelectCommand2
            // 
            this.sqlSelectCommand2.CommandText = "SELECT CategoryGroupID, CategoryID, GroupID FROM CategoryGroup WHERE (CategoryID " +
                "= @CategoryID)";
            this.sqlSelectCommand2.Connection = this.sqlConnection1;
            this.sqlSelectCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null)});
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
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
                        new System.Data.Common.DataColumnMapping("CategoryDescription", "CategoryDescription"),
                        new System.Data.Common.DataColumnMapping("CategoryDate", "CategoryDate"),
                        new System.Data.Common.DataColumnMapping("Publish", "Publish")})});
            this.daCategory.UpdateCommand = this.sqlUpdateCommand1;
            // 
            // sqlDeleteCommand1
            // 
            this.sqlDeleteCommand1.CommandText = resources.GetString("sqlDeleteCommand1.CommandText");
            this.sqlDeleteCommand1.Connection = this.cn;
            this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Publish", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
            this.sqlInsertCommand1.Connection = this.cn;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, "CategoryParentID"),
            new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, "CategoryName"),
            new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 2500, "CategoryPath"),
            new System.Data.SqlClient.SqlParameter("@CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, "CategoryDescription"),
            new System.Data.SqlClient.SqlParameter("@CategoryDate", System.Data.SqlDbType.DateTime, 4, "CategoryDate"),
            new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish")});
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath, CategoryDescript" +
                "ion, CategoryDate, Publish FROM Category WHERE (CategoryID = @categoryid)";
            this.sqlSelectCommand1.Connection = this.cn;
            this.sqlSelectCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@categoryid", System.Data.SqlDbType.Int, 4, "CategoryID")});
            // 
            // sqlUpdateCommand1
            // 
            this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
            this.sqlUpdateCommand1.Connection = this.cn;
            this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, "CategoryParentID"),
            new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, "CategoryName"),
            new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 2500, "CategoryPath"),
            new System.Data.SqlClient.SqlParameter("@CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, "CategoryDescription"),
            new System.Data.SqlClient.SqlParameter("@CategoryDate", System.Data.SqlDbType.DateTime, 4, "CategoryDate"),
            new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryDate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Publish", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID")});
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(256, 328);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.AcceptsReturn = true;
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsCategory, "Category.CategoryDescription", true));
            this.txtDescription.Location = new System.Drawing.Point(88, 56);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(320, 88);
            this.txtDescription.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(336, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // daCategoryGroup
            // 
            this.daCategoryGroup.DeleteCommand = this.sqlDeleteCommand2;
            this.daCategoryGroup.InsertCommand = this.sqlInsertCommand2;
            this.daCategoryGroup.SelectCommand = this.sqlSelectCommand2;
            this.daCategoryGroup.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "CategoryGroup", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("CategoryGroupID", "CategoryGroupID"),
                        new System.Data.Common.DataColumnMapping("CategoryID", "CategoryID"),
                        new System.Data.Common.DataColumnMapping("GroupID", "GroupID")})});
            this.daCategoryGroup.UpdateCommand = this.sqlUpdateCommand2;
            // 
            // sqlDeleteCommand2
            // 
            this.sqlDeleteCommand2.CommandText = resources.GetString("sqlDeleteCommand2.CommandText");
            this.sqlDeleteCommand2.Connection = this.sqlConnection1;
            this.sqlDeleteCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryGroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@CategoryID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlInsertCommand2
            // 
            this.sqlInsertCommand2.CommandText = "INSERT INTO CategoryGroup(CategoryID, GroupID) VALUES (@CategoryID, @GroupID); SE" +
                "LECT CategoryGroupID, CategoryID, GroupID FROM CategoryGroup WHERE (CategoryGrou" +
                "pID = @@IDENTITY)";
            this.sqlInsertCommand2.Connection = this.sqlConnection1;
            this.sqlInsertCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null)});
            // 
            // sqlUpdateCommand2
            // 
            this.sqlUpdateCommand2.CommandText = resources.GetString("sqlUpdateCommand2.CommandText");
            this.sqlUpdateCommand2.Connection = this.sqlConnection1;
            this.sqlUpdateCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryGroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Select_CategoryGroupID", System.Data.SqlDbType.Int, 4, "CategoryGroupID")});
            // 
            // txtCategoryName
            // 
            this.txtCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCategoryName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsCategory, "Category.CategoryName", true));
            this.txtCategoryName.Location = new System.Drawing.Point(88, 8);
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Size = new System.Drawing.Size(320, 20);
            this.txtCategoryName.TabIndex = 1;
            this.txtCategoryName.Validated += new System.EventHandler(this.txtCategoryName_Validated);
            this.txtCategoryName.Validating += new System.ComponentModel.CancelEventHandler(this.txtCategoryName_Validating);
            // 
            // groupPicker1
            // 
            this.groupPicker1.AllowRemoveEveryone = true;
            this.groupPicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPicker1.Location = new System.Drawing.Point(16, 167);
            this.groupPicker1.Name = "groupPicker1";
            this.groupPicker1.RightListColumnHeaderText = "Shared With";
            this.groupPicker1.Size = new System.Drawing.Size(392, 147);
            this.groupPicker1.TabIndex = 7;
            this.groupPicker1.RemovedGroup += new msn2.net.Pictures.Controls.RemovedGroupEventHandler(this.groupPicker1_RemovedGroup);
            this.groupPicker1.AddedGroup += new msn2.net.Pictures.Controls.AddedGroupEventHandler(this.groupPicker1_AddedGroup);
            // 
            // checkBoxPublish
            // 
            this.checkBoxPublish.Checked = true;
            this.checkBoxPublish.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPublish.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.dsCategory, "Category.Publish", true));
            this.checkBoxPublish.Location = new System.Drawing.Point(88, 136);
            this.checkBoxPublish.Name = "checkBoxPublish";
            this.checkBoxPublish.Size = new System.Drawing.Size(320, 24);
            this.checkBoxPublish.TabIndex = 6;
            this.checkBoxPublish.Text = "Publish";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date:";
            // 
            // categoryDatePicker
            // 
            this.categoryDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryDatePicker.Location = new System.Drawing.Point(88, 32);
            this.categoryDatePicker.Name = "categoryDatePicker";
            this.categoryDatePicker.ShowCheckBox = true;
            this.categoryDatePicker.Size = new System.Drawing.Size(320, 20);
            this.categoryDatePicker.TabIndex = 3;
            // 
            // fEditCategory
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(424, 358);
            this.Controls.Add(this.categoryDatePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxPublish);
            this.Controls.Add(this.groupPicker1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtCategoryName);
            this.Controls.Add(this.label1);
            this.Name = "fEditCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Category";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.fEditCategory_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.dsCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private void btnOK_Click(object sender, System.EventArgs e)
		{

			buttonPushed = true;

			if (categoryDatePicker.Checked) 
			{
				curCategoryRow.CategoryDate = categoryDatePicker.Value;
			} 
			else 
			{
				curCategoryRow.SetCategoryDateNull();
			}

			this.BindingContext[dsCategory, "Category"].EndCurrentEdit();
			daCategory.Update(dsCategory, "Category");
			daCategoryGroup.Update(dsCategory, "CategoryGroup");

            DataSetCategory.CategoryRow row = dsCategory.Category.Rows[0] as DataSetCategory.CategoryRow;
            int categoryId = row.CategoryID;
            this.category = PicContext.Current.CategoryManager.GetCategory(categoryId);

            this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			buttonPushed = true;

			mblnCancel = true;
            this.Close();
		}

		private void groupPicker1_AddedGroup(object sender, msn2.net.Pictures.Controls.GroupPickerEventArgs e)
		{
			// add the group to the PictureGroup dataset
			dsCategory.CategoryGroup.AddCategoryGroupRow(
				(DataSetCategory.CategoryRow) dsCategory.Category.Rows[0], e.GroupID);

		}

		private void groupPicker1_RemovedGroup(object sender, msn2.net.Pictures.Controls.GroupPickerEventArgs e)
		{
			int categoryId = dsCategory.Category[0].CategoryID;

			foreach (DataSetCategory.CategoryGroupRow row in dsCategory.CategoryGroup.Rows) 
			{
				if (row.CategoryID == categoryId && row.GroupID == e.GroupID) 
				{
					row.Delete();
					break;
				}
			}
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
				daCategoryGroup.SelectCommand.Parameters["@CategoryID"].Value = value;
				daCategoryGroup.Fill(dsCategory, "CategoryGroup");
				curCategoryRow = (DataSetCategory.CategoryRow) dsCategory.Category.Rows[0];

				if (!curCategoryRow.IsCategoryDateNull()) 
				{ 
					categoryDatePicker.Checked = true;
					categoryDatePicker.Value = curCategoryRow.CategoryDate;
				} 
				else 
				{
					categoryDatePicker.Checked = false;
				}

				// load selected groups
				groupPicker1.ClearSelectedGroups();
				foreach (DataSetCategory.CategoryGroupRow groupRow in dsCategory.CategoryGroup.Rows) 
				{
					groupPicker1.AddSelectedGroup(groupRow.GroupID);
				}
			}
		}

		public void NewCategory(int parentCategoryID) 
		{
			mintParentCategoryID = parentCategoryID;

			// create a new row
			curCategoryRow = dsCategory.Category.NewCategoryRow();
			curCategoryRow.CategoryName = "(new)";
			curCategoryRow.CategoryParentID = parentCategoryID;
			dsCategory.Category.AddCategoryRow(curCategoryRow);
            
			// add everyone group
			groupPicker1.AddSelectedGroup(1);
			dsCategory.CategoryGroup.AddCategoryGroupRow(curCategoryRow, 1);

		}

		private void txtCategoryName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try 
			{
				if (txtCategoryName.Text.Length == 0) 
				{
					throw new Exception("Category name is required!");
				}
			} 
			catch (Exception ex) 
			{
                errorProvider1.SetError(txtCategoryName, ex.Message);
				e.Cancel = true;
			}
		}

		private void txtCategoryName_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(txtCategoryName, "");
		}

		private void fEditCategory_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!buttonPushed)
                btnCancel_Click(sender, e);				
		}

		public int ParentCategoryID 
		{
			get 
			{
				return mintParentCategoryID;
			}
		}

		public bool Cancel 
		{
			get 
			{
				return mblnCancel;
			}
		}

		public Category SelectedCategory 
		{
			get 
			{
				return category;
			}
		}
	}
}
