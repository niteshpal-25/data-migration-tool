namespace DataUploader_DadarToTaloja.Models
{
    public class Departments
    {
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public string Location { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsAdded { get; set; }
    }
}
