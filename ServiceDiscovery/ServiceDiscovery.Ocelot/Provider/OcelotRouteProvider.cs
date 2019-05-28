using Newtonsoft.Json;
using Ocelot.Configuration.File;
using ServiceDiscovery.Ocelot.Options;
using ServiceRegister.Options;
using ServiceRegister.Provider;
using ServiceRegister.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Ocelot.Provider
{
    public class OcelotRouteProvider: IRouteProvider<string>
    {
        private readonly IRepository<string> _repository;
        private readonly OcelotServiceDiscoveryOptions _serviceDiscoveryOptions;

        public string ServiceName { get; set; }

        public OcelotRouteProvider(
            IRepository<string> repository,
            OcelotServiceDiscoveryOptions serviceDiscoveryOptions)
        {
            this._repository = repository;
            this._serviceDiscoveryOptions = serviceDiscoveryOptions;
        }

        public virtual string GenerateRoutes(List<ApiDescriptionOptions> apis)
        {
            List<FileReRoute> ocelotReRoutes = new List<FileReRoute>();            
            var fileConfigJson = _repository.Get(_serviceDiscoveryOptions.Key);
            var fileConfig = JsonConvert.DeserializeObject<FileConfiguration>(fileConfigJson);

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
            var json = JsonConvert.SerializeObject(fileConfig, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return json;
        }
    }
}
