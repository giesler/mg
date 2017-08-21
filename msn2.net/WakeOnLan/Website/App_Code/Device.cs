using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for Device
/// </summary>
public class Device
{
	public Device()
	{
	}

    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string HostNameOrIPAddress { get; set; }

    [DataMember]
    public string MacAddress { get; set; }
}