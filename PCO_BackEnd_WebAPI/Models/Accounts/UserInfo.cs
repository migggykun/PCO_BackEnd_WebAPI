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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [Required]
        public virtual Address Address{ get; set; }

        [StringLength(256)]
        public string Organization { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }
        public virtual MembershipType MembershipType { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [StringLength(256, ErrorMessage = "{0} length must be lesser than {1}.")]
        [Required]
        public string School { get; set; }

        [Required]
        public DateTime YearGraduated { get; set; }

    }
}
