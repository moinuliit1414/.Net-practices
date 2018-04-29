using System;
using System.Globalization;
using System.Threading;
using log4net.Appender;
using log4net.Core;
using JetEngine.LogEngine.Interfaces;

namespace JetEngine.LogEngine.Appenders
{
    /// <summary>
    /// Asynchronous appender skeleton
    /// </summary>
    public abstract class AsyncAppenderBase : ForwardingAppender
    {
        private readonly object _syncRoot = new object();
        private readonly AutoResetEvent _flushEvent = new AutoResetEvent(false);
        private readonly LoggingEvent _stopEvent;
        private IAsyncBuffer<LoggingEvent> _asyncBuffer;
        private Thread _thread;
        private bool _isActive;
        private bool _isStopRequest;
        private bool _isStopReceived;


        protected AsyncAppenderBase()
        {
            _stopEvent = new LoggingEvent(GetType(), null, null, Level.Emergency, "STOP", null);
            Fix = FixFlags.Message | FixFlags.ThreadName | FixFlags.Exception;
            base.ErrorHandler = new AsyncErrorHandler(this);
        }


        #region Properties

        public FixFlags Fix { get; set; }

        #endregion Properties


        #region Protected

        protected virtual void SendAsync(
            LoggingEvent loggingEvent,
            bool force)
        {
            var buffer = _asyncBuffer;
            if (!_isActive || buffer == null)
            {
                return;
            }
            buffer.Add(loggingEvent, force);
        }

        protected void NotifyBufferReady()
        {
            if (!_isActive)
            {
                return;
            }
            _flushEvent.Set();
        }

        #endregion Protected


        #region Abstract

        protected abstract IAsyncBuffer<LoggingEvent> CreateAsyncBuffer();

        #endregion Abstract


        #region Private

        private void Start()
        {
            lock (_syncRoot)
            {
                if (_isActive)
                {
                    return;
                }

                var buffer = CreateAsyncBuffer();
                buffer.BufferReady += (s, e) => NotifyBufferReady();
                _asyncBuffer = buffer;

                _isStopRequest = false;
                _isStopReceived = false;
                _isActive = true;
                _thread = CreateThread(FlushThreadProc);
                _thread.Start();
            }
        }

        private void Stop()
        {
            lock (_syncRoot)
            {
                if (!_isActive)
                {
                    return;
                }

                _isStopRequest = true;
                SendAsync(_stopEvent, true);

                _flushEvent.Set();
                _thread.Join();

                _asyncBuffer = null;
                _thread = null;
                _isStopRequest = false;
                _isStopReceived = false;
                _isActive = false;
            }
        }

        private Thread CreateThread(ThreadStart threadProc)
        {
            return new Thread(threadProc)
            {
                Name = Name,
                IsBackground = true,
                Priority = ThreadPriority.Lowest,
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void FlushThreadProc()
        {
            while (true)
            {
                try
                {
                    FlushEvents(_isStopRequest);
                    if (_isStopRequest && _isStopReceived)
                    {
                        return;
                    }
                    if (!_isStopRequest)
                    {
                        _flushEvent.WaitOne();
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.Error("FlushThread error", ex);
                    return;
                }
            }
        }

        private void FlushEvents(bool isStopRequest)
        {
            var buffer = _asyncBuffer;
            if (buffer == null)
            {
                // no buffer => do nothing
                return;
            }
            var logEvents = buffer.Acquire(isStopRequest);
            if (logEvents == null)
            {
                // unsuccessful aquire => do nothing
                return;
            }
            try
            {
                foreach (var loggingEvent in logEvents)
                {
                    if (loggingEvent == _stopEvent)
                    {
                        _isStopReceived = true;
                        continue;
                    }
                    base.Append(loggingEvent);
                }
            }
            finally
            {
                buffer.Release();
            }
        }

        #endregion Private


        #region ForwardingAppender

        protected override void OnClose()
        {
            Stop();
            base.OnClose();
            _flushEvent.Dispose();
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            if (loggingEvents == null)
            {
                return;
            }
            foreach (var logEvent in loggingEvents)
            {
                Append(logEvent);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent != null)
            {
                loggingEvent.Fix = Fix;
            }
            SendAsync(loggingEvent, false);
        }

        #endregion ForwardingAppender

        #region IOptionHandler

        public override void ActivateOptions()
        {
            Stop();
            base.ActivateOptions();
            Start();
        }

        #endregion IOptionHandler


        #region AsyncErrorHandler

        private class AsyncErrorHandler : IErrorHandler
        {
            #region Fields

            private readonly AsyncAppenderBase _appender;
            private readonly Type _appenderType;
            private readonly string _appenderName;

            #endregion Fields


            #region .ctor

            public AsyncErrorHandler(AsyncAppenderBase appender)
            {
                _appender = appender;
                if (appender != null)
                {
                    _appenderType = appender.GetType();
                    _appenderName = appender.Name;
                }
            }

            #endregion .ctor


            #region IErrorHandler

            public void Error(string message)
            {
                Add(MakeEvent(message, null));
            }

            public void Error(string message, Exception ex)
            {
                Add(MakeEvent(message, ex));
            }

            public void Error(string message, Exception ex, ErrorCode errorCode)
            {
                Add(MakeEvent(errorCode, message, ex));
            }

            #endregion IErrorHandler


            #region Private

            private void Add(LoggingEvent loggingEvent)
            {
                if (loggingEvent != null)
                {
                    loggingEvent.Fix = _appender.Fix;
                }
                _appender.SendAsync(loggingEvent, true);
            }

            private LoggingEvent MakeEvent(
                ErrorCode errorCode,
                string message,
                Exception ex)
            {
                var msg = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", errorCode, message);
                return new LoggingEvent(_appenderType, null, _appenderName, Level.Error, msg, ex);
            }

            private LoggingEvent MakeEvent(string message, Exception ex)
            {
                return new LoggingEvent(_appenderType, null, _appenderName, Level.Error, message, ex);
            }

            #endregion Private
        }

        #endregion AsyncErrorHandler
    }
}
