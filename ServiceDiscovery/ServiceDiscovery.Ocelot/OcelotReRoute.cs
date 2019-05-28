using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Ocelot
{
    public class OcelotReRoute
    {
        public string DownstreamPathTemplate { get; set; }

        public string UpstreamPathTemplate { get; set; }

        public List<string> UpstreamHttpMethod { get; set; }

        public RouteClaimsRequirement RouteClaimsRequirement { get; set; }

        public string ServiceName { get; set; }

        public string DownstreamScheme { get; set; }

        public AuthenticationOptions AuthenticationOptions { get; set; }
    }

    public class RouteClaimsRequirement
    {
        public string Permission { get; set; }
    }

    public class AuthenticationOptions
    {
        public string AuthenticationProviderKey { get; set; }

        public List<string> AllowedScopes { get; set; }
    }
}
