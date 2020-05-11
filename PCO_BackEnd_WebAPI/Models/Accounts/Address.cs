using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    [Table("Addresses")]
    public partial class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string StreetAddress { get; set; }

        [Required]
        [StringLength(128)]
        public string Barangay { get; set; }

        [Required]
        [StringLength(128)]
        public string City { get; set; }

        [Required]
        [StringLength(128)]
        public string Province { get; set; }

        [Required]
        [StringLength(4)]
        public string Zipcode { get; set; }
    }
}