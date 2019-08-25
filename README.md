# Microsoft.Extensions.Configuration.Etcd

Etcd as a configuration source for Asp.Net Core applications.

## Usage

```cs
// Register your etcd endpoint to the process of building your host in Program.cs
...
// AspNet Core 3.0
.ConfigureAppConfiguration(config =>
{
    config.AddEtcd("127.0.0.1", 2379, options=>
    {
        //options.Prefix = "ETCD_"; // Prefix for matching configuration keys for etcd. Default is "ETCD_".
        //options.RemovePrefix = true; // Should the lib remove the prefix from the key when used for querying. Default is true.
        //options.UserName = ...; // Auth options
        //options.Password = ...; // Auth options
    });
})
...
```

## Note

Values fetched from cluster will be cached to local memory. To refresh them, provide a `IChangeToken` producer to the options to refresh the cache. Sample implementation:

```cs
// EtcdConfigurationRefresher.cs
public class EtcdConfigurationRefresher : IChangeToken
{
    private Registration? registration;

    public bool ActiveChangeCallbacks => false;

    public bool HasChanged => false;

    public IDisposable RegisterChangeCallback(Action<object> callback, object state)
    {
        registration = new Registration(this, callback, state);
        return registration;
    }

    public void Refresh()
    {
        registration?.Refresh();
    }

    public void Reset()
    {
        registration = null;
    }

    public static EtcdConfigurationRefresher Instance { get; } = new EtcdConfigurationRefresher();

    private class Registration : IDisposable
    {
        private readonly EtcdConfigurationRefresher refresher;
        private readonly Action<object> callback;
        private readonly object state;

        public Registration(EtcdConfigurationRefresher refresher, Action<object> callback, object state)
        {
            this.refresher = refresher;
            this.callback = callback;
            this.state = state;
        }

        public void Dispose()
        {
            refresher.Reset();
        }

        public void Refresh()
        {
            callback?.Invoke(state);
        }
    }
}

// Program.cs
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            config.AddEtcd("127.0.0.1", 2379, options=>
            {
                options.ChangeTokenProvider = () => EtcdConfigurationRefresher.Instance;
            });
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

// ValuesController.cs
[HttpGet("reset")]
public void Reset()
{
    EtcdConfigurationRefresher.Instance.Refresh();
}
```

Calling `EtcdConfigurationRefresher.Instance.Refresh()` will clear all local cache.
