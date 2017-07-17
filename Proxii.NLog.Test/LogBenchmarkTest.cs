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
    public class LogBenchmarkTest : BaseTest
    {
        [Test]
        public void LogBenchmark_Default()
        {
            const string expectedPattern = @"method Do\(Int32 times\) took \d+\.\d{2} ms";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark()
                              .Create();

            proxy.Do(1000);

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }
    }
}
