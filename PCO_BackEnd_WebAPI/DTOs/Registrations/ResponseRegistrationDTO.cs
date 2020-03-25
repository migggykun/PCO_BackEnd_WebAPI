using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class ResponseRegistrationDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConferenceId { get; set; }

        public int RegistrationStatusId { get; set; }

        public int? PromoId { get; set; }

    }
}