using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using log4net.Core;
using JetEngine.LogEngine.Interfaces;

namespace JetEngine.LogEngine.AsyncBuffers
{
    public class AsyncBufferConcurrent : IAsyncBuffer<LoggingEvent>
    {
        #region Fields

        private readonly IGrowLimiter _growLimiter;
        private readonly ConcurrentQueue<LoggingEvent> _queue = new ConcurrentQueue<LoggingEvent>();

        #endregion Fields


        public AsyncBufferConcurrent(IGrowLimiter growLimiter)
        {
            _growLimiter = growLimiter;
        }


        #region IAsyncBuffer

        public event EventHandler BufferReady;

        public void Add(LoggingEvent item, bool force)
        {
            var notifyRequired = false;
            if (!force &&
                _growLimiter != null &&
                _growLimiter.CheckLimitReached(_queue.Count))
            {
                return;
            }
            if (_queue.Count == 0)
            {
                // first event appears, so we need to notify that we have events
                notifyRequired = true;
            }
            _queue.Enqueue(item);
            if (notifyRequired)
            {
                OnBufferReady();
            }
        }

        public IEnumerable<LoggingEvent> Acquire(bool force)
        {
            var buffer = new List<LoggingEvent>();
            var logEvent = default(LoggingEvent);
            while (_queue.TryDequeue(out logEvent))
            {
                buffer.Add(logEvent);
            }
            return buffer;
        }

        public void Release()
        {
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
