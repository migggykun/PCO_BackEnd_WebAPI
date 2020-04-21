using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Security.DTO
{
    public class UserInformationDTO
    {
        public string Token { get; set; }

        public bool IsAdmin { get; set; }
    }
}