using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

namespace pics.Controls.Mobile
{
    public class PictureImageControl: Image
    {
        internal void SetPictureById(int pictureId, int maxWidth, int maxHeight)
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            this.ImageUrl = string.Format("{0}GetImage.axd?p={1}&mw={2}&mh={3}&sb=0", appPath, pictureId, maxWidth, maxHeight);            
        }

        internal void SetMaxPicSize(int p)
        {
            
        }

        public string BackgroundColor { get; set; }
    }
}
