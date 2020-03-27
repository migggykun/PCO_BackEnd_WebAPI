﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class UpdatePaymentDTO
    {
        [Required]
        public int AmountPaid { get; set; }

        public string ProofOfPayment { get; set; }

        public string Remarks { get; set; }
    }
}