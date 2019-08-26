using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdOptions
    {
        public delegate void ExceptionHandler(Exception ex);

        public Func<IChangeToken>? ChangeTokenProvider { get; set; }

        public string Prefix { get; set; } = "ETCD_";

        public bool RemovePrefix { get; set; } = true;

        public bool UseLocalCache { get; set; } = true;

        public event ExceptionHandler OnException;

        internal void InvokeOnException(Exception ex)
        {
            OnException?.Invoke(ex);
        }
    }
}
