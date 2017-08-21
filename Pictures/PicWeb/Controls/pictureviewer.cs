using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using System.Text;
using msn2.net.Pictures;

namespace pics.Controls
{
	/// <summary>
	/// Summary description for PictureViewer.
	/// </summary>
	public class PictureViewer: WebControl, INamingContainer
	{
		private int pictureId;
		private string caption;
		private string zoomUrl;
		private int recordNumber;
		private bool showCheckBox;
		private bool showRecordNumber;
		private bool showFrame;

		public PictureViewer(int pictureId, string zoomUrl, string caption, int recordNumber, bool showCheckBox, bool showRecordNumber, bool showFrame)
		{
			this.pictureId			= pictureId;
			this.zoomUrl			= zoomUrl;
			this.caption			= caption;
			this.recordNumber		= recordNumber;
			this.showCheckBox		= showCheckBox;
			this.showRecordNumber	= showRecordNumber;
			this.showFrame			= showFrame;
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls ();

			if (showFrame)
			{
				CreateFramedViewer();
			}
			else
			{
				CreateFramelessViewer();
			}
		}

		private void CreateFramedViewer()
		{
			// Create table to contain thumb
			Table t = new Table();
			t.CellPadding = 3;
			t.CellSpacing = 0;
			this.Controls.Add(t);

			// Only row in the table
			TableRow tr = new TableRow();
			t.Rows.Add(tr);

			// Add cell with picture
			TableCell tcPic = new TableCell();
			tr.Cells.Add(tcPic);

			// Add a container table for the picture
			Table tPic				= new Table();
			tPic.Height				= 145;
			tPic.Width				= 145;
			tPic.CellPadding		= 5;
			tPic.CellSpacing		= 0;
			tPic.CssClass			= "pictureFrame";
			tPic.HorizontalAlign	= HorizontalAlign.Center;
			tcPic.Controls.Add(tPic);

			tcPic.Style.Add("filter", "progid:DXImageTransform.Microsoft.Shadow(color='#666666', Direction=135, Strength=8)");

			// container table row
			TableRow trtPic = new TableRow();
			tPic.Controls.Add(trtPic);

			// container table cell
			TableCell tctPic = new TableCell();
			tctPic.ColumnSpan		= 2;
			tctPic.HorizontalAlign	= HorizontalAlign.Center;
			tctPic.VerticalAlign	= VerticalAlign.Middle;
			tctPic.Height			= 145;
			tctPic.Width			= 145;
			tctPic.CssClass			= "picTopBack";
			tctPic.Style.Add("POSITION", "relative");
			trtPic.Controls.Add(tctPic);

			if (showCheckBox)
			{
				Panel panel = new Panel();
				panel.CssClass	= "checkboxPanel";
				tctPic.Controls.Add(panel);

				PictureCheckBox checkBox		= new PictureCheckBox(pictureId);
				checkBox.AutoPostBack			= true;
				checkBox.CheckedChanged			+= new EventHandler(checkBox_Clicked);
				panel.Controls.Add(checkBox);

				// See if we need to set the checkbox state
				PictureIdCollection mySelectedList	= Global.SelectedPictures;
				if (mySelectedList.Contains(pictureId))
				{
					checkBox.Checked			= true;
				}
			
			}

			// Add link to cell for clicking on picture
			HyperLink lnkPicZoomPic	= new HyperLink();
			lnkPicZoomPic.NavigateUrl	= zoomUrl;
			tctPic.Controls.Add(lnkPicZoomPic);

			// Add picture to cell
			Picture curPic = new Picture();
			curPic.SetPictureById(pictureId, 125, 125);
			//				curPic.Filename = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "FileName").ToString();
			//				curPic.Height	= Convert.ToInt32(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Height"));
			//				curPic.Width	= Convert.ToInt32(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Width"));
			lnkPicZoomPic.Controls.Add(curPic);

			TableRow noteRow = new TableRow();
			tPic.Rows.Add(noteRow);

			TableCell noteCell = new TableCell();
			noteCell.CssClass		= "picNoteCell";
			noteRow.Cells.Add(noteCell);

			HyperLink lnkPicZoom	= new HyperLink();
			lnkPicZoom.NavigateUrl	= zoomUrl;
			lnkPicZoom.CssClass		= "whitenote";
			lnkPicZoom.Text			= caption;
			if (lnkPicZoom.Text.Length > 15)
				lnkPicZoom.Text = lnkPicZoom.Text.Substring(0, 13) + "...";
			noteCell.Controls.Add(lnkPicZoom);

			if (lnkPicZoom.Text.Length == 0) 
			{
				Literal lit = new Literal();
				lit.Text = "&nbsp;";
				noteCell.Controls.Add(lit);
			}

			TableCell tcCounter = new TableCell();
			tcCounter.CssClass		= "picNoteCell";
			tcCounter.VerticalAlign = VerticalAlign.Bottom;
			tcCounter.HorizontalAlign = HorizontalAlign.Right;
			noteRow.Cells.Add(tcCounter);

			if (showRecordNumber) 
			{
				HyperLink countLink		= new HyperLink();
				countLink.Text			= recordNumber.ToString();
				countLink.CssClass		= "recnum";
				countLink.NavigateUrl	= zoomUrl;
				tcCounter.Controls.Add(countLink);
			}
		}


