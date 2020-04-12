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
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed!")]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed!")]
        public int membershipTypeId { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]+)?$", ErrorMessage = "Invalid amount!")]
        public double regularPrice { get; set; }
    }
}