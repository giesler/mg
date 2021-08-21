using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp("http://192.168.1.210:8888/JSON?request=getlocations");
            var response = req.GetResponse();
            using (var dataStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(dataStream))
                {
                    string responseString = reader.ReadToEnd();
                    this.location1.Items.Add(responseString);

                    var locations = JsonConvert.DeserializeObject<Locations>(responseString);

                    this.location1.DataSource = locations.location1;
                    this.location1.DataBind();
                    this.location1.SelectedIndex = 0;

                    this.location2.DataSource = locations.location2;
                    this.location2.DataBind();
                    this.location2.SelectedIndex = 0;


                }
            }

            if (Request.Cookies["data"] != null)
            {
                var cookie = Request.Cookies["data"];
                this.location1.SelectedValue = cookie.Values["l1"];
                this.location2.SelectedValue = cookie.Values["l2"];
            }
        }

        if (this.location1.SelectedIndex != 0 || this.location2.SelectedIndex != 0)
        {
            HttpCookie cookie = new HttpCookie("data");
            cookie.Expires = DateTime.Now.AddYears(1);
            cookie.Values.Add("l1", this.location1.SelectedValue);
            cookie.Values.Add("l2", this.location2.SelectedValue);
            Response.Cookies.Add(cookie);

            string qs = "&location1=" + (this.location1.SelectedItem.ToString()) + "&location2=" + (this.location2.SelectedItem.ToString());

            var itemReq = HttpWebRequest.CreateHttp("http://192.168.1.210:8888/JSON?request=getstatus&user=home&pass=4362" + qs);
            var itemResponse = itemReq.GetResponse();
            using (var data = itemResponse.GetResponseStream())
            {
                using (var reader = new StreamReader(data))
                {
                    string resposneString = reader.ReadToEnd();
                    var items = JsonConvert.DeserializeObject<DeviceData>(resposneString);

                    var filtered = items.Devices.Where(i => i.Hide_From_View == false);
                    this.items.DataSource = filtered;
                    this.items.DataBind();
                }
            }
        }
        else
        {
            this.items.DataSource = null;
            this.items.DataBind();
        }
    }
    protected void controlLink_Click(object sender, EventArgs e)
    {
        LinkButton button = sender as LinkButton;

        Response.Redirect("control.aspx?ref=" + button.CommandArgument);
    }
}
