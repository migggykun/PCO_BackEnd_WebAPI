using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    public partial class MembershipType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MembershipType()
        {
        }

        public int membershipTypeId { get; set; }

        [Required]
        [StringLength(15)]
        public string membershipName { get; set; }

        [StringLength(128)]
        public string membershipDescription { get; set; }
    }
}
