using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for CategoryPicker.
	/// </summary>
	public class PersonPicker : System.Windows.Forms.UserControl
	{

		private msn2.net.Pictures.Controls.DataSetCategory dsCategory = new msn2.net.Pictures.Controls.DataSetCategory();
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView lvPeople;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private msn2.net.Pictures.Controls.PeopleCtl peopleCtl1;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PersonPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			this.peopleCtl1 = new msn2.net.Pictures.Controls.PeopleCtl();
			this.btnAdd = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lvPeople = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnRemove = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// peopleCtl1
			// 
			this.peopleCtl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.peopleCtl1.Location = new System.Drawing.Point(0, 0);
			this.peopleCtl1.Name = "peopleCtl1";
			this.peopleCtl1.Size = new System.Drawing.Size(152, 192);
			this.peopleCtl1.TabIndex = 0;
			this.peopleCtl1.DoubleClickPerson += new msn2.net.Pictures.Controls.DoubleClickPersonEventHandler(this.peopleCtl1_DoubleClickPerson);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAdd.Location = new System.Drawing.Point(4, 68);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(24, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = ">";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lvPeople);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(160, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(320, 192);
			this.panel1.TabIndex = 2;
			// 
			// lvPeople
			// 
			this.lvPeople.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.columnHeader1});
			this.lvPeople.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvPeople.FullRowSelect = true;
			this.lvPeople.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvPeople.HideSelection = false;
			this.lvPeople.Location = new System.Drawing.Point(32, 0);
			this.lvPeople.Name = "lvPeople";
			this.lvPeople.Size = new System.Drawing.Size(288, 192);
			this.lvPeople.TabIndex = 1;
			this.lvPeople.View = System.Windows.Forms.View.Details;
			this.lvPeople.DoubleClick += new System.EventHandler(this.lvPeople_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "People";
			this.columnHeader1.Width = 400;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnRemove);
			this.panel2.Controls.Add(this.btnAdd);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(32, 192);
			this.panel2.TabIndex = 0;
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRemove.Location = new System.Drawing.Point(4, 100);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(24, 23);
			this.btnRemove.TabIndex = 0;
			this.btnRemove.Text = "<";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(152, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 192);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// PersonPicker
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.peopleCtl1);
			this.Name = "PersonPicker";
			this.Size = new System.Drawing.Size(480, 192);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		
		// An event that clients can use to be notified whenever the
		// elements of the list change.
		public event AddedPersonEventHandler AddedPerson;
        public event RemovedPersonEventHandler RemovedPerson;

		public void AddSelectedPerson(int PersonID) 
		{
			DataSetPerson.PersonRow cr = peopleCtl1.FindPersonInfo(PersonID);

			if (cr == null) 
			{
				MessageBox.Show("Perosn information for id " + PersonID.ToString() + " was not found.");
				return;
			}

			ListViewItem li = lvPeople.Items.Add(cr.FullName.ToString());
			li.Tag = cr;
		}

		public void ClearSelectedPeople() 
		{
			lvPeople.Items.Clear();
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
            DataSetPerson.PersonRow cr = peopleCtl1.SelectedPerson;

			if (cr == null) 
			{
				MessageBox.Show("You must select a person!");
				return;
			}


			// make sure row isn't already added
			foreach (ListViewItem liTemp in lvPeople.Items) 
			{
				if (((DataSetPerson.PersonRow)liTemp.Tag).PersonID == cr.PersonID) 
				{
					MessageBox.Show("'" + cr.FullName + "' has already been added.","Add Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
			}
	
			ListViewItem li = lvPeople.Items.Add(cr.FullName.ToString());
			li.Tag = cr;

			PersonPickerEventArgs ex = new PersonPickerEventArgs();
			ex.PersonID = cr.PersonID;

			if (AddedPerson != null)
				AddedPerson(this, ex);

		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (lvPeople.SelectedItems.Count == 0) 
			{
				MessageBox.Show("You must select one or more people to remove!","Remove People", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			foreach (ListViewItem li in lvPeople.SelectedItems) 
			{
				PersonPickerEventArgs ex = new PersonPickerEventArgs();
				ex.PersonID = ((DataSetPerson.PersonRow)li.Tag).PersonID;
				lvPeople.Items.Remove(li);
				if (RemovedPerson != null)
					RemovedPerson (this, ex);
			}
		}

		private void peopleCtl1_DoubleClickPerson(object sender, msn2.net.Pictures.Controls.PersonCtlEventArgs e)
		{
			btnAdd_Click(sender, e );
		}

		private void lvPeople_DoubleClick(object sender, System.EventArgs e)
		{
            btnRemove_Click(sender, e);
		}

	}

	// A delegate type for hooking up change notifications.
	public delegate void AddedPersonEventHandler(object sender, PersonPickerEventArgs e);
	public delegate void RemovedPersonEventHandler(object sender, PersonPickerEventArgs e);

	// class for passing events up
	public class PersonPickerEventArgs: EventArgs 
	{
		public int PersonID;
	}



}
