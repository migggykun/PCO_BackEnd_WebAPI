using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestPRCDetailDTO
    {
        public string IdNumber { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}