using PCO_BackEnd_WebAPI.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class ResponseListRegistrationDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ConferenceId { get; set; }

        [Required]
        public int RegistrationStatusId { get; set; }

        public int?  PromoId { get; set; }

        public double? Amount { get; set; }

        public double? Discount { get; set; }

        [Required]
        public ResponseAccountDTO User { get; set; }
    }
}