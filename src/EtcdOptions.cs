using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdOptions
    {
        public delegate void ExceptionHandler(Exception ex);

        public EtcdOptions(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; }

        public int Port { get; }

        public string CaCert { get; set; } = string.Empty;

        public Func<IChangeToken>? ChangeTokenProvider { get; set; }

        public string ClientCert { get; set; } = string.Empty;

        public string ClientKey { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Prefix { get; set; } = "ETCD_";

        public string PublicRootCa { get; set; } = string.Empty;

        public bool RemovePrefix { get; set; } = true;

        public string UserName { get; set; } = string.Empty;

        public event ExceptionHandler OnException;

        internal void InvokeOnException(Exception ex)
        {
            OnException?.Invoke(ex);
        }
    }
}
