using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Accounts;
namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    public partial class Rate
    {
        [Key]
        public int ratesId { get; set; }

        public int conferenceId { get; set; }

        public int membershipTypeId { get; set; }

        public int regularPrice { get; set; }

        public virtual Conference Conference { get; set; }

        public virtual MembershipType MembershipType { get; set; }
    }
}
