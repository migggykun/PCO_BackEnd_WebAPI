using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class ResponseConferenceDayDTO
    {
        public int Id { get; set; }

        public int ConferenceId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public virtual ICollection<ResponseConferenceActivityDTO> ConferenceActivities { get; set; }
    }
}