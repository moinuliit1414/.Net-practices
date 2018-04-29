using JetEngine.LogEngine.AsyncBuffers;
using JetEngine.LogEngine.Interfaces;
using JetEngine.LogEngine.Limiters;
using System;
using System.Linq;
using log4net.Core;

namespace JetEngine.LogEngine.Appenders
{
    /// <summary>
    /// Async appender with no lock synchronization
    /// </summary>
    public class AsyncConcurrentAppender : AsyncAppenderBase
    {
        private int _growLimit;


        public AsyncConcurrentAppender()
        {
            GrowType = GrowType.Grow;
            GrowLimit = 1000000;
        }


        public GrowType GrowType { get; set; }

        public int GrowLimit
        {
            get { return _growLimit; }
            set { _growLimit = Math.Max(1, value); }
        }


        protected override IAsyncBuffer<LoggingEvent> CreateAsyncBuffer()
        {
            var limiter = new GrowLimiter(GrowType, GrowLimit);
            limiter.Error += (s, e) => ErrorHandler.Error(e.Text);
            return new AsyncBufferConcurrent(limiter);
        }
    }
}
