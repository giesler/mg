using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Cams
/// </summary>
public class Cams
{
    public static Cam CoopBottom { get; private set; }
    public static Cam CoopTop { get; private set; }
    public static Cam CoopSide { get; private set; }
    public static Cam Driveway { get; private set; }
    public static Cam Front { get; private set; }
    public static Cam Side { get; private set; }

	static Cams()
	{
        Cams.CoopBottom = new Cam("1") { HostPrefix = "cam1", Orientation = Orientation.Vertical };
        Cams.CoopTop = new Cam("2") { HostPrefix = "cam2", Orientation = Orientation.Vertical };
        Cams.CoopSide = new Cam("3") { HostPrefix = "cam3" };
        Cams.Driveway = new Cam("dw1") { HostPrefix = "cam4" };
        Cams.Front = new Cam("front") { HostPrefix = "cam5" };
        Cams.Side = new Cam("side") { HostPrefix = "cam6" };
	}
}