using System.Data;
using System.Data.SqlClient;
namespace DataUploader_DadarToTaloja.Data
{
    public class ServerDbConnectionFactory
    {
        private readonly string _connectionString;

        public ServerDbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ServerConn");
        }
        public SqlConnection Create()
            => new SqlConnection(_connectionString);
    }
}
