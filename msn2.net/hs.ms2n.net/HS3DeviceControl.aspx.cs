using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeServices
{
    public partial class HS3DeviceControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id");
            }

            string by = Request.QueryString["by"];
            if (string.IsNullOrEmpty(by))
            {
                throw new ArgumentException("by");
            }

            string v = Request.QueryString["v"];
            if (string.IsNullOrEmpty(v))
            {
                throw new ArgumentNullException("v");
            }

            string url = string.Format("http://192.168.1.210:8888/JSON?request=controldeviceby{0}&ref={1}&{0}={2}", by, id, v);

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            var response = request.GetResponse();

            HttpContext.Current.Response.Write(response.ToString());

        }
    }
}