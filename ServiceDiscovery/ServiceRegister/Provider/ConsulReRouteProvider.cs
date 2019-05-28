using Consul;
using Ocelot.Configuration.File;
using ServiceRegister.Options;
using ServiceRegister.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Provider
{
    public class ConsulReRouteProvider : IRouteProvider
    {
        private readonly IConsulClient _consulClient;
        private readonly IRepository<string> _repository;
        private readonly ServiceDiscoveryOptions _serviceDiscoveryOptions;

        public string ServiceName { get; set; }
        public string ConsulKey { get; set; }

        public ConsulReRouteProvider(IConsulClient consulClient,
            IRepository repository,
            ServiceDiscoveryOptions serviceDiscoveryOptions)
        {
            _consulClient = consulClient;
            _repository = repository;
            _serviceDiscoveryOptions = serviceDiscoveryOptions;

            ServiceName = _serviceDiscoveryOptions.ServiceName;
        }

        public virtual FileConfiguration GenerateReRoutes(List<ApiDescriptionOptions> apis)
        {
            List<FileReRoute> ocelotReRoutes = new List<FileReRoute>();
            var fileConfig = _repository.Get(_serviceDiscoveryOptions.ConsulKVKey);

            foreach (var api in apis)
            {
                // 排除Ocelot的接口
                if (!api.ActionDescriptor.DisplayName.StartsWith("Ocelot"))
                {
                    // 删除重复接口
                    fileConfig.ReRoutes.RemoveAll(l => l.UpstreamPathTemplate == $"/{ServiceName}/{api.RelativePath}");

                    Dictionary<string, string> permissionDic = new Dictionary<string, string>();
                    permissionDic.Add("permission", api.Permission);

                    ocelotReRoutes.Add(new FileReRoute()
                    {
                        DownstreamPathTemplate = $"/{api.RelativePath}",
                        DownstreamScheme = "http",
                        UpstreamPathTemplate = $"/{ServiceName}/{api.RelativePath}",
                        UpstreamHttpMethod = new List<string>() { api.HttpMethod },
                        ServiceName = ServiceName,
                        AuthenticationOptions = new FileAuthenticationOptions() { AuthenticationProviderKey = "IdentityBearer" },
                        RouteClaimsRequirement = permissionDic
                    });
                }
            }

            fileConfig.ReRoutes.AddRange(ocelotReRoutes);
            return fileConfig;
        }

        public virtual void UpdateConsulKV(List<ApiDescriptionOptions> apis)
        {
            var fileConfig = GenerateReRoutes(apis);
            _repository.Add(fileConfig);
        }
    }
}
