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
        NodeData TurnOff(string address);

        [OperationContract]
        NodeData TurnOn(string address);

        [OperationContract]
        bool GetStatus(string address);

        [OperationContract]
        NodeData SetLevel(string address, int level);

        [OperationContract]
        NodeData Lock(string address);

        [OperationContract]
        NodeData Unlock(string address);

        [OperationContract]
        void RunProgram(ProgramRunType programRunType, string programId);

    }

    public enum ProgramRunType
    {
        run,
        runThen,
        runElse,
        stop,
        enable,
        disable,
        enableRunAtStartup,
        disableRunAtStartup
    }
}
