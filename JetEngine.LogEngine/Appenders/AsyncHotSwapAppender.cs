using System;
using log4net.Core;
using JetEngine.LogEngine.Limiters;
using JetEngine.LogEngine.Interfaces;

namespace JetEngine.LogEngine.Appenders
{
    /// <summary>
    /// Async appender with "Hot Swap" synchronization
    /// </summary>
    public class AsyncHotSwapAppender : AsyncAppenderBase
    {
        private int _growLimit;


        public AsyncHotSwapAppender()
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
            return new AsyncBufferHotSwap(limiter);
        }
    }
}
