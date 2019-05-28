using ServiceRegister.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Consul.Options
{
    public class ServiceDiscoveryOptions
    {
        public string ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }

        public string ConsulKVKey { get; set; }

        public ConsulOptions Consul { get; set; }
    }

    public class ConsulOptions
    {
        public string HttpEndPoint { get; set; }
    }
}
