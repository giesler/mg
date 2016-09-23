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
	//public static CamView GetCoopView()
	//{
 //       CamView view = new CamView
 //       {
 //           MaxHeight = 64,
 //           Name = "Coop",
 //           Orientation = Orientation.Vertical,
 //           RefreshInterval = TimeSpan.FromSeconds(20)
 //       };
        
 //       view.Cameras.Add(Cams.CoopBottom);
 //       view.Cameras.Add(Cams.CoopTop);

 //       return view;
 //   }

    //public static CamView GetCoopSideView()
    //{
    //    CamView view = new CamView
    //    {
    //        MaxHeight = 64,
    //        Name = "Outside Coop",
    //        Orientation = Orientation.Horizontal,
    //        RefreshInterval = TimeSpan.FromSeconds(20),
    //    };
    //    view.Cameras.Add(Cams.CoopSide);

    //    return view;
    //}

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