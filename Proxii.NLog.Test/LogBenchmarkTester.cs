using System;

namespace Proxii.NLog.Test
{
    public interface ILogBenchmarkTester
    {
        void Do(int times);
    }

    public class LogBenchmarkTester : ILogBenchmarkTester
    {
        public void Do(int times)
        {
            var rng = new Random();

            // do arbitrary work so it actually takes time
            for (int i = 0; i < times; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var foo = rng.Next() * rng.Next();
                }
            }
        }
    }
}