		private void CreateFramelessViewer()
		{
			Table t			= new Table();
			t.CellSpacing	= 0;
			t.CellPadding	= 0;
			this.Controls.Add(t);

			TableRow tr		= new TableRow();
			t.Rows.Add(tr);

			if (showCheckBox)
			{
				TableCell checkCell			= new TableCell();
				checkCell.VerticalAlign		= VerticalAlign.Top;
				tr.Cells.Add(checkCell);

				PictureCheckBox checkBox	= new PictureCheckBox(pictureId);
				checkBox.AutoPostBack		= true;
				checkBox.CheckedChanged		+= new EventHandler(checkBox_Clicked);
				checkCell.Controls.Add(checkBox);

				// See if we need to set the checkbox state
				PictureIdCollection mySelectedList	= Global.SelectedPictures;
				if (mySelectedList.Contains(pictureId))
				{
					checkBox.Checked			= true;
				}
			}

			TableCell picCell	= new TableCell();
			tr.Cells.Add(picCell);

			Table tPic			= new Table();
			tPic.CellPadding	= 0;
			tPic.CellSpacing	= 0;
			tPic.CssClass		= "picviewThumbBorder";
            picCell.Controls.Add(tPic);

			TableRow trInner	= new TableRow();
			tPic.Rows.Add(trInner);

			TableCell tc	= new TableCell();
			trInner.Cells.Add(tc);

			// Add link to cell for clicking on picture
			HyperLink lnkPicZoomPic	= new HyperLink();
			lnkPicZoomPic.NavigateUrl	= zoomUrl;
			tc.Controls.Add(lnkPicZoomPic);

			// Add picture to cell
			Picture curPic	= new Picture();
			curPic.SetPictureById(pictureId, 125, 125);
			lnkPicZoomPic.Controls.Add(curPic);

			if (showRecordNumber) 
			{
				TableCell countCell		= new TableCell();
				countCell.VerticalAlign	= VerticalAlign.Bottom;
				tr.Cells.Add(countCell);

				HyperLink countLink		= new HyperLink();
				countLink.Text			= recordNumber.ToString();
				countLink.CssClass		= "recnum";
				countLink.NavigateUrl	= zoomUrl;
				countCell.Controls.Add(countLink);
			}
		}


		private void checkBox_Clicked(object sender, EventArgs e)
		{
			if (PictureCheckBoxClicked != null)
			{
				PictureCheckBox p = (PictureCheckBox) sender;
				PictureCheckBoxClicked(sender, new PictureCheckBoxEventArgs(p.PicId, p.Checked));
			}
		}

		public event PictureCheckBoxClickedEventHandler PictureCheckBoxClicked;

		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write("<div id=\"" + this.UniqueID + "\"");
			if (this.CssClass != null)
			{
				writer.Write(" class=\"" + this.CssClass + "\"");
			}
			writer.Write(">");
			base.Render (writer);
			writer.Write("</div>");
		}

	
	}

	public delegate void PictureCheckBoxClickedEventHandler(object sender, PictureCheckBoxEventArgs e);

	public class PictureCheckBoxEventArgs: EventArgs
	{
		private int picId;
		private bool isChecked;

		public PictureCheckBoxEventArgs(int picId, bool isChecked)
		{
			this.PicId = picId;
			this.isChecked = isChecked;
		}

		public int PicId
		{
			get 
			{
				return picId;
			}
			set
			{
				picId = value;
			}
		}

		public bool Checked
		{
			get
			{
				return isChecked;
			}
		}
	}
}
