using System.Threading.Tasks;
using DCS.ApplicationService;
using DCS.ApplicationService.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DCS.Host.Controllers
{
    [Route("api/distance")]
    public class DistanceController
    {
        private readonly DistanceCalculationService _calculationService;

        public DistanceController(DistanceCalculationService calculationService)
        {
            _calculationService = calculationService;
        }
        [HttpGet("iataCode&destIataCode")]
        public async Task<AirportsDistanceState> GetDistanceBetweenAirports(string iataCode, string destIataCode)
        {
            var response =  await _calculationService.CalculateDistanceBetweenAirports(iataCode, destIataCode);
            return new AirportsDistanceState
            {
                Distance = response.distance,
                IsSuccess = response.IsSuccess,
                FaultMessage = response.FaultMessage
            };
        }
    }
}