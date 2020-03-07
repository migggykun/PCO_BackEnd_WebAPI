using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Accounts;

namespace PCO_BackEnd_WebAPI.Models.AccountBindingModels
{
    public class UserAccountBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage="Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^\+639?(\d[0-9]{9})", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }

        
        public PRCDetail PrcDetail { get; set; }

        public UserInfo UserInfo { get; set; }

        public MembershipAssignment MembershipAssignment { get; set; }

        public bool isAdmin { get; set; }


    }
}