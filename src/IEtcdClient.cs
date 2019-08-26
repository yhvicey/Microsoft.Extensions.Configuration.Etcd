using System;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public interface IEtcdClient : IDisposable
    {
        bool TryGet(string key, out string? value);

        void Watch(string key, Action<string?, string?> callback);
    }
}
