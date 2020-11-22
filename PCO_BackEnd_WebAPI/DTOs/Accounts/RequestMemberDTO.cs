using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestMemberDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime MemberSince { get; set; }
    }
}