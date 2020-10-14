using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class ResponseActivityScheduleDTO
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }

        //public ResponseActivityDTO Activity { get; set; } to do: uncomment after activity DTO is created.

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}