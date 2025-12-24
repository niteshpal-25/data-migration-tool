using DataUploader_DadarToTaloja.Data;
using DataUploader_DadarToTaloja.Interfaces;
using System.Data;
using System.Data.SqlClient;
using DataUploader_DadarToTaloja.Models;

namespace DataUploader_DadarToTaloja.Services
{
    public class DepartmentExportService : IDepartmentDetailsExportService
    {
        private readonly LocalDbConnectionFactory _localDb;
        private readonly ServerDbConnectionFactory _serverDb;
        private readonly ILogger<DepartmentExportService> _logger;

        public DepartmentExportService(
            LocalDbConnectionFactory localDb,
            ServerDbConnectionFactory serverDb,
            ILogger<DepartmentExportService> logger)
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
                _logger.LogInformation("Fetching Department Details");

                var records = await GetUserDetailsRecordsAsync();

                if (!records.Any())
                {
                    _logger.LogInformation("No Department Details records found to export.");
                    return 0;
                }

                recordCount = records.Count;

                foreach (var item in records)
                {
                    try
                    {
                        await UpdateServerAsync(item);
                        await UpdateLocalAsync(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error exporting Department Details for EmpID: {EmpID}", item.DeptID);
                    }
                }

                _logger.LogInformation("Completed exporting Department Details. Total records processed: {Count}", recordCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting Department Details data");
                recordCount = 0;
            }

            return recordCount;
        }

        private async Task<List<Departments>> GetUserDetailsRecordsAsync()
        {
            var list = new List<Departments>();

            try
            {
                using var conn = _localDb.Create();
                using var cmd = new SqlCommand("SP_GetUSP_Export_UserDetails", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new Departments
                    {
                        DeptID = Convert.ToInt32(reader["DeptID"]),
                        DeptName = reader["DeptName"].ToString(),
                        Location = reader["Location"].ToString(),
                        CreatedDate = reader["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedDate"])                       
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Department Details from local database.");
            }

            return list;
        }

        private async Task UpdateServerAsync(Departments item)
        {
            try
            {
                using var conn = _serverDb.Create();
                using var cmd = new SqlCommand("SP_Insert_Export_UserDetails", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@EmpID", item.DeptID);
                cmd.Parameters.AddWithValue("@UserName", item.DeptName);
                cmd.Parameters.AddWithValue("@Pswrd", item.Location);
                cmd.Parameters.AddWithValue("@UserType", item.CreatedDate);
                cmd.Parameters.AddWithValue("@IsAdded", 1);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting Department Details into server for EmpID: {EmpID}", item.DeptID);
                throw;
            }
        }

        private async Task UpdateLocalAsync(Departments item)
        {
            try
            {
                using var conn = _localDb.Create();
                using var cmd = new SqlCommand("USP_UpdateUSerDetails", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@DeptID", item.DeptID);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating local Department Details for DeptID: {DeptID}", item.DeptID);
                throw;
            }
        }
    }
}
