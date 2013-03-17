using System;
using Castle.DynamicProxy;

namespace Simpler.Core
{
    public class ExecuteInterceptor : IInterceptor
    {
        public ExecuteInterceptor(string name, Action<IInvocation> action)
        {
            _name = name;
            _action = action;
        }

        string _name;
        Action<IInvocation> _action;
        int _count = 0;

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.Equals("Execute"))
            {
                //_count++;
                //if (_count > 1)
                //{
                //    throw new Exception("what?");
                //}

                _action(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}