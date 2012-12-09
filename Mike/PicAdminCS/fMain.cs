using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PicAdminCS
{
    
	/// <summary>
	/// Summary description for fMain.
	/// </summary>
	public class fMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuFileExit;
		private System.Data.SqlClient.SqlDataAdapter daPictureDate;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panelPic;
		private System.Windows.Forms.PictureBox pbPic;
		private System.Windows.Forms.MenuItem menuAddPictures;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.ListView lvPics;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TreeView tvDates;
		private PicAdminCS.CategoryTree categoryTree1;
		private PicAdminCS.PeopleCtl peopleCtl1;
		private System.Windows.Forms.ContextMenu mnuPictureList;
		private System.Windows.Forms.MenuItem mnuPictureListEdit;
		private System.Windows.Forms.MenuItem mnuPictureListDelete;

		protected Image imgCurImage = null;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set the connection string
			cn.ConnectionString = "data source=kyle;initial catalog=picdb;user id=sa;password=too;persist security info=False";

			// Load the trees
			LoadTreeView();
			
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
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.mnuPictureListDelete = new System.Windows.Forms.MenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tvDates = new System.Windows.Forms.TreeView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.categoryTree1 = new PicAdminCS.CategoryTree();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.peopleCtl1 = new PicAdminCS.PeopleCtl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelPic = new System.Windows.Forms.Panel();
			this.pbPic = new System.Windows.Forms.PictureBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuAddPictures = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.daPictureDate = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.mnuPictureListEdit = new System.Windows.Forms.MenuItem();
			this.mnuPictureList = new System.Windows.Forms.ContextMenu();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.lvPics = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panelPic.SuspendLayout();
			this.SuspendLayout();
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Publish";
			this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader5.Width = 50;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Filename";
			this.columnHeader4.Width = 130;
			// 
			// mnuPictureListDelete
			// 
			this.mnuPictureListDelete.Index = 1;
			this.mnuPictureListDelete.Text = "&Delete";
			this.mnuPictureListDelete.Click += new System.EventHandler(this.mnuPictureListDelete_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage2,
																					  this.tabPage3});
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(200, 569);
			this.tabControl1.TabIndex = 9;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.tvDates});
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(192, 543);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Date";
			// 
			// tvDates
			// 
			this.tvDates.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvDates.FullRowSelect = true;
			this.tvDates.HideSelection = false;
			this.tvDates.ImageIndex = -1;
			this.tvDates.Name = "tvDates";
			this.tvDates.SelectedImageIndex = -1;
			this.tvDates.Size = new System.Drawing.Size(192, 543);
			this.tvDates.TabIndex = 0;
			this.tvDates.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDates_AfterSelect);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.categoryTree1});
			this.tabPage2.Location = new System.Drawing.Point(4, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(192, 561);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Category";
			// 
			// categoryTree1
			// 
			this.categoryTree1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.categoryTree1.Name = "categoryTree1";
			this.categoryTree1.Size = new System.Drawing.Size(192, 543);
			this.categoryTree1.TabIndex = 0;
			this.categoryTree1.ClickCategory += new PicAdminCS.ClickCategoryEventHandler(this.categoryTree1_ClickCategory);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.peopleCtl1});
			this.tabPage3.Location = new System.Drawing.Point(4, 4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(192, 561);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Person";
			// 
			// peopleCtl1
			// 
			this.peopleCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.peopleCtl1.Name = "peopleCtl1";
			this.peopleCtl1.Size = new System.Drawing.Size(192, 543);
			this.peopleCtl1.TabIndex = 0;
			this.peopleCtl1.ClickPerson += new PicAdminCS.ClickPersonEventHandler(this.peopleCtl1_ClickPerson);
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.panelPic});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(203, 257);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(493, 312);
			this.panel1.TabIndex = 5;
			// 
			// panelPic
			// 
			this.panelPic.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.panelPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelPic.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.pbPic});
			this.panelPic.Location = new System.Drawing.Point(104, 8);
			this.panelPic.Name = "panelPic";
			this.panelPic.Size = new System.Drawing.Size(304, 296);
			this.panelPic.TabIndex = 1;
			// 
			// pbPic
			// 
			this.pbPic.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.pbPic.BackColor = System.Drawing.SystemColors.Window;
			this.pbPic.Name = "pbPic";
			this.pbPic.Size = new System.Drawing.Size(152, 294);
			this.pbPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbPic.TabIndex = 0;
			this.pbPic.TabStop = false;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuAddPictures,
																					  this.menuItem3,
																					  this.menuFileExit});
			this.menuItem1.Text = "File";
			// 
			// menuAddPictures
			// 
			this.menuAddPictures.Index = 0;
			this.menuAddPictures.Text = "&Add Pictures";
			this.menuAddPictures.Click += new System.EventHandler(this.menuAddPictures_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// menuFileExit
			// 
			this.menuFileExit.Index = 2;
			this.menuFileExit.Text = "E&xit";
			this.menuFileExit.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(200, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 569);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter2.Location = new System.Drawing.Point(203, 254);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(493, 3);
			this.splitter2.TabIndex = 6;
			this.splitter2.TabStop = false;
			// 
			// daPictureDate
			// 
			this.daPictureDate.SelectCommand = this.sqlSelectCommand1;
			this.daPictureDate.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																									new System.Data.Common.DataTableMapping("Table", "Table", new System.Data.Common.DataColumnMapping[] {
																																																			 new System.Data.Common.DataColumnMapping("PicYear", "PicYear"),
																																																			 new System.Data.Common.DataColumnMapping("PicMonth", "PicMonth"),
																																																			 new System.Data.Common.DataColumnMapping("PicDay", "PicDay")})});
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = @"SELECT DATEPART(yyyy, PictureDate) AS PicYear, DATEPART(mm, PictureDate) AS PicMonth, DATEPART(dd, PictureDate) AS PicDay FROM Picture GROUP BY DATEPART(yyyy, PictureDate), DATEPART(mm, PictureDate), DATEPART(dd, PictureDate) ORDER BY DATEPART(yyyy, PictureDate), DATEPART(mm, PictureDate), DATEPART(dd, PictureDate)";
			this.sqlSelectCommand1.Connection = this.cn;
			// 
			// mnuPictureListEdit
			// 
			this.mnuPictureListEdit.Index = 0;
			this.mnuPictureListEdit.Text = "&Edit";
			this.mnuPictureListEdit.Click += new System.EventHandler(this.mnuPictureListEdit_Click);
			// 
			// mnuPictureList
			// 
			this.mnuPictureList.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuPictureListEdit,
																						   this.mnuPictureListDelete});
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Title";
			this.columnHeader3.Width = 150;
			// 
			// lvPics
			// 
			this.lvPics.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.columnHeader1,
																					 this.columnHeader2,
																					 this.columnHeader3,
																					 this.columnHeader4,
																					 this.columnHeader5});
			this.lvPics.ContextMenu = this.mnuPictureList;
			this.lvPics.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvPics.FullRowSelect = true;
			this.lvPics.HideSelection = false;
			this.lvPics.Location = new System.Drawing.Point(203, 0);
			this.lvPics.Name = "lvPics";
			this.lvPics.Size = new System.Drawing.Size(493, 254);
			this.lvPics.TabIndex = 8;
			this.lvPics.View = System.Windows.Forms.View.Details;
			this.lvPics.DoubleClick += new System.EventHandler(this.lvPics_DoubleClick);
			this.lvPics.SelectedIndexChanged += new System.EventHandler(this.lvPics_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Pic ID";
			this.columnHeader1.Width = 0;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Date";
			this.columnHeader2.Width = 80;
			// 
			// fMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(696, 569);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lvPics,
																		  this.splitter2,
																		  this.panel1,
																		  this.splitter1,
																		  this.tabControl1});
			this.Menu = this.mainMenu1;
			this.Name = "fMain";
			this.Text = "Pic Admin";
			this.Load += new System.EventHandler(this.fMain_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panelPic.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new fMain());
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void LoadTreeView() 
		{

			DataSet ds = new DataSet();
			TreeNode nYear, nMonth, nDay;

			daPictureDate.Fill(ds, "PictureDates");
			
			foreach (DataRow dr in ds.Tables["PictureDates"].Rows) 
			{
				nYear  = GetNode(tvDates.Nodes, dr[0].ToString());
				nYear.Tag = "DatePart(yyyy, PictureDate) = " + nYear.Text;
				nYear.Expand();
				nMonth = GetNode(nYear.Nodes, MonthString(Convert.ToInt32(dr[1])));
				nMonth.Tag = "DatePart(yyyy, PictureDate) = " + nYear.Text + " AND "
					+ "DatePart(mm, PictureDate) = " + dr[1].ToString();

				nDay   = GetNode(nMonth.Nodes, dr[2].ToString());
				nDay.Tag = "DatePart(yyyy, PictureDate) = " + nYear.Text + " AND "
					+ "DatePart(mm, PictureDate) = " + dr[1].ToString() + " AND "
					+ "DatePart(dd, PictureDate) = " + nDay.Text;
			}

		}

		private String MonthString(int Month) 
		{
			switch (Month) 
			{
				case 1: { return "January"; }
				case 2: { return "February"; }
				case 3: { return "March"; }
				case 4: { return "April"; }
				case 5: { return "May"; }
				case 6: { return "June"; }
				case 7: { return "July"; }
				case 8: { return "August"; }
				case 9: { return "September"; }
				case 10: { return "October"; }
				case 11: { return "November"; }
				case 12: { return "December"; }
			}
			return "Invalid Month";
		}


		private TreeNode GetNode(TreeNodeCollection cNodes, String sNode) 
		{
			foreach(TreeNode n in cNodes)
			{
				if (n.Text == sNode)
					return n;
			}
			
			// not found, so add
			return cNodes.Add(sNode);

	}

		private void fMain_Load(object sender, System.EventArgs e)
		{

			pbPic.Width = pbPic.Parent.Width;
			pbPic.Height = pbPic.Parent.Height;

		}

		private void tvDates_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{

			TreeNode nCur = tvDates.SelectedNode;
			if (nCur == null) return;

			ListViewItem li;
			lvPics.Items.Clear();

			cn.Open();
			SqlCommand cmd = new SqlCommand("select PictureID, PictureDate, Title, Filename, Publish from Picture where " + nCur.Tag, cn);
			SqlDataReader dr = cmd.ExecuteReader();
			String strPublish;

			while (dr.Read()) 
			{
				if (dr.GetBoolean(4)) strPublish = "x"; else strPublish = "";
				
				li = lvPics.Items.Add(new ListViewItem( 
					new String[] { dr.GetInt32(0).ToString(),
									dr.GetDateTime(1).ToShortDateString(),
									dr.GetString(2),
									dr.GetString(3),
									strPublish
							 } ));
				
			}

			dr.Close();
			cn.Close();

		}

		private void lvPics_SelectedIndexChanged(object sender, System.EventArgs e)
		{

			try 
			{

				ListViewItem li;
				if (lvPics.SelectedItems.Count != 1) return;

				li = lvPics.SelectedItems[0];

				String strFile = "\\\\kenny\\inetpub\\pictures\\" + li.SubItems[3].Text;
				strFile = strFile.Replace("/", "\\");

				imgCurImage = Image.FromFile(strFile);
			
				panelPic.Width = (int) ( ( (float) imgCurImage.Width / (float) imgCurImage.Height) 
																	* (float) panelPic.Height );
				pbPic.Width = panelPic.Width;
				pbPic.Height = panelPic.Height;

				panelPic.Left = (lvPics.Width/2) - (pbPic.Width/2);

				pbPic.Image = imgCurImage;

				// release image
				// img.Dispose();
				
			}
			catch (FileNotFoundException fnfe) 
			{
				MessageBox.Show("The file '" + fnfe.Message + "' was not found when attempting to load the picture preview.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}

		}

		private void splitter2_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			lvPics_SelectedIndexChanged(sender, e);
		}

		private void lvPics_DoubleClick(object sender, System.EventArgs e)
		{
			ListViewItem li;
			
			if (lvPics.SelectedItems.Count == 0) return;

			li = lvPics.SelectedItems[0];

			fPicture f = new fPicture();

			f.LoadPicture(Convert.ToInt32(li.Text));
			f.LoadImage("\\\\kenny\\inetpub\\pictures\\" + 
				li.SubItems[3].Text.Replace("/", "\\") );
			
			f.ShowDialog();

			if (!f.mblnCancel) 
			{
				li.SubItems[2].Text = f.Title;
			}

		}

		private void menuAddPictures_Click(object sender, System.EventArgs e)
		{
			fAddPictures f = new fAddPictures();
			f.ShowDialog();
			LoadTreeView();

		}

		private void categoryTree1_ClickCategory(object sender, PicAdminCS.CategoryTreeEventArgs e)
		{

			ListViewItem li;
			lvPics.Items.Clear();

			cn.Open();
			SqlCommand cmd = new SqlCommand("select PictureID, PictureDate, Title, Filename, "
				+ "Publish from Picture where PictureID in "
				+ "(select pc.PictureID from PictureCategory pc inner join CategorySubCategory csc ON csc.SubCategoryID = pc.CategoryID "
				+ "where csc.CategoryID = " + e.categoryRow.CategoryID.ToString() + ")", cn);
			SqlDataReader dr = cmd.ExecuteReader();
			String strPublish;

			while (dr.Read()) 
			{
				if (dr.GetBoolean(4)) strPublish = "x"; else strPublish = "";
				
				li = lvPics.Items.Add(new ListViewItem( 
					new String[] { dr.GetInt32(0).ToString(),
									 dr.GetDateTime(1).ToShortDateString(),
									 dr.GetString(2),
									 dr.GetString(3),
									 strPublish
								 } ));
				
			}

			dr.Close();
			cn.Close();
			
		}

		private void peopleCtl1_ClickPerson(object sender, PicAdminCS.PersonCtlEventArgs e)
		{

			ListViewItem li;
			lvPics.Items.Clear();

			cn.Open();
			SqlCommand cmd = new SqlCommand("select PictureID, PictureDate, Title, Filename, "
				+ "Publish from Picture where PictureID in "
				+ "(select PictureID from PicturePerson where PersonID = " + e.personRow.PersonID.ToString() + ")", cn);
			SqlDataReader dr = cmd.ExecuteReader();
			String strPublish;

			while (dr.Read()) 
			{
				if (dr.GetBoolean(4)) strPublish = "x"; else strPublish = "";
				
				li = lvPics.Items.Add(new ListViewItem( 
					new String[] { dr.GetInt32(0).ToString(),
									 dr.GetDateTime(1).ToShortDateString(),
									 dr.GetString(2),
									 dr.GetString(3),
									 strPublish
								 } ));
				
			}

			dr.Close();
			cn.Close();
			

		}

		private void mnuPictureListEdit_Click(object sender, System.EventArgs e)
		{
			lvPics_DoubleClick(sender, e);
		}

		private void mnuPictureListDelete_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to permenantly remove the selected picture(s)?  The picture file will also be deleted!", "Confirm Delete", 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
			{
				imgCurImage.Dispose();
				imgCurImage = null;
				
				pbPic.Image = null;
				pbPic.Refresh();

				cn.Open();

				foreach (ListViewItem li in lvPics.SelectedItems) 
				{
					String strPath = "\\\\kenny\\inetpub\\pictures\\" + li.SubItems[3].Text;
					try 
					{
						// attempt to delete the file
						if (File.Exists(strPath))
							File.Delete(strPath);

						// we want to delete from db, but only if file delete worked
						SqlCommand cmd = new SqlCommand("delete from Picture where PictureID = " + li.Text, cn);
						cmd.ExecuteNonQuery();

						// remove the deleted item
						lvPics.Items.Remove(li);
					}
					catch (IOException ioe) 
					{
                        MessageBox.Show(ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					}
					

				}
				
				cn.Close();
			}
				
		}

	}
}
