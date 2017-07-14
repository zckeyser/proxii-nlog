namespace Proxii.NLog.Test
{
    public interface ILimitedLogTester
    {
        void Do();
    }

    public class LimitedLogTester : ILimitedLogTester
    {
        public void Do()
        {
            // we don't actually need this to do anything,
            // we just want to make sure its logging gets
            // invoked correctly
        }
    }
}
