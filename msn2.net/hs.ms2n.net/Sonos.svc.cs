using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using msn2.net;
using System.ServiceModel.Activation;

namespace HomeServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Sonos : ISonos
    {
        public SonosPlayingData GetNowPlaying(string roomName)
        {
            return SonosIntegration.GetPlayingData(roomName);
        }

        public IEnumerable<ZonePlayerStatus> GetPlayers()
        {
            return SonosIntegration.GetPlayers();
        }
    }
}
