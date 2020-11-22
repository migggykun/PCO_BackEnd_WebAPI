using PCO_BackEnd_WebAPI.ValidationsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestUserInfoDTO
    {

        [Required]
        [Display(Name = "First name")]
        [StringLength(512, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle initial")]
        [StringLength(256, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(256, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string LastName { get; set; }
   
        [Required]
        public RequestAddressDTO Address { get; set; }

        [StringLength(256, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string Organization { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [StringLength(256, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string School { get; set; }

        [Required]
        [IsDateValid]
        public DateTime YearGraduated { get; set; }
    }
}