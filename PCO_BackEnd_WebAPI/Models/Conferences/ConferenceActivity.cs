using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Registrations;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("ConferenceActivities")]
    public class ConferenceActivity
    {
        [Key]
        public int Id { get; set; }

        public int ConferenceDayId { get; set; }

        public int ActivityScheduleId { get; set; }
        
        public virtual ActivitySchedule ActivitySchedule { get; set; }

        public virtual ICollection<Rate> ActivityRates { get; set; }

        public virtual ICollection<ActivitiesToAttend> ActivitiesToAttend { get; set; }
    }
}