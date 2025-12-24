using DataUploader_DadarToTaloja.Data;
using DataUploader_DadarToTaloja.Interfaces;
using System.Data;
using System.Data.SqlClient;
using DataUploader_DadarToTaloja.Models;

namespace DataUploader_DadarToTaloja.Services
{
    public class UserDetailsExportService : IUserDetailsExportService
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
                {
                    _logger.LogInformation("No User Details records found to export.");
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
                        // Log exception per record but continue with other records
                        _logger.LogError(ex, "Error exporting User Details for EmpID: {EmpID}", item.EmpID);
                    }
                }

                _logger.LogInformation("Completed exporting User Details. Total records processed: {Count}", recordCount);
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
                    list.Add(new UserDetails
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        EmpID = reader["EmpID"].ToString(),
                        UserName = reader["UserName"].ToString(),
                        Pswrd = reader["Pswrd"].ToString(),
                        UserType = reader["UserType"].ToString(),
                        Dept = reader["Dept"].ToString(),
                        Email_ID = reader["email_ID"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DOB = reader["DOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DOB"]),
                        IsActive = reader["IsActive"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching User Details from local database.");
            }

            return list;
        }

        private async Task UpdateServerAsync(UserDetails item)
        {
            try
            {
                using var conn = _serverDb.Create();
                using var cmd = new SqlCommand("SP_Insert_Export_UserDetails", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@EmpID", item.EmpID);
                cmd.Parameters.AddWithValue("@UserName", item.UserName);
                cmd.Parameters.AddWithValue("@Pswrd", item.Pswrd);
                cmd.Parameters.AddWithValue("@UserType", item.UserType);
                cmd.Parameters.AddWithValue("@Dept", item.Dept);
                cmd.Parameters.AddWithValue("@email_ID", item.Email_ID);
                cmd.Parameters.AddWithValue("@Gender", item.Gender);
                cmd.Parameters.AddWithValue("@DOB", item.DOB ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", item.IsActive ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsAdded", 1);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting User Details into server for EmpID: {EmpID}", item.EmpID);
                throw; 
            }
        }

        private async Task UpdateLocalAsync(UserDetails item)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating local User Details for UserID: {UserID}", item.UserID);
                throw; // rethrow so ExportAsync can log the record-specific error
            }
        }
    }
}
