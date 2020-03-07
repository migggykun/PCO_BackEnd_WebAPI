using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class PRCDetailDTO
    {
        public string prcId { get; set; }

        public DateTime prc_expiration_date { get; set; }
    }
}