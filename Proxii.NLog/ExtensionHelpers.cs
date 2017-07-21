using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;

namespace Proxii.NLog
{
    internal static class ExtensionHelpers
    {
        /// <summary>
        /// In order to map logs to the classes calling them,
        /// since the default behavior would make every log
        /// look like it was coming from Proxii
        /// </summary>
        private static readonly Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();

        internal static Logger GetLogger(this Type type)
        {
            var className = type.FullName;

            if (!loggers.ContainsKey(className))
                loggers[className] = LogManager.GetLogger(className);

            return loggers[className];
        }

        internal static string GetMethodSignature(this MethodInfo method)
        {
            return $"{method.Name}({ParameterListToString(method.GetParameters())})";
        }

        internal static string ParameterListToString(IEnumerable<ParameterInfo> parameters)
        {
            return string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
        }
    }
}
