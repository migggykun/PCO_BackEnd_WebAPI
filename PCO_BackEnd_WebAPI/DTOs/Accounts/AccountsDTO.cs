﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Accounts
{
    public class AccountsDTO
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public PRCDetailDTO PrcDetail { get; set; }

        public UserInfoDTO UserInfo { get; set; }

        public MembershipAssignmentDTO MembershipAssignment { get; set; }
    }
}