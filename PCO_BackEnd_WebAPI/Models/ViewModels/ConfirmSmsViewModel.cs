using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.ViewModels
{
    public class ConfirmSmsViewModel
    {
        public string PhoneNumber { get; set; }

        public string Token { get; set; }
        public string Email { get; set; }
    }
}