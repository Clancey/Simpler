using System;
using System.Data;

namespace Simpler.Data.Jobs
{
    public class ExecuteAction : InJob<ExecuteAction.Input>
    {
        public override void Specs()
        {
            // todo
        }

        public class Input
        {
            public IDbConnection Connection { get; set; }
            public string Sql { get; set; }
            public object Values { get; set; }
            public Action<IDbCommand> Action { get; set; }
        }

        public BuildParameters BuildParameters { get; set; }

        public override void Run()
        {
            Check.That(!String.IsNullOrEmpty(In.Sql), "Sql property must be set.");

            using (var command = In.Connection.CreateCommand())
            {
                if (In.Connection.State != ConnectionState.Open)
                {
                    In.Connection.Open();
                }

                command.Connection = In.Connection;
                command.CommandText = In.Sql;

                if (In.Values != null)
                {
                    BuildParameters.Command = command;
                    BuildParameters.Values = In.Values;
                    BuildParameters.Run();
                }

                In.Action(command);
            }
        }
    }
}
