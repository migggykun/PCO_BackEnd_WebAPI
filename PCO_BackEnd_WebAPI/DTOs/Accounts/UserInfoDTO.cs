﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class UserInfoDTO
    {
        public int Id { get; set; }

        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string address { get; set; }

        public string organization { get; set; }
    }
}