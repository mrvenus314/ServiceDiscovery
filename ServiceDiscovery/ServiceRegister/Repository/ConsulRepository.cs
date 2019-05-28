using Consul;
using Newtonsoft.Json;
using Ocelot.Configuration.File;
using ServiceRegister.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Repository
{
    public class ConsulRepository : IRepository
    {
        private readonly IConsulClient _consulClient;
        private readonly ServiceDiscoveryOptions _serviceDiscoveryOptions;

        public string ConsulKVKey { get; set; }

        public ConsulRepository(IConsulClient consulClient,
            ServiceDiscoveryOptions serviceDiscoveryOptions)
        {
            _consulClient = consulClient;
            _serviceDiscoveryOptions = serviceDiscoveryOptions;

            ConsulKVKey = _serviceDiscoveryOptions.ConsulKVKey;
        }

        public void Add(FileConfiguration fileConfiguration)
        {
            // 序列化
            var json = JsonConvert.SerializeObject(fileConfiguration, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var kv_value = Encoding.Default.GetBytes(json);

            // put consul_kv
            KVPair kVPair = new KVPair(ConsulKVKey) { Key = ConsulKVKey, Value = kv_value };
            _consulClient.KV.Put(kVPair);
        }

        public FileConfiguration Get(string key)
        {
            var byteValue = _consulClient.KV.Get(key).Result.Response.Value;
            var stringValue = Encoding.ASCII.GetString(byteValue);

            // 反序列化
            FileConfiguration fileConfiguration = JsonConvert.DeserializeObject<FileConfiguration>(stringValue);

            return fileConfiguration;
        }
    }
}
