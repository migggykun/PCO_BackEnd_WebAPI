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
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed.")]
        public int membershipTypeId { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]+)?$", ErrorMessage = "{0} is an invalid amount!")]
        public double regularPrice { get; set; }
    }
}