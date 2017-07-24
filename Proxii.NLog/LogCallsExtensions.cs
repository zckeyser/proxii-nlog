using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Proxii.NLog
{
    public static class LogCallsExtensions
    {
        /// <summary>
        /// Logs method calls to the configured logger using the given format string.
        /// 
        /// Method call logging is performed before invocation.
        /// 
        /// The format string replaces:
        /// "%method%" with the method signature
        /// "%args%" with a comma-delimited argument list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="logLevel">The NLog level to log the message to. Defaults to LogLevel.Info</param>
        /// <param name="format">The format string to use to generate the message</param>
        /// <returns></returns>
        public static IProxii<T> LogCalls<T>(this IProxii<T> proxii, LogLevel logLevel, string format)
            where T : class
        {
            var logger = typeof(T).GetLogger();

            if (string.IsNullOrWhiteSpace(format))
            {
                logger.Log(LogLevel.Warn, "LogCalls() provided with empty or null format string");
                return proxii;
            }


            var nlogFormat = format.Replace("%method%", "{0}")
                                   .Replace("%args%", "{1}");

            return proxii.BeforeInvoke(CreateLogCallsAction(logger, logLevel, nlogFormat));
        }

        /// <summary>
        /// Logs method calls to the configured logger based on a default format.
        /// 
        /// for MethodSignature level, the format is "called method %method%"
        /// for Args level, the format is "called method %method% with arguments (%args)"
        /// 
        /// Method call logging is performed before invocation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="detailLevel">The level of information to provide in the log message</param>
        /// <returns></returns>
        public static IProxii<T> LogCalls<T>(this IProxii<T> proxii, LogLevel logLevel, MethodCallDetailLevel detailLevel = MethodCallDetailLevel.MethodSignature)
            where T : class
        {
            switch (detailLevel)
            {
                case MethodCallDetailLevel.MethodSignature:
                    return LogCalls(proxii, logLevel, "called method %method%");
                case MethodCallDetailLevel.Args:
                    return LogCalls(proxii, logLevel, "called method %method% with arguments (%args%)");
                default:
                    return proxii;
            }
        }

        /// <summary>
        /// Logs method calls to the configured logger based on a default format.
        /// 
        /// for MethodName level, the format is "called method %method%"
        /// for Args level, the format is "called method %method% with arguments (%args)"
        /// 
        /// Method call logging is performed before invocation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="detailLevel">The level of information to provide in the log message</param>
        /// <returns></returns>
        public static IProxii<T> LogCalls<T>(this IProxii<T> proxii, MethodCallDetailLevel detailLevel = MethodCallDetailLevel.MethodSignature)
            where T : class
        {
            return LogCalls(proxii, LogLevel.Info, detailLevel);
        }

        /// <summary>
        /// Logs method calls to the configured logger using the given format string.
        /// 
        /// Method call logging is performed before invocation.
        /// 
        /// The format string replaces "%method%" with the method signature and
        /// "%args%" with a comma-delimited argument list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="logLevel">The NLog level to log the message to. Defaults to LogLevel.Info</param>
        /// <param name="format">The format string to use to generate the message</param>
        /// <returns></returns>
        public static IProxii<T> LogCalls<T>(this IProxii<T> proxii, string format)
            where T : class
        {
            return LogCalls(proxii, LogLevel.Info, format);
        }

        private static Action<MethodInfo, object[]> CreateLogCallsAction(Logger logger, LogLevel logLevel, string format)
        {
            return (method, args) =>
            {
                logger.Log(logLevel, format, method.GetMethodSignature(), string.Join(", ", args));
            };
        }
    }
}
