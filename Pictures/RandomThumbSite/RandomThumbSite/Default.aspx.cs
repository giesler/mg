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

namespace RandomThumbSite
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PicContext context = PicContext.Current;
            PictureData random = context.PictureManager.GetRandomPicture();

            int maxWidth = 125;
            if (Request["mw"] != null)
            {
                maxWidth = int.Parse(Request["mw"]);
            }
            int maxHeight = 125;
            if (Request["mh"] != null)
            {
                maxHeight = int.Parse(Request["mh"]);
            }

            string categories = string.Empty;
            foreach (CategoryInfo cat in context.PictureManager.GetPictureCategories(random.Id))
            {
                if (categories.Length > 0)
                {
                    categories += ", ";
                }
                categories += cat.Name;
            }

            string url = string.Format("getpic.axd?p={0}&mw={1}&mh={2}", random.Id, maxWidth, maxHeight);
            string imageInfo = string.Format("{0}{1}{2}{1}{3}", random.Title, Environment.NewLine, random.DateTaken.ToShortDateString(), categories);

            this.image.ImageUrl = url;
            this.image.AlternateText = imageInfo;

            this.imageLink.Target = "_new";
            this.imageLink.NavigateUrl = string.Format("http://pics.msn2.net/picview.aspx?p={0}&type=random", random.Id);
        }

        protected void next_Click(object sender, EventArgs e)
        {
            // Gets another pic
        }
    }
}
