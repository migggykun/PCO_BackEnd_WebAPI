using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class ConferenceDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int AttendanceLimit { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Banner { get; set; }

        public virtual PromoDTO Promo { get; set; }

        public virtual ICollection<RateDTO> Rates { get; set; }
    }
}