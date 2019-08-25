using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd.Sample
{
    public class EtcdConfigurationRefresher : IChangeToken
    {
        private Registration? registration;

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

        public void Reset()
        {
            registration = null;
        }

        public static EtcdConfigurationRefresher Instance { get; } = new EtcdConfigurationRefresher();

        private class Registration : IDisposable
        {
            private readonly EtcdConfigurationRefresher refresher;
            private readonly Action<object> callback;
            private readonly object state;

            public Registration(EtcdConfigurationRefresher refresher, Action<object> callback, object state)
            {
                this.refresher = refresher;
                this.callback = callback;
                this.state = state;
            }

            public void Dispose()
            {
                refresher.Reset();
            }

            public void Refresh()
            {
                callback?.Invoke(state);
            }
        }
    }
}
