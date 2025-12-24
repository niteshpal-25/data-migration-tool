namespace DataUploader_DadarToTaloja.Interfaces
{
    public interface IPIHoldExportService
    {
        Task<int> ExportAsync();
    }
}
