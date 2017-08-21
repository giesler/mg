using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HomeServices
{
    [ServiceContract]
    public interface IDevicePower
    {
        [OperationContract]
        void Resume(string accessKey, string macAddress);
        
        [OperationContract]
        void Suspend(string accessKey, string hostNameOrIPAddress);
    }
}
