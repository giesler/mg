using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class control : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["ref"]))
        {
            Response.Redirect("/");
        }

        if (!Page.IsPostBack)
        {
            string qs = "ref=" + Request.QueryString["ref"];
            var itemReq = HttpWebRequest.CreateHttp("http://192.168.1.210:8888/JSON?request=getstatus&user=home&pass=4362&" + qs);
            var itemResponse = itemReq.GetResponse();
            using (var data = itemResponse.GetResponseStream())
            {
                using (var reader = new StreamReader(data))
                {
                    string resposneString = reader.ReadToEnd();
                    var items = JsonConvert.DeserializeObject<DeviceData>(resposneString);

                    this.name.Text = items.Devices[0].Name;
                    this.status.Text = items.Devices[0].Status;
                }
            }

            HttpWebRequest req = HttpWebRequest.CreateHttp("http://192.168.1.210:8888/JSON?request=getcontrol&ref=" + Request.QueryString["ref"]);
            var response = req.GetResponse();
            using (var dataStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(dataStream))
                {
                    string responseString = reader.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<DeviceControlInfo>(responseString);

                    this.items.DataSource = data.ControlPairs;
                    this.items.DataBind();
                }
            }
        }
    }

    protected void controlLink_Click(object sender, EventArgs e)
    {
        string id = Request.QueryString["ref"];
        var button = sender as LinkButton;

        var req = HttpWebRequest.CreateHttp("http://192.168.1.210:8888/JSON?request=controldevicebyvalue&ref=" + id + "&value=" + button.CommandArgument);
        req.GetResponse();
        Thread.Sleep(TimeSpan.FromMilliseconds(1500));

        Response.Redirect("control.aspx?ref=" + id);
    }
}