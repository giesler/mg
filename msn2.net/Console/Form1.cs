using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace msn2.net.ProjectF
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.ListView listView1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Login l = new Login();
			if (l.ShowDialog(this) == DialogResult.Cancel)
			{
				this.DialogResult = DialogResult.Cancel;
				this.Close();
				Application.Exit();
				return;
			}

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.MSNBCHeadlines()));

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.MSNBCWeather()));

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.Favorites()));

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.Notes()));

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.WebSearch()));

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.ShellLaunch()));

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.RandomPicture()));

			this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - 50;
			this.Top  = 3;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.CheckBoxes = true;
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(144, 120);
			this.listView1.TabIndex = 1;
			this.listView1.View = System.Windows.Forms.View.List;
			this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(144, 120);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listView1});
			this.Name = "Form1";
			this.RolledUp = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "s";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Form1 f = new Form1();
			if (f.DialogResult != DialogResult.Cancel)
				Application.Run(f);
		}

		private void listView1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			FormListViewItem item = (FormListViewItem) listView1.Items[e.Index];
			item.Visible = !item.Checked;
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (FormListViewItem item in listView1.Items)
			{
                item.AllowUnload = true;		
                item.Close();				
			}
		}

		private class FormListViewItem: ListViewItem
		{
			private msn2.net.Controls.ShellForm page;

			public FormListViewItem(Form parent, msn2.net.Controls.ShellForm page): base(page.Text)
			{
				parent.AddOwnedForm(page);

				this.page = page;
				page.VisibleChanged += new EventHandler(Page_VisibleChanged);
				page.Show();

                page.AllowUnload = false;				

			}

			private void Page_VisibleChanged(object sender, EventArgs e)
			{
				this.Checked = page.Visible;
			}

			public bool Visible
			{
				get { return page.Visible; }
				set { page.Visible = value; }
			}

			public bool AllowUnload
			{
				get { return page.AllowUnload; }
				set { page.AllowUnload = value; }
			}

			public void Close()
			{
				page.Close();
			}
		}
	}
}
