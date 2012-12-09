using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PicAdmin
{
	// events
	public delegate void ClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	public delegate void DoubleClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	
	// class for passing events up
	public class PersonCtlEventArgs: EventArgs 
	{
		public DataSetPerson.PersonRow personRow;
	}

	/// <summary>
	/// Summary description for PeopleCtl.
	/// </summary>
	public class PeopleCtl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Data.DataView dvPersonFullName;
		private System.Data.DataView dvPersonLastName;
		private System.Data.DataView dvPersonFirstName;
		private System.Windows.Forms.MenuItem menuAddPerson;
		private System.Windows.Forms.MenuItem menuEditPerson;
		private System.Windows.Forms.MenuItem menuDeletePerson;
		private TreeNode nDefaultAddPoint;
		private System.Windows.Forms.TreeView tvPerson;
		private PicAdmin.DataSetPerson dsPerson;
		private System.Data.DataView dvPersonFind;
		private System.Data.SqlClient.SqlDataAdapter daPerson;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Data.SqlClient.SqlConnection sqlConnection1;
		private System.Windows.Forms.TextBox findString;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;


		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public PeopleCtl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Set the connection string
			cn.ConnectionString = "data source=kyle;initial catalog=picdb;user id=sa;password=too;persist security info=False";

			// Load all people
			daPerson.Fill(dsPerson, "Person");

			// load initial tree state
			TreeNode n;
			TreeNode nRoot = new TreeNode("People");
			nRoot.Tag = "";
			tvPerson.Nodes.Add (nRoot);
			nRoot.Tag = "root";
			nRoot.Expand();
			n = nRoot.Nodes.Add("By Name");
			nDefaultAddPoint = n;
			n.Tag = "FullName";
			n.Nodes.Add("<to load>");
			n = nRoot.Nodes.Add("By First Name");
			n.Tag = "FirstName";
			n.Nodes.Add("<to load>");
			n = nRoot.Nodes.Add("By Last Name");
			n.Tag = "LastName";
			n.Nodes.Add("<to load>");
			n = nRoot.Nodes.Add("Find");
			n.Tag = "Find";
			
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
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.dvPersonFullName = new System.Data.DataView();
			this.dsPerson = new PicAdmin.DataSetPerson();
			this.tvPerson = new System.Windows.Forms.TreeView();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuAddPerson = new System.Windows.Forms.MenuItem();
			this.menuEditPerson = new System.Windows.Forms.MenuItem();
			this.menuDeletePerson = new System.Windows.Forms.MenuItem();
			this.dvPersonFirstName = new System.Data.DataView();
			this.dvPersonLastName = new System.Data.DataView();
			this.dvPersonFind = new System.Data.DataView();
			this.daPerson = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.findString = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFullName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFirstName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonLastName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFind)).BeginInit();
			this.SuspendLayout();
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// dvPersonFullName
			// 
			this.dvPersonFullName.RowFilter = "FullName IS NOT NULL";
			this.dvPersonFullName.Sort = "FullName";
			this.dvPersonFullName.Table = this.dsPerson.Person;
			// 
			// dsPerson
			// 
			this.dsPerson.DataSetName = "DataSetPicture";
			this.dsPerson.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsPerson.Namespace = "http://www.tempuri.org/DataSetPicture.xsd";
			// 
			// tvPerson
			// 
			this.tvPerson.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.tvPerson.ContextMenu = this.contextMenu1;
			this.tvPerson.FullRowSelect = true;
			this.tvPerson.HideSelection = false;
			this.tvPerson.ImageIndex = -1;
			this.tvPerson.Location = new System.Drawing.Point(0, 24);
			this.tvPerson.Name = "tvPerson";
			this.tvPerson.SelectedImageIndex = -1;
			this.tvPerson.Size = new System.Drawing.Size(184, 128);
			this.tvPerson.TabIndex = 0;
			this.tvPerson.DoubleClick += new System.EventHandler(this.tvPerson_DoubleClick);
			this.tvPerson.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPerson_AfterSelect);
			this.tvPerson.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvPerson_BeforeExpand);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuAddPerson,
																						 this.menuEditPerson,
																						 this.menuDeletePerson});
			// 
			// menuAddPerson
			// 
			this.menuAddPerson.Index = 0;
			this.menuAddPerson.Text = "&Add Person";
			this.menuAddPerson.Click += new System.EventHandler(this.menuAddPerson_Click);
			// 
			// menuEditPerson
			// 
			this.menuEditPerson.Index = 1;
			this.menuEditPerson.Text = "&Edit Person";
			this.menuEditPerson.Click += new System.EventHandler(this.menuEditPerson_Click);
			// 
			// menuDeletePerson
			// 
			this.menuDeletePerson.Index = 2;
			this.menuDeletePerson.Text = "&Delete Person";
			this.menuDeletePerson.Click += new System.EventHandler(this.menuDeletePerson_Click);
			// 
			// dvPersonFirstName
			// 
			this.dvPersonFirstName.RowFilter = "FirstName IS NOT NULL";
			this.dvPersonFirstName.Sort = "FirstName";
			this.dvPersonFirstName.Table = this.dsPerson.Person;
			// 
			// dvPersonLastName
			// 
			this.dvPersonLastName.RowFilter = "LastName IS NOT NULL";
			this.dvPersonLastName.Sort = "LastName, FirstName";
			this.dvPersonLastName.Table = this.dsPerson.Person;
			// 
			// dvPersonFind
			// 
			this.dvPersonFind.Table = this.dsPerson.Person;
			// 
			// daPerson
			// 
			this.daPerson.DeleteCommand = this.sqlDeleteCommand1;
			this.daPerson.InsertCommand = this.sqlInsertCommand1;
			this.daPerson.SelectCommand = this.sqlSelectCommand1;
			this.daPerson.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							   new System.Data.Common.DataTableMapping("Table", "Person", new System.Data.Common.DataColumnMapping[] {
																																																		 new System.Data.Common.DataColumnMapping("PersonID", "PersonID"),
																																																		 new System.Data.Common.DataColumnMapping("LastName", "LastName"),
																																																		 new System.Data.Common.DataColumnMapping("FirstName", "FirstName"),
																																																		 new System.Data.Common.DataColumnMapping("FullName", "FullName")})});
			this.daPerson.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = @"DELETE FROM Person WHERE (PersonID = @PersonID) AND (FirstName = @FirstName OR @FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @FullName OR @FullName1 IS NULL AND FullName IS NULL) AND (LastName = @LastName OR @LastName1 IS NULL AND LastName IS NULL)";
			this.sqlDeleteCommand1.Connection = this.sqlConnection1;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			// 
			// sqlConnection1
			// 
			this.sqlConnection1.ConnectionString = "data source=kyle;initial catalog=picdb;password=tOO;persist security info=True;us" +
				"er id=sa;workstation id=CHEF;packet size=4096";
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO Person(LastName, FirstName, FullName) VALUES (@LastName, @FirstName, " +
				"@FullName); SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (Pe" +
				"rsonID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.sqlConnection1;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT PersonID, LastName, FirstName, FullName FROM Person";
			this.sqlSelectCommand1.Connection = this.sqlConnection1;
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Person SET LastName = @LastName, FirstName = @FirstName, FullName = @FullName WHERE (PersonID = @Original_PersonID) AND (FirstName = @Original_FirstName OR @Original_FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @Original_FullName OR @Original_FullName1 IS NULL AND FullName IS NULL) AND (LastName = @Original_LastName OR @Original_LastName1 IS NULL AND LastName IS NULL); SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (PersonID = @Select_PersonID)";
			this.sqlUpdateCommand1.Connection = this.sqlConnection1;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, "PersonID"));
			// 
			// findString
			// 
			this.findString.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.findString.Location = new System.Drawing.Point(32, 0);
			this.findString.Name = "findString";
			this.findString.Size = new System.Drawing.Size(88, 20);
			this.findString.TabIndex = 1;
			this.findString.Text = "<enter name>";
			this.findString.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.findString_KeyPress);
			this.findString.Enter += new System.EventHandler(this.findString_Enter);
			// 
			// label1
			// 
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Find:";
			// 
			// button1
			// 
			this.button1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button1.Location = new System.Drawing.Point(136, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(48, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Find";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// PeopleCtl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.findString,
																		  this.button1,
																		  this.label1,
																		  this.tvPerson});
			this.Name = "PeopleCtl";
			this.Size = new System.Drawing.Size(184, 160);
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFullName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFirstName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonLastName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvPersonFind)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void tvPerson_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "<to load>")
                FillChildren(e.Node);
		}

		private void FillChildren(TreeNode n) 
		{
			// clear out all child nodes
			n.Nodes.Clear();

			TreeNode nChild;
			DataView dv = dvPersonFullName;

			// select the appropriate view
			if (n.Tag.ToString() == "FullName")
				dv = dvPersonFullName;
			else if (n.Tag.ToString() == "FirstName")
				dv = dvPersonFirstName;
			else if (n.Tag.ToString() == "LastName")
				dv = dvPersonLastName;
			else if (n.Tag.ToString() == "Find")
				dv = dvPersonFind;
			

			foreach (DataRowView dr in dv) 
			{
				// add this row as a node
				DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) dr.Row;
				if (n.Tag.ToString() == "FullName" || n.Tag.ToString() == "Find")
					nChild = n.Nodes.Add(pr.FullName);
				else if (n.Tag.ToString() == "FirstName")
					nChild = n.Nodes.Add(pr.FirstName + " " + pr.LastName);
				else //if (n.Tag.ToString() == "LastName")
					nChild = n.Nodes.Add(pr.LastName + ", " + pr.FirstName);
				nChild.Tag = pr;

			}

		}

		private void menuAddPerson_Click(object sender, System.EventArgs e)
		{

			TreeNode n;
			n = tvPerson.SelectedNode;

			// make sure at one of the roots...
			if (n.Parent != tvPerson.Nodes[0])
				n = n.Parent;
			
			if (n == null) 
			{
				n = nDefaultAddPoint;
			}


			fEditPerson p = new fEditPerson();
			p.NewPerson();
			p.ShowDialog();

			if (!p.Cancel) 
			{
				// add new tree node
				TreeNode nChild;
				DataSetPerson.PersonRow pr = p.SelectedPerson;

				// fill in node
				if (n.Tag.ToString() == "FullName" || n.Tag.ToString() == "Find")
					nChild = n.Nodes.Add(pr.FullName);
				else if (n.Tag.ToString() == "FirstName")
					nChild = n.Nodes.Add(pr.FirstName + " " + pr.LastName);
				else //if (n.Tag.ToString() == "LastName")
					nChild = n.Nodes.Add(pr.LastName + ", " + pr.FirstName);

				// add the new row to our ds
				DataSetPerson.PersonRow prNew = dsPerson.Person.NewPersonRow();
				if (!pr.IsLastNameNull())
					prNew.LastName = pr.LastName;
				if (!pr.IsFirstNameNull())
					prNew.FirstName = pr.FirstName;
				if (!pr.IsFullNameNull())
					prNew.FullName  = pr.FullName;
				prNew.PersonID  = pr.PersonID;
				dsPerson.Person.AddPersonRow(prNew);

				// get key and update node tag
				nChild.Tag = prNew;

				// expand parent node and select new node
				if (!n.IsExpanded) 
				{
					n.Expand();
					FillChildren(n);
				}
				tvPerson.SelectedNode = nChild;
			}


		}

		private void menuEditPerson_Click(object sender, System.EventArgs e)
		{
			TreeNode n;
			n = tvPerson.SelectedNode;
			if (n == null) 
			{
				MessageBox.Show("You must select a person to edit.", "Edit Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			
			if (n.Tag.ToString().Equals("FullName") || n.Tag.ToString().Equals("FirstName") || 
					n.Tag.ToString().Equals("LastName") || n.Tag.ToString().Equals("root"))
				return;

			DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) n.Tag;
			fEditPerson p = new fEditPerson();
			p.PersonID = pr.PersonID;

			p.ShowDialog();

			if (!p.Cancel) 
			{
				pr = p.SelectedPerson;

				// update node
				if (n.Parent.Tag.ToString() == "FullName" || n.Parent.Tag.ToString() == "Find")
					n.Text = pr.FullName;
				else if (n.Parent.Tag.ToString() == "FirstName")
					n.Text = pr.FirstName + " " + pr.LastName;
				else if (n.Parent.Tag.ToString() == "LastName")
					n.Text = pr.LastName + ", " + pr.FirstName;


			}

		}

		private void menuDeletePerson_Click(object sender, System.EventArgs e)
		{
			TreeNode n;
			n = tvPerson.SelectedNode;
			if (n == null) 
			{
				MessageBox.Show("You must select a person to delete.", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}


			if (n.Tag.ToString().Equals("FullName") || n.Tag.ToString().Equals("FirstName") || 
				n.Tag.ToString().Equals("LastName") || n.Tag.ToString().Equals("root"))
			{
				MessageBox.Show("You must select a person to delete.", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			// make sure we want to delete
			if (MessageBox.Show("Would you like to delete '" + n.Text + "'?", 
				"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
			{
				DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) n.Tag;
				pr.Delete();
				daPerson.Update(dsPerson, "Person");
				tvPerson.Nodes.Remove(n);  
			}
		}


		public DataSetPerson.PersonRow FindPersonInfo(int PersonID) 
		{
			return (dsPerson.Person.FindByPersonID(PersonID));
		}

		public DataSetPerson.PersonRow SelectedPerson
		{
			get 
			{
				if (tvPerson.SelectedNode == null)
					return null;

				return (DataSetPerson.PersonRow) tvPerson.SelectedNode.Tag;
			}

		}

		// events
		public event ClickPersonEventHandler ClickPerson;
		public event DoubleClickPersonEventHandler DoubleClickPerson;

		private void tvPerson_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode n = tvPerson.SelectedNode;
			if (n == null)
				return;

			if (n.Tag.ToString().Equals("FullName") || n.Tag.ToString().Equals("FirstName") || 
				n.Tag.ToString().Equals("LastName") || n.Tag.ToString().Equals("root") ||
				n.Tag.ToString().Equals("Find"))
				return;
            
			// Fire event for other controls to catch if they want
			PersonCtlEventArgs ex = new PersonCtlEventArgs();
			ex.personRow = (DataSetPerson.PersonRow) n.Tag;
			
			if (DoubleClickPerson != null)
				DoubleClickPerson(this, ex);

		}

		private void tvPerson_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Action != TreeViewAction.Unknown) 
			{

				if (e.Node.Tag.ToString().Equals("FullName") || e.Node.Tag.ToString().Equals("FirstName") || 
					e.Node.Tag.ToString().Equals("LastName") || e.Node.Tag.ToString().Equals("root"))
					return;

				if (e.Node.Tag.ToString().Equals("Find")) 
				{
					fPromptText p = new fPromptText();
					p.Message = "Enter any part of the name:";
					p.ShowDialog();

					if (p.Cancel) return;

					dvPersonFind.RowFilter = "LastName like '%" + p.Value + "%' OR FirstName like '%" + p.Value + "%' OR FullName like '%" + p.Value + "%'";
					foreach (DataRowView dr in dvPersonFind) 
					{
						// add this row as a node
						DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) dr.Row;
						TreeNode nChild = e.Node.Nodes.Add(pr.FullName);
						nChild.Tag = pr;
					}
					e.Node.Expand();

					return;

				}

				// Fire event for other controls to catch if they want
				PersonCtlEventArgs ex = new PersonCtlEventArgs();
				ex.personRow = (DataSetPerson.PersonRow) e.Node.Tag;
			
				if (ClickPerson != null)
					ClickPerson(this, ex);
			}

		}

		private void findString_Enter(object sender, System.EventArgs e)
		{
			if (findString.Text.Equals("<enter name>"))
				findString.Text = "";
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			// select the 'find' node
			TreeNode n = tvPerson.Nodes[0];  // root node
			n = n.Nodes[3];  // find node
			n.Expand();
			
			string search = findString.Text;
			bool selectedPerson = false;

			dvPersonFind.RowFilter = "LastName like '%" + search + "%' OR FirstName like '%" + search + "%' OR FullName like '%" + search + "%'";
			foreach (DataRowView dr in dvPersonFind) 
			{
				// add this row as a node
				DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) dr.Row;
				TreeNode nChild = n.Nodes.Add(pr.FullName);
				nChild.Tag = pr;
				if (!selectedPerson) 
				{
					selectedPerson = true;
					tvPerson.SelectedNode = nChild;
				}
			}

		
		}

		private void findString_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13 ) 
			{
				button1_Click(sender, e);
				e.Handled = true;
			}
		}

	}
}
