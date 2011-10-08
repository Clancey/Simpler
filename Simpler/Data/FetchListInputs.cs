using System.Data;
using Simpler.Data.Tasks;

namespace Simpler.Data
{
    // todo - just ideas

//    abstract class SqlTask : Task
//    {
//        // Inputs
//        public virtual IDbConnection OpenConnection { get; set; }
//    }

//    abstract class FetchListUsingSqlTask : SqlTask
//    {
//        // Sub-tasks
//        public BuildParameters BuildParameters { get; set; }
//        public FetchList FetchList { get; set; }

//        public override void Execute()
//        {
//            using (var command = OpenConnection.CreateCommand())
//            {
//                command.Connection = OpenConnection;

//                dynamic sqlTask = this;
//                command.CommandText = sqlTask.Sql;

//                // todo - look for a ConnectionName as well?

//                if (Inputs != null)
//                {
//                    BuildParameters.CommandWithParameters = command;
//                    BuildParameters.ObjectWithValues = Inputs;
//                    BuildParameters.Execute();
//                }

//                // Fetch the list of SomeStuff and set the output property.
//                FetchList.SelectCommand = command;
//                FetchList.Execute();
//                Outputs = new {FetchList.ObjectsFetched};
//            }
//        }
//    }

//    class FetchSomeStuff : FetchListUsingSqlTask
//    {
//        public string Sql =
//            @"
//            SELECT 
//                SomeStuff
//            FROM 
//                ABunchOfJoinedTables
//            WHERE 
//                OneOfTheTables.SomeColumn = @Parameters.SomeCriteria 
//            ";      
//    }

    //    class FetchSomeStuffUsingStatics : FetchListUsingSqlTask
    //    {
    //        public string Sql =
    //            @"
    //            SELECT 
    //                SomeStuff
    //            FROM 
    //                ABunchOfJoinedTables
    //            WHERE 
    //                OneOfTheTables.SomeColumn = @Parameters.SomeCriteria 
    //            ";

    //        // Outputs
    //        public SomeStuff SomeStuff { get; set; }

    //        public override void Execute()
    //        {
    //            base.Execute();
    //            SomeStuff = Outputs.ObjectsFetched;
    //        }
    //    }
    class Program
    {
        public Program()
        {
            var fetchList = TaskFactory<FetchList>.Create();
            var list = fetchList.Execute(inputs: new
            {
                ConnectionName = "MainDatabase",
                Sql =
                    @"
                    SELECT 
                        SomeStuff
                    FROM 
                        ABunchOfJoinedTables
                    WHERE 
                        OneOfTheTables.SomeColumn = @Parameters.SomeCriteria 
                    "
            }).Outputs;
        }
    }

    public class FetchListInputs
    {
        public IDbConnection Connection { get; set; }
        public string Sql { get; set; }
    }
}
