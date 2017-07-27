using System;
using Newtonsoft.Json;
using NLog;

namespace Proxii.NLog
{
    public static class LogExceptionsExtension
    {
        public static IProxii<T> LogExceptions<T>(this IProxii<T> proxii, LogLevel logLevel)
            where T : class
        {
            return proxii.Catch<Exception>(LogExceptionsAction(typeof(T).GetLogger(), logLevel));
        }

        public static IProxii<T> LogExceptions<T>(this IProxii<T> proxii)
            where T : class
        {
            return proxii.LogExceptions(LogLevel.Error);
        }

        private static Action<Exception> LogExceptionsAction(Logger logger, LogLevel logLevel)
        {
            return e =>
            {
                logger.Log(logLevel, JsonConvert.SerializeObject(e));
            };
        }
    }
}
