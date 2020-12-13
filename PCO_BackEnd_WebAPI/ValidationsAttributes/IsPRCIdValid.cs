using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PCO_BackEnd_WebAPI.DTOs.Accounts;

namespace PCO_BackEnd_WebAPI.ValidationsAttributes
{
    public class IsPRCIdValid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var prcDetail = validationContext.ObjectInstance as RequestPRCDetailDTO;
            if(string.IsNullOrEmpty(prcDetail.IdNumber))
            {
                return ValidationResult.Success;
            }
            else
            {
                if(prcDetail.IdNumber.Count() == 7)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("PRC Registration No should be 7 digits");
                }
            }
        }
    }
}