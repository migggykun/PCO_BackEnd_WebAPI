using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences;

namespace PCO_BackEnd_WebAPI.Models.Conferences.Promos
{
    [Table("Promos")]
    public partial class Promo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Promo()
        {
            PromoMembers = new HashSet<PromoMember>();
        }

        public int promoId { get; set; }

        [Required]
        [StringLength(20)]
        public string promoName { get; set; }

        [Required]
        [StringLength(100)]
        public string promoDescription { get; set; }

        public DateTime validity_StartDate { get; set; }

        public DateTime validity_EndDate { get; set; }

        public virtual Conference Conference { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromoMember> PromoMembers { get; set; }
    }
}
