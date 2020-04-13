using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class ResponseAccountDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [Required]
        public string PhoneNumber { get; set; }


        public ResponsePRCDetailDTO PRCDetail { get; set; }

        [Required]
        public ResponseUserInfoDTO UserInfo { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public DateTime MemberSince { get; set; }
    }
}