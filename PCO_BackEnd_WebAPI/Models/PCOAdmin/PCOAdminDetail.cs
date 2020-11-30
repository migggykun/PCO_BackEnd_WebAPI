using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.PCOAdmin
{
    public class PCOAdminDetail
    {
        public double AnnualMembershipFee { get; set; }

        public string WebsitePassword { get; set; }
    }
}