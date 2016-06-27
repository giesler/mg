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
        public long RainDelayExpirationDate { get; set; }

        [DataMember(Name = "rainDelayStartDate")]
        public long RainDelayStartDate { get; set; }

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
}
