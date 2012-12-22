using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CamAlertService
{
    [ServiceContract]
    public interface ICamVideoStorage
    {
        [OperationContract]
        byte[] GetFile(string fileName);
    }
}
