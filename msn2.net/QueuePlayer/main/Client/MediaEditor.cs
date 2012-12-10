using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.QueuePlayer.Shared;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for MediaEdit.
	/// </summary>
	public class MediaEditor : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.TextBox textBoxArtist;
		private System.Windows.Forms.TextBox textBoxFilename;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBoxAlbum;
		private System.Windows.Forms.TextBox textBoxGenre;
		private System.Windows.Forms.TextBox textBoxComments;
		private System.Windows.Forms.TextBox textBoxBitrate;
		private DataSetMedia.MediaRow row;
		private System.Windows.Forms.TextBox textBoxTrack;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textBoxAdded;
		private System.Windows.Forms.TextBox textBoxUpdated;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textBoxDuration;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constuctor
		/// </summary>
		public MediaEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.textBoxName.Text = "Select a song.";
		}

		/// <summary>
		/// Loads media editor with the passed media row
		/// </summary>
		/// <param name="row">Media row to load</param>
		public MediaEditor(DataSetMedia.MediaRow row)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if (row == null)
			{
				this.textBoxName.Text = "Select a song.";
			}
			else 
			{
				LoadMedia(row);
			}

		}

		/// <summary>
		/// Loads the specified entry into the form
		/// </summary>
		/// <param name="row">Media row to load</param>
		public void LoadMedia(DataSetMedia.MediaRow row)
		{
			this.row = row;

			this.textBoxName.DataBindings.Add("Text", row, "Name");
			this.textBoxArtist.DataBindings.Add("Text", row, "Artist");
			this.textBoxAlbum.DataBindings.Add("Text", row, "Album");
			this.textBoxGenre.DataBindings.Add("Text", row, "Genre");
			this.textBoxComments.DataBindings.Add("Text", row, "Comments");
			this.textBoxFilename.DataBindings.Add("Text", row, "MediaFile");
			if (!row.IsTrackNull())
				this.textBoxTrack.DataBindings.Add("Text", row, "Track");
			if (!row.IsBitrateNull())
				this.textBoxBitrate.DataBindings.Add("Text", row, "Bitrate");
			if (!row.IsDateAddedNull())
				this.textBoxAdded.DataBindings.Add("Text", row, "DateAdded");
			if (!row.IsDateUpdatedNull())
				this.textBoxUpdated.DataBindings.Add("Text", row, "DateUpdated");
			if (!row.IsDurationNull())
				this.textBoxDuration.Text = ((int)(row.Duration / 60)).ToString() + ":" + ((int)(row.Duration % 60)).ToString("00");
		
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.textBoxArtist = new System.Windows.Forms.TextBox();
			this.textBoxFilename = new System.Windows.Forms.TextBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.textBoxAlbum = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxGenre = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxComments = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textBoxBitrate = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textBoxTrack = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textBoxAdded = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textBoxUpdated = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textBoxDuration = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Artist:";
			// 
			// label3
			// 
			this.label3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label3.Location = new System.Drawing.Point(8, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 23);
			this.label3.TabIndex = 12;
			this.label3.Text = "Filename:";
			// 
			// textBoxName
			// 
			this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxName.Location = new System.Drawing.Point(80, 8);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(416, 20);
			this.textBoxName.TabIndex = 1;
			this.textBoxName.Text = "";
			// 
			// textBoxArtist
			// 
			this.textBoxArtist.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxArtist.Location = new System.Drawing.Point(80, 32);
			this.textBoxArtist.Name = "textBoxArtist";
			this.textBoxArtist.Size = new System.Drawing.Size(416, 20);
			this.textBoxArtist.TabIndex = 3;
			this.textBoxArtist.Text = "";
			// 
			// textBoxFilename
			// 
			this.textBoxFilename.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxFilename.Location = new System.Drawing.Point(80, 184);
			this.textBoxFilename.Name = "textBoxFilename";
			this.textBoxFilename.ReadOnly = true;
			this.textBoxFilename.Size = new System.Drawing.Size(416, 20);
			this.textBoxFilename.TabIndex = 13;
			this.textBoxFilename.TabStop = false;
			this.textBoxFilename.Text = "";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonOK.Location = new System.Drawing.Point(336, 264);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 16;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonCancel.Location = new System.Drawing.Point(416, 264);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 17;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// textBoxAlbum
			// 
			this.textBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxAlbum.Location = new System.Drawing.Point(80, 56);
			this.textBoxAlbum.Name = "textBoxAlbum";
			this.textBoxAlbum.Size = new System.Drawing.Size(328, 20);
			this.textBoxAlbum.TabIndex = 5;
			this.textBoxAlbum.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 23);
			this.label4.TabIndex = 4;
			this.label4.Text = "Album:";
			// 
			// textBoxGenre
			// 
			this.textBoxGenre.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxGenre.Location = new System.Drawing.Point(80, 80);
			this.textBoxGenre.Name = "textBoxGenre";
			this.textBoxGenre.Size = new System.Drawing.Size(416, 20);
			this.textBoxGenre.TabIndex = 9;
			this.textBoxGenre.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 80);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 23);
			this.label5.TabIndex = 8;
			this.label5.Text = "Genre:";
			// 
			// textBoxComments
			// 
			this.textBoxComments.AcceptsReturn = true;
			this.textBoxComments.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxComments.Location = new System.Drawing.Point(80, 104);
			this.textBoxComments.Multiline = true;
			this.textBoxComments.Name = "textBoxComments";
			this.textBoxComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxComments.Size = new System.Drawing.Size(416, 72);
			this.textBoxComments.TabIndex = 11;
			this.textBoxComments.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 104);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 23);
			this.label6.TabIndex = 10;
			this.label6.Text = "Comments:";
			// 
			// textBoxBitrate
			// 
			this.textBoxBitrate.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxBitrate.Location = new System.Drawing.Point(80, 208);
			this.textBoxBitrate.Name = "textBoxBitrate";
			this.textBoxBitrate.ReadOnly = true;
			this.textBoxBitrate.Size = new System.Drawing.Size(168, 20);
			this.textBoxBitrate.TabIndex = 15;
			this.textBoxBitrate.TabStop = false;
			this.textBoxBitrate.Text = "";
			// 
			// label7
			// 
			this.label7.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label7.Location = new System.Drawing.Point(8, 208);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 23);
			this.label7.TabIndex = 14;
			this.label7.Text = "Bitrate:";
			// 
			// textBoxTrack
			// 
			this.textBoxTrack.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.textBoxTrack.Location = new System.Drawing.Point(456, 56);
			this.textBoxTrack.Name = "textBoxTrack";
			this.textBoxTrack.Size = new System.Drawing.Size(40, 20);
			this.textBoxTrack.TabIndex = 7;
			this.textBoxTrack.Text = "";
			// 
			// label8
			// 
			this.label8.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.label8.Location = new System.Drawing.Point(416, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 23);
			this.label8.TabIndex = 6;
			this.label8.Text = "Track:";
			// 
			// textBoxAdded
			// 
			this.textBoxAdded.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxAdded.Location = new System.Drawing.Point(80, 232);
			this.textBoxAdded.Name = "textBoxAdded";
			this.textBoxAdded.ReadOnly = true;
			this.textBoxAdded.Size = new System.Drawing.Size(168, 20);
			this.textBoxAdded.TabIndex = 19;
			this.textBoxAdded.TabStop = false;
			this.textBoxAdded.Text = "";
			// 
			// label9
			// 
			this.label9.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label9.Location = new System.Drawing.Point(8, 232);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(72, 23);
			this.label9.TabIndex = 18;
			this.label9.Text = "Added:";
			// 
			// textBoxUpdated
			// 
			this.textBoxUpdated.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxUpdated.Location = new System.Drawing.Point(328, 232);
			this.textBoxUpdated.Name = "textBoxUpdated";
			this.textBoxUpdated.ReadOnly = true;
			this.textBoxUpdated.Size = new System.Drawing.Size(168, 20);
			this.textBoxUpdated.TabIndex = 21;
			this.textBoxUpdated.TabStop = false;
			this.textBoxUpdated.Text = "";
			// 
			// label10
			// 
			this.label10.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label10.Location = new System.Drawing.Point(256, 232);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(72, 23);
			this.label10.TabIndex = 20;
			this.label10.Text = "Updated:";
			// 
			// textBoxDuration
			// 
			this.textBoxDuration.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxDuration.Location = new System.Drawing.Point(328, 208);
			this.textBoxDuration.Name = "textBoxDuration";
			this.textBoxDuration.ReadOnly = true;
			this.textBoxDuration.Size = new System.Drawing.Size(168, 20);
			this.textBoxDuration.TabIndex = 23;
			this.textBoxDuration.TabStop = false;
			this.textBoxDuration.Text = "";
			// 
			// label11
			// 
			this.label11.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label11.Location = new System.Drawing.Point(256, 208);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(72, 23);
			this.label11.TabIndex = 22;
			this.label11.Text = "Duration:";
			// 
			// MediaEditor
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(504, 294);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBoxDuration,
																		  this.label11,
																		  this.textBoxUpdated,
																		  this.label10,
																		  this.textBoxAdded,
																		  this.label9,
																		  this.textBoxTrack,
																		  this.label8,
																		  this.textBoxBitrate,
																		  this.label7,
																		  this.textBoxComments,
																		  this.label6,
																		  this.textBoxGenre,
																		  this.label5,
																		  this.textBoxAlbum,
																		  this.label4,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.textBoxFilename,
																		  this.textBoxArtist,
																		  this.textBoxName,
																		  this.label3,
																		  this.label2,
																		  this.label1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MediaEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Media File Editor";
			this.Load += new System.EventHandler(this.MediaEditor_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			if (textBoxTrack.DataBindings.Count == 0 && textBoxTrack.Text.Length > 0)
				row.Track = Convert.ToInt32(textBoxTrack.Text);
			this.BindingContext[row].EndCurrentEdit();
			row.EndEdit();
			this.Visible = false;
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			if (row != null)
			{
				row.RejectChanges();
				row.ClearErrors();
			}
			this.Visible = false;		
		}

		private void MediaEditor_Load(object sender, System.EventArgs e)
		{
		
		}

		public string MediaName 
		{
			get { return textBoxName.Text; }
			set { textBoxName.Text = value; }
		}

		public string Artist 
		{
			get { return textBoxArtist.Text; }
			set { textBoxArtist.Text = value; }
		}

		public string Album 
		{
			get { return textBoxAlbum.Text; }
			set { textBoxAlbum.Text = value; }
		}

		public string Genre 
		{
			get { return textBoxGenre.Text; }
			set { textBoxGenre.Text = value; }
		}

		public string Comments 
		{
			get { return textBoxComments.Text; }
			set { textBoxComments.Text = value; }
		}

		public string Filename 
		{
			set { textBoxFilename.Text = value; }
		}

		public int Bitrate 
		{
			set { textBoxBitrate.Text = String.Format("{0} Kbps", value); }
		}

	}
}
