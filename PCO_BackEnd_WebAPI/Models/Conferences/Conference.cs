using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Images;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    public partial class Conference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Conference()
        {
            Rates = new HashSet<Rate>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [Index("IX_Conferences_Title", IsUnique = true)]
        public string Title { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public  Banner Banner { get; set; }

        public int? PromoId { get; set; }
        public virtual Promo Promo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rate> Rates { get; set; }

        public virtual ICollection<ConferenceDay> ConferenceDays { get; set; }
    }
}
