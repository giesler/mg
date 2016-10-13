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
    public static Cam CoopDoor { get; private set; }
    public static Cam GarageDoor { get; private set; }
    public static Cam Driveway { get; private set; }
    public static Cam Front { get; private set; }
    public static Cam Side { get; private set; }

	static Cams()
	{
        Cams.CoopTop = new Cam("CoopTop") { HostPrefix = "cam2" };
        Cams.CoopDoor = new Cam("CoopDoor") { HostPrefix = "cam3" };
        Cams.GarageDoor = new Cam("gdoor") { HostPrefix="cam1"};
        Cams.Driveway = new Cam("dw1") { HostPrefix = "cam4" };
        Cams.Front = new Cam("front") { HostPrefix = "cam5" };
        Cams.Side = new Cam("side") { HostPrefix = "cam6" };
	}
}