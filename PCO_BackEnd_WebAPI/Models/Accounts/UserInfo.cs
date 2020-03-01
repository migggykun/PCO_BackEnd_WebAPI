using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    [Table("UserInfos")]
    public partial class UserInfo
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [StringLength(30)]
        public string firstName { get; set; }

        [Required]
        [StringLength(30)]
        public string middleName { get; set; }

        [Required]
        [StringLength(30)]
        public string lastName { get; set; }

        [Required]
        [StringLength(100)]
        public string address { get; set; }

        [StringLength(50)]
        public string organization { get; set; }

        public virtual ApplicationUser applicationUser { get; set; }
    }
}
