namespace DataUploader_DadarToTaloja.Models
{
    public class Project
    {
        public int project_id { get; set; }
        public string project_name { get; set; }
        public string dept_id { get; set; }
        public bool isactive { get; set; }
        public bool IsAdded { get; set; }
    }
}
