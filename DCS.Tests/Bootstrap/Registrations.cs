using DCS.ApplicationService.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DCS.Tests.Bootstrap
{
    public static class Registrations
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IataCodeValidator>();
            return services;
        }
    }
}