using DataUploader_DadarToTaloja.Data;
using DataUploader_DadarToTaloja.Interfaces;
using System.Data;
using System.Data.SqlClient;
using DataUploader_DadarToTaloja.Models;

namespace DataUploader_DadarToTaloja.Services
{
    public class UserDetailsExportService: IUserDetailsExportService
    {
        private readonly LocalDbConnectionFactory _localDb;
        private readonly ServerDbConnectionFactory _serverDb;
        private readonly ILogger<UserDetailsExportService> _logger;

        public UserDetailsExportService(
            LocalDbConnectionFactory localDb,
            ServerDbConnectionFactory serverDb,
            ILogger<UserDetailsExportService> logger)
        {
            _localDb = localDb;
            _serverDb = serverDb;
            _logger = logger;
        }

        public async Task<int> ExportAsync()
        {
            int recordCount = 0;

            try
            {
                _logger.LogInformation("Fetching User Details");

                var records = await GetUserDetailsRecordsAsync();

                if (!records.Any())
                    return 0;

                recordCount = records.Count;

                foreach (var item in records)
                {
                    await UpdateServerAsync(item);
                    await UpdateLocalAsync(item);
                }

                _logger.LogInformation("Completed User Details");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting User Details data");
                recordCount = 0;
            }

            return recordCount;
        }

        private async Task<List<UserDetails>> GetUserDetailsRecordsAsync()
        {
            var list = new List<UserDetails>();

            using var conn = _localDb.Create();
            using var cmd = new SqlCommand("SP_GetUSP_Export_UserDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new UserDetails
                {
                   
                });
            }
            return list;
        }

        private async Task UpdateServerAsync(UserDetails item)
        {
            using var conn = _serverDb.Create();
            using var cmd = new SqlCommand("USP_UpdateWOHold", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            //cmd.Parameters.AddWithValue("@IsClear", item.IsClear);
            //cmd.Parameters.AddWithValue("@PINO", item.PINO);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task UpdateLocalAsync(UserDetails item)
        {
            using var conn = _localDb.Create();
            using var cmd = new SqlCommand("USP_UpdateUSerDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@UserID", item.UserID);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
