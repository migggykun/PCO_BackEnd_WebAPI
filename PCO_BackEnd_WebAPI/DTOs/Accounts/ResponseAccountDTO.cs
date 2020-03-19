using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class ResponseAccountDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public ResponsePRCDetailDTO PRCDetail { get; set; }

        public ResponseUserInfoDTO UserInfo { get; set; }

        public bool isAdmin { get; set; }
    }
}