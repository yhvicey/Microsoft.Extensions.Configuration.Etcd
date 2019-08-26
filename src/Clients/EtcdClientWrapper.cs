using System;
using System.Collections.Concurrent;
using System.Linq;
using dotnet_etcd;
using Etcdserverpb;
using Google.Protobuf;

namespace Microsoft.Extensions.Configuration.Etcd.Clients
{

    public class EtcdClientWrapper : IEtcdClient
    {
        private readonly EtcdClient client;
        private readonly ConcurrentDictionary<string, string> previousWatchedValues = new ConcurrentDictionary<string, string>();

        public EtcdClientWrapper(EtcdClientWrapperOptions options)
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
            var req = new WatchRequest
            {
                CreateRequest = new WatchCreateRequest
                {
                    Key = ByteString.CopyFromUtf8(key),
                },
            };
            client.Watch(key, resp =>
            {
                previousWatchedValues.TryGetValue(key, out string? previousWatchedValue);
                if (resp.Events.Any())
                {
                    var evt = resp.Events.First();
                    var value = evt.Kv.Value.ToStringUtf8();
                    callback?.Invoke(previousWatchedValue, value);
                    previousWatchedValues[key] = value;
                }
                else
                {
                    callback?.Invoke(previousWatchedValue, null);
                    previousWatchedValues.TryRemove(key, out _);
                }
            });
        }
    }
}
