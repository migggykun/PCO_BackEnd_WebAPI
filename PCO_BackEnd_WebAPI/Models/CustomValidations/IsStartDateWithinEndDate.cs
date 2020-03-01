using PCO_BackEnd_WebAPI.Models.Conferences;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.CustomValidations
{
    public class IsStartDateWithinEndDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var conference = validationContext.ObjectInstance as Conference;

            if (conference.start_date <= conference.end_date)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date specified invalid.");
            }
        }
    }
}