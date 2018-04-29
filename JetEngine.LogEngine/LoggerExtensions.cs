using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.LogEngine
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, Exception ex)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Debug(ex, string.Empty);
        }

        public static void Info(this ILogger logger, Exception ex)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Info(ex, string.Empty);
        }

        public static void Warn(this ILogger logger, Exception ex)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Warn(ex, string.Empty);
        }

        public static void Error(this ILogger logger, Exception ex)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Error(ex, string.Empty);
        }

        public static void Fatal(this ILogger logger, Exception ex)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Fatal(ex, string.Empty);
        }

        public static void Debug(this ILogger logger, string fmt, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Debug(null, fmt, args);
        }

        public static void Info(this ILogger logger, string fmt, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Info(null, fmt, args);
        }

        public static void Warn(this ILogger logger, string fmt, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Warn(null, fmt, args);
        }

        public static void Error(this ILogger logger, string fmt, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Error(null, fmt, args);
        }

        public static void Fatal(this ILogger logger, string fmt, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            logger.Fatal(null, fmt, args);
        }
    }
}
