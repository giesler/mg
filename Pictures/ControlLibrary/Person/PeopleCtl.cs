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
        private List<Person> cachedPersonList;
        List<Person> recentList;
        private LinkLabel addLink;
        private Label matchCount;
        private ListView list;

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
            this.cachedPersonList = (from p in PicContext.Current.UserManager.GetPeople()
                                     select p).ToList<Person>();
            this.recentList = PicContext.Current.UserManager.GetRecentUsers();
        
            this.Invoke(new MethodInvoker(delegate (){
                this.DisplayList(this.recentList);
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
            this.addLink = new System.Windows.Forms.LinkLabel();
            this.matchCount = new System.Windows.Forms.Label();
            this.list = new System.Windows.Forms.ListView();
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
            this.searchText.Size = new System.Drawing.Size(248, 20);
            this.searchText.TabIndex = 11;
            this.searchText.Text = "<enter name to search>";
            this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            this.searchText.Enter += new System.EventHandler(this.searchText_Enter);
            // 
            // addLink
            // 
            this.addLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addLink.Location = new System.Drawing.Point(217, 27);
            this.addLink.Name = "addLink";
            this.addLink.Size = new System.Drawing.Size(35, 16);
            this.addLink.TabIndex = 18;
            this.addLink.TabStop = true;
            this.addLink.Text = "&Add...";
            this.addLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // matchCount
            // 
            this.matchCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.matchCount.Location = new System.Drawing.Point(3, 27);
            this.matchCount.Name = "matchCount";
            this.matchCount.Size = new System.Drawing.Size(208, 16);
            this.matchCount.TabIndex = 17;
            this.matchCount.Text = "Recently selected...";
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.list.HideSelection = false;
            this.list.Location = new System.Drawing.Point(4, 46);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(248, 108);
            this.list.TabIndex = 19;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.List;
            this.list.DoubleClick += new System.EventHandler(this.list_DoubleClick);
            // 
            // PeopleCtl
            // 
            this.Controls.Add(this.list);
            this.Controls.Add(this.addLink);
            this.Controls.Add(this.matchCount);
            this.Controls.Add(this.searchText);
            this.Name = "PeopleCtl";
            this.Size = new System.Drawing.Size(255, 157);
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
            string text = searchText.Text.ToLower().Trim();

            if (text.Length > 0)
            {
                if (this.cachedPersonList != null)
                {
                    List<Person> list = (from p in this.cachedPersonList
                                         where p.FirstName.ToLower().Contains(text)
                                             || p.LastName.ToLower().Contains(text)
                                             || p.FullName.ToLower().Contains(text)
                                         select p).ToList<Person>();


                    this.DisplayList(list);

                    this.matchCount.Text = "Found " + list.Count.ToString() + " matches";
                }
            }
            else if (this.recentList != null)
            {
                this.DisplayList(this.recentList);
                this.matchCount.Text = "Recently selected...";
            }
            
        }

        private void DisplayList(List<Person> list)
        {
            this.list.Items.Clear();
            foreach (Person p in list.OrderBy(p => p.FullName))
            {
                ListViewItem item = new ListViewItem(p.FullName) { Tag = p };
                this.list.Items.Add(item);
            }
        }

        private void addLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            menuAddPerson_Click(null, null);
        }

        private void searchText_Enter(object sender, EventArgs e)
        {
            if (this.searchText.Text.Equals("<enter name to search>"))
            {
                this.searchText.Text = "";
                this.searchText.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            }

            this.searchText.SelectionStart = 0;
            this.searchText.SelectionLength = this.searchText.Text.Length;
        }

        private void list_DoubleClick(object sender, EventArgs e)
        {
            if (this.list.SelectedItems.Count > 0)
            {
                ListViewItem item = this.list.SelectedItems[0];
                selectedPerson = item.Tag as Person;
                this.FireSelectPerson();
            }
        }
	}

	public delegate void ClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	public delegate void DoubleClickPersonEventHandler(object sender, PersonCtlEventArgs e);
	
	public class PersonCtlEventArgs: EventArgs 
	{
        public Person Person { get; set; }
	}

}
