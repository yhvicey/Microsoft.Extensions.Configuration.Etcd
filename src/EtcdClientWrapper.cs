using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnet_etcd;
using Grpc.Core;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdClientOptions
    {
        public EtcdClientOptions(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; }

        public int Port { get; }

        public string CaCert { get; set; } = string.Empty;

        public string ClientCert { get; set; } = string.Empty;

        public string ClientKey { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PublicRootCa { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }

    public class EtcdClientWrapper : IEtcdClient
    {
        private readonly EtcdClient client;

        public EtcdClientWrapper(EtcdClientOptions options)
        {
            client = new EtcdClient(options.Host, options.Port, options.UserName, options.CaCert, options.ClientCert, options.ClientKey, options.PublicRootCa);
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public bool TryGet(string key, out string? value)
        {
            var resp = client.Get(key);
            if (resp.Count == 0)
            {
                value = default;
                return false;
            }
            value = resp.Kvs.First().Value.ToStringUtf8().Trim();
            return true;
        }

        public void Watch(string key, Action<string?, string?> callback)
        {
            client.Watch(key, resp =>
            {
                if (resp.Events.Any())
                {
                    var evt = resp.Events.First();
                    var value = evt.Kv.Value.ToStringUtf8();
                    callback?.Invoke(previousWatchedValue, value);
                    previousWatchedValue = value;
                }
                else
                {
                    callback?.Invoke(previousWatchedValue, null);
                    previousWatchedValue = null;
                }
            });
        }

        private string? previousWatchedValue = null;
    }
}
