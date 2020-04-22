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
            if (value.ToString() == "")
            {
                return ValidationResult.Success;
            }

            DateTime minDate =  Convert.ToDateTime("2000-01-01T00:00:00");
            DateTime maxDate = Convert.ToDateTime("3000-01-01T00:00:00");
            DateTime date;
            DateTime.TryParse(value.ToString(), out date);
            DateTime result;

            if (date == DateTime.MinValue)
            {
                return new ValidationResult("Invalid Date Format.");
            }
            else
            {
                if(DateTime.TryParse(date.ToString(), new CultureInfo("fil-PH"), DateTimeStyles.None, out result))
                {
                    if (date >= minDate && date <= maxDate)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Invalid Date. Must be within " + minDate + " and " + maxDate);
                    }
                }
                else
                {
                    return new ValidationResult("Invalid Date Format.");
                }
            }

        }
    }
}