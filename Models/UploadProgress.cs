namespace DataUploader_DadarToTaloja.Models
{
    public class UploadProgress
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime StartTime { get; set; }

    }
}
