using DCS.Core;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;

namespace DCS.Infrastructure.ExternalServiceProxies.AirportService
{
    public interface IAirportService
    {
        Result<AirportInfo> GetAirportInfo(string iataCode);
    }
}