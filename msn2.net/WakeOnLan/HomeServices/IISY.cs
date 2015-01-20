using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HomeServices
{
    [ServiceContract]
    public interface IISY
    {
        [OperationContract]
        IEnumerable<NodeData> GetNodes();

        [OperationContract]
        IEnumerable<GroupData> GetGroups();

        [OperationContract]
        NodeData GetNode(string address);

        [OperationContract]
        void TurnOff(string address);

        [OperationContract]
        void TurnOn(string address);

        [OperationContract]
        bool GetStatus(string address);
    }
}
