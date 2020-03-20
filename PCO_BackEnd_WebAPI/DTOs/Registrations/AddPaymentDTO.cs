using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class AddPaymentDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AmountPaid { get; set; }

        [Required]
        public string ProofOfPayment { get; set; }

    }
}