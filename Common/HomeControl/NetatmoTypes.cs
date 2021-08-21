using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net
{
    [DataContract]
    public class OathToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "scope")]
        public string[] Scopes { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DataMember(Name = "expire_in")]
        public int ExpireIn { get; set; }
    }

    [DataContract]
    public class DeviceListResponse
    {
        [DataMember(Name = "body")]
        public DeviceWrapper Devices { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "time_exec")]
        public float TimeExec { get; set; }

        [DataMember(Name = "time_server")]
        public double TimeServerMS { get; set; }

        public DateTime TimeServer
        {
            get
            {
                return NetatmoIntegration.GetDate(this.TimeServerMS);
            }
        }
    }

    [DataContract]
    public class DeviceWrapper
    {
        [DataMember(Name = "devices")]
        public Device[] Devices { get; set; }
    }

    [DataContract]
    public class Device
    {
        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "cipher_id")]
        public string CipherId { get; set; }

        [DataMember(Name = "last_status_store")]
        public double LastStatusStoreMS { get; set; }

        public DateTime LastStatusStore
        {
            get
            {
                return NetatmoIntegration.GetDate(this.LastStatusStoreMS);
            }
        }

        [DataMember(Name = "modules")]
        public Module[] Modules { get; set; }

        [DataMember(Name = "dashboard_data")]
        public DashboardData DashboardData { get; set; }

        [DataMember(Name = "station_name")]
        public string StationName { get; set; }

        [DataMember(Name = "module_name")]
        public string ModuleName { get; set; }

    }

    [DataContract]
    public class Module
    {
        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "last_message")]
        public double LastMessageMS { get; set; }

        public DateTime LastMessage
        {
            get
            {
                return NetatmoIntegration.GetDate(this.LastMessageMS);
            }
        }

        [DataMember(Name = "last_seen")]
        public double LastSeenMS { get; set; }

        public DateTime LastSeen
        {
            get
            {
                return NetatmoIntegration.GetDate(this.LastSeenMS);
            }
        }

        [DataMember(Name = "dashboard_data")]
        public DashboardData DashboardData { get; set; }

        [DataMember(Name = "data_type")]
        public string[] DataType { get; set; }

        [DataMember(Name = "module_name")]
        public string ModuleName { get; set; }

        [DataMember(Name = "last_setup")]
        public double LastSetupMS { get; set; }

        public DateTime LastSetup
        {
            get
            {
                return NetatmoIntegration.GetDate(this.LastSetupMS);
            }
        }

        [DataMember(Name = "battery_vp")]
        public int BatteryVP { get; set; }

        [DataMember(Name = "battery_percent")]
        public int BatteryPercent { get; set; }

        [DataMember(Name = "rf_status")]
        public int RFStatus { get; set; }

        [DataMember(Name = "firmware")]
        public int Firmware { get; set; }

        public override string ToString()
        {
            return this.ModuleName + " (" + string.Join(",", this.DataType) + ")";
        }
    }

    [DataContract]
    public class DashboardData
    {
        [DataMember(Name = "time_utc")]
        public double TimeUtcMS { get; set; }

        public DateTime TimeUtc
        {
            get
            {
                return NetatmoIntegration.GetDate(this.TimeUtcMS);
            }
        }

        [DataMember(Name = "Temperature")]
        public float Temperature { get; set; }

        [DataMember(Name = "temp_trend")]
        public string TempTrend { get; set; }

        [DataMember(Name = "Humidity")]
        public int Humidity { get; set; }

        [DataMember(Name = "date_max_temp")]
        public double DateMaxTempMS { get; set; }

        public DateTime DateMaxTemp
        {
            get
            {
                return NetatmoIntegration.GetDate(this.DateMaxTempMS);
            }
        }

        [DataMember(Name = "date_min_temp")]
        public double DateMinTempMS { get; set; }

        public DateTime DateMinTemp
        {
            get
            {
                return NetatmoIntegration.GetDate(this.DateMinTempMS);
            }
        }

        [DataMember(Name = "min_temp")]
        public float MinTemp { get; set; }

        [DataMember(Name = "max_temp")]
        public float MaxTemp { get; set; }

        [DataMember(Name = "AbsolutePressure")]
        public float AbsolutePresure { get; set; }

        [DataMember(Name = "Noise")]
        public int Noise { get; set; }

        [DataMember(Name = "Pressure")]
        public float Pressure { get; set; }

        [DataMember(Name = "CO2")]
        public float CO2 { get; set; }

    }
}