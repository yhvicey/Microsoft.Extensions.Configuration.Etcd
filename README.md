# Microsoft.Extensions.Configuration.Etcd

Etcd as a configuration source for Asp.Net Core applications.

## Usage

```cs
// Register your etcd endpoint to the process of building your host in Program.cs
...
// AspNet Core 3.0
.ConfigureAppConfiguration(config =>
{
    config.AddEtcd("127.0.0.1", 2379,
        configureClientOptions: clientOptions =>
        {
            //clientOptions.UserName = ...; // Auth options
            //clientOptions.Password = ...; // Auth options
            //...
        },
        configureOptions: options=>
        {
            //options.Prefix = "ETCD_"; // Prefix for matching configuration keys for etcd. Default is "ETCD_".
            //options.RemovePrefix = true; // Should the lib remove the prefix from the key when used for querying or not. Default is true.
            //options.UseLocalCache = true; // Should the lib cache fetched value to local cache or not. Default is true.
        });
})
...
```

## Note

Values fetched from cluster will be cached to local memory. To refresh them, provide a `IChangeToken` producer to the options to refresh the cache. A default manual refresher can be used as below:

```cs
// Program.cs
config.AddEtcd("127.0.0.1", 2379,
    configureOptions: options=>
    {
        options.ChangeTokenProvider = () => EtcdConfigurationManualRefresher.Instance;
    });
```

Then invoke `EtcdConfigurationManualRefresher.Instance.Refresh()` whenever you want to clear all local cache:

```cs
// ValuesController.cs
[HttpGet("reset")]
public void Reset()
{
    EtcdConfigurationManualRefresher.Instance.Refresh();
}
```
