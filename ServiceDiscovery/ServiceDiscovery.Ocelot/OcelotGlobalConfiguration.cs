using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Ocelot
{
    public class OcelotGlobalConfiguration
    {
        public ServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }
    }

    public class ServiceDiscoveryProvider
    {
        public string Host { get; set; }

        public string Port { get; set; }

        public string ConfigurationKey { get; set; }

        public string Type { get; set; }

        public string PollingInterval { get; set; }
    }
}
