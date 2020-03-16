using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class AddRateWithConferenceDTO
    {
        [Required]
        public int membershipTypeId { get; set; }

        [Required]
        public int regularPrice { get; set; }
    }
}