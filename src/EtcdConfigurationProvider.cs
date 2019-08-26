using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly IList<IDisposable> refresherRegistrations = new List<IDisposable>();
        private readonly IEtcdClient client;
        private readonly EtcdOptions options;

        public EtcdConfigurationProvider(EtcdOptions options, IEtcdClient client)
        {
            this.options = options;
            this.client = client;
            if (options.Refreshers.Any())
            {
                // Register refreshers
                foreach (var refresher in options.Refreshers)
                {
                    refresherRegistrations.Add(ChangeToken.OnChange(
                        () => refresher.ProduceChangeToken(client),
                        () => Data.Clear()));
                }
            }
        }

        public void Dispose()
        {
            foreach (var refresherRegistration in refresherRegistrations)
            {
                refresherRegistration.Dispose();
            }
            client.Dispose();
        }

        public override bool TryGet(string key, out string? value)
        {
            value = default;

            // Filter keys
            if (!key.StartsWith(options.Prefix))
            {
                return false;
            }

            // Process key
            if (options.RemovePrefix)
            {
                key = key.Substring(options.Prefix.Length);
            }

            // Try get from cache
            if (options.UseLocalCache && Data.ContainsKey(key))
            {
                value = Data[key];
                return true;
            }

            // Try get from remote
            try
            {
                if (client.TryGet(key, out value))
                {
                    if (options.UseLocalCache)
                    {
                        Data[key] = value;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                options.InvokeOnException(ex);
                return false;
            }
        }

        public override void Set(string key, string value)
        {
            Data[key] = value;
        }
    }
}
