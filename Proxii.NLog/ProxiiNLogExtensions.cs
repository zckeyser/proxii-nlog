﻿using System.Collections.Generic;
using NLog;
using System;
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
        /// "%method%" with the method name
        /// "%args%" with a comma-delimited argument list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="logLevel">The NLog level to log the message to. Defaults to LogLevel.Info</param>
        /// <param name="format">The format string to use to generate the message</param>
        /// <returns></returns>
        public static IProxii<T> LogCalls<T>(this IProxii<T> proxii, LogLevel logLevel, string format = null)
            where T : class
        {
            var logger = GetLogger(typeof(T).FullName);

            if (string.IsNullOrWhiteSpace(format))
                logger.Log(LogLevel.Warn, "LogCalls() provided with empty or null format string");

            var nlogFormat = format.Replace("%method%", "{0}")
                                   .Replace("%args%", "{1}");

            return proxii.BeforeInvoke(CreateAction(logger, logLevel, format));
        }

        private static Action<MethodInfo, object[]> CreateAction(Logger logger, LogLevel logLevel, string format)
        {
            return (m, args) =>
            {
                logger.Log(logLevel, format, m.Name, string.Join(", ", args.ToString()));
            };
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
        public static IProxii<T> LogCalls<T>(this IProxii<T> proxii, LogLevel logLevel, MethodCallDetailLevel detailLevel)
            where T : class
        {
            switch (detailLevel)
            {
                case MethodCallDetailLevel.MethodName:
                    return LogCalls(proxii, logLevel, "called method %method%");
                case MethodCallDetailLevel.Args:
                    return LogCalls(proxii, logLevel, "Called method %method% with arguments (%args%)");
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
        public static IProxii<T> LogCalls<T>(this IProxii<T> proxii, MethodCallDetailLevel detailLevel = MethodCallDetailLevel.MethodName)
            where T : class
        {
            return LogCalls(proxii, LogLevel.Info, detailLevel);
        }

        /// <summary>
        /// Logs method calls to the configured logger using the given format string.
        /// 
        /// Method call logging is performed before invocation.
        /// 
        /// The format string replaces "%method%" with the method name and
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

        /// <summary>
        /// Logs a benchmark for all intercepted method calls using the given format string
        /// 
        /// The format string replaces
        /// "%timing%" with the timing in ms
        /// "%method%" with the method name
        /// "%args%" with a comma-delimited argument list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="logLevel">NLog visibility level to log at</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static IProxii<T> LogBenchmark<T>(this IProxii<T> proxii, LogLevel logLevel, string format)
            where T : class
        {
            return proxii.Benchmark((timing, method, args) =>
            {
                var message = format.Replace("%timing%", timing.ToString("F2"))
                                    .Replace("%method%", method.Name)
                                    .Replace("%args%", string.Join(", ", args));

                GetLogger(typeof(T).FullName).Log(logLevel, message);
            });
        }

        /// <summary>
        /// Logs a benchmark for all intercepted method calls using the given format string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="logLevel">NLog visibility level to log at</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static IProxii<T> LogBenchmark<T>(this IProxii<T> proxii, string format)
            where T : class
        {
            return LogBenchmark(proxii, LogLevel.Info, format);
        }

        private static Logger GetLogger(string className)
        {
            if (!loggers.ContainsKey(className))
                loggers[className] = LogManager.GetLogger(className);

            return loggers[className];
        }
    }
}