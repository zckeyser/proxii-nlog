namespace Proxii.NLog.Test
{
    public interface ILogTester
    {
        void Do();
        void Do(string s, int i);
    }

    public class LogTester : ILogTester
    {
        public void Do()
        {
            
        }

        public void Do(string s, int i)
        {
            
        }
    }
}
