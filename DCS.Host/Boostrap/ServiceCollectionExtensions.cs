using System.Net.Http;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DCS.Host.Boostrap
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AirportServiceConfiguration>(configuration.GetSection("ConnectedServices:AirportService"));
            services.AddSingleton(isp => isp.GetRequiredService<IOptions<AirportServiceConfiguration>>().Value);
            services.AddSingleton<IAirportService, AirportServiceProxy>();
            
            services.AddSingleton<HttpClient>();
            return services;
        }
    }
}