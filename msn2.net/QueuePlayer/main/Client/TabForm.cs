using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace msn2.net.QueuePlayer.Client
{
	public class TabForm : msn2.net.Controls.ShellForm
	{
		public System.Windows.Forms.Panel panelTabs;
		private System.Windows.Forms.Panel panelQuickSearch;
		private System.Windows.Forms.TextBox textBoxSearch;
		private System.Windows.Forms.Button buttonSearch;
		private System.ComponentModel.IContainer components = null;
		private UMPlayer player = null;
		private Search searchForm = null;

		public TabForm(UMPlayer player)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.player	= player;

			// THE NEXT LINES OF CODE ARE FROM MAGIC LIBRARY 
			// Calculate the IDE background colour as only half as dark as the control colour
			int red = 255 - ((255 - SystemColors.Control.R) / 3);
			int green = 255 - ((255 - SystemColors.Control.G) / 3);
			int blue = 255 - ((255 - SystemColors.Control.B) / 3);
			textBoxSearch.BackColor = Color.FromArgb(red, green, blue);

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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TabForm));
			this.panelTabs = new System.Windows.Forms.Panel();
			this.panelQuickSearch = new System.Windows.Forms.Panel();
			this.textBoxSearch = new System.Windows.Forms.TextBox();
			this.buttonSearch = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.panelTabs.SuspendLayout();
			this.panelQuickSearch.SuspendLayout();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// panelTabs
			// 
			this.panelTabs.BackColor = System.Drawing.Color.Transparent;
			this.panelTabs.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.panelQuickSearch});
			this.panelTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTabs.Name = "panelTabs";
			this.panelTabs.Size = new System.Drawing.Size(264, 168);
			this.panelTabs.TabIndex = 5;
			// 
			// panelQuickSearch
			// 
			this.panelQuickSearch.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this.textBoxSearch,
																						   this.buttonSearch});
			this.panelQuickSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelQuickSearch.Location = new System.Drawing.Point(0, 148);
			this.panelQuickSearch.Name = "panelQuickSearch";
			this.panelQuickSearch.Size = new System.Drawing.Size(264, 20);
			this.panelQuickSearch.TabIndex = 0;
			// 
			// textBoxSearch
			// 
			this.textBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxSearch.Multiline = true;
			this.textBoxSearch.Name = "textBoxSearch";
			this.textBoxSearch.Size = new System.Drawing.Size(208, 20);
			this.textBoxSearch.TabIndex = 0;
			this.textBoxSearch.Text = "<search for music>";
			this.textBoxSearch.Leave += new System.EventHandler(this.textBoxSearch_Leave);
			this.textBoxSearch.Enter += new System.EventHandler(this.textBoxSearch_Enter);
			// 
			// buttonSearch
			// 
			this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Right;
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSearch.Location = new System.Drawing.Point(208, 0);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(56, 20);
			this.buttonSearch.TabIndex = 1;
			this.buttonSearch.Text = "search";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// TabForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(264, 168);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelTabs});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TabForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "QueuePlayer Media";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.panelTabs.ResumeLayout(false);
			this.panelQuickSearch.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void textBoxSearch_Enter(object sender, System.EventArgs e)
		{
			textBoxSearch.SelectAll();
			this.AcceptButton = buttonSearch;
		}

		private void textBoxSearch_Leave(object sender, System.EventArgs e)
		{
			this.AcceptButton = null;
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			if (searchForm == null)
			{
				searchForm = new Search(this.textBoxSearch.Text);
				searchForm.Location	= new Point(this.Left, this.Top + this.Height);
				searchForm.Width	= this.Width;
				searchForm.Height	= Math.Min(300, Screen.GetBounds(this).Height - this.Top - this.Height);
				searchForm.Closed	+= new EventHandler(SearchForm_Closed);
			}
			else
			{
				searchForm.SearchNow(this.textBoxSearch.Text);
			}
			searchForm.Show();
		}

		private void SearchForm_Closed(object sender, EventArgs e)
		{
			searchForm = null;
		}

	}
}

