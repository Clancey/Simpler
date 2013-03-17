using Castle.DynamicProxy;
using System;

namespace Simpler.Core.Tasks
{
    public class CreateTask : InOutTask<CreateTask.Input, CreateTask.Output>
    {
        public static readonly object SyncRoot = new Object();
        static volatile ProxyGenerator proxyGenerator;
        static ProxyGenerator ProxyGenerator
        {
            get
            {
                if (proxyGenerator == null)
                {
                    lock (SyncRoot)
                    {
                        if (proxyGenerator == null)
                            proxyGenerator = new ProxyGenerator();
                    }
                }

                return proxyGenerator;
            }
        }

        public class Input
        {
            public Type TaskType { get; set; }
            public ExecuteInterceptor ExecuteInterceptor { get; set; }
        }

        public class Output
        {
            public object TaskInstance { get; set; }
        }

        public ExecuteTask ExecuteTask { get; set; }

        public override void Execute()
        {
            lock (SyncRoot)
            {
                if (In.ExecuteInterceptor == null)
                {
                    In.ExecuteInterceptor = new ExecuteInterceptor(
                        In.TaskType.FullName,
                        invocation =>
                            {
                                if (ExecuteTask == null) ExecuteTask = new ExecuteTask();
                                ExecuteTask.In.Task = (Task) invocation.InvocationTarget;
                                ExecuteTask.In.Invocation = invocation;
                                ExecuteTask.Execute();
                            });
                }

                Out.TaskInstance = ProxyGenerator.CreateClassProxy(In.TaskType, In.ExecuteInterceptor);
            }
        }
    }
}
