using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class control : System.Web.UI.Page
{
    const string UpstairHallwayAddress = "24060";
    const string MediaRoomSideLightsAddress = "58666";
    const string GarageSwitchAddress = "28 CA 15 2";
    const string GarageSensorAddress = "28 CA 15 1";
    const string GardenDripName = "Garden drip";

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
   //         Response.Redirect("http://login.msn2.net/?r=http://control.msn2.net/");
        }

        if (!Page.IsPostBack)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            List<IsyData.GroupData> groups = isy.GetGroups().ToList();

            // first group contains all ISY nodes
            var garage = groups.First().Nodes.FirstOrDefault(g => g.Address == GarageSensorAddress);
            this.garageStatus.Text = garage.Status;

            var mc = groups.FirstOrDefault(g => g.Address == MediaRoomSideLightsAddress);
            this.mediaRoomStatus.Text = mc != null ? mc.Status : "unkown status";

            var upstairs = groups.FirstOrDefault(g => g.Address == UpstairHallwayAddress);
            this.upstairsHallStatus.Text = upstairs != null ? upstairs.Status : "unkown status";            

            DeviceControlService.DeviceControlClient dc = new DeviceControlService.DeviceControlClient();
            var status = dc.GetDeviceStatus(GardenDripName);
            
            this.dripToggleOn.Enabled = !status.IsOn;
            this.dripToggleOff.Enabled = status.IsOn;
            this.dripStatus.Text = status.StatusText;
        }
    }

    protected void toggleGarage_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(GarageSwitchAddress);
        Response.Redirect(Request.Url.ToString(), true);
    }

    protected void mediaRoomOn_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(MediaRoomSideLightsAddress);
        Response.Redirect(Request.Url.ToString(), true);
    }

    protected void mediaRoomOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(MediaRoomSideLightsAddress);
        Response.Redirect(Request.Url.ToString(), true);
    }

    protected void upstairsHallOn_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(UpstairHallwayAddress);
        Response.Redirect(Request.Url.ToString(), true);
    }

    protected void upstairsHallOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(UpstairHallwayAddress);
        Response.Redirect(Request.Url.ToString(), true);
    }

    protected void dripToggleOn_Click(object sender, EventArgs e)
    {
        DeviceControlService.DeviceControlClient dc = new DeviceControlService.DeviceControlClient();
        dc.TurnOn(GardenDripName, TimeSpan.FromMinutes(15));
        
        Thread.Sleep(TimeSpan.FromSeconds(2));
        Response.Redirect(Request.Url.ToString());
    }

    protected void dripToggleOff_Click(object sender, EventArgs e)
    {
        DeviceControlService.DeviceControlClient dc = new DeviceControlService.DeviceControlClient();
        dc.TurnOff(GardenDripName);

        Thread.Sleep(TimeSpan.FromSeconds(2));
        Response.Redirect(Request.Url.ToString());
    }
}