using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Bank
{
    public class RequestBankDetailDTO
    {
        public string Name { get; set; }

        public string AccountNumber { get; set; }

        public string Branch { get; set; }
    }
}