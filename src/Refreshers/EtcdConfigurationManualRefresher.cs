namespace Microsoft.Extensions.Configuration.Etcd.Refreshers
{
    public class EtcdConfigurationManualRefresher : Refresher
    {
        public static EtcdConfigurationManualRefresher Instance { get; } = new EtcdConfigurationManualRefresher();

        private EtcdConfigurationManualRefresher()
        {
        }

        public new void Refresh()
            => base.Refresh();
    }
}
