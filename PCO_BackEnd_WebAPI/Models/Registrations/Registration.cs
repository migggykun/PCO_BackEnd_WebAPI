using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Accounts;
namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public partial class Registration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Registration()
        {
            DailyAttendanceRecords = new HashSet<DailyAttendanceRecord>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ConferenceId { get; set; }

        public virtual Conference Conference { get; set; }

        public DateTime? RegistrationDate { get; set; }

        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyAttendanceRecord> DailyAttendanceRecords { get; set; }

        public virtual RegistrationPayment RegistrationPayment { get; set; }
    }
}
