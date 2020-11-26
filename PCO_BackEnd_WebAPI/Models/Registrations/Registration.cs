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
            ActivitiesToAttend = new HashSet<ActivitiesToAttend>();
            //Conference = new Conference();
            //User = new ApplicationUser();
            //Promo = new Promo();
            //RegistrationPayment = new Payment();
        }

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConferenceId { get; set; }

        public bool IsBundle { get; set; }

        public int? PromoId { get; set; }

        public double? Amount { get; set; }

        public double? Discount { get; set; }

        public int RegistrationStatusId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Promo Promo { get; set; }

        public virtual Conference Conference { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivitiesToAttend> ActivitiesToAttend { get; set; }
    }
}
