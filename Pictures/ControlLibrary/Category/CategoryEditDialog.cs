using System;
using System.Drawing;
using System.Data.Linq;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
    /// Summary description for CategoryEditDialog.
	/// </summary>
	public class CategoryEditDialog : System.Windows.Forms.Form
	{
		Label label1;
		TextBox txtCategoryName;
		Label label2;
		Button btnOK;
		Button btnCancel;
        TextBox txtDescription;
        msn2.net.Pictures.Controls.GroupPicker groupPicker1;
		ErrorProvider errorProvider1;
		CheckBox checkBoxPublish;
        Label label3;
        DateTimePicker categoryDatePicker;
        IContainer components;
        PicContext context;
        Category category;
        List<int> removeList = new List<int>();

		public CategoryEditDialog(PicContext context, Category category)
		{
			InitializeComponent();

            this.context = context.Clone();
            this.category = category;

            if (this.category.Id > 0)
            {
                this.category = this.context.CategoryManager.GetCategory(this.category.Id);
            }
    
            this.txtCategoryName.Text = category.Name;
            this.txtDescription.Text = category.Description;

            foreach (CategoryGroup group in this.category.CategoryGroups)
            {
                this.groupPicker1.AddSelectedGroup(group.GroupID.Value);
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
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCategoryName = new System.Windows.Forms.TextBox();
            this.groupPicker1 = new msn2.net.Pictures.Controls.GroupPicker();
            this.checkBoxPublish = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.categoryDatePicker = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
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
            // txtCategoryName
            // 
            this.txtCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCategoryName.Location = new System.Drawing.Point(88, 8);
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Size = new System.Drawing.Size(320, 20);
            this.txtCategoryName.TabIndex = 1;
            this.txtCategoryName.Validated += new System.EventHandler(this.txtCategoryName_Validated);
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
            // 
            // checkBoxPublish
            // 
            this.checkBoxPublish.Checked = true;
            this.checkBoxPublish.CheckState = System.Windows.Forms.CheckState.Checked;
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
            // CategoryEditDialog
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
            this.Name = "CategoryEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Category";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            bool cancel = false;

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
                cancel = true;
            }

            if (cancel == false)
            {
                if (categoryDatePicker.Checked)
                {
                    this.category.Date = categoryDatePicker.Value;
                }
                else
                {
                    this.category.Date = null;
                }

                this.category.Name = this.txtCategoryName.Text;
                this.category.Description = this.txtDescription.Text;

                List<CategoryGroup> addList = new List<CategoryGroup>();
                foreach (int groupId in this.groupPicker1.SelectedGroupIds)
                {
                    if (this.category.CategoryGroups.Any(cg => cg.GroupID == groupId) == false)
                    {
                        CategoryGroup catGroup = new CategoryGroup { Category = this.category, GroupID = groupId };
                        this.category.CategoryGroups.Add(catGroup);
                    }
                }

                List<CategoryGroup> removeGroups = new List<CategoryGroup>();
                foreach (int groupId in this.removeList)
                {
                    CategoryGroup group = this.category.CategoryGroups.FirstOrDefault(cg => cg.GroupID == groupId);
                    if (group != null)
                    {
                        removeGroups.Add(group);
                    }
                }

                if (this.category.Id == 0)
                {
                    this.context.CategoryManager.Add(this.category);
                }
                else
                {
                    removeGroups.ForEach(g => this.context.CategoryManager.RemoveGroup(this.category, g));
                }

                this.context.SubmitChanges();

                this.category = this.context.CategoryManager.RefreshCategory(this.category);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            this.DialogResult = DialogResult.Cancel;
            this.Close();
		}

		private void txtCategoryName_Validated(object sender, System.EventArgs e)
		{
			errorProvider1.SetError(txtCategoryName, "");
		}

		public Category Category 
		{
			get 
			{
				return category;
			}
		}

        private void groupPicker1_RemovedGroup(object sender, GroupPickerEventArgs e)
        {
            this.removeList.Add(e.GroupID);
        }
	}
}
