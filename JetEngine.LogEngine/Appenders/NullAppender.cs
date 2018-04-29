using System;
using System.Linq;
using log4net.Appender;
using log4net.Core;

namespace JetEngine.LogEngine.Appenders
{
    /// <summary>
    /// Null appender for perfromance measurements
    /// </summary>
    public class NullAppender : IBulkAppender, IAppender, IOptionHandler
    {
        #region IBulkAppender

        public string Name { get; set; }

        public void DoAppend(LoggingEvent[] loggingEvents)
        {
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
        }

        public void Close()
        {
        }

        #endregion IBulkAppender


        #region IOptionHandler

        public void ActivateOptions()
        {
        }

        #endregion IOptionHandler
    }
}
