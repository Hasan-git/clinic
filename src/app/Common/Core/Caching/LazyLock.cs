using System;

namespace Clinic.Common.Core.Caching
{
    public sealed class LazyLock
    {
        private volatile bool _got;
        private object _value;

        public TValue Get<TValue>(Func<TValue> activator)
        {
            if (!_got || _value == null)
            {
                lock (this)
                {
                    if (!_got || _value == null)
                    {
                        _value = activator();
                        _got = true;
                    }
                }
            }

            return (TValue)_value;
        }
    }
}
