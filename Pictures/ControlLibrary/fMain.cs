using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web;
using System.Net;
using System.Text;

namespace msn2.net.Pictures.Controls
{
    
	/// <summary>
	/// Summary description for fMain.
	/// </summary>
	public class fMain : System.Windows.Forms.Form
	{
		#region Sorter
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
            
		#endregion

		#region Declares
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuFileExit;
		private System.Data.SqlClient.SqlDataAdapter daPictureDate;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
        private System.Data.SqlClient.SqlConnection cn;
        private System.Windows.Forms.MenuItem menuAddPictures;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.ContextMenu mnuPictureList;
		private System.Windows.Forms.MenuItem mnuPictureListEdit;
		private System.Windows.Forms.MenuItem mnuPictureListDelete;
		protected Image imgCurImage = null;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuPictureListMoveUp;
        private System.Windows.Forms.MenuItem mnuPictureListMoveDown;
        private System.Windows.Forms.MenuItem menuUpdateCachedPictures;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private System.Windows.Forms.StatusBarPanel statusBarPanel2;
        private System.ComponentModel.IContainer components;
		private string currentListViewQuery;
		private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem menuAddToCategory;
        private MenuItem menuRemoveFromCategory;
        private MenuItem menuItem8;
        private MenuItem menuAddSecurityGroup;
        private MenuItem menuRemoveSecurityGroup;

        private ViewPanel viewPanel;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox viewbyToolStripComboBox;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox imageSizeCombo;
        private ToolStripButton toolStripSelectAll;
        private ToolStripButton toolStripClearAll;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer1;
        private Panel panel1;
        private PictureList pictureList1;
        private SelectedPicturePanel selectedPictures;
        private ToolStripButton copytofolderToolStripButton;
		#endregion

