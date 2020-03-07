using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public partial class RegistrationPayment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int registrationPaymentId { get; set; }

        [Required]
        [StringLength(128)]
        public string transactionNumber { get; set; }

        public DateTime paymentSubmissionDate { get; set; }

        public int amountPaid { get; set; }

        [Required]
        [MaxLength(255)]
        public byte[] proofOfPayment { get; set; }

        public bool? isPaymentConfirmed { get; set; }

        public virtual Registration Registration { get; set; }
    }
}
