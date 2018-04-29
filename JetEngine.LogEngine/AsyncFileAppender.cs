using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using log4net.Appender;
using log4net.Core;

namespace JetEngine.LogEngine
{
    public class AsyncFileAppender : RollingFileAppender
    {
        private Queue<LoggingEvent> pendingTasks;
        private readonly object lockObject = new object();
        private readonly ManualResetEvent manualResetEvent;
        private bool onClosing;

        public AsyncFileAppender()
        {
            pendingTasks = new Queue<LoggingEvent>();
            manualResetEvent = new ManualResetEvent(false);
            Start();
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            if (loggingEvents != null)
            {
                foreach (LoggingEvent loggingEvent in loggingEvents)
                    Append(loggingEvent);

            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (FilterEvent(loggingEvent))
                Enqueue(loggingEvent);
        }

        private void Start()
        {
            if (!onClosing)
            {
                Thread thread = new Thread(LogMessages);
                thread.Start();
            }
        }

        private void LogMessages()
        {
            LoggingEvent loggingEvent;
            while (!onClosing)
            {
                while (!DeQueue(out loggingEvent))
                {
                    Thread.Sleep(10);
                    if (onClosing)
                        break;
                }

                if (loggingEvent != null)
                {
                    base.Append(loggingEvent);
                }
            }

            manualResetEvent.Set();
        }

        private void Enqueue(LoggingEvent loggingEvent)
        {
            lock (lockObject)
            {
                pendingTasks.Enqueue(loggingEvent);
            }
        }

        private bool DeQueue(out LoggingEvent loggingEvent)
        {
            lock (lockObject)
            {
                if (pendingTasks.Count > 0)
                {
                    loggingEvent = pendingTasks.Dequeue();
                    return true;
                }
                else
                {
                    loggingEvent = null;
                    return false;
                }
            }
        }

        protected override void OnClose()
        {
            onClosing = true;
            manualResetEvent.WaitOne(TimeSpan.FromSeconds(10));
            base.OnClose();
        }
    }
}
