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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string IdNumber{ get; set; }

        [Column(TypeName = "date")]
        public DateTime ExpirationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime RegistrationDate { get; set; }
    }
}
