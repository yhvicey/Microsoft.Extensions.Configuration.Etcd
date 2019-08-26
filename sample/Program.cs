using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Etcd.Refreshers;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Configuration.Etcd.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddEtcd("127.0.0.1", 2379,
                        configureClientOptions: clientOptions =>
                        {
                            //clientOptions.UserName = "";
                            //clientOptions.Password = "";
                        },
                        configureOptions: options =>
                        {
                            //options.Prefix = "ETCD_"; // Prefix for matching configuration keys for etcd. Default is "ETCD_".
                            //options.RemovePrefix = true; // Should the lib remove the prefix from the key when used for querying or not. Default is true.
                            //options.UseLocalCache = true; // Should the lib cache fetched value to local cache or not. Default is true.
                            options.Refreshers.Add(EtcdConfigurationManualRefresher.Instance);
                            options.Refreshers.Add(new EtcdConfigurationMonitorRefresher("KeyToMonitor", (oldValue, newValue) =>
                            {
                                if (oldValue != newValue)
                                {
                                    Console.WriteLine($"Monitored value changed from {oldValue} to {newValue}, refreshing etcd configuration.");
                                    return true;
                                }
                                return false;
                            }));
                        });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
