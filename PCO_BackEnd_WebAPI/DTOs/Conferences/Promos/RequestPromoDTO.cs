using PCO_BackEnd_WebAPI.ValidationsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences.Promos
{
    public class RequestPromoDTO
    {
        [Required]
        [Display(Name = "Promo name")]
        [StringLength(128, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Promo description")]
        [StringLength(512, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Start date is invalid!")]
        [IsDateValid]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "End date is invalid!")]
        [IsDateValid]
        public DateTime End { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{1,9}(?:\.[0-9]{1,2})?$", ErrorMessage = "Invalid amount!")]
        public double Amount { get; set; }

        [Required]
        public List<int> MembershipTypeIds { get; set; }
    }
}