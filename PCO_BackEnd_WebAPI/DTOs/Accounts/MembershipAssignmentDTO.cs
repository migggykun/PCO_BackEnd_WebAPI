using PCO_BackEnd_WebAPI.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class MembershipAssignmentDTO
    {
        public int Id { get; set; }

        public int membershipTypeId { get; set; }

        public virtual MembershipType MembershipType { get; set; }
    }
}