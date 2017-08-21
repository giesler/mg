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
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Category> recentCategories = PicHttpContext.Current.CategoryManager.RecentCategorires();
            this.recent.Controls.Add(Categories.CreateCategoryTable(recentCategories));

            var random = PicHttpContext.Current.PictureManager.GetRandomPicture();
            this.randomTitle.Text = random.Title;

            string navigateUrl = string.Format("Picture.aspx?p={0}", random.Id);
            this.randomTitle.NavigateUrl = navigateUrl;
            this.randomImageLink.NavigateUrl = navigateUrl;

            PictureImageControl image = new PictureImageControl();
            image.SetPictureById(random.Id, 125, 125);
            image.SetMaxPicSize(50);
            this.randomImageLink.Controls.Clear();
            this.randomImageLink.Controls.Add(image);
        }
    }
}
