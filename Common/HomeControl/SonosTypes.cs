using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net
{
    public class SonosPlayingData
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string AlbumArtUri { get; set; }

        public override bool Equals(object obj)
        {
            SonosPlayingData data = obj as SonosPlayingData;
            if (data == null)
            {
                return false;
            }

            return this.Title == data.Title && this.Album == data.Album && this.Artist == data.Artist && this.AlbumArtUri == data.AlbumArtUri;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract]
    public class ZonePlayerSupportInfo
    {
        [DataMember(Name ="ZonePlayers")]
        public List<ZonePlayerStatus> ZonePlayers { get; set; }
    }

    [DataContract]
    public class ZonePlayerStatus
    {
        [DataMember]
        public string Group { get; set; }

        [DataMember]
        public int GroupNumber { get; set; }

        [DataMember]
        public bool Coordinator { get; set; }

        [DataMember]
        public bool WirelessMode { get; set; }

        [DataMember]
        public bool WirelessLeafOnly { get; set; }

        [DataMember]
        public bool HasConfiguredSSID { get; set; }

        [DataMember]
        public int ChannelFrequency { get; set; }

        [DataMember]
        public bool BehindWifiExt { get; set; }

        [DataMember]
        public bool WifiEnabled { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string UUID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.GroupNumber); ;
        }
    }
}
