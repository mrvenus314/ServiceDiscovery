using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDiscovery.Consul.Options;
using ServiceDiscovery.Consul.Repository;
using ServiceRegister;
using ServiceRegister.Provider;
using ServiceRegister.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Consul
{
    public static class ConsulExtension
    {
        /// <summary>
        /// 添加consul
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, Action<ServiceDiscoveryOptions> configOptions)
        {
            // 配置consul注册地址            
            ServiceDiscoveryOptions consulOptions = new ServiceDiscoveryOptions();

            configOptions(consulOptions);

            services.AddSingleton(consulOptions);
            

            // 配置consul客户端
            services.AddSingleton<IConsulClient>(client => new ConsulClient(config =>
            {
                var serviceDiscoveryOptions = client.GetRequiredService<ServiceDiscoveryOptions>();

                if (!string.IsNullOrWhiteSpace(serviceDiscoveryOptions.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(serviceDiscoveryOptions.Consul.HttpEndPoint);
                }
            }));

            return services;
        }

        /// <summary>        
        /// 添加consul
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            // 配置consul注册地址
            ServiceDiscoveryOptions consulOptions = new ServiceDiscoveryOptions()
            {
                ServiceId = configurationRoot["ServiceDiscovery:ServiceId"],
                ServiceName = configurationRoot["ServiceDiscovery:ServiceName"],
                Address = configurationRoot["ServiceDiscovery:Address"],
                Port = int.Parse(configurationRoot["ServiceDiscovery:Port"]),
                ConsulKVKey = configurationRoot["ServiceDiscovery:ConsulKVKey"],
                Consul = new ConsulOptions() { HttpEndPoint = configurationRoot["ServiceDiscovery:Consul:HttpEndpoint"] }
            };
            services.AddSingleton(consulOptions);            

            // 配置consul客户端
            services.AddSingleton<IConsulClient>(client => new ConsulClient(config =>
            {
                var serviceDiscoveryOptions = client.GetRequiredService<ServiceDiscoveryOptions>();

                if (!string.IsNullOrWhiteSpace(serviceDiscoveryOptions.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(serviceDiscoveryOptions.Consul.HttpEndPoint);
                }
            }));

            return services;
        }

        public static IServiceCollection AddConsulKVRepository(this IServiceCollection services)
        {
            services.AddSingleton<IRepository<string>, ConsulRepository>();
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            IConsulClient consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            IApplicationLifetime appLife = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var serviceOptions = app.ApplicationServices.GetRequiredService<ServiceDiscoveryOptions>();

            var serviceId = serviceOptions.ServiceId;
            var serviceName = serviceOptions.ServiceName;
            var address = serviceOptions.Address;
            var port = serviceOptions.Port;

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(30),

                // 默认健康检查接口
                HTTP = $"{Uri.UriSchemeHttp}://{address}:{port}/HealthCheck",
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                Address = address,
                ID = serviceId,
                Name = serviceName,
                Port = port
            };

            // 向consul注册服务
            consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            // 当服务停止后向consul发送的请求，注销服务
            appLife.ApplicationStopping.Register(() =>
            {
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            });

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"health check service:{httpCheck.HTTP}");

            app.Map("/HealthCheck", s =>
            {
                s.Run(async context =>
                {
                    await context.Response.WriteAsync("ok");
                });
            });

            return app;
        }

        /// <summary>
        /// 使用Consul_KV
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configurationRoot"></param>
        /// <returns></returns>
        public static IApplicationBuilder WithConsulKV(this IApplicationBuilder app)
        {
            var repository = app.ApplicationServices.GetRequiredService<IRepository<string>>();
            var provider = app.ApplicationServices.GetRequiredService<IRouteProvider<string>>();

            var route = provider.GenerateRoutes(ServiceRegisterExtension.ApiList);
            repository.Add(route);

            return app;
        }
    }
}
