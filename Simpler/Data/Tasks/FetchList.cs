using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that will create a list of objects of the given type T using the results of the given command.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list to return.</typeparam>
    public class FetchList : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual dynamic[] ObjectsFetched { get; private set; }

        // Sub-tasks
        public virtual UseDataRecordToBuildDynamic UseDataRecordToBuild { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (UseDataRecordToBuild == null) UseDataRecordToBuild = new UseDataRecordToBuildDynamic();

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
