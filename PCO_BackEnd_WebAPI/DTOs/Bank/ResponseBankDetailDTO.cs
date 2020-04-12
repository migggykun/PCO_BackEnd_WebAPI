using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Bank
{
    public class ResponseBankDetailDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AccountNo { get; set; }

        public string Branch { get; set; }
    }
}