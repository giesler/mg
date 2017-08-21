using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace chickweb
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetRequestInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} - {1} - {2}", Environment.MachineName, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);

            return sb.ToString();
        }
    }
}
