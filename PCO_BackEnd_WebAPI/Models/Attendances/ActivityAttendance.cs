using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Attendances
{
    [Table("pc0_Database_Staging.[ActivityAttendance]")]
    public class ActivityAttendance
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConferenceActivityId { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime TimeOut { get; set; }

    }
}