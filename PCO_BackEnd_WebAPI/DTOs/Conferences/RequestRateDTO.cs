using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class RequestRateDTO
    {
        [Required]
        public int conferenceId { get; set; }

        [Required]
        public int membershipTypeId { get; set; }

        [Required]
        public int regularPrice { get; set; }
    }
}