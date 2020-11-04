using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class UpdateConferenceDTO : BaseConferenceDTO
    {
        [Required]
        public virtual ICollection<UpdateRateWithConferenceDTO> Rates { get; set; }

        [Required]
        public virtual ICollection<RequestConferenceDayDTO> ConferenceDays { get; set; }
    }
}