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
    public partial class Registration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Registration()
        {
            DailyAttendanceRecords = new HashSet<DailyAttendanceRecord>();
        }

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ConferenceId { get; set; }

        public virtual Conference Conference { get; set; }

        public int RegistrationStatusId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyAttendanceRecord> DailyAttendanceRecords { get; set; }

        public virtual Payment RegistrationPayment { get; set; }

        public int? PromoId { get; set; }
        public virtual Promo Promo { get; set; }
    }
}
