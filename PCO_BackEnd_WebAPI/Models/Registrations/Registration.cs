using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public partial class Registration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Registration()
        {
            DailyAttendanceRecords = new HashSet<DailyAttendanceRecord>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int registrationId { get; set; }

        [Required]
        public int userId { get; set; }

        public int conferenceId { get; set; }

        public DateTime? registrationDate { get; set; }

        public virtual Conference Conference { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyAttendanceRecord> DailyAttendanceRecords { get; set; }

        public virtual RegistrationPayment RegistrationPayment { get; set; }
    }
}
