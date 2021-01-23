using System.Diagnostics;
using System.Threading.Tasks;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using DCS.Infrastructure.Tests.Bootstrap;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DCS.Infrastructure.Tests.ExternalServices
{
    [TestFixture]
    public class AirportServiceProxyTests
    {
        [Test]
        public async Task ShouldGetAirportInfo()
        {
            var airportService = GetService();
            var airportInfoResponse = await airportService.GetAirportInfo("OVB");
            airportInfoResponse.IsSuccess.Should().BeTrue();
            airportInfoResponse.FaultMessage.Should().BeNull();
            airportInfoResponse.Value.Should().NotBeNull();
            
            var airportInfo = airportInfoResponse.Value;
            airportInfo.Country.Should().Be("Russian Federation");
            airportInfo.CityIata.Should().Be("OVB");
            airportInfo.Iata.Should().Be("OVB");
            airportInfo.City.Should().Be("Novosibirsk");
            airportInfo.TimezoneRegionName.Should().Be("Asia/Novosibirsk");
            airportInfo.CountryIata.Should().Be("RU");
            airportInfo.Rating.Should().Be(2);
            airportInfo.Name.Should().Be("Novosibirsk");
            airportInfo.Type.Should().Be("airport");
            airportInfo.Hubs.Should().Be(1);

            var location = airportInfo.Location;
            location.Latitude.Should().Be(55.009011);
            location.Longitude.Should().Be(82.666999);
        }
        
        [Test]
        public async Task ShouldGetNotFoundForNonExistingAirport()
        {
            var airportService = GetService();
            var airportInfoResponse = await airportService.GetAirportInfo("XXX");

            airportInfoResponse.IsSuccess.Should().BeFalse();
            airportInfoResponse.FaultMessage.Should().Be("Not Found");
        }
        
        [DebuggerStepThrough]
        private static IAirportInfoService GetService()
        {
            return CompositionRoot.ServiceProvider.GetRequiredService<IAirportInfoService>();
        }
    }
}