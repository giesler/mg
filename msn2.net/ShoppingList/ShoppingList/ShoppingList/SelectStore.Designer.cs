namespace mn2.net.ShoppingList
{
    partial class SelectStore
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.cancel = new System.Windows.Forms.MenuItem();
            this.list = new System.Windows.Forms.ListView();
            this.nameColumn = new System.Windows.Forms.ColumnHeader();
            this.countColumn = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.cancel);
            // 
            // cancel
            // 
            this.cancel.Text = "&Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // list
            // 
            this.list.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.list.Columns.Add(this.nameColumn);
            this.list.Columns.Add(this.countColumn);
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.list.FullRowSelect = true;
            this.list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.list.Location = new System.Drawing.Point(0, 0);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(240, 268);
            this.list.TabIndex = 0;
            this.list.View = System.Windows.Forms.View.Details;
            this.list.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "ColumnHeader";
            this.nameColumn.Width = 100;
            // 
            // countColumn
            // 
            this.countColumn.Text = "ColumnHeader";
            this.countColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.countColumn.Width = 30;
            // 
            // SelectStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.list);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "SelectStore";
            this.Text = "Select a store";
            this.Resize += new System.EventHandler(this.SelectStore_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.MenuItem cancel;
        private System.Windows.Forms.ColumnHeader countColumn;
    }
}