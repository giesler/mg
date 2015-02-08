using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class control : System.Web.UI.Page
{
    const string UpstairHallwayAddress = "24060";
    const string MediaRoomSideLightsAddress = "58666";
    const string GarageSwitchAddress = "28 CA 15 2";
    const string GarageSensorAddress = "28 CA 15 1";
    const string CoopDoorAddress = "32 49 A5 1";

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
            Response.Redirect("Login.aspx");
        }

        if (!Page.IsPostBack)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            List<IsyData.GroupData> groups = isy.GetGroups().ToList();

            this.garageStatus.Text = GetStatus(groups, GarageSensorAddress);
            this.mediaRoomStatus.Text = GetStatus(groups, MediaRoomSideLightsAddress);
            this.upstairsHallStatus.Text = GetStatus(groups, UpstairHallwayAddress);
            this.coopStatus.Text = GetStatus(groups, CoopDoorAddress);
        }
    }

    string GetStatus(List<IsyData.GroupData> groups, string address)
    {
        foreach (IsyData.GroupData group in groups)
        {
            if (group.Address == address)
            {
                return group.Status;
            }

            IsyData.NodeData node = group.Nodes.FirstOrDefault(n => n.Address == address);
            if (node != null)
            {
                return node.Status;
            }
        }

        return "unknown";
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
}