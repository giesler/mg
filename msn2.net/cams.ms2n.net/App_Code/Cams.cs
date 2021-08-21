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
    public static Cam GarageDoor { get; private set; }
    public static Cam Driveway { get; private set; }

	static Cams()
	{
        Cams.GarageDoor = new Cam("gdw") { HostPrefix="cam1"};
        Cams.Driveway = new Cam("dwety") { HostPrefix = "cam2" };
	}
}