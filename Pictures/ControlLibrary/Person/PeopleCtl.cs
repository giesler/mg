using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace msn2.net.Pictures.Controls
{

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
		private msn2.net.Pictures.Controls.DataSetPerson dsPerson;
		private System.Data.DataView dvPersonFind;
		private System.Data.SqlClient.SqlDataAdapter daPerson;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        private msn2.net.Pictures.Controls.picsvc.PictureManager pictureManager1;
        private TextBox searchText;
        private SplitContainer splitContainer1;
        private FlowLayoutPanel matchList;
        private LinkLabel addLink;
        private Label matchCount;
        private Label label1;
        private FlowLayoutPanel recentList;
        private List<Person> cachedPersonList = new List<Person>();

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public PeopleCtl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            if (this.DesignMode == false)
            {
                try
                {
                    // Set the connection string
                    if (PicContext.Current != null)
                    {
                        this.sqlConnection1.ConnectionString = PicContext.Current.Config.ConnectionString;
                        this.cn.ConnectionString = PicContext.Current.Config.ConnectionString;

                        // Load all people
                        ReloadPeople();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void ReloadPeople()
        {
            dsPerson = new DataSetPerson();
            daPerson.Fill(dsPerson, "Person");

            this.cachedPersonList = (from p in PicContext.Current.DataContext.Persons
                                     select p).ToList<Person>();

            this.AddPersonList(PicContext.Current.UserManager.GetRecentUsers(), this.recentList);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PeopleCtl));
            this.cn = new System.Data.SqlClient.SqlConnection();
            this.dvPersonFullName = new System.Data.DataView();
            this.dsPerson = new msn2.net.Pictures.Controls.DataSetPerson();
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
            this.pictureManager1 = new msn2.net.Pictures.Controls.picsvc.PictureManager();
            this.searchText = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.matchList = new System.Windows.Forms.FlowLayoutPanel();
            this.addLink = new System.Windows.Forms.LinkLabel();
            this.matchCount = new System.Windows.Forms.Label();
            this.recentList = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonFullName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsPerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonFirstName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonLastName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonFind)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cn
            // 
            this.cn.ConnectionString = "data source=picdbserver;initial catalog=picdb;integrated security=SSPI;persist se" +
                "curity info=False;workstation id=CHEF;packet size=4096";
            this.cn.FireInfoMessageEventOnUserErrors = false;
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
            this.dsPerson.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.sqlDeleteCommand1.CommandText = resources.GetString("sqlDeleteCommand1.CommandText");
            this.sqlDeleteCommand1.Connection = this.sqlConnection1;
            this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FullName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FullName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "LastName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "LastName", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "data source=picdbserver;integrated security=sspi;initial catalog=picdb;persist se" +
                "curity info=False";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = "INSERT INTO Person(LastName, FirstName, FullName) VALUES (@LastName, @FirstName, " +
                "@FullName); SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (Pe" +
                "rsonID = @@IDENTITY)";
            this.sqlInsertCommand1.Connection = this.sqlConnection1;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "LastName", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FullName", System.Data.DataRowVersion.Current, null)});
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "SELECT PersonID, LastName, FirstName, FullName FROM Person";
            this.sqlSelectCommand1.Connection = this.sqlConnection1;
            // 
            // sqlUpdateCommand1
            // 
            this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
            this.sqlUpdateCommand1.Connection = this.sqlConnection1;
            this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "LastName", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FullName", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FullName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "FullName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "LastName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((byte)(0)), ((byte)(0)), "LastName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, "PersonID")});
            // 
            // pictureManager1
            // 
            this.pictureManager1.Credentials = null;
            this.pictureManager1.Url = "http://www.msn2.net/Pictures/PictureManager.asmx";
            this.pictureManager1.UseDefaultCredentials = false;
            // 
            // searchText
            // 
            this.searchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.searchText.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.searchText.Location = new System.Drawing.Point(4, 4);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(249, 20);
            this.searchText.TabIndex = 11;
            this.searchText.Text = "<enter name>";
            this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            this.searchText.Enter += new System.EventHandler(this.searchText_Enter);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(7, 33);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.matchList);
            this.splitContainer1.Panel1.Controls.Add(this.addLink);
            this.splitContainer1.Panel1.Controls.Add(this.matchCount);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.recentList);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(246, 276);
            this.splitContainer1.SplitterDistance = 122;
            this.splitContainer1.TabIndex = 14;
            // 
            // matchList
            // 
            this.matchList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.matchList.AutoScroll = true;
            this.matchList.BackColor = System.Drawing.SystemColors.Window;
            this.matchList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.matchList.Location = new System.Drawing.Point(0, 24);
            this.matchList.Name = "matchList";
            this.matchList.Size = new System.Drawing.Size(246, 95);
            this.matchList.TabIndex = 0;
            // 
            // addLink
            // 
            this.addLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addLink.Location = new System.Drawing.Point(169, 8);
            this.addLink.Name = "addLink";
            this.addLink.Size = new System.Drawing.Size(74, 13);
            this.addLink.TabIndex = 15;
            this.addLink.TabStop = true;
            this.addLink.Text = "&Add new...";
            this.addLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addLink_LinkClicked);
            // 
            // matchCount
            // 
            this.matchCount.Location = new System.Drawing.Point(3, 8);
            this.matchCount.Name = "matchCount";
            this.matchCount.Size = new System.Drawing.Size(160, 16);
            this.matchCount.TabIndex = 14;
            this.matchCount.Text = "Enter text above to search";
            // 
            // recentList
            // 
            this.recentList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.recentList.AutoScroll = true;
            this.recentList.BackColor = System.Drawing.SystemColors.Window;
            this.recentList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.recentList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.recentList.Location = new System.Drawing.Point(0, 20);
            this.recentList.Name = "recentList";
            this.recentList.Size = new System.Drawing.Size(246, 127);
            this.recentList.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Recently selected";
            // 
            // PeopleCtl
            // 
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.searchText);
            this.Name = "PeopleCtl";
            this.Size = new System.Drawing.Size(259, 313);
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonFullName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsPerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonFirstName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonLastName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPersonFind)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void menuAddPerson_Click(object sender, System.EventArgs e)
		{
			fEditPerson p = new fEditPerson();
			p.NewPerson();
			p.ShowDialog();

			if (!p.Cancel) 
			{
				DataSetPerson.PersonRow pr = p.SelectedPerson;

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

                this.selectedPerson = prNew;

                FireSelectPerson();

                this.ReloadPeople();
			}


		}

        private void FireSelectPerson()
        {
            if (DoubleClickPerson != null)
            {
                DoubleClickPerson(this, new PersonCtlEventArgs() { personRow = selectedPerson });
            }

            LinkLabel match = null;
            foreach (LinkLabel ll in this.recentList.Controls)
            {
                int id = (int)ll.Tag;
                if (id == selectedPerson.PersonID)
                {
                    match = ll;
                    break;
                }
            }

            if (match == null)
            {
                match = CreatePersonLabel(selectedPerson.FullName, selectedPerson.PersonID);
                this.recentList.Controls.Add(match);
                match.BringToFront();
            }
            else
            {
                match.BringToFront();
            }
        }

		private void menuEditPerson_Click(object sender, System.EventArgs e)
		{
            //ListViewItem person = null;

            //DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) person.Tag;
            //fEditPerson p = new fEditPerson();
            //p.PersonID = pr.PersonID;

            //p.ShowDialog();

            //if (!p.Cancel) 
            //{
            //    pr = p.SelectedPerson;
            //}

		}

		private void menuDeletePerson_Click(object sender, System.EventArgs e)
		{
            //if (lvBrowse.SelectedItems.Count == 0) 
            //{
            //    MessageBox.Show("You must select a person to delete.", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

            //ListViewItem item = lvBrowse.SelectedItems[0];

            //// make sure we want to delete
            //if (MessageBox.Show("Would you like to delete '" + item.Text + "'?", 
            //    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
            //{
            //    DataSetPerson.PersonRow pr = (DataSetPerson.PersonRow) item.Tag;
            //    pr.Delete();
            //    daPerson.Update(dsPerson, "Person");

            //    lvBrowse.Items.Remove(lvBrowse.SelectedItems[0]);
            //}
		}


		public DataSetPerson.PersonRow FindPersonInfo(int PersonID) 
		{
			return (dsPerson.Person.FindByPersonID(PersonID));
		}

        private DataSetPerson.PersonRow selectedPerson = null;

		public DataSetPerson.PersonRow SelectedPerson
		{
			get 
			{
                return selectedPerson;
			}

		}

		// events
		public event ClickPersonEventHandler ClickPerson;
		public event DoubleClickPersonEventHandler DoubleClickPerson;

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            string text = searchText.Text.ToLower();

            List<Person> list = (from p in this.cachedPersonList
                    where p.FirstName.ToLower().Contains(text) 
                        || p.LastName.ToLower().Contains(text) 
                        || p.FullName.ToLower().Contains(text)
                    select p).ToList<Person>();

            AddPersonList(list, this.matchList);

            this.matchCount.Text = "Found " + list.Count.ToString() + " matches";
        }

        private void AddPersonList(List<Person> list, Control parentControl)
        {
            parentControl.Controls.Clear();
            foreach (Person p in list)
            {
                string fullName = p.FullName;
                int id = p.PersonID;

                LinkLabel ll = CreatePersonLabel(fullName, id);
                parentControl.Controls.Add(ll);
            }
        }

        private LinkLabel CreatePersonLabel(string fullName, int id)
        {
            LinkLabel ll = new LinkLabel();
            ll.Width = this.matchList.ClientSize.Width;
            ll.Height = 14;
            ll.Anchor = AnchorStyles.Left & AnchorStyles.Right;
            ll.Text = fullName;
            ll.Tag = id;
            ll.Click += new EventHandler(ll_Click);
            return ll;
        }

        void ll_Click(object sender, EventArgs e)
        {
            LinkLabel ll = (LinkLabel)sender;
            selectedPerson = FindPersonInfo((int)ll.Tag);
            this.FireSelectPerson();
        }

        private void addLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            menuAddPerson_Click(null, null);
        }

        private void searchText_Enter(object sender, EventArgs e)
        {
            if (this.searchText.Text.Equals("<enter name>"))
            {
                this.searchText.Text = "";
                this.searchText.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            }

            this.searchText.SelectionStart = 0;
            this.searchText.SelectionLength = this.searchText.Text.Length;
        }
	}

	// events
	public delegate void ClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	public delegate void DoubleClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	
	// class for passing events up
	public class PersonCtlEventArgs: EventArgs 
	{
		public DataSetPerson.PersonRow personRow;
	}

}
