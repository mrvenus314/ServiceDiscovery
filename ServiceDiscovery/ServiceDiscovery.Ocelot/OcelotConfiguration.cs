using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Ocelot
{
    public class OcelotConfiguration
    {
        public OcelotGlobalConfiguration GlobalConfiguration { get; set; }

        public List<OcelotReRoute> ReRoutes { get; set; }
    }
}
