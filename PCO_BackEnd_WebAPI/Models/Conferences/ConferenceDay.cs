using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("pc0_Database_Staging.[dbo.ConferenceDays]")]
    public class ConferenceDay
    {
        public ConferenceDay()
        {
            ConferenceActivities = new HashSet<ConferenceActivity>();
        }

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ConferenceId { get; set; }

        public DateTime Date { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public virtual ICollection <ConferenceActivity> ConferenceActivities { get; set; }
    }
}