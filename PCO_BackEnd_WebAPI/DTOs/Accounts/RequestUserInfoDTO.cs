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
        [StringLength(512, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string FirstName { get; set; }

        [StringLength(256, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string MiddleName { get; set; }

        [StringLength(256, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string LastName { get; set; }
   
        [Required]
        public RequestAddressDTO Address { get; set; }

        [StringLength(512, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string Organization { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }
    }
}