namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationSource : IConfigurationSource
    {
        private readonly EtcdOptions options;

        public EtcdConfigurationSource(EtcdOptions options)
        {
            this.options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EtcdConfigurationProvider(options);
        }
    }
}
