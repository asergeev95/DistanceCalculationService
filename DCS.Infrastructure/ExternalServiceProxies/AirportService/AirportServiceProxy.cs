using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DCS.Core;
using DCS.Infrastructure.ExternalServiceProxies.AirportService.Contracts;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Serilog;

namespace DCS.Infrastructure.ExternalServiceProxies.AirportService
{
    public class AirportServiceProxy : IAirportService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cacheService;
        private readonly AirportServiceConfiguration _serviceConfiguration;

        public AirportServiceProxy(HttpClient httpClient, IDistributedCache cacheService, AirportServiceConfiguration serviceConfiguration)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _serviceConfiguration = serviceConfiguration;
        }

        public async Task<Result<Contracts.AirportInfo>> GetAirportInfo(string iataCode)
        {
            var encodedAirportInfo = await _cacheService.GetAsync(iataCode);
            if (encodedAirportInfo == null)
            {
                var (url, method) = BuildGetAirportInfoUrl(iataCode);
                var (isSuccess, faultMessage, value) = GetResponseAsync<AirportInfo>(url, method).Result;
                
                
                
                switch (isSuccess)
                {
                    case false:
                        return new Result<Contracts.AirportInfo>
                        {
                            Value = null,
                            FaultMessage = faultMessage,
                            IsSuccess = false
                        };
                    default:
                    {
                        var airportInfo = Map(value);
                        encodedAirportInfo = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(airportInfo));
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(20))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                        await _cacheService.SetAsync(iataCode, encodedAirportInfo, options);
                        return new Result<Contracts.AirportInfo>
                        {
                            Value = airportInfo,
                            FaultMessage = null,
                            IsSuccess = true
                        };
                    }
                }
            }

            var jsonAirportInfo = Encoding.UTF8.GetString(encodedAirportInfo);
            return new Result<Contracts.AirportInfo>
            {
                Value = JsonConvert.DeserializeObject<Contracts.AirportInfo>(jsonAirportInfo),
                FaultMessage = null,
                IsSuccess = true
            };
        }

        private async Task<(bool IsSuccess, string FaultMessage, T Value)> GetResponseAsync<T>(string url, HttpMethod method, string content = null)
            where T : class
        {
            try
            {
                var httpRequest = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(url),
                    Content = new StringContent(content ?? string.Empty, Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return (true, null, JsonConvert.DeserializeObject<T>(json));
                }

                var body = await response.Content.ReadAsStringAsync();
                return (false, body, null);
            }
            catch (Exception e)
            {
                Log.Error("An error occured during airport service request, {@exception}", e);
                throw;
            }
        }

        private (string Url, HttpMethod Method) BuildGetAirportInfoUrl(string iataCode)
        {
            return ($"{_serviceConfiguration.Url}/airports/{iataCode}", HttpMethod.Get);
        }

        private static Contracts.AirportInfo Map(AirportInfo response)
        {
            return new()
            {
                City = response.city,
                Country = response.country,
                Hubs = response.hubs,
                Iata = response.iata,
                Location = new Location
                {
                    Latitude = response.location.lat,
                    Longitude = response.location.lon
                },
                Name = response.name,
                Rating = response.rating,
                Type = response.type,
                CityIata = response.city_iata,
                CountryIata = response.country_iata,
                TimezoneRegionName = response.timezone_region_name
            };
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private record AirportInfo
        {
            public string country { get; init; }
            public string city_iata { get; init; }
            public string iata { get; init; }
            public string city { get; init; }
            public string timezone_region_name { get; init; }
            public string country_iata { get; init; }
            public double rating { get; init; }
            public string name { get; init; }
            public Location location { get; init; }
            public string type { get; init; }
            public uint hubs { get; init; }

            [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
            internal class Location
            {
                public double lon { get; init; }
                public double lat { get; init; }
            }
        }
    }
}