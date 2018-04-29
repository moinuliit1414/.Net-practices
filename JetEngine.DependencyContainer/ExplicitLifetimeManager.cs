using System;
using System.Collections.Generic;
using System.Text;
using Unity.Lifetime;

namespace JetEngine.DependencyContainer
{
    public class ExplicitLifetimeManager : SynchronizedLifetimeManager, IDisposable
    {
        private readonly bool _isAutoDisposable;
        private object _value;
        private bool _disposed = false;

        public ExplicitLifetimeManager() : this(false) { }

        public ExplicitLifetimeManager(bool isAutoDisposable)
        {
            _isAutoDisposable = isAutoDisposable;
        }

        protected object SynchronizedGetValue()
        {
            return _value;
        }

        protected void SynchronizedSetValue(object newValue)
        {
            _value = newValue;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsAutoDisposable
        {
            get
            {
                return _isAutoDisposable;
            }
        }

        public void RemoveValue()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (_value != null)
            {
                if (IsAutoDisposable && _value is IDisposable && disposing)
                {
                    ((IDisposable)_value).Dispose();
                }
                _value = null;
            }

            _disposed = true;
        }

        protected override object SynchronizedGetValue(ILifetimeContainer container)
        {
            return _value;
        }

        protected override void SynchronizedSetValue(object newValue, ILifetimeContainer container)
        {
            throw new NotImplementedException();
        }

        protected override LifetimeManager OnCreateLifetimeManager()
        {
            throw new NotImplementedException();
        }
    }
}