		#region Constructor
		public fMain()
		{
			// Show status window
			fStatus stat			= new fStatus();
			stat.StatusText			= "Loading...";
			stat.Max				= 0;
			stat.Show();
			stat.Refresh();

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            stat.StatusText = "Yep, it still takes a while to start...";
            stat.Refresh();

            PicContext.Load(Msn2Config.Load(), 1);

            // Set the connection string
            cn.ConnectionString = PicContext.Current.Config.ConnectionString;

            viewPanel = new ViewPanel();
            viewPanel.RefreshView += new EventHandler(viewPanel_RefreshView);
            viewPanel.Dock = DockStyle.Fill;
            this.splitContainer2.Panel1.Controls.Add(viewPanel);

            stat.Hide();
			stat = null;

            pictureList1.ContextMenu = mnuPictureList;

            pictureList1.SelectedChanged += new PictureItemEventHandler(pictureList1_SelectedChanged);
            pictureList1.ItemSelected += new PictureItemEventHandler(pictureList1_ItemSelected);
            pictureList1.ItemUnselected += new PictureItemEventHandler(pictureList1_ItemUnselected);
            pictureList1.PictureDoubleClick += new PictureItemEventHandler(pictureList1_DoubleClick);

//            Mapping.TopoMap map = new Mapping.TopoMap();
//            map.Dock = DockStyle.Fill;
//
//            Form form = new Form();
//            form.WindowState = FormWindowState.Maximized;
//            form.Controls.Add(map);
//
//            form.Show();
//            
//            map.LoadMap();

        }
		#endregion
		#region Disposal
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
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.cn = new System.Data.SqlClient.SqlConnection();
            this.mnuPictureListMoveDown = new System.Windows.Forms.MenuItem();
            this.daPictureDate = new System.Data.SqlClient.SqlDataAdapter();
            this.mnuPictureList = new System.Windows.Forms.ContextMenu();
            this.mnuPictureListEdit = new System.Windows.Forms.MenuItem();
            this.mnuPictureListDelete = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuPictureListMoveUp = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuAddToCategory = new System.Windows.Forms.MenuItem();
            this.menuRemoveFromCategory = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuAddSecurityGroup = new System.Windows.Forms.MenuItem();
            this.menuRemoveSecurityGroup = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuAddPictures = new System.Windows.Forms.MenuItem();
            this.menuUpdateCachedPictures = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuFileExit = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.viewbyToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.imageSizeCombo = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSelectAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripClearAll = new System.Windows.Forms.ToolStripButton();
            this.copytofolderToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureList1 = new msn2.net.Pictures.Controls.PictureList();
            this.selectedPictures = new msn2.net.Pictures.Controls.SelectedPicturePanel();
            ((System.ComponentModel.ISupportInitialize)(this.daPictureDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
// 
// sqlSelectCommand1
// 
            this.sqlSelectCommand1.CommandText = @"SELECT DATEPART(yyyy, PictureDate) AS PicYear, DATEPART(mm, PictureDate) AS PicMonth, DATEPART(dd, PictureDate) AS PicDay FROM Picture GROUP BY DATEPART(yyyy, PictureDate), DATEPART(mm, PictureDate), DATEPART(dd, PictureDate) ORDER BY DATEPART(yyyy, PictureDate), DATEPART(mm, PictureDate), DATEPART(dd, PictureDate)";
            this.sqlSelectCommand1.Connection = this.cn;
// 
// cn
// 
            this.cn.ConnectionString = "data source=picdbserver;initial catalog=picdb;integrated security=SSPI;persist se" +
                "curity info=False;workstation id=CHEF;packet size=4096";
            this.cn.FireInfoMessageEventOnUserErrors = false;
// 
// mnuPictureListMoveDown
// 
            this.mnuPictureListMoveDown.Index = 4;
            this.mnuPictureListMoveDown.Name = "mnuPictureListMoveDown";
            this.mnuPictureListMoveDown.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.mnuPictureListMoveDown.Text = "Move D&own";
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
            this.mnuPictureListMoveDown,
            this.menuItem6,
            this.menuItem7,
            this.menuAddToCategory,
            this.menuRemoveFromCategory,
            this.menuItem8,
            this.menuAddSecurityGroup,
            this.menuRemoveSecurityGroup});
            this.mnuPictureList.Name = "mnuPictureList";
            this.mnuPictureList.Popup += new System.EventHandler(this.mnuPictureList_Popup);
// 
// mnuPictureListEdit
// 
            this.mnuPictureListEdit.Index = 0;
            this.mnuPictureListEdit.Name = "mnuPictureListEdit";
            this.mnuPictureListEdit.Text = "&Edit";
            this.mnuPictureListEdit.Click += new System.EventHandler(this.mnuPictureListEdit_Click);
// 
// mnuPictureListDelete
// 
            this.mnuPictureListDelete.Index = 1;
            this.mnuPictureListDelete.Name = "mnuPictureListDelete";
            this.mnuPictureListDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.mnuPictureListDelete.Text = "&Delete";
            this.mnuPictureListDelete.Click += new System.EventHandler(this.mnuPictureListDelete_Click);
// 
// menuItem2
// 
            this.menuItem2.Index = 2;
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Text = "-";
// 
// mnuPictureListMoveUp
// 
            this.mnuPictureListMoveUp.Index = 3;
            this.mnuPictureListMoveUp.Name = "mnuPictureListMoveUp";
            this.mnuPictureListMoveUp.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.mnuPictureListMoveUp.Text = "Move &Up";
// 
// menuItem6
// 
            this.menuItem6.Index = 5;
            this.menuItem6.Name = "menuItem6";
            this.menuItem6.Text = "&Picture Info";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
// 
// menuItem7
// 
            this.menuItem7.Index = 6;
            this.menuItem7.Name = "menuItem7";
            this.menuItem7.Text = "-";
// 
// menuAddToCategory
// 
            this.menuAddToCategory.Index = 7;
            this.menuAddToCategory.Name = "menuAddToCategory";
            this.menuAddToCategory.Text = "&Add to category...";
            this.menuAddToCategory.Click += new System.EventHandler(this.menuAddToCategory_Click);
// 
// menuRemoveFromCategory
// 
            this.menuRemoveFromCategory.Index = 8;
            this.menuRemoveFromCategory.Name = "menuRemoveFromCategory";
            this.menuRemoveFromCategory.Text = "&Remove from category...";
            this.menuRemoveFromCategory.Click += new System.EventHandler(this.menuRemoveFromCategory_Click);
// 
// menuItem8
// 
            this.menuItem8.Index = 9;
            this.menuItem8.Name = "menuItem8";
            this.menuItem8.Text = "-";
// 
// menuAddSecurityGroup
// 
            this.menuAddSecurityGroup.Index = 10;
            this.menuAddSecurityGroup.Name = "menuAddSecurityGroup";
            this.menuAddSecurityGroup.Text = "Add security gruop...";
            this.menuAddSecurityGroup.Click += new System.EventHandler(this.menuAddSecurityGroup_Click);
// 
// menuRemoveSecurityGroup
// 
            this.menuRemoveSecurityGroup.Index = 11;
            this.menuRemoveSecurityGroup.Name = "menuRemoveSecurityGroup";
            this.menuRemoveSecurityGroup.Text = "Remove security group...";
            this.menuRemoveSecurityGroup.Click += new System.EventHandler(this.menuRemoveSecurityGroup_Click);
// 
// menuItem3
// 
            this.menuItem3.Index = 3;
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Text = "-";
// 
// mainMenu1
// 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                this.menuItem1
            });
            this.mainMenu1.Name = "mainMenu1";
