using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.ValidationsAttributes
{
    public class IsDateValid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string date = (string)value;
            DateTime result;
            if (string.IsNullOrEmpty(date))
            {
                return ValidationResult.Success;
            }
            else
            {
                if(DateTime.TryParse(date, new CultureInfo("fil-PH"), DateTimeStyles.None, out result))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Invalid Date Format");
                }
            }

        }
    }
}