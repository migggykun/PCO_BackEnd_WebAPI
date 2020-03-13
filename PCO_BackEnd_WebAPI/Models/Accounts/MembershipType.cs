using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    public partial class MembershipType
    {

        public MembershipType()
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }
    }
}
