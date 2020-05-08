using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Security.Cryptography.Models
{
    [Table("ClientInfos")]
    public partial class ClientInfo
    {
        public int id { get; set; }

        [Required]
        [StringLength(20)]
        public string salt { get; set; }

        [Required]
        [StringLength(100)]
        public string clientId { get; set; }

        [Required]
        [StringLength(100)]
        public string clientSecret { get; set; }

        [Required]
        [StringLength(10)]
        public string type { get; set; }
    }
}
