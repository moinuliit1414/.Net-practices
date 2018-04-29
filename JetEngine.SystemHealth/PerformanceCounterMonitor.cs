using System;
using System.Diagnostics;
using System.Globalization;

namespace JetEngine.SystemHealth
{
    public class PerformanceCounterMonitor
    {
        private const string NotAvailable = "n\a";

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
         //PerformanceCounter("Processor", "% Processor Time", "_Total");
         //PerformanceCounter("Processor", "% Privileged Time", "_Total");
         //PerformanceCounter("Processor", "% Interrupt Time", "_Total");
         //PerformanceCounter("Processor", "% DPC Time", "_Total");
         //PerformanceCounter("Memory", "Available MBytes", null);
         //PerformanceCounter("Memory", "Committed Bytes", null);
         //PerformanceCounter("Memory", "Commit Limit", null);
         //PerformanceCounter("Memory", "% Committed Bytes In Use", null);
         //PerformanceCounter("Memory", "Pool Paged Bytes", null);
         //PerformanceCounter("Memory", "Pool Nonpaged Bytes", null);
         //PerformanceCounter("Memory", "Cache Bytes", null);
         //PerformanceCounter("Paging File", "% Usage", "_Total");
         //PerformanceCounter("PhysicalDisk", "Avg. Disk Queue Length", "_Total");
         //PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
         //PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
         //PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Read", "_Total");
         //PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Write", "_Total");
         //PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
         //PerformanceCounter("Process", "Handle Count", "_Total");
         //PerformanceCounter("Process", "Thread Count", "_Total");
         //PerformanceCounter("System", "Context Switches/sec", null);
         //PerformanceCounter("System", "System Calls/sec", null);
         //PerformanceCounter("System", "Processor Queue Length", null);
    }
}
