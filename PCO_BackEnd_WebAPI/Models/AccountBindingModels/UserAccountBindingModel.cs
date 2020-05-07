using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.ValidationsAttributes;

namespace PCO_BackEnd_WebAPI.Models.AccountBindingModels
{
    public class UserAccountBindingModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [IsValidPhoneNumber(true,11,11)]
        public string PhoneNumber { get; set; }

        
        public RequestPRCDetailDTO PrcDetail { get; set; }

        public RequestUserInfoDTO UserInfo { get; set; }

        public bool IsAdmin { get; set; }


    }
}