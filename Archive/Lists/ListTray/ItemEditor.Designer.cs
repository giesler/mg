namespace ShoppingListTray
{
    partial class ItemEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.store = new System.Windows.Forms.ComboBox();
            this.item = new System.Windows.Forms.TextBox();
            this.save = new System.Windows.Forms.Button();
            this.saveNew = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // store
            // 
            this.store.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.store.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.store.FormattingEnabled = true;
            this.store.Location = new System.Drawing.Point(4, 3);
            this.store.Name = "store";
            this.store.Size = new System.Drawing.Size(342, 21);
            this.store.TabIndex = 15;
            // 
            // item
            // 
            this.item.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.item.Location = new System.Drawing.Point(4, 30);
            this.item.Multiline = true;
            this.item.Name = "item";
            this.item.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.item.Size = new System.Drawing.Size(342, 20);
            this.item.TabIndex = 1;
            this.item.TextChanged += new System.EventHandler(this.item_TextChanged);
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.Location = new System.Drawing.Point(107, 60);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 2;
            this.save.Text = "&Add";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // saveNew
            // 
            this.saveNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveNew.Location = new System.Drawing.Point(188, 60);
            this.saveNew.Name = "saveNew";
            this.saveNew.Size = new System.Drawing.Size(75, 23);
            this.saveNew.TabIndex = 3;
            this.saveNew.Text = "Add && &New";
            this.saveNew.UseVisualStyleBackColor = true;
            this.saveNew.Click += new System.EventHandler(this.saveNew_Click);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(269, 60);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "&Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ItemEditor
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(350, 91);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.saveNew);
            this.Controls.Add(this.save);
            this.Controls.Add(this.item);
            this.Controls.Add(this.store);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ItemEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "List Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox store;
        private System.Windows.Forms.TextBox item;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button saveNew;
        private System.Windows.Forms.Button cancel;
    }
}