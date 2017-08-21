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
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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
            string path = @"\";
            if (Request["p"] != null)
            {
                path = Request["p"];
            }
            float scale = 1.0F;
            if (Request["s"] != null)
            {
                scale = float.Parse(Request["s"]);
            }
            string bgColor = "#5D646D";
            if (Request["b"] != null)
            {
                bgColor = Request["b"];
            }
            if (Request["d"] != null)
            {
                this.details.Visible = Request["d"] != "0";
            }
            int maxWidthFixed = 0;
            if (Request["mwf"] != null)
            {
                maxWidthFixed = int.Parse(Request["mwf"]);
            }
            int maxHeightFixed = 0;
            if (Request["mhf"] != null)
            {
                maxHeightFixed = int.Parse(Request["mhf"]);
            }

            this.bodyTag.Style.Add("background-color", bgColor);

            int heightOffset = this.details.Visible ? 50 : 5;
            this.picPanel.Height = (int)(scale * maxHeight) + heightOffset;
            this.picPanel.Width = (int)(scale * maxWidth) + 5;

            if (maxHeightFixed > 0 && this.picPanel.Height.Value > maxHeightFixed)
            {
                this.picPanel.Height = maxHeightFixed;
            }
            if (maxWidthFixed > 0 && this.picPanel.Width.Value > maxWidthFixed)
            {
                this.picPanel.Width = maxWidthFixed;
            }

            PicContext context = PicContext.Current;
            Picture random = context.PictureManager.GetRandomPicture(maxWidth, maxHeight, path, 0);

            if (random != null)
            {
                PictureCache pc = context.PictureManager.GetPictureCache(random.Id, maxWidth, maxHeight);

                string categories = string.Empty;
                foreach (Category cat in context.PictureManager.GetPictureCategories(random.Id))
                {
                    if (categories.Length > 0)
                    {
                        categories += ", ";
                    }
                    categories += cat.Name;
                }

                string url = string.Format("getpic.axd?p={0}&mw={1}&mh={2}", random.Id, maxWidth, maxHeight);
                string imageInfo = string.Format("{0}{1}{2}{1}{3}", random.Title, Environment.NewLine, random.PictureDate.ToShortDateString(), categories);

                this.image.ImageUrl = url;
                this.image.AlternateText = imageInfo;
                                
                this.image.Height = Unit.Pixel((int) (scale * maxHeight));
                this.image.Width = Unit.Pixel((int) (scale * maxWidth));

                if (pc.Width > pc.Height)
                {
                    this.image.Height = Unit.Pixel((int)(scale * pc.Height));
                }

                if (maxHeightFixed  > 0 && maxHeightFixed > this.image.Height.Value)
                {
                    this.image.Height = maxHeightFixed;
                    this.image.Width = Unit.Empty;
                }
                else if (maxWidthFixed > 0 && maxWidthFixed > this.image.Width.Value)
                {
                    this.image.Width = maxWidthFixed;
                    this.image.Height = Unit.Empty;
                }

                this.imageLink.Target = "_new";
                this.imageLink.NavigateUrl = string.Format("http://pics.msn2.net/picview.aspx?p={0}&type=random", random.Id);

                string title = random.Title;
                if (title.Length > 15)
                {
                    title = title.Substring(0, 14) + "...";
                    this.titleLabel.ToolTip = random.Title;
                }
                this.titleLabel.Text = title;

                this.dateLabel.Text = random.PictureDate.ToShortDateString();

                if (categories.Length > 15)
                {
                    this.categories.ToolTip = categories;
                    categories = categories.Substring(0, 14);
                }
                this.categories.Text = categories;
            }
            else
            {
                this.titleLabel.Text = "No picture";
            }
        }

        protected void next_Click(object sender, EventArgs e)
        {
            // Gets another pic
        }
    }
}
