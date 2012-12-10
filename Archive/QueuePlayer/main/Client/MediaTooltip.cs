using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.QueuePlayer.Shared;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for MediaTooltip.
	/// </summary>
	public class MediaTooltip : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.Label labelName;
		public System.Windows.Forms.Label labelArtist;
		public System.Windows.Forms.Label labelAlbum;
		public System.Windows.Forms.Label labelTrack;

		private bool paintCalculated = false;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MediaTooltip()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelTrack = new System.Windows.Forms.Label();
			this.labelAlbum = new System.Windows.Forms.Label();
			this.labelArtist = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.labelTrack,
																				 this.labelAlbum,
																				 this.labelArtist,
																				 this.labelName});
			this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.ForeColor = System.Drawing.Color.LawnGreen;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(248, 72);
			this.panel1.TabIndex = 1;
			// 
			// labelTrack
			// 
			this.labelTrack.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.labelTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTrack.ForeColor = System.Drawing.Color.LawnGreen;
			this.labelTrack.Location = new System.Drawing.Point(184, 48);
			this.labelTrack.Name = "labelTrack";
			this.labelTrack.Size = new System.Drawing.Size(56, 23);
			this.labelTrack.TabIndex = 4;
			this.labelTrack.Text = "Track x";
			this.labelTrack.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelAlbum
			// 
			this.labelAlbum.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelAlbum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelAlbum.ForeColor = System.Drawing.Color.LawnGreen;
			this.labelAlbum.Location = new System.Drawing.Point(8, 48);
			this.labelAlbum.Name = "labelAlbum";
			this.labelAlbum.Size = new System.Drawing.Size(192, 24);
			this.labelAlbum.TabIndex = 3;
			this.labelAlbum.Text = "[Album]";
			this.labelAlbum.Paint += new System.Windows.Forms.PaintEventHandler(this.labelAlbum_Paint);
			// 
			// labelArtist
			// 
			this.labelArtist.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelArtist.CausesValidation = false;
			this.labelArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelArtist.ForeColor = System.Drawing.Color.LawnGreen;
			this.labelArtist.Location = new System.Drawing.Point(8, 24);
			this.labelArtist.Name = "labelArtist";
			this.labelArtist.Size = new System.Drawing.Size(232, 23);
			this.labelArtist.TabIndex = 2;
			this.labelArtist.Text = "[Artist]";
			this.labelArtist.Paint += new System.Windows.Forms.PaintEventHandler(this.labelArtist_Paint);
			// 
			// labelName
			// 
			this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelName.ForeColor = System.Drawing.Color.LawnGreen;
			this.labelName.Location = new System.Drawing.Point(8, 8);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(232, 23);
			this.labelName.TabIndex = 1;
			this.labelName.Text = "[Name]";
			this.labelName.Paint += new System.Windows.Forms.PaintEventHandler(this.labelName_Paint);
			// 
			// MediaTooltip
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(248, 72);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MediaTooltip";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "MediaTooltip";
			this.TopMost = true;
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.MediaTooltip_Paint);
			this.MouseLeave += new System.EventHandler(this.MediaTooltip_MouseLeave);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void MediaTooltip_MouseLeave(object sender, System.EventArgs e)
		{
			this.Visible = false;
		}

		private void labelName_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		}

		private void MediaTooltip_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (paintCalculated)
				return;

			paintCalculated = true;

			// Measure length of name
			SizeF sizeName = new SizeF();
			sizeName = e.Graphics.MeasureString(labelName.Text, labelName.Font);
			this.Width = Convert.ToInt32(sizeName.Width) + (labelName.Left * 3);

			// Measure length of artist
			SizeF sizeArtist = new SizeF();
			sizeArtist = e.Graphics.MeasureString(labelArtist.Text, labelName.Font);
			if (this.Width < Convert.ToInt32(sizeArtist.Width) + (labelName.Left * 2))
			{
				this.Width = Convert.ToInt32(sizeArtist.Width) + (labelName.Left * 2);
			}

			// Measure length of album, and increase size of form as needed
			if (labelAlbum.Text.Length > 0)
			{
				SizeF sizeAlbum = new SizeF();
				sizeAlbum = e.Graphics.MeasureString(labelAlbum.Text, labelAlbum.Font);
				if (this.Width < Convert.ToInt32(sizeAlbum.Width) + (labelName.Left * 2) + labelTrack.Width)
				{
					this.Width = Convert.ToInt32(sizeAlbum.Width) + (labelName.Left * 2) + labelTrack.Width;
				}
			}
		}

		public bool PaintCalculated
		{
			set { paintCalculated = value; }
		}

		private void labelAlbum_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		}

		private void labelArtist_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		}

		public new void Show()
		{
			ClientUtilities.FadeIn(this);
		}
		
		public new void Hide()
		{
			ClientUtilities.FadeOut(this);
		}

	}
}
