﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.AccountViewModels
{
    public class ConfirmEmailViewModel
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }
}