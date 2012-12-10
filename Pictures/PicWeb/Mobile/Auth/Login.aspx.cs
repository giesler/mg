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

namespace pics.Controls.Mobile.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["picLogonEmail"] != null)
            {
                this.email.Text = Request.Cookies["picLogonEmail"].Value;
            }
        }

        protected void login_Click(object sender, EventArgs e)
        {
            string pwd = UserManager.GetEncryptedPassword(this.password.Text);

            bool valid = false;
            PersonInfo info = PicContext.Current.UserManager.Login(this.email.Text, pwd, ref valid);

            if (info != null)
            {
                FormsAuthentication.SetAuthCookie(info.Id.ToString(), chkSave.Checked);
                Response.Redirect(Request.ApplicationPath + "mobile/default.aspx");

                HttpCookie cookie = new HttpCookie("picLogonEmail", this.email.Text);
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.SetCookie(cookie);
            }
            else
            {
                this.error.Visible = true;
            }
        }
    }
}
