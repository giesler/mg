using msn2.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HomeServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISonos" in both code and config file together.
    [ServiceContract]
    public interface ISonos
    {
        [OperationContract]
        SonosPlayingData GetNowPlaying(string roomName);

        [OperationContract]
        IEnumerable<ZonePlayerStatus> GetPlayers();
    }
}
