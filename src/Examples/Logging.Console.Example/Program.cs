using Logging.Unity.Interception;
using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

namespace Logging.Console.Example
{
    class Program
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        static void Main(string[] args)
        {
            Container.AddNewExtension<Interception>();
            Container.RegisterType<ISomeService, SomeService>(
                new Interceptor<InterfaceInterceptor>(), new InterceptionBehavior<LoggingInterceptionBehavior>());
            Execute();
            System.Console.Read();
        }

        private static async void Execute()
        {
            var service = Container.Resolve<ISomeService>();

            for (int i = 0; i < 5; i++)
            {
                service.Do();
                var result1 = service.DoWithResult();
                await service.DoAsync();
                var result2 = await service.DoAsyncWithResult();
            }
        }
    }
}
