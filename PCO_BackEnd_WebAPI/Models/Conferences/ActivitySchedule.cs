using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("ActivitySchedules")]
    public class ActivitySchedule
    {
        [Key]
        public int Id { get; set; }

        public int ActivityId { get; set; }
        
        public virtual Activity Activity { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}