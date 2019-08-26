using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationManualRefresher : IChangeToken
    {
        public static EtcdConfigurationManualRefresher Instance { get; } = new EtcdConfigurationManualRefresher();

        private Registration? registration;

        private EtcdConfigurationManualRefresher()
        {
        }

        public bool ActiveChangeCallbacks => false;

        public bool HasChanged => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            registration = new Registration(this, callback, state);
            return registration;
        }

        public void Refresh()
        {
            registration?.Refresh();
        }

        internal void Reset()
        {
            registration = null;
        }

        private class Registration : IDisposable
        {
            private readonly EtcdConfigurationManualRefresher refresher;
            private readonly Action<object> callback;
            private readonly object state;

            public Registration(EtcdConfigurationManualRefresher refresher, Action<object> callback, object state)
            {
                this.refresher = refresher;
                this.callback = callback;
                this.state = state;
            }

            public void Dispose()
            {
                refresher.Reset();
            }

            internal void Refresh()
            {
                callback?.Invoke(state);
            }
        }
    }
}
