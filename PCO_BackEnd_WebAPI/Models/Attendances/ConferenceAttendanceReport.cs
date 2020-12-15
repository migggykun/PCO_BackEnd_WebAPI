using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Attendances
{
    public class ConferenceAttendanceReport: ActivityAttendanceReport 
    {
        public ConferenceAttendanceReport()
        {
            ActivityDate = null;
        }
        public string ActivityName { get; set; }

        public DateTime? ActivityDate { get; set; }
    }
}