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
		#region Declares

		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox textBox1;
		
		private bool			dirty				= false;

		#endregion

		#region Constructors / Disposal

		public Notes()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.Text = "Notes - UNINITIALIZED";
		}

		public Notes(Data data): base(data)
		{
			InitializeComponent();

			NoteConfigData noteConfigData = (NoteConfigData) Data.ConfigData;
			this.textBox1.Text = noteConfigData.Note;
			dirty = false;
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

		#endregion

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
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
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(216, 134);
			this.textBox1.TabIndex = 7;
			this.textBox1.Text = "textBoxNote";
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// Notes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(216, 134);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBox1});
			this.Name = "Notes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TitleVisible = true;
			this.Leave += new System.EventHandler(this.Notes_Leave);
			this.Deactivate += new System.EventHandler(this.Notes_Deactivate);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Behaviors

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			dirty = true;

			NoteConfigData noteConfigData = (NoteConfigData) Data.ConfigData;
			noteConfigData.Note = this.textBox1.Text;
		}
        
		private void Notes_Leave(object sender, System.EventArgs e)
		{
		}

		#endregion

		private void Notes_Deactivate(object sender, System.EventArgs e)
		{
			if (dirty)
			{
				Data.Save();
				dirty = false;
			}
		}

	}

	#region NoteConfigData

	public class NoteConfigData: msn2.net.Configuration.ConfigData
	{
		public static string TypeName = "Note";
		private string note;

		public NoteConfigData()
		{}

		public NoteConfigData(string note)
		{
			this.note = note;
		}

		public string Note
		{
			get { return note; }
			set { note = value; }
		}

		public override int IconIndex
		{
			get { return 2; }
		}

		#region Add

		public static new Data Add(object sender, ConfigDataAddEventArgs e)
		{
			InputPrompt p = new InputPrompt("Name the new note:");

			if (p.ShowShellDialog(e.Owner) == DialogResult.Cancel)
				return null;

			NoteConfigData noteConfigData = new NoteConfigData("");
			return e.Parent.Get(p.Value, noteConfigData, typeof(NoteConfigData));
		}

		#endregion

		#region Edit

		public static void Edit(object sender, ConfigDataAddEventArgs e)
		{
			Notes note = new Notes(e.Parent);
			note.Show();

			return; // e.Parent;
		}

		#endregion

	}

	#endregion

}

