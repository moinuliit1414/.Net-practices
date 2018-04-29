using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Deployment.Application;

namespace JetEngine.LogEngine
{
    public static class BuildInfo
    {
        private static readonly object _syncRoot = new object();

        public static string Configuration { get; private set; }
        public static string Version { get; private set; }
        public static DateTime TimeStamp { get; private set; }
        public static string ApplicationVersion { get; private set; }
        public static string InternalVersion { get; private set; }
        public static string Product { get; private set; }
        public static string Copyright { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void Initialize(Assembly asm)
        {
            try
            {
                lock (_syncRoot)
                {
                    var version = asm.GetCustomAttribute<AssemblyFileVersionAttribute>();
                    InternalVersion = version != null ? version.Version : null;

                    if (ApplicationDeployment.IsNetworkDeployed)
                    {
                        var deployment = ApplicationDeployment.CurrentDeployment;
                        Version = deployment.CurrentVersion.ToString();
                    }
                    else
                    {
                        Version = InternalVersion;
                    }
                    if (Version != null)
                    {
                        var dotIndex = Version.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
                        if (dotIndex >= 0)
                        {
                            ApplicationVersion = Version.Substring(0, dotIndex);
                        }
                    }

                    var config = asm.GetCustomAttribute<AssemblyConfigurationAttribute>();
                    Configuration = config != null ? config.Configuration : null;

                    var product = asm.GetCustomAttribute<AssemblyProductAttribute>();
                    Product = product != null ? product.Product : null;

                    var copyright = asm.GetCustomAttribute<AssemblyCopyrightAttribute>();
                    Copyright = copyright != null ? copyright.Copyright : null;

                    TimeStamp = GetLinkerTimestamp(asm);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static DateTime GetLinkerTimestamp(Assembly assembly)
        {
            try
            {
                var fileInfo = new FileInfo(assembly.Location);
                if (!fileInfo.Exists)
                {
                    return DateTime.MinValue;
                }
                var seconds = 0;
                using (var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var buf = new byte[2048];
                    stream.Seek(60, SeekOrigin.Begin);
                    stream.Read(buf, 0, 4);
                    var offsetPeHeader = BitConverter.ToInt32(buf, 0);
                    var offsetTimeStamp = offsetPeHeader + 8;
                    if (offsetTimeStamp + 4 > stream.Length)
                    {
                        return DateTime.MinValue;
                    }
                    stream.Seek(offsetTimeStamp, SeekOrigin.Begin);
                    stream.Read(buf, 0, 4);
                    seconds = BitConverter.ToInt32(buf, 0);
                }
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(seconds);
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
