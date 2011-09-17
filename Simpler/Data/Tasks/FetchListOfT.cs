using System.Collections.Generic;
using System.Data;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that will create a list of objects of the given type T using the results of the given command.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list to return.</typeparam>
    public class FetchListOf<T> : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual T[] ObjectsFetched { get; private set; }

        // Sub-tasks
        public virtual UseDataRecordToBuildInstanceOf<T> UseDataRecordToBuildInstanceOf { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (UseDataRecordToBuildInstanceOf == null) UseDataRecordToBuildInstanceOf = new UseDataRecordToBuildInstanceOf<T>();

            var objectList = new List<T>();

            using (var dataReader = SelectCommand.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    UseDataRecordToBuildInstanceOf.DataRecord = dataReader;
                    UseDataRecordToBuildInstanceOf.Execute();
                    objectList.Add(UseDataRecordToBuildInstanceOf.Object);
                }
            }

            ObjectsFetched = objectList.ToArray();
        }
    }
}
