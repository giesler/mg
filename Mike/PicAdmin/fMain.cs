using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace PicAdmin
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
		private PicAdmin.CategoryTree categoryTree1;
		private PicAdmin.PeopleCtl peopleCtl1;
		private System.Windows.Forms.MenuItem menuUpdateCachedPictures;
		private System.Windows.Forms.ImageList pictureList;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Splitter splitter3;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItemThumbs;
		private System.Windows.Forms.MenuItem menuItemDetails;
		private System.ComponentModel.IContainer components;

		public fMain()
		{
			fStatus stat = new fStatus();
			stat.StatusText = "Loading...";
			stat.Max = 0;
			stat.Show();
			stat.Refresh();

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set the connection string
			cn.ConnectionString = "data source=kyle;initial catalog=picdb;user id=sa;password=too;persist security info=False";

			lvPics.ListViewItemSorter = new PicListViewSorter();

			// Load the trees
			LoadTreeView();

			stat.Hide();
			stat = null;
			
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
			this.components = new System.ComponentModel.Container();
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
			this.categoryTree1 = new PicAdmin.CategoryTree();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tvAddedDate = new System.Windows.Forms.TreeView();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.peopleCtl1 = new PicAdmin.PeopleCtl();
			this.daPictureDate = new System.Data.SqlClient.SqlDataAdapter();
			this.mnuPictureList = new System.Windows.Forms.ContextMenu();
			this.mnuPictureListEdit = new System.Windows.Forms.MenuItem();
			this.mnuPictureListDelete = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuPictureListMoveUp = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.lvPics = new System.Windows.Forms.ListView();
			this.pictureList = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuAddPictures = new System.Windows.Forms.MenuItem();
			this.menuUpdateCachedPictures = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemThumbs = new System.Windows.Forms.MenuItem();
			this.menuItemDetails = new System.Windows.Forms.MenuItem();
			this.panelPic.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
			this.SuspendLayout();
			// 
			// pbPic
			// 
			this.pbPic.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.pbPic.BackColor = System.Drawing.SystemColors.Window;
			this.pbPic.Name = "pbPic";
			this.pbPic.Size = new System.Drawing.Size(152, 166);
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
			this.splitter2.Location = new System.Drawing.Point(0, 586);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(696, 3);
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
			this.panelPic.Size = new System.Drawing.Size(304, 168);
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
			this.tabControl1.Location = new System.Drawing.Point(0, 25);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(256, 539);
			this.tabControl1.TabIndex = 9;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.tvDates});
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(248, 513);
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
			this.tvDates.Size = new System.Drawing.Size(248, 513);
			this.tvDates.TabIndex = 0;
			this.tvDates.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDates_AfterSelect);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.categoryTree1});
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(248, 513);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Category";
			// 
			// categoryTree1
			// 
			this.categoryTree1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.categoryTree1.Name = "categoryTree1";
			this.categoryTree1.Size = new System.Drawing.Size(248, 513);
			this.categoryTree1.TabIndex = 0;
			this.categoryTree1.ClickCategory += new PicAdmin.ClickCategoryEventHandler(this.categoryTree1_ClickCategory);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.tvAddedDate});
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(248, 513);
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
			this.tvAddedDate.Size = new System.Drawing.Size(248, 513);
			this.tvAddedDate.TabIndex = 1;
			this.tvAddedDate.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAddedDate_AfterSelect);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.peopleCtl1});
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(248, 513);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Person";
			// 
			// peopleCtl1
			// 
			this.peopleCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.peopleCtl1.Name = "peopleCtl1";
			this.peopleCtl1.Size = new System.Drawing.Size(248, 513);
			this.peopleCtl1.TabIndex = 0;
			this.peopleCtl1.ClickPerson += new PicAdmin.ClickPersonEventHandler(this.peopleCtl1_ClickPerson);
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
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "-";
			// 
			// lvPics
			// 
			this.lvPics.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
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
			this.lvPics.LargeImageList = this.pictureList;
			this.lvPics.Location = new System.Drawing.Point(259, 25);
			this.lvPics.Name = "lvPics";
			this.lvPics.Size = new System.Drawing.Size(437, 352);
			this.lvPics.SmallImageList = this.pictureList;
			this.lvPics.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvPics.TabIndex = 8;
			this.lvPics.DoubleClick += new System.EventHandler(this.lvPics_DoubleClick);
			this.lvPics.SelectedIndexChanged += new System.EventHandler(this.lvPics_SelectedIndexChanged);
			// 
			// pictureList
			// 
			this.pictureList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.pictureList.ImageSize = new System.Drawing.Size(100, 100);
			this.pictureList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem4});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuAddPictures,
																					  this.menuUpdateCachedPictures,
																					  this.menuItem3,
																					  this.menuFileExit});
			this.menuItem1.Text = "&File";
			// 
			// menuAddPictures
			// 
			this.menuAddPictures.Index = 0;
			this.menuAddPictures.Text = "&Add Pictures";
			this.menuAddPictures.Click += new System.EventHandler(this.menuAddPictures_Click);
			// 
			// menuUpdateCachedPictures
			// 
			this.menuUpdateCachedPictures.Index = 1;
			this.menuUpdateCachedPictures.Text = "Update Cached Pictures";
			this.menuUpdateCachedPictures.Click += new System.EventHandler(this.menuUpdateCachedPictures_Click);
			// 
			// menuFileExit
			// 
			this.menuFileExit.Index = 3;
			this.menuFileExit.Text = "E&xit";
			this.menuFileExit.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.panelPic});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(259, 380);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(437, 184);
			this.panel1.TabIndex = 5;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 564);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanel1,
																						  this.statusBarPanel2});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(696, 22);
			this.statusBar1.TabIndex = 10;
			this.statusBar1.Text = "statusBar1";
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.statusBarPanel1.Width = 10;
			// 
			// statusBarPanel2
			// 
			this.statusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel2.Width = 670;
			// 
			// toolBar1
			// 
			this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButton1});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(696, 25);
			this.toolBar1.TabIndex = 11;
			this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBar1.Visible = false;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Text = "Add";
			this.toolBarButton1.ToolTipText = "Add Pictures";
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(256, 25);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 539);
			this.splitter1.TabIndex = 12;
			this.splitter1.TabStop = false;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter3.Location = new System.Drawing.Point(259, 377);
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(437, 3);
			this.splitter3.TabIndex = 13;
			this.splitter3.TabStop = false;
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemThumbs,
																					  this.menuItemDetails});
			this.menuItem4.Text = "&View";
			// 
			// menuItemThumbs
			// 
			this.menuItemThumbs.Checked = true;
			this.menuItemThumbs.Index = 0;
			this.menuItemThumbs.Text = "&Thumbnails";
			this.menuItemThumbs.Click += new System.EventHandler(this.menuItemThumbs_Click);
			// 
			// menuItemDetails
			// 
			this.menuItemDetails.Index = 1;
			this.menuItemDetails.Text = "&Details";
			this.menuItemDetails.Click += new System.EventHandler(this.menuItemDetails_Click);
			// 
			// fMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(696, 589);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lvPics,
																		  this.splitter3,
																		  this.panel1,
																		  this.splitter1,
																		  this.tabControl1,
																		  this.toolBar1,
																		  this.statusBar1,
																		  this.splitter2});
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
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
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
			if (e.Action != TreeViewAction.Unknown) 
			{
				TreeNode nCur = tvDates.SelectedNode;
				if (nCur == null) return;

				AddPicturesToList(nCur.Tag.ToString());
			}
		}

		private void AddPicturesToList(String strWhereClause) 
		{

			// clear current list
			lvPics.Items.Clear();

			// figure out SQL statement
			String strSQL = "select p.PictureID, PictureDate, Title, "
				+ "pc.Filename, Publish, PictureSort "
				+ "from Picture p inner join PictureCache pc on pc.PictureID = p.PictureID "
				+ "where pc.MaxWidth = 125 and pc.MaxHeight = 125";
			if (strWhereClause.Length > 0)
				strSQL += " and " + strWhereClause;
			strSQL = strSQL + " ORDER BY PictureDate, PictureSort";

			// create dataadapter and ds
			DataSetPicture dsPicTemp = new DataSetPicture();
			SqlCommand cmd = new SqlCommand(strSQL, cn);
			if (cn.State != ConnectionState.Open)
				cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			pictureList.Images.Clear();

			// loop through RS adding items
			while (dr.Read())
			{
				// create and init a new list view item
				ListViewItem li = new ListViewItem(dr["PictureID"].ToString());
				li.Tag = dr["PictureID"].ToString();
				for (int i = 0; i < lvPics.Columns.Count-1; i++)
					li.SubItems.Add("");

				/*
				using (Image img = Image.FromFile(@"\\kenny\inetpub\pics.msn2.net\piccache\" + dr["Filename"].ToString()) ) 
				{
                    pictureList.Images.Add(img); 
				}
				li.ImageIndex = pictureList.Images.Count-1;
				*/
							
				// Add fields
				li.SubItems[1].Text = Convert.ToDateTime(dr["PictureDate"]).ToShortDateString();

				string title = "";
				if (dr["Title"] != null) 
				{
					li.SubItems[3].Text += dr["Title"].ToString();
					title += li.SubItems[3].Text;
				}

				li.SubItems[3].Text = dr["Filename"].ToString();

				if (Convert.ToBoolean(dr["Publish"])) 
				{
					title += "\nPublished";
					li.SubItems[4].Text = "x";
					li.ForeColor = Color.Green;
				} 
				else 
				{
					title += "\nNot Published";
					li.ForeColor = Color.Red;
				}
				li.Text = title;

				li.SubItems[5].Text = dr["PictureSort"].ToString();;

				// add to the list
				lvPics.Items.Add(li);
			}

			dr.Close();

			statusBar1.Panels[0].Text = lvPics.Items.Count + " pictures";
			this.Refresh();

			ThreadStart threadStart = new ThreadStart(this.LoadThumbs);
			Thread t = new Thread(threadStart);
			t.Start();

		}


		public void LoadThumbs() 
		{
			foreach (ListViewItem li in lvPics.Items) 
			{
				string filename = li.SubItems[3].Text;                
                
				using (Image img = Image.FromFile(@"\\kenny\inetpub\pics.msn2.net\piccache\" + filename) ) 
				{
					pictureList.Images.Add(img); 
				}
				li.ImageIndex = pictureList.Images.Count-1;
											
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

				String strFile = @"\\kenny\inetpub\pictures\" + li.SubItems[3].Text;
				strFile = strFile.Replace("/", "\\");


				// See if there is a resized image
				if (cn.State == ConnectionState.Closed)
					cn.Open();
				SqlCommand cmd = new SqlCommand("select * from PictureCache where PictureID = @PictureID and MaxWidth = 750 and MaxHeight = 700", cn);
				cmd.Parameters.Add("@PictureID", Convert.ToInt32(li.Tag));
				SqlDataReader dr = cmd.ExecuteReader();
				
				if (dr.Read()) 
				{
                    strFile = @"\\kenny\inetpub\pics.msn2.net\piccache\" + dr["Filename"].ToString();
				}
				dr.Close();

				if (!File.Exists(strFile)) 
				{
					MessageBox.Show("The file '" + strFile + "' was not found.", "Loading Picture", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}


				imgCurImage = Image.FromFile(strFile);
							
				panelPic.Width = (int) ( ( (float) imgCurImage.Width / (float) imgCurImage.Height) 
					* (float) panelPic.Height );
				pbPic.Width = panelPic.Width;
				pbPic.Height = panelPic.Height;

				panelPic.Left = (lvPics.Width/2) - (pbPic.Width/2);

				pbPic.Image = imgCurImage;
				//imgCurImage.Dispose();

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
			if (lvPics.SelectedItems.Count == 0) return;
			li = lvPics.SelectedItems[0];

			bool initialPicture = true;
			fPicture f = new fPicture();
			fPicture fOld = null;

			while (f.MovePrevious || f.MoveNext || initialPicture) 
			{
				// Check if we should move to previous item, if not exit
				if (f.MovePrevious) 
				{
					if (li.Index > 0)
						li = lvPics.Items[li.Index-1];
					else
						break;
				} 
					// Check if we should move to next item, if not exit
				else if (f.MoveNext)
				{
					if (li.Index < lvPics.Items.Count-1)
						li = lvPics.Items[li.Index+1];
					else
						break;
				}
					// Otherwise it is the first / selected list item
				else 
				{
					li = lvPics.SelectedItems[0];
					initialPicture = false;
				}

				f = new fPicture();
				f.MainForm = this;
				if (fOld != null) 
				{
					f.Left = fOld.Left;
					f.Top  = fOld.Top;
					f.Width = fOld.Width;
					f.Height = fOld.Height;
					f.WindowState = fOld.WindowState;
				}

				// Load the selected picture
				f.LoadPicture(Convert.ToInt32(li.Tag));
				f.ShowDialog();

				// If not cancelling, update title
				if (!f.mblnCancel) 
				{
					li.SubItems[2].Text = f.Title;
					if (f.Publish) 
					{
						li.SubItems[4].Text = "x";
						li.ForeColor = Color.Green; 
					}
					else 
					{
						li.SubItems[4].Text = "";
						li.ForeColor = Color.Red;
					}
				}
				else
					break;

				// Set the ref to the old copy
				fOld = f;

			}

		}

		private void menuAddPictures_Click(object sender, System.EventArgs e)
		{
			
			fAddPictures f = new fAddPictures();
			f.ShowDialog();
			LoadTreeView();

		}

		private void categoryTree1_ClickCategory(object sender, PicAdmin.CategoryTreeEventArgs e)
		{
				
			/*AddPicturesToList("p.PictureID in "
				+ "(select pc.PictureID from PictureCategory pc inner join CategorySubCategory csc ON csc.SubCategoryID = pc.CategoryID "
				+ "where csc.CategoryID = " + e.categoryRow.CategoryID.ToString() + ")");
			*/
			AddPicturesToList("p.PictureID in "
				+ "(select pc.PictureID from PictureCategory pc where pc.CategoryID = "
				+ e.categoryRow.CategoryID.ToString() + ")");
		}

		private void peopleCtl1_ClickPerson(object sender, PicAdmin.PersonCtlEventArgs e)
		{

			AddPicturesToList("p.PictureID in "
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

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				foreach (ListViewItem li in lvPics.SelectedItems) 
				{
					
					String strPath = "\\\\kenny\\inetpub\\pictures\\" + li.SubItems[3].Text;
					try 
					{
						// Load the list of cached images
						SqlCommand cmdCached = new SqlCommand("select * from PictureCache where PictureID = @PictureID", cn);
						cmdCached.Parameters.Add("@PictureID", Convert.ToInt32(li.Tag));
						SqlDataReader dr = cmdCached.ExecuteReader();
						while (dr.Read()) 
						{
							try 
							{
								string cachedFile = @"\\kenny\inetpub\pics.msn2.net\piccache\" + dr["Filename"].ToString();
                                if (File.Exists(cachedFile))
									File.Delete(cachedFile);
							} 
							catch (IOException ioe) 
							{
								MessageBox.Show(ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							}

						}
						dr.Close();
						
						// attempt to delete the file
						if (File.Exists(strPath))
							File.Delete(strPath);

						// we want to delete from db, but only if file delete worked
						SqlCommand cmd = new SqlCommand("delete from Picture where PictureID = " + li.Tag, cn);
						cmd.ExecuteNonQuery();

						// remove the deleted item
						lvPics.Items.Remove(li);
					}
					catch (IOException ioe) 
					{
                        MessageBox.Show(ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
			if (cn.State != ConnectionState.Open)
				cn.Open();

			SqlTransaction trans = cn.BeginTransaction();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection  = cn;
			cmd.Transaction = trans;

			try 
			{

				// Update the database for the current picture
				cmd.CommandText = "update Picture set PictureSort = " + li2.SubItems[5].Text 
					+ " where PictureID = " + li1.Tag;
				cmd.ExecuteNonQuery();

				// Update the database for the other picture
				cmd.CommandText = "update Picture set PictureSort = " + li1.SubItems[5].Text 
					+ " where PictureID = " + li2.Tag;
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
		}

		private void tvAddedDate_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Action != TreeViewAction.Unknown) 
			{
				AddPicturesToList(e.Node.Tag.ToString());
			}
		}

		private void menuUpdateCachedPictures_Click(object sender, System.EventArgs e)
		{

			if (MessageBox.Show("This will check ALL images to see if the date/time stamp has changed, and will recreate any cached images that are out of date.", 
				"Cache Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, 
				MessageBoxDefaultButton.Button2) == DialogResult.OK) 
			{

				ImageUtilities util = new ImageUtilities();

				SqlDataAdapter da = new SqlDataAdapter("select * from Picture", cn);
				DataSetPicture dsPicture = new DataSetPicture();
				da.Fill(dsPicture, "Picture");

				fStatus stat = new fStatus(this, "Creating cached images...", dsPicture.Picture.Rows.Count);
				int count = 0;

				foreach (DataSetPicture.PictureRow row in dsPicture.Picture.Rows) 
				{
					util.CreateUpdateCache(row.PictureID);

					count++;
					stat.Current = count;
					if (count % 10 == 0)
						stat.StatusText = "Creating cached images... (" + count.ToString() + "/" + dsPicture.Picture.Rows.Count + ")";
				}
			}
		}

		private void menuItemThumbs_Click(object sender, System.EventArgs e)
		{
			menuItemThumbs.Checked = true;
			menuItemDetails.Checked = false;
			lvPics.View = View.LargeIcon;
		}

		private void menuItemDetails_Click(object sender, System.EventArgs e)
		{
			menuItemDetails.Checked = true;
			menuItemThumbs.Checked = false;
			lvPics.View = View.Details;
		}



	}
}
