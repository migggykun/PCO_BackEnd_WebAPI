using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class ResponsePRCDetailDTO
    {
        public string Id { get; set; }

        public string IdNumber { get; set; }

        public string ExpirationDate { get; set; }
    }
}