using System.Threading;
using System.Threading.Tasks;

namespace Logging.Console.Example
{
    public interface ISomeService
    {
        void Do();
        int DoWithResult();
        Task DoAsync();
        Task<int> DoAsyncWithResult();

    }

    public class SomeService : ISomeService
    {
        public void Do()
        {
            Thread.Sleep(100);
        }

        public int DoWithResult()
        {
            Thread.Sleep(100);
            return 100;
        }

        public async Task DoAsync()
        {
            await Task.Delay(100);
        }

        public async Task<int> DoAsyncWithResult()
        {
            await Task.Delay(100);
            return 100;
        }
    }
}
