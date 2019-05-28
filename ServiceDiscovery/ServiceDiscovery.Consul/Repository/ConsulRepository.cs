using Consul;
using Newtonsoft.Json;
using ServiceDiscovery.Consul.Options;
using ServiceRegister.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceDiscovery.Consul.Repository
{
    public class ConsulRepository : IRepository<string>
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

        public virtual void Add(string route)
        {
            var kv_value = Encoding.Default.GetBytes(route);

            // put consul_kv
            KVPair kVPair = new KVPair(ConsulKVKey) { Key = ConsulKVKey, Value = kv_value };
            _consulClient.KV.Put(kVPair);
        }

        public virtual string Get(string key)
        {
            var byteValue = _consulClient.KV.Get(key).Result.Response.Value;
            var stringValue = Encoding.ASCII.GetString(byteValue);

            return stringValue;
        }
    }
}
