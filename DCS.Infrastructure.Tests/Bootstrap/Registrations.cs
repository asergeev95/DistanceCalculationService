﻿using System.Net.Http;
using DCS.Infrastructure.Caching;
using DCS.Infrastructure.ExternalServiceProxies.AirportService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DCS.Tests.Bootstrap
{
    public static class Registrations
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AirportServiceConfiguration>(serviceConfiguration =>
            {
                serviceConfiguration.Url = configuration.GetSection("ConnectedServices:AirportService:Url").Value;
            });
            services.AddSingleton(isp => isp.GetRequiredService<IOptions<AirportServiceConfiguration>>().Value);
            services.AddTransient<IAirportService, AirportServiceProxy>();
            
            services.AddSingleton<HttpClient>();
            services.AddSingleton<ICacheService, NullObjectCacheAdapter>();
            return services;
        }
    }
}