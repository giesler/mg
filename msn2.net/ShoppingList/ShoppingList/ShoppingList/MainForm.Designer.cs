namespace msn2.net.ShoppingList
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuRefresh = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuUndo = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuUpdateApp = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuTextBigger = new System.Windows.Forms.MenuItem();
            this.menuTextNormal = new System.Windows.Forms.MenuItem();
            this.menuTextSmall = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Item = new System.Windows.Forms.ColumnHeader();
            this.newItem = new System.Windows.Forms.TextBox();
            this.add = new System.Windows.Forms.Button();
            this.store = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuRefresh);
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // menuRefresh
            // 
            this.menuRefresh.Text = "&Refresh";
            this.menuRefresh.Click += new System.EventHandler(this.menuRefresh_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuUndo);
            this.menuItem1.MenuItems.Add(this.menuItem3);
            this.menuItem1.MenuItems.Add(this.menuUpdateApp);
            this.menuItem1.MenuItems.Add(this.menuItem5);
            this.menuItem1.MenuItems.Add(this.menuItem2);
            this.menuItem1.MenuItems.Add(this.menuItem4);
            this.menuItem1.MenuItems.Add(this.menuExit);
            this.menuItem1.Text = "&Menu";
            // 
            // menuUndo
            // 
            this.menuUndo.Enabled = false;
            this.menuUndo.Text = "&Undo";
            this.menuUndo.Click += new System.EventHandler(this.menuUndo_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "-";
            // 
            // menuUpdateApp
            // 
            this.menuUpdateApp.Text = "U&pdate app";
            this.menuUpdateApp.Click += new System.EventHandler(this.menuUpdateApp_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.menuTextBigger);
            this.menuItem2.MenuItems.Add(this.menuTextNormal);
            this.menuItem2.MenuItems.Add(this.menuTextSmall);
            this.menuItem2.Text = "Text Size";
            // 
            // menuTextBigger
            // 
            this.menuTextBigger.Text = "&Bigger";
            this.menuTextBigger.Click += new System.EventHandler(this.menuTextBigger_Click);
            // 
            // menuTextNormal
            // 
            this.menuTextNormal.Checked = true;
            this.menuTextNormal.Text = "&Normal";
            this.menuTextNormal.Click += new System.EventHandler(this.menuTextNormal_Click);
            // 
            // menuTextSmall
            // 
            this.menuTextSmall.Text = "&Small";
            this.menuTextSmall.Click += new System.EventHandler(this.menuTextSmall_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.Add(this.Item);
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(4, 58);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(233, 207);
            this.listView1.TabIndex = 0;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            // 
            // Item
            // 
            this.Item.Text = "item";
            this.Item.Width = 60;
            // 
            // newItem
            // 
            this.newItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.newItem.Location = new System.Drawing.Point(4, 31);
            this.newItem.Name = "newItem";
            this.newItem.Size = new System.Drawing.Size(176, 21);
            this.newItem.TabIndex = 1;
            // 
            // add
            // 
            this.add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add.Location = new System.Drawing.Point(186, 31);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(51, 21);
            this.add.TabIndex = 2;
            this.add.Text = "&Add";
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // store
            // 
            this.store.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.store.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.store.Location = new System.Drawing.Point(3, 3);
            this.store.Name = "store";
            this.store.Size = new System.Drawing.Size(234, 22);
            this.store.TabIndex = 3;
            this.store.SelectedIndexChanged += new System.EventHandler(this.store_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.store);
            this.Controls.Add(this.add);
            this.Controls.Add(this.newItem);
            this.Controls.Add(this.listView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "Shopping List";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Item;
        private System.Windows.Forms.TextBox newItem;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.ComboBox store;
        private System.Windows.Forms.MenuItem menuRefresh;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuUndo;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuUpdateApp;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuTextBigger;
        private System.Windows.Forms.MenuItem menuTextNormal;
        private System.Windows.Forms.MenuItem menuTextSmall;
        private System.Windows.Forms.MenuItem menuItem4;
    }
}

