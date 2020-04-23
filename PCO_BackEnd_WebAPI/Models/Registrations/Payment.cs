using PCO_BackEnd_WebAPI.Models.Images;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public class Payment
    {
        [Key]
        [Column("id")]
        public int RegistrationId { get; set; }

        [Required]
        [StringLength(128)]
        public string TransactionNumber { get; set; }

        public DateTime PaymentSubmissionDate { get; set; }

        public Receipt Receipt { get; set; }

        public int AmountPaid { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public virtual Registration Registration { get; set; }

        public string Remarks { get; set; }
    }
}
