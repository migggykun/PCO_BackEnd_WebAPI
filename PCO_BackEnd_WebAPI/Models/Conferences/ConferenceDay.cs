using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    [Table("ConferenceDays")]
    public class ConferenceDay
    {
        public ConferenceDay()
        {
            ConferenceActivities = new HashSet<ConferenceActivity>();
        }

        [Key]
        public int Id { get; set; }

        public int ConferenceId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public virtual ICollection <ConferenceActivity> ConferenceActivities { get; set; }
    }
}