// 
// menuItem1
// 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAddPictures,
            this.menuUpdateCachedPictures,
            this.menuItem5,
            this.menuItem3,
            this.menuFileExit});
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Text = "&File";
// 
// menuAddPictures
// 
            this.menuAddPictures.Index = 0;
            this.menuAddPictures.Name = "menuAddPictures";
            this.menuAddPictures.Text = "&Add Pictures";
            this.menuAddPictures.Click += new System.EventHandler(this.menuAddPictures_Click);
// 
// menuUpdateCachedPictures
// 
            this.menuUpdateCachedPictures.Index = 1;
            this.menuUpdateCachedPictures.Name = "menuUpdateCachedPictures";
            this.menuUpdateCachedPictures.Text = "Update Cached Pictures";
            this.menuUpdateCachedPictures.Click += new System.EventHandler(this.menuUpdateCachedPictures_Click);
// 
// menuItem5
// 
            this.menuItem5.Index = 2;
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Text = "&Validate cached pictures";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
// 
// menuFileExit
// 
            this.menuFileExit.Index = 4;
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new System.EventHandler(this.menuItem2_Click);
// 
// statusBar1
// 
            this.statusBar1.Location = new System.Drawing.Point(0, 359);
            this.statusBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(566, 16);
            this.statusBar1.TabIndex = 10;
            this.statusBar1.Text = "statusBar1";
// 
// statusBarPanel1
// 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 10;
// 
// statusBarPanel2
// 
            this.statusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel2.Name = "statusBarPanel2";
            this.statusBarPanel2.Width = 539;
// 
// toolStrip1
// 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.viewbyToolStripComboBox,
            this.toolStripLabel1,
            this.imageSizeCombo,
            this.toolStripSelectAll,
            this.toolStripClearAll,
            this.copytofolderToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Raft = System.Windows.Forms.RaftingSides.None;
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Click += new System.EventHandler(this.toolStrip1_Click);
// 
// toolStripLabel2
// 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.SettingsKey = "fMain.toolStripLabel2";
            this.toolStripLabel2.Text = "View by:";
// 
// viewbyToolStripComboBox
// 
            this.viewbyToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewbyToolStripComboBox.Items.AddRange(new object[] {
            "category",
            "date picture taken",
            "date picture added",
            "person"});
            this.viewbyToolStripComboBox.Name = "viewbyToolStripComboBox";
            this.viewbyToolStripComboBox.SettingsKey = "fMain.viewbyToolStripComboBox";
            this.viewbyToolStripComboBox.Size = new System.Drawing.Size(100, 25);
            this.viewbyToolStripComboBox.Text = "category";
            this.viewbyToolStripComboBox.Click += new System.EventHandler(this.viewbyToolStripComboBox_Click);
// 
// toolStripLabel1
// 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.SettingsKey = "fMain.toolStripLabel1";
            this.toolStripLabel1.Text = "Image Size:";
