using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Bank
{
    public class RequestAddBankDetailDTO
    {
        [Required]
        public string AccountName { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string BankName { get; set; }
    }
}