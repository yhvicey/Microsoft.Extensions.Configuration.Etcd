using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Configuration.Etcd
{
    internal class NullChangeToken : IChangeToken
    {
        public static NullChangeToken Instance { get; } = new NullChangeToken();

        private NullChangeToken()
        {
        }

        public bool HasChanged => false;

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return NullDisposable.Instance;
        }
    }
}