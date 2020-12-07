using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.AccountBindingModels
{
    public class SMSBindingModel
    {
        [Required(ErrorMessage = "Please choose Sms receiver!")]
        public List<int> UserIds{ get; set; }

        
        [Required]
        [Display(Name = "Message")]
        [StringLength(512, ErrorMessage = "{0}'s maximum length is {1}.")]
        public string Message { get; set; }
    }
}