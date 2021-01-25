using System.Threading.Tasks;
using DCS.ApplicationService.Contracts;
using DCS.ApplicationService.Validation;
using DCS.Core;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;
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

        public async Task<AirportsDistanceState> CalculateDistanceBetweenAirports(string iataCode, string destinationIataCode)
        {
            var iataCodeValidationResult = await _validator.ValidateAsync(iataCode);
            if (iataCodeValidationResult.IsValid == false)
            {
                return new AirportsDistanceState
                {
                    Distance = -1,
                    FaultMessage = string.Join("; ", iataCodeValidationResult.Errors),
                    IsSuccess = false
                };
            }
            iataCodeValidationResult = await _validator.ValidateAsync(destinationIataCode);
            if (iataCodeValidationResult.IsValid == false)
            {
                return new AirportsDistanceState
                {
                    Distance = -1,
                    FaultMessage = string.Join("; ", iataCodeValidationResult.Errors),
                    IsSuccess = false
                };
            }
            
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
            return new AirportsDistanceState
            {
                Distance = distanceInMeters * 0.00062,
                FaultMessage = null,
                IsSuccess = true
            };
        }

        private static AirportsDistanceState GetErrorResponse(string iataCode, Result<AirportInfo> airportInfo)
        {
            return new()
            {
                Distance = -1,
                FaultMessage = $"Unable to receive information about airport with code {iataCode}. Error: {airportInfo.FaultMessage}",
                IsSuccess = false
            };
        }
    }
}