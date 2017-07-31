using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Proxii.NLog.Test
{
    [TestFixture]
    public class LogCallsObjectTest : BaseTest
    {
        const string ISODateTimePattern = "\"\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\.\\d+[+-]\\d{2}:\\d{2}\"";

        [Test]
        public void LogCallsObject_LowDetail()
        {
            var proxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCallsObject(LogDetailLevel.Low)
                              .Create();

            proxy.Do();

            string pattern = $"{{\"methodName\":\"Do\",\"timestamp\":{ISODateTimePattern},\"className\":\"Proxii\\.NLog\\.Test\\.ILogTester\"}}";

            Assert.That(Regex.IsMatch(InfoLogContents[0], pattern), Is.True);
        }

        [Test]
        public void LogCallsObject_MediumDetail()
        {

        }

        [Test]
        public void LogCallsObject_DefaultsToLowDetail()
        {

        }

        [Test]
        public void LogCallsObject_UsesLogLevel()
        {

        }
    }
}
