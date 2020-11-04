using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class ResponseActivityRateDTO
    {
        [Required]
        public int membershipTypeId { get; set; }

        [Required]
        public double regularPrice { get; set; }
    }
}