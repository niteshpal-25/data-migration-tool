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
        public DateTime? LastLoggedIn { get; set; }
        public DateTime? LastLoggedOut { get; set; }
        public bool? IsloggedOut { get; set; }

        public string MailDomain { get; set; }
        public string Email_ID { get; set; }
        public string Email_Pswrd { get; set; }
        public short? PortNo { get; set; }

        public string AccountName { get; set; }
        public string ContactNo { get; set; }

        public bool? POApproval { get; set; }
        public bool? IndentApproval { get; set; }
        public bool? QuotationApproval { get; set; }
        public bool? IsPOMailSent { get; set; }
        public bool? GRNreminderMail { get; set; }

        public DateTime? PswrdLastModifiedOn { get; set; }
        public bool? IsUploaded { get; set; }

        public string OldUsername { get; set; }
        public bool? ShowPendWOMsg { get; set; }
        public DateTime? SnoozeTime { get; set; }

        public string Gender { get; set; }
        public DateTime? DOB { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsSuperUser { get; set; }
        public int? IsMessengerApplicable { get; set; }

        public string Version { get; set; }
        public bool? IsSwitch { get; set; }
        public string FileName { get; set; }

        public int? SalesPersonID { get; set; }
        public string UType { get; set; }
        public bool? IsFGUser { get; set; }

        public string Coordinator { get; set; }
        public int? AppForDifferentiator { get; set; }
        public int? TL_ID { get; set; }
        public bool? IsAdded { get; set; }
    }
}
