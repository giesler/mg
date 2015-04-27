using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HomeServices
{
    [ServiceContract]
    public interface IDeviceControl
    {
        [OperationContract]
        void ToggleDevice(string name);

        [OperationContract]
        DeviceStatus GetDeviceStatus(string name);

        [OperationContract]
        void TurnOff(string name);

        [OperationContract]
        void TurnOn(string name, TimeSpan duration);
    }

    [DataContract]
    public class DeviceStatus
    {
        [DataMember]
        public bool IsOn { get; set; }

        [DataMember]
        public string StatusText { get; set; }
    }
}
