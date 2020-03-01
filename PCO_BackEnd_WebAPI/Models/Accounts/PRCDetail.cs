using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    public partial class PRCDetail
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [StringLength(20)]
        public string prcId { get; set; }

        [Column(TypeName = "date")]
        public DateTime prc_expiration_date { get; set; }

        public virtual ApplicationUser applicationUser { get; set; }
    }
}
