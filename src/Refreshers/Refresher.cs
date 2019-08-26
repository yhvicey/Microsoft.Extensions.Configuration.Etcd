namespace Microsoft.Extensions.Configuration.Etcd.Refreshers
{
    public class Refresher
    {
        private EtcdConfigurationChangeToken? changeToken = null;

        public void Refresh()
        {
            changeToken?.Refresh();
        }

        internal virtual EtcdConfigurationChangeToken ProduceChangeToken(IEtcdClient client)
        {
            changeToken = new EtcdConfigurationChangeToken();
            return changeToken;
        }
    }
}
