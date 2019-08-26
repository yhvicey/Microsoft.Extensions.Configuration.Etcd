namespace Microsoft.Extensions.Configuration.Etcd.Clients
{
    public class EtcdClientWrapperOptions
    {
        public EtcdClientWrapperOptions(string host, int port)
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
}
