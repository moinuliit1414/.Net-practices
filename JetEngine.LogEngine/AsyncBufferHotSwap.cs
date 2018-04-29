using System;
using System.Linq;
using System.Collections.Generic;
using log4net.Core;
using JetEngine.LogEngine.Interfaces;

namespace JetEngine.LogEngine
{
    public class AsyncBufferHotSwap : IAsyncBuffer<LoggingEvent>
    {
        #region Fields

        private readonly object _syncRoot = new object();
        private readonly IGrowLimiter _growLimiter;
        private volatile List<LoggingEvent> _currentList = new List<LoggingEvent>();
        private volatile List<LoggingEvent> _shadowList = new List<LoggingEvent>();

        #endregion Fields


        public AsyncBufferHotSwap(IGrowLimiter growLimiter)
        {
            _growLimiter = growLimiter;
        }


        #region IAsyncBuffer

        public event EventHandler BufferReady;

        public void Add(LoggingEvent item, bool force)
        {
            var notifyRequired = false;
            lock (_syncRoot)
            {
                if (!force &&
                    _growLimiter != null &&
                    _growLimiter.CheckLimitReached(_currentList.Count + _shadowList.Count))
                {
                    return;
                }
                if (_currentList.Count == 0)
                {
                    // first event appears, so we need to notify that we have events
                    notifyRequired = true;
                }
                _currentList.Add(item);
            }
            if (notifyRequired)
            {
                OnBufferReady();
            }
        }

        public IEnumerable<LoggingEvent> Acquire(bool force)
        {
            lock (_syncRoot)
            {
                // Hot Swap
                var tmp = _currentList;
                _currentList = _shadowList;
                _shadowList = tmp;
            }
            return _shadowList;
        }

        public void Release()
        {
            _shadowList.Clear();
        }

        #endregion IAsyncBuffer


        #region Private

        protected virtual void OnBufferReady()
        {
            var handler = BufferReady;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion Private
    }
}
