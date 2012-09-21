﻿using System;
using Castle.DynamicProxy;

namespace Simpler.Core.Tasks
{
    public class FireEvents : InTask<FireEvents.Input>
    {
        public class Input
        {
            public Task Task { get; set; }
            public IInvocation Invocation { get; set; }
        }

        public override void Execute()
        {
            var callbackAttributes = Attribute.GetCustomAttributes(In.Task.GetType(), typeof (EventsAttribute));
            var overrideAttribute = Attribute.GetCustomAttribute(In.Task.GetType(), typeof (OverrideAttribute));

            try
            {
                foreach (var callbackAttribute in callbackAttributes)
                {
                    ((EventsAttribute)callbackAttribute).BeforeExecute(In.Task);
                }

                if (overrideAttribute != null)
                {
                    ((OverrideAttribute)overrideAttribute).ExecuteOverride(In.Invocation);
                }
                else
                {
                    In.Invocation.Proceed();
                }
            }
            catch (Exception exception)
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute) callbackAttributes[i]).OnError(In.Task, exception);
                }

                throw;
            }
            finally
            {
                for (var i = callbackAttributes.Length - 1; i >= 0; i--)
                {
                    ((EventsAttribute) callbackAttributes[i]).AfterExecute(In.Task);
                }
            }
        }
    }
}