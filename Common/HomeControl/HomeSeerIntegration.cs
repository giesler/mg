using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net.HomeSeer
{
    public class HomeSeerIntegration
    { 
        public Root GetHomeSeerDeviceStatus()
        {
            var itemReq = HttpWebRequest.CreateHttp("http://hs2:8080/JSON?request=getstatus");
            var itemResponse = itemReq.GetResponse();
            using (var data = itemResponse.GetResponseStream())
            {
                using (var reader = new StreamReader(data))
                {
                    string resposneString = reader.ReadToEnd();
                    var items = JsonConvert.DeserializeObject<Root>(resposneString);
                    return items;
                }
            }
        }
    }

    public class DeviceType
    {
        [JsonProperty("Device_API")]
        public int DeviceAPI { get; set; }

        [JsonProperty("Device_API_Description")]
        public string DeviceAPIDescription { get; set; }

        [JsonProperty("Device_Type")]
        public int DeviceTypeId { get; set; }

        [JsonProperty("Device_Type_Description")]
        public string DeviceTypeDescription { get; set; }

        [JsonProperty("Device_SubType")]
        public int DeviceSubType { get; set; }

        [JsonProperty("Device_SubType_Description")]
        public string DeviceSubTypeDescription { get; set; }
    }

    public class Device
    {
        [JsonProperty("ref")]
        public int Ref { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("location2")]
        public string Location2 { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("device_type_string")]
        public string DeviceTypeString { get; set; }

        [JsonProperty("last_change")]
        public DateTime LastChange { get; set; }

        [JsonProperty("relationship")]
        public int Relationship { get; set; }

        [JsonProperty("hide_from_view")]
        public bool HideFromView { get; set; }

        [JsonProperty("associated_devices")]
        public List<int> AssociatedDevices { get; set; }

        [JsonProperty("device_type")]
        public DeviceType DeviceType { get; set; }

        [JsonProperty("device_type_values")]
        public object DeviceTypeValues { get; set; }

        [JsonProperty("UserNote")]
        public string UserNote { get; set; }

        [JsonProperty("UserAccess")]
        public string UserAccess { get; set; }

        [JsonProperty("status_image")]
        public string StatusImage { get; set; }

        [JsonProperty("voice_command")]
        public string VoiceCommand { get; set; }

        [JsonProperty("misc")]
        public int Misc { get; set; }

        [JsonProperty("interface_name")]
        public string InterfaceName { get; set; }
    }

    public class Root
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("TempFormatF")]
        public bool TempFormatF { get; set; }

        [JsonProperty("Devices")]
        public List<Device> Devices { get; set; }
    }
}