// 
// imageSizeCombo
// 
            this.imageSizeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imageSizeCombo.Items.AddRange(new object[] {
            "50",
            "100",
            "150",
            "200",
            "250",
            "300"});
            this.imageSizeCombo.Name = "imageSizeCombo";
            this.imageSizeCombo.SettingsKey = "fMain.toolStripComboBox1";
            this.imageSizeCombo.Size = new System.Drawing.Size(75, 25);
            this.imageSizeCombo.Text = "100";
            this.imageSizeCombo.SelectedIndexChanged += new System.EventHandler(this.imageSizeCombo_SelectedIndexChanged);
// 
// toolStripSelectAll
// 
            this.toolStripSelectAll.Name = "toolStripSelectAll";
            this.toolStripSelectAll.SettingsKey = "fMain.toolStripButton1";
            this.toolStripSelectAll.Text = "Select All";
            this.toolStripSelectAll.Click += new System.EventHandler(this.toolStripSelectAll_Click);
// 
// toolStripClearAll
// 
            this.toolStripClearAll.Name = "toolStripClearAll";
            this.toolStripClearAll.SettingsKey = "fMain.toolStripButton1";
            this.toolStripClearAll.Text = "Clear All";
            this.toolStripClearAll.Click += new System.EventHandler(this.toolStripClearAll_Click);
// 
// copytofolderToolStripButton
// 
            this.copytofolderToolStripButton.Name = "copytofolderToolStripButton";
            this.copytofolderToolStripButton.SettingsKey = "fMain.copytofolderToolStripButton";
            this.copytofolderToolStripButton.Text = "Copy to folder";
            this.copytofolderToolStripButton.Click += new System.EventHandler(this.copytofolderToolStripButton_Click);
// 
// splitContainer2
// 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
// 
// Panel2
// 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(566, 334);
            this.splitContainer2.SplitterDistance = 189;
            this.splitContainer2.TabIndex = 17;
            this.splitContainer2.Text = "splitContainer2";
// 
// splitContainer1
// 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
// 
// Panel1
// 
            this.splitContainer1.Panel1.Controls.Add(this.pictureList1);
// 
// Panel2
// 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2MinSize = 150;
            this.splitContainer1.Size = new System.Drawing.Size(373, 334);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.TabIndex = 16;
            this.splitContainer1.Text = "splitContainer1";
// 
// panel1
// 
            this.panel1.Controls.Add(this.selectedPictures);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 150);
            this.panel1.TabIndex = 6;
// 
// pictureList1
// 
            this.pictureList1.BackColor = System.Drawing.Color.White;
            this.pictureList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureList1.Location = new System.Drawing.Point(0, 0);
            this.pictureList1.Name = "pictureList1";
            this.pictureList1.Size = new System.Drawing.Size(373, 180);
            this.pictureList1.TabIndex = 0;
            this.pictureList1.MultiSelectStart += new System.EventHandler(this.pictureList1_MultiSelectStart);
            this.pictureList1.MultiSelectEnd += new System.EventHandler(this.pictureList1_MultiSelectEnd);
// 
// selectedPictures
// 
            this.selectedPictures.BackColor = System.Drawing.SystemColors.Control;
            this.selectedPictures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedPictures.Location = new System.Drawing.Point(0, 0);
            this.selectedPictures.Name = "selectedPictures";
            this.selectedPictures.Size = new System.Drawing.Size(373, 150);
            this.selectedPictures.TabIndex = 0;
