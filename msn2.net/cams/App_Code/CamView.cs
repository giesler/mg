using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for CamView
/// </summary>
public class CamView
{
	public CamView()
	{
        this.Cameras = new List<Cam>();
	}

    public string Name { get; set; }

    public int? MaxHeight { get; set; }

    public List<Cam> Cameras { get; set; }

    public Orientation Orientation { get; set; }

    public TimeSpan RefreshInterval { get; set; }
}