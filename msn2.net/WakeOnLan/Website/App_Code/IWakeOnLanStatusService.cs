using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

[ServiceContract]
public interface IWakeOnLanStatusService
{
	[OperationContract]
	IEnumerable<Device> GetDevices();

    [OperationContract]
    void UpdateDeviceStatus(int id, bool isActive, string macAddress);
}

