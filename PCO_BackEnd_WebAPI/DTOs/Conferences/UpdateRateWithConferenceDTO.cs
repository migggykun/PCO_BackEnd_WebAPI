using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class UpdateRateWithConferenceDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int membershipTypeId { get; set; }

        [Required]
        public int regularPrice { get; set; }
    }
}