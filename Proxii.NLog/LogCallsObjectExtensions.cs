using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Proxii.NLog
{
    public static class LogCallsObjectExtensions
    {
        /// <summary>
        /// Logs a JSON object on interception in varying detail levels (https://github.com/zckeyser/proxii-nlog/wiki/LogCallsObject)
        /// </summary>
        public static IProxii<T> LogCallsObject<T>(this IProxii<T> proxii, LogLevel logLevel, LogDetailLevel detailLevel = LogDetailLevel.Low)
            where T : class
        {
            return proxii.BeforeInvoke(CreateLogCallsObjectAction(typeof(T).GetLogger(), logLevel, detailLevel));
        }

        /// <summary>
        /// Logs a JSON object on interception in varying detail levels (https://github.com/zckeyser/proxii-nlog/wiki/LogCallsObject)
        /// </summary>
        public static IProxii<T> LogCallsObject<T>(this IProxii<T> proxii, LogDetailLevel detailLevel = LogDetailLevel.Low)
            where T : class
        {
            return proxii.LogCallsObject(LogLevel.Info, detailLevel);
        }

        private static Action<MethodInfo, object[]> CreateLogCallsObjectAction(Logger logger, LogLevel logLevel, LogDetailLevel detailLevel)
        {
            return (method, args) =>
            {
                switch (detailLevel)
                {
                    case LogDetailLevel.Low:
                        logger.Log(logLevel, GetLowDetailLog(method));
                        break;
                    case LogDetailLevel.Medium:
                        logger.Log(logLevel, GetMediumDetailLog(method, args));
                        break;
                    case LogDetailLevel.High:
                        // i don't think this is actually possible to make
                        break;
                    default:
                        break;
                }
                
            };
        }

        private static LowDetailLog GetLowDetailLog(MethodInfo method)
        {
            return new LowDetailLog
            {
                timestamp = DateTime.Now.ToString("O"),
                className = method.DeclaringType.FullName,
                methodName = method.Name
            };
        }

        private static MediumDetailLog GetMediumDetailLog(MethodInfo method, object[] args)
        {
            return new MediumDetailLog
            {
                timestamp = DateTime.Now.ToString("O"),
                className = method.DeclaringType.FullName,
                methodName = method.Name,
                methodSignature = method.GetMethodSignature(),
                arguments = args
            };
        }
    }

    public enum LogDetailLevel
    {
        Low,
        Medium,
        High
    }

    public class LowDetailLog
    {
        public string methodName;
        public string timestamp;
        public string className;
    }

    public class MediumDetailLog
    {
        public string methodName;
        public string timestamp;
        public string className;
        public string methodSignature;
        public object[] arguments;
    }

    public class HighDetailLog
    {
        public string methodName;
        public string timestamp;
        public string className;
        public string methodSignature;
        public string[] arguments;
        public object callingObject;
    }
}
