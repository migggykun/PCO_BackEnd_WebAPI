using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    public partial class MembershipAssignment
    {
        [Key]
        public int Id { get; set; }

        public int membershipTypeId { get; set; }

        public virtual MembershipType MembershipType { get; set; }

        public virtual ApplicationUser applicationUser { get; set; }
    }
}
