﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class UpdateConferenceDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int AttendanceLimit { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Banner { get; set; }

        public int PromoId { get; set; }

        public virtual ICollection<UpdateRateWithConferenceDTO> Rates { get; set; }
    }
}