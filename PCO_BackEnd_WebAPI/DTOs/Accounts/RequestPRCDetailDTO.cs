using PCO_BackEnd_WebAPI.ValidationsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestPRCDetailDTO
    {
        [IsPRCIdValid]
        public string IdNumber { get; set; }

        [IsDateValid]
        public string ExpirationDate { get; set; }
    }
}