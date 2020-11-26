using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class ResponseConferenceActivityDTO
    {
        public int Id { get; set; }

        public int ConferenceDayId { get; set; }

        public int ActivityScheduleId { get; set; }
        public ResponseActivityScheduleDTO ActivitySchedule { get; set; }

        public ICollection<ResponseRateDTO> ActivityRates { get; set; }
    }
}