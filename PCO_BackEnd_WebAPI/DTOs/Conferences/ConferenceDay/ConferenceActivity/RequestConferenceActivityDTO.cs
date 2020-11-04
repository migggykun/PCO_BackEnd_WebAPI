using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class RequestConferenceActivityDTO
    {
        public int Id { get; set; }

        public int ConferenceDayId { get; set; }

        public RequestActivityScheduleDTO ActivitySchedule { get; set; }

        public virtual ICollection<RequestActivityRateDTO> ActivityRates { get; set; }
    }
}