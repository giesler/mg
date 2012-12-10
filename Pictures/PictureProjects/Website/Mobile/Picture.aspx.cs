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

namespace pics.Controls.Mobile
{
    public partial class PicturePage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {            
            base.OnInit(e);

            if (Request.Cookies["picSizePref"] != null)
            {
                this.size.SelectedIndex = int.Parse(Request.Cookies["picSizePref"].Value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                int pictureId = int.Parse(Request.QueryString["p"]);

                if (Request.QueryString["c"] != null)
                {
                    this.categoryLink.NavigateUrl = "Pictures.aspx?c=" + Request.QueryString["c"];
                    this.nextRandom.Visible = false;
                }
                else
                {
                    this.categoryLink.Visible = false;
                }

                LoadPicture(pictureId);
            }
        }

        private void LoadPicture(int pictureId)
        {
            Picture picture = PicContext.Current.PictureManager.GetPicture(pictureId);
            this.Header.Title = picture.Title;

            int maxSize = 200;

            if (size.SelectedIndex == 1)
            {
                maxSize = 450;
            }
            else if (size.SelectedIndex == 2)
            {
                maxSize = 700;
            }

            PictureImageControl image = new PictureImageControl();
            image.SetPictureById(picture.Id, 700, 750);
            image.SetMaxPicSize(maxSize);
            this.content.Controls.Add(image);
        }

        protected void nextRandom_Click(object sender, EventArgs e)
        {
            Picture picture = PicContext.Current.PictureManager.GetRandomPicture();
            Response.Redirect("Picture.aspx?p=" + picture.Id.ToString());
        }

        protected void size_SelectedIndexChanged(object sender, EventArgs e)
        {
            HttpCookie cookie = new HttpCookie("picSizePref", this.size.SelectedIndex.ToString());
            cookie.Expires = DateTime.Now.AddDays(90);
            Response.Cookies.Add(cookie);

            this.LoadPicture(int.Parse(Request.QueryString["p"]));
        }
    }
}
