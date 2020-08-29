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
        views.Add(CamViews.GetGarageDoorView());

        return views;
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

}