﻿using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts
{
    public class MembershipTypeRepository : Repository<MembershipType>, IMembershipTypeRepository
    {
        public MembershipTypeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public MembershipType GetMembershipTypeByName(string membershipTypeName)
        {
            var membershipType = appDbContext.MembershipTypes
                                             .FirstOrDefault(e => string.Compare(e.membershipName, membershipTypeName, true) == 0);
            return membershipType;
        }

        public ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}