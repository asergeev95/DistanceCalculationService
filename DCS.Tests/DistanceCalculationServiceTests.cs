using System;
using System.Threading.Tasks;
using DCS.ApplicationService;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;
using DCS.Tests.Bootstrap;
using DCS.Tests.Mocks;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DCS.Tests
{
    [TestFixture]
    public class DistanceCalculationServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            MockAirportInfoService.Recycle();
        }
        
        [Test]
        public async Task ShouldGetZeroDistanceFromAirportToItself()
        {
            MockAirportInfoService.AirportInfoValueToReturn = new AirportInfo
            {
                City = "Novosibirsk",
                Country = "Russian Federation",
                Hubs = 1,
                Iata = "OVB",
                Location = new Location
                {
                    Longitude = 82.666999,
                    Latitude = 55.009011
                },
                Name = "Novosibirsk",
                Rating = 2,
                Type = "airport",
                CityIata = "OVB",
                CountryIata = "RU",
                TimezoneRegionName = "Asia/Novosibirsk"
            };

            var service = CompositionRoot.ServiceProvider.GetRequiredService<DistanceCalculationService>();
            var res = await service.CalculateDistanceBetweenAirports("OVB", "OVB");
            res.IsSuccess.Should().BeTrue();
            res.FaultMessage.Should().BeNull();
            res.Distance.Should().Be(0);
        }

        [Test]
        public void ShouldThrowAnExceptionOnInvalidIataCode([Values("XX", "XXXX", "АБВ")] string iataCode)
        {
            var service = CompositionRoot.ServiceProvider.GetRequiredService<DistanceCalculationService>();
            Func<Task> func = async () => await service.CalculateDistanceBetweenAirports(iataCode, "OVB");
            func.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldGetFailureOnAirportInfoServiceFailureRequest()
        {
            MockAirportInfoService.IsSuccess = false;
            MockAirportInfoService.FaultMessage = "FaultMessage";
            var service = CompositionRoot.ServiceProvider.GetRequiredService<DistanceCalculationService>();
            var res = await service.CalculateDistanceBetweenAirports("XXX", "DME");
            res.IsSuccess.Should().BeFalse();
            res.FaultMessage.Should().Be("Unable to receive information about airport with code XXX. Error: FaultMessage");
        }
        
    }
}