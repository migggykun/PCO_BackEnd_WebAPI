using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestAccountDTO
    {
        [Required]
        [StringLength(128, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string Email { get; set; }

        [Required]
        [StringLength(28, ErrorMessage = "{0} length must be lesser than {1}.")]
        public string PhoneNumber { get; set; }


        public RequestPRCDetailDTO PRCDetail { get; set; }

        [Required]
        public RequestUserInfoDTO UserInfo { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}