using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Attendances
{
    public class ConferenceAttendanceHistory: ConferenceAttendanceReport
    {
        [Key]
        public int ConferenceId { get; set; }
        public string ConferenceName { get; set; }
    }
}