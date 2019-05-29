# ServiceDiscovery

基于consul，ocelot的服务注册，服务发现
目前仅实现了利用consulkv来做api的存储

## api配置

```csharp
services.AddOcelot(
                config => { config.Key = "Oceolot_Api"; })
                .AddConsul(
                config =>
                {
                    config.Address = "127.0.0.1";
                    config.ServiceId = "serivice";
                    config.ServiceName = "serivice";
                    config.Port = 5001;
                    config.ConsulKVKey = "Oceolot_Api";
                    config.Consul = new ConsulOptions() { HttpEndPoint = "http://127.0.0.1:8500" };
                }).AddConsulKVRepository();


app.UseConsul().UseApiRegister(apiDescriptionGroupCollectionProvider).WithConsulKV();
```
## ocelot网关配置

```csharp
services.AddOcelot(Configuration)
            .AddConsul()
            .AddConfigStoredInConsul();
```

## 支持ocelot权限

```csharp
[HttpGet]
[PermissionFilter(Permission = "ewip.salary.CalculateSalary")]
public async Task<string> Get()
{
    return $"API001:{DateTime.Now.ToString()}";
}
```
