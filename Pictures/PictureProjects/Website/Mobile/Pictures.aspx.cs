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
        protected void Page_Load(object sender, EventArgs e)
        {
            int categoryId = int.Parse(Request.QueryString["c"]);

            Category category = PicContext.Current.CategoryManager.GetCategory(categoryId);
            this.Page.Title = category.Name;
            this.categoryHeading.Text = category.Name;
            this.categoryDescription.Text = category.Description;

            List<Picture> pictures = PicContext.Current.PictureManager.GetPicturesByCategory(categoryId);

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
    }
}
