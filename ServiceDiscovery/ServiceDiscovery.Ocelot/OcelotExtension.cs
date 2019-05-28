using Microsoft.Extensions.DependencyInjection;
using ServiceDiscovery.Ocelot.Options;
using ServiceDiscovery.Ocelot.Provider;
using ServiceRegister.Provider;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Ocelot
{
    public static class OcelotExtension
    {
        public static IServiceCollection AddOcelot(this IServiceCollection services, Action<OcelotServiceDiscoveryOptions> action)
        {
            OcelotServiceDiscoveryOptions ocelotServiceDiscoveryOptions = new OcelotServiceDiscoveryOptions();
            action(ocelotServiceDiscoveryOptions);

            services.AddSingleton(ocelotServiceDiscoveryOptions);
            services.AddSingleton<IRouteProvider<string>, OcelotRouteProvider>();

            return services;
        }
    }
}
