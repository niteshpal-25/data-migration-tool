using System;

namespace DataUploader_DadarToTaloja.Models
{
    public class UserDetails
    {
        public int UserID { get; set; }
        public string EmpID { get; set; }
        public string UserName { get; set; }
        public string Pswrd { get; set; }
        public string UserType { get; set; }
        public string Dept { get; set; }
        public string Email_ID { get; set; }        
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsAdded { get; set; }
    }
}
