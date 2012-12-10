using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for MediaError.
	/// </summary>
	public class MediaError : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label description;
		private System.Windows.Forms.Label mediaFileInfo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label filename;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MediaError(string description, string mediaFileInfo, string filename)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.description.Text	= description;
			this.mediaFileInfo.Text	= mediaFileInfo;
			this.filename.Text		= filename;
			
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
			this.description = new System.Windows.Forms.Label();
			this.mediaFileInfo = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.filename = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(448, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "There was a problem playing the song listed below.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Problem Description:";
			// 
			// description
			// 
			this.description.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.description.Location = new System.Drawing.Point(136, 40);
			this.description.Name = "description";
			this.description.Size = new System.Drawing.Size(272, 24);
			this.description.TabIndex = 2;
			this.description.Text = "[description]";
			// 
			// mediaFileInfo
			// 
			this.mediaFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.mediaFileInfo.Location = new System.Drawing.Point(136, 64);
			this.mediaFileInfo.Name = "mediaFileInfo";
			this.mediaFileInfo.Size = new System.Drawing.Size(272, 24);
			this.mediaFileInfo.TabIndex = 4;
			this.mediaFileInfo.Text = "[media file info]";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(128, 23);
			this.label4.TabIndex = 3;
			this.label4.Text = "Media File:";
			// 
			// filename
			// 
			this.filename.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.filename.Location = new System.Drawing.Point(136, 88);
			this.filename.Name = "filename";
			this.filename.Size = new System.Drawing.Size(272, 24);
			this.filename.TabIndex = 6;
			this.filename.Text = "[filename]";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 23);
			this.label5.TabIndex = 5;
			this.label5.Text = "Filename:";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(320, 120);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// MediaError
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonOK;
			this.ClientSize = new System.Drawing.Size(416, 158);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonOK,
																		  this.filename,
																		  this.label5,
																		  this.mediaFileInfo,
																		  this.label4,
																		  this.description,
																		  this.label2,
																		  this.label1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MediaError";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Error Playing Media";
			this.Load += new System.EventHandler(this.MediaError_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void MediaError_Load(object sender, System.EventArgs e)
		{
		
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

	}
}
