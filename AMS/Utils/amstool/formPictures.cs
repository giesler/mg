using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace XMAdmin
{
	/// <summary>
	/// Summary description for formPictures.
	/// </summary>
	public class formPictures : CustomMdiChild
	{
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PictureBox picturePreview;
		private System.Windows.Forms.ListView listPictures;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonRemove;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.ColumnHeader columnFile;
		private System.Windows.Forms.ColumnHeader columnPath;
		private System.Windows.Forms.Panel panelControls;
		private System.Windows.Forms.Panel panelPictures;
		private System.Windows.Forms.ContextMenu menuPictures;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.Button buttonSponsor;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public formPictures()
			: base()
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
			this.listPictures = new System.Windows.Forms.ListView();
			this.columnFile = new System.Windows.Forms.ColumnHeader();
			this.columnPath = new System.Windows.Forms.ColumnHeader();
			this.menuPictures = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.buttonRemove = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonClear = new System.Windows.Forms.Button();
			this.picturePreview = new System.Windows.Forms.PictureBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panelControls = new System.Windows.Forms.Panel();
			this.buttonSponsor = new System.Windows.Forms.Button();
			this.panelPictures = new System.Windows.Forms.Panel();
			this.panelControls.SuspendLayout();
			this.panelPictures.SuspendLayout();
			this.SuspendLayout();
			// 
			// listPictures
			// 
			this.listPictures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.columnFile,
																						   this.columnPath});
			this.listPictures.ContextMenu = this.menuPictures;
			this.listPictures.Dock = System.Windows.Forms.DockStyle.Left;
			this.listPictures.FullRowSelect = true;
			this.listPictures.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listPictures.Name = "listPictures";
			this.listPictures.Size = new System.Drawing.Size(264, 771);
			this.listPictures.TabIndex = 0;
			this.listPictures.View = System.Windows.Forms.View.Details;
			this.listPictures.SelectedIndexChanged += new System.EventHandler(this.listPictures_SelectedIndexChanged);
			// 
			// columnFile
			// 
			this.columnFile.Text = "File";
			this.columnFile.Width = 112;
			// 
			// columnPath
			// 
			this.columnPath.Text = "Path";
			this.columnPath.Width = 123;
			// 
			// menuPictures
			// 
			this.menuPictures.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Set Sponsor...";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// buttonRemove
			// 
			this.buttonRemove.Location = new System.Drawing.Point(96, 8);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(72, 24);
			this.buttonRemove.TabIndex = 0;
			this.buttonRemove.Text = "Remove";
			this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(16, 8);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(72, 24);
			this.buttonAdd.TabIndex = 0;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(176, 8);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(72, 24);
			this.buttonClear.TabIndex = 1;
			this.buttonClear.Text = "Clear";
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// picturePreview
			// 
			this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picturePreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picturePreview.Location = new System.Drawing.Point(268, 0);
			this.picturePreview.Name = "picturePreview";
			this.picturePreview.Size = new System.Drawing.Size(884, 771);
			this.picturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picturePreview.TabIndex = 2;
			this.picturePreview.TabStop = false;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(264, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(4, 771);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// panelControls
			// 
			this.panelControls.Controls.AddRange(new System.Windows.Forms.Control[] {
																						this.buttonSponsor,
																						this.buttonClear,
																						this.buttonRemove,
																						this.buttonAdd});
			this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelControls.Location = new System.Drawing.Point(2, 2);
			this.panelControls.Name = "panelControls";
			this.panelControls.Size = new System.Drawing.Size(1152, 46);
			this.panelControls.TabIndex = 0;
			// 
			// buttonSponsor
			// 
			this.buttonSponsor.Location = new System.Drawing.Point(256, 8);
			this.buttonSponsor.Name = "buttonSponsor";
			this.buttonSponsor.Size = new System.Drawing.Size(88, 24);
			this.buttonSponsor.TabIndex = 2;
			this.buttonSponsor.Text = "Sponsors";
			this.buttonSponsor.Click += new System.EventHandler(this.buttonSponsor_Click);
			// 
			// panelPictures
			// 
			this.panelPictures.Controls.AddRange(new System.Windows.Forms.Control[] {
																						this.picturePreview,
																						this.splitter1,
																						this.listPictures});
			this.panelPictures.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelPictures.Location = new System.Drawing.Point(2, 48);
			this.panelPictures.Name = "panelPictures";
			this.panelPictures.Size = new System.Drawing.Size(1152, 771);
			this.panelPictures.TabIndex = 1;
			// 
			// formPictures
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1156, 821);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelPictures,
																		  this.panelControls});
			this.DockPadding.All = 2;
			this.Name = "formPictures";
			this.Text = "Pictures";
			this.Load += new System.EventHandler(this.formPictures_Load);
			this.panelControls.ResumeLayout(false);
			this.panelPictures.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// redraw pictures in listview
		/// </summary>
		private void DrawPictures()
		{
			//clear listview
			listPictures.Items.Clear();

			//insert each picture
			ListViewItem item;
			Picture pic;
			foreach(DictionaryEntry entry in Pictures.Files)
			{
				pic = (Picture)entry.Value;
				item = new ListViewItem(new string[] {
					pic.Name,
					pic.Path });
				item.Tag = pic;
				listPictures.Items.Add(item);
			}
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			//prompt for a direcotry
			formChooseDirectory dlg = new formChooseDirectory();
			dlg.Text = "Choose Folder";
			if (dlg.ShowDialog(this) != DialogResult.OK)
				return;

			//add all jpeg files
			int curFiles = Pictures.Files.Count;
			try
			{
				Pictures.SearchPath(dlg.Path);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Error searching path:\n"+ex.Message, "Search Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//update ui
			DrawPictures();
			MessageBox.Show(this, String.Format("Added {0} files.", Pictures.Files.Count-curFiles),
				"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		private void buttonRemove_Click(object sender, System.EventArgs e)
		{
			//delete all selected pictures
			foreach(ListViewItem item in listPictures.SelectedItems)
			{
				Pictures.Files.Remove(((Picture)item.Tag).Md5);
				item.Remove();
			}
		}

		private void buttonClear_Click(object sender, System.EventArgs e)
		{
			//remove all items
			Pictures.Files.Clear();
			listPictures.Items.Clear();
		}

		private void listPictures_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//ignore if no selection
			if (listPictures.SelectedItems.Count < 1)
			{
				picturePreview.Image = null;
				return;
			}

			//display the picture on the right
			Image i = ((Picture)listPictures.SelectedItems[0].Tag).Image;
			Size s = picturePreview.Size;
			picturePreview.Image = i;
			picturePreview.Size = s;
			
		}

		private void formPictures_Load(object sender, System.EventArgs e)
		{
			DrawPictures();
		}

		private void buttonSponsor_Click(object sender, System.EventArgs e)
		{
			//show a sponsor dialog
			formSponsors dlg = new formSponsors();
			dlg.ShowDialog(this);
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			if (listPictures.SelectedItems.Count < 1)
			{
				MessageBox.Show(this, "Select some pictures first.");
				return;
			}
		
			//show the sponsor dialog, with the pictures
			//already selected
			formSponsors dlg = new formSponsors();
			foreach(ListViewItem lvi in listPictures.SelectedItems)
			{
				dlg.InitialPictures.Add(lvi.Tag);
			}
			dlg.ShowDialog(this);
		}
	}
}
