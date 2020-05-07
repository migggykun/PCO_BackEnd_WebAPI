using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.ValidationsAttributes
{
    public class IsValidPhoneNumber : ValidationAttribute
    {
        private bool isRequired;
        private int maxLength;
        private int minLength;

        public IsValidPhoneNumber(bool IsRequired,int MaxLength,int MinLength = 0)
        {
            isRequired = IsRequired;
            maxLength = MaxLength;
            minLength = MinLength;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string phoneNumber = (string)value;

            if(phoneNumber.Length == 0 && isRequired)
            {
                return new ValidationResult("Phone number is required!");
            }

            if(phoneNumber.Length > maxLength)
            {
                return new ValidationResult("Phone number must " + maxLength + " digits!");
            }

            if (phoneNumber.Length < minLength)
            {
                return new ValidationResult("Phone number must " + maxLength + " digits!");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, "^[0-9]*$"))
            {
                return new ValidationResult("Phone number must not contains character");
            }

            return ValidationResult.Success;
        }
    }
}