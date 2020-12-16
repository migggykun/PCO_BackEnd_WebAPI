using System;

namespace PCO_BackEnd_WebAPI.Models.Attendances
{
    public class ActivityAttendanceReport
    {
        public ActivityAttendanceReport() 
        {
            PrcId = null;
            PrcExpiration = null;
            TimeIn = null;
            TimeOut = null;
        }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string PrcId { get; set; }

        public DateTime? PrcExpiration { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public double? Amount { get; set; }

        public double? Discount { get; set; }

        public string RegistrationStatus { get; set; }

        public bool isBundle { get; set; }

        public int? CpdUnits { get; set; }

        public string CpdAccreditationNumber { get; set; }
        
        public string ActivityName { get; set; }
        
        public DateTime? ActivityDate { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        public string Organization { get; set; }
        
        public string PcoMembershipStatus { get; set; }
        
        public string PcoMembershipType { get; set; }
    }
}