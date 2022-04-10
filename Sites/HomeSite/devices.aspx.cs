using msn2.net.HomeSeer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class devices : System.Web.UI.Page
{
    Root root = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Debugger.IsAttached)
        {
            HttpCookie cookie = Request.Cookies["Login"];
            if (cookie == null || cookie.Value != "1")
            {
                Response.Redirect("https://home.giesler.org/login/?r=https://home.giesler.org/");
            }
        }

        

        try
        {
            TryLoadData();
        }
        catch (Exception ex)
        {
            errorPanel.Visible = true;
            weatherPanel.Visible = false;
            error.Text = "Error loading devices: " + ex.Message;
            error.ToolTip = ex.StackTrace;
        }
    }

    void TryLoadData()
    {
        HttpWebRequest deviceRequest = (HttpWebRequest)HttpWebRequest.Create("https://svcs.giesler.org:8443/HomeSeerStatus.aspx");
        HttpWebResponse response = (HttpWebResponse)deviceRequest.GetResponse();

        DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Root));
        object obj2 = json.ReadObject(response.GetResponseStream());
        root = obj2 as Root;

        if (root == null)
        {
            throw new Exception("No data returned");
        }

        SetLockStatus(garageBackLockAction, root, "Garage Back Door");
        SetLockStatus(garageInsideLockAction, root, "Garage Inside Door Lock");
        SetLockStatus(patioGarageLockAction, root, "Garage Patio Door");
        SetLockStatus(patioKitchenLockAction, root, "Kitchen Entry Door Lock");

        //SetGarageDoorStatus(garageDoorNorthAction, root, "North Garage Door - Input Sensor");
        //SetGarageDoorStatus(garageDoorCenterAction, root, "Center Garage Door - Input Sensor");
        garageDoorCenterAction.ForeColor = Color.DarkGray;
        //SetGarageDoorStatus(garageDoorSouthAction, root, "South Garage Door - Input Sensor");
        SetDoorStatus(shedDoor, root, "Shed Door - Door Sensor");
    }

    void SetLockStatus(LinkButton lnk, Root root, string deviceName)
    {
        Device device = root.Devices.First(d => d.Name != null && d.Name.Equals(deviceName));

        lnk.ToolTip = device.Status;
        if (device.Status == "Unlocked")
        {
            lnk.ForeColor = Color.DarkRed;
            lnk.ToolTip = "lock";
        }
        else if (device.Status == "Locked")
        {
            lnk.ForeColor = Color.DarkGreen;
            lnk.ToolTip = "unlock";
        }
        else
        {
            lnk.ForeColor = Color.DarkGray;
            lnk.Enabled = false;
        }
    }

    void SetDoorStatus(Label label, Root root, string deviceName)
    {
        Device device = root.Devices.First(d => d.Name != null && d.Name.Equals(deviceName));

        label.ToolTip = device.Status;

        if (device.Status == "Open")
        {
            label.ForeColor = Color.DarkRed;
        }
        else if (device.Status == "Closed")
        {
            label.ForeColor = Color.DarkGreen;
        }
    }

    void SetGarageDoorStatus(LinkButton lnk, Root root, string deviceName)
    {
        Device device = root.Devices.First(d => d.Name != null && d.Name.Equals(deviceName));

        lnk.ToolTip = device.Status;

        if (device.Status == "On")
        {
            lnk.ForeColor = Color.DarkRed;
        }
        else if (device.Status == "Off")
        {
            lnk.ForeColor = Color.DarkGreen;
        }
    }

    protected void garageBackLockActionClick(object sender, EventArgs e)
    {
        LockAction("Garage Back Door");
    }

    protected void garageInsideLockActionClick(object sender, EventArgs e)
    {
        LockAction("Garage Inside Door Lock");
    }

    protected void patioGarageLockActionClick(object sender, EventArgs e)
    {
        LockAction("Garage Patio Door");
    }

    protected void patioKitchenLockActionClick(object sender, EventArgs e)
    {
        LockAction("Kitchen Entry Door Lock");
    }

    void LockAction(string lockName)
    {
        Device device = root.Devices.First(d => d.Name.Equals(lockName));

        string status = device.Status == "Locked" ? "Unlock" : "Lock";

        string url = "https://svcs.giesler.org:8443/HS3DeviceControl.aspx?by=label&id=" + device.Ref.ToString() + "&v=" + status;
        HttpWebRequest deviceRequest = (HttpWebRequest)HttpWebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)deviceRequest.GetResponse();
        Thread.Sleep(TimeSpan.FromSeconds(2));

        TryLoadData();
    }

    protected void garageDoorNorthAction_Click(object sender, EventArgs e)
    {
        GarageDoorAction("North Garage Door - Output Relay");
    }

    protected void garageDoorCenterAction_Click(object sender, EventArgs e)
    {
        GarageDoorAction("Center Garage Door - Output Relay");
    }

    protected void garageDoorSouthAction_Click(object sender, EventArgs e)
    {
        GarageDoorAction("South Garage Door - Output Relay");
    }

    void GarageDoorAction(string deviceName)
    {
        Device device = root.Devices.First(d => d.Name.Equals(deviceName));

        string url = "https://svcs.giesler.org:8443/HS3DeviceControl.aspx?by=label&id=" + device.Ref.ToString() + "&v=On";
        HttpWebRequest deviceRequest = (HttpWebRequest)HttpWebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)deviceRequest.GetResponse();
        Thread.Sleep(TimeSpan.FromSeconds(10));

        TryLoadData();
    }
}