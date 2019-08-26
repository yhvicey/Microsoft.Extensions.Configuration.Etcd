namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationSource : IConfigurationSource
    {
        private readonly EtcdOptions options;
        private readonly IEtcdClient client;

        public EtcdConfigurationSource(EtcdOptions options, IEtcdClient client)
        {
            this.options = options;
            this.client = client;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EtcdConfigurationProvider(options, client);
        }
    }
}
