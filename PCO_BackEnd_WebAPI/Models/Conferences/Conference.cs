using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    public partial class Conference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Conference()
        {
            Rates = new HashSet<Rate>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Index("IX_Conferences_Title", IsUnique = true)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public int AttendanceLimit { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public string Banner { get; set; }

        public virtual Promo Promo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
