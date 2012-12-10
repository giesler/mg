using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace UMClient
{
	/// <summary>
	/// Summary description for MediaBar.
	/// </summary>
	public class MediaBar : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Label labelNowPlaying;

		private int startX, startY;
		public System.Windows.Forms.Button buttonPlayStop;
		public System.Windows.Forms.Button buttonPause;
		public System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonClose;
		private bool moving = false;
		public System.Windows.Forms.Button buttonPrevious;
		private System.Windows.Forms.ContextMenu contextMenuQuickList;
		private UMPlayer umPlayer;

		public MediaBar(UMPlayer umPlayer)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.umPlayer = umPlayer;
			this.TopLevel = true;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}

		public string CurrentSong 
		{
			set 
			{
				labelNowPlaying.Text = value;
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MediaBar));
			this.labelNowPlaying = new System.Windows.Forms.Label();
			this.buttonPlayStop = new System.Windows.Forms.Button();
			this.buttonPause = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonPrevious = new System.Windows.Forms.Button();
			this.contextMenuQuickList = new System.Windows.Forms.ContextMenu();
			this.SuspendLayout();
			// 
			// labelNowPlaying
			// 
			this.labelNowPlaying.BackColor = System.Drawing.Color.Transparent;
			this.labelNowPlaying.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelNowPlaying.ForeColor = System.Drawing.Color.Black;
			this.labelNowPlaying.Name = "labelNowPlaying";
			this.labelNowPlaying.Size = new System.Drawing.Size(192, 23);
			this.labelNowPlaying.TabIndex = 0;
			this.labelNowPlaying.Text = "[nothing playing]";
			this.labelNowPlaying.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelNowPlaying_MouseUp);
			this.labelNowPlaying.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelNowPlaying_MouseMove);
			this.labelNowPlaying.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelNowPlaying_MouseDown);
			// 
			// buttonPlayStop
			// 
			this.buttonPlayStop.BackColor = System.Drawing.Color.Transparent;
			this.buttonPlayStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlayStop.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonPlayStop.ForeColor = System.Drawing.Color.Black;
			this.buttonPlayStop.Location = new System.Drawing.Point(224, 0);
			this.buttonPlayStop.Name = "buttonPlayStop";
			this.buttonPlayStop.Size = new System.Drawing.Size(24, 16);
			this.buttonPlayStop.TabIndex = 1;
			this.buttonPlayStop.Text = "[]";
			this.buttonPlayStop.Click += new System.EventHandler(this.buttonPlayStop_Click);
			// 
			// buttonPause
			// 
			this.buttonPause.BackColor = System.Drawing.Color.Transparent;
			this.buttonPause.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPause.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonPause.ForeColor = System.Drawing.Color.Black;
			this.buttonPause.Location = new System.Drawing.Point(248, 0);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new System.Drawing.Size(24, 16);
			this.buttonPause.TabIndex = 2;
			this.buttonPause.Text = "||";
			this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.BackColor = System.Drawing.Color.Transparent;
			this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNext.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonNext.ForeColor = System.Drawing.Color.Black;
			this.buttonNext.Location = new System.Drawing.Point(272, 0);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(24, 16);
			this.buttonNext.TabIndex = 3;
			this.buttonNext.Text = ">>";
			this.buttonNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonNext_MouseUp);
			// 
			// buttonClose
			// 
			this.buttonClose.BackColor = System.Drawing.Color.Transparent;
			this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonClose.ForeColor = System.Drawing.Color.Black;
			this.buttonClose.Location = new System.Drawing.Point(304, 0);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(24, 16);
			this.buttonClose.TabIndex = 4;
			this.buttonClose.Text = "X";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.BackColor = System.Drawing.Color.Transparent;
			this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPrevious.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonPrevious.ForeColor = System.Drawing.Color.Black;
			this.buttonPrevious.Location = new System.Drawing.Point(200, 0);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(24, 16);
			this.buttonPrevious.TabIndex = 5;
			this.buttonPrevious.Text = "<<";
			this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
			// 
			// MediaBar
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(328, 16);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonPrevious,
																		  this.buttonClose,
																		  this.buttonNext,
																		  this.buttonPause,
																		  this.buttonPlayStop,
																		  this.labelNowPlaying});
			this.ForeColor = System.Drawing.Color.LimeGreen;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Location = new System.Drawing.Point(400, 0);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MediaBar";
			this.Opacity = 0.699999988079071;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Ultimate Music Media Bar";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MediaBar_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MediaBar_KeyUp);
			this.ResumeLayout(false);

		}
		#endregion

		private void labelNowPlaying_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			startX = e.X;
			startY = e.Y;
			moving = true;		
		}

		private void labelNowPlaying_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (moving) 
			{
				this.Left = this.Left + e.X - startX;
				this.Top  = this.Top  + e.Y - startY;
			}
		}

		private void labelNowPlaying_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			moving = false;
		}

		private void MediaBar_Load(object sender, System.EventArgs e)
		{
			this.Width = 325;
			this.Height = 16;

			Graphics gr = this.CreateGraphics();
			SolidBrush brush = new SolidBrush(Color.FromArgb(30, 30, 30));
			Pen pen = new Pen(brush, 2);
			gr.DrawLine(pen, new Point(0, 0), new Point(this.Width, 0));
		}

		private void timerPaused_Tick(object sender, System.EventArgs e)
		{
			labelNowPlaying.Visible = !labelNowPlaying.Visible;
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void buttonPlayStop_Click(object sender, System.EventArgs e)
		{
			umPlayer.PlayStop();
		}

		private void buttonPause_Click(object sender, System.EventArgs e)
		{
			umPlayer.Pause();
		}

		private void buttonPrevious_Click(object sender, System.EventArgs e)
		{
			umPlayer.Previous();			
		}

		private void MediaBar_Paint(object sender, PaintEventArgs pe)
		{
		}

		private void AdvanceToSong(object sender, System.EventArgs e)
		{
            MediaListItemMenuItem item = (MediaListItemMenuItem) sender;

			umPlayer.client.mediaServer.AdvanceToSong(item.MediaRow.MediaId);
		}

		private void MediaBar_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			umPlayer.UMPlayer_KeyUp(sender, e);
		}

		private void buttonNext_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				umPlayer.Next();
			}
			else if (e.Button == MouseButtons.Right)
			{
				contextMenuQuickList.MenuItems.Clear();

				// get the queue from the server
				umPlayer.FillMenuFromQueue(contextMenuQuickList);

				foreach (MenuItem item in contextMenuQuickList.MenuItems)
				{
					item.Click += new EventHandler(AdvanceToSong);
				}

				contextMenuQuickList.Show(buttonNext, this.PointToClient(MousePosition));
			}
		}

	}
}
