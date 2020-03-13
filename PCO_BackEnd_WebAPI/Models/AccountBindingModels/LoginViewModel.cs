using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.AccountBindingModels
{
    public class LoginViewModel
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}