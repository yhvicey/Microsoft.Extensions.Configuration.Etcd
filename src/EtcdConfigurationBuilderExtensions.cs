using System;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public static class EtcdConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEtcd(
            this IConfigurationBuilder configurationBuilder,
            string host,
            int port,
            Action<EtcdOptions>? configureOptions = null)
            => configurationBuilder.AddEtcd(host, port, clientOptions => { }, configureOptions);

        public static IConfigurationBuilder AddEtcd(
            this IConfigurationBuilder configurationBuilder,
            string host,
            int port,
            Action<EtcdClientOptions>? configureClientOptions = null,
            Action<EtcdOptions>? configureOptions = null)
        {
            var clientOptions = new EtcdClientOptions(host, port);
            configureClientOptions?.Invoke(clientOptions);

            return configurationBuilder.AddEtcd(new EtcdClientWrapper(clientOptions), configureOptions);
        }

        public static IConfigurationBuilder AddEtcd(
            this IConfigurationBuilder configurationBuilder,
            IEtcdClient client,
            Action<EtcdOptions>? configureOptions = null)
        {
            var options = new EtcdOptions();
            configureOptions?.Invoke(options);

            return configurationBuilder.Add(new EtcdConfigurationSource(options, client));
        }
    }
}