// 
// fMain
// 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(566, 375);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.toolStrip1);
            this.Menu = this.mainMenu1;
            this.Name = "fMain";
            this.Text = "Pic Admin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.daPictureDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion
		#region STAThread - Main
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new fMain());
		}
		#endregion

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void AddPicturesToList(String strWhereClause) 
		{
			currentListViewQuery	= strWhereClause;

            statusBar1.Panels[0].Text = "Loading pictures";

            PictureCollection pictures = PicContext.Current.PictureManager.GetPictures(strWhereClause);

            this.selectedPictures.ClearPictures();

            statusBar1.Panels[0].Text = "Loading " + pictures.Count.ToString() + " pictures";

            pictureList1.LoadPictures(pictures);

			statusBar1.Panels[0].Text = pictures.Count + " pictures";
		}

		private void menuAddPictures_Click(object sender, System.EventArgs e)
		{
			
			fAddPictures f = new fAddPictures();
            try
            {
                f.ShowDialog();
            }
            catch (ArgumentException)
            {
                // BUGBUG: accesibility exception
            }
            viewPanel.RefreshData();

		}

		private void mnuPictureListEdit_Click(object sender, System.EventArgs e)
		{
            pictureList1_DoubleClick(null, null);
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

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				foreach (int pictureId in pictureList1.SelectedItems)
				{
					try 
					{
                        // remove the deleted item
                        pictureList1.Remove(pictureId);

                        // Load the list of cached images
						SqlCommand cmdCached = new SqlCommand("select * from PictureCache where PictureID = @PictureID", cn);
						cmdCached.Parameters.Add("@PictureID", SqlDbType.Int);
                        cmdCached.Parameters["@PictureID"].Value = pictureId;
                        SqlDataReader dr = cmdCached.ExecuteReader();
						while (dr.Read()) 
						{
							try 
							{
								string cachedFile = PicContext.Current.Config.CacheDirectory + @"\" + dr["Filename"].ToString();
								if (File.Exists(cachedFile))
									File.Delete(cachedFile);
							} 
							catch (IOException ioe) 
							{
								MessageBox.Show(ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							}

						}
						dr.Close();

						// we want to delete from db, but only if file delete worked
						SqlCommand cmd = new SqlCommand("delete from Picture where PictureID = " + pictureId.ToString(), cn);
                        cmd.ExecuteNonQuery();

                    }
					catch (IOException ioe) 
					{
						MessageBox.Show(ioe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					

				}
				
				cn.Close();
			}
				
		}

//		private void SwapSortVals(ListViewItem li1, ListViewItem li2) 
//		{
//			// open connection
//			if (cn.State != ConnectionState.Open)
//				cn.Open();
//
//			SqlTransaction trans = cn.BeginTransaction();
//			SqlCommand cmd = new SqlCommand();
//			cmd.Connection  = cn;
//			cmd.Transaction = trans;
//
//			try 
//			{
//
//				// Update the database for the current picture
//				cmd.CommandText = "update Picture set PictureSort = " + li2.SubItems[5].Text 
//					+ " where PictureID = " + li1.Tag;
//				cmd.ExecuteNonQuery();
//
//				// Update the database for the other picture
//				cmd.CommandText = "update Picture set PictureSort = " + li1.SubItems[5].Text 
//					+ " where PictureID = " + li2.Tag;
//				cmd.ExecuteNonQuery();
//
//				trans.Commit();
//
//				// Get the current value and save it
//				String strTempVal = li1.SubItems[5].Text;
//			
//				// Update the list items
//				li1.SubItems[5].Text = li2.SubItems[5].Text;
//				li2.SubItems[5].Text = strTempVal;
//				lvPics.Sort();
//            
//			}
//			catch (Exception excep) 
//			{
//				trans.Rollback();			
//				MessageBox.Show(excep.ToString());
//			}
//			finally 
//			{
//				// Close connection
//				cn.Close();
//			}
//		}
//
		private void mnuPictureList_Popup(object sender, System.EventArgs e)
		{

/*			// disable all controls, and only enable depending on number of items selected
			foreach (MenuItem mni in mnuPictureList.MenuItems) 
				mni.Enabled = false;

			if (pictureList1.SelectedItems.Count > 0) 
			{
				mnuPictureListDelete.Enabled = true;
				mnuPictureListEdit.Enabled   = true;
                menuAddToCategory.Enabled = true;
                menuRemoveFromCategory.Enabled = true;
                menuAddSecurityGroup.Enabled = true;
                menuRemoveSecurityGroup.Enabled = true;

//                // only enable up and down with one item selected, and not when at top or bottom
//                if (pictureList1.SelectedItems.Count == 1)
//                {
//					if (lvPics.SelectedItems[0].Index != 0)
//						mnuPictureListMoveUp.Enabled = true;
//					if (lvPics.SelectedItems[0].Index != lvPics.Items.Count-1)
//						mnuPictureListMoveDown.Enabled = true;
//
//				}
			}
*/
		}

		public void ClearCurrentImage() 
		{
			// clear current images
			if (imgCurImage != null) 
			{
				imgCurImage.Dispose();
				imgCurImage = null;
			}
		}

		private void menuUpdateCachedPictures_Click(object sender, System.EventArgs e)
		{

			if (cacheStatus == null)
			{
				if (MessageBox.Show("This will check ALL images to see if the date/time stamp has changed, and will recreate any cached images that are out of date.", 
					"Cache Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, 
					MessageBoxDefaultButton.Button2) == DialogResult.OK) 
				{
					cacheStatus = new fStatus(this, "Starting process...", 0);

					Thread t = new Thread(new ThreadStart(ProcessCache));
					t.Start();
				}
			}
			else
			{
				MessageBox.Show("You cannot process cached images while they are already being processed on another thread.");
			}

		}

		private fStatus cacheStatus;

		private void CheckForCacheFiles()
		{
			SqlDataAdapter da		= new SqlDataAdapter("select * from PictureCache", cn);
			SqlCommandBuilder bld	= new SqlCommandBuilder(da);
			DataSet ds				= new DataSet();

			// Get the current cache list
			da.Fill(ds, "PictureCache");

			cacheStatus.StatusText	= "Verifying images...";
			cacheStatus.Max			= ds.Tables["PictureCache"].Rows.Count;

			foreach (DataRow dr in ds.Tables["PictureCache"].Rows)
			{
				// Check if file exists
				string filename		= PicContext.Current.Config.CacheDirectory + @"\" 
										+ dr["Filename"].ToString();

				if (!File.Exists(filename))
				{
					dr.Delete();
				}

				cacheStatus.Current++;
			}

			ds.WriteXml(@"c:\deletes.xml");
			da.Update(ds, "PictureCache");

//			cacheStatus.Dispose();
			cacheStatus				= null;
		}

		private void ProcessCache()
		{
			ImageUtilities util = new ImageUtilities();

			SqlDataAdapter da = new SqlDataAdapter("select * from Picture where PictureId not in (select PictureId from PictureCache where width = 750)", cn);
			DataSetPicture dsPicture = new DataSetPicture();
			da.Fill(dsPicture, "Picture");

			cacheStatus.StatusText	= "Creating cached images...";
			cacheStatus.Max			= dsPicture.Picture.Rows.Count;
			int count				= 0;

			foreach (DataSetPicture.PictureRow row in dsPicture.Picture.Rows) 
			{
				util.CreateUpdateCache(row.PictureID);

				count++;
				cacheStatus.Current = count;
			}

            if (cacheStatus.InvokeRequired)
            {
                cacheStatus.BeginInvoke(new MethodInvoker(cacheStatus.Close));
            }
            else
            {
                cacheStatus.Close();
            }

		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{

			if (cacheStatus == null)
			{
				if (MessageBox.Show("This will verify ALL cached image rows.", 
					"Cache Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, 
					MessageBoxDefaultButton.Button2) == DialogResult.OK) 
				{
					cacheStatus = new fStatus(this, "Starting process...", 0);

					Thread t = new Thread(new ThreadStart(CheckForCacheFiles));
					t.Start();
				}
			}
			else
			{
				MessageBox.Show("You cannot process cached images while they are already being processed on another thread.");
			}

		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
		
		}

        private void menuAddToCategory_Click(object sender, EventArgs e)
        {
            fSelectCategory cat = fSelectCategory.GetSelectCategoryDialog();
            if (cat.ShowDialog(this) == DialogResult.OK)
            {
                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    PicContext.Current.PictureManager.AddToCategory(pictureId, cat.SelectedCategory.CategoryId);
                }
                
            }
        }

        private void menuRemoveFromCategory_Click(object sender, EventArgs e)
        {
            fSelectCategory cat = new fSelectCategory();
            if (cat.ShowDialog(this) == DialogResult.OK)
            {
                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    PicContext.Current.PictureManager.RemoveFromCategory(pictureId, cat.SelectedCategory.CategoryId);
                }
            }
        }

        private void menuAddSecurityGroup_Click(object sender, EventArgs e)
        {
            fGroupSelect sel = new fGroupSelect();
            if (sel.ShowDialog(this) == DialogResult.OK)
            {
                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    PicContext.Current.PictureManager.AddToSecurityGroup(pictureId, sel.SelectedGroup.GroupID);
                }
            }

        }

        private void menuRemoveSecurityGroup_Click(object sender, EventArgs e)
        {
            fGroupSelect sel = new fGroupSelect();
            if (sel.ShowDialog(this) == DialogResult.OK)
            {
                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    PicContext.Current.PictureManager.RemoveFromSecurityGroup(pictureId, sel.SelectedGroup.GroupID);
                }
            }

        }

        void viewPanel_RefreshView(object sender, EventArgs e)
        {
            this.AddPicturesToList(viewPanel.WhereClause);
        }

        void pictureList1_DoubleClick(object sender, PictureItemEventArgs e)
        {
            if (e.Picture == null)
            {
                return;
            }

            int pictureId = e.Picture.Id;

            Slideshow ss = new Slideshow(new GetPreviousItemIdDelegate(pictureList1.GetPreviousPicture),
                new GetNextItemIdDelegate(pictureList1.GetNextPicture));
            ss.SetPicture(e.Picture);
            ss.SetSourceForm(this);
            ss.Show();

//            fPicture f = new fPicture();
//            f.NavigationControlsDataQuery = currentListViewQuery;
//
//            f.MainForm = this;
//
//            // Load the selected picture
//            f.LoadPicture(pictureId);
//            f.Show();
        }

        void pictureList1_SelectedChanged(object sender, PictureItemEventArgs e)
        {
        }

        private void imageSizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureList1.SetImageSize(int.Parse(imageSizeCombo.SelectedItem.ToString()));
        }

        private void toolStripSelectAll_Click(object sender, EventArgs e)
        {
            pictureList1.SelectAll();
        }

        private void toolStripClearAll_Click(object sender, EventArgs e)
        {
            pictureList1.ClearAll();
        }

        void pictureList1_ItemSelected(object sender, PictureItemEventArgs e)
        {
            this.selectedPictures.AddPicture(e.Picture);
        }

        void pictureList1_ItemUnselected(object sender, PictureItemEventArgs e)
        {
            this.selectedPictures.RemovePicture(e.Picture.Id);
        }

        private void pictureList1_MultiSelectStart(object sender, EventArgs e)
        {
            this.selectedPictures.MultiSelectStart();
        }

        private void pictureList1_MultiSelectEnd(object sender, EventArgs e)
        {
            this.selectedPictures.MultiSelectEnd();
        }

        private void copytofolderToolStripButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                string destFolder = dialog.SelectedPath;

                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    PictureData picture = PicContext.Current.PictureManager.GetPicture(pictureId);

                    string sourceFilename = PicContext.Current.Config.PictureDirectory
                        + @"\" + picture.Filename;

                    string fileExtenstion = sourceFilename.Substring(sourceFilename.LastIndexOf(".") + 1);

                    string destFilename = destFolder + @"\" + picture.Id.ToString() + "_" +
                        picture.Title + "." + fileExtenstion;

                    bool loop = true;
                    bool abort = false;

                    while (loop)
                    {
                        if (File.Exists(sourceFilename))
                        {
                            File.Copy(sourceFilename, destFilename, true);
                            loop = false;
                        }
                        else
                        {
                            DialogResult response = MessageBox.Show("Unable to find picture #" + picture.Id.ToString() + ": " + sourceFilename, "Can't find file", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);
                            if (response == DialogResult.Abort)
                            {
                                loop = false;
                                abort = true;
                            }
                            else if (response == DialogResult.Ignore)
                            {
                                loop = false;
                            }
                        }
                    }

                    if (abort)
                    {
                        break;
                    }
                }
            }
        }

        private void toolStrip1_Click(object sender, EventArgs e)
        {
        
        }

        private void viewbyToolStripComboBox_Click(object sender, EventArgs e)
        {
            ViewMode viewMode = ViewMode.Category;
            switch (viewbyToolStripComboBox.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    viewMode = ViewMode.DatePictureTaken;
                    break;
                case 2:
                    viewMode = ViewMode.DatePictureAdded;
                    break;
                case 3:
                    viewMode = ViewMode.Person;
                    break;
            }

            this.viewPanel.SetView(viewMode);
        }
    }
}
