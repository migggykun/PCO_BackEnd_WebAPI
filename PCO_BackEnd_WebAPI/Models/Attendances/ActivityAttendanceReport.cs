using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Attendances
{
    public class ActivityAttendanceReport
    {
        public ActivityAttendanceReport() 
        {
            PRCId = null;
            PRCExpiration = null;
            TimeIn = null;
            TimeOut = null;
        }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string PRCId { get; set; }

        public DateTime? PRCExpiration { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public double? Amount { get; set; }

        public double? Discount { get; set; }

        public string RegistrationStatus { get; set; }

        public bool isBundle { get; set; }

    }
}