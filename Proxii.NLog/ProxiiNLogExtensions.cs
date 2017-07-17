using System.Collections.Generic;
using NLog;
using System;
using System.Linq;
using System.Reflection;

namespace Proxii.NLog
{
    public static class ProxiiNLogExtensions
    {
        /// <summary>
        /// In order to map logs to the classes calling them,
        /// since the default behavior would make every log
        /// look like it was coming from Proxii
        /// </summary>
        private static readonly Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();

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
            var logger = GetLogger(typeof(T).FullName);

            if (string.IsNullOrWhiteSpace(format))
            {
                logger.Log(LogLevel.Warn, "LogCalls() provided with empty or null format string");
                return proxii;
            }
                

            var nlogFormat = format.Replace("%method%", "{0}")
                                   .Replace("%args%", "{1}");

            return proxii.BeforeInvoke(CreateLogCallsAction(logger, logLevel, nlogFormat));
        }

        private static Action<MethodInfo, object[]> CreateLogCallsAction(Logger logger, LogLevel logLevel, string format)
        {
            return (method, args) =>
            {
                logger.Log(logLevel, format, GetMethodSignature(method), string.Join(", ", args));
            };
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

        private static Logger GetLogger(string className)
        {
            if (!loggers.ContainsKey(className))
                loggers[className] = LogManager.GetLogger(className);

            return loggers[className];
        }

        private static string GetMethodSignature(MethodInfo method)
        {
            return $"{method.Name}({ParameterListToString(method.GetParameters())})";
        }

        private static string ParameterListToString(IEnumerable<ParameterInfo> parameters)
        {
            return string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
        }
    }
}
