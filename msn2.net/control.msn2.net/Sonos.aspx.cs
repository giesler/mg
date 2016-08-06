using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sonos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SonosService.SonosClient client = new SonosService.SonosClient();
        this.players.DataSource = client.GetPlayers();
        this.players.DataBind();
    }

    protected string GetUrl(string url)
    {
        return string.Format("http://{0}:1400/reboot", url);
    }
}