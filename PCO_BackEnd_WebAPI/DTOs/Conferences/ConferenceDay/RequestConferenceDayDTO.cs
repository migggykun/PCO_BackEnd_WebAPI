using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class RequestConferenceDayDTO
    {
        public int Id { get; set; }

        public int ConferenceId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public virtual ICollection<RequestConferenceActivityDTO> ConferenceActivities { get; set; }

    }       
}