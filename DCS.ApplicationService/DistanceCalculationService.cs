using System.Threading.Tasks;
using DCS.ApplicationService.Validation;
using DCS.Core;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;
using FluentValidation;
using GeoCoordinatePortable;

namespace DCS.ApplicationService
{
    public class DistanceCalculationService
    {
        private readonly IAirportInfoService _airportInfoService;
        private readonly IataCodeValidator _validator;

        public DistanceCalculationService(IAirportInfoService airportInfoService, IataCodeValidator validator)
        {
            _airportInfoService = airportInfoService;
            _validator = validator;
        }

        public async Task<(string FaultMessage, bool IsSuccess, double Distance)> CalculateDistanceBetweenAirports(string iataCode, string destinationIataCode)
        {
            await _validator.ValidateAndThrowAsync(iataCode);
            await _validator.ValidateAndThrowAsync(destinationIataCode);
            
            var firstAirportInfo = await _airportInfoService.GetAirportInfo(iataCode.ToUpperInvariant());
            if (firstAirportInfo.IsSuccess == false)
            {
                return GetErrorResponse(iataCode, firstAirportInfo);
            }
            var secondAirportInfo = await _airportInfoService.GetAirportInfo(destinationIataCode.ToUpperInvariant());
            if (secondAirportInfo.IsSuccess == false)
            {
                return GetErrorResponse(destinationIataCode, secondAirportInfo);
            }
            var firstAirportCoordinates = new GeoCoordinate(firstAirportInfo.Value.Location.Latitude, firstAirportInfo.Value.Location.Longitude);
            var secondAirportCoordinates = new GeoCoordinate(secondAirportInfo.Value.Location.Latitude, secondAirportInfo.Value.Location.Longitude);
            var distanceInMeters = firstAirportCoordinates.GetDistanceTo(secondAirportCoordinates);
            return (null, true, distanceInMeters * 0.00062);
        }

        private static (string, bool, int) GetErrorResponse(string iataCode, Result<AirportInfo> firstAirportInfo)
        {
            return ($"Unable to receive information about airport with code {iataCode}. Error: {firstAirportInfo.FaultMessage}", false, -1);
        }
    }
}