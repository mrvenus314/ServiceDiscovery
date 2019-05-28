# ServiceDiscovery
基于consul，ocelot的服务注册，服务发现
目前仅实现了利用consulkv来做api的存储


services.AddOcelot(
                config => { config.Key = "Oceolot_Api"; })
                .AddConsul(
                config =>
                {
                    config.Address = "127.0.0.1";
                    config.ServiceId = "service";
                    config.ServiceName = "service";
                    config.Port = 5001;
                    config.ConsulKVKey = "Oceolot_Api";
                    config.Consul = new ConsulOptions() { HttpEndPoint = "http://127.0.0.1:8500" };
                }).AddConsulKVRepository();
                
                
app.UseConsul().UseApiRegister(apiDescriptionGroupCollectionProvider).WithConsulKV();                
