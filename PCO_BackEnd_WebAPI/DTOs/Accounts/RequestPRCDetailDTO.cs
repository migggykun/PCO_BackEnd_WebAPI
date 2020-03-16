using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestPRCDetailDTO
    {
        [Required(AllowEmptyStrings= true)]
        public string IdNumber { get; set; }

        [Required(AllowEmptyStrings= true)]
        public string ExpirationDate { get; set; }
    }
}