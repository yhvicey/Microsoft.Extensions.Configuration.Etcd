using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration.Etcd.Refreshers;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdOptions
    {
        public delegate void ExceptionHandler(Exception ex);

        public IList<Refresher> Refreshers { get; set; } = new List<Refresher>();

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
