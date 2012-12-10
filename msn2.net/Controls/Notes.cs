using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using msn2.net;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	public class Notes : msn2.net.Controls.ShellForm
	{
		private Crownwood.Magic.Controls.TabControl tabControl1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.ComponentModel.IContainer components = null;

		public Notes()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.Text = "Notes";

			LoadNotes();

			// Default values calc'd based on screen size
//			int defaultLeft = Screen.PrimaryScreen.Bounds.Right - this.Width - 50;
//			int defaultTop	= Screen.PrimaryScreen.Bounds.Bottom  - this.Height - 500;
			
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
			this.tabControl1 = new Crownwood.Magic.Controls.TabControl();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm;
			this.tabControl1.ContextMenu = this.contextMenu1;
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.HotTextColor = System.Drawing.SystemColors.ActiveCaption;
			this.tabControl1.HotTrack = false;
			this.tabControl1.ImageList = null;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.PositionTop = true;
			this.tabControl1.SelectedIndex = -1;
			this.tabControl1.ShowArrows = true;
			this.tabControl1.ShowClose = false;
			this.tabControl1.ShrinkPagesToFit = false;
			this.tabControl1.Size = new System.Drawing.Size(360, 248);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.TextColor = System.Drawing.SystemColors.MenuText;
			this.tabControl1.SelectionChanged += new System.EventHandler(this.tabControl1_SelectionChanged);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemAdd,
																						 this.menuItemEdit,
																						 this.menuItemDelete});
			// 
			// menuItemAdd
			// 
			this.menuItemAdd.Index = 0;
			this.menuItemAdd.Text = "&Add";
			this.menuItemAdd.Click += new System.EventHandler(this.menuItemAdd_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.Text = "&Edit";
			this.menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 2;
			this.menuItemDelete.Text = "&Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
			// 
			// Notes
			// 
			this.AutoLayout = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(360, 248);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.Name = "Notes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadNotes()
		{
			tabControl1.TabPages.Clear();

			SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			cn.Open();
			SqlCommand cmd = new SqlCommand("s_Notes_List", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@UserId", SqlDbType.NVarChar, 50);
			cmd.Parameters["@UserId"].Value = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			SqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				NotePage page =
					new NotePage(dr["Title"].ToString(), Convert.ToInt32(dr["NoteId"]));
				tabControl1.TabPages.Add(page);
			}

			dr.Close();
			cn.Close();

		}

		private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
		{
			NotePage page = (NotePage) tabControl1.TabPages[tabControl1.SelectedIndex];
			page.Page_Selected(this, e);
		}

		private void menuItemAdd_Click(object sender, System.EventArgs e)
		{
			InputPrompt prompt = new InputPrompt("Enter note name");
			if (prompt.ShowDialog(this) == DialogResult.Cancel)
				return;

			SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
			cn.Open();
			SqlCommand cmd = new SqlCommand("s_Notes_Item_Add", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			
			cmd.Parameters.Add("@NoteId", SqlDbType.Int);
			cmd.Parameters["@NoteId"].Direction = ParameterDirection.Output;

			cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 50);
			cmd.Parameters["@Title"].Value = prompt.Value;

			cmd.ExecuteNonQuery();

			LoadNotes();		
		}

		private void menuItemEdit_Click(object sender, System.EventArgs e)
		{
		
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
		
		}

		#region NotePage - Tab page

		public class NotePage: Crownwood.Magic.Controls.TabPage
		{
			private System.Windows.Forms.RichTextBox textBox;
			private int noteId;
			private string originalText = "";
			private System.Windows.Forms.ImageList il = new System.Windows.Forms.ImageList();

			public NotePage(string title, int noteId): this(title, noteId, new System.Windows.Forms.RichTextBox())
			{}

			public NotePage(string title, int noteId, System.Windows.Forms.RichTextBox _textBox): base(title, _textBox)
			{
				this.noteId = noteId;

				this.textBox = _textBox;
				textBox.Multiline = true;
				textBox.AcceptsTab = true;
				textBox.LostFocus += new EventHandler(TextBox_Leave);
				
			}

			public void Page_Selected(object sender, EventArgs e)
			{
				SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
				cn.Open();
				SqlCommand cmd = new SqlCommand("s_Notes_Item", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@NoteId", SqlDbType.Int);
				cmd.Parameters["@NoteId"].Value   = noteId;

				SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
				dr.Read();
				textBox.Text = dr["NoteText"].ToString();
				originalText = textBox.Text;

				dr.Close();
				cn.Close();

			}

			public void TextBox_Leave(object sender, EventArgs e)
			{
				if (textBox.Text == originalText)
					return;

				SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
				cn.Open();
				SqlCommand cmd = new SqlCommand("s_Notes_Item_Save", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@NoteId", SqlDbType.Int);
				cmd.Parameters.Add("@NoteText", SqlDbType.NVarChar, 3000);

				cmd.Parameters["@NoteId"].Value   = noteId;
				cmd.Parameters["@NoteText"].Value = textBox.Text;

				cmd.ExecuteNonQuery();
				cn.Close();

				originalText = textBox.Text;
			}

		}

		#endregion
	}
}

