using DataUploader_DadarToTaloja.Data;
using DataUploader_DadarToTaloja.Interfaces;
using System.Data;
using System.Data.SqlClient;
using DataUploader_DadarToTaloja.Models;

namespace DataUploader_DadarToTaloja.Services
{
    public class PIHoldExportService : IPIHoldExportService
    {
        private readonly LocalDbConnectionFactory _localDb;
        private readonly ServerDbConnectionFactory _serverDb;
        private readonly ILogger<PIHoldExportService> _logger;

        public PIHoldExportService(
            LocalDbConnectionFactory localDb,
            ServerDbConnectionFactory serverDb,
            ILogger<PIHoldExportService> logger)
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
                _logger.LogInformation("Fetching PI Hold Details");

                var records = await GetPIHoldRecordsAsync();

                if (!records.Any())
                    return 0;

                recordCount = records.Count;

                foreach (var item in records)
                {
                    await UpdateServerAsync(item);
                    await UpdateLocalAsync(item);
                }

                _logger.LogInformation("Completed PI Hold export");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting PI Hold data");
                recordCount = 0;
            }

            return recordCount;
        }

        private async Task<List<PIHoldDto>> GetPIHoldRecordsAsync()
        {
            var list = new List<PIHoldDto>();

            using var conn = _localDb.Create();
            using var cmd = new SqlCommand("USP_Export_PI_Hold", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new PIHoldDto
                {
                    PINO = reader["PINO"].ToString(),
                    IsClear = Convert.ToBoolean(reader["IsClear"])
                });
            }

            return list;
        }

        private async Task UpdateServerAsync(PIHoldDto item)
        {
            using var conn = _serverDb.Create();
            using var cmd = new SqlCommand("USP_UpdateWOHold", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@IsClear", item.IsClear);
            cmd.Parameters.AddWithValue("@PINO", item.PINO);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task UpdateLocalAsync(PIHoldDto item)
        {
            using var conn = _localDb.Create();
            using var cmd = new SqlCommand("USP_UpdateWODetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PINO", item.PINO);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
