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
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public RequestPRCDetailDTO PRCDetail { get; set; }

        [Required]
        public RequestUserInfoDTO UserInfo { get; set; }
    }
}