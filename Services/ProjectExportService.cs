using DataUploader_DadarToTaloja.Data;
using DataUploader_DadarToTaloja.Interfaces;
using System.Data;
using System.Data.SqlClient;
using DataUploader_DadarToTaloja.Models;

namespace DataUploader_DadarToTaloja.Services
{
    public class ProjectExportService : IProjectDetailsExportService
    {
        private readonly LocalDbConnectionFactory _localDb;
        private readonly ServerDbConnectionFactory _serverDb;
        private readonly ILogger<ProjectExportService> _logger;

        public ProjectExportService(
            LocalDbConnectionFactory localDb,
            ServerDbConnectionFactory serverDb,
            ILogger<ProjectExportService> logger)
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
                _logger.LogInformation("Fetching project Details");

                var records = await GetProjectRecordsAsync();

                if (!records.Any())
                {
                    _logger.LogInformation("No project Details records found to export.");
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
                        _logger.LogError(ex, "Error exporting project Details for project_id: {project_id}", item.project_id);
                    }
                }

                _logger.LogInformation("Completed exporting project Details. Total records processed: {Count}", recordCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting project Details data");
                recordCount = 0;
            }

            return recordCount;
        }

        private async Task<List<Project>> GetProjectRecordsAsync()
        {
            var list = new List<Project>();

            try
            {
                using var conn = _localDb.Create();
                using var cmd = new SqlCommand("SP_GetUSP_Export_project", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new Project
                    {
                        project_id = Convert.ToInt32(reader["project_id"]),
                        project_name = reader["project_name"].ToString(),
                        dept_id = reader["dept_id"].ToString(),
                        isactive = Convert.ToBoolean(reader["isactive"])
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project Details from local database.");
            }

            return list;
        }

        private async Task UpdateServerAsync(Project item)
        {
            try
            {
                using var conn = _serverDb.Create();
                using var cmd = new SqlCommand("SP_Insert_Export_project", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                cmd.Parameters.AddWithValue("@project_name", item.project_name);
                cmd.Parameters.AddWithValue("@dept_id", item.dept_id);
                cmd.Parameters.AddWithValue("@isactive", item.isactive);
                cmd.Parameters.AddWithValue("@IsAdded", 1);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting Department Details into server for project_id: {project_id}", item.project_id);

                throw;
            }
        }

        private async Task UpdateLocalAsync(Project item)
        {
            try
            {
                using var conn = _localDb.Create();
                using var cmd = new SqlCommand("USP_UpdateProject", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@project_id", item.project_id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating local Department Details for DeptID: {DeptID}", item.project_id);
                throw;
            }
        }
    }
}
