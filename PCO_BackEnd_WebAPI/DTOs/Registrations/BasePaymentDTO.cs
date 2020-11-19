using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class BasePaymentDTO
    {
        [Required]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]+)?$", ErrorMessage = "Invalid amount!")]
        public double AmountPaid { get; set; }

        public string ProofOfPayment { get; set; }

        /*[StringLength(512, ErrorMessage = "{0} length must be less than {1}.")] TODO: custom validation*/
        public string Remarks { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed!")]
        public int refId { get; set; }

        public string paymentType { get; set; }
    }
}