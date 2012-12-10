using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace XMAdmin
{
	/// <summary>
	/// Summary description for formSponsors.
	/// </summary>
	public class formSponsors : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonRemove;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Label labelPictures;
		private System.Windows.Forms.Label labelSponsor;
		private System.Windows.Forms.ComboBox comboSponsor;
		private System.Windows.Forms.Button buttonSet;
		private System.Windows.Forms.ListView listviewPictures;
		private System.Windows.Forms.ColumnHeader headerFilename;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		//combo box item
		public class SponsorItem
		{
			public string name;
			public int id;
			
			public override string ToString()
			{
				return name;
			}
		}
		
		//listview item
		public class PictureItem : ListViewItem
		{
			public PictureItem(string vfilename)
				: base(vfilename)
			{
				filename = vfilename;
				
				//read file
				FileStream fs = File.OpenRead(filename);
				byte[] buf = new byte[fs.Length];
				fs.Read(buf, 0, (int)fs.Length);
				fs.Close();

				//calculate md5
				XMMd5Engine e = new XMMd5Engine();
				e.Update(buf, (uint)buf.Length);
				e.Finish();
				md5 = e.Md5;
			}
			public PictureItem(string vfilename, XMGuid vmd5)
				: base(vfilename)
			{
				filename = vfilename;
				md5 = vmd5;
			}
			public string filename;
			public XMGuid md5;
		}

		public ArrayList InitialPictures = new ArrayList();

		public formSponsors()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.buttonRemove = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.listviewPictures = new System.Windows.Forms.ListView();
			this.headerFilename = new System.Windows.Forms.ColumnHeader();
			this.labelPictures = new System.Windows.Forms.Label();
			this.buttonSet = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.labelSponsor = new System.Windows.Forms.Label();
			this.comboSponsor = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// buttonRemove
			// 
			this.buttonRemove.Location = new System.Drawing.Point(320, 448);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(72, 32);
			this.buttonRemove.TabIndex = 1;
			this.buttonRemove.Text = "Remove";
			this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.AddExtension = false;
			this.openFileDialog1.Filter = "JPEG Files(*.jpeg,*.jpg)|*.jpeg;*.jpg|All Files(*.*)|*.*";
			this.openFileDialog1.Multiselect = true;
			this.openFileDialog1.Title = "Select Pictures";
			// 
			// listviewPictures
			// 
			this.listviewPictures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.headerFilename});
			this.listviewPictures.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listviewPictures.Location = new System.Drawing.Point(8, 96);
			this.listviewPictures.Name = "listviewPictures";
			this.listviewPictures.Size = new System.Drawing.Size(384, 344);
			this.listviewPictures.TabIndex = 0;
			this.listviewPictures.View = System.Windows.Forms.View.Details;
			// 
			// headerFilename
			// 
			this.headerFilename.Text = "File";
			this.headerFilename.Width = 350;
			// 
			// labelPictures
			// 
			this.labelPictures.Location = new System.Drawing.Point(8, 80);
			this.labelPictures.Name = "labelPictures";
			this.labelPictures.Size = new System.Drawing.Size(192, 16);
			this.labelPictures.TabIndex = 2;
			this.labelPictures.Text = "Pictures";
			// 
			// buttonSet
			// 
			this.buttonSet.Location = new System.Drawing.Point(272, 24);
			this.buttonSet.Name = "buttonSet";
			this.buttonSet.Size = new System.Drawing.Size(80, 32);
			this.buttonSet.TabIndex = 5;
			this.buttonSet.Text = "Set Sponsor";
			this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(240, 448);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(72, 32);
			this.buttonAdd.TabIndex = 1;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// labelSponsor
			// 
			this.labelSponsor.Location = new System.Drawing.Point(8, 16);
			this.labelSponsor.Name = "labelSponsor";
			this.labelSponsor.Size = new System.Drawing.Size(152, 16);
			this.labelSponsor.TabIndex = 3;
			this.labelSponsor.Text = "Sponsor";
			// 
			// comboSponsor
			// 
			this.comboSponsor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSponsor.DropDownWidth = 208;
			this.comboSponsor.Location = new System.Drawing.Point(8, 32);
			this.comboSponsor.Name = "comboSponsor";
			this.comboSponsor.Size = new System.Drawing.Size(208, 21);
			this.comboSponsor.TabIndex = 4;
			// 
			// formSponsors
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(404, 489);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonSet,
																		  this.comboSponsor,
																		  this.labelSponsor,
																		  this.labelPictures,
																		  this.buttonAdd,
																		  this.buttonRemove,
																		  this.listviewPictures});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "formSponsors";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Picture Sponsors";
			this.Load += new System.EventHandler(this.formSponsors_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonRemove_Click(object sender, System.EventArgs e)
		{
			//remove items from the listview
			PictureItem[] pi = new PictureItem[listviewPictures.SelectedItems.Count];
			listviewPictures.SelectedItems.CopyTo(pi, 0);
			foreach(PictureItem p in pi)
			{
				listviewPictures.Items.Remove(p);
			}
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			//get a list of filenames
			//openFileDialog1.Reset();
			if (openFileDialog1.ShowDialog()==DialogResult.OK)
			{
				foreach(string fn in openFileDialog1.FileNames)
				{
					listviewPictures.Items.Add(new PictureItem(fn));
				}
			}
		}

		private void formSponsors_Load(object sender, System.EventArgs e)
		{
			//add items to the combo
			SqlDataReader rs = Data.Exec("select * from adcompany");
			while (rs.Read())
			{
				SponsorItem si = new SponsorItem();
				si.name = (string)rs["CompanyName"];
				si.id = (int)rs["CompanyID"];			
				comboSponsor.Items.Add(si);
			}
			rs.Close();
			
			//load any pictures
			foreach(Picture p in InitialPictures)
			{
				PictureItem pi = new PictureItem(p.Path, p.Md5);
				listviewPictures.Items.Add(pi);
			}
		}

		private void buttonSet_Click(object sender, System.EventArgs e)
		{
			//confirm
			if (DialogResult.Yes !=
				MessageBox.Show(this, "Sure?", "Confirm",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question))
			{
				return;
			}
			
			//check the sponsor id
			int sid = ((SponsorItem)comboSponsor.SelectedItem).id;
			
			//run update for each md5
			ArrayList errors = new ArrayList();
			foreach(PictureItem pic in listviewPictures.Items)
			{
				try {
					Data.ExecNoResults(String.Format(
						"update media set sponsor={0} where md5={1}",
						sid,
						pic.md5.ToStringDB()));
				}
				catch(Exception x)
				{
					errors.Add(pic);
					System.Diagnostics.Debug.WriteLine(x);
				}
			}
			
			//show results
			MessageBox.Show(this, String.Format(
				"Finished.\nErrors: {0}.\n", errors.Count));
			if (errors.Count > 0)
			{
				//print out failed files
				string s = "Errors:\n";
				foreach(PictureItem pic in errors)
				{
					s += pic.filename + "\n";
				}
				MessageBox.Show(this, s);
			}
				
		}
	}
}
