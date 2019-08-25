using System;
using System.Linq;
using dotnet_etcd;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly IDisposable changeTokenRegistration;
        private readonly EtcdClient client;
        private readonly EtcdOptions options;

        public EtcdConfigurationProvider(EtcdOptions options)
        {
            this.options = options;
            client = new EtcdClient(options.Host, options.Port, options.UserName, options.CaCert, options.ClientCert, options.ClientKey, options.PublicRootCa);
            // Register change token provider
            var changeTokenProvider = options.ChangeTokenProvider ?? new Func<IChangeToken>(() => NullChangeToken.Instance);
            changeTokenRegistration = ChangeToken.OnChange(
                options.ChangeTokenProvider ?? (() => NullChangeToken.Instance),
                () =>
                {
                    Data.Clear();
                });
        }

        public void Dispose()
        {
            changeTokenRegistration.Dispose();
            client.Dispose();
        }

        public override bool TryGet(string key, out string? value)
        {
            value = default;
            if (!key.StartsWith(options.Prefix))
            {
                return false;
            }
            if (options.RemovePrefix)
            {
                key = key.Substring(options.Prefix.Length);
            }
            if (Data.TryGetValue(key, out value))
            {
                return true;
            }
            try
            {
                var resp = client.Get(key);
                if (resp.Count == 0)
                {
                    return false;
                }
                value = resp.Kvs.First().Value.ToStringUtf8().Trim();
                Data[key] = value;
                return true;
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
