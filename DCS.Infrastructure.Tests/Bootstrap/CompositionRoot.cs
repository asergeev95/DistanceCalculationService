using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DCS.Tests.Bootstrap
{
    internal static class CompositionRoot
    {
        public static IServiceProvider ServiceProvider => ServiceCollection.BuildServiceProvider();
        public static IServiceCollection ServiceCollection => LazyContainer.InternalServiceCollection;

        [UsedImplicitly]
        private class LazyContainer
        {
            static LazyContainer()
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                InternalServiceCollection = new ServiceCollection()
                    .RegisterServices(config);
            }

            internal static readonly IServiceCollection InternalServiceCollection;
        }
    }
}