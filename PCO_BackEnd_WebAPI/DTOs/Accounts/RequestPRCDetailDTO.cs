using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestPRCDetailDTO
    {
        [Required(AllowEmptyStrings= true)]
        [StringLength(64, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string IdNumber { get; set; }

        [Required(AllowEmptyStrings= true)]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date.")]
        public string ExpirationDate { get; set; }
    }
}