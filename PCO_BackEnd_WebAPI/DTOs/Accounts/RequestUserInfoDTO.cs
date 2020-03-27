﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class RequestUserInfoDTO
    {

        [Required]
        public string FirstName { get; set; }


        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Organization { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }
    }
}