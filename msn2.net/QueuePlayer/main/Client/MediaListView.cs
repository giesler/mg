using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.QueuePlayer.Shared;
using System.Diagnostics;
using msn2.net.Common;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for MediaListView.
	/// </summary>
	public class MediaListView : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.ColumnHeader columnHeader16;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Panel panelBase;
		public System.Windows.Forms.ListView lv;

		private decimal totalDuration = 0;
		private System.Windows.Forms.Label labelTotalDuration;
		private System.Windows.Forms.Label labelSongCount;

		private bool showDeleteColumn = false;
		private System.Windows.Forms.ColumnHeader columnHeaderDel;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MediaListView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			UpdateTotals();

			// THE NEXT LINES OF CODE ARE FROM MAGIC LIBRARY 
			// Calculate the IDE background colour as only half as dark as the control colour
			int red = 255 - ((255 - SystemColors.Control.R) / 3);
			int green = 255 - ((255 - SystemColors.Control.G) / 3);
			int blue = 255 - ((255 - SystemColors.Control.B) / 3);
			lv.BackColor = Color.FromArgb(red, green, blue);

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lv = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.panelBase = new System.Windows.Forms.Panel();
			this.labelTotalDuration = new System.Windows.Forms.Label();
			this.labelSongCount = new System.Windows.Forms.Label();
			this.columnHeaderDel = new System.Windows.Forms.ColumnHeader();
			this.panelBase.SuspendLayout();
			this.SuspendLayout();
			// 
			// lv
			// 
			this.lv.AllowDrop = true;
			this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				 this.columnHeader1,
																				 this.columnHeader2,
																				 this.columnHeader15,
																				 this.columnHeader16,
																				 this.columnHeader3,
																				 this.columnHeaderDel});
			this.lv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lv.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lv.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lv.FullRowSelect = true;
			this.lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lv.HideSelection = false;
			this.lv.Name = "lv";
			this.lv.Size = new System.Drawing.Size(288, 256);
			this.lv.TabIndex = 6;
			this.lv.View = System.Windows.Forms.View.Details;
			this.lv.Resize += new System.EventHandler(this.lv_Resize);
			this.lv.DoubleClick += new System.EventHandler(this.lv_DoubleClick);
			this.lv.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lv_MouseUp);
			this.lv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp);
			this.lv.DragDrop += new System.Windows.Forms.DragEventHandler(this.lv_DragDrop);
			this.lv.DragEnter += new System.Windows.Forms.DragEventHandler(this.lv_DragEnter);
			this.lv.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lv_ItemDrag);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 150;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Artist";
			this.columnHeader2.Width = 150;
			// 
			// columnHeader15
			// 
			this.columnHeader15.Text = "Album";
			// 
			// columnHeader16
			// 
			this.columnHeader16.Text = "Track";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Duration";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader3.Width = 100;
			// 
			// panelBase
			// 
			this.panelBase.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.labelTotalDuration,
																					this.labelSongCount});
			this.panelBase.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBase.Location = new System.Drawing.Point(0, 256);
			this.panelBase.Name = "panelBase";
			this.panelBase.Size = new System.Drawing.Size(288, 16);
			this.panelBase.TabIndex = 7;
			// 
			// labelTotalDuration
			// 
			this.labelTotalDuration.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.labelTotalDuration.Location = new System.Drawing.Point(200, 0);
			this.labelTotalDuration.Name = "labelTotalDuration";
			this.labelTotalDuration.Size = new System.Drawing.Size(88, 16);
			this.labelTotalDuration.TabIndex = 1;
			this.labelTotalDuration.Text = "h:mm:ss";
			this.labelTotalDuration.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelSongCount
			// 
			this.labelSongCount.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelSongCount.Name = "labelSongCount";
			this.labelSongCount.Size = new System.Drawing.Size(192, 16);
			this.labelSongCount.TabIndex = 0;
			this.labelSongCount.Text = "# songs";
			// 
			// columnHeaderDel
			// 
			this.columnHeaderDel.Text = "Del";
			this.columnHeaderDel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// MediaListView
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lv,
																		  this.panelBase});
			this.Name = "MediaListView";
			this.Size = new System.Drawing.Size(288, 272);
			this.Resize += new System.EventHandler(this.MediaListView_Resize);
			this.panelBase.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void lv_Resize(object sender, System.EventArgs e)
		{
			double []arSizes = {0, 0, 0, 0, 0, 0};

			if (lv.Width < 250)
			{
				arSizes[0] = 0.5;
				arSizes[1] = 0.5;
			}
			else if (lv.Width < 400)
			{
				if (showDeleteColumn)
				{
					arSizes[0] = 0.35;
					arSizes[1] = 0.35;
					arSizes[4] = 0.2;
					arSizes[5] = 0.1;
				}
				else
				{
					arSizes[0] = 0.4;
					arSizes[1] = 0.4;
					arSizes[4] = 0.2;
				}
			}
			else
			{
				if (showDeleteColumn)
				{
					arSizes[0] = 0.30;
					arSizes[1] = 0.22;
					arSizes[2] = 0.22;
					arSizes[3] = 0.09;
					arSizes[4] = 0.10;
					arSizes[5] = 0.05;
				}
				else
				{
					arSizes[0] = 0.30;
					arSizes[1] = 0.25;
					arSizes[2] = 0.25;
					arSizes[3] = 0.10;
					arSizes[4] = 0.10;
				}
			}

			for (int i = 0; i < 6; i++)
			{
				lv.Columns[i].Width = (int)((lv.Width-25) * arSizes[i]);
			}

		}

		private void lv_DoubleClick(object sender, System.EventArgs e)
		{
			if (MediaDoubleClick != null)
				MediaDoubleClick(sender, e);
		}

		private void lv_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (MediaKeyUp != null)
				MediaKeyUp(sender, e);

		}

		public new event EventHandler MediaDoubleClick;
		public new event KeyEventHandler MediaKeyUp;

		public void AddItem(MediaListViewItem item)
		{
			lv.Items.Add(item);
			totalDuration += item.Entry.Duration;

			UpdateTotals();
		}

		public void InsertItem(MediaListViewItem item, int position)
		{
			lv.Items.Insert(position, item);
			totalDuration += item.Entry.Duration;

			UpdateTotals();
		}

		public void Clear()
		{
			lv.Items.Clear();
			totalDuration = 0;

			UpdateTotals();
		}

		public void RemoveItem(Guid guid)
		{
			foreach (MediaListViewItem item in new IterIsolate(lv.Items))
			{
				if (item.Guid == guid)
				{
					lv.Items.Remove(item);
					totalDuration -= item.Entry.Duration;
					UpdateTotals();
					break;
				}
			}
		}

		private void UpdateTotals()
		{
            labelSongCount.Text		= String.Format("{0} songs", lv.Items.Count);
			labelTotalDuration.Text = Utilities.DurationToString(totalDuration);
		}

		private void MediaListView_Resize(object sender, System.EventArgs e)
		{
			if (this.Height < 300)
			{
				panelBase.Visible = false;
				lv.HeaderStyle = ColumnHeaderStyle.None;
			}
			else
			{
				panelBase.Visible = true;
				lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			}
		}

		private void lv_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			if (lv.SelectedItems.Count > 0)
			{
				MediaListViewItemCollection col = new MediaListViewItemCollection();
				foreach (MediaListViewItem item in lv.SelectedItems)
				{
					col.Add(item);
				}
				lv.DoDragDrop(col, DragDropEffects.All);            			
			}
		}

		private void lv_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent("msn2.net.QueuePlayer.Client.MediaListViewItemCollection", false))
			{
				if (MediaItemDroppedEvent != null)
				{
					Point currentPoint = lv.PointToClient(new Point(e.X, e.Y));
					MediaListViewItem targetItem = (MediaListViewItem) lv.GetItemAt(currentPoint.X, currentPoint.Y);
					MediaListViewItemCollection sourceItems = 
						(MediaListViewItemCollection)
						e.Data.GetData("msn2.net.QueuePlayer.Client.MediaListViewItemCollection", false);

					MediaItemDroppedEvent(this, new MediaItemDropEventArgs(sourceItems, targetItem));
				}
				
			}
		}

		private void lv_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent("msn2.net.QueuePlayer.Client.MediaListViewItemCollection", false))
			{
				e.Effect = DragDropEffects.Copy;
			}            
		}

		public event MediaItemDelete MediaItemDeleteEvent;

		private void lv_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (showDeleteColumn && e.Button == MouseButtons.Left && lv.SelectedItems.Count > 0 
				&& MediaItemDeleteEvent != null)
			{
				int sum = 0;

				foreach (ColumnHeader c in lv.Columns)
				{
					if (c != columnHeaderDel)
						sum += c.Width;
				}
                
				// check if user clicked before the del column
                if (e.X < sum)
					return;

				// check if user clicked after the del column
                if (e.X > sum + columnHeaderDel.Width)
					return;

				// We are in the right column
                ListViewItem item = lv.GetItemAt(e.X, e.Y);
				if (item != null)
				{
					MediaListViewItem mediaItem = (MediaListViewItem) item;
                    if (MediaItemDeleteEvent != null)
						MediaItemDeleteEvent(this, new MediaItemDeleteEventArgs(mediaItem));
				}
			}

		}

		public event MediaItemDropped MediaItemDroppedEvent;

		public System.Windows.Forms.ListView.ListViewItemCollection Items
		{
			get { return lv.Items; }
		}

		public System.Windows.Forms.ListView.SelectedListViewItemCollection SelectedItems
		{
			get 
			{
				return lv.SelectedItems;
			}
		}

		[Category("Appearance")]
		public bool ShowDeleteColumn
		{
			get
			{
				return showDeleteColumn;
			}
			set
			{
				showDeleteColumn = value;
			}
		}
	}

	public delegate void MediaItemDropped(object sender, MediaItemDropEventArgs e);
	public delegate void MediaItemDelete(object sender, MediaItemDeleteEventArgs e);

	public class MediaItemDeleteEventArgs: EventArgs
	{
		private MediaListViewItem item;

		public MediaItemDeleteEventArgs(MediaListViewItem item)
		{
			this.item = item;
		}

		public MediaListViewItem Item
		{
			get
			{
				return item;
			}
		}
	}

	public class MediaItemDropEventArgs: EventArgs
	{
		private MediaListViewItemCollection sourceItems;
		private MediaListViewItem targetItem;

		public MediaItemDropEventArgs(MediaListViewItemCollection sourceItems,
			MediaListViewItem targetItem)
		{
			this.sourceItems	= sourceItems;
			this.targetItem		= targetItem;
		}

		public MediaListViewItemCollection SourceItems
		{
			get { return sourceItems; }
		}

		public MediaListViewItem TargetItem
		{
			get { return targetItem; }
		}
	}

	public class MediaListViewItemCollection: System.Collections.CollectionBase
	{
		public int Add(MediaListViewItem item)
		{
			return InnerList.Add(item);
		}

	}
}
