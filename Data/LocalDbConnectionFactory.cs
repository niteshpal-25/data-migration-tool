using System.Data;
using System.Data.SqlClient;
namespace DataUploader_DadarToTaloja.Data
{
    public class LocalDbConnectionFactory
    {
        private readonly string _connectionString;

        public LocalDbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LocalConn");
        }

        public SqlConnection Create()
            => new SqlConnection(_connectionString);
    }
}
