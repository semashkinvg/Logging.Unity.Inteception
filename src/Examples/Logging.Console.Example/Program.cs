using Logging.Unity.Interception;
using System;
using System.Collections.Generic;
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
                service.DoWithParams(1, 123.2312, null);
                service.DoWithParams(1, 3214123.2312,
                    new List<Tuple<string, int, object, DateTime>>()
                    {
                        new Tuple<string, int, object, DateTime>("a", 1, 2, DateTime.MinValue)
                    });
                var result1 = service.DoWithResult();
                await service.DoAsync();

                await service.DoWithParamsAsync(1, 123.2312, null);
                await service.DoWithParamsAsync(1, 3214123.2312,
                    new List<Tuple<string, int, object, DateTime>>()
                    {
                        new Tuple<string, int, object, DateTime>("a", 1, 2, DateTime.MinValue)
                    });

                var result2 = await service.DoAsyncWithResult();
            }
        }
    }
}
