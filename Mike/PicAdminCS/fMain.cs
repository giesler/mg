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
		public class PicListViewSorter : IComparer 
		{
			public int Compare(Object obj1, Object obj2) 
			{
				if (!(obj1 is ListViewItem) || !(obj2 is ListViewItem))
					throw new ArgumentException("Objects passed to PicListViewSorter must be ListViewItems");
				
				int intVal1 = Convert.ToInt32(((ListViewItem)obj1).SubItems[5].Text);
				int intVal2 = Convert.ToInt32(((ListViewItem)obj2).SubItems[5].Text);
				return (intVal1.CompareTo(intVal2));
			}
		}
            
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
		private System.Windows.Forms.ContextMenu mnuPictureList;
		private System.Windows.Forms.MenuItem mnuPictureListEdit;
		private System.Windows.Forms.MenuItem mnuPictureListDelete;

		protected Image imgCurImage = null;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuPictureListMoveUp;
		private System.Windows.Forms.MenuItem mnuPictureListMoveDown;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TreeView tvAddedDate;
		private PicAdminCS.CategoryTree categoryTree1;
		private PicAdminCS.PeopleCtl peopleCtl1;
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

			lvPics.ListViewItemSorter = new PicListViewSorter();

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
			this.pbPic = new System.Windows.Forms.PictureBox();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.mnuPictureListMoveDown = new System.Windows.Forms.MenuItem();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.panelPic = new System.Windows.Forms.Panel();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tvDates = new System.Windows.Forms.TreeView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.categoryTree1 = new PicAdminCS.CategoryTree();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tvAddedDate = new System.Windows.Forms.TreeView();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.daPictureDate = new System.Data.SqlClient.SqlDataAdapter();
			this.mnuPictureList = new System.Windows.Forms.ContextMenu();
			this.mnuPictureListEdit = new System.Windows.Forms.MenuItem();
			this.mnuPictureListDelete = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuPictureListMoveUp = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.lvPics = new System.Windows.Forms.ListView();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuAddPictures = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.peopleCtl1 = new PicAdminCS.PeopleCtl();
			this.panelPic.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
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
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = @"SELECT DATEPART(yyyy, PictureDate) AS PicYear, DATEPART(mm, PictureDate) AS PicMonth, DATEPART(dd, PictureDate) AS PicDay FROM Picture GROUP BY DATEPART(yyyy, PictureDate), DATEPART(mm, PictureDate), DATEPART(dd, PictureDate) ORDER BY DATEPART(yyyy, PictureDate), DATEPART(mm, PictureDate), DATEPART(dd, PictureDate)";
			this.sqlSelectCommand1.Connection = this.cn;
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// mnuPictureListMoveDown
			// 
			this.mnuPictureListMoveDown.Index = 4;
			this.mnuPictureListMoveDown.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
			this.mnuPictureListMoveDown.Text = "Move D&own";
			this.mnuPictureListMoveDown.Click += new System.EventHandler(this.mnuPictureListMoveDown_Click);
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter2.Location = new System.Drawing.Point(203, 260);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(493, 3);
			this.splitter2.TabIndex = 6;
			this.splitter2.TabStop = false;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Date";
			this.columnHeader2.Width = 80;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Title";
			this.columnHeader3.Width = 150;
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
			// columnHeader1
			// 
			this.columnHeader1.Text = "Pic ID";
			this.columnHeader1.Width = 0;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Sort";
			this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader6.Width = 0;
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
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage2,
																					  this.tabPage4,
																					  this.tabPage3});
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(200, 575);
			this.tabControl1.TabIndex = 9;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.tvDates});
			this.tabPage1.Location = new System.Drawing.Point(4, 40);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(192, 531);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Picture Date";
			// 
			// tvDates
			// 
			this.tvDates.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvDates.FullRowSelect = true;
			this.tvDates.HideSelection = false;
			this.tvDates.ImageIndex = -1;
			this.tvDates.Name = "tvDates";
			this.tvDates.SelectedImageIndex = -1;
			this.tvDates.Size = new System.Drawing.Size(192, 531);
			this.tvDates.TabIndex = 0;
			this.tvDates.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDates_AfterSelect);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.categoryTree1});
			this.tabPage2.Location = new System.Drawing.Point(4, 40);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(192, 531);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Category";
			// 
			// categoryTree1
			// 
			this.categoryTree1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.categoryTree1.Name = "categoryTree1";
			this.categoryTree1.Size = new System.Drawing.Size(192, 531);
			this.categoryTree1.TabIndex = 0;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.tvAddedDate});
			this.tabPage4.Location = new System.Drawing.Point(4, 40);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(192, 531);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Added Date";
			// 
			// tvAddedDate
			// 
			this.tvAddedDate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvAddedDate.FullRowSelect = true;
			this.tvAddedDate.HideSelection = false;
			this.tvAddedDate.ImageIndex = -1;
			this.tvAddedDate.Name = "tvAddedDate";
			this.tvAddedDate.SelectedImageIndex = -1;
			this.tvAddedDate.Size = new System.Drawing.Size(192, 531);
			this.tvAddedDate.TabIndex = 1;
			this.tvAddedDate.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAddedDate_AfterSelect);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.peopleCtl1});
			this.tabPage3.Location = new System.Drawing.Point(4, 40);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(192, 531);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Person";
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
			// mnuPictureList
			// 
			this.mnuPictureList.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuPictureListEdit,
																						   this.mnuPictureListDelete,
																						   this.menuItem2,
																						   this.mnuPictureListMoveUp,
																						   this.mnuPictureListMoveDown});
			this.mnuPictureList.Popup += new System.EventHandler(this.mnuPictureList_Popup);
			// 
			// mnuPictureListEdit
			// 
			this.mnuPictureListEdit.Index = 0;
			this.mnuPictureListEdit.Text = "&Edit";
			this.mnuPictureListEdit.Click += new System.EventHandler(this.mnuPictureListEdit_Click);
			// 
			// mnuPictureListDelete
			// 
			this.mnuPictureListDelete.Index = 1;
			this.mnuPictureListDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.mnuPictureListDelete.Text = "&Delete";
			this.mnuPictureListDelete.Click += new System.EventHandler(this.mnuPictureListDelete_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.Text = "-";
			// 
			// mnuPictureListMoveUp
			// 
			this.mnuPictureListMoveUp.Index = 3;
			this.mnuPictureListMoveUp.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
			this.mnuPictureListMoveUp.Text = "Move &Up";
			this.mnuPictureListMoveUp.Click += new System.EventHandler(this.mnuPictureListMoveUp_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// lvPics
			// 
			this.lvPics.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.columnHeader1,
																					 this.columnHeader2,
																					 this.columnHeader3,
																					 this.columnHeader4,
																					 this.columnHeader5,
																					 this.columnHeader6});
			this.lvPics.ContextMenu = this.mnuPictureList;
			this.lvPics.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvPics.FullRowSelect = true;
			this.lvPics.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvPics.HideSelection = false;
			this.lvPics.Location = new System.Drawing.Point(203, 0);
			this.lvPics.Name = "lvPics";
			this.lvPics.Size = new System.Drawing.Size(493, 260);
			this.lvPics.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvPics.TabIndex = 8;
			this.lvPics.View = System.Windows.Forms.View.Details;
			this.lvPics.DoubleClick += new System.EventHandler(this.lvPics_DoubleClick);
			this.lvPics.SelectedIndexChanged += new System.EventHandler(this.lvPics_SelectedIndexChanged);
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
			// menuFileExit
			// 
			this.menuFileExit.Index = 2;
			this.menuFileExit.Text = "E&xit";
			this.menuFileExit.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(200, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 575);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.panelPic});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(203, 263);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(493, 312);
			this.panel1.TabIndex = 5;
			// 
			// peopleCtl1
			// 
			this.peopleCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.peopleCtl1.Name = "peopleCtl1";
			this.peopleCtl1.Size = new System.Drawing.Size(192, 531);
			this.peopleCtl1.TabIndex = 0;
			// 
			// fMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(696, 575);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lvPics,
																		  this.splitter2,
																		  this.panel1,
																		  this.splitter1,
																		  this.tabControl1});
			this.Menu = this.mainMenu1;
			this.Name = "fMain";
			this.Text = "Pic Admin";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.fMain_Closing);
			this.Load += new System.EventHandler(this.fMain_Load);
			this.panelPic.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
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

				nDay   = GetNode(nMonth.Nodes, nMonth.Text + " " + dr[2].ToString());
				nDay.Tag = "DatePart(yyyy, PictureDate) = " + nYear.Text + " AND "
					+ "DatePart(mm, PictureDate) = " + dr[1].ToString() + " AND "
					+ "DatePart(dd, PictureDate) = " + dr[2].ToString();
			}

			
			// Get the added date tree vals
			string sql = "SELECT DATEPART(yyyy, PictureAddDate) AS PicYear, DATEPART(mm, PictureAddDate) AS PicMonth, " 
				+ "DATEPART(dd, PictureAddDate) AS PicDay FROM Picture " 
				+ "GROUP BY DATEPART(yyyy, PictureAddDate), DATEPART(mm, PictureAddDate), DATEPART(dd, PictureAddDate) "
				+ "ORDER BY DATEPART(yyyy, PictureAddDate), DATEPART(mm, PictureAddDate), DATEPART(dd, PictureAddDate)";
			SqlDataAdapter daAdded = new SqlDataAdapter(sql, cn);

			DataSet dsAdded = new DataSet();
			daAdded.Fill(dsAdded, "PictureAddedDates");

			foreach (DataRow dr in dsAdded.Tables["PictureAddedDates"].Rows) 
			{
				nYear  = GetNode(tvAddedDate.Nodes, dr[0].ToString());
				nYear.Tag = "DatePart(yyyy, PictureAddDate) = " + nYear.Text;
				nYear.Expand();
				nMonth = GetNode(nYear.Nodes, MonthString(Convert.ToInt32(dr[1])));
				nMonth.Tag = "DatePart(yyyy, PictureAddDate) = " + nYear.Text + " AND "
					+ "DatePart(mm, PictureAddDate) = " + dr[1].ToString();

				nDay   = GetNode(nMonth.Nodes, nMonth.Text + " " + dr[2].ToString());
				nDay.Tag = "DatePart(yyyy, PictureAddDate) = " + nYear.Text + " AND "
					+ "DatePart(mm, PictureAddDate) = " + dr[1].ToString() + " AND "
					+ "DatePart(dd, PictureAddDate) = " + dr[2].ToString();
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

			AddPicturesToList(nCur.Tag.ToString());

		}

		private void AddPicturesToList(String strWhereClause) 
		{

			// clear current list
			lvPics.Items.Clear();

			// figure out SQL statement
			String strSQL = "select PictureID, PictureDate, Title, Filename, Publish, PictureSort from Picture ";
			if (strWhereClause.Length > 0)
				strSQL += "WHERE " + strWhereClause;
			strSQL = strSQL + " ORDER BY PictureDate, PictureSort";

			// create dataadapter and ds
			DataSetPicture dsPicTemp = new DataSetPicture();
			SqlDataAdapter da = new SqlDataAdapter(strSQL, cn);
			da.Fill(dsPicTemp, "Picture");

			// loop through RS adding items
			foreach (DataSetPicture.PictureRow pr in dsPicTemp.Picture.Rows) 
			{
				// create and init a new list view item
				ListViewItem li = new ListViewItem(pr.PictureID.ToString());
				for (int i = 0; i < lvPics.Columns.Count-1; i++)
					li.SubItems.Add("");

				// Add fields
				li.SubItems[1].Text = pr.PictureDate.ToShortDateString();

				if (!pr.IsTitleNull())
					li.SubItems[2].Text = pr.Title;

				li.SubItems[3].Text = pr.Filename;

				if (pr.Publish)
					li.SubItems[4].Text = "x";

				li.SubItems[5].Text = pr.PictureSort.ToString();

				// add to the list
				lvPics.Items.Add(li);
			}

		}

		private void lvPics_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			LoadImage();
		}

		private void LoadImage() 
		{

			try 
			{
				// get rid of current image if we have one
				if (imgCurImage != null) 
				{   
					imgCurImage.Dispose();
					imgCurImage = null;
				}
				pbPic.Image = null;

				// bail out if more than one image is selected
				if (lvPics.SelectedItems.Count != 1) 
					return;

				ListViewItem li;
				li = lvPics.SelectedItems[0];

				String strFile = "\\\\kenny\\inetpub\\pictures\\" + li.SubItems[3].Text;
				strFile = strFile.Replace("/", "\\");

				if (!File.Exists(strFile)) 
				{
					MessageBox.Show("The file '" + strFile + "' was not found.", "Loading Picture", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// figure out the temp location, create directory if needed
				String strTempFile = System.Environment.GetEnvironmentVariable("TEMP") 
										+ "\\_pics\\" + li.SubItems[3].Text;
				strTempFile = strTempFile.Replace("/", "\\");
				String strTempDir = strTempFile.Substring(0, strTempFile.LastIndexOf("\\"));
				if (!Directory.Exists(strTempDir)) 
					Directory.CreateDirectory(strTempDir);

				// copy file to temp location
				if (!File.Exists(strTempFile) ||
						File.GetLastWriteTime(strTempFile) < File.GetLastWriteTime(strFile))
					File.Copy(strFile, strTempFile, true);

				System.Threading.Thread.Sleep(100);

				imgCurImage = Image.FromFile(strTempFile);
			
				panelPic.Width = (int) ( ( (float) imgCurImage.Width / (float) imgCurImage.Height) 
					* (float) panelPic.Height );
				pbPic.Width = panelPic.Width;
				pbPic.Height = panelPic.Height;

				panelPic.Left = (lvPics.Width/2) - (pbPic.Width/2);

				pbPic.Image = imgCurImage;

			}
			catch (FileNotFoundException fnfe) 
			{
				MessageBox.Show("The file '" + fnfe.Message + "' was not found when attempting to load the picture preview.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			catch (OutOfMemoryException oexcep) 
			{
				MessageBox.Show("The file could not be loaded.  There is not enough memory available.  (Error: " + oexcep.Message + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}
		private void splitter2_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			lvPics_SelectedIndexChanged(sender, e);
		}

		private void lvPics_DoubleClick(object sender, System.EventArgs e)
		{
			ListViewItem li;
			fStatus fStat = new fStatus(this, "Loading picture...", 0);

			if (lvPics.SelectedItems.Count == 0) return;

			li = lvPics.SelectedItems[0];

			fPicture f = new fPicture();

			f.MainForm = this;
			f.LoadPicture(Convert.ToInt32(li.Text));
//			f.LoadImage("\\\\kenny\\inetpub\\pictures\\" + li.SubItems[3].Text.Replace("/", "\\") );
			fStat.Hide();

			f.ShowDialog();

			if (!f.mblnCancel) 
			{
				li.SubItems[2].Text = f.Title;
				if (f.Publish)
					li.SubItems[4].Text = "x";
				else
					li.SubItems[4].Text = "";

				if (pbPic.Image == null)
					LoadImage();
			}

			while (f.MovePrevious || f.MoveNext) 
			{
				fStat.ShowStatusForm();

				// Check if we should move to previous item, if not exit
				if (f.MovePrevious) 
				{
					if (li.Index > 0)
						li = lvPics.Items[li.Index-1];
					else
						break;
				} 
					// Check if we should move to next item, if not exit
				else 
				{
					if (li.Index < lvPics.Items.Count-1)
						li = lvPics.Items[li.Index+1];
					else
						break;
				}

				// Load the selected picture
				f.LoadPicture(Convert.ToInt32(li.Text));
				//f.LoadImage("\\\\kenny\\inetpub\\pictures\\" + li.SubItems[3].Text.Replace("/", "\\") );
	
				fStat.Hide();
				f.ShowDialog();

				// If not cancelling, update title
				if (!f.mblnCancel) 
				{
					li.SubItems[2].Text = f.Title;
					if (f.Publish)
						li.SubItems[4].Text = "x";
					else
						li.SubItems[4].Text = "";
				}
				else
					break;

			}
			fStat.Hide();

		}

		private void menuAddPictures_Click(object sender, System.EventArgs e)
		{
			fAddPictures f = new fAddPictures();
			f.ShowDialog();
			LoadTreeView();

		}

		private void categoryTree1_ClickCategory(object sender, PicAdminCS.CategoryTreeEventArgs e)
		{
				
			AddPicturesToList("PictureID in "
				+ "(select pc.PictureID from PictureCategory pc inner join CategorySubCategory csc ON csc.SubCategoryID = pc.CategoryID "
				+ "where csc.CategoryID = " + e.categoryRow.CategoryID.ToString() + ")");
			
		}

		private void peopleCtl1_ClickPerson(object sender, PicAdminCS.PersonCtlEventArgs e)
		{

			AddPicturesToList("PictureID in "
				+ "(select PictureID from PicturePerson where PersonID = " + e.personRow.PersonID.ToString() + ")");

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
				if (imgCurImage != null) 
				{
					imgCurImage.Dispose();
					imgCurImage = null;
				}

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

		private void mnuPictureListMoveUp_Click(object sender, System.EventArgs e)
		{

			// get the current item, then swap with the other item
			ListViewItem li = lvPics.SelectedItems[0];
			SwapSortVals(li, lvPics.Items[li.Index-1]);

		}

		private void mnuPictureListMoveDown_Click(object sender, System.EventArgs e)
		{

			// get the current item, then swap with the other item
			ListViewItem li = lvPics.SelectedItems[0];
			SwapSortVals(li, lvPics.Items[li.Index+1]);

		}

		private void SwapSortVals(ListViewItem li1, ListViewItem li2) 
		{
			// open connection
			cn.Open();

			SqlTransaction trans = cn.BeginTransaction();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection  = cn;
			cmd.Transaction = trans;

			try 
			{

				// Update the database for the current picture
				cmd.CommandText = "update Picture set PictureSort = " + li2.SubItems[5].Text 
					+ " where PictureID = " + li1.Text;
				cmd.ExecuteNonQuery();

				// Update the database for the other picture
				cmd.CommandText = "update Picture set PictureSort = " + li1.SubItems[5].Text 
					+ " where PictureID = " + li2.Text;
				cmd.ExecuteNonQuery();

				trans.Commit();

				// Get the current value and save it
				String strTempVal = li1.SubItems[5].Text;
			
				// Update the list items
				li1.SubItems[5].Text = li2.SubItems[5].Text;
				li2.SubItems[5].Text = strTempVal;
				lvPics.Sort();
            
			}
			catch (Exception excep) 
			{
				trans.Rollback();			
				MessageBox.Show(excep.ToString());
			}
			finally 
			{
				// Close connection
				cn.Close();
			}
		}

		private void mnuPictureList_Popup(object sender, System.EventArgs e)
		{

			// disable all controls, and only enable depending on number of items selected
			foreach (MenuItem mni in mnuPictureList.MenuItems) 
				mni.Enabled = false;

			if (lvPics.SelectedItems.Count > 0) 
			{
				mnuPictureListDelete.Enabled = true;
				mnuPictureListEdit.Enabled   = true;

                // only enable up and down with one item selected, and not when at top or bottom
				if (lvPics.SelectedItems.Count == 1) 
				{
					if (lvPics.SelectedItems[0].Index != 0)
						mnuPictureListMoveUp.Enabled = true;
					if (lvPics.SelectedItems[0].Index != lvPics.Items.Count-1)
						mnuPictureListMoveDown.Enabled = true;

				}
			}

		}

		public void ClearCurrentImage() 
		{
			// clear current images
			if (imgCurImage != null) 
			{
				imgCurImage.Dispose();
				imgCurImage = null;
			}
			pbPic.Image = null;
		}

		private void fMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try 
			{
				if (Directory.Exists(System.Environment.GetEnvironmentVariable("TEMP") + "\\_piccache"))
				{
					if (imgCurImage != null) 
					{   
						imgCurImage.Dispose();
						imgCurImage = null;
					}
					pbPic.Image = null;
					fStatus fStat = new fStatus(null, "Cleaning up...", 0);
					Directory.Delete(System.Environment.GetEnvironmentVariable("TEMP") + "\\_piccache\\", true);
					fStat.Hide();
				} 
			}
			catch (Exception) 
			{
                MessageBox.Show("There was still a problem cleaning up.  But I caught it so it doesn't look quite so bad.  Blah.  Anyways, thanks for using me!", "S H I T ! ! !", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void tvAddedDate_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			AddPicturesToList(e.Node.Tag.ToString());
		}


	}
}
