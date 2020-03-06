using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.CustomValidations;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    public partial class Conference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Conference()
        {
            Rates = new HashSet<Rate>();
        }

        public int conferenceId { get; set; }

        [Required]
        [StringLength(20)]
        [Index("IX_Conferences_Title", IsUnique = true)]
        public string title { get; set; }

        [Required]
        [StringLength(100)]
        public string description { get; set; }

        public int attendance_limit { get; set; }

        [Required]
        [IsStartDateWithinEndDate]
        public DateTime start_date { get; set; }

        [Required]
        public DateTime end_date { get; set; }

        [MaxLength(255)]
        public byte[] banner { get; set; }

        public virtual Promo Promo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
