using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for TitleBarButtons.
	/// </summary>
	public class TitleBarButtons : System.Windows.Forms.UserControl
	{
		#region Declares

		public System.Windows.Forms.Button buttonPrevious;
		public System.Windows.Forms.Button buttonNext;
		public System.Windows.Forms.Button buttonStop;
		public System.Windows.Forms.Button buttonPlayPause;
		private System.ComponentModel.Container components = null;
		private UMPlayer umPlayer = null;

		#endregion

		#region Constructors

		public TitleBarButtons(UMPlayer umPlayer)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.umPlayer = umPlayer;
		}

		#endregion

		#region Disposal

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

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TitleBarButtons));
			this.buttonPrevious = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonPlayPause = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.BackColor = System.Drawing.Color.Transparent;
			this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPrevious.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonPrevious.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(24, 16);
			this.buttonPrevious.TabIndex = 9;
			this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
			this.buttonPrevious.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonPrevious_Paint);
			// 
			// buttonNext
			// 
			this.buttonNext.BackColor = System.Drawing.Color.Transparent;
			this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNext.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonNext.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonNext.Location = new System.Drawing.Point(72, 0);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(24, 16);
			this.buttonNext.TabIndex = 8;
			this.buttonNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonNext_MouseUp);
			this.buttonNext.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonNext_Paint);
			// 
			// buttonStop
			// 
			this.buttonStop.BackColor = System.Drawing.Color.Transparent;
			this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonStop.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonStop.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonStop.Location = new System.Drawing.Point(48, 0);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(24, 16);
			this.buttonStop.TabIndex = 7;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			this.buttonStop.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonStop_Paint);
			// 
			// buttonPlayPause
			// 
			this.buttonPlayPause.BackColor = System.Drawing.Color.Transparent;
			this.buttonPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlayPause.Font = new System.Drawing.Font("Tahoma", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonPlayPause.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonPlayPause.Location = new System.Drawing.Point(24, 0);
			this.buttonPlayPause.Name = "buttonPlayPause";
			this.buttonPlayPause.Size = new System.Drawing.Size(24, 16);
			this.buttonPlayPause.TabIndex = 6;
			this.buttonPlayPause.Click += new System.EventHandler(this.buttonPlayPause_Click);
			this.buttonPlayPause.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonPlayPause_Paint);
			// 
			// TitleBarButtons
			// 
			this.BackColor = System.Drawing.Color.Gray;
			this.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("$this.BackgroundImage")));
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonPrevious,
																		  this.buttonNext,
																		  this.buttonStop,
																		  this.buttonPlayPause});
			this.Name = "TitleBarButtons";
			this.Size = new System.Drawing.Size(96, 16);
			this.ResumeLayout(false);

		}
		#endregion

		#region Paint functions

		private void buttonPrevious_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= 6;
			int height  = 6;
			int x		= buttonPlayPause.Width / 2 - (width / 2);
			int y		= buttonPlayPause.Height / 2 - (height / 2);
			int offset	= 5;

			// draw play
			Point[] points = new Point[4];

			points[0] = new Point(x + width + offset/2, y);						// upper left
			points[1] = new Point(x + width + offset/2, y + height);	// center right
			points[2] = new Point(x + offset/2, y + height / 2);				// lower left
			points[3] = new Point(x + width + offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.LawnGreen), points, System.Drawing.Drawing2D.FillMode.Alternate);

			points[0] = new Point(x + width - offset/2, y);						// upper left
			points[1] = new Point(x + width - offset/2, y + height);	// center right
			points[2] = new Point(x - offset/2, y + height / 2);				// lower left
			points[3] = new Point(x + width - offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.LawnGreen), points, System.Drawing.Drawing2D.FillMode.Alternate);
		}

		private void buttonPlayPause_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= 6;
			int height  = 6;
			int x		= buttonPlayPause.Width / 2 - (width / 2);
			int y		= buttonPlayPause.Height / 2 - (height / 2);
			int offset	= 2;

			if (umPlayer.client.mediaServer.CurrentPlayState == msn2.net.QueuePlayer.Server.MediaServer.PlayState.Playing)
			{
				// draw pause
				Point pt1 = new Point(x + width / 2 - offset, y);
				Point pt2 = new Point(x + width / 2 - offset, y + height);

				Point pt3 = new Point(x + width / 2 + offset, y);
				Point pt4 = new Point(x + width / 2 + offset, y + height);

				using (Pen pen = new Pen(new SolidBrush(Color.LawnGreen), 2))
				{
					e.Graphics.DrawLine(pen, pt1, pt2);
					e.Graphics.DrawLine(pen, pt3, pt4);
				}
			}
			else
			{
				// draw play
				Point[] points = new Point[4];

				points[0] = new Point(x, y);						// upper left
				points[1] = new Point(x + width, y + height /2);	// center right
				points[2] = new Point(x, y + height);				// lower left
				points[3] = new Point(x, y);						// upper left again

				e.Graphics.FillPolygon(new SolidBrush(Color.LawnGreen), points, System.Drawing.Drawing2D.FillMode.Alternate);
			}

		}

		private void buttonStop_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= 6;
			int x		= buttonStop.Width / 2 - (width / 2);
			int y		= buttonStop.Height / 2 - (width / 2);

			e.Graphics.FillRectangle(new SolidBrush(Color.LawnGreen), x, y, width, width);
		}

		private void buttonNext_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= 6;
			int height  = 6;
			int x		= buttonPlayPause.Width / 2 - (width / 2);
			int y		= buttonPlayPause.Height / 2 - (height / 2);
			int offset	= 5;

			// draw play
			Point[] points = new Point[4];

			points[0] = new Point(x - offset/2, y);						// upper left
			points[1] = new Point(x + width - offset/2, y + height /2);	// center right
			points[2] = new Point(x - offset/2, y + height);				// lower left
			points[3] = new Point(x - offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.LawnGreen), points, System.Drawing.Drawing2D.FillMode.Alternate);

			points[0] = new Point(x + offset/2, y);						// upper left
			points[1] = new Point(x + width + offset/2, y + height /2);	// center right
			points[2] = new Point(x + offset/2, y + height);				// lower left
			points[3] = new Point(x + offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.LawnGreen), points, System.Drawing.Drawing2D.FillMode.Alternate);
		}

		#endregion

		#region Events

		private void buttonPrevious_Click(object sender, System.EventArgs e)
		{
			umPlayer.Previous();
		}

		private void buttonPlayPause_Click(object sender, System.EventArgs e)
		{
			if (umPlayer.client.mediaServer.CurrentPlayState == msn2.net.QueuePlayer.Server.MediaServer.PlayState.Paused)
			{
				umPlayer.Play();
			}
			else
			{
				umPlayer.Pause();
			}
		}

		private void buttonStop_Click(object sender, System.EventArgs e)
		{
            umPlayer.Stop();		
		}

		private void buttonNext_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				umPlayer.SetWaitForMessage();
				umPlayer.client.mediaServer.Next();
			}
			else if (e.Button == MouseButtons.Right)
			{
				ContextMenu contextMenuQuickList = new ContextMenu();

				// get the queue from the server
				umPlayer.FillMenuFromQueue(contextMenuQuickList, true);

				foreach (MenuItem item in contextMenuQuickList.MenuItems)
				{
					item.Click += new EventHandler(umPlayer.AdvanceToSong);
				}

				contextMenuQuickList.Show(buttonNext, buttonNext.PointToClient(MousePosition));
			}

		}

		#endregion

	}
}
