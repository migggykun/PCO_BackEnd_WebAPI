using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.AccountBindingModels
{
    public class CustomEmailBindingModel
    {
        [Required(ErrorMessage = "Please choose Email receiver!")]
        public List<int> UserIds{ get; set; }

        [Required]
        [Display(Name = "Header")]
        [StringLength(160, ErrorMessage = "{0}'s maximum length is {1}.")]
        public string Header { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string Body { get; set; }
    }
}