﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestMembershipTypeDTO
    {
        [Required]
        [StringLength(128, ErrorMessage = "{0} length must be between 1 and {1}!")]
        public string Name { get; set; }

        [Required]
        [StringLength(512, ErrorMessage = "{0} length must be between 1 and {1}!")]
        public string Description { get; set; }
    }
}