using System;
using System.Runtime.Serialization;

namespace msn2.net
{
    [DataContract]
    public class PersonResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "username")]
        public string UserName { get; set; }

        [DataMember(Name = "fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "devices")]
        public Device[] Devices { get; set; }

        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }

    [DataContract]
    public class Device
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "zones")]
        public Zone[] Zones { get; set; }

        [DataMember(Name = "timeZone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "lattitude")]
        public decimal Lattitude { get; set; }

        [DataMember(Name = "longitude")]
        public decimal Longitude { get; set; }

        [DataMember(Name = "zip")]
        public string Zip { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "scheduleRules")]
        public ScheduleRules[] ScheduleRules { get; set; }

        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; set; }

        [DataMember(Name = "rainDelayExpirationDate")]
        public double RainDelayExpirationDateMS { get; set; }

        public DateTime RainDelayExpirationDate
        {
            get
            {
                return RachioIntegration.GetDate(this.RainDelayExpirationDateMS);
            }
        }

        [DataMember(Name = "rainDelayStartDate")]
        public double RainDelayStartDateMS { get; set; }

        public DateTime RainDelayStartDate
        {
            get
            {
                return RachioIntegration.GetDate(this.RainDelayStartDateMS);
            }
        }

        [DataMember(Name = "macAddress")]
        public string MacAddress { get; set; }

        [DataMember(Name = "elevation")]
        public decimal Elevation { get; set; }

        [DataMember(Name = "webhooks")]
        public WebHooks[] WebHooks { get; set; }

        [DataMember(Name = "paused")]
        public bool Paused { get; set; }

        [DataMember(Name = "on")]
        public bool On { get; set; }

        [DataMember(Name = "flexScheduleRules")]
        public FlexScheduleRules[] FlexScheduleRules { get; set; }

        [DataMember(Name = "utcOffset")]
        public long UtcOffset { get; set; }
    }

    [DataContract]
    public class Zone
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "zoneNumber")]
        public int ZoneNumber { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }

        [DataMember(Name = "customNozzle")]
        public Nozzle Nozzle { get; set; }

        [DataMember(Name = "availableWater")]
        public decimal AvailableWater { get; set; }

        [DataMember(Name = "rootZoneDepth")]
        public decimal RootZoneDepth { get; set; }

        [DataMember(Name = "managementAllowedDepletion")]
        public decimal ManagementAllowedDepletion { get; set; }

        [DataMember(Name = "efficiency")]
        public decimal Efficiency { get; set; }

        [DataMember(Name = "yardAreaSquareFeet")]
        public decimal YardAreaSquareFeet { get; set; }

        [DataMember(Name = "irrigationAmount")]
        public decimal IrrigationAmount { get; set; }

        [DataMember(Name = "depthOfWater")]
        public decimal DepthOfWater { get; set; }

        [DataMember(Name = "runtime")]
        public int Runtime { get; set; }

        [DataMember(Name = "lastWateredDuration")]
        public int LastWateredDuration { get; set; }

        [DataMember(Name = "lastWateredDate")]
        public long LastWateredDate { get; set; }
    }

    [DataContract]
    public class Nozzle
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "inchesPerHour")]
        public decimal InchesPerHour { get; set; }
    }

    [DataContract]
    public class ScheduleRules
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "externalName")]
        public string ExternalName { get; set; }
    }

    [DataContract]
    public class WebHooks { }

    [DataContract]
    public class FlexScheduleRules { }

    [DataContract]
    public class Event
    {
        [DataMember(Name = "createDate")]
        public double CreateDateMS { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return RachioIntegration.GetDate(CreateDateMS);
            }
        }

        [DataMember(Name = "lastUpdatedDate")]
        public double LastUpdatedDateMS { get; set; }

        public DateTime LastUpdatedDate
        {
            get
            {
                return RachioIntegration.GetDate(LastUpdatedDateMS);
            }
        }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "deviceId")]
        public string DeviceId { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "eventDate")]
        public double EventDateMS { get; set; }
        
        public DateTime EventDate
        {
            get
            {
                return RachioIntegration.GetDate(EventDateMS);
            }
        }

        [DataMember(Name = "eventDatas")]
        public EventData[] EventData { get; set; }

        [DataMember(Name = "iconUrl")]
        public string IconUrl { get; set; }

        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        public override string ToString()
        {
            return this.Summary;
        }
    }

    [DataContract]
    public class EventData
    {
        [DataMember(Name = "createDate")]
        public double CreateDateMS { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return RachioIntegration.GetDate(CreateDateMS);
            }
        }

        [DataMember(Name = "lastUpdatedDate")]
        public double LastUpdatedDateMS { get; set; }
        
        public DateTime LastUpdatedDate
        {
            get
            {
                return RachioIntegration.GetDate(LastUpdatedDateMS);
            }
        }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "convertedValue")]
        public string Value { get; set; }

        public override string ToString()
        {
            return this.Key + " = " + this.Value;
        }
    }
}