using PCO_BackEnd_WebAPI.DTOs.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class RequestActivityScheduleDTO
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }
        
        public RequestActivityDTO Activity { get; set; } 

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }
}