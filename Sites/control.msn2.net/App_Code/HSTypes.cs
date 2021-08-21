using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class Locations
{
    public string Name { get; set; }

    public string Version { get; set; }

    public string[] location1 { get; set; }

    public string[] location2 { get; set; }
}

public class DeviceData
{
    public string Name { get; set; }

    public string Version { get; set; }

    public List<Device> Devices { get; set; }
}

public class Device
{
    public string Ref { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string Location2 { get; set; }

    public string Value { get; set; }

    public string Status { get; set; }

    public string Device_Type_String { get; set; }

    public string Last_Change { get; set; }

    public int Relationship { get; set; }

    public bool Hide_From_View { get; set; }

    public int[] Associated_Devices { get; set; }

    public DeviceType Device_Type { get; set; }

    public string Device_Image { get; set; }

    public string UserNote { get; set; }

    public string UserAccess { get; set; }

    public string Status_Image { get; set; }

    public string Voice_Command { get; set; }
}

public class DeviceType
{
    public int Device_API { get; set; }

    public string Device_API_Description { get; set; }

    public int Device_Type { get; set; }

    public string Device_Type_Description { get; set; }

    public int Device_SubType { get; set; }

    public string Device_SubType_Description { get; set; }
}

public class DeviceControlInfo
{
    public DeviceControl[] ControlPairs { get; set; }
}

public class DeviceControl
{
    public bool Do_Update { get; set; }
    public bool SingleRangeEntry { get; set; }

    public int ControlButtonType { get; set; }

    public string ControlButtonCustom { get; set; }

    public int CCIndex { get; set; }

    public string Range { get; set; }

    public int Ref { get; set; }

    public string Label { get; set; }

    public int ControlType { get; set; }

    public DeviceControlLocation ControlLocation { get; set; }

    public int ControlLoc_Row { get; set; }

    public int ControlLoc_Column { get; set; }

    public int ControlLoc_ColumnSpan { get; set; }

    public int ControlUse { get; set; }

    public int ControlValue { get; set; }

    public string ControlString { get; set; }

    public string ControlStringList { get; set; }

    public string ControlStringSelected { get; set; }

    public bool ControlFlag { get; set; }
}

public class DeviceControlLocation
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int ColumnSpan { get; set; }
}