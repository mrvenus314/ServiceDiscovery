using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Ocelot.Options
{
    public class OcelotServiceDiscoveryOptions
    {
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }

        public string DownstreamHost { get; set; }

        public int DownstreamPort { get; set; }
    }
}
