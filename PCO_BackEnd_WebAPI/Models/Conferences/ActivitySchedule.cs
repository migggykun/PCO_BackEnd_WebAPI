using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("pc0_Database_Staging.[dbo.ActivitySchedules]")]
    public class ActivitySchedule
    {
    
        public int Id { get; set; }

        public int ActivityId { get; set; }
        
        public virtual Activity Activity { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}