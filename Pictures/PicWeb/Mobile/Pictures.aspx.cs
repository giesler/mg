using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using msn2.net.Pictures;
using System.Collections.Generic;

namespace pics.Controls.Mobile
{
    public partial class Pictures : System.Web.UI.Page
    {
        int categoryId = 0;
        DateTime startTime = DateTime.MinValue;
        DateTime endTime = DateTime.MaxValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                this.pageCount.Items.Add(new ListItem("10", "10"));
                this.pageCount.Items.Add(new ListItem("20", "20"));
                this.pageCount.Items.Add(new ListItem("50", "50"));
                this.pageCount.Items.Add(new ListItem("100", "100"));
                this.pageCount.SelectedIndex = 1;
            }

            if (Request.QueryString["c"] != null && Request.QueryString["from"] == null)
            {
                this.categoryId = int.Parse(Request.QueryString["c"]);

                Category category = PicContext.Current.CategoryManager.GetCategory(categoryId);
                this.Page.Title = category.Name;
                this.categoryHeading.Text = category.Name;
                this.categoryDescription.Text = category.Description;
            }
            else
            {
                this.categoryHeading.Text = "Search Results";

                if (Request.QueryString["c"] != null)
                {
                    this.categoryId = int.Parse(Request.QueryString["c"]);
                }

                this.startTime = DateTime.Parse(Request.QueryString["from"]).Date;
                this.endTime = DateTime.Parse(Request.QueryString["to"]).Date;
                this.endTime = this.endTime.AddDays(1).AddSeconds(-1);
                                
                this.categoryDescription.Text = this.startTime.Date.ToShortDateString() + " - " + this.endTime.Date.ToShortDateString();

                if (categoryId > 0)
                {
                    Category category = PicContext.Current.CategoryManager.GetCategory(categoryId);
                    this.categoryDescription.Text += ", " + category.Name;
                }
            }

            if (this.IsPostBack == false)
            {
                LoadPictures(1);
            }
        }

        void LoadPictures(int page)
        {
            List<Picture> pictures = null;

            if (this.categoryId > 0)
            {
                pictures = PicContext.Current.PictureManager.GetPictures(categoryId, this.startTime, this.endTime);
            }
            else
            {
                pictures = PicContext.Current.PictureManager.GetPicturesByDate(this.startTime, this.endTime);
            }

            this.pictureCount.Text = pictures.Count.ToString();
            int pageCount = int.Parse(this.pageCount.SelectedValue);
            if (pictures.Count > pageCount)
            {
                this.pager.Visible = true;

                int totalPages = pictures.Count / pageCount;
                if (pictures.Count % pageCount != 0)
                {
                    totalPages++;
                }

                this.page.Items.Clear();
                this.pagesCount.Text = totalPages.ToString();
                for (int i = 1; i <= totalPages; i++)
                {
                    ListItem item = new ListItem(i.ToString());
                    this.page.Items.Add(item);
                    if (i == page)
                    {
                        item.Selected = true;
                    }
                }

                int skipCount = (page - 1) * 20;
                pictures = pictures.Skip(skipCount).Take(pageCount).ToList();
            }
            else
            {
                this.pager.Visible = false;
            }

            this.content.Controls.Clear();

            Table t = new Table();
            t.CellPadding = 0;
            t.CellSpacing = 0;
            t.Width = Unit.Percentage(100);
            this.content.Controls.Add(t);

            int index = 0;
            TableRow tr = null;

            foreach (Picture picture in pictures)
            {
                if (index % 2 == 0)
                {
                    tr = new TableRow();
                    t.Rows.Add(tr);
                }

                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Center;
                tc.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(tc);

                string zoomUrl = string.Format("Picture.aspx?p={0}&c={1}", picture.Id, categoryId);
                HyperLink lnk = new HyperLink { NavigateUrl = zoomUrl };
                tc.Controls.Add(lnk);

                PictureCache pc = PicContext.Current.PictureManager.GetPictureCache(picture.Id, 125, 125);
                
                PictureImageControl curPic = new PictureImageControl { BackgroundColor = "White" };
                curPic.SetPictureById(picture.Id, 125, 125);
                if (pc != null)
                {
                    curPic.Height = (int) (pc.Height.Value * 0.82F);
                    curPic.Width = (int) (pc.Width.Value * 0.82F);
                }
                lnk.Controls.Add(curPic);

                index++;
            }

            if (pictures.Count == 0)
            {
                tr = new TableRow();
                t.Rows.Add(tr);

                TableCell tc = new TableCell();
                tr.Cells.Add(tc);

                tc.Controls.Add(new HtmlLiteral("No pictures."));
            }

        }

        protected void OnPageChanged(object sender, EventArgs e)
        {
            int pageNumber = int.Parse(this.page.SelectedItem.Text);
            LoadPictures(pageNumber);
        }
    }
}
