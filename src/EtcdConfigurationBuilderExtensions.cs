using System;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public static class EtcdConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEtcd(
            this IConfigurationBuilder configurationBuilder,
            string host,
            int port)
            => configurationBuilder.AddEtcd(host, port, options => { });

        public static IConfigurationBuilder AddEtcd(
            this IConfigurationBuilder configurationBuilder,
            string host,
            int port,
            Action<EtcdOptions>? configureOptions = null)
        {
            var options = new EtcdOptions(host, port);
            configureOptions?.Invoke(options);
            configurationBuilder.Add(new EtcdConfigurationSource(options));

            return configurationBuilder;
        }
    }
}
