using msn2.net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class control : System.Web.UI.Page
{
    const string UpstairHallwayAddress = "24060";
    const string KitchenLightsAddress = "2B B8 3E 1";
    const string LivingRoomSideLightsAddress = "18965";
    const string MediaRoomSideLightsAddress = "58666";
    const string MasterSinkLightAddress = "2B BD 8C 1";
    const string MasterBathFanAddress = "34 75 65 1";
    const string Garage1SwitchAddress = "28 CA 15 2";
    const string Garage1SensorAddress = "28 CA 15 1";
    const string Garage2SwitchAddress = "34 88 F9 2";
    const string Garage2SensorAddress = "34 88 F9 1";
    const string GarageEntryDoorAddress = "ZW002_1";
    const string GarageOutsideDoorAddress = "ZW008_1";
    const string FrontDoorAddress = "ZW003_1";
    const string CoopDoorAddress = "32 49 A5 1";
    const string GardenDripName = "Garden drip";
    const string NeilsRoomLightAddress = "2B B6 38 1";
    const string MediaRoomMainLightsAddress = "2295";

    string activeGroup = "switches";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Debugger.IsAttached)
        {
            HttpCookie cookie = Request.Cookies["Login"];
            if (cookie == null || cookie.Value != "1")
            {
                Response.Redirect("http://login.msn2.net/?r=http://control.msn2.net/");
            }
        }

        object group = Request.QueryString["g"];
        if (group != null)
        {
            this.activeGroup = group.ToString();
        }

        if (!Page.IsPostBack)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            List<IsyData.GroupData> groups = isy.GetGroups().ToList();

            // first group contains all ISY nodes
            var garage1 = groups.First().Nodes.FirstOrDefault(g => g.Address == Garage1SensorAddress);
            this.garage1Status.Text = garage1.Status;

            var garage2 = groups.First().Nodes.FirstOrDefault(g => g.Address == Garage2SensorAddress);
            this.garage2Status.Text = garage2.Status;

            var garageEntryDoor = groups.First().Nodes.FirstOrDefault(g => g.Address == GarageEntryDoorAddress);
            this.garageEntryStatus.Text = garageEntryDoor.Status;

            var garageOutsideDoor = groups.First().Nodes.FirstOrDefault(g => g.Address == GarageOutsideDoorAddress);
            this.garageOutsideStatus.Text = garageOutsideDoor.Status;

            var frontDoor = groups.First().Nodes.FirstOrDefault(g => g.Address == FrontDoorAddress);
            this.frontDoorStatus.Text = frontDoor.Status;

            var lr = groups.FirstOrDefault(g => g.Address == LivingRoomSideLightsAddress);
            this.livingRoomStatus.Text = lr.Status;

            var mr = groups.FirstOrDefault(g => g.Address == MediaRoomMainLightsAddress);
            this.mediaRoomMainStatus.Text = mr != null ? mr.Status : "unknown status";

            var mc = groups.FirstOrDefault(g => g.Address == MediaRoomSideLightsAddress);
            this.mediaRoomStatus.Text = mc != null ? mc.Status : "unkown status";

            var kitchen = groups.First().Nodes.FirstOrDefault(g => g.Address == KitchenLightsAddress);
            this.kitchenStatus.Text = kitchen != null ? kitchen.Status : "unknown status";

            var upstairs = groups.FirstOrDefault(g => g.Address == UpstairHallwayAddress);
            this.upstairsHallStatus.Text = upstairs != null ? upstairs.Status : "unkown status";

            var masterSinkLight = groups.First().Nodes.FirstOrDefault(g => g.Address == MasterSinkLightAddress);
            this.masterSinkLightStatus.Text = masterSinkLight != null ? masterSinkLight.Status : "unknown status";

            var masterBathFan = groups.First().Nodes.FirstOrDefault(g => g.Address == MasterBathFanAddress);
            this.masterBathFanStatus.Text = masterBathFan != null ? masterBathFan.Status : "unknown status";

            var coopDoor = groups.First().Nodes.FirstOrDefault(g => g.Address == CoopDoorAddress);
            this.coopDoorStatus.Text = coopDoor != null ? coopDoor.Status : "unknown status";

            var neilLight = groups.First().Nodes.FirstOrDefault(g => g.Address == NeilsRoomLightAddress);
            this.neilsRoomLightStatus.Text = neilLight != null ? neilLight.Status : "unknown status";

            try
            {
                this.gardenDripStatus.Text = RachioIntegration.GetCurrentStatus();
            }
            catch (Exception ex)
            {
                this.gardenDripStatus.Text = "error loading status";
                this.gardenDripStatus.ToolTip = ex.ToString();
            }

            // shows 7:33 am - actual 11:33 am
            // tomorrow 2:30 am, actual 6/28 7:22 pm
            /*            try
                        {
                            DeviceControlService.DeviceControlClient dc = new DeviceControlService.DeviceControlClient();
                            var status = dc.GetDeviceStatus(GardenDripName);

                            this.dripToggleOn.Enabled = !status.IsOn;
                            this.dripToggleOff.Enabled = status.IsOn;
                            this.dripStatus.Text = status.StatusText;
                        }
                        catch (Exception ex)
                        {
                            this.dripStatus.Text = "error: " + ex.GetType().Name;
                            this.dripStatus.ToolTip = ex.Message;
                        }*/
        }
    }

    protected void toggleGarage1_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(Garage1SwitchAddress);
        this.Redirect("doors");
    }

    protected void toggleGarage2_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(Garage2SwitchAddress);
        this.Redirect("doors");
    }
    protected void livingRoomOn_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(LivingRoomSideLightsAddress);
        this.Redirect("switches");
    }

    protected void livingRoomOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(LivingRoomSideLightsAddress);
        this.Redirect("switches");
    }
    
    protected void mediaRoomOn_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(MediaRoomSideLightsAddress);
        this.Redirect("switches");
    }

    protected void mediaRoomOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(MediaRoomSideLightsAddress);
        this.Redirect("switches");
    }

    protected void kitchenOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(KitchenLightsAddress);
        this.Redirect("switches");
    }

    protected void upstairsHallOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(UpstairHallwayAddress);
        this.Redirect("switches");
    }

    protected void dripToggleOn_Click(object sender, EventArgs e)
    {
        DeviceControlService.DeviceControlClient dc = new DeviceControlService.DeviceControlClient();
        dc.TurnOn(GardenDripName, TimeSpan.FromMinutes(15));

        Thread.Sleep(TimeSpan.FromSeconds(2));
        this.Redirect("commands");
    }

    protected void dripToggleOff_Click(object sender, EventArgs e)
    {
        DeviceControlService.DeviceControlClient dc = new DeviceControlService.DeviceControlClient();
        dc.TurnOff(GardenDripName);

        Thread.Sleep(TimeSpan.FromSeconds(2));
        this.Redirect("commands");
    }
    
    protected void masterSinkLightOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(MasterSinkLightAddress);
        this.Redirect("switches");
    }

    protected void masterBathFanOn_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOn(MasterBathFanAddress);
        this.Redirect("switches");
    }

    protected void masterBathFanOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(MasterBathFanAddress);
        this.Redirect("switches");
    }

    protected void level100_Click(object sender, EventArgs e)
    {
        if (this.levelItem.Value.ToLower() == "master sink light")
        {
            SetLevel(sender, MasterSinkLightAddress);
        }
        else if (this.levelItem.Value.ToLower() == "upstairs hall light")
        {
            SetLevel(sender, UpstairHallwayAddress);
        }
        else if (this.levelItem.Value.ToLower() == "kitchen light")
        {
            SetLevel(sender, KitchenLightsAddress);
        }
        else if (this.levelItem.Value.ToLower() == "neils room lights")
        {
            SetLevel(sender, NeilsRoomLightAddress);
        }
        else if (this.levelItem.Value.ToLower() == "media room lights")
        {
            SetLevel(sender, MediaRoomMainLightsAddress);
        }

        throw new Exception("Unrecognized value: " + this.levelItem.Value);
    }

    private void SetLevel(object sender, string address)
    {
        Button button = (Button)sender;
        int level = int.Parse(button.ID.Replace("level", ""));
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.SetLevel(address, level);
        this.Redirect("switches");
    }

    private void Redirect(string activeGroup)
    {
        Thread.Sleep(1000);
        Response.Redirect("/?g=" + activeGroup, true);
    }


    protected void garageOutsideUnlock_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.Unlock(GarageOutsideDoorAddress);
        this.Redirect("doors");
    }

    protected void garageOutsideLock_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.Lock(GarageOutsideDoorAddress);
        this.Redirect("doors");
    }

    protected void garageEntryLock_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.Lock(GarageEntryDoorAddress);
        this.Redirect("doors");
    }

    protected void garageEntryUnlock_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.Unlock(GarageEntryDoorAddress);
        this.Redirect("doors");
    }

    protected void frontDoorLock_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.Lock(FrontDoorAddress);
        this.Redirect("doors");
    }

    protected void frontDoorUnlock_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.Unlock(FrontDoorAddress);
        this.Redirect("doors");
    }

    protected void mainRoomsAudioUnmute_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.RunProgram(IsyData.ProgramRunType.run, "0011");
        this.Redirect("commands");
    }

    protected void mainRoomsAudioMute_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.RunProgram(IsyData.ProgramRunType.run, "0010");
        this.Redirect("commands");
    }

    protected void gardenDripOff_Click(object sender, EventArgs e)
    {
        RachioIntegration.StopDrip();
        this.Redirect("commands");
    }

    protected void duration1_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        int level = int.Parse(button.ID.Replace("duration", ""));

        RachioIntegration.StartDrip(TimeSpan.FromMinutes(level));
        this.Redirect("commands");
    }

    protected void tvAudioOn_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.RunProgram(IsyData.ProgramRunType.runThen, "0015");
        this.Redirect("commands");
    }

    protected void tvAudioOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.RunProgram(IsyData.ProgramRunType.runElse, "0015");
        this.Redirect("commands");
    }

    protected void neilsRoomLightOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(NeilsRoomLightAddress);
        this.Redirect("switches");
    }

    protected string GetActiveGroup()
    {
        return this.activeGroup;
    }

    protected void mediaRoomMainOff_Click(object sender, EventArgs e)
    {
        IsyData.ISYClient client = new IsyData.ISYClient();
        client.TurnOff(MediaRoomMainLightsAddress);
        this.Redirect("switches");
    }
}