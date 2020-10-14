using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Conferences;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("pc0_Database_Staging.[dbo.ConferenceActivities]")]
    public class ConferenceActivity
    {
        public int Id { get; set; }

        public int ConferenceDayId { get; set; }

        public int ActivityScheduleId { get; set; }
        
        public virtual ActivitySchedule ActivitySchedule { get; set; }
    }
}