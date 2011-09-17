using System.Data;
using System.Linq;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that will create a dynamic object using the result of the given command.  
    /// 
    /// Single() is used to make sure only one object is returned.  If you aren't sure that just one record 
    /// exists, you should use FetchListOf instead (and then call ObjectsFetched.FirstOrDefault(), for example).
    /// </summary>
    public class FetchSingle : Task
    {
        // Inputs
        public virtual IDbCommand SelectCommand { get; set; }

        // Outputs
        public virtual dynamic ObjectFetched { get; private set; }

        // Sub-tasks
        public virtual FetchList FetchList { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (FetchList == null) FetchList = new FetchList();

            FetchList.SelectCommand = SelectCommand;
            FetchList.Execute();
            ObjectFetched = FetchList.ObjectsFetched.Single();
        }
    }
}
