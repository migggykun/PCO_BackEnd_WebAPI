using PCO_BackEnd_WebAPI.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using PCO_BackEnd_WebAPI.Models.Pagination;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts
{
    public interface IMembershipTypeRepository : IRepository<MembershipType>
    {
        MembershipType GetMembershipTypeByName(string membershipTypeName);
        MembershipType UpdateMembershipType(int id, MembershipType entityToUpdate);
        PageResult<MembershipType> GetPagedMembershipTypes(int page, int size, string filter);
    }
}