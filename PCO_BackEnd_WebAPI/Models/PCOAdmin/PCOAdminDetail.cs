using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.PCOAdmin
{
    [Table("pc0_Database_Staging.[dbo.PCOAdmin]")]
    public class PCOAdminDetail
    {
        [Key]
        public int Id { get; set; }

        public double AnnualMembershipFee { get; set; }

        public string WebsitePassword { get; set; }
    }
}