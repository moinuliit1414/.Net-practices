using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.LogEngine
{
    public interface ILogger
    {
        string Name { get; }

        void SystemInfo(SystemInfoType type);

        void Debug(Exception ex, string fmt, params object[] args);
        void Info(Exception ex, string fmt, params object[] args);
        void Warn(Exception ex, string fmt, params object[] args);
        void Error(Exception ex, string fmt, params object[] args);
        void Fatal(Exception ex, string fmt, params object[] args);

        /// <summary>
        /// Create time context
        /// </summary>
        /// <param name="name">Context name, for example - method name</param>
        IDisposable CreateContext(string name);
        /// <summary>
        /// Create time context
        /// </summary>
        /// <param name="fmtName">Context name format, for example - method name</param>
        /// <param name="args">Name arguments</param>
        IDisposable CreateContext(string fmtName, params object[] args);
    }

    [Flags]
    public enum SystemInfoType
    {
        None = 0,
        IPAddress = 1,
        Uptime = 2,
        AvailableMemory = 4,
        Simple = IPAddress | Uptime,
        All = IPAddress | Uptime | AvailableMemory
    }
}
