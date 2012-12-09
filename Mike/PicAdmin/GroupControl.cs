using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace PicAdmin
{
	// events
	public delegate void ClickGroupEventHandler(object sender, GroupControlEventArgs e);
	public delegate void DoubleClickGroupEventHandler(object sender, GroupControlEventArgs e);
	
	// class for passing events up
	public class GroupControlEventArgs: EventArgs 
	{
		public DataSetGroup.GroupRow groupRow;
	}

	/// <summary>
	/// Summary description for GroupControl.
	/// </summary>
	public class GroupControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Data.SqlClient.SqlDataAdapter daGroup;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Data.SqlClient.SqlConnection cn;
		private PicAdmin.DataSetGroup dsGroup;
		private System.Windows.Forms.ListView groupList;
		private System.Windows.Forms.MenuItem menuAdd;
		private System.Windows.Forms.MenuItem menuEdit;
		private System.Windows.Forms.MenuItem menuDelete;

		// events from this class
		public event ClickGroupEventHandler ClickGroup;
		public event DoubleClickGroupEventHandler DoubleClickGroup;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GroupControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// load the list of people
			daGroup.Fill(dsGroup, "Group");

			// fill the listview
			foreach (DataSetGroup.GroupRow row in dsGroup.Group.Rows) 
			{
                ListViewItem item = new ListViewItem(row.GroupName);
				item.Tag = row;
				groupList.Items.Add(item);
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
			this.groupList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuAdd = new System.Windows.Forms.MenuItem();
			this.menuEdit = new System.Windows.Forms.MenuItem();
			this.menuDelete = new System.Windows.Forms.MenuItem();
			this.daGroup = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.dsGroup = new PicAdmin.DataSetGroup();
			((System.ComponentModel.ISupportInitialize)(this.dsGroup)).BeginInit();
			this.SuspendLayout();
			// 
			// groupList
			// 
			this.groupList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1});
			this.groupList.ContextMenu = this.contextMenu1;
			this.groupList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupList.FullRowSelect = true;
			this.groupList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.groupList.HideSelection = false;
			this.groupList.MultiSelect = false;
			this.groupList.Name = "groupList";
			this.groupList.Size = new System.Drawing.Size(150, 150);
			this.groupList.TabIndex = 0;
			this.groupList.View = System.Windows.Forms.View.Details;
			this.groupList.Click += new System.EventHandler(this.groupList_Click);
			this.groupList.DoubleClick += new System.EventHandler(this.groupList_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Group";
			this.columnHeader1.Width = 200;
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuAdd,
																						 this.menuEdit,
																						 this.menuDelete});
			// 
			// menuAdd
			// 
			this.menuAdd.Index = 0;
			this.menuAdd.Text = "&Add";
			this.menuAdd.Click += new System.EventHandler(this.menuAdd_Click);
			// 
			// menuEdit
			// 
			this.menuEdit.Index = 1;
			this.menuEdit.Text = "&Edit";
			this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
			// 
			// menuDelete
			// 
			this.menuDelete.Index = 2;
			this.menuDelete.Text = "&Delete";
			this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// daGroup
			// 
			this.daGroup.DeleteCommand = this.sqlDeleteCommand1;
			this.daGroup.InsertCommand = this.sqlInsertCommand1;
			this.daGroup.SelectCommand = this.sqlSelectCommand1;
			this.daGroup.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							  new System.Data.Common.DataTableMapping("Table", "Group", new System.Data.Common.DataColumnMapping[] {
																																																	   new System.Data.Common.DataColumnMapping("GroupID", "GroupID"),
																																																	   new System.Data.Common.DataColumnMapping("GroupName", "GroupName")})});
			this.daGroup.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = "DELETE FROM [Group] WHERE (GroupID = @GroupID) AND (GroupName = @GroupName OR @Gr" +
				"oupName1 IS NULL AND GroupName IS NULL)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupName", System.Data.DataRowVersion.Original, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;password=tOO;persist security info=True;us" +
				"er id=sa;workstation id=CHEF;packet size=4096";
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO [Group] (GroupName) VALUES (@GroupName); SELECT GroupID, GroupName FR" +
				"OM [Group] WHERE (GroupID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupName", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT GroupID, GroupName FROM [Group]";
			this.sqlSelectCommand1.Connection = this.cn;
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = "UPDATE [Group] SET GroupName = @GroupName WHERE (GroupID = @Original_GroupID) AND" +
				" (GroupName = @Original_GroupName OR @Original_GroupName1 IS NULL AND GroupName " +
				"IS NULL); SELECT GroupID, GroupName FROM [Group] WHERE (GroupID = @Select_GroupI" +
				"D)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			// 
			// dsGroup
			// 
			this.dsGroup.DataSetName = "DataSetGroup";
			this.dsGroup.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsGroup.Namespace = "http://www.tempuri.org/DataSetGroup.xsd";
			// 
			// GroupControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupList});
			this.Name = "GroupControl";
			((System.ComponentModel.ISupportInitialize)(this.dsGroup)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void groupList_Click(object sender, System.EventArgs e)
		{
			if (groupList.SelectedItems.Count == 0)
				return;

			ListViewItem item = groupList.SelectedItems[0];

			GroupControlEventArgs groupEventArgs = new GroupControlEventArgs();
			groupEventArgs.groupRow = (DataSetGroup.GroupRow) item.Tag;

			if (ClickGroup != null)
				ClickGroup(this, groupEventArgs);
		}

		private void groupList_DoubleClick(object sender, System.EventArgs e)
		{
			if (groupList.SelectedItems.Count == 0)
				return;

			ListViewItem item = groupList.SelectedItems[0];
			
			GroupControlEventArgs groupEventArgs = new GroupControlEventArgs();
			groupEventArgs.groupRow = (DataSetGroup.GroupRow) item.Tag;

			if (DoubleClickGroup != null)
				DoubleClickGroup(this, groupEventArgs);

		}

		private void menuAdd_Click(object sender, System.EventArgs e)
		{
			// create a prompt form to prompt for the new name
			fPromptText f = new fPromptText();
			f.FormCaption = "New Group";
			f.Message	  = "Group Name:";
			f.ShowDialog();

			if (!f.Cancel) 
			{
				// check for dupes
				foreach (ListViewItem item in groupList.Items) 
				{
					if (item.Text.Equals(f.Value)) 
					{
						MessageBox.Show("You cannot add a duplicate group name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}

				DataSetGroup.GroupRow row = dsGroup.Group.NewGroupRow();
				row.GroupName = f.Value;
				dsGroup.Group.AddGroupRow(row);
				daGroup.Update(dsGroup, "Group");

				ListViewItem newGroup = new ListViewItem(row.GroupName);
				newGroup.Tag = row;
				groupList.Items.Add(newGroup);
			}
		}

		private void menuDelete_Click(object sender, System.EventArgs e)
		{
			if (groupList.SelectedItems.Count == 0)
				return;

			ListViewItem item = groupList.SelectedItems[0];

			if (item.Text.Equals("Everyone")) 
			{
				MessageBox.Show("You cannot delete the 'Everyone' group.", "Groups", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (MessageBox.Show("Are you sure you want to delete this group?", 
				"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				// delete the selected group
                DataSetGroup.GroupRow row = (DataSetGroup.GroupRow) item.Tag;
				row.Delete();

				// update the db
				daGroup.Update(dsGroup, "Group");
			}

		}

		private void menuEdit_Click(object sender, System.EventArgs e)
		{
			if (groupList.SelectedItems.Count == 0)
				return;

			ListViewItem item = groupList.SelectedItems[0];

			if (item.Text.Equals("Everyone")) 
			{
                MessageBox.Show("You cannot modify the 'Everyone' group.", "Groups", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			DataSetGroup.GroupRow row = (DataSetGroup.GroupRow) item.Tag;

			fPromptText f = new fPromptText();
			f.FormCaption = "Edit Group";
			f.Message	  = "Group Name:";
			f.Value		  = row.GroupName;
			f.ShowDialog();

			if (!f.Cancel) 
			{
				// check for dupes
				foreach (ListViewItem itemCheck in groupList.Items) 
				{
					if (itemCheck.Text.Equals(f.Value) && itemCheck.Index != item.Index) 
					{
						MessageBox.Show("You cannot change the name to a name that already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				row.GroupName = f.Value;
				item.Text	  = f.Value;
				daGroup.Update(dsGroup, "Group");
			}

		}

		public DataSetGroup.GroupRow FindGroupInfo(int groupId) 
		{
			return (dsGroup.Group.FindByGroupID(groupId));
		}

		public DataSetGroup.GroupRow SelectedGroup 
		{
			get 
			{
				if (groupList.SelectedItems.Count == 0)
				{
					return null;
				} 
				else 
				{
					ListViewItem item = groupList.SelectedItems[0];
					return (DataSetGroup.GroupRow) item.Tag;
				}
			}
		}

	}
}
