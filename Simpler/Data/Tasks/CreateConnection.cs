using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Simpler.Data.Tasks
{
    class CreateConnection : Task
    {
        // Inputs
        public string ConnectionStringSettingName{ get; set; }

        // Outputs
        public IDbConnection Connection { get; set; }

        public override void Execute()
        {
            var connectionStringSetting = ConfigurationManager.ConnectionStrings["ConnectionStringSettingName"];
            if (connectionStringSetting.ProviderName == "ConnectionStringSettingName")
            {
                Connection = new SqlConnection(connectionStringSetting.ConnectionString);
            }
        }
    }
}
