using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using log4net;

namespace JetEngine.LogEngine
{
    public class Logger : ILogger
    {
        private const string DefaultLoggerName = "Common";
        private const string NotAvailable = "n\a";

        private readonly ILog _logger;
        private readonly ConcurrentDictionary<int, ThreadContext> _threads = new ConcurrentDictionary<int, ThreadContext>();

        public string Name { get; private set; }

        public Logger()
            : this(DefaultLoggerName)
        {
        }

        public Logger(string name)
        {
            Name = name;
            _logger = LogManager.GetLogger(name);
        }

        #region ILogger

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual void Debug(Exception ex, string fmt, params object[] args)
        {
            try
            {
                if (ex == null)
                {
                    _logger.Debug(FormatMessage(fmt, args));
                }
                else
                {
                    _logger.Debug(FormatMessage(fmt, args), ex);
                }
            }
            catch (Exception logEx)
            {
                InternalError(ex, logEx);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual void Info(Exception ex, string fmt, params object[] args)
        {
            try
            {
                if (ex == null)
                {
                    _logger.Info(FormatMessage(fmt, args));
                }
                else
                {
                    _logger.Info(FormatMessage(fmt, args), ex);
                }
            }
            catch (Exception logEx)
            {
                InternalError(ex, logEx);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual void Warn(Exception ex, string fmt, params object[] args)
        {
            try
            {
                if (ex == null)
                {
                    _logger.Warn(FormatMessage(fmt, args));
                }
                else
                {
                    _logger.Warn(FormatMessage(fmt, args), ex);
                }
            }
            catch (Exception logEx)
            {
                InternalError(ex, logEx);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual void Error(Exception ex, string fmt, params object[] args)
        {
            try
            {
                if (ex == null)
                {
                    _logger.Error(FormatMessage(fmt, args));
                }
                else
                {
                    _logger.Error(FormatMessage(fmt, args), ex);
                }
            }
            catch (Exception logEx)
            {
                InternalError(ex, logEx);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual void Fatal(Exception ex, string fmt, params object[] args)
        {
            try
            {
                if (ex == null)
                {
                    _logger.Fatal(FormatMessage(fmt, args));
                }
                else
                {
                    _logger.Fatal(FormatMessage(fmt, args), ex);
                }
            }
            catch (Exception logEx)
            {
                InternalError(ex, logEx);
            }
        }

        public IDisposable CreateContext(string name)
        {
            return new TimeContext(this, name, true);
        }

        public IDisposable CreateContext(string fmtName, params object[] args)
        {
            var name = string.Format(CultureInfo.InvariantCulture, fmtName, args);
            return new TimeContext(this, name, true);
        }

        public void SystemInfo(SystemInfoType type)
        {
            if (type.HasFlag(SystemInfoType.IPAddress))
            {
                this.Info("IP Address:   {0}", GetLocalIPAddress());
            }
            if (type.HasFlag(SystemInfoType.AvailableMemory))
            {
                this.Info("Memory:       {0} MB", GetAvailableMemory());
            }
            if (type.HasFlag(SystemInfoType.IPAddress))
            {
                TimeSpan upTime = TimeSpan.FromMilliseconds(Environment.TickCount);
                this.Info("Uptime:       {0}", upTime.ToString(@"dd\.hh\:mm\:ss", CultureInfo.InvariantCulture));
            }
            Instance.Info(string.Empty);
        }

        #endregion ILogger


        #region Private

        private string FormatMessage(string fmt, object[] args)
        {
            var result = fmt;
            if (args.Length > 0)
            {
                result = string.Format(CultureInfo.InvariantCulture, fmt, args);
            }
            var space = Context.Prefix;
            if (!string.IsNullOrEmpty(space))
            {
                result = space + result;
            }
            return result;
        }

        private ThreadContext Context
        {
            get
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                return _threads.GetOrAdd(threadId, arg => new ThreadContext(arg));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void InternalError(Exception ex, Exception intEx)
        {
            try
            {
                if (ex == null)
                {
                    _logger.Error(string.Empty, intEx);
                }
                else
                {
                    _logger.Error(ex.ToString(), intEx);
                }
            }
            catch (Exception logEx)
            {
                _logger.Error("Logger.InternalError", logEx);
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (SocketException)
            {
                // just do nothing since we have code below
            }
            return NotAvailable;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private string GetAvailableMemory()
        {
            try
            {
                using (PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes"))
                {
                    return ramCounter.NextValue().ToString(CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                return NotAvailable;
            }
            
        }

        #endregion Private


        #region Common Logger Singleton

        private static readonly ILogger _instance = new Logger();

        /// <summary>
        /// Get common logger instance
        /// </summary>
        public static ILogger Instance
        {
            get { return _instance; }
        }

        #endregion Common Logger Singleton

        #region TimeContext

        private sealed class TimeContext : IDisposable
        {
            private readonly Logger _logger;
            private readonly string _name;
            private readonly long _stampStart;
            private readonly bool _logTime;
            private bool _disposed;


            public TimeContext(Logger logger, string name, bool logTime)
            {
                _logger = logger;
                _name = name;
                _logTime = logTime;
                _stampStart = Stopwatch.GetTimestamp();
                _logger.Debug("{0} start", _name);
                _logger.Context.LevelIncrement();
            }

            public void Dispose()
            {
                if (_disposed)
                {
                    _logger.Warn("TimeContext.Dispose: already disposed!");
                    return;
                }
                _disposed = true;
                var duration = (Stopwatch.GetTimestamp() - _stampStart) * 1000D / (double)Stopwatch.Frequency;
                _logger.Context.LevelDecrement();
                var fmt = _logTime ?
                    "{0} finish in {1:F6} [ms]" :
                    "{0} finish";
                _logger.Debug(fmt, _name, duration);
            }
        }

        [DebuggerDisplay("ThreadContext: {_threadId}, {_level}")]
        private sealed class ThreadContext
        {
            private const int MaxLevel = 10;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "For debug purposes")]
            private readonly int _threadId;
            private int _level;

            public ThreadContext(int threadId)
            {
                _threadId = threadId;
            }

            public string Prefix { get; private set; }

            public void LevelIncrement()
            {
                var level = ++_level;
                level = level < 0 ? 0 : level;
                level = level > MaxLevel ? MaxLevel : level;
                Prefix = new string(' ', level * 4);
            }

            public void LevelDecrement()
            {
                var level = --_level;
                level = level < 0 ? 0 : level;
                level = level > MaxLevel ? MaxLevel : level;
                Prefix = new string(' ', level * 4);
            }
        }

        #endregion TimeContext


        #region Logger Start/Finish

        public static void Startup(SystemInfoType logSystemInfo = SystemInfoType.All)
        {
            var asm = Assembly.GetCallingAssembly();
            BuildInfo.Initialize(asm);
            Instance.Info(
                "===================================={0} - START====================================",
                Process.GetCurrentProcess().ProcessName);
            Instance.Info("Version:      {0}", BuildInfo.Version);
            Instance.Info("Internal:     {0} [{1}]", BuildInfo.InternalVersion, BuildInfo.Configuration);
            Instance.Info("Stamp:        {0:s} UTC", BuildInfo.TimeStamp.ToUniversalTime());
            Instance.Info("Entry:        {0}", asm.FullName);
            Instance.Info("Runtime:      {0}", Environment.Version);
            Instance.Info("OS Version:   {0} {1}", Environment.OSVersion, Environment.Is64BitOperatingSystem ? "x64" : "x86");
            Instance.Info("Machine Name: {0}", Environment.MachineName);
            Instance.Info("Process Type: {0}", Environment.Is64BitProcess ? "x64" : "x86");
            Instance.Info("Command Line: {0}", Environment.CommandLine);
            Instance.Info("Identity:     {0}", WindowsIdentity.GetCurrent().Name);

            if (logSystemInfo > SystemInfoType.None)
            {
                Instance.SystemInfo(logSystemInfo);
            }
            else
            {
                Instance.Info(string.Empty);
            }
        }

        public static void Shutdown()
        {
            Instance.Info(
                "===================================={0} - EXIT=====================================",
                Process.GetCurrentProcess().ProcessName);
            log4net.LogManager.Shutdown();
        }

        #endregion Logger Start/Finish
    }
}
