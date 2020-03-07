using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class MembershipTypeDTO
    {
        public int membershipTypeId { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The name should not be greater than 15 characters.")]
        public string membershipName { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "The name should not be greater than 128 characters.")]
        public string membershipDescription { get; set; }

    }
}