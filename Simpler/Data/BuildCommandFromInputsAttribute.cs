using Castle.DynamicProxy;
using Simpler.Construction;

namespace Simpler.Data
{
    public class BuildSelectCommandFromInputs : ExecutionOverrideAttribute
    {
        public override void ExecutionOverride(IInvocation executeInvocation)
        {
            dynamic task = executeInvocation.InvocationTarget;
            if (task.Inputs != null)
            {
                var inputs = (FetchListInputs)(task.Inputs);
                using (var command = inputs.Connection.CreateCommand())
                {
                    command.CommandText = task.Inputs.Sql;
                    task.SelectCommand = command;

                    executeInvocation.Proceed();
                }
            }
            else
            {
                executeInvocation.Proceed();
            }
        }
    }
}