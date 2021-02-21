using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    [Table("MemberRegistration")]
    public partial class MemberRegistration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MemberRegistration()
        {
        }

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public double? Amount { get; set; }

        public int MemberRegistrationStatusId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
