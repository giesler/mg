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
    public partial class PictureDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int pictureId = int.Parse(Request.QueryString["p"]);

            Picture picture = PicContext.Current.PictureManager.GetPicture(pictureId);

            this.title.Text = picture.Title;
            this.description.Text = picture.Description;
            this.dateTaken.Text = "Taken " + picture.PictureDate.ToShortDateString();

            PictureImageControl img = new PictureImageControl();
            img.SetPictureById(picture.Id, 125, 125);
            img.SetMaxPicSize(50);
            this.content.Controls.Add(img);
        }
    }
}
