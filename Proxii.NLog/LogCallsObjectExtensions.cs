using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxii.NLog
{
    public static class LogCallsObjectExtensions
    {
        /// <summary>
        /// Logs a JSON object on interception in varying detail levels (https://github.com/zckeyser/proxii-nlog/wiki/LogCallsObject)
        /// </summary>
        public static IProxii<T> LogCallsObject<T>(this IProxii<T> proxii, LogDetailLevel detailLevel)
            where T : class
        {


            return proxii;
        }

        private static Action<MethodInfo, object[]> CreateLogCallsObjectAction(Logger logger, LogLevel logLevel, string format)
        {
            return (method, args) =>
            {
                logger.Log(logLevel, format, method.GetMethodSignature(), string.Join(", ", args));
            };
        }
    }

    public enum LogDetailLevel
    {
        Low,
        Medium,
        High
    }
}
