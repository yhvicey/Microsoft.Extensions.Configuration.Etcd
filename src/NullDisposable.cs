using System;

namespace Microsoft.Extensions.Configuration.Etcd
{
    internal class NullDisposable : IDisposable
    {
        public static NullDisposable Instance { get; } = new NullDisposable();

        private NullDisposable()
        {
        }

        public void Dispose()
        {
        }
    }
}