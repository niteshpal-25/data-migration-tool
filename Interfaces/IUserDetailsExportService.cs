namespace DataUploader_DadarToTaloja.Interfaces
{
    public interface IUserDetailsExportService
    {
        Task<int> ExportAsync();
    }
}
