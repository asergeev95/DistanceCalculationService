namespace DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts
{
    public class AirportInfo
    {
        public string Country { get; set; }
        public string CityIata { get; set; }
        public string Iata { get; set; }
        public string City { get; set; }
        public string TimezoneRegionName { get; set; }
        public string CountryIata { get; set; }
        public double Rating { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Type { get; set; }
        public uint Hubs { get; set; }
    }
    
    public class Location
    {
        public double Londitute { get; set; }
        public double Latitude { get; set; }
    }
}