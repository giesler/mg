using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace PicAdmin
{
	// A delegate type for hooking up change notifications.
	public delegate void AddedGroupEventHandler(object sender, GroupPickerEventArgs e);
	public delegate void RemovedGroupEventHandler(object sender, GroupPickerEventArgs e);

	// class for passing events up
	public class GroupPickerEventArgs: EventArgs 
	{
		public int GroupID;
	}

	/// <summary>
	/// Summary description for GroupPicker.
	/// </summary>
	public class GroupPicker : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private PicAdmin.GroupControl groupControl;
		private System.Windows.Forms.ListView selectedGroups;
		protected bool allowRemoveEveryone = true;

		public event AddedGroupEventHandler AddedGroup;
		public event RemovedGroupEventHandler RemovedGroup;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GroupPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

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
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.btnAdd = new System.Windows.Forms.Button();
			this.groupControl = new PicAdmin.GroupControl();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnRemove = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.selectedGroups = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(150, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 168);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAdd.Location = new System.Drawing.Point(4, 56);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(24, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = ">";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// groupControl
			// 
			this.groupControl.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupControl.Name = "groupControl";
			this.groupControl.Size = new System.Drawing.Size(150, 168);
			this.groupControl.TabIndex = 0;
			this.groupControl.DoubleClickGroup += new PicAdmin.DoubleClickGroupEventHandler(this.groupControl_DoubleClickGroup);
			// 
			// panel2
			// 
			this.panel2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.btnRemove,
																				 this.btnAdd});
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(32, 168);
			this.panel2.TabIndex = 0;
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRemove.Location = new System.Drawing.Point(4, 88);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(24, 23);
			this.btnRemove.TabIndex = 0;
			this.btnRemove.Text = "<";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.selectedGroups,
																				 this.panel2});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(153, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(287, 168);
			this.panel1.TabIndex = 2;
			// 
			// selectedGroups
			// 
			this.selectedGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.columnHeader1});
			this.selectedGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectedGroups.FullRowSelect = true;
			this.selectedGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.selectedGroups.HideSelection = false;
			this.selectedGroups.Location = new System.Drawing.Point(32, 0);
			this.selectedGroups.Name = "selectedGroups";
			this.selectedGroups.Size = new System.Drawing.Size(255, 168);
			this.selectedGroups.TabIndex = 1;
			this.selectedGroups.View = System.Windows.Forms.View.Details;
			this.selectedGroups.DoubleClick += new System.EventHandler(this.selectedGroups_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Selected Gruops";
			this.columnHeader1.Width = 1000;
			// 
			// GroupPicker
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.splitter1,
																		  this.groupControl});
			this.Name = "GroupPicker";
			this.Size = new System.Drawing.Size(440, 168);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			DataSetGroup.GroupRow row = groupControl.SelectedGroup;

			if (row == null) 
			{
				MessageBox.Show("You must select a group!");
				return;
			}

			// make sure row isn't already added
			foreach (ListViewItem liTemp in selectedGroups.Items) 
			{
				if (((DataSetGroup.GroupRow)liTemp.Tag).GroupID == row.GroupID) 
				{
					MessageBox.Show("'" + row.GroupName + "' has already been added.","Add Group", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
			}
	
			ListViewItem li = selectedGroups.Items.Add(row.GroupName.ToString());
			li.Tag = row;


			GroupPickerEventArgs ex = new GroupPickerEventArgs ();
			ex.GroupID = row.GroupID;

			if (AddedGroup != null)
				AddedGroup(this, ex);
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (selectedGroups.SelectedItems.Count == 0) 
			{
				MessageBox.Show("You must select one or more groups to remove!","Remove Group", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			foreach (ListViewItem li in selectedGroups.SelectedItems) 
			{
				GroupPickerEventArgs ex = new GroupPickerEventArgs();
				ex.GroupID = ((DataSetGroup.GroupRow)li.Tag).GroupID;

				// check for 'everyone' group and whether we will allow removal
				if (ex.GroupID == 1 && !allowRemoveEveryone) 
				{
					MessageBox.Show("You cannot remove the 'Everyone' group from a person!","Remove Group", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
				else 
				{
					selectedGroups.Items.Remove(li);
					if (RemovedGroup != null)
						RemovedGroup (this, ex);
				}
			}

		}

		private void selectedGroups_DoubleClick(object sender, System.EventArgs e)
		{
			btnRemove_Click(sender, e);
		}

		public void AddSelectedGroup(int groupId) 
		{
			DataSetGroup.GroupRow row = groupControl.FindGroupInfo(groupId);

			if (row == null) 
			{
				MessageBox.Show("Group information for id " + groupId.ToString() + " was not found.");
				return;
			}

			ListViewItem li = selectedGroups.Items.Add(row.GroupName.ToString());
			li.Tag = row;
		}

		public void ClearSelectedGroups() 
		{
			selectedGroups.Items.Clear();
		}

		private void groupControl_DoubleClickGroup(object sender, PicAdmin.GroupControlEventArgs e)
		{
			btnAdd_Click(sender, e);
		}

		public bool AllowRemoveEveryone 
		{
			set 
			{
				allowRemoveEveryone = value;
			}
			get 
			{
				return allowRemoveEveryone;
			}
		}

		public ArrayList SelectedGroupIds
		{
			get 
			{
				ArrayList selected = new ArrayList();

				foreach (ListViewItem item in selectedGroups.Items) 
				{
                    DataSetGroup.GroupRow row = (DataSetGroup.GroupRow) item.Tag;
					selected.Add(row.GroupID);
				}

				return selected;
			}
		}
	}
}
