using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Cam
/// </summary>
public class Cam
{
	public Cam(string id)
	{
        this.Id = id;
        this.Orientation = Orientation.Horizontal;
	}

    public string Id { get; set; }

    public Orientation Orientation { get; set; }

    public string HostPrefix { get; set; }

}