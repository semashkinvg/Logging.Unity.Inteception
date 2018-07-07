using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Console.Example
{
    public interface ISomeService
    {
        void Do();
        void DoWithParams(int a, double b, IEnumerable<Tuple<string, int, object, DateTime>> c);
        int DoWithResult();
        Task DoAsync();
        Task DoWithParamsAsync(int a, double b, IEnumerable<Tuple<string, int, object, DateTime>> c);
        Task<int> DoAsyncWithResult();

    }

    public class SomeService : ISomeService
    {
        public void Do()
        {
            Thread.Sleep(100);
        }

        public void DoWithParams(int a, double b, IEnumerable<Tuple<string, int, object, DateTime>> c)
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

        public async Task DoWithParamsAsync(int a, double b, IEnumerable<Tuple<string, int, object, DateTime>> c)
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
