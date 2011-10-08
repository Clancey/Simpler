using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    // todo - rename to Fetch and have task return Outputs (instead of Task)? would allow this
            //Fetch.Execute(
            //    new
            //    {
            //        Sql = "select something"
            //    })
            //    .ToArray();
    
    /// <summary>
    /// Task that will create a list of dynamic objects using the results of the given command.
    /// </summary>
    [BuildSelectCommandFromInputs]
    public class FetchList : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual dynamic[] ObjectsFetched { get; private set; }

        // Sub-tasks
        public virtual UseDataRecordToBuildInstance UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (UseDataRecordToBuild == null) UseDataRecordToBuild = new UseDataRecordToBuildInstance();

            //if (Inputs != null)
            //{
            //    var inputs = (FetchListInputs)(Inputs);
            //    SelectCommand = inputs.Connection.CreateCommand();
            //    SelectCommand.CommandText = Inputs.Sql;
            //}

            var objectList = new List<dynamic>();
            using (var dataReader = SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    UseDataRecordToBuild.DataRecord = dataReader;
                    UseDataRecordToBuild.Execute();
                    objectList.Add(UseDataRecordToBuild.Object);
                }
            }
            ObjectsFetched = objectList.ToArray();
        }
    }
}
