using System;
using System.Globalization;
using System.Threading;
using log4net.Core;

namespace AutoTrade.Core
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="logger">The logger to use to log the message</param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public static void LogInfo(this ILogger logger, string message, params object[] parameters)
        {
            Log(logger, Level.Info, message, null, parameters);
        }

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="logger">The logger to use to log the message</param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public static void LogInfo(this ILogger logger, string message, Exception ex, params object[] parameters)
        {
            Log(logger, Level.Info, message, ex, parameters);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public static void LogWarning(this ILogger logger, string message, params object[] parameters)
        {
            Log(logger, Level.Warn, message, null, parameters);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public static void LogWarning(this ILogger logger, string message, Exception ex, params object[] parameters)
        {
            Log(logger, Level.Warn, message, ex, parameters);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public static void LogError(this ILogger logger, string message, params object[] parameters)
        {
            Log(logger, Level.Error, message, null, parameters);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        public static void LogError(this ILogger logger, string message, Exception ex, params object[] parameters)
        {
            Log(logger, Level.Error, message, ex, parameters);
        }

        /// <summary>
        /// Logs an error message with an exception
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="parameters"></param>
        public static void LogException(this ILogger logger, string message, Exception ex, params object[] parameters)
        {
            Log(logger, Level.Error, message, ex, parameters);
        }

        /// <summary>
        /// Logs a message using the logger
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="parameters"></param>
        private static void Log(ILogger logger, Level logLevel, string message, Exception ex, params object[] parameters)
        {
            logger.Log(new LoggingEvent(new LoggingEventData
            {
                Level = logLevel,
                TimeStamp = DateTime.Now,
                ThreadName = Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.CurrentCulture),
                ExceptionString = ex != null ? ex.ToString() : null,
                Message = string.Format(message, parameters)
            }));
        }
    }
}
