using System;

namespace Microsoft.Extensions.Configuration.Etcd.Refreshers
{
    public class EtcdConfigurationMonitorRefresher : Refresher
    {
        private readonly Func<string?, string?, bool> callback;
        private readonly string keyToMonitor;

        public EtcdConfigurationMonitorRefresher(string keyToMonitor, Func<string?, string?, bool>? callback = null)
        {
            this.keyToMonitor = keyToMonitor;
            this.callback = callback ?? ((string? oldValue, string? newValue) =>
            {
                return oldValue != newValue;
            });
        }

        internal override EtcdConfigurationChangeToken ProduceChangeToken(IEtcdClient client)
        {
            var changeToken = base.ProduceChangeToken(client);

            client.Watch(keyToMonitor, (oldValue, newValue) =>
            {
                if (callback(oldValue, newValue))
                {
                    Refresh();
                }
            });

            return changeToken;
        }
    }
}
