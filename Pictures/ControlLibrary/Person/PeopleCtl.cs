using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace msn2.net.Pictures.Controls
{

	/// <summary>
	/// Summary description for PeopleCtl.
	/// </summary>
	public class PeopleCtl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuAddPerson;
		private System.Windows.Forms.MenuItem menuEditPerson;
		private System.Windows.Forms.MenuItem menuDeletePerson;
        private msn2.net.Pictures.Controls.picsvc.PictureManager pictureManager1;
        private TextBox searchText;
        private SplitContainer splitContainer1;
        private FlowLayoutPanel matchList;
        private LinkLabel addLink;
        private Label matchCount;
        private Label label1;
        private FlowLayoutPanel recentList;
        private List<Person> cachedPersonList;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public PeopleCtl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.DesignMode == false)
            {
                // Load all people
                ReloadPeople();
            }
        }

        private void ReloadPeople()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ReloadThread), null);
        }

        private void ReloadThread(object state)
        {
            this.cachedPersonList = (from p in PicContext.Current.DataContext.Persons
                                     select p).ToList<Person>();
        
            this.Invoke(new MethodInvoker(delegate (){
                this.AddPersonList(PicContext.Current.UserManager.GetRecentUsers(), this.recentList);
                this.searchText.Focus();
            }));
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
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuAddPerson = new System.Windows.Forms.MenuItem();
            this.menuEditPerson = new System.Windows.Forms.MenuItem();
            this.menuDeletePerson = new System.Windows.Forms.MenuItem();
            this.pictureManager1 = new msn2.net.Pictures.Controls.picsvc.PictureManager();
            this.searchText = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.matchList = new System.Windows.Forms.FlowLayoutPanel();
            this.addLink = new System.Windows.Forms.LinkLabel();
            this.matchCount = new System.Windows.Forms.Label();
            this.recentList = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.matchList.Paint += new System.Windows.Forms.PaintEventHandler(this.matchList_Paint);
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
                Person person = new Person();
                if (p.SelectedPerson.IsLastNameNull() == false)
                {
                    person.LastName = p.SelectedPerson.LastName;
                }
                if (p.SelectedPerson.IsFirstNameNull() == false)
                {
                    person.FirstName = p.SelectedPerson.FirstName;
                }
                if (p.SelectedPerson.IsFullNameNull() == false)
                {
                    person.FullName = p.SelectedPerson.FullName;
                }
                if (p.SelectedPerson.IsEmailNull() == false)
                {
                    person.Email = p.SelectedPerson.Email;
                }

                PicContext.Current.UserManager.AddPerson(person);

                this.selectedPerson = person;

                FireSelectPerson();

                this.ReloadPeople();
			}


		}

        private void FireSelectPerson()
        {
            if (DoubleClickPerson != null)
            {
                DoubleClickPerson(this, new PersonCtlEventArgs() { Person = selectedPerson });
            }

            LinkLabel match = null;
            foreach (LinkLabel ll in this.recentList.Controls)
            {
                Person person = (Person)ll.Tag;
                if (person.PersonID == selectedPerson.PersonID)
                {
                    match = ll;
                    break;
                }
            }

            if (match == null)
            {
                match = CreatePersonLabel(selectedPerson);
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
            Person person = this.SelectedPerson;
            if (person != null)
            {
                fEditPerson p = new fEditPerson();
                p.PersonID = person.PersonID;

                p.ShowDialog();

                if (!p.Cancel)
                {
                    selectedPerson = PicContext.Current.UserManager.GetPersonA(p.PersonID);
                }
            }
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


        private Person selectedPerson = null;

		public Person SelectedPerson
		{
			get 
			{
                return selectedPerson;
			}

		}

		// events
		public event DoubleClickPersonEventHandler DoubleClickPerson;

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            string text = searchText.Text.ToLower();

            if (this.cachedPersonList != null)
            {
                List<Person> list = (from p in this.cachedPersonList
                                     where p.FirstName.ToLower().Contains(text)
                                         || p.LastName.ToLower().Contains(text)
                                         || p.FullName.ToLower().Contains(text)
                                     select p).ToList<Person>();

                AddPersonList(list, this.matchList);

                this.matchCount.Text = "Found " + list.Count.ToString() + " matches";
            }
        }

        private void AddPersonList(List<Person> list, Control parentControl)
        {
            parentControl.Controls.Clear();
            foreach (Person p in list)
            {
                string fullName = p.FullName;
                int id = p.PersonID;

                LinkLabel ll = CreatePersonLabel(p);
                parentControl.Controls.Add(ll);
            }
        }

        private LinkLabel CreatePersonLabel(Person p)
        {
            LinkLabel ll = new LinkLabel();
            ll.Width = this.matchList.ClientSize.Width;
            ll.Height = 14;
            ll.Anchor = AnchorStyles.Left & AnchorStyles.Right;
            ll.Text = p.FullName;
            ll.Tag = p;
            ll.Click += new EventHandler(ll_Click);
            ll.ContextMenu = this.contextMenu1;
            return ll;
        }

        void ll_Click(object sender, EventArgs e)
        {
            LinkLabel ll = (LinkLabel)sender;
            selectedPerson = ll.Tag as Person;
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

        private void matchList_Paint(object sender, PaintEventArgs e)
        {

        }
	}

	public delegate void ClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	public delegate void DoubleClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	
	public class PersonCtlEventArgs: EventArgs 
	{
        public Person Person { get; set; }
	}

}
