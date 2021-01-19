using System.Threading.Tasks;
using DCS.Core;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;

namespace DCS.Infrastructure.ExternalServiceProxies.AirportService
{
    public interface IAirportService
    {
        Task<Result<AirportInfo>> GetAirportInfo(string iataCode);
    }
}