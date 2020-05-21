using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.AccountBindingModels
{
    public class SMSBindingModel
    {
        [Required]
        public List<int> UserIds{ get; set; }

        [Required]
        public string Message { get; set; }
    }
}