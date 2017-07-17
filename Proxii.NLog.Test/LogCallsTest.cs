using NUnit.Framework;
using System.IO;
using NLog;

namespace Proxii.NLog.Test
{
    [TestFixture]
    public class LogCallsTest : BaseTest
    {
        [Test]
        public void LogCalls_MethodSignatureDetailLevel()
        {
            var proxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls()
                              .Create();

            proxy.Do();
            proxy.Do("foo", 10);

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(2));
            Assert.That(logContents[0].Contains("called method Do()"), Is.True);
            Assert.That(logContents[1].Contains("called method Do(String s, Int32 i)"), Is.True);
        }

        [Test]
        public void LogCalls_ArgsDetailLevel()
        {
            var proxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls(MethodCallDetailLevel.Args)
                              .Create();

            proxy.Do("foo", 10);

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(logContents[0].Contains("called method Do(String s, Int32 i) with arguments (foo, 10)"), Is.True);
        }

        [Test]
        public void LogCalls_FormatString()
        {
            string format = "this formatted message was called when intercepting %method% with arguments (%args%)";

            var proxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls(format)
                              .Create();

            proxy.Do("foo", 10);

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(logContents[0].Contains("this formatted message was called when intercepting Do(String s, Int32 i) with arguments (foo, 10)"), Is.True);
        }

        [Test]
        public void LogCalls_WarnsOnEmptyFormatString()
        {
            var proxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls("")
                              .Create();

            proxy.Do("foo", 10);

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(logContents[0].Contains("WARN|Proxii.NLog.Test.ILogTester|LogCalls() provided with empty or null format string"), Is.True);
        }

        [Test]
        public void LogCalls_UsesLogLevel()
        {
            var traceProxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls(LogLevel.Trace)
                              .Create();

            var infoProxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls()
                              .Create();

            traceProxy.Do();
            infoProxy.Do();

            var traceLogContents = TraceLogContents;
            var infoLogContents = InfoLogContents;

            Assert.That(traceLogContents.Length, Is.EqualTo(2));
            Assert.That(traceLogContents[0].Contains("TRACE"), Is.True);
            Assert.That(traceLogContents[1].Contains("INFO"), Is.True);

            Assert.That(infoLogContents.Length, Is.EqualTo(1));
            Assert.That(infoLogContents[0].Contains("INFO"), Is.True);
        }

        [Test]
        public void LogCalls_DefaultsToInfoLogLevel()
        {
            var proxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls()
                              .Create();

            proxy.Do();

            var logContents = InfoLogContents;

            Assert.That(logContents.Length, Is.EqualTo(1));
            Assert.That(logContents[0].Contains("INFO"), Is.True);
        }

        [Test]
        public void LogCalls_UsesCorrectLogger()
        {
            var normalProxy = Proxii.Proxy<ILogTester, LogTester>()
                              .LogCalls()
                              .Create();

            var limitedProxy = Proxii.Proxy<ILimitedLogTester, LimitedLogTester>()
                              .LogCalls()
                              .Create();

            normalProxy.Do();
            limitedProxy.Do();

            var normalLogContents = InfoLogContents;
            var limitedLogContents = LimitedLogContents;

            Assert.That(normalLogContents.Length, Is.EqualTo(2));
            Assert.That(normalLogContents[0].Contains("Proxii.NLog.Test.ILogTester"));
            Assert.That(normalLogContents[1].Contains("Proxii.NLog.Test.ILimitedLogTester"));

            Assert.That(limitedLogContents.Length, Is.EqualTo(1));
            Assert.That(limitedLogContents[0].Contains("Proxii.NLog.Test.ILimitedLogTester"));
        }
    }
}
