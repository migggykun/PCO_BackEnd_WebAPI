using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class RegistrationFeeDTO
    {
        public double Price { get; set; }

        public double Discount { get; set; }

        public bool IsPromoApplied { get; set; }
    }
}