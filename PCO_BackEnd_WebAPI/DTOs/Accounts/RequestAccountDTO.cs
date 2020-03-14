using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestAccountDTO
    {

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public RequestPRCDetailDTO PRCDetail { get; set; }

        public RequestUserInfoDTO UserInfo { get; set; }
    }
}