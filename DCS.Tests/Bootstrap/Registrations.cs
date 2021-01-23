using DCS.ApplicationService;
using DCS.ApplicationService.Validation;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using DCS.Tests.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DCS.Tests.Bootstrap
{
    public static class Registrations
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IataCodeValidator>();
            services.AddSingleton<IAirportInfoService, MockAirportInfoService>();
            services.AddSingleton<DistanceCalculationService>();
            return services;
        }
    }
}