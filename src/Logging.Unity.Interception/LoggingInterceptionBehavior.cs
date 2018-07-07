using Logging.Unity.Interception.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Logging.Unity.Interception
{

    public class LoggingInterceptionBehavior : IInterceptionBehavior
    {
        internal static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Stopwatch, Task>> _wrapperCreators =
            new ConcurrentDictionary<Type, Func<Task, IMethodInvocation, Stopwatch, Task>>();

        public IMethodReturn Invoke(IMethodInvocation input,
            GetNextInterceptionBehaviorDelegate getNext)
        {
            var sw = Stopwatch.StartNew();
            Log.Debug($"{BuildMethodInvocationDescr(input)} started");
            var result = getNext()(input, getNext);
            var method = input.MethodBase as MethodInfo;
            if (result.ReturnValue != null
                && method != null
                && typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                // If this method returns a Task, override the original return value
                var task = (Task)result.ReturnValue;
                return input.CreateMethodReturn(
                    this.GetWrapperCreator(method.ReturnType)(task, input, sw), result.Outputs);
            }

            Log.Debug($"{BuildMethodInvocationDescr(input)} finished {sw.Elapsed}");
            return result;
        }

        private Task CreateGenericWrapperTask<T>(Task task, IMethodInvocation input, Stopwatch sw)
        {
            return this.DoCreateGenericWrapperTask<T>((Task<T>)task, input, sw);
        }

        protected virtual string BuildMethodInvocationDescr(IMethodInvocation input)
        {
            return input.CreateMethodInvocationDescription();
        }

        protected virtual async Task<T> DoCreateGenericWrapperTask<T>(Task<T> task,
            IMethodInvocation input, Stopwatch sw)
        {
            try
            {
                return await task;
            }
            finally
            {
                Log.Debug($"{BuildMethodInvocationDescr(input)} finished {sw.Elapsed}");
            }
        }

        protected virtual async Task CreateWrapperTask(Task task,
            IMethodInvocation input, Stopwatch sw)
        {
            try
            {
                await task;
            }
            finally
            {
                Log.Debug($"{BuildMethodInvocationDescr(input)} finished {sw.Elapsed}");
            }

        }

        private Func<Task, IMethodInvocation, Stopwatch, Task> GetWrapperCreator(Type taskType)
        {
            return this._wrapperCreators.GetOrAdd(
                taskType,
                (Type t) =>
                {
                    if (t == typeof(Task))
                    {
                        return this.CreateWrapperTask;
                    }
                    else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        return (Func<Task, IMethodInvocation, Stopwatch, Task>)this.GetType()
                            .GetMethod("CreateGenericWrapperTask",
                                BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(new Type[] { t.GenericTypeArguments[0] })
                            .CreateDelegate(typeof(Func<Task, IMethodInvocation, Stopwatch, Task>), this);
                    }
                    else
                    {
                        // Other cases are not supported
                        return (task, a, _) => task;
                    }
                });
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }


        public bool WillExecute
        {
            get { return true; }
        }
    }
}
