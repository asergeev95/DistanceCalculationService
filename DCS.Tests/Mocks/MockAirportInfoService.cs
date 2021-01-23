using System.Threading.Tasks;
using DCS.Core;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;

namespace DCS.Tests.Mocks
{
    public class MockAirportInfoService : IAirportInfoService
    {
        public static AirportInfo AirportInfoValueToReturn = new();
        public static bool IsSuccess;
        public static string FaultMessage;

        public static void Recycle()
        {
            AirportInfoValueToReturn = new AirportInfo();
            IsSuccess = true;
            FaultMessage = null;
        }
        public Task<Result<AirportInfo>> GetAirportInfo(string iataCode)
        {

            return Task.FromResult(new Result<AirportInfo>()
            {
                Value = AirportInfoValueToReturn,
                FaultMessage = FaultMessage,
                IsSuccess = IsSuccess
            });
        }
    }
}