using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class BaseConferenceDTO
    {
        [Required]
        [StringLength(128, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(512, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        [StringLength(512, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date.")]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date.")]
        public DateTime End { get; set; }

        [StringLength(512, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Banner { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed.")]
        public int? PromoId { get; set; }
    }
}