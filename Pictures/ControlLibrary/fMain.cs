using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Windows.Media.Imaging;

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

        MainMenu mainMenu1;
        MenuItem menuItem1;
        MenuItem menuFileExit;
        System.Data.SqlClient.SqlDataAdapter daPictureDate;
        System.Data.SqlClient.SqlCommand sqlSelectCommand1;
        System.Data.SqlClient.SqlConnection cn;
        MenuItem menuAddPictures;
        MenuItem menuItem3;
        ContextMenu mnuPictureList;
        MenuItem mnuPictureListEdit;
        MenuItem mnuPictureListDelete;
        Image imgCurImage = null;
        MenuItem menuItem2;
        MenuItem mnuPictureListMoveUp;
        MenuItem mnuPictureListMoveDown;
        MenuItem menuUpdateCachedPictures;
        StatusBar statusBar1;
        StatusBarPanel statusBarPanel1;
        StatusBarPanel statusBarPanel2;
        System.ComponentModel.IContainer components;
        string currentListViewQuery;
        MenuItem menuItem5;
        MenuItem menuItem6;
        MenuItem menuItem7;
        MenuItem menuAddToCategory;
        MenuItem menuRemoveFromCategory;
        MenuItem menuItem8;
        MenuItem menuAddSecurityGroup;
        MenuItem menuRemoveSecurityGroup;

        ToolStrip toolStrip1;
        ToolStripLabel toolStripLabel1;
        ToolStripComboBox imageSizeCombo;
        ToolStripButton toolStripSelectAll;
        ToolStripButton toolStripClearAll;
        SplitContainer mainSplitContainer;
        SplitContainer rightListContainer;
        Panel panel1;
        PictureList pictureList1;
        SelectedPicturePanel selectedPictures;
        MenuItem menuRandomSlideshow;
        ToolStripSeparator toolStripSeparator1;
        ToolStripSeparator toolStripSeparator2;
        ToolStripButton copytofolderToolStripButton;
        MenuItem menuRotate;
        MenuItem menuRotateRight90;
        MenuItem menuRotateLeft90;
        MenuItem menuRotate180;
        ToolStripButton toolFileInfo;
        PictureFilterTreeView filter;
        ToolStripLabel toolStripLabel2;
        ToolStripComboBox maxPicCount;
        PictureControlSettings settings = new PictureControlSettings();
        MenuItem menuItem4;
        bool loading = true;
        #endregion

        #region Constructor
        public fMain()
        {
            // Show status window
            //stat.StatusText			= "Loading...";
            //stat.Max				= 0;

            settings.Reload();

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Login
            PictureConfig config = PictureConfig.Load();
            bool loginResult = PicContext.LoginWindowsUser(config);
            fStatus stat = new fStatus();
            if (loginResult == false)
            {
                // Prompt for email/pwd
                if (LoginUser(config, stat) == false)
                {
                    return;
                }
            }

            stat.StatusText = "Loading pictures...";

            // Set the connection string
            cn.ConnectionString = PicContext.Current.Config.ConnectionString;

            this.filter.Load();

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

            stat.Close();
            stat = null;

            this.maxPicCount.SelectedIndex = 1;
            this.imageSizeCombo.SelectedIndex = 1;

            this.loading = false;
        }

        private bool LoginUser(PictureConfig config, fStatus stat)
        {
            //
            // Prompt for login info
            //

            LoginDialog loginDialog = new LoginDialog();
            loginDialog.Email = settings.Last_Login_Email;
            while (loginDialog.ShowDialog(this) == DialogResult.OK)
            {
                stat.StatusText = "Logging on...";
                stat.Show(this);

                if (PicContext.Login(config, loginDialog.Email, loginDialog.Password) == true)
                {
                    settings.Last_Login_Email = loginDialog.Email;
                    break;
                }

                DialogResult result = MessageBox.Show(
                    this,
                    "Login failed.",
                    "MSN2 Login",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Exclamation);
                if (result == DialogResult.Cancel)
                {
                    break;
                }

                stat.Hide();
            }

            return (PicContext.Current != null);
        }

        #endregion

        #region Disposal
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.cn = new System.Data.SqlClient.SqlConnection();
            this.mnuPictureListMoveDown = new System.Windows.Forms.MenuItem();
            this.daPictureDate = new System.Data.SqlClient.SqlDataAdapter();
            this.mnuPictureList = new System.Windows.Forms.ContextMenu();
            this.mnuPictureListEdit = new System.Windows.Forms.MenuItem();
            this.menuRotate = new System.Windows.Forms.MenuItem();
            this.menuRotateRight90 = new System.Windows.Forms.MenuItem();
            this.menuRotateLeft90 = new System.Windows.Forms.MenuItem();
            this.menuRotate180 = new System.Windows.Forms.MenuItem();
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
            this.menuRandomSlideshow = new System.Windows.Forms.MenuItem();
            this.menuUpdateCachedPictures = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuFileExit = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.imageSizeCombo = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.maxPicCount = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSelectAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripClearAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.copytofolderToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolFileInfo = new System.Windows.Forms.ToolStripButton();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.filter = new msn2.net.Pictures.Controls.PictureFilterTreeView();
            this.rightListContainer = new System.Windows.Forms.SplitContainer();
            this.pictureList1 = new msn2.net.Pictures.Controls.PictureList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectedPictures = new msn2.net.Pictures.Controls.SelectedPicturePanel();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.rightListContainer.Panel1.SuspendLayout();
            this.rightListContainer.Panel2.SuspendLayout();
            this.rightListContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sqlSelectCommand1
            // 
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
            this.mnuPictureListMoveDown.Index = 5;
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
            this.menuRotate,
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
            this.mnuPictureList.Popup += new System.EventHandler(this.mnuPictureList_Popup);
            // 
            // mnuPictureListEdit
            // 
            this.mnuPictureListEdit.Index = 0;
            this.mnuPictureListEdit.Text = "&Edit";
            this.mnuPictureListEdit.Click += new System.EventHandler(this.mnuPictureListEdit_Click);
            // 
            // menuRotate
            // 
            this.menuRotate.Index = 1;
            this.menuRotate.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuRotateRight90,
            this.menuRotateLeft90,
            this.menuRotate180});
            this.menuRotate.Text = "&Rotate";
            // 
            // menuRotateRight90
            // 
            this.menuRotateRight90.Index = 0;
            this.menuRotateRight90.Text = "&Right 90";
            this.menuRotateRight90.Click += new System.EventHandler(this.menuRotateRight90_Click);
            // 
            // menuRotateLeft90
            // 
            this.menuRotateLeft90.Index = 1;
            this.menuRotateLeft90.Text = "&Left 90";
            this.menuRotateLeft90.Click += new System.EventHandler(this.menuRotateLeft90_Click);
            // 
            // menuRotate180
            // 
            this.menuRotate180.Index = 2;
            this.menuRotate180.Text = "180";
            this.menuRotate180.Click += new System.EventHandler(this.menuRotate180_Click);
            // 
            // mnuPictureListDelete
            // 
            this.mnuPictureListDelete.Index = 2;
            this.mnuPictureListDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.mnuPictureListDelete.Text = "&Delete";
            this.mnuPictureListDelete.Click += new System.EventHandler(this.mnuPictureListDelete_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "-";
            // 
            // mnuPictureListMoveUp
            // 
            this.mnuPictureListMoveUp.Index = 4;
            this.mnuPictureListMoveUp.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.mnuPictureListMoveUp.Text = "Move &Up";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 6;
            this.menuItem6.Text = "&Picture Info";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 7;
            this.menuItem7.Text = "-";
            // 
            // menuAddToCategory
            // 
            this.menuAddToCategory.Index = 8;
            this.menuAddToCategory.Text = "&Add to category...";
            this.menuAddToCategory.Click += new System.EventHandler(this.menuAddToCategory_Click);
            // 
            // menuRemoveFromCategory
            // 
            this.menuRemoveFromCategory.Index = 9;
            this.menuRemoveFromCategory.Text = "&Remove from category...";
            this.menuRemoveFromCategory.Click += new System.EventHandler(this.menuRemoveFromCategory_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 10;
            this.menuItem8.Text = "-";
            // 
            // menuAddSecurityGroup
            // 
            this.menuAddSecurityGroup.Index = 11;
            this.menuAddSecurityGroup.Text = "Add security gruop...";
            this.menuAddSecurityGroup.Click += new System.EventHandler(this.menuAddSecurityGroup_Click);
            // 
            // menuRemoveSecurityGroup
            // 
            this.menuRemoveSecurityGroup.Index = 12;
            this.menuRemoveSecurityGroup.Text = "Remove security group...";
            this.menuRemoveSecurityGroup.Click += new System.EventHandler(this.menuRemoveSecurityGroup_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 5;
            this.menuItem3.Text = "-";
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
            this.menuRandomSlideshow,
            this.menuUpdateCachedPictures,
            this.menuItem4,
            this.menuItem5,
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
            // menuRandomSlideshow
            // 
            this.menuRandomSlideshow.Index = 1;
            this.menuRandomSlideshow.Text = "&Random slideshow";
            this.menuRandomSlideshow.Click += new System.EventHandler(this.menuRandomSlideshow_Click);
            // 
            // menuUpdateCachedPictures
            // 
            this.menuUpdateCachedPictures.Index = 2;
            this.menuUpdateCachedPictures.Text = "Update Cached Pictures";
            this.menuUpdateCachedPictures.Click += new System.EventHandler(this.menuUpdateCachedPictures_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.Text = "&Validate cached pictures";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Index = 6;
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 389);
            this.statusBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(1018, 16);
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
            this.statusBarPanel2.Width = 991;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.imageSizeCombo,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.maxPicCount,
            this.toolStripSelectAll,
            this.toolStripClearAll,
            this.toolStripSeparator2,
            this.copytofolderToolStripButton,
            this.toolFileInfo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1018, 25);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(66, 22);
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
            this.imageSizeCombo.Size = new System.Drawing.Size(75, 25);
            this.imageSizeCombo.SelectedIndexChanged += new System.EventHandler(this.imageSizeCombo_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(77, 22);
            this.toolStripLabel2.Text = "Max Pictures:";
            // 
            // maxPicCount
            // 
            this.maxPicCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.maxPicCount.Items.AddRange(new object[] {
            "100",
            "250",
            "500",
            "1000",
            "2500",
            "5000"});
            this.maxPicCount.Name = "maxPicCount";
            this.maxPicCount.Size = new System.Drawing.Size(75, 25);
            this.maxPicCount.SelectedIndexChanged += new System.EventHandler(this.maxPicCount_SelectedIndexChanged);
            // 
            // toolStripSelectAll
            // 
            this.toolStripSelectAll.Name = "toolStripSelectAll";
            this.toolStripSelectAll.Size = new System.Drawing.Size(59, 22);
            this.toolStripSelectAll.Text = "Select All";
            this.toolStripSelectAll.Click += new System.EventHandler(this.toolStripSelectAll_Click);
            // 
            // toolStripClearAll
            // 
            this.toolStripClearAll.Name = "toolStripClearAll";
            this.toolStripClearAll.Size = new System.Drawing.Size(55, 22);
            this.toolStripClearAll.Text = "Clear All";
            this.toolStripClearAll.Click += new System.EventHandler(this.toolStripClearAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // copytofolderToolStripButton
            // 
            this.copytofolderToolStripButton.Image = global::msn2.net.Pictures.Controls.Properties.Resources.move;
            this.copytofolderToolStripButton.Name = "copytofolderToolStripButton";
            this.copytofolderToolStripButton.Size = new System.Drawing.Size(103, 22);
            this.copytofolderToolStripButton.Text = "Copy to folder";
            this.copytofolderToolStripButton.Click += new System.EventHandler(this.copytofolderToolStripButton_Click);
            // 
            // toolFileInfo
            // 
            this.toolFileInfo.Image = ((System.Drawing.Image)(resources.GetObject("toolFileInfo.Image")));
            this.toolFileInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFileInfo.Name = "toolFileInfo";
            this.toolFileInfo.Size = new System.Drawing.Size(73, 22);
            this.toolFileInfo.Text = "Get XMP";
            this.toolFileInfo.Click += new System.EventHandler(this.toolFileInfo_Click);
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.filter);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.rightListContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(1018, 364);
            this.mainSplitContainer.SplitterDistance = 167;
            this.mainSplitContainer.TabIndex = 17;
            this.mainSplitContainer.Text = "splitContainer2";
            // 
            // filter
            // 
            this.filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filter.HideSelection = false;
            this.filter.ImageIndex = 0;
            this.filter.Location = new System.Drawing.Point(0, 0);
            this.filter.Name = "filter";
            this.filter.SelectedImageIndex = 0;
            this.filter.Size = new System.Drawing.Size(167, 364);
            this.filter.TabIndex = 0;
            this.filter.FilterChanged += new msn2.net.Pictures.Controls.FilterChangedHandler(this.filter_FilterChanged);
            // 
            // rightListContainer
            // 
            this.rightListContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightListContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.rightListContainer.Location = new System.Drawing.Point(0, 0);
            this.rightListContainer.Name = "rightListContainer";
            this.rightListContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // rightListContainer.Panel1
            // 
            this.rightListContainer.Panel1.Controls.Add(this.pictureList1);
            // 
            // rightListContainer.Panel2
            // 
            this.rightListContainer.Panel2.Controls.Add(this.panel1);
            this.rightListContainer.Panel2MinSize = 50;
            this.rightListContainer.Size = new System.Drawing.Size(847, 364);
            this.rightListContainer.SplitterDistance = 223;
            this.rightListContainer.TabIndex = 16;
            this.rightListContainer.Text = "splitContainer1";
            // 
            // pictureList1
            // 
            this.pictureList1.BackColor = System.Drawing.Color.White;
            this.pictureList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureList1.Location = new System.Drawing.Point(0, 0);
            this.pictureList1.Name = "pictureList1";
            this.pictureList1.SelectedItems = ((System.Collections.Generic.List<int>)(resources.GetObject("pictureList1.SelectedItems")));
            this.pictureList1.Size = new System.Drawing.Size(847, 223);
            this.pictureList1.TabIndex = 0;
            this.pictureList1.MultiSelectEnd += new System.EventHandler(this.pictureList1_MultiSelectEnd);
            this.pictureList1.MultiSelectStart += new System.EventHandler(this.pictureList1_MultiSelectStart);
            this.pictureList1.SelectedChanged += new msn2.net.Pictures.Controls.PictureItemEventHandler(this.pictureList1_SelectedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.selectedPictures);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(847, 137);
            this.panel1.TabIndex = 6;
            // 
            // selectedPictures
            // 
            this.selectedPictures.BackColor = System.Drawing.SystemColors.Control;
            this.selectedPictures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedPictures.Location = new System.Drawing.Point(0, 0);
            this.selectedPictures.Name = "selectedPictures";
            this.selectedPictures.Size = new System.Drawing.Size(847, 137);
            this.selectedPictures.TabIndex = 0;
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "Update category cache";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // fMain
            // 
            this.ClientSize = new System.Drawing.Size(1018, 405);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "fMain";
            this.Text = "MSN2 Pictures";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            this.mainSplitContainer.ResumeLayout(false);
            this.rightListContainer.Panel1.ResumeLayout(false);
            this.rightListContainer.Panel2.ResumeLayout(false);
            this.rightListContainer.ResumeLayout(false);
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

        protected override void OnLoad(EventArgs e)
        {
            if (PicContext.Current == null)
            {
                this.Close();
            }
            else
            {
                base.OnLoad(e);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            settings.Save();
        }

        void menuItem2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        void menuAddPictures_Click(object sender, System.EventArgs e)
        {

            AddPictureDialog f = new AddPictureDialog(PicContext.Current);
            try
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    this.filter.SelectCategory(f.ImportCategory);
                }
            }
            catch (ArgumentException)
            {
                // BUGBUG: accesibility exception
            }
        }

        void mnuPictureListEdit_Click(object sender, System.EventArgs e)
        {
            PictureItemEventArgs args = new PictureItemEventArgs(
                pictureList1.GetSelectedPictureData());

            pictureList1_DoubleClick(null, args);
        }

        void mnuPictureListDelete_Click(object sender, System.EventArgs e)
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

        void mnuPictureList_Popup(object sender, System.EventArgs e)
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

        void menuUpdateCachedPictures_Click(object sender, System.EventArgs e)
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

        void CheckForCacheFiles()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from PictureCache", cn);
            SqlCommandBuilder bld = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();

            // Get the current cache list
            da.Fill(ds, "PictureCache");

            cacheStatus.StatusText = "Verifying images...";
            cacheStatus.Max = ds.Tables["PictureCache"].Rows.Count;

            foreach (DataRow dr in ds.Tables["PictureCache"].Rows)
            {
                // Check if file exists
                string filename = PicContext.Current.Config.CacheDirectory + @"\"
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
            cacheStatus = null;
        }

        void ProcessCache()
        {
            ImageUtilities util = new ImageUtilities();

            var q = from p in PicContext.Current.PictureManager.GetPictures()
                    where (from pc in p.PictureCaches
                           where pc.Width == 750
                           select pc).Any() == false
                    select p;

            cacheStatus.StatusText = "Creating cached images...";
            int count = q.Count();

            foreach (Picture picture in q)
            {
                util.CreateUpdateCache(picture.Id);

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

        void menuItem5_Click(object sender, System.EventArgs e)
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

        void menuItem6_Click(object sender, System.EventArgs e)
        {

        }

        void menuAddToCategory_Click(object sender, EventArgs e)
        {
            fSelectCategory cat = fSelectCategory.GetSelectCategoryDialog();
            if (cat.ShowDialog(this) == DialogResult.OK)
            {
                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    PicContext.Current.PictureManager.AddToCategory(pictureId, cat.SelectedCategory.Id);
                }

            }
        }

        void menuRemoveFromCategory_Click(object sender, EventArgs e)
        {
            fSelectCategory cat = new fSelectCategory();
            if (cat.ShowDialog(this) == DialogResult.OK)
            {
                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    PicContext.Current.PictureManager.RemoveFromCategory(pictureId, cat.SelectedCategory.Id);
                }
            }
        }

        void menuAddSecurityGroup_Click(object sender, EventArgs e)
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

        void menuRemoveSecurityGroup_Click(object sender, EventArgs e)
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

        void filter_FilterChanged(string whereClause)
        {
            this.currentListViewQuery = whereClause;

            statusBar1.Panels[0].Text = "Finding pictures...";

            this.selectedPictures.ClearPictures();

            ThreadPool.QueueUserWorkItem(new WaitCallback(QueryPictures), this.filter.GetPictureQuery());
        }

        void QueryPictures(object oQuery)
        {
            if (oQuery != null)
            {
                IQueryable<Picture> query = (IQueryable<Picture>)oQuery;
                List<Picture> pictures = query.ToList<Picture>();

                this.BeginInvoke(new WaitCallback(DisplayPictures), pictures);
            }
        }

        void DisplayPictures(object pics)
        {
            List<Picture> allPictures = (List<Picture>)pics;
            int maxCount = int.Parse(this.maxPicCount.SelectedItem.ToString());

            if (allPictures.Count < maxCount)
            {
                statusBar1.Panels[0].Text = "Displaying " + allPictures.Count.ToString() + " pictures...";
                pictureList1.LoadPictures(allPictures);
                statusBar1.Panels[0].Text = allPictures.Count + " pictures";
            }
            else
            {
                statusBar1.Panels[0].Text = "Displaying " + maxCount.ToString() + "/" + allPictures.Count.ToString() + " pictures...";

                List<Picture> displayPictures = new List<Picture>();
                for (int i = 0; i < maxCount; i++)
                {
                    displayPictures.Add(allPictures[i]);
                }
                pictureList1.LoadPictures(displayPictures);
                statusBar1.Panels[0].Text = string.Format("{0}/{1} pictures", displayPictures.Count, allPictures.Count);
            }
        }


        void pictureList1_DoubleClick(object sender, PictureItemEventArgs e)
        {
            if (e.Picture == null)
            {
                return;
            }

            Slideshow ss = new Slideshow(
                this.settings,
                new msn2.net.Pictures.Controls.Slideshow.GetPreviousItemIdDelegate(pictureList1.GetPreviousPicture),
                new msn2.net.Pictures.Controls.Slideshow.GetNextItemIdDelegate(pictureList1.GetNextPicture));
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
            bool itemSelected = (pictureList1.SelectedItems.Count > 0);

            toolStripClearAll.Enabled = itemSelected;
            toolStripSelectAll.Enabled = (pictureList1.SelectedItems.Count != pictureList1.Controls.Count);
            copytofolderToolStripButton.Enabled = itemSelected;
        }

        void imageSizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureList1.SetImageSize(int.Parse(imageSizeCombo.SelectedItem.ToString()));
        }

        void toolStripSelectAll_Click(object sender, EventArgs e)
        {
            pictureList1.SelectAll();
        }

        void toolStripClearAll_Click(object sender, EventArgs e)
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

        void pictureList1_MultiSelectStart(object sender, EventArgs e)
        {
            this.selectedPictures.MultiSelectStart();
        }

        void pictureList1_MultiSelectEnd(object sender, EventArgs e)
        {
            this.selectedPictures.MultiSelectEnd();
        }

        void copytofolderToolStripButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                string destFolder = dialog.SelectedPath;

                foreach (int pictureId in pictureList1.SelectedItems)
                {
                    Picture picture = PicContext.Current.PictureManager.GetPicture(pictureId);

                    string sourceFilename = PicContext.Current.Config.PictureDirectory
                        + @"\" + picture.Filename;

                    string fileExtenstion = sourceFilename.Substring(sourceFilename.LastIndexOf(".") + 1);

                    string destFilename = destFolder + @"\" + picture.Id.ToString() + "_" +
                        SafeFilename(picture.Title) + "." + fileExtenstion;

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

        private string SafeFilename(string title)
        {
            // \ / ; * ? " < > |

            string output = title;

            output = output.Replace(@"\", "_");
            output = output.Replace(@"/", "_");
            output = output.Replace(@";", "_");
            output = output.Replace(@"*", "_");
            output = output.Replace(@"?", "_");
            output = output.Replace(@"""", "_");
            output = output.Replace(@"<", "_");
            output = output.Replace(@">", "_");
            output = output.Replace(@"|", "_");

            return output;
        }

        void menuRandomSlideshow_Click(object sender, EventArgs e)
        {
            RandomSlideshow random = new RandomSlideshow();
            random.SetSourceForm(this);
            random.Show();
        }

        void menuRotateRight90_Click(object sender, EventArgs e)
        {
            RotateSelectedPictures(RotateFlipType.Rotate90FlipNone);
        }

        void RotateSelectedPictures(RotateFlipType rft)
        {
            PictureManager pm = PicContext.Current.PictureManager;

            foreach (int pictureId in pictureList1.SelectedItems)
            {
                pm.RotateImage(pictureId, rft);

                ImageUtilities iu = new ImageUtilities();
                iu.CreateUpdateCache(pictureId);

                pictureList1.ReloadPicture(pictureId);
            }
        }

        void menuRotateLeft90_Click(object sender, EventArgs e)
        {
            RotateSelectedPictures(RotateFlipType.Rotate270FlipNone);
        }

        void menuRotate180_Click(object sender, EventArgs e)
        {
            RotateSelectedPictures(RotateFlipType.Rotate180FlipNone);
        }

        void toolFileInfo_Click(object sender, EventArgs e)
        {
            if (pictureList1.SelectedItems.Count > 0)
            {
                Picture picture = PicContext.Current.PictureManager.GetPicture(pictureList1.SelectedItems[0]);
                string fileName = Path.Combine(PicContext.Current.Config.PictureDirectory, picture.Filename);

                string xmp = GetXmpXmlDocFromImage(fileName);
                MessageBox.Show(xmp, "XMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void UpdateDateTimes()
        {
            DialogResult result = MessageBox.Show(
                "Click 'Yes' to update date/time values or 'No' to only count the pictures with incorrect values.",
                "Change Picture Dates?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            int diffCount = 0;

            foreach (int pictureId in pictureList1.SelectedItems)
            {
                Picture picture = PicContext.Current.PictureManager.GetPicture(pictureId);
                string fileName = Path.Combine(PicContext.Current.Config.PictureDirectory, picture.Filename);

                string metaDataDate = GetDatePictureTaken(fileName);
                if (metaDataDate != null)
                {
                    DateTime dt = DateTime.Parse(metaDataDate);
                    TimeSpan diff = picture.PictureDate - dt;
                    if (diff.TotalHours > 3)
                    {
                        diffCount++;
                        if (result == DialogResult.Yes)
                        {
                            picture.PictureDate = dt;
                            PicContext.Current.SubmitChanges();
                        }
                        Debug.WriteLine(picture.Id.ToString() + ": Current: " + picture.PictureDate.ToString() + ", actual: " + dt.ToString());
                    }
                }
            }

            if (result == DialogResult.Yes)
            {
                MessageBox.Show(diffCount.ToString() + " pictures were updated.", "Update Dates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(diffCount.ToString() + " pictures would be updated.", "Update Dates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static string GetXmpXmlDocFromImage(string filename)
        {
            string contents;
            string xmlPart;
            string beginCapture = "<rdf:RDF";
            string endCapture = "</rdf:RDF>";
            int beginPos;
            int endPos;

            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                contents = sr.ReadToEnd();
                Debug.Write(contents.Length + " chars" + Environment.NewLine);
                sr.Close();
            }

            beginPos = contents.IndexOf(beginCapture, 0);
            endPos = contents.IndexOf(endCapture, 0);

            Debug.Write("xml found at pos: " + beginPos.ToString() + " - " + endPos.ToString());

            xmlPart = contents.Substring(beginPos, (endPos - beginPos) + endCapture.Length);

            Debug.Write("Xml len: " + xmlPart.Length.ToString());

            return xmlPart;
        }

        private XmlDocument LoadDoc(string xmpXmlDoc)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(xmpXmlDoc);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured while loading XML metadata from image. The error was: " + ex.Message);
            }

            try
            {
                doc.LoadXml(xmpXmlDoc);

                XmlNamespaceManager NamespaceManager = new XmlNamespaceManager(doc.NameTable);
                NamespaceManager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                NamespaceManager.AddNamespace("exif", "http://ns.adobe.com/exif/1.0/");
                NamespaceManager.AddNamespace("x", "adobe:ns:meta/");
                NamespaceManager.AddNamespace("xap", "http://ns.adobe.com/xap/1.0/");
                NamespaceManager.AddNamespace("tiff", "http://ns.adobe.com/tiff/1.0/");
                NamespaceManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

                // get ratings
                XmlNode xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description/xap:Rating", NamespaceManager);

                // Alternatively, there is a common form of RDF shorthand that writes simple properties as
                // attributes of the rdf:Description element.
                if (xmlNode == null)
                {
                    xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description", NamespaceManager);
                    xmlNode = xmlNode.Attributes["xap:Rating"];
                }

                if (xmlNode != null)
                {
                    Debug.WriteLine("Rating: " + xmlNode.InnerText);
                }

                // get keywords
                xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description/dc:subject/rdf:Bag", NamespaceManager);

                if (xmlNode != null)
                {
                    foreach (XmlNode li in xmlNode)
                    {
                        Debug.Write(li.InnerText + " ");
                    }
                }

                // get description
                xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description/dc:description/rdf:Alt", NamespaceManager);

                if (xmlNode != null)
                {
                    Debug.Write(xmlNode.ChildNodes[0].InnerText);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while readning meta-data from image. The error was: " + ex.Message);
            }

            return doc;
        }

        void maxPicCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading == false)
            {
                this.filter_FilterChanged(this.filter.WhereClause);
            }
        }

        void menuItem4_Click(object sender, EventArgs e)
        {
            PicContext.Current.CategoryManager.ReloadCategoryCache();
        }

        public static string GetDatePictureTaken(string fileName)
        {
            string dateTaken = null;

            using (StreamReader stream = new StreamReader(fileName))
            {
                BitmapSource source = null;
                try
                {
                    source = BitmapFrame.Create(stream.BaseStream);
                    BitmapMetadata metaData = source.Metadata as BitmapMetadata;
                    if (metaData != null && metaData.DateTaken != null)
                    {
                        dateTaken = metaData.DateTaken;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("Date taken read error: " + ex.ToString());
                }
            }

            return dateTaken;
        }
    }
}
