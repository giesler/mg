using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace msn2.net.BarMonkey.RelayController
{
    [ServiceContract]
    public interface IRelayController
    {
        [OperationContract]
        void ConnectTest();

        [OperationContract]
        void SendBatch(List<BatchItem> batch);
    }

    [DataContract]
    public class BatchItem
    {
        int relayNumber;
        int group;
        double seconds;

        [DataMember]
        public int RelayNumber
        {
            get { return this.relayNumber; }
            set { this.relayNumber = value; }
        }

        [DataMember]
        public int Group
        {
            get { return group; }
            set { group = value; }
        }

        [DataMember]
        public double Seconds
        {
            get { return seconds; }
            set { seconds = value; }
        }

    }
}
