using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Attendances
{
    [Table("ActivityAttendance")]
    public class ActivityAttendance
    {
        public ActivityAttendance()
        {
            TimeIn = null;
            TimeOut = null;
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConferenceActivityId { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

    }
}