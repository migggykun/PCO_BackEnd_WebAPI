using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string TransactionNumber { get; set; }

        public DateTime PaymentSubmissionDate { get; set; }

        [Required]
        public string ProofOfPayment { get; set; }

        public int AmountPaid { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public virtual Registration Registration { get; set; }
    }
}
