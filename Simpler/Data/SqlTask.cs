using System;
using System.Data;
using Simpler.Data.Tasks;

namespace Simpler.Data
{
    // todo - just ideas

    abstract class SqlTask : Task
    {
        // Inputs
        public virtual IDbConnection OpenConnection { get; set; }
        public virtual object Parameters { get; set; }
    }

    abstract class FetchListUsingSqlTask : SqlTask
    {
        // Sub-tasks
        public BuildParameters BuildParameters { get; set; }
        public FetchList FetchList { get; set; }

        public override void Execute()
        {
            using (var command = OpenConnection.CreateCommand())
            {
                command.Connection = OpenConnection;

                dynamic sqlTask = this;
                command.CommandText = sqlTask.Sql;

                // todo - look for a ConnectionName as well?

                if (Parameters != null)
                {
                    BuildParameters.CommandWithParameters = command;
                    BuildParameters.ObjectWithValues = this;
                    BuildParameters.Execute();
                }

                // Fetch the list of SomeStuff and set the output property.
                FetchList.SelectCommand = command;
                FetchList.Execute();
                Outputs = FetchList.ObjectsFetched;
            }
        }
    }

    class FetchSomeStuff : FetchListUsingSqlTask
    {
        public string Sql =
            @"
            SELECT 
                SomeStuff
            FROM 
                ABunchOfJoinedTables
            WHERE 
                OneOfTheTables.SomeColumn = @Parameters.SomeCriteria 
            ";      
    }

    abstract class PersistSingleUsingSqlTask : SqlTask
    {
        // Sub-tasks
        public BuildParameters BuildParameters { get; set; }
        public PersistSingle PersistSingle { get; set; }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    class PersistSomeStuff : PersistSingleUsingSqlTask
    {
        public string Sql =
            @"
            UPDATE 
                SomeStuff
            SET 
                SomeColumn = @SomeColumnNewValue
            WHERE 
                SomeTable.SomeColumnOtherColumn = @Parameters.SomeCriteria 
            ";      
    }
}
