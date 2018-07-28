using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for CamViews
/// </summary>
public class CamViews
{

    public static List<CamView> GetAll()
    {
        List<CamView> views = new List<CamView>();

        views.Add(CamViews.GetDrivewayView());
        views.Add(CamViews.GetFrontView());
        views.Add(CamViews.GetSideView());
        views.Add(CamViews.GetGarageDoorView());
        views.Add(CamViews.GetCoopTopView());
//        views.Add(CamViews.GetCoopDoorView());

        return views;
    }
    public static CamView GetCoopTopView()
    {
        CamView view = new CamView
        {
            MaxHeight = 64,
            Name = "Coop Top",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };

        view.Cameras.Add(Cams.CoopTop);

        return view;
    }

    public static CamView GetCoopDoorView()
    {
        CamView view = new CamView
        {
            MaxHeight = 64,
            Name = "Coop Door",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20),
        };
        view.Cameras.Add(Cams.CoopDoor);

        return view;
    }

    public static CamView GetGarageDoorView()
    {
        CamView view = new CamView
        {
            MaxHeight = 64,
            Name = "Garage Door",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20),
        };
        view.Cameras.Add(Cams.GarageDoor);

        return view;
    }

    public static CamView GetDrivewayView()
    {
        CamView view = new CamView
        {
            MaxHeight = 64,
            Name = "Driveway",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };

        view.Cameras.Add(Cams.Driveway);

        return view;
    }

    public static CamView GetFrontView()
    {
        CamView view = new CamView
        {
            MaxHeight = 64,
            Name = "Front",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };

        view.Cameras.Add(Cams.Front);

        return view;
    }

    public static CamView GetSideView()
    {
        CamView view = new CamView
        {
            MaxHeight = 64,
            Name = "Side",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };

        view.Cameras.Add(Cams.Side);

        return view;
    }


}