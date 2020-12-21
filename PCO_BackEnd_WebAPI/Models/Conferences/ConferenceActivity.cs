using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Registrations;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("pc0_Database_Staging.[dbo.ConferenceActivities]")]
    public class ConferenceActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ConferenceDayId { get; set; }

        public int ActivityScheduleId { get; set; }
        
        public virtual ActivitySchedule ActivitySchedule { get; set; }

        public virtual ICollection<Rate> ActivityRates { get; set; }

        public virtual ICollection<ActivitiesToAttend> ActivitiesToAttend { get; set; }
    }
}