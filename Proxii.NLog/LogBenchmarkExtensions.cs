using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Proxii.NLog
{
    public static class LogBenchmarkExtensions
    {
        /// <summary>
        /// Logs a benchmark for all intercepted method calls using the given format string
        /// 
        /// The format string replaces:
        /// "%timing%" with the timing in ms. By default uses 2-digit floating point format (e.g. 2.64)
        /// If an alternative format is desired, pass a format string as specified in https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// 
        /// "%method%" with the method signature
        /// "%args%" with a comma-delimited argument list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="logLevel">NLog visibility level to log at</param>
        /// <param name="messageFormat">Format string for message. See method description/docs</param>
        /// <param name="timingFormat">Formatting to use for the timing parameter</param>
        /// <returns></returns>
        public static IProxii<T> LogBenchmark<T>(this IProxii<T> proxii, LogLevel logLevel, string messageFormat, string timingFormat = "F2")
            where T : class
        {
            var logger = GetLogger(typeof(T).FullName);
            var nLogFormat = messageFormat.Replace("%timing%", "{0}")
                                          .Replace("%method%", "{1}")
                                          .Replace("%args%", "{2}");

            return proxii.Benchmark(GetLogBenchmarkAction(logger, logLevel, nLogFormat, timingFormat));
        }

        /// <summary>
        /// Logs a benchmark for all intercepted method calls using the given format string
        /// 
        /// The format string replaces:
        /// "%timing%" with the timing in ms. By default uses 2-digit floating point format (e.g. 2.64)
        /// If an alternative format is desired, pass a format string as specified in https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// 
        /// "%method%" with the method signature
        /// "%args%" with a comma-delimited argument list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="messageFormat">Format string for message. See method description/docs.</param>
        /// <param name="timingFormat">Formatting to use for the timing parameter</param>
        /// <returns></returns>
        public static IProxii<T> LogBenchmark<T>(this IProxii<T> proxii, string messageFormat, string timingFormat = "F2")
            where T : class
        {
            return LogBenchmark(proxii, LogLevel.Info, messageFormat, timingFormat);
        }

        /// <summary>
        /// Logs a benchmark for all intercepted method calls 
        /// using the default format "method %method% took %timing% ms"
        /// 
        /// The format string replaces:
        /// "%timing%" with the timing in ms. By default uses 2-digit floating point format (e.g. 2.64)
        /// If an alternative format is desired, pass a format string <see cref="https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings"/>
        /// 
        /// "%method%" with the method signature
        /// "%args%" with a comma-delimited argument list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxii"></param>
        /// <param name="messageFormat">Format string for message. See method description/docs.</param>
        /// <param name="timingFormat">Formatting to use for the timing parameter</param>
        /// <returns></returns>
        public static IProxii<T> LogBenchmark<T>(this IProxii<T> proxii, string timingFormat = "F2")
            where T : class
        {
            return LogBenchmark(proxii, LogLevel.Info, "method %method% took %timing% ms", timingFormat);
        }

        private static Action<double, MethodInfo, object[]> GetLogBenchmarkAction(Logger logger, LogLevel logLevel, string format, string timingFormat)
        {
            return (timing, method, args) =>
            {
                logger.Log(logLevel, format, timing.ToString(timingFormat), GetMethodSignature(method), string.Join(", ", args));
            };
        }


    }
}
