using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestAddressDTO
    {
        [Required]
        [StringLength(128, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string StreetAddress { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string Barangay { get; set; }

        [StringLength(128, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string City { get; set; }

        [StringLength(128, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string Province { get; set; }

        [StringLength(4, ErrorMessage = "{0} length must be lesser than {1}.", MinimumLength = 4)]
        [Required]
        public string Zipcode { get; set; }

    }
}