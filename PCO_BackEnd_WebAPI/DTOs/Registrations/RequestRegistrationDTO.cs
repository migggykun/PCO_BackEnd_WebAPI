using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class RequestRegistrationDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ConferenceId { get; set; }

        public int? PromoId { get; set; }
    }
}