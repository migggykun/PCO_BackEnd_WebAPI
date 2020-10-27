using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class AddConferenceActivityDTO
    {
        public int ActivityScheduleId { get; set; }
        public AddActivityScheduleDTO ActivitySchedule { get; set; }
    }
}