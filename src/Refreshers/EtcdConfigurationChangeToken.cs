using Microsoft.Extensions.Primitives;
using System;

namespace Microsoft.Extensions.Configuration.Etcd.Refreshers
{
    public class EtcdConfigurationChangeToken : IChangeToken
    {
        private Registration? registration;

        public bool ActiveChangeCallbacks => false;

        public bool HasChanged => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            registration?.Dispose();
            registration = new Registration(callback, state);
            return registration;
        }

        public void Refresh()
        {
            if (registration != null && registration.Disposed != true)
            {
                registration.Refresh();
            }
        }

        private class Registration : IDisposable
        {
            private readonly Action<object> callback;
            private readonly object state;

            public Registration(Action<object> callback, object state)
            {
                this.callback = callback;
                this.state = state;
            }

            public bool Disposed { get; private set; } = false;

            public void Dispose()
            {
                Disposed = true;
            }

            internal void Refresh()
            {
                if (!Disposed)
                {
                    callback?.Invoke(state);
                }
            }
        }
    }
}
