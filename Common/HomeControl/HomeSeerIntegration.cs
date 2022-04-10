using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net.HomeSeer
{
    public class HomeSeerIntegration
    {
        public Root GetHomeSeerDeviceStatus()
        {
            var items = JsonConvert.DeserializeObject<Root>(GetHomeSeerDeviceStatusJson());
            return items;
        }

        public string GetHomeSeerDeviceStatusJson()
        {
            var itemReq = HttpWebRequest.CreateHttp("http://hs2:8080/JSON?request=getstatus");
            var itemResponse = itemReq.GetResponse();
            using (var data = itemResponse.GetResponseStream())
            {
                using (var reader = new StreamReader(data))
                {                    
                    string resposneString = reader.ReadToEnd();
                    return resposneString;
                }
            }
        }
    }

    [DataContract]
    public class DeviceType
    {
        [DataMember(Name = "Device_API")]
        public int DeviceAPI { get; set; }

        [DataMember(Name = "Device_API_Description")]
        public string DeviceAPIDescription { get; set; }

        [DataMember(Name = "Device_Type")]
        public int DeviceTypeId { get; set; }

        [DataMember(Name = "Device_Type_Description")]
        public string DeviceTypeDescription { get; set; }

        [DataMember(Name = "Device_SubType")]
        public int DeviceSubType { get; set; }

        [DataMember(Name = "Device_SubType_Description")]
        public string DeviceSubTypeDescription { get; set; }
    }

    [DataContract]
    public class Device
    {
        [DataMember(Name = "ref")]
        public int Ref { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "location2")]
        public string Location2 { get; set; }

        [DataMember(Name = "value")]
        public double Value { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "device_type_string")]
        public string DeviceTypeString { get; set; }

        [DataMember(Name = "last_change")]
        public DateTime LastChange { get; set; }

        [DataMember(Name = "relationship")]
        public int Relationship { get; set; }

        [DataMember(Name = "hide_from_view")]
        public bool HideFromView { get; set; }

        [DataMember(Name = "associated_devices")]
        public List<int> AssociatedDevices { get; set; }

        [DataMember(Name = "device_type")]
        public DeviceType DeviceType { get; set; }

        [DataMember(Name = "device_type_values")]
        public object DeviceTypeValues { get; set; }

        [DataMember(Name = "UserNote")]
        public string UserNote { get; set; }

        [DataMember(Name = "UserAccess")]
        public string UserAccess { get; set; }

        [DataMember(Name = "status_image")]
        public string StatusImage { get; set; }

        [DataMember(Name = "voice_command")]
        public string VoiceCommand { get; set; }

        [DataMember(Name = "misc")]
        public int Misc { get; set; }

        [DataMember(Name = "interface_name")]
        public string InterfaceName { get; set; }
    }

    [DataContract]
    public class Root
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Version")]
        public string Version { get; set; }

        [DataMember(Name = "TempFormatF")]
        public bool TempFormatF { get; set; }

        [DataMember(Name = "Devices")]
        public List<Device> Devices { get; set; }
    }
}
