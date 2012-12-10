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
    public partial class Categories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int categoryId = int.Parse(Request.QueryString["c"]);
            Category category = PicContext.Current.CategoryManager.GetCategory(categoryId);

            this.categoryHeading.Text = category.Name;
            this.categoryDescription.Text = category.Description;

            List<Category> subCategories = PicContext.Current.CategoryManager.GetCategoriesWithPictures(categoryId);

            this.content.Controls.Add(CreateCategoryTable(subCategories));
        }

        public static Table CreateCategoryTable(List<Category> recentCategories)
        {
            Table t = new Table();

            foreach (Category category in recentCategories)
            {
                TableRow tr = new TableRow();
                t.Rows.Add(tr);

                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tc);

                string navigateUrl = "Pictures.aspx?c=" + category.Id.ToString();

                int subCategories = PicContext.Current.CategoryManager.GetCategoriesWithPictures(category.Id).Count;
                if (subCategories > 0)
                {
                    navigateUrl = "Categories.aspx?c=" + category.Id.ToString();
                }

                if (category.PictureId != 0 && category.PictureId.HasValue)
                {
                    HyperLink imgLink = new HyperLink { NavigateUrl = navigateUrl };
                    tc.Controls.Add(imgLink);

                    PictureImageControl pic = new PictureImageControl();
                    pic.SetPictureById(category.PictureId.Value, 125, 125);
                    pic.SetMaxPicSize(50);
                    pic.BackgroundColor = "White";
                    imgLink.Controls.Add(pic);

                }
                else
                {
                    HyperLink imglnk = new HyperLink { NavigateUrl = navigateUrl };
                    tc.Controls.Add(imglnk);

                    Image image = new Image { Height = 40, Width = 40 };
                    image.ImageUrl = "../Images/folder40.gif";
                    imglnk.Controls.Add(image);
                }

                tc = new TableCell();
                tr.Cells.Add(tc);

                HyperLink lnk = new HyperLink();
                lnk.NavigateUrl = navigateUrl;
                lnk.Text = category.Name;
                tc.Controls.Add(lnk);
            }
            return t;
        }

    }
}
