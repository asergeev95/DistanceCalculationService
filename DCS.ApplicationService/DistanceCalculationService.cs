using DCS.ApplicationService.Validation;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using FluentValidation;
using GeoCoordinatePortable;

namespace DCS.ApplicationService
{
    public class DistanceCalculationService
    {
        private readonly IAirportService _airportService;
        private readonly IataCodeValidator _validator;

        public DistanceCalculationService(IAirportService airportService, IataCodeValidator validator)
        {
            _airportService = airportService;
            _validator = validator;
        }

        public (string FaultMessage, bool IsSuccess, double distance) CalculateDistanceBetweenAirports(string iataCode, string destinationIataCode)
        {
            _validator.ValidateAndThrow(iataCode);
            _validator.ValidateAndThrow(destinationIataCode);
            
            var firstAirportInfo = _airportService.GetAirportInfo(iataCode.ToUpperInvariant());
            if (firstAirportInfo.IsSuccess == false)
            {
                return ($"Unable to receive information about airport with code {iataCode}. Error: {firstAirportInfo.FaultMessage}", false, -1);
            }
            var secondAirportInfo = _airportService.GetAirportInfo(destinationIataCode.ToUpperInvariant());
            if (secondAirportInfo.IsSuccess == false)
            {
                return ($"Unable to receive information about airport with code {destinationIataCode}. Error: {secondAirportInfo.FaultMessage}", false, -1);
            }
            var firstAirportCoordinates = new GeoCoordinate(firstAirportInfo.Value.Location.Latitude, firstAirportInfo.Value.Location.Longitude);
            var secondAirportCoordinates = new GeoCoordinate(secondAirportInfo.Value.Location.Latitude, secondAirportInfo.Value.Location.Longitude);
            var distanceInMeters = firstAirportCoordinates.GetDistanceTo(secondAirportCoordinates);
            return (null, true, distanceInMeters * 0.00062);
        }
    }
}