using System.Threading.Tasks;
using DCS.Core;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;

namespace DCS.Infrastructure.ExternalServiceProxies.AirportService
{
    public interface IAirportInfoService
    {
        Task<Result<AirportInfo>> GetAirportInfo(string iataCode);
    }
}