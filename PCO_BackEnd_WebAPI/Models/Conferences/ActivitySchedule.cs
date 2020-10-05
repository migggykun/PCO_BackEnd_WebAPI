using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Conferences.Activities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("pc0_Database_Staging.[dbo.ActivitySchedules]")]
    public class ActivitySchedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}