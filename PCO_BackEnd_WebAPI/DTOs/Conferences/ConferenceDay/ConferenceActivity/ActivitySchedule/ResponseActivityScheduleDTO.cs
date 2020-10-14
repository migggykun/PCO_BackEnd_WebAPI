using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.DTOs.Activities;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class ResponseActivityScheduleDTO
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }

        public ResponseActivityDTO Activity { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}