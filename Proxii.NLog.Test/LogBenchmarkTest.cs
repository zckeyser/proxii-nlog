using System.Text.RegularExpressions;
using NLog;
using NUnit.Framework;

namespace Proxii.NLog.Test
{
    [TestFixture]
    public class LogBenchmarkTest : BaseTest
    {
        [Test]
        public void LogBenchmark_Default()
        {
            const string expectedPattern = @"INFO\|.+method Do\(Int32 times\) took \d+\.\d{2} ms";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark()
                              .Create();

            proxy.Do(1000);

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }

        [Test]
        public void LogBenchmark_UsesLogLevel_OnlyLogLevel()
        {
            const string expectedPattern = @"TRACE\|.+method Do\(Int32 times\) took \d+\.\d{2} ms";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark(LogLevel.Trace)
                              .Create();

            proxy.Do(1000);

            var logContents = TraceLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }

        [Test]
        public void LogBenchmark_UsesLogLevel_LogLevelAndTimingFormat()
        {
            const string expectedPattern = @"TRACE\|.+method Do\(Int32 times\) took \d+\.\d{2} ms";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark(LogLevel.Trace, "F2")
                              .Create();

            proxy.Do(1000);

            var logContents = TraceLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }

        [Test]
        public void LogBenchmark_UsesLogLevel_LogLevelMessageAndTimingFormat()
        {
            const string expectedPattern = @"TRACE\|.+method Do\(Int32 times\) took \d+\.\d{2} ms";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark(LogLevel.Trace, "method %method% took %timing% ms", "F2")
                              .Create();

            proxy.Do(1000);

            var logContents = TraceLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }

        [Test]
        public void LogBenchmark_FormatsCorrectly_TimingFormatOnly()
        {
            const string expectedPattern = @"INFO\|.+method Do\(Int32 times\) took \d+\.\d{4} ms";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark("F4")
                              .Create();

            proxy.Do(1000);

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }

        [Test]
        public void LogBenchmark_FormatsCorrectly_LogLevelAndTimingFormat()
        {
            const string expectedPattern = @"TRACE\|.+method Do\(Int32 times\) took \d+\.\d{4} ms";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark(LogLevel.Trace, "F4")
                              .Create();

            proxy.Do(1000);

            var logContents = TraceLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }

        [Test]
        public void LogBenchmark_FormatsCorrectly_LogLevelMessageAndTimingFormat()
        {
            const string expectedPattern = @"TRACE\|.+running Do\(Int32 times\) with arguments \(1000\) took \d+\.\d{4} milliseconds";

            var proxy = Proxii.Proxy<ILogBenchmarkTester, LogBenchmarkTester>()
                              .LogBenchmark(LogLevel.Trace, "running %method% with arguments (%args%) took %timing% milliseconds", "F4")
                              .Create();

            proxy.Do(1000);

            var logContents = TraceLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(logContents[0], expectedPattern), Is.True);
        }
    }
}
