using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Proxii.NLog.Test
{
    public class BaseTest
    {
        protected const string TempDirectory = @"C:\temp";
        protected const string TestDirectory = @"C:\temp\proxii-nlog-test";
        protected const string InfoLogFilePath = @"C:\temp\proxii-nlog-test\info.log";
        protected const string TraceLogFilePath = @"C:\temp\proxii-nlog-test\trace.log";
        protected const string LimitedLogFilePath = @"C:\temp\proxii-nlog-test\limited.log";

        protected string[] InfoLogContents => File.ReadAllLines(InfoLogFilePath);
        protected string[] TraceLogContents => File.ReadAllLines(TraceLogFilePath);
        protected string[] LimitedLogContents => File.ReadAllLines(LimitedLogFilePath);

        [SetUp]
        public void SetUp()
        {
            // in case a previous run got cut short
            if (Directory.Exists(TestDirectory))
                Directory.Delete(TestDirectory, true);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(TestDirectory, true);

            // get rid of the temp directory if our folder was the only thing in it
            if (!Directory.EnumerateFiles(TempDirectory).Any())
                Directory.Delete(TempDirectory);
        }
    }
}
