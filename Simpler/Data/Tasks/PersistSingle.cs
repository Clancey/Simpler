using System;
using System.Data;
using Simpler.Data.Exceptions;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that will persist the given object using the given command.  It is assumed that the command's CommandText
    /// contains parameters place holders that match up with properties in the given object to persist.
    /// </summary>
    public class PersistSingle : Task
    {
        // Inputs
        public virtual IDbCommand PersistCommand { get; set; }
        public virtual object ObjectToPersist { get; set; }

        // Sub-tasks
        public virtual BuildParameters BuildParameters { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (BuildParameters == null) BuildParameters = new BuildParameters();

            BuildParameters.CommandWithParameters = PersistCommand;
            BuildParameters.ObjectWithValues = ObjectToPersist;
            BuildParameters.Execute();

            var rowsPersisted = PersistCommand.ExecuteNonQuery();

            if (rowsPersisted != 1)
            {
                throw new ObjectPersistanceException(String.Format("Expected 1 row to be persisted, but actual count was {0}.", rowsPersisted));
            }
        }
    }
}
