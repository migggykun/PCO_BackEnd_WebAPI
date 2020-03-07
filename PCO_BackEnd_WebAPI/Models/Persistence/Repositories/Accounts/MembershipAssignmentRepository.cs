using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts
{
    public class MembershipAssignmentRepository : Repository<MembershipAssignment> , IMembershipAssignmentRepository
    {
        public MembershipAssignmentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}