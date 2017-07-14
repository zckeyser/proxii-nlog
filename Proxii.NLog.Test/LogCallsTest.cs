using NUnit.Framework;
using System.IO;

namespace Proxii.NLog.Test
{
    [TestFixture]
    public class LogCallsTest
    {
        [TearDown]
        public void TearDown()
        {
            // delete temp files created by test logging
            var dir = new DirectoryInfo("c:\temp");

            foreach (var file in dir.EnumerateFiles("proxii-nlog-test-*.txt"))
            {
                file.Delete();
            }
        }

        [Test]
        public void LogCalls_MethodNameDetailLevel()
        {

        }

        [Test]
        public void LogCalls_ArgsDetailLevel()
        {

        }

        [Test]
        public void LogCalls_FormatString()
        {

        }

        [Test]
        public void LogCalls_WarnsOnEmptyFormatString()
        {

        }

        [Test]
        public void LogCalls_UsesLogLevel()
        {

        }

        [Test]
        public void LogCalls_DefaultsToInfoLogLevel()
        {

        }

        [Test]
        public void LogCalls_UsesCorrectLogger()
        {

        }
    }
}